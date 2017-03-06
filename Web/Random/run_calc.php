<?php
// NOM1@IRIS - NIKOLAI OMMUNDSEN
function ext_exec($cmd,$echo=false) {
	$wsh = new COM("WScript.Shell");
	$oexec = $wsh->Run($cmd,0,false);
	if($echo == true) {
		echo $cmd;
	}
	return ($oexec == 0) ? true : false;
}
if(isset($_REQUEST['calc'])) {
	ext_exec("calc.exe");
} elseif(isset($_REQUEST['word'])) {
	shell_exec("WINWORD");
} elseif(isset($_REQUEST['iexplore'])) {
	exec("iexplore");
} elseif(isset($_REQUEST['vg'])) {
	ext_exec("http://www.vg.no");
}
?>
<h1>PHP START APPLICATION ON SERVER</h1><br />
<a href="?calc">Calculator</a><br />
<a href="?vg">VG i standard nettleser</a><br />
