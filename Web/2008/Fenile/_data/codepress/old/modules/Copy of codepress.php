<?php
/*
 * CodePress - Real Time Syntax Highlighting Editor written in JavaScript -  http://codepress.fermads.net/
 *
 * Copyright (C) 2006 Fernando M.A.d.S. <fermads@gmail.com>
 *
 * This program is free software; you can redistribute it and/or modify it under the terms 
 * of the GNU Lesser General Public License as published by the Free Software Foundation.
 *
 * Read the full licence: http://www.opensource.org/licenses/lgpl-license.php
 *
 * Very simple implementation of server side script to open files and send to CodePress interface
 */

/*
 * root directory of files to open/edit from server
 * there are 2 ways to set the file directory. See examples below, edit and comment/uncomment the appropriate
 */

// RELATIVE to codepress directory instalation: will open file from [root path of your server]/[codepress directory]/examples/
if (@$_REQUEST['side']) {
	$site = $_REQUEST['side'];
	$lastchar = strlen($site)-1;
	if ( @$site[$lastchar] != "/" and $site != "" ) {
		$site .= "/";
	}
} else {
	$site = "";
}

$path['files'] = "examples/"; 

// ABSOLUTE from your webserver root path: will open files from [root path of your server]/examples/
// $path['files'] = "/examples"; 


/* * * * * * * * * * * * * * 
 * no need to change below * 
 * * * * * * * * * * * * * */
 
$path['webdocs'] = preg_replace("/\/modules/","",dirname($_SERVER['SCRIPT_NAME']));
$path['server'] = $_SERVER['DOCUMENT_ROOT'];

$code = "";
$engine = $_GET['engine'];
$language = (isset($_GET['language'])) ? $_GET['language'] : "generic" ;

if(isset($_GET['file'])) {
    $file = preg_replace("/\.\.\//","",$_GET['file']); // don't let users go up with ../../
	if($path['files']{0}=="/") $full_file = $path['server'].'/'.$path['files']."/".$file; // absolute path
	else $full_file = $path['server'].'/'.$path['webdocs'].'/'.$path['files']."/".$file; // relative path

	if(file_exists($full_file)) {
	    $code = htmlspecialchars(file_get_contents($full_file));
		//$code = preg_replace("/\r\n/","<br>",$code); // opera and khtml engines
	}
}

?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<meta name="description" content="CodePress source code editor window" />
	<link type="text/css" href="../themes/default/codepress.css?timestamp=<?=time()?>" rel="stylesheet" />
	<link type="text/css" href="../languages/<?php echo $language; ?>.css?timestamp=<?php echo time(); ?>" rel="stylesheet" id="cp-lang-style" />
	<script type="text/javascript" src="../engines/<?php echo $engine; ?>.js?timestamp=<?php echo time(); ?>"></script>
	<script type="text/javascript" src="../languages/<?php echo $language; ?>.js?timestamp=<?php echo time(); ?>"></script>
</head>
<?php
if (@$engine == "gecko") echo "<body id='code'>".$code."</body>";
else if(@$engine == "msie") echo "<body><pre id='code'>".$code."</pre></body>";
?>
</html>