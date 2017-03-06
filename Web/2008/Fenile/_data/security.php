<?php
//////////////////////////
/* FENILE SECURITY FILE */
//////////////////////////

/*if ( $_SERVER['HTTP_HOST'] != "127.0.0.1" ) {
	die("ERROR: Hacker Alert!");
}*/
if ( isset($_SESSION['fenile_password'],$_SESSION['fenile_username'],$_SESSION['fenile_login']) ) {
	$f_user     = $_SESSION['fenile_username'];
	$f_password = $_SESSION['fenile_password'];
	$result = mysql_query("SELECT * FROM fenile_users WHERE username = '".$f_user."';") or die(error(22,$f_user));
	while ( $row = mysql_fetch_array($result) ) {
		if ( $row['password'] != $f_password ) {
			//alert($row['password']." ".$f_password);
			alert('ERROR: Login Error!');                               
			session_destroy();
		}
	}	
	unset($result,$f_user,$f_password);
} else {
	if ( isset($_REQUEST['l_submit'],$_REQUEST['l_username'],$_REQUEST['l_password']) ) {
		$f_user     = $_REQUEST['l_username'];
		$f_password = $_REQUEST['l_password'];
		$f_password = crypt($f_password,$f_password);
		$result = mysql_query("SELECT * FROM fenile_users WHERE username = '".$f_user."';") or die(error(22,$f_user));
		while ( $row = mysql_fetch_array($result) ) {
			if ( $row['username'] == $f_user and $row['password'] == $f_password ) {
				$_SESSION['fenile_username'] = $f_user;
				$_SESSION['fenile_password'] = $f_password;
				$_SESSION['fenile_login']    = true;
				alert("You're logged in!");
				redirectURL('?');
				exit;
			}
		}
		alert('Wrong Username / Password');
		unset($result,$f_user,$f_password);
		goBACK(-1);
		exit;
	} elseif ($fenile['install'] == true) {
		$fenile['login_r'] = true;
	} else {
		$fenile['login_r'] = false;
	}
}
?>