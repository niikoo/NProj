<?php
header( "Expires: Mon, 20 Dec 1998 01:00:00 GMT" );
header( "Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT" );
header( "Cache-Control: no-cache, must-revalidate" );
header( "Pragma: no-cache" );
?>
<!--<form method="get">
<input type="text" value="<?php echo @$_REQUEST['remote']; ?>" name="remote" />
<input type="text" value="<?php echo @$_REQUEST['port']; ?>" name="port" />
<select name="command">
	<option value="on">On</option>
    <option value="off">Off</option>
</select>
<input type="submit" name="submit" value="Ok" />
</form>-->
<?php
$address = @$_REQUEST['remote'];
$port = @$_REQUEST['port'];
  if (isset($_REQUEST['port']) and
      (!strlen($_REQUEST['port'])==0))
    $port=$_REQUEST['port'];
  else
    unset($port);
   
  if (isset($port) and ($socket=socket_create(AF_INET, SOCK_STREAM, SOL_TCP)) and (@socket_connect($socket, $address, $port)))
    {
      $text="Connection successful on IP $address, port $port";
    }
  else
    $text="Unable to connect<pre>".socket_strerror(socket_last_error())."</pre>";
   
  echo "<html><head><META HTTP-EQUIV='CACHE-CONTROL' CONTENT='NO-CACHE'></head><body>".
       $text.
       "</body></html>";
/*$line = "\x00\xbf\x00\x00\x01\x00\xc0";
@socket_send($socket,$line,strlen($line),0) or print("error");
$line = "\x00\x85\x00\x00\x01\x04\x8a";
@socket_send($socket,$line,strlen($line),0) or print("error");*/
echo " ".date("H:i:s:")." ";
echo " COMMAND USED: ".@$_REQUEST['command']." ";
if(@$_REQUEST['command'] == "on") {
	$line = "\x02\x00\x00\x00\x00\x02";
	for($i=0;$i<40;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
	if(@strstr(socket_read($socket,1400),"\x22\x00")) {
		echo " TURNED ON ";
	} else {
		echo " ERROR TURNING ON ";
	}
} elseif(@$_REQUEST['command'] == "off") {
	$line = "\x02\x01\x00\x00\x00\x03";
	for($i=0;$i<40;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
	if(@strstr(socket_read($socket,1400),"\x22\x00")) {
		echo " E-TURNED OFF ";
	} else {
		echo " X-TURNING OFF ";
	}
} elseif(@$_REQUEST['command'] == "freeze_on") {
	$line = "\x01\x98\x00\x00\x01\x01\x9B";
	for($i=0;$i<30;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
} elseif(@$_REQUEST['command'] == "freeze_off") {
	$line = "\x01\x98\x00\x00\x01\x02\x9C";
	for($i=0;$i<30;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
} elseif(@$_REQUEST['command'] == "vidmute_on") {
	$line = "\x02\x10\x00\x00\x00\x12";
	for($i=0;$i<40;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
	if(@strstr(socket_read($socket,1400),"\x22\x10")) {
		echo " E-VIDEMUTE ON ";
	} else {
		echo " X-VIDMUTE ON ";
	}
} elseif(@$_REQUEST['command'] == "comp1") {
	$line = "\x02\x03\x00\x00\x02\x01\x01\x09";
	for($i=0;$i<40;$i++) {
		socket_send($socket,$line,strlen($line),0) or print("error");
	}
	if(@strstr(socket_read($socket,1400),"\x22\x10")) {
		echo " COMP1 ";
	}
} elseif(@$_REQUEST['command'] == "vidmute_off") {
	$line = "\x02\x11\x00\x00\x00\x13";
	for($i=0;$i<40;$i++) {
		@socket_send($socket,$line,strlen($line),0) or print("error");
	}
	if(@strstr(socket_read($socket,1400),"\x22\x11")) {
		echo "  VIDMUTE OFF ";
	} else {
		echo " ERROR VIDMUTE OFF ";
	}
} else {
	echo "error no command!!!!";
}
echo " Sent: {$line} Output from projector: " . @socket_read($socket,1400);
@socket_close($socket);
?>