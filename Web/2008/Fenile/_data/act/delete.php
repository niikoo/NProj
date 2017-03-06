<?php
if ( @!$_REQUEST['delete1'] ) {
	die();
}
if ( @!$_REQUEST['filename'] ) {
	alert('No Files Selected or Unselected Type');
	goBACK(-1);
	exit;
} else {
	$filename = $_REQUEST['filename'];
}

echo '
	<script>
	alert("Files / Folders To Delete: '.count($filename).'");
	</script>
';

$tfiles = 0;
$tdirs = 0;
$terrors = 0;
$tdels = 0;
$filestodelete = count($filename);

foreach ( $filename as $var => $filename ) {
	if ( is_writable($site.$filename) ) {
		if ( is_dir($site.$filename) ) {
			removeDir($site.$filename);
			$tdirs++;
			$tdels++;
		} elseif ( is_file($site.$filename) ) {
			unlink($site.$filename);
			$tfiles++;
			$tdels++;
		} else {
			alert("ERROR: Can't Find File");
			goBACK(-1);
			exit;
		}
	} elseif ( !is_writable($site.$filename) ) {
		alert('The File '.$filename.' is Read Only');
		$terrors++;
	} else {
		alert("ERROR: The File ".$filename." Doesn't Exist");
		$terrors++;
	}
}
if ( $terrors >= 1 and $tdels >= 1 ) {
	alert('Almost All Files Were Deleted');
} elseif ( $filestodelete == $tdels ) {
	alert("All Files Were Deleted: {$tfiles} File(s) and {$tdirs} Folder(s)");
} else {
	alert('No Files Deleted, '.$terrors.' Errors');
}
redirectURL($_SERVER['PHP_SELF']."?side=".$site);
?>
