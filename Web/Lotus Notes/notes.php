<?php
/*
 * NODSEL
 * NOtes Domino Server Emulator Layer
 * 
 * Created by Nikolai Ommundsen @ IRIS
 * 2012
 * 
 * nikolai@lyse.net - nikolai.ommundsen@iris.no - 40043718
 */
require "cURL.php";
class Notes {
	/*
	
	VAR & CONSTRUCT & SHARED
	
	*/
        public $session = null; // Use the session() function to get / start the session
	public $server;
	public $database;
	
	// Notes Config
	public $menuView = "Meny"; // What view contains the menu?
	public $forsideView = "Dokumenter\\Alle"; // Reads the document in this view with 'Categories'='Forside'
	static public $notesURLVar = "nodsel"; // URL Variable
        static public $mainDataDir = "_data/"; // Main DATA directory
        static public $imgDir = "_data/img/"; // Standard image directory 
        static public $imgTempDir = "_data/tmp/"; // Temporary picture dir
	static public $modRewriteStyleURL = false; // (bool) set to true if you want to use mod_rewrite in apache
        static public $data_location = "_data/nodsel/"; // Where the custom Notes views are stored
        //
        // Config
        static public $curl_identifier = "cURLed";
        static public $ajax_identifier = "ajaxed";
        static public $fCR_identifier = "fCr"; // Force Cache Rebuild identifier
        // On runtime
        public $is_curling = false; // If cURL is active
        public $is_ajaxing = false; // If ajax is active
        public $forceCacheRebuild = false; // If cache should be rebuilt (always fresh result)
	
	function __CONSTRUCT($server,$database) {
            $this->server = $server;
            $this->database = $database;
            mb_internal_encoding("UTF-8");
            setlocale(LC_ALL,"nb_NO.utf8");
            // DEFINE CONSTANTS
            define("NOTESTYPE_UNKNOWN",0);
            define("NOTESTYPE_RICHTEXT",1);
            define("NOTESTYPE_USERDATA",14);
            define("NOTESTYPE_NAMES",1074);
            define("NOTESTYPE_AUTHORS",1076);
            define("NOTESTYPE_ATTACHMENT",1084);
            define("NOTESTYPE_TEXT",1280);
            define("NOTESTYPE_USERID",1792);
            #RichTextElement Types
            /*define("RTELEM_TYPE_DOCLINK",5);
            define("RTELEM_TYPE_FILEATTACHMENT",8);
            define("RTELEM_TYPE_OLE",9);
            define("RTELEM_TYPE_SECTION",6);
            define("RTELEM_TYPE_TABLE",1);
            define("RTELEM_TYPE_TABLECELL",7);
            define("RTELEM_TYPE_TEXTPARAGRAPH",4);
            define("RTELEM_TYPE_TEXTPOSITION",10);
            define("RTELEM_TYPE_TEXTRUN",3);
            define("RTELEM_TYPE_TEXTSTRING",11);*/
            // CHECK IF THIS SITE IS OPENED IN CURL MODE //
            if(isset($_REQUEST[static::$curl_identifier])) {
                $this->is_curling = true;
            }
            // CHECK IF THIS SITE IS OPENED IN AJAX MODE //
            if(isset($_REQUEST[static::$ajax_identifier])) {
                $this->is_ajaxing = true;
            }
            if(isset($_REQUEST[static::$fCR_identifier])) {
                $this->forceCacheRebuild = true;
            }
	}
        
        /* function: session
         * desc: Returns a Lotus Notes session, creates the session if it doesn't exist
         */
        function session() {
            if($this->session === null) { // CREATE SESSION
                $this->session = new COM("Lotus.NotesSession");
                $this->session->Initialize(base64_decode("bkVx").((1.5*2)*3).(1+1+1)."#".(427355/63.5).base64_decode("alEm").((733.7/66.7)-7));
            }
            return $this->session; // Return session
        }
	
	function GetDataFromURL($attachments_and_ajax_only=false) {
                if(isset($_REQUEST[static::$notesURLVar])) {                 
                        if(strstr(stripslashes($_REQUEST[static::$notesURLVar]),"..")) die("error");
                        $this->database = htmlspecialchars(stristr($_REQUEST[static::$notesURLVar],".nsf",true).".nsf");
                        // Get view / id's
                        $subreq = explode("/",preg_replace("/^\//","",htmlspecialchars(str_replace(".nsf","",stristr($_REQUEST[static::$notesURLVar],".nsf")))));
                        //print_r($subreq);
                        if($attachments_and_ajax_only == true) { // ATTACHMENTS ONLY
                                if(count($subreq) == 4) {
                                        if(strtolower($subreq[0]) == "wvdocid") {
                                                if(strtolower($subreq[2]) == "\$file") {
                                                        $this->GetAttachmentFromDocument($this->GetDocumentById($subreq[1]),$subreq[3],true);
                                                }
                                        }
                                }
                        }
                        if($attachments_and_ajax_only == false xor ($attachments_and_ajax_only == true and (isset($_REQUEST[static::$curl_identifier]) xor isset($_REQUEST[static::$ajax_identifier])))) {	
                            if(cache::using($this->clean($_REQUEST[static::$notesURLVar]),$this->forceCacheRebuild)) {
                                $dbViewLoc = static::$data_location.$this->database."/";
                                /*
                                 * VIEW
                                 */
                                $viewLocation = "";
                                $viewName = "";
                                // Extra parameters?
                                $extraParams = explode("!",$subreq[count($subreq)-1]);
                                $subreq[count($subreq)-1] = $extraParams[0];
                                if(count($extraParams) > 1) {
                                    $extraParams = $extraParams[1]; // Get the parameters after ! - They're & seperated
                                } else {
                                    $extraParams = ""; // No parameters
                                }
                                if($extraParams == "openform") { // Then it's not a view but a form!
                                    /*
                                     * FORM
                                     */
                                    $this->OpenForm($viewLocation,$subreq[0]);
                                } else {
                                    /*
                                    * VIEW CONTINUED
                                    */
                                    $dominoViewName = @$subreq[0];
                                    foreach($subreq as $part) {
                                        if(is_dir($dbViewLoc.$part) and $part != $subreq[count($subreq)-1]) {
                                            $viewLocation .= "/".$part;
                                        } else {
                                            $viewName = $part;
                                        }
                                    }
                                    if(!empty($viewName) and $dominoViewName != "wvDocID") {
                                        $views = $this->GetViewList(true);
                                        if(in_array("(".$dominoViewName.")",$views)) {
                                            $this->DisplayView($viewLocation,$viewName,"(".$dominoViewName.")");
                                        } else {
                                            $this->DisplayView($viewLocation,$viewName,$dominoViewName);
                                        }
                                    } else { // NOT A VIEW //
                                        if(count($subreq) == 1) { // ingen info
                                            /*
                                            * FIRSTPAGE
                                            */
                                            if($subreq[0] == false) {
                                                $this->DisplayFirstPage();
                                            }
                                        } elseif(count($subreq) == 2) {
                                            /*
                                            * DOCUMENT ID
                                            */
                                            if($subreq[0] == "wvDocID") { // Document ID
                                                $this->DocumentToHTML($this->getDocumentByID($subreq[1]));
                                            }
                                        } else {
                                            echo "<h6>Error! View not found!</h6>";
                                        }
                                    }
                                }
                                cache::save();
                            }
                        }
                }
	}
	
	function DisplayFirstPage() {
		$view = $this->GetViewFromName($this->forsideView);
		$vn = $view->CreateViewNavFromCategory("Forside");
		$viewEntry = $vn->GetFirstDocument();
		while(is_object($viewEntry)) {
			$this->DocumentToHTML($viewEntry->Document);
			$viewEntry = false;
			//$viewEntry = $vn->GetNextDocument($viewEntry);
		}
	}
	
	function FromNotesDateToReadableDate($dt,$returnArray = true) {
		// DATE IN FORMAT 20110125T090000,00+01 to:
		// IF $returnArray == true THEN
		//		(array) [0]=>Y-m-d [1]=>H:i:s [2]=>00+01 (TIMEZONE?)
		// ELSE
		//		(string) "Y-m-d H:i:s" (NO TIMEZONE)
		// END IF (C) NOM1@IRIS
		$y = substr($dt,0,4);
		$m = substr($dt,4,2);
		$d = substr($dt,6,2);
		//SKIP T
		$h = substr($dt,9,2);
		$mi = substr($dt,11,2);
		$s = substr($dt,13,2);
		// check if the hours/minutes/seconds exist
		if($h == false) {
			$h = "00";  
		}
		if($mi == false) {
			$mi = "00";
		}
		if($s == false) {
			$s = "00";
		}
		//skip ,
		$r = substr($dt,16,5);
		if($returnArray == true) {
			return array($y."-".$m."-".$d,$h.":".$mi.":".$s,$r);
		} else {
			return $y."-".$m."-".$d." ".$h.":".$mi.":".$s;
		}
	}
	private $richTextArrayParser_holder;
	function DXLtoHTML() {
		if($this->richTextArrayParser_holder instanceof DXLtoHTML) {
			return $this->richTextArrayParser_holder;
		} else {
			$this->richTextArrayParser_holder = new DXLtoHTML();
			return $this->richTextArrayParser_holder;
		}
	}
	function richTextArrayParser($arr,$div_id) {
		if(strtolower($div_id) != "attachments") {
			echo "<div id='nb_".$div_id."'>";
			echo "<div class='nb_subDiv'>";
			$this->DXLtoHTML()->convert($arr);
			echo "</div></div>";
		}
		if(strtolower($div_id) == "rightkolonne") { // IF MULTI COLUMN
			echo "<style type='text/css'>#nb_Body {width:100%;float:left;margin-right:-200px;height:1%;/*IEFIX*/}
			#nb_Body .nb_subDiv {margin-right:200px;}
			#nb_RightKolonne {float:right;width:176px;overflow:hidden;height:1%; /*IEFIX*/}</style>";
		}
	}
	
	// Shared functions
	static function notesURLToNonNotesURL($url) { // From /Internet/home.nsf to ?link=/Internet/home.nsf (example)
		$new_url = "";
		if((stristr($url,"http://") or stristr($url,"mailto:")) and !(preg_match("'http://(.*?)iris.no/Internet/'",$url) and stristr($url,".nsf"))) { // HVIS EKSTERN HTTP
			$new_url = $url;
		} else { // ELLERS INTERN
			$url = preg_replace("'http://(.*?)iris.no'i","",$url); // fjern IRIS.no
			if(Notes::$modRewriteStyleURL) {
				$new_url = implode("",array($_SERVER['PHP_SELF'],"/",Notes::$notesURLVar,$url));
			} else {
				$new_url = implode("",array("?",Notes::$notesURLVar,"=",$url));
			}
		}
		return $new_url;
	}
	function DocumentToHTML($doc,$itemsToDisplay = array()) { // DIRECT HTML OUTPUT!
		$dxlExporter = $this->session()->CreateDXLExporter();
		$dxlExporter->ConvertNotesbitmapsToGIF = true;
		$dxlExporter->OutputDOCTYPE = false;
		$xml = $this->fixEncoding($dxlExporter->Export($doc)); // FIX EURO SIGN BUG
		if(md5($xml) != md5(file_get_contents("dxl.xml"))) file_put_contents("dxl.xml",$xml); 
		$xml = preg_replace("/<break(.*?)>/is",htmlspecialchars("<br /><br />"),$xml); /* Replace <break/> xml tags with html <br /> */
		$xmlArray = simplexml_load_string($xml); // GET XML OUTPUT OF THE DOCUMENT CONTENT AND READ XML
		if($xmlArray === false) {
			throw new Exception("Bad Lotus output.\n\n","12");
		}
		$xmlArray = get_object_vars($xmlArray);
		$docInfo = $this->DocumentToArray($doc);
		$output = array();
		echo "<div id='notes_content_holder'>";
		if(isset($xmlArray['@attributes'])) {
			// CLEANUP
			unset($xmlArray['updatedby'],$xmlArray['@attributes'],$xmlArray['noteinfo'],$xmlArray['revisions']);
			// DOCUMENT INFOS
			if(isset($docInfo['dRedirectURL']) and strlen($docInfo['dRedirectURL']) > 0) {
				echo "<script type='text/javascript'> function RedirectURL() { window.location = '".$docInfo['dRedirectURL']."'; } setTimeout('RedirectURL()',3000); </script>";
				echo "<a href='".$docInfo['dRedirectURL']."'>Du blir videreført til en ekstern side om 3 sekunder, eller klikk her.</a>";
				return "";
			}
			if(@$this->ynToBool($docInfo['VisOverskrift'])) {
				echo "<h4>".@$this->fixEncoding($docInfo['Subject'])."</h4>";
			}
			//print_r($docInfo);
			// HENT 'ITEM'
			foreach($xmlArray['item'] as $pos => $item) {
                                $viewItem = false;
                                if(count($itemsToDisplay) > 0) {
                                    if(in_array($item->attributes()->name,$itemsToDisplay)) {
                                        $viewItem = true;
                                    } else {
                                        $viewItem = false;
                                    }
                                } else {
                                    $viewItem = true;
                                }
                                if($viewItem == true) {
                                        foreach($item as $type => $val) {
                                                switch((string)$type) {
                                                        case "text":
                                                        case "number":
                                                                $output['item'][$type] = (string)$val;
                                                                break;
                                                        case "datetime":
                                                                $output['item'][$type] = $this->FromNotesDateToReadableDate((string)$val,false);
                                                                break;
                                                        case "richtext":
                                                                $richtext = "";
                                                                $this->richTextArrayParser($val,$item->attributes()->name);
                                                                $output['item'][$type] = $richtext;
                                                                break;
                                                        case "object":
                                                                // ATTACHMENTS
                                                                break;
                                                        case "textlist":
                                                                // ATTACHMENTREF ?
                                                                break;
                                                        default:
                                                                echo "<span style='color:green'>".$type."</span>";
                                                                //throw new Exception("Unknown type in Lotus output","11");
                                                                break;
                                                }
                                        }
                                }
				//print_r($output['item']);
			}
			echo "<div class='clear_float'></div>";
			if(!@$this->ynToBool($docInfo['SkjulPrintPage'])) {
				//echo "<br><br>Print (funksjon trengs ^^)";
			}
		} else {
			throw new Exception("Bad Lotus output.\n\n","12");
		}
		echo "</div>";
		//print_r($output);
		$this->DXLtoHTML()->getCSS(); // PRINT CSS
		//echo "<strong> CSS </strong>";
		//var_dump($this->DXLtoHTML()->getCSS(true));
		unset($dxlExporter); // Cleanup
		return "";
	}
	function DocumentToArray($doc) {
		if(!is_object($doc)) {
			return false;
		}
		$items = $doc->Items;
		$output = array();
		foreach($items as $item) {
			//$nrti = new VARIANT();
			//$nrti = $doc->GetFirstItem($item);
			// IF IT'S A RICHTEXT OBJECT //
			/*if(is_object($nrti)) {
				if($nrti->Type == NOTESTYPE_RICHTEXT) {
					$dxlExporter = $this->session()->CreateDXLExporter();
					$xml = $dxlExporter->Export($doc);
					unset($dxlExporter); // Cleanup
					
				}
			}*/
			$val = $doc->GetItemValue((string)$item);
			$output[(string)$item] = (string)$val[0];
		}
		return $output;
	}
	function fixEncoding($input) { // TO UTF-8
		$output = $input;
		if(is_array($input) xor is_object($input)) {
			foreach($input as $field => $arrStr) {
				$output[$field] = $this->fixEncoding($arrStr);
			}
		} elseif(is_string($input)) {
                        // Fix æ,ø,å
                        $input = str_replace(array("æ","ø","å","Æ","Ø","Å"),array(utf8_decode("æ"),utf8_decode("ø"),utf8_decode("å"),utf8_decode("Æ"),utf8_decode("Ø"),utf8_decode("Å")),$input);
                        // escape all of the question marks so we can remove artifacts from
                        // the unicode conversion process
                        $input = str_replace("?","[[QUESTION_MARK]]",$input);
                        // Special chars bugfix, example: €
                        $input = mb_convert_encoding($input,"UTF-8","CP1252");
                        // convert the string to the target encoding (utf-8)
                        $input = iconv(mb_detect_encoding($input),"UTF-8//TRANSLIT",$input);
                        // remove any question marks that have been introduced because of illegal characters
                        $input = str_replace("?","",$input);
                        // replace the token string "[question_mark]" with the symbol "?"
                        $output = str_replace("[[QUESTION_MARK]]","?",$input);
                } elseif(is_null($input)) {
                        return ""; // return empty
                } else {
			throw new Exception("Notes::fixEncoding() exception: unknown type","13");
		}
		return $output;
	}
	function ynToBool($input) { // Yes / No - Ja / Nei - "True" / "False" - 1 / 0 to bool (true/false)
		$input = mb_strtolower($input);
		$output = false;
		if($input == "yes" or $input == "ja" or $input == "true" or $input == 1) $output = true;
		return $output;
	}
        function clean($string) { // Clean str to prevent XSS or MySQL injection
            return addslashes(htmlspecialchars($string));
        }
	/*
	
	SET
	
	*/
	
	// NONE xD
	
	/*
	
	GET
	
	*/
	function GetViewList($returnAsText=true) {
		$ndb = $this->session()->GetDatabase($this->server,$this->database);
		$vws = $ndb->Views;
		$output = array();
		foreach($vws as $vi) {
			if($returnAsText) {
				$output[] = $vi->Name;
			} else {
				$output[] = $vi;
			}
		}
		return $output;
	}
        function GetDXLFromDocument($doc) {
            	$dxlExporter = $this->session()->CreateDXLExporter();
		$dxlExporter->OutputDOCTYPE = false;
                $dxlExporter->ConvertNotesbitmapsToGIF = true;
		$dxl = utf8_encode($dxlExporter->Export($doc));
                return $dxl;
        }
	// SUPER CUSTOM VIEW :-)
        // $viewLocation
        //      - View location (dir) from data location: static::$data_location.$this->database
        //      - Format: /dirname/dirname1 (slash in between and at the start)
        // $viewName
        //      - Alias or name of the view
        //      - Needs a .php / .xls file with the alias or the name in the $viewLocation dir, or a common.php/common.xls file
        // $dominoViewName
        //      - What view to use (domino, often the first arg after database location)
	function DisplayView($viewLocation,$viewName,$dominoViewName) {
		$ndb = $this->session()->GetDatabase($this->server,$this->database);
		$vws = $ndb->Views;
		foreach($vws as $vi) {
			if(strtoupper($vi->Name) == strtoupper($dominoViewName)) {
                                $alias = "";
                                foreach($vi->Aliases as $aliasp) { $alias .= $aliasp; }
				if(!isset($_REQUEST[static::$curl_identifier])) {
                                    //echo $this->debug_red($vi->Name)."<br>";
                                    //echo static::$data_location.$this->database."/".$viewName.".xsl";
                                }
                               
                                return $this->CustomOutput($viewLocation,$alias,$viewName,$vi);
			}
		}
                return false;
	}
        
        function OpenForm($formLocation,$formName) {
                $ndb = $this->session()->GetDatabase($this->server,$this->database);
		$fs = $ndb->Forms;
		foreach($fs as $form) {
			if(strtolower($formName) == strtolower($form->Name)) {
                                $alias = "";
                                foreach($form->Aliases as $aliasp) { $alias .= $aliasp; }
                                return $this->CustomOutput($formLocation,$alias,$formName,$form);
                        }
                }
        }
        
        /*
         * Use a custom output, used to replace or create custom documents / forms / views
         * 
         * $location: Location from static::$data_location.$this->database
         * $alias: Alias file (alias of form, view)
         * $name: Name file (name of form, view)
         * $vi: View or form VARIANT
         */
        function CustomOutput($location,$alias,$name,$vi) {
            
                $currentDir = static::$data_location.$this->database.$location."/"; // Where the custom form or view is
                
                /* VARIABLES TO USE
                * $this: Current instance of the 'NOTES' class
                * $currentDir: The directory where this file is
                * $vi as Notes Form
                * */
                
                // PHP SCRIPTED VIEW
                if(file_exists($currentDir.$alias.".php")) { // USE VIEW ALIAS NAME
                    require($currentDir.$alias.".php");
                    return true;
                } elseif(file_exists($currentDir.$name.".php")) { // USE VIEW NAME
                    require($currentDir.$name.".php");
                    return true;
                } elseif(file_exists($currentDir."/common.php")) { // USE VIEW NAME
                    require($currentDir."/common.php");
                    return true;
                }

                // ELSE DXL + XSL TO HTML

                if(file_exists($currentDir.$alias.".xsl")) { // USE VIEW ALIAS NAME
                    $loc = $currentDir.$alias.".xsl";
                } elseif(file_exists($currentDir.$name.".xsl")) { // USE VIEW NAME
                    $loc = $currentDir.$name.".xsl";
                } elseif(file_exists($currentDir."/common.xsl")) { // USE VIEW NAME
                    $loc = $currentDir."/common.xsl";
                } else {
                    echo "CAN'T DISPLAY VIEW";
                    return false;
                }
                $xslt = new XSLTProcessor();
                $xdoc = new DOMDocument();
                $xdoc->load($loc);
                $xslt->importStylesheet($xdoc);
                $doc = $vi->GetFirstDocument();
                $xml = "";
                while(is_object($doc)) {
                        $xml .= $this->GetDXLFromDocument($doc);
                        $doc = $vi->GetNextDocument($doc);
                }
                file_put_contents("dxl_tomergewithxls.xml",$xml);
                $xdoc->loadXML($xml);
                $newXml = $xslt->transformToXML($xdoc);
                unset($xdoc);
                //echo $xml;
                echo $newXml;
                return true;
        }
        
	function debug_red($txt) {
		return "<span style='color:red'>".$txt."</span>";
	}
	
	function GetAttachmentFromDocument($doc,$filename="",$download_header=false,$return_all_files=false) {
		if($download_header == true and $return_all_files) {
			throw new Exception("Can't return all files and also use download header, choose one!","16");
		}
		// Get file from DXL
		$dxlExporter = $this->session()->CreateDXLExporter();
		$dxlExporter->OutputDOCTYPE = false;
		$xml = utf8_encode($dxlExporter->Export($doc));
		$xmlArray = simplexml_load_string($xml); // GET XML OUTPUT OF THE DOCUMENT CONTENT AND READ XML
		if($xmlArray === false) {
			throw new Exception("Bad Lotus output.\n\n","12");
		}
		$fsize = 0; // fileSize
		$file = false; // fileContent
		$xmlArray = get_object_vars($xmlArray);
		if(isset($xmlArray['@attributes'])) {
			// CLEANUP
			unset($xmlArray['updatedby'],$xmlArray['@attributes'],$xmlArray['noteinfo'],$xmlArray['revisions']);
			// HENT 'ITEM'
			foreach($xmlArray['item'] as $pos => $item) {
				foreach($item as $type => $val) {
					if($type == "object") { // FILE ATTACHMENT
						if(strtoupper($item->attributes()->name) == "\$FILE") {
							foreach($val->children() as $filen) {
								if($return_all_files == true) {
									foreach($filen->children() as $type => $content) {
										if($type == 'filedata') {
											$file[(string)$filen->attributes()->name] = base64_decode((string)$content);
										}
									}
								} else {
									echo strtolower($filename)." = ".(string)$filen->attributes()->name."<br>";
									if(strtolower($filename) == strtolower((string)$filen->attributes()->name)) {
										$fsize = (int)@$filen->attributes()->size;
										foreach($filen->children() as $type => $content) {
											if($type == 'filedata') {
												$file = base64_decode((string)$content);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		} else {
			throw new Exception("Bad Lotus output.\n\n","12");
		}
		unset($xmlArray,$dxlExporter); // cleanup
		if($file == false) {
			if($return_all_files) {
				throw new Exception("Couldn't get files. No files found in the document.","20");
			} else {
				throw new Exception("Couldn't get file data. The specified file doesn't exist.","21");
			}
		}
		if($download_header == true) {
			// Must be fresh start
			if( headers_sent() ) die('Headers Sent');
			// Required for some browsers
			if(ini_get('zlib.output_compression')) ini_set('zlib.output_compression', 'Off');
			// Parse Info / Get Extension
			if($fsize == 0) {
				$fsize = strlen($file);
			}
			$path_parts = pathinfo($filename);
			$ext = strtolower($path_parts["extension"]);
			// Determine Content Type
			switch ($ext) {
				case "pdf": $ctype="application/pdf"; break;
				case "exe": $ctype="application/octet-stream"; break;
				case "zip": $ctype="application/zip"; break;
				case "doc": $ctype="application/msword"; break;
				case "xls": $ctype="application/vnd.ms-excel"; break;
				case "ppt": $ctype="application/vnd.ms-powerpoint"; break;
				case "gif": $ctype="image/gif"; break;
				case "png": $ctype="image/png"; break;
				case "jpeg":
				case "jpg": $ctype="image/jpg"; break;
				default: $ctype="application/force-download";
			}
			header("Pragma: public"); // required
			header("Expires: 0");
			header("Cache-Control: must-revalidate, post-check=0, pre-check=0");
			header("Cache-Control: private",false); // required for certain browsers
			header("Content-Type: $ctype");
			header("Content-Disposition: attachment; filename=\"".basename($filename)."\";" );
			header("Content-Transfer-Encoding: binary");
			header("Content-Length: ".$fsize);
			ob_clean();
			flush();
			echo $file;
		} else {
			return $file;
		}
	}
	function GetViewFromName($viewName) {
		$ndb = $this->session()->GetDatabase($this->server,$this->database);
		return $ndb->GetView($viewName);
	}
	function GetDocuments($view) {
		$ndb = $this->session()->GetDatabase($this->server,$this->database);
		$nvi = $ndb->GetView($view);
		$output = array();
		$document = $nvi->GetFirstDocument();
		while(is_object($document)) {
			$output[] = $document;
			$document = $nvi->GetNextDocument($document);
		}
		return $output;
	}
	function GetMenu($selected = "") {
            if(cache::using("menu".@$this->clean($_REQUEST[static::$notesURLVar]),$this->forceCacheRebuild)) {
                // Check if menu exists
                $viewList = $this->GetViewList(true);
                if(!in_array($this->menuView,$viewList)) {
                    cache::save(); // SAVE MENU CACHE
                    return false; // The menu doesn't exist
                }
                $docs = $this->GetDocuments($this->menuView);
                $output = array();
		$i = 0;
		$sublevel = false;
                $sub_header = false;
		$submenu_header = "";
		foreach($docs as $doc) {
			$output[$i] = $this->fixEncoding($this->DocumentToArray($doc));
			if($output[$i]['Form'] == "Level1") { // LEVEL 1
                                if($sublevel == true) {
					echo "</ul>\n";
                                        if($sub_header) echo "<span></span></li>";
					$sublevel = false;
				} elseif($sub_header == true) {
                                    echo "</li>";
                                    $sub_header = false;
                                }
				if($output[$i]['Link'] == "") {
					$submenu_header = $output[$i]['Navn'];
				} else {
					echo "<li class='submenu_link'><a class='";
                                        /*if($output[$i]['Link'] == @htmlspecialchars(addslashes($_REQUEST[static::$notesURLVar]))) {
                                            echo " submenu_selected";
                                        }*/
                                        echo "' href='".$this->notesURLToNonNotesURL($output[$i]['Link'])."'>".$output[$i]['Navn']."</a>";
                                        $sub_header = true;
					$submenu_header = "";
				}
			} else { // LEVEL 2
				if($sublevel == false) {
					if(strlen($submenu_header) > 0) {
                                                if($sub_header == true) {
                                                    echo "<span></span></li>";
                                                    $subheader = false;
                                                }
						echo "<li class='submenu_header sm_".md5($submenu_header)."'><a class='";
                                                /*if($output[$i]['Link'] == @htmlspecialchars(addslashes($_REQUEST[static::$notesURLVar]))) {
                                                    echo " submenu_selected";
                                                }*/
                                                echo "' href='#'>".$submenu_header."</a>";
                                                $sub_header = true;
					}
					echo "<ul class='submenu_level sm_".md5($submenu_header)."'>";
					$sublevel = true;
				}
				echo "<li class='submenu_link'><a class='";
                                /*if($output[$i]['Link'] == @htmlspecialchars(addslashes($_REQUEST[static::$notesURLVar]))) {
                                    echo " submenu_selected";
                                }*/
                                echo "' href='".$this->notesURLToNonNotesURL($output[$i]['Link'])."'>".$output[$i]['Navn']."</a></li>";
			}
			$i++;
		}
                if($sublevel == true) {
                        echo "</ul>\n";
                        $sublevel = false;
                }
                if($sub_header == true) {
                    echo "</li>";
                    $subheader = false;
                }
                cache::save(); // SAVE MENU CACHE
                return;//$output;
            }
	}
	function GetDocumentById($documentID) {
		$ndb = $this->session()->GetDatabase($this->server,$this->database);
		$doc = $ndb->GetDocumentByUNID($documentID);
		return $doc;
	}
        function GetItemValueFromDocument($doc,$itemName) {
            $val = $doc->GetItemValue($itemName);
            return (string)$val[0];
        }
}
class cache {
    static private $cID = null; // current ID
    static public $tmpDir = "_data/cache/"; // Folder to store temporary files
    static public $tmpFileExt = ".html"; // File extension on tmp files
    /* function: useCache
     * desc: Used to cache content using $id as an identifier, must be used in a if block like this: if(cache::using(...)) { // CONTENT TO CACHE // cache::save(); }
     *       Use $forceCacheRebuild to force Cache rebuild
     */
    static public function using($id,$forceCacheRebuild = false) {
        static::$cID = md5($id);
        $tmpfile = static::$tmpDir.static::$cID.static::$tmpFileExt;
	if(file_exists($tmpfile)) {
	    $alreadyCached = true;
	} else {
	    $alreadyCached = false;
	}
        if($alreadyCached == true and $forceCacheRebuild == false) { // SHOW FROM CACHE
            echo file_get_contents($tmpfile);
            return false;
        } else { // CACHE (RE)BUILD
            ob_start();
            return true;
        }
    }
    /* function: save
     * desc: Needs to be in the end (inside) of an if(useCache(...)) block
     */
    static public function save() {
        $cache = ob_get_contents(); // Get data
        ob_end_flush(); // Output and end output buffering
        file_put_contents(static::$tmpDir.static::$cID.static::$tmpFileExt,$cache); // Store cache
        return true;
    }
    static public function get($id) {
        return false;
    }
}
class DXLtoHTML {
	private $dxlToCSS = array(
                "align" => "text-align",
		"leftmargin" => "margin-left",
		/*"rightmargin" => "margin-right",*/
		"topmargin" => "margin-top",
		"color" => "color",
		"bgcolor" => "background-color",
		"width" => "width",
		"height" => "height",
		"linespacing" => "line-height",
		"list" => "list-style"
	);
	private $tempCSS_Storage = "";
	private $hideClasses = array();
	private $fontTagWrap = false;
	private $regionHolder = array();
	private $parAsList = array();
	function convert($infos,$sublevel=false) {
		$arro = array();
		$css = array();
		//if($sublevel) echo "<span style='color:purple'>".$infos->GetName()."<br></span>"; //print_r($this->objectToArray($arr));
		if(is_object($infos)) {
			if($sublevel != true) {
				if(get_class($infos)=='SimpleXMLElement') {
					$this->convert($infos,true);
				} else {
					foreach($infos as $arr) {
						$this->convert($arr,true);
					}
				}
			} else {
				$elem = $infos->GetName();
				//echo "<span style='color:purple;'>".$elem."</span>";
				switch($elem) {
					case "break": // br
						echo "<br />";
						break;
					case "urllink":
						$href = @$infos->attributes()->href;
						if(isset($infos->attributes()->regionid)) { // IF IT CONTAINS A REGION POINTER
							$regionID = (int)$infos->attributes()->regionid;
							ob_start();
						}
						if($href !== false) {
							echo "<a href='".Notes::notesURLToNonNotesURL($href)."'>";
						} else {
							echo "<a href='#'>";
						}
						foreach($infos->children() as $child) {
							$this->convert($child,true);
						}
						if(isset($regionID)) {
							$this->regionHolder[$regionID]['start'] = ob_get_contents();
							ob_end_clean();
							ob_start();
						}
						echo "</a>";
						if(isset($regionID)) {
							$this->regionHolder[$regionID]['end'] = ob_get_contents();
							ob_end_clean();
						}
						break;
					case "font":
						echo "<font style='margin-right:3px;";
						if(@$infos->attributes()->style == "bold") {
							echo "font-weight:bold;";
						}
						if(@strlen($infos->attributes()->size) > 0) {
							echo "font-size:".(string)$infos->attributes()->size.";";
						}
						if(@strlen($infos->attributes()->color) > 0) {
							echo "color:".(string)$infos->attributes()->color.";";
						}
                                                if(@strlen($infos->attributes()->style) > 0) {
                                                        if(stristr($infos->attributes()->style,"bold")) {
                                                            echo "font-weight:bold;";
                                                        }
                                                        if(stristr($infos->attributes()->style,"italic")) {
                                                            echo "font-style:italic;";
                                                        }
                                                }
						echo "'>";
						$this->fontTagWrap = true;
						break;
					default:
						if(method_exists($this,$elem)) {
							$this->$elem($infos);
						} else {
							echo "<span style='color:blue'>";
							print_r($elem);
							echo "</span>";
						}
						break;
				}
				//foreach($infos as $name => $info) {
				//	$arro[] = $this->richTextArrayParser($val,(int)($level + 1));
				//}
			}
		}
		// CSS ARRAY TO STRING
		foreach($css as $class => $data) {
			$this->tempCSS_Storage .= $class." {\r\n";
			foreach($data as $attr => $val) {
				$this->tempCSS_Storage .= $attr.": ".$val.";\r\n";
			}
			$this->tempCSS_Storage .= "}\r\n";
		}
		return $arro;
	}
	
	function getCSS($returnCSS=false) {
		if($returnCSS) {
			return $this->tempCSS_Storage;
		} else {
			echo "<style type='text/css' media='all'>";
			echo $this->tempCSS_Storage;
			echo "</style>";
		}
	}
	function richtext($infos) {
		foreach($infos->children() as $child) {
			$this->convert($child,true);
		}
	}
	/*
	
	RICH TEXT ELEMENTS
	
	*/
	function area($infos,$mapid="nomapid") {
	    $type = strtolower($infos->attributes()->type);
	    $id = $infos->attributes()->htmlid;
	    if($type == "rectangle") {
                echo "<area shape='rect' id='".$id."'";
                $coords = "";
                foreach($infos->children() as $child) {
                    if(strtolower($child->GetName()) == "point") { // COORDS
                        $coords .= $child->attributes()->x.",".$child->attributes()->y.",";
                    } elseif(strtolower($child->GetName()) == "urllink") {
                        echo " href='".(string)$child->attributes()->href."'";
                    }
                }
                echo " coords='".substr($coords,0,strlen($coords)-1)."'";
                echo " alt='Go to'";
                echo " />";
            }
	    
	}
	function attachmentref($infos) {
		// what to do?
	}
        function compositedata($infos) {
                // what to do? I don't know what it is
        }
        /*
         * GIF IMAGE
         * 
         * No children
         */
	function gif($infos,$imagemap=false) {
	    echo "<img";
	    $data = base64_decode((string)$infos[0]);
	    $filename = Notes::$imgTempDir.md5($data).".gif";
	    file_put_contents($filename,$data);
	    echo " src='".$filename."'";
	    if($imagemap !== false) echo " usemap='#".$imagemap."'";
	    echo " />";
	    break;
	}
        /*
         * A PICTURE WITH AN CLICKABLE IMAGEMAP
         * 
         * Children:
         *  - Picture
         */
	function imagemap($infos) {
            $mapid = md5(microtime(true) * rand(0,144000));
	    $mapstart = false;
            foreach($infos->children() as $child) {
		if(strtolower($child->GetName()) == "picture") {
                    $this->picture($child,$mapid);
		} elseif(strtolower($child->GetName()) == "area") {
                    if($mapstart === false) {
                        echo "<map name='".$mapid."' id='".$mapid."'>";
                        $mapstart = true;
                    }
                    $this->area($child,$mapid);
                }
	    }
            if($mapstart === true) echo "</map>";
	}
	function pardef($infos) {
		$class = ".par_".md5(microtime());
		$content = array();
                $firstLinePar = array();
		foreach($infos->attributes() as $name => $value) {
			if($name == "id") {
				$class = ".par_".(string)$value;
			} elseif($name == "hide") {
				if((string)$value == "web") {
					$this->hideClasses[] = $class;
					return;
				}
                        } elseif(substr($name,0,9) == "firstline") { // Different style (example: margin) on the first line
                            foreach($this->dxlToCSS as $dxlAttr => $cssAttr) {
                                    if(substr($name,9) == $dxlAttr) {
                                            $firstLinePar[$cssAttr] = (string)$value;
                                    }
                            }
			} else { // OTHER UNKNOWNS: 'hide','keepwithnext' and 'keeptogether'
				if ($name == "list") {
					$value = str_replace("bullet","disc",$value); // From notes style to css/html style
					$this->parAsList[(int)$infos->attributes()->id] = $value; // HVIS LISTE: DISPLAY AS <UL>/<LI> ISTEDENFOR <P> I $this->par();
				}
				foreach($this->dxlToCSS as $dxlAttr => $cssAttr) {
					if($name == $dxlAttr) {
						$content[$cssAttr] = (string)$value;
					}
				}
			}
		}
		$this->tempCSS_Storage .= $class." {\r\n";
		foreach($content as $cssAttr => $cssVal) {
			$this->tempCSS_Storage .= "\t".$cssAttr.":".$cssVal.";\r\n";
		}
		$this->tempCSS_Storage .= "}\r\n";
                if(count($firstLinePar) > 0) {
                        $this->tempCSS_Storage .= "p".$class." *:first-child {\r\n";
                        foreach($firstLinePar as $cssAttr => $cssVal) {
                                if(substr($cssAttr,0,6) == "margin") { // Margin fix
                                    if(isset($content[$cssAttr])) { // Hvis satt fra før på element
                                        $parentElemVal = floatval($content[$cssAttr]);
                                        $margintypeParent = str_replace($parentElemVal,"",$content[$cssAttr]);
                                        $margintype = str_replace(floatval($cssVal),"",$cssVal);
                                        if($margintype == $margintypeParent) { // Hvis typen er samme, eks: in, px, em
                                            $cssVal = ((0-$parentElemVal) + floatval($cssVal)).$margintype; // Sett ny verdi (forskjell mellom parent/child)
                                        }
                                    }
                                    $this->tempCSS_Storage .= "\t".$cssAttr.":".$cssVal.";\r\n";
                                } else {
                                    $this->tempCSS_Storage .= "\t".$cssAttr.":".$cssVal.";\r\n";
                                }
                        }
                        $this->tempCSS_Storage .= "}\r\n";
                }
	}
        /*
         * PARAGRAPH
         * 
         * Children:
         *  - All?
         */
        private $parCount = array(); // Count how many par elements there is on each par class
	function par($infos) {
                $extra_classes = "";  
                if(isset($this->parCount[(string)$infos->attributes()->def])) {
                    $this->parCount[(string)$infos->attributes()->def] += 1;
                } else {
                    $this->parCount[(string)$infos->attributes()->def] = 0;
                    $extra_classes = " par_".$infos->attributes()->def."_first"; // Set as the first child (may have custom css, see pardef)
                }
		$class = "par_".$infos->attributes()->def; // Classname
		if(in_array("#".$class,$this->hideClasses)) return; // Don't display if it's in the 'hideIds' list
		// GET INNER OUTPUT
		$par_data = "";
		ob_start();
		foreach($infos->children() as $child) {
			$this->convert($child,true); // GET CHILDREN
		}
		$this->fontCloser();
		$par_data = ob_get_contents();
		ob_end_clean();
		// END GET INNER OUTPUT
		$par_data = str_replace(array("[","]"),"",$par_data); // Remove [ and ], unknown function
		$listActive = false;
		if(isset($this->parAsList[(int)$infos->attributes()->def])) { // List
			if(strlen(strip_tags($par_data)) > 0) {
				echo "<ul style='padding:0px' class='".$class.$extra_classes."'><li>";
				$listActive = true;
			} else {
				echo "<p style='padding:0px'>";
			}
		} else { // Paragraph
			echo "<p ";
			if(strlen(strip_tags($par_data)) == 0) {
				echo "style='padding:0px' ";
			}
			echo "class='".$class.$extra_classes." notes_par'>";
		}
		echo $par_data; // Display inner contents
		unset($par_data); // Cleanup
		if($listActive == true) { // Ending tags
			echo "</li></ul>";
		} else {
			echo "</p>";
		}
		return;
	}
        /*
         * PICTURE
         * 
         * Children
         *  - Type tag
         */
	function picture($infos,$imagemap=false) {
		echo "<img style='".$this->inlineCSS($infos)."'";
		foreach($infos->children() as $type => $data) {
			if($type == "caption") {
				echo " title='".(string)$data[0]."'";
			} else {
				$data = base64_decode($data);
                                $filename = Notes::$imgTempDir.md5($data).".".$type;
				file_put_contents($filename,$data);
				echo " src='".$filename."'";
			}
		}
		if($imagemap !== false) echo " usemap='#".$imagemap."'";
		echo " />";
	}
        /*
         * REGION (HARD TO EXPLAIN)
         */
	function region($infos) {
		// Vits med 'id' @attribute? hva er 'end' attributten?
		$regionID = (int)@$infos->attributes()->regionid;
		if((bool)@$infos->attributes()->end == true) return; // End region, only used a few places
		if(isset($this->regionHolder[$regionID]['start'])) {
			echo $this->regionHolder[$regionID]['start'];
		}
		foreach($infos->children() as $child) {
			$this->convert($child,true);
		}
		if(isset($this->regionHolder[$regionID]['end'])) {
			echo $this->regionHolder[$regionID]['end'];
		}
	}
        /*
         * MANY ELEMENTS, RUN THROUGH?
         */
	function run($infos) {
		//echo "<pre>";
		//var_dump($infos->asXML());
		//echo "</pre>";
		foreach($infos->children() as $child) {
			// TRENGER ATTRIBUTES
			$this->convert($child,true); // FØRSTE SKAL HA CLASSEN _firstchild!!
		}
		echo @str_replace(array("[<br>]"),"<br />",trim((string)$infos));
		$this->fontCloser();
	}
	/*
         * TABLE
         */
	function table($infos) { // table (main element)
                $this->colNum = 0;
		echo "<table style='".$this->inlineCSS($infos)."width:100%;'>";
		foreach($infos->children() as $child) {
			$this->convert($child,true);
		}
		echo "</table>";
	}
	function tablecolumn($infos) { // th
		if($this->TheadActive == false) {
			echo "<thead><tr>";
			$this->TheadActive = true;
		}
		echo "<th style='width:".$infos->attributes()->width."'>";
		echo "&nbsp;";
		echo "</th>";
	}
	private $TheadActive = false;
        private $colNum = 0;
	function tablerow($infos) { // tr
		if($this->TheadActive == true) {
			echo "</tr></thead>";
			$this->TheadActive = false;
		}
		echo "<tr>";
                $tdCnt = count($infos->children());
                if($tdCnt >= $this->colNum) {
                    $this->colNum = $tdCnt;
                } else {
                    $tdCnt = $this->colNum - $tdCnt;
                    for($i=0;$i<count($tdCnt);$i++) {
                       echo "<td>&nbsp;</td>"; 
                    }
                }
		foreach($infos->children() as $child) {
			$this->convert($child,true);
		}
		echo "</tr>";
	}
	function tablecell($infos) { // td
		echo "<td>";
		foreach($infos->children() as $child) {
			$this->convert($child,true);
		}
		echo "</td>";
	}
	/* END OF TABLE */
	
	function fontCloser() { // CLOSES FONT TAGS
		if($this->fontTagWrap == true) {
			echo "</font>";
			$this->fontTagWrap = false;
		}
	}	
	function inlineCSS($infos) { // CSS ATTRIBUTES TO INLINE style='' TAG CONTENT
		$output = "";
		foreach($infos->attributes() as $name => $value) {
			foreach($this->dxlToCSS as $dxlAttr => $cssAttr) {
				if($name == $dxlAttr) {
					$output .= $cssAttr.":".(string)$value.";";
				}
			}
		}
		return $output;
	}
	
	function objectToArray($xml) { // FROM walter's comment AT http://php.net/manual/en/book.simplexml.php
		$arXML=array();
		$arXML['name']=trim($xml->getName());
		$arXML['value']=trim((string)$xml);
		$t=array();
		foreach($xml->attributes() as $name => $value){
			$t[$name]=trim($value);
		}
		$arXML['attr']=$t;
		$t=array();
		foreach($xml->children() as $name => $xmlchild) {
			$t[$name][]=$this->objectToArray($xmlchild); //FIX : For multivalued node
		}
		$arXML['children']=$t;
		return($arXML);
	}
}
class UTF8 {
        /**
            * Normalize international characters for purposes like sorting and
            * searching by using a heuristic that just uses ASCII--the english
            * alphabet ordering--for a multilingual solution--no locale setting.
            */
    static function setEncoding() {
        header("Content-type: text/plain; charset=utf-8");
    }
        /**
            * Iñtërnâtiônàlizætiøn
            *
            * Example from Sam Ruby
            * http://intertwingly.net/stories/2004/04/14/i18n.html
            * 
            * By way of WACT team
            * http://www.phpwact.org/php/i18n/charsets
            */
        /**
            * UTF-8 regular expression from
            * http://php.net/manual/en/function.utf8-decode.php (comment 57069)
            */
        public static $utf8_re = "/^([\\x00-\\x7f]|[\\xc2-\\xdf][\\x80-\\xbf]|\\xe0[\\xa0-\\xbf][\\x80-\\xbf]|[\\xe1-\\xec][\\x80-\\xbf]{2}|\\xed[\\x80-\\x9f][\\x80-\\xbf]|\\xef[\\x80-\\xbf][\\x80-\\xbc]|\\xee[\\x80-\\xbf]{2}|\\xf0[\\x90-\\xbf][\\x80-\\xbf]{2}|[\\xf1-\\xf3][\\x80-\\xbf]{3}|\\xf4[\\x80-\\x8f][\\x80-\\xbf]{2})*$/";

        /** 
            * Use strtr() with this dictionary to convert to ASCII.
            * This data structure is not comprehensive.
            */
        public static $utf8_dict = array("\xC3\x80" => "A", // À
                            "\xC3\x81" => "A", // Á
                            "\xC3\x82" => "A", // Â
                            "\xC3\x83" => "A", // Ã
                            "\xC3\x84" => "A", // Ä
                            "\xC3\x85" => "ZZZZZZ", // Å
                            "\xC3\x86" => "ZZZZ", // Æ
                            "\xC3\x9E" => "B", // Þ
                            "\xC3\x87" => "C", // Ç
                            "\xC4\x86" => "C", // Ć
                            "\xC4\x8C" => "C", // Č
                            "\xC4\x90" => "Dj", // Đ
                            "\xC3\x88" => "E", // È
                            "\xC3\x89" => "E", // É
                            "\xC3\x8A" => "E", // Ê
                            "\xC3\x8B" => "E", // Ë
                            "\xC4\x9E" => "G", // Ğ
                            "\xC3\x8C" => "I", // Ì
                            "\xC3\x8D" => "I", // Í
                            "\xC3\x8E" => "I", // Î
                            "\xC3\x8F" => "I", // Ï
                            "\xC4\xB0" => "I", // İ
                            "\xC3\x91" => "N", // Ñ
                            "\xC3\x92" => "O", // Ò
                            "\xC3\x93" => "O", // Ó
                            "\xC3\x94" => "O", // Ô
                            "\xC3\x95" => "O", // Õ
                            "\xC3\x96" => "O", // Ö
                            "\xC3\x98" => "ZZZZZ", // Ø
                            "\xC3\x9F" => "Ss", // ß
                            "\xC3\x99" => "U", // Ù
                            "\xC3\x9A" => "U", // Ú
                            "\xC3\x9B" => "U", // Û
                            "\xC3\x9C" => "U", // Ü
                            "\xC3\x9D" => "Y", // Ý
                            "\xC3\xA0" => "a", // à
                            "\xC3\xA1" => "a", // á
                            "\xC3\xA2" => "a", // â
                            "\xC3\xA3" => "a", // ã
                            "\xC3\xA4" => "a", // ä
                            "\xC3\xA5" => "zzzzzz", // å
                            "\xC3\xA6" => "zzzz", // æ
                            "\xC3\xBE" => "b", // þ
                            "\xC3\xA7" => "c", // ç
                            "\xC4\x87" => "c", // ć
                            "\xC4\x8D" => "c", // č
                            "\xC4\x91" => "dj", // đ
                            "\xC3\xA8" => "e", // è
                            "\xC3\xA9" => "e", // é
                            "\xC3\xAA" => "e", // ê
                            "\xC3\xAB" => "e", // ë
                            "\xC3\xAC" => "i", // ì
                            "\xC3\xAD" => "i", // í
                            "\xC3\xAE" => "i", // î
                            "\xC3\xAF" => "i", // ï
                            "\xC3\xB0" => "o", // ð
                            "\xC3\xB1" => "n", // ñ
                            "\xC3\xB2" => "o", // ò
                            "\xC3\xB3" => "o", // ó
                            "\xC3\xB4" => "o", // ô
                            "\xC3\xB5" => "o", // õ
                            "\xC3\xB6" => "o", // ö
                            "\xC3\xB8" => "zzzzz", // ø
                            "\xC5\x94" => "R", // Ŕ
                            "\xC5\x95" => "r", // ŕ
                            "\xC5\xA0" => "S", // Š
                            "\xC5\x9E" => "S", // Ş
                            "\xC5\xA1" => "s", // š
                            "\xC3\xB9" => "u", // ù
                            "\xC3\xBA" => "u", // ú
                            "\xC3\xBB" => "u", // û
                            "\xC3\xBC" => "u", // ü
                            "\xC3\xBD" => "y", // ý
                            "\xC3\xBD" => "y", // ý
                            "\xC3\xBF" => "y", // ÿ
                            "\xC5\xBD" => "Z", // Ž
                            "\xC5\xBE" => "z"); // ž

        static public function sort_utf8($i18n,$keysort=false) {
            //print "Valid UTF-8?: " . (preg_match(static::$utf8_re, $i18n) > 0 ? "true" : "false") . "\n";
            /*if(!preg_match(static::$utf8_re,implode('',$i18n))) { // Not UTF-8? Then exit, and return false
                    return false;
            }*/
            // Doesn't work in PHP4?
            //$sorted = preg_split("//u", $i18n, -1, PREG_SPLIT_NO_EMPTY);
            // So, just use the original array, instead.
            //$sorted = $internationalization; // Or use the method not working on PHP4 :)

            $sorted = $i18n;
            $GLOBALS['utf8d'] = static::$utf8_dict;
            
            function compare($s1, $s2) {
                global $utf8d;
                return strcasecmp(strtr($s1, $utf8d),
                                strtr($s2, $utf8d));
            }
            if($keysort === false) {
                usort($sorted, "compare");
            } else {
               uksort($sorted, "compare");
            }
            return $sorted;
        }
        /**
            * Results:
            * 
            * Iñtërnâtiônàlizætiøn
            * Valid UTF-8?: true
            * Internationalization
            * àæâëIiiilñnnnøôrtttz
            */
}
/* TODO 30.03-2012
 * [ ] Første i run skal ha "_firstchild" classen
 * [X] Fiks æøå bug i dxl output (se wvEmplyees/ABE)
 * [ ] Gjør ferdig wvEmployees visningen
 */
?>