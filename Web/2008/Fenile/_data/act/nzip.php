<?php
$ziper = new zipfile();
if ( isset($_REQUEST['newzip_s']) ) {
	if ( isset($_REQUEST['filename']) and !isset($_REQUEST['new_zip']) ) {
		alert("Missing new zip filename");
		goBACK(-1);
	} elseif ( isset($_REQUEST['new_zip']) and !isset($_REQUEST['filename']) ) {
		alert("No files selected to add");
		goBACK(-1);
	} elseif ( isset($_REQUEST['filename'],$_REQUEST['new_zip'])) {
		$addfiles1 = array();
		foreach($_REQUEST['filename'] as $file6) {
			$addfiles1[] = $site.$file6;
		}
		if ( @$addfiles1 ) {
			$ziper->addFiles($addfiles1); //add files
			$ziper->output($site.$_REQUEST['new_zip'].".zip");
			alert("The zipfile: ".$_REQUEST['new_zip'].".zip is created");
			redirectURL("?side=".$site);
		} else {
			error(61);
			exit;
		}
	}
}
?>