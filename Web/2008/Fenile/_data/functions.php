<?php
/*
FUNCTIONS.PHP

Copyright (C) niikoo 2007
*/

#-----------------#
# OTHER FUNCTIONS #
#-----------------#

function msgBOX($msgBOXtext) {
	echo "<script> alert(\"$msgBOXtext\"); </script>";
}

function alert($msgBOXtext) {
	echo "<script> alert(\"$msgBOXtext\"); </script>";
}

function goBACK($tmtgb) { // use: goBACK( number ) , how many times to go back / forward ; - = back , else forward
	echo "<script> history.go($tmtgb); <"."/script>";
}

function redirectURL($url) { //function to Javascript Redirect Browser
	echo "
	<script>
	function browserRedirect()

	{

		var ns4 = document.layers;

		var ns6 = document.getElementById && !document.all;

  		var ie4 = document.all;

  		URLStr = \"$url\";

  		location = URLStr;
	
	}
	browserRedirect();
	</script>
	";
}

function dispact($file,$type) {
	global $actiondisp_edit;
	global $site;
	global $msword_doc;
	if ( $type == 'file' ) {
		echo "<a href='?download=1&side={$site}&file={$file}' alt='Download file' title='Download file'>D</a>&nbsp;";
		foreach( $actiondisp_edit as $editextension ) {
			if ( get_file_extension($file) == ".".$editextension ) {
				echo "<a href='?act=edit&side={$site}&file={$file}' alt='Edit' title='Edit'>E</a>&nbsp;";
			}
		}
		foreach( $actiondisp_edit as $sourceextension ) {
			if ( get_file_extension($file) == ".".$sourceextension ) {
				echo "<a href='?act=edit&sub=source&side={$site}&file={$file}' alt='Source' title='Source'>S</a>&nbsp;";
			}
		}
		if ( get_file_extension($file) == ".sql" ) {
			echo "<a href='?act=query&side={$site}&file={$file}' alt='SQL Query' title='SQL Query'>Q</a>&nbsp;";
		}
		/*if ( get_file_extension($file) == ".zip" ) {
			echo "<a href='?act=nzip&side={$site}&zipfile={$file}' alt='N-Zip' title='N-Zip'>Z</a>&nbsp;";
		}*/
	}
}

function get_themes() {
	global $fenile;
	$themes = map_dirs("./_data/theme/");
	$th = "";
	if ( @$fenile['theme'] == "default" ) {
		$sh = "<option value='default' >Default</option>";
		$sh .= "<option value='default' disabled='disabled'>--------------</option>";
		$i = "1";
	} elseif ( @$fenile['theme'] != "default" ) {
		$th = "<option value='default'>Default</option>".$th;
		$i = "1";
	}
	foreach ( $themes as $id => $file ) {
		if (strtolower(get_file_extension("_data/theme/".$file)) == ".css" and is_file("_data/theme/".$file)) {
			$file = str_replace(get_file_extension("_data/theme/".$file),"",$file);
			if ( $file != "default" ) {
				if ( $file == @$fenile['theme'] ) {
					$filen = str_replace("_"," ",$file);
					$sh = "<option value='{$file}' selected='selected'>{$filen}</option>";
					$sh .= "<option value='default' disabled='disabled'>--------------</option>";
				} else {
					$filen = str_replace("_"," ",$file);
					$th .= "<option value='{$file}'>{$filen}</option>";
				}
			}
		}
	}
	if ( $i != "1" ) {
		$sh = "<option value='default' selected='selected'>Default</option>".$th;
		$sh = "<option value='default' disabled='disabled'>--------------</option>".$th;
		$i = "1";
	}
	echo $sh.$th;
}

#----------------------#
# FILESYSTEM FUNCTIONS #
#----------------------#

function get_file_extension($file) {
	if ( strstr($file,".") == true ) {
		$fext = explode(".",$file);
		$output = "";
		foreach($fext as $part) {
			$output = ".".$part;
		}
		return $output;
	} else {
		return "";
	}
}
	
function size_hum_read($size){
/*
Returns a human readable size
*/
  $i=0;
  $iec = array("B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB");
  while (($size/1024)>1) {
  	$size=$size/1024;
  	$i++;
  }
  return substr($size,0,strpos($size,'.')+4)." ".$iec[$i];
}

function removeDir($path) { 
   // Add trailing slash to $path if one is not there 
   if (substr($path, -1, 1) != "/") { 
       $path .= "/"; 
   } 
   foreach (glob($path . "*") as $file) { 
       if (is_file($file) === TRUE) { 
           // Remove each file in this Directory 
           unlink($file); 
           echo "Removed File: " . $file . "<br>"; 
       } 
       else if (is_dir($file) === TRUE) { 
           // If this Directory contains a Subdirectory, run this Function on it 
           removeDir($file); 
       } 
   } 
   // Remove Directory once Files have been removed (If Exists) 
   if (is_dir($path) === TRUE) { 
       rmdir($path); 
       echo "<script> alert('Removed Directory: " . $path . " '); </script>"; 
   } 
}

function if_read_only($file) {
	$fileperms = substr(sprintf('%o',@fileperms($site.$file)), -4);
	if ( strstr($fileperms,"4") ) {
		return true;
	} else {
		return false;
	}
}

function dirsize($dir,$buf=2)
{
  static $buffer;
  if(isset($buffer[$dir]))
    return $buffer[$dir];
  if(is_file($dir))
    return filesize($dir);
  if($dh=@opendir($dir))
  {
    $size=0;
    while(($file=readdir($dh))!==false)
    {
      if($file=='.' || $file=='..')
        continue;
      $size+=dirsize($dir.'/'.$file,$buf-1);
    }
    closedir($dh);
    if($buf>0)
      $buffer[$dir]=$size;
    return $size;
  }
  return false;
}

if ( !function_exists('file_put_contents')) {
	function file_put_contents($filename,$text) {
		if ( !file_exists($filename) ) {
			$fp = @fopen($filename,"w");
			if ( $fp == false ) {
				return false;
			}
			if (is_array($data)) {
            	$data = implode('', $data);
        	} else {
        	    $data = (string) $data;
        	}
			fwrite($fp,"");
			fclose($fp);
			if ( file_exists($filename) ) {
				return true;
			} else {
				alert('Unknown Error!');
				return false;
			}
		} else {
			return false;
		}
	}
}
if ( !function_exists('file_get_contents')) {
	function file_get_contents($filename) {
		if ( !file_exists($filename) ) {
			return implode('', file($filename));
		}
	}
}
#-----------------#
# IMAGE FUNCTIONS #
#-----------------#

/*function get_image_size($img_path) {
	$img_size = getimagesize($img_path);
		$w = $img_size[0];
		$h = $img_size[1];
		
		while ( $h > 110 and $w > 110 ) {
			$h /= 2;
		}
		if ( $w > 110 or $h > 110 ) {
			$w = 110; $h = 110;
		}
		return array($w,$h);
}*/

#-----------------#
#      ERROR      #
#-----------------#

function error($num, $alt = "",$alt1 = "",$alt2 = "",$custom = false) {
	echo "<br />ERROR: ";
	if ( $custom == true ) {
		echo $alt;
	} else {
		switch(@$num) {
			case "1":   echo "Can't Connect To MySQL database"; break;
			case "2":   echo "Unknown Action"; break;
			case "3":   echo ""; break;
			case "4":   echo "Can't Select DB: ".$alt; break;
			case "5":   echo "Can't Select standard DB: fenile"; break;
			case "6":   echo "MySQL error: ".$alt; break;
			case "7":   echo "Can't Create Default DataBase: fenile"; break;
			case "8":   echo "Can't delete the user: ".$alt; break;
			case "9":   echo "Can't create the user: ".$alt; break;
			case "10":  echo "There was an error uploading the file, please try again."; break; 
			case "15":  echo "Missing Paramenter In Request"; break;
			case "16":  echo "Can't update theme, MySQL error"; break;
			case "18":  echo "You need to cut or copy files before use!"; break;
			case "20":  echo "Can't run copy / paste function"; break;
			case "21":  echo "Can't copy the file ".$alt; break;
			case "22":  echo "The username: ".$alt." doesn't exist"; break;
			case "23":  echo "The username: ".$alt." already exist"; break;
			case "29":  echo "The file: ".$alt." already exist"; break;
			case "30":  echo "The file: ".$alt." doesn't exist"; break;
			case "31":  echo "Too Short CHMOD String"; break;
			case "36":  echo "Can't rename {$alt} to {$alt1}, the file already exist"; break;
			case "37":  echo "Can't rename {$alt} to {$alt1}"; break;
			case "40":  echo "Error, Can't Open File For Writting"; break;
			case "41":  echo "Can't Write To File"; break;
			case "42":  echo "Save Error"; break;
			case "61":  echo "ZIP error!"; break;
			//--------
			default:    echo "Unknown Error"; break;
			
			# OLDER #
			//case "60":  echo "PHP_ZIP extension disabled in php.ini file. To use the N-Zip function, please enable the extension."; break;
		}
		echo " - Error no. ".$num;
	}
}
?>