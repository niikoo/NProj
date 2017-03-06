<?php
if ( @!$_REQUEST['filename'] and @$_REQUEST['chmods'] and @$_REQUEST['chmodn'] ) {
	alert('No Files Selected or Unselected Type');
	goBACK(-1);
	exit;
} else {
	$filename = $_REQUEST['filename'];
	$chmod = $_REQUEST['chmodn'];
}
foreach ( $filename as $var => $filename ) {
	if ( file_exists($site.$filename) ) {
		if ( strlen($chmod) != 3 ) {
			die(error(31));
		}
		if ( is_file($site.$filename) and $chmod ) {
			$chmod = '666';
		}
		switch($chmod) {
			case "777": chmod($site.$filename,'0777') or $error=1; break;
			case "755": chmod($site.$filename,'0755') or $error=1; break;
			case "750": chmod($site.$filename,'0750') or $error=1; break;
			case "666": chmod($site.$filename,'0666') or $error=1; break;
			case "644": chmod($site.$filename,'0644') or $error=1; break;
			case "444": chmod($site.$filename,'0444') or $error=1; break;
			default:    chmod($site.$filename,'0777') or $error=1; break;
		}
	} else {
		$error = true;
	}
}
if ( @$error != true ) {
	alert("CHMOD Complete");
} else {
	alert("CHMOD Error");
}
redirectURL($_SERVER['PHP_SELF']."?side=".$site);
exit;
?>