<?php
if ( $dir_display = @$_REQUEST['dir_disp'] ) {
	$_SESSION['fenile_lastview'] = $dir_display;
	//setcookie("fenile_lastview",$dir_display) or die("ERROR: Can't Create Cookie");
	redirectURL("?side=".$site);
	exit;
}
if ( isset($_REQUEST['installdel']) ) {
	unlink("_data/install.php");
	alert('Install File; install.php deleted!');
}

// ACTIONS //

if ( @$_REQUEST['act'] == "logout" ) {
	session_destroy();
	alert("You're logged out!");
	redirectURL("?");
	exit;
}
if ( @$_REQUEST['act'] == "edit" and @$_REQUEST['file'] and @$_REQUEST['text'] ) {
		if ( @$_REQUEST['saveas'] and @$_REQUEST['saveas_name'] ) {
			if(file_exists($site.$_REQUEST['saveas_name']) ) {
				alert("The File ".$_REQUEST['saveas_name']." Already Exist");
			}
			$fp = fopen($site.$_REQUEST['saveas_name'],"w+") or die(error(40));
			fwrite($fp,@$_REQUEST['text']);
			alert('Save Complete');
			redirectURL($_SERVER['PHP_SELF']."?side=".$site);
			exit;
		} elseif ( @$_REQUEST['save'] ) {
			$fp1 = fopen($site.$_REQUEST['file'],"w+") or die(error(40));
			fwrite($fp1,@$_REQUEST['text']) or die(error(41));
			alert('Save Complete');
			redirectURL("?act=edit&side=".$site."&file=".$_REQUEST['file']);
			exit;
		} else {
			alert('ERROR: Save Error');
			redirectURL("?act=edit&side=".$site."&file=".$_REQUEST['file']."&text=".$_REQUEST['text']);
		}
		exit;
}
if ( @$_REQUEST['act'] == "hssf" ) {
	if ( @$fenile['sysfiles'] != true ) {
		mysql_query("UPDATE fenile_config SET value='1' WHERE id='sysfiles'") or die(error('6',mysql_error()));
		alert("System files: Visible");
	} else {
		mysql_query("UPDATE fenile_config SET value='0' WHERE id='sysfiles'") or die(error('6',mysql_error()));
		alert("System files: Hidden");
	}
	redirectURL("?side=".$site);
}
if ( @$_REQUEST['act'] ) {
	$act = @$_REQUEST['act'];
	$file2 = @$_REQUEST['file'];
	if ( $actfp = @fopen("_data/act/".$act.".php","r") ) {
		require "_data/act/".$act.".php";
		exit;
	} else {
		echo "<br /><br />ERROR: Can't Find This Action";
		exit;
	}
}
?>