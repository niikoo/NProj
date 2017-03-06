<?php
/*
 * CodePress - Real Time Syntax Highlighting Editor written in JavaScript -  http://codepress.fermads.net/
 *
 * Copyright (C) 2006 Fernando M.A.d.S. <fermads@gmail.com>
 * Copyright (C) 2007 niikoo <nikolai@lyse.net>
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
// ABSOLUTE from your webserver root path: will open files from [root path of your server]/examples/
// $path['files'] = "/examples"; 


/* * * * * * * * * * * * * * 
 * no need to change below * 
 * * * * * * * * * * * * * */
 
$path['webdocs'] = preg_replace("/\/modules/","",dirname($_SERVER['SCRIPT_NAME']));
$path['server'] = $_SERVER['DOCUMENT_ROOT'];

$code = "";
$engine = $_REQUEST['engine'];
$language = (isset($_REQUEST['language'])) ? $_REQUEST['language'] : "generic" ;

if(isset($_REQUEST['file'])) {
    $file = "../../../".$_REQUEST['file'];
	if(file_exists($file)) {
	    $code = htmlspecialchars(@file_get_contents($file));
	}
}

?>
	<link type="text/css" href="../themes/default/codepress.css?timestamp=<?=time()?>" rel="stylesheet" />
	<link type="text/css" href="../languages/<?php echo $language; ?>.css?timestamp=<?php echo time(); ?>" rel="stylesheet" id="cp-lang-style" />
	<script type="text/javascript" src="../engines/<?php echo $engine; ?>.js?timestamp=<?php echo time(); ?>"></script>
	<script type="text/javascript" src="../languages/<?php echo $language; ?>.js?timestamp=<?php echo time(); ?>"></script>
<?php
if (@$engine == "gecko") echo "<body id='code'>".$code."</body>";
else if(@$engine == "msie") echo "<body><pre id='code'>".$code."</pre></body>";
?>