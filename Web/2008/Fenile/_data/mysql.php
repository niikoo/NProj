<?php
#----MYSQL----#

if ( @file_exists("_data/config.php") ) {

function mysql_con() {
	global $fenile;
	
	$fenile['mysql_connection'] = 
	mysql_connect($fenile['mysql_host'],$fenile['mysql_user'],$fenile['mysql_password'])
	or die("ERROR: Can't Connect To MySQL database!");
	
	if ( $fenile['mysql_db'] ) {
		mysql_select_db($fenile['mysql_db'],$fenile['mysql_connection'])
		or die("ERROR: Can't Select DB: ".$fenile['mysql_db']);
	} else {
		mysql_select_db("fenile",$fenile['mysql_connection'])
		or die("ERROR: Can't Select standard DB: fenile");
	}
}
mysql_con();
$result = mysql_query("SELECT * FROM fenile_config;");
while ( $row = mysql_fetch_array($result) ) {
	$fenile[$row['id']] = $row['value'];
}
unset($result);

}


#------------#
# List Users #
#------------#

function list_users() {
	global $fenile;
	mysql_con();
	$result = mysql_query("SELECT * FROM fenile_users");
	$i=0;
	while ( $row = mysql_fetch_array($result) ) {
		echo "<option>".$row['username']."</option>";
		$i++;
	}
	unset($result);
}
?>