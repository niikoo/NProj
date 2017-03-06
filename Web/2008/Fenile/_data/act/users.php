<?php
if ( isset($_REQUEST['u_add'],$_REQUEST['u_add_username'],$_REQUEST['u_add_password']) ) {
	// ADD USER
	$username = $_REQUEST['u_add_username'];
	$password = $_REQUEST['u_add_password'];
	$password = crypt($password,$password."fpmbn0270");
	$result = mysql_query("SELECT * FROM fenile_users WHERE username = '".$username."';");
	while ( $row = mysql_fetch_array($result) ) {
		if ( $row['password'] == $password ) {
			$userex = 1;
		}
	}
	if ( @$userex == 1 ) {
		error(23,$username);
	} else {
		$query = mysql_query("INSERT INTO fenile_users SET username = '".$username."', password = '".$password."';") or error(6,mysql_error());
		if ( @$query ) {
			alert("The user ".$username." is created");
			redirectURL($_SERVER['PHP_SELF']."?side=".$site);
		} else {
			error(8,$username);
		}
	}
} elseif ( isset($_REQUEST['u_delete'],$_REQUEST['u_add_username'],$_REQUEST['u_add_password']) ) {
	//DELETE USER
	$username = $_REQUEST['u_select_username'];
	$query = mysql_query("DELETE FROM fenile_users WHERE username='".$username."';") or error(6,mysql_error());
	if ( @$query ) {
		alert("The user ".$username." is deleted");
		redirectURL($_SERVER['PHP_SELF']."?side=".$site);
	} else {
		error(8,$username);
	}
} else {
	error(15);
}
?>