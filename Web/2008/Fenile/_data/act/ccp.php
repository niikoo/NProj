<?php
if ( @!$_REQUEST['paste'] ) {
	if ( @!$_REQUEST['filename'] ) {
		alert('No Files Selected or Unselected Type');
		goBACK(-1);
		exit;
	} else {
		$filename = $_REQUEST['filename'];
	}
}
if ( isset($_REQUEST['copy']) ) {

$_SESSION['copy_side'] = $site; 
$_SESSION['copy_action'] = "copy";

foreach ( $filename as $id => $name ) {
	$_SESSION['copy_files'][] = $name;
}

alert("Copy");
redirectURL('?side='.$site);
exit;

} elseif ( isset($_REQUEST['cut']) ) {

$_SESSION['copy_side'] = $site; 
$_SESSION['copy_action'] = "cut";

foreach ( $filename as $id => $name ) {
	$_SESSION['copy_files'][] = $name;
}

alert("Cut");
redirectURL('?side='.$site);
exit;

} elseif ( isset($_REQUEST['paste']) ) {
	if ( isset($_SESSION['copy_side'],$_SESSION['copy_action'],$_SESSION['copy_files']) ) {
		$from = $_SESSION['copy_side'];
		$action = $_SESSION['copy_action'];
		$filename1 = $_SESSION['copy_files'];
		foreach ( $filename1 as $id => $name ) {
			if ( $action == "cut" ) {
				copy($from.$name,$site.$name) or die(error(21,$name));
				unlink($from.$name);
			} elseif ( $action == "copy" ) {
				copy($from.$name,$site.$name) or die(error(21,$name));
			} else {
				die(error(2));
			}
		}
		unset($_SESSION['copy_side'],$_SESSION['copy_action'],$_SESSION['copy_files']);
		alert('Pasted');
		redirectURL('?side='.$site);
		exit;
	} else {
		die(error(18));
	}
} else {
	die(error(20));
}
?>