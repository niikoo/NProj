<?php
if ( @!$_REQUEST['filename'] ) {
	alert("No files selected");
	goBACK(-1);
	exit;
} else {
	$filename = $_REQUEST['filename'];
}
if ( @$_REQUEST['rename_f'] and $_REQUEST['filename'] and $originalname = @$_REQUEST['originalname'] ) {
	foreach ( $filename as $var => $filename ) {
		if ( !file_exists($site.$filename) ) {
			rename($site.$originalname[$var],$site.$filename)
			or error(37,$originalname[$var],$filename);
			exit;
		} else {
			error(36,$originalname[$var],$filename);
			exit;
		}
	}
	redirectURL($_SERVER['PHP_SELF']."?side=".$site);
	exit;
}
echo "<h2>Rename Files</h2><br>";
echo "<form action='{$_SERVER['PHP_SELF']}?rename=1&rename_f=1&side={$site}' method='post'>";
$i=0;
$nr=1;
foreach ( $filename as $var => $filename ) {
	echo "<br />{$nr} - <input type='text' value='{$filename}' name='filename[]' /> - {$filename}
	<input type='hidden' name='originalname[]' value='{$filename}' />";
	$nr++;
}
echo "<br /><br /><input type='submit' value='   .: Rename Files :.   ' /><input type='reset' value=' .: Reset :. ' /><br></form>";
?>