<?php
/*
______         _ _       ______          _           _   
|  ___|       (_) |      | ___ \        (_)         | |  
| |_ ___ _ __  _| | ___  | |_/ / __ ___  _  ___  ___| |_ 
|  _/ _ \ '_ \| | |/ _ \ |  __/ '__/ _ \| |/ _ \/ __| __|
| ||  __/ | | | | |  __/ | |  | | | (_) | |  __/ (__| |_ 
\_| \___|_| |_|_|_|\___| \_|  |_|  \___/| |\___|\___|\__|
                                       _/ |              
                                      |__/               							  
                 _          _                    _ _ _                
                | |        | |                  (_|_) |               
 ____   ____  _ | | ____   | | _  _   _    ____  _ _| |  _ ___   ___  
|    \ / _  |/ || |/ _  )  | || \| | | |  |  _ \| | | | / ) _ \ / _ \ 
| | | ( ( | ( (_| ( (/ /   | |_) ) |_| |  | | | | | | |< ( |_| | |_| |
|_|_|_|\_||_|\____|\____)  |____/ \__  |  |_| |_|_|_|_| \_)___/ \___/ 
                                 (____/  
                        
Some Rights Reserved (cc) niikoo 2006-2008

- Thanks To: -
Codepress        ; http://www.codepress.org          ; Syntax colored editor
Hasin Hayder     ; http://www.hasinme.info           ; Zip File Creater Class
Valerio Proietti ; http://mootools.net/developers/   ; Mootools v1.2.0
Adobe            ; http://www.adobe.com              ; SPRY Tabbed panel
--------------
*/

// VERSION NUMBER //
$fenile['version']         = "0.65";
$fenile['licence_name']    = "Creative Commons";
$fenile['licence_url']     = "http://creativecommons.org/licenses/by-nc-sa/3.0/";

session_start();

include_once "_data/prerequest.php";    # Pre Requests

if ( @function_exists("date_default_timezone_set") ) {
	date_default_timezone_set("Europe/Paris"); //TIMEZONE : DEFAULT Europe/Paris : GMT +1
}

if (@$_REQUEST['side']) {
	$site = $_REQUEST['side'];
	$lastchar = strlen($site)-1;
	if ( @$site[$lastchar] != "/" and $site != "" ) {
		$site .= "/";
	}
} else {
	$site = "";
}

if ( @$_SESSION['fenile_lastview'] ) {
	$view = $_SESSION['fenile_lastview'];
} else {
	$view = 1;
}

// DEFINE $php_self_file = $_SERVER['PHP_SELF'] filename //
$php_self_file = $_SERVER['PHP_SELF'];
$php_self_file = explode("/",$php_self_file);
foreach ( $php_self_file as $file5 ) {
	$php_self_file = $file5;
}
// END DEFINE //

// INCLUDE PART 1 //
	if ( file_exists("_data/config.php") ) {
		include_once "_data/config.php";    # MySQL configuration file
	} else {
		$fenile['install'] = false;
	}
	include_once "_data/mysql.php";         # MySQL database reader
	include_once "_data/arrays.php";        # Arrays
	include_once "_data/functions.php";     # function library
	include_once "_data/security.php";      # security
	include_once "_data/open.php";          # directory loader
	include_once "_data/icons.php";         # icon finder function
	include_once "_data/generate.php";      # file view generator
	
//	include_once "_data/ajax.php";          # AJAX functions
	include_once "_data/zip.lib.php";       # ZIP Create Class
// END OF INCLUDE PART 1 //
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Fenile Project</title>
    <meta name="Author" content="niikoo"/>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<!-- Include Part 2 -->
<!--[if lt IE 7.]>
<script defer type="text/javascript" src="_data/pngfix.js"></script>
<![endif]-->
<link href="_data/theme/<?php echo ( @$fenile['theme'] != "" ? @$fenile['theme'] : "default"); ?>.css" rel="stylesheet" type="text/css" />
<script src="_data/SpryAssets/SpryTabbedPanels.js" type="text/javascript"></script>
<link href="_data/SpryAssets/SpryTabbedPanels.css" rel="stylesheet" type="text/css" />
<script src='_data/javascripts.js' language='javascript'></script>
<script src='_data/js/mootools.js' language='javascript'></script>
<!-- End of Include Part 2 -->
</head>
<?php if ( @$fenile['login_r'] != true ) {
echo '<img src="_data/img/head.png" alt="Fenile Project" width="800" height="100"/>'; //echo main header
}
echo "<!-- FENILE PROJECT, MADE BY NIIKOO -->";
	if ( @$fenile['install'] != true ) {
		if ( file_exists("_data/install.php") ) {
			@unlink("_data/config.php");
			include "_data/install.php";
		} else {
			die("The Installation File 'install.php' is Deleted!"); # Deleted 
		}
	}
		
/* INCLUDE PART 3 */
	include_once "_data/pwfield.php";   # password field
	include_once "_data/request.php";   # requests
/* END OF INCLUDE PART 3 */
?>
<body>
<div id="header">
<h2>Folder View:
  <select name="dir_disp" id="fldr_display" onchange="MM_jumpMenu('parent',this,0)">
  <?php // onChange="window.location=this.options[this.selectedIndex].value;"  : old ?>
    <option value="?dir_disp=1&side=<?php echo $site; ?>" <?php if (@$view == "1") { echo "selected='selected'"; } ?>>List</option>
    <option value="?dir_disp=2&side=<?php echo $site; ?>" <?php if (@$view == "2") { echo "selected='selected'"; } ?>>Icons</option>
    <option value="?dir_disp=3&side=<?php echo $site; ?>" <?php if (@$view == "3") { echo "selected='selected'"; } ?>>Thumbnails</option>
  </select>
    <br />
  <br />
    Current dir: 
    <?php
/*if (@$site) {
echo " root/$site";
} else {
echo " root";
}
*/
 if ( @$site ) {
	$upst = explode("/",strtolower($site));
	$ix = "";
	$lastchar1 = count($upst);
	unset($upst[$lastchar1]);
	unset($upst[$lastchar1-1]);
	$upst1 = "";
	echo "<a href='?side=' class='doli'>root</a>";
	foreach ( $upst as $partofupst ) {
		$upst1 .= $partofupst."/";
		echo "/<a href='?side=".@$upst1."' class='doli'>".$partofupst."</a>";
	}
	unset($upst1);
	unset($upst);
} else {
	echo "root";
}
?>
</h2>
</div>
<form name="isel" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>" enctype='multipart/form-data'>
<input type="hidden" name="side" value="<?php echo $site; ?>" />
<input type="hidden" name="act" />
<div id="editforms" class="clearfix">  
  <div id="tabs" class="TabbedPanels">
    <ul class="TabbedPanelsTabGroup">
      <li class="TabbedPanelsTab">None</li>
      <li class="TabbedPanelsTab">Global Tools</li>
      <li class="TabbedPanelsTab">File Tools</li>
      <li class="TabbedPanelsTab" tabindex="0">Zip  Functions</li>
      <li class="TabbedPanelsTab">Advanced Tools</li>
      <li class="TabbedPanelsTab">Settings</li>
      <li class="TabbedPanelsTab">User Control</li>
    </ul>
    <div class="TabbedPanelsContentGroup">
    <!--NONE-->
      <div class="TabbedPanelsContent"></div>
      <div class="TabbedPanelsContent">
        
        <div class="fprop"> <img src="_data/img/fprop/add.png" width="64px" height="64px" alt="Add"/><br />
          Filename:
          <input name="add_file" type="text" id="add_file" />
          <input name="add_type" type="radio" value="file" />
          File&nbsp;
          <input name="add_type" type="radio" value="dir" />
          Dir&nbsp;<br />
          <div style="padding:3px">
            <input name="add" type="button" id="add" value="        .: Add Dir / File :.        " onclick="fcp('add')" />
          </div>
        </div>
        
        
        <div class="fprop"> <img src="_data/img/fprop/upload.png" width="64px" height="64px" alt="Upload"/><br />
            <!--Number of Upload Forms: 
  			<input name="uploadforms" type="text" value="1" size="1" maxlenght="2" onchange="javascript:ajax('&uploadformnum=1&num=' + this.value)" />-->
            <!--<span id="uploadformajax"></span>-->
            <input name="upload_file" type="file" />
          <br />
            <input name="upload" type="button" value="       .: Upload :.       " onclick="fcp('upload')" />
        </div>
        
        
        <!--<div class="fprop"> <img src="_data/img/fprop/search.png" width="64px" height="64px" alt="Search"/><br />
          <strong>Search</strong><br />
           <input name="search" type="text" class="searchbox" size="22" onkeypress="if(this.value != '') ajaxsearch(this.value);"/>
          <br />
        </div>-->
        
        <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
        </div>
      </div>
      
      <div class="TabbedPanelsContent">
        <div class="fprop"> <img src="_data/img/fprop/delete.png" width="64px" height="64px" alt="del"/><br />
            <br />
            <input name="delete1" type="button" id="delete" value="     .: Delete Selected Folder/File(s) :.     " onclick="fconfirm()" />
        </div>
        <div class="fprop"> <img src="_data/img/fprop/rename.png" width="64px" height="64px" alt="rename" /><br />
            <br />
            <input name="rename" type="button" value="    .: Rename Folder(s) / File(s) :.    " onclick="fcp('rename')" />
        </div>
        <div class="fprop"> <img src="_data/img/fprop/copy.png" width="64px" height="64px" alt="Cut / Copy / Paste" /><br />
            <br />
            <input name="copy" type="button" value=" .: Copy :. " onclick="fcp('ccp')" />
            <input name="cut" type="button" value=" .: Cut :. " onclick="fcp('ccp')" />
            <input name="paste" type="button" value=" .: Paste :. " onclick="fcp('ccp')" />
        </div>
        <div class="fprop"> <img src="_data/img/fprop/readonly.png" width="64px" height="64px" alt="Read only"/><br />
          Read Only:
          <select name="chmodn">
        <option value="777" selected="selected">Off</option>
        <option value="444">On</option>
      </select>
      <br />
      <input name="chmods" type="button" id="chmods" value="    .: Change :.    " onclick="fcp('chmod')" />
        </div>

        <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
        </div>
      </div>
      <div class="hidden">
      
        <div class="fprop"> <img src="_data/img/fprop/newzip.png" width="64px" height="64px" alt="Read only"/><br />
          Zipname:
          <input type="text" name="new_zip" value="" />
          .zip <br />
          <input name="newzip_s" type="button" id="chmods" value="    .: Add files to Zip :.    " onclick="fcp('nzip')" />
        </div>

        <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
        </div> 
      </div>
      <div class="TabbedPanelsContent">
      
      
        <div class="fprop"> <img src="_data/img/fprop/chmod.png" width="64px" height="64px" alt="CHMOD"/><br />
          CHMOD to:
          <select name="chmodn">
            <option value="777">777</option>
            <option value="755">755</option>
            <option value="750">750</option>
            <option value="666">666</option>
            <option value="644">644</option>
            <option value="444">444</option>
          </select>
          <br />
         <input name="chmods" type="button" id="chmods" value="    .: CHMOD selected files :.    " onclick="cconfirm()" />
        </div>
        
        
        <div class="fprop"> <img src="_data/img/fprop/query.png" width="64px" height="64px" style="padding:3px" alt="SQL query sender" /><br />
          <br />
            <input name="query" type="button" id="query" value="    .: SQL Query Sender :.    " onclick="fcp('query')" />
            <br />
        </div>     
  
        
        <div class="fprop"> <img src="_data/img/fprop/bug.png" width="64" height="64" alt="Report bugs" /><br />       
         <br />
          <input name="bug" type="button" id="bug" value="    .: Report Bug :.    " onclick="window.open('mailto:bugs@niikoo.net')" />
        </div>
        
        <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
        </div>
       
      </div>
      
      <div class="TabbedPanelsContent">
       
      <div class="fprop"> <img src="_data/img/fprop/system.png" width="64px" height="64px" style="padding:3px" alt="System files"/><br />
      		Current setting: <?php if($fenile['sysfiles']==true){echo "Visible";}else{echo "Hidden";} ?><br />
            <input name="hssf" type="button" id="hssf" value="    .: Hide / Show System files :.    " onclick="fcp('hssf')" />
            <br />
        </div>
      
      
        <div class="fprop"> <img src="_data/img/fprop/theme.png" width="64" height="64" alt="Select Theme" /><br />
         Theme:
         <label>
      		 <select name="theme_select" id="theme_select">
        	 	<?php get_themes(); ?>
   	   	     </select>
      		<br />
      		<input name="theme" type="button" id="theme" value="    .: Select Theme :.    " onclick="fcp('theme')" />
      	</label>
       </div>
       
       <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
       </div>
     
     </div>
     
     <div class="TabbedPanelsContent">
        
        
        <div class="fpropa"> <img src="_data/img/fprop/user.png" width="64px" height="64px" alt="Add user" /><br />
          <strong>Add a new user:</strong>          <br />
          <br />
          <table width="200" border="0" class="std_font">
             <tr>
               <td>Username: </td>
               <td><input type="text" name="u_add_username" /></td>
             </tr>
             <tr>
               <td>Password: </td>
               <td><input type="password" name="u_add_password" /></td>
             </tr>
           </table>
           <input name="u_add" type="button" value="    .: Add User :.    " onclick="fcp('users')" />
        </div>
        
        
        <div class="fpropa"> 
          <img src="_data/img/fprop/delete_user.png" width="64px" height="64px" style="padding:3px" alt="Delete user" /><br />
              <strong>Delete User</strong>
              <br /><br />
            <select name="u_select_username" id="select">
            <?php
			list_users();
			?>
            </select>
            <br /><br />
            <input name="u_delete" type="button" value="    .: Delete User :.    " id="u_delete" onclick="u_dconfirm();this.type='submit'" />
        </div>
        <div class="fpropa"> 
          <img src="_data/img/fprop/users.png" width="64px" height="64px" style="padding:3px" alt="Log Out" /><br />
            <strong>Log Out</strong>
            <br /><br /><br />
            <input name="logout" type="button" value="         .: Log Out :.         " id="logout" onclick="fcp('logout')" />
        </div>
        
        
        <div style="clear:both" class="clearfix">
          <!-- END OF FLOAT -->
        </div>
      
      
      </div>
    
    </div>

</div>

<br /><br /><br />

<div id="outer_container" class="clearfix">
<div id="container" class="clearfix">

  <?php
/* File And Dir Counter Variables */
$tdirs = 0;
$tfiles = 0;
/* End of file and dir counter variable group */

if (@$file_names) {

	foreach( $file_names as $file1 ) { // DISPLAY FOLDERS
		$hidethisfile = false;
		if (is_dir($site.$file1)) {
			foreach ( $dosh as $dontshow ) {
				if ( strtolower($dontshow) == strtolower($file1) ) {
					$hidethisfile = true;
				}
			}
			$pos = strpos($site.$file1,"_data");
			if ( @$hidethisfile == false and $pos !== false ) {
				$hidethisfile = true;
			}
			if ( @$hidethisfile == false ) {
				$tdirs++;
				htmlspecialchars(stripslashes(generate($view,strtolower($file1),"dir",$site)));
			} elseif ( @$hidethisfile == true and $fenile['sysfiles'] == true ) {
				$tdirs++;
				htmlspecialchars(stripslashes(generate($view,strtolower($file1),"dir",$site,true)));
			}
		}
		$hidethisfile = false;
	}
	
	foreach( $file_names as $file1 ) { // DISPLAY FILES
		$hidethisfile = false;
		if (is_file(@$site.$file1)) {
			foreach ( $dosh as $dontshow ) {
				if ( strtolower($dontshow) == strtolower($file1) ) {
					$hidethisfile = true;
				}
			}
			$pos = strpos($site.$file1,"_data");
			if ( @$hidethisfile == false and $pos !== false ) {
				$hidethisfile = true;
			}
			if ( @$hidethisfile == false ) {
				$tfiles++;
				htmlspecialchars(stripslashes(generate($view,strtolower($file1),"file",$site)));
			} elseif ( @$hidethisfile == true and $fenile['sysfiles'] == true ) {
				$tfiles++;
				htmlspecialchars(stripslashes(generate($view,strtolower($file1),"file",$site,true)));
			}
		}
		$hidethisfile = false;
	}

} else {
	echo "<br /><h2><strong>No Directories / Files!</strong></h2><br /><br />";
	$nofiles = true;
}

// TABLE FOOTER
if ( $view == 1 and @$nofiles != true ) {
	echo "
		<thead class='tableHead'>
			<tr class='head'>
				<th align='center'>
				Select All: <input type='checkbox' name='selectall' id='selectall' onClick='selectAll()' /></th>
				<th align='center'>";
				if ( @!$tfiles ) {
					$tfiles = 0;
				} elseif ( @!$tdirs ) {
					$tdirs = 0;
				}
				echo "{$tdirs} ";
				echo($tdirs==1?"directory":"directories");
				echo ", {$tfiles} file(s)";
				echo "</th><th></th><th align='center'>";
				if ( @!$totalsize ) {
				echo "0 Bytes";
				} else {
				echo size_hum_read($totalsize);
				}
				echo "
				<th align='center'>Free Space: ";
				echo(size_hum_read(disk_free_space("/")) ? size_hum_read(disk_free_space("/")) : "-");
				echo "</th>
				<th></th>
				<th></th>
			</tr>
		</thead>
	";
}
?>
</form>
<div style="clear:both"></div>
</div>
</div>
</div>
<?php /*
function ae_detect_ie()
{
    if (isset($_SERVER['HTTP_USER_AGENT']) && 
    (strpos($_SERVER['HTTP_USER_AGENT'], 'MSIE') !== false))
        return true;
    else
        return false;
}
if (ae_detect_ie() == false) {
?>
<div id="footer" align="center">
Fenile Project - Some Rights Reserved (cc) niikoo 2006-<?php echo (date('Y') > '2006' ? date('Y') : "2007");?>
</div>
<?php } */ ?>
<script type="text/javascript">
<!--
var TabbedPanels1 = new Spry.Widget.TabbedPanels("tabs");
//-->
</script>
</body>
</html>
