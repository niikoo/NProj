<?php
# INSTALL #
if ( @$fenile['install'] != true and isset($_REQUEST['i_terms']) ) {
	foreach($_POST as $var => $val) {
		$var = str_replace("i_","",$var);
		$fenile_i[$var] = $val;
		if ( $val == "" and $var != "mysql_password" ) {
			alert('Some Fields Are Empty');
			goBACK(-1);
			exit;
		}
	}
	$fenile_i['mysql_connection'] = mysql_connect($fenile_i['mysql_host'],$fenile_i['mysql_user'],$fenile_i['mysql_password'])
	or die(error(1));
	if ( @$fenile_i['mysql_db'] == "" and !mysql_query("SELECT DATABASE FENILE") ) {
		mysql_query("CREATE DATABASE fenile;");
		unset($rowi);
		$fenile_i['mysql_db'] = "fenile";
	}
	if ( $fenile_i['mysql_db'] ) {
		mysql_select_db($fenile_i['mysql_db'],$fenile_i['mysql_connection'])
		or die(error(4,$fenile_i['mysql_db']));
	} else {
		mysql_select_db("fenile",$fenile_i['mysql_connection'])
		or die(error(5));
	}
	if ( @$fenile_i['mysql_db'] == "" ) {
		mysql_query("CREATE DATABASE fenile;");
		unset($rowi);
		$fenile_i['mysql_db'] = "fenile";
	}
	if ( $fenile_i['mysql_db'] ) {
		mysql_select_db($fenile_i['mysql_db'],$fenile_i['mysql_connection'])
		or die(error(4,$fenile_i['mysql_db']));
	} else {
		mysql_select_db("fenile",$fenile_i['mysql_connection'])
		or die(error(5));
	}
	$query['droptables1'] = "DROP TABLE fenile_config;";	
	$query['droptables2'] = "DROP TABLE fenile_users;";	
	
	$query['config'] =
	"CREATE TABLE fenile_config (
	id varchar(255),
	value varchar(255)
	) type=MyISAM;";
	
	$query['users'] =
	"CREATE TABLE fenile_users (
	username varchar(255),
	password varchar(255)
	) type=MyISAM;";
	
	// INSERT INTO TABLE fenile_config //
	
	$query['theme'] = "INSERT INTO fenile_config SET id = 'theme',value='default'";
	$query['install'] = "INSERT INTO fenile_config SET id = 'install',value='true'";
	$query['usepw'] = "INSERT INTO fenile_config SET id = 'usepw',value='true'";
	$query['sysfiles'] = "INSERT INTO fenile_config SET id= 'sysfiles', value='false'";
	
		$user = $fenile_i['security_user'];
		$pass = @$fenile_i['security_password'];
		$pass = @crypt($pass,$pass."fpmbn0270");
		$query['username'] = "INSERT INTO fenile_users SET username = '".$user."', password = '".@$pass."';";
	
	
	$mysql_errors = 0;
	foreach($query as $name => $query) {
		$result[$name] = mysql_query($query)
		or $mysql_errors += 1;
	}
	$contents = "
	<?php
	//--------------------------//
	//FENILE PROJECT CONFIG FILE//
	//--------------------------//

	#----MYSQL----#
	\$fenile['mysql_host'] = '".$fenile_i['mysql_host']."'; # MYSQL HOSTNAME
	\$fenile['mysql_user'] = '".$fenile_i['mysql_user']."'; # MYSQL USERNAME
	\$fenile['mysql_password'] = '".$fenile_i['mysql_password']."'; # MYSQL PASSWORD
	\$fenile['mysql_db'] = '".$fenile_i['mysql_db']."'; # MYSQL DATABASE
	?>";
	file_put_contents("_data/config.php",$contents);
	if ( $mysql_errors >= 1 ) {
		alert('Some Errors Occured During The Installation Progress');
		alert('Total Errors: '.$mysql_errors);
		alert('Please Try Agian');
		mysql_query("UPDATE fenile_config SET value = 'false' WHERE id = 'install'");
		goBACK(-1);
		exit;
	}
	if ( @$fenile_i['other_delinstall'] == true ) {
		redirectURL("?installdel=1");
	} else {
		redirectURL("?");
	}
	exit;
}
?>
<style type="text/css">
#i_table {
	font-family:Verdana, Arial, Helvetica, sans-serif;
	font-size:10px;
}
.i_thead {
	font-size:11px;
	font-weight:bold;
}
</style>
<script type="text/javascript">
function i_mysql_db_checkbox() {
	if ( document.install.i_mysql_db_default.checked == true ) {
		document.install.i_mysql_db.value = "fenile";
		//document.install.i_mysql_db.style.backgroundColor = "#AAFFAA";
		document.install.i_mysql_db.disabled = true;
	} else {
		document.install.i_mysql_db.value = "";
		//document.install.i_mysql_db.style.backgroundColor = "#F1F1F1";
		document.install.i_mysql_db.disabled = false;
	}
}
function openClose_oc() {
	openClose('security_openclose1');
	openClose('security_openclose2');
	openClose('security_openclose3');
	openClose('security_openclose4');
	openClose('security_openclose5');
	openClose('security_openclose6');
}
function acptoa() {
	if ( document.install.i_terms.checked == true ) {
		document.install.i_submit.disabled = false;
	} else {
		document.install.i_submit.disabled = true;
	}
	openClose('nacp');
}
function pwc() {
	form = document.install
	if (form.i_security_password.value == form.i_security_password_confirm.value) {
		form.submit();
	} else {
		alert('Error: Passwords don\'t match');
		form.i_security_password.value = "";
		form.i_security_password_confirm.value = "";
		form.i_security_password.style.backgroundColor = "#FFAAAA";
		form.i_security_password_confirm.style.backgroundColor = "#FFAAAA";
	}
}
</script>
<form id="install" name="install" method="post" action="">
  <h2 align="center"><strong>Welcome To The Fenile Installation </strong></h2>
    <br />
	<table width="420" height="auto" border="0" align="center" id="i_table" style="border:1px #000000 dotted;">
  	  <tr>
        <td><div align="center" class="i_thead"><strong>MySQL:</strong></div></td>
        <td>&nbsp;</td>
      </tr>
	  <tr>
        <td width="215">MySQL Host: </td>
        <td width="161"><label>
          <input name="i_mysql_host" type="text" id="i_mysql_host" value="127.0.0.1" />
        </label></td>
      </tr>
      <tr>
        <td>MySQL Username: </td>
        <td><input name="i_mysql_user" type="text" id="i_mysql_user" value="root" /></td>
      </tr>
      <tr>
        <td>MySQL Password: </td>
        <td><input name="i_mysql_password" type="password" id="i_mysql_password" /></td>
      </tr>
      <tr>
        <td>MySQL Database: </td>
        <td><input name="i_mysql_db" type="text" id="i_mysql_db" value="fenile" />
          <label><br />
          Default:
          <input name="i_mysql_db_default" type="checkbox" id="i_mysql_db_default" onClick="i_mysql_db_checkbox()" checked="checked" />
        </label></td>
      </tr>
	  <tr height="1px">
        <td><hr size="1px" width="100%" color="#000000" /></td>
        <td><hr size="1px" width="100%" color="#000000" /></td>
      </tr>
	  <tr>
        <td><div align="center" class="i_thead"><strong>Password Protection:</strong></div></td>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td>Password Protect Fenile: </td>
        <td><input name="i_security_usepw" type="checkbox" id="i_security_usepw" onChange="openClose_oc()" checked="checked" disabled="disabled" /></td>
		<script> i_mysql_db_checkbox(); </script>
      </tr>
      <tr>
        <td><div id="security_openclose1">Admin Username:</div></td>
        <td><div id="security_openclose2">
		<input name="i_security_user" type="text" id="i_security_user" maxlength="25" />
        </div></td>
	  </tr>
      <tr>
        <td><div id="security_openclose3">Admin Password:</div></td>
        <td><div id="security_openclose4">
          <input name="i_security_password" type="password" id="i_security_password" maxlength="15" />
        </div></td>
      </tr>
      <tr>
        <td><div id="security_openclose5">Confirm Password:</div></td>
        <td><div id="security_openclose6">
          <input name="i_security_password_confirm" type="password" id="i_security_password_confirm" maxlength="15" />
        </div></td>
      </tr>
	  <tr height="1px">
        <td><hr size="1px" width="100%" color="#000000" /></td>
        <td><hr size="1px" width="100%" color="#000000" /></td>
      </tr>
	  <tr>
        <td><div align="center" class="i_thead"><strong>Other:</strong></div></td>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td>Theme:</td>
        <td>
			<select name="i_other_theme" id="i_other_theme" title="Select the fenile theme">
            	<?php get_themes(); ?>
       	  </select>		</td>
      </tr>
	  <tr>
        <td>Delete Installation File After Install: </td>
        <td>
		<input name="i_other_delinstall" type="checkbox" id="i_other_delinstall" checked="checked" title="Click here to delete the installation file after install" />
          (Recommended) </td>
      </tr>
	  <tr height="1px">
        <td><hr size="1px" width="100%" color="#000000" /></td>
        <td><hr size="1px" width="100%" color="#000000" /></td>
      </tr>
	  <tr>
        <td>I accept the <a href="javascript:void(0)" onClick="window.open('<?php echo $fenile['licence_url']; ?>')">I accept the <?php echo $fenile['licence_name']; ?> licence</a>: </td>
        <td><input name="i_terms" type="checkbox" id="i_terms" onchange="acptoa()" title="Click here to accept the <?php echo $fenile['licence_name']; ?> licence"/></td>
      </tr>
	  <tr>
        <td>
        	<input name="i_post" type="hidden" value="true"/>
			<input name="i_submit" type="button" id="i_submit" onclick="pwc()" value="     .:   Install   :.     " disabled="disabled" />
			<input name="i_reset" type="button" id="i_reset" onClick="history.go(0)" value=" .: Reset :. " />        </td>
        <td><div id="nacp" style="color:#990000">
		Accept the <?php echo $fenile['licence_name']; ?> licence to continue...
		</div></td>
      </tr>
      </table>
</form>
<div id="footer1" align="center">
  <h3><br /><a href="_data/readme.html" title="Open Fenile Readme" target="_blank">
    Open Readme</a></h3>
  <p>Fenile Project - Some Rights Reserved (cc) niikoo 2006-<?php echo (date('Y') > '2006' ? date('Y') : "2007");?></p>
  </div>
<?php exit; ?>