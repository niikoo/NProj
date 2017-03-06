<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="refresh" content="120">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>Server status</title>
<style type="text/css">
html,body {
	margin:0;
	padding:0;
	color:white;
	background:#333333;
	-moz-transition:all 4s ease;
	font-family:"MS Reference Sans Serif","Calibri",Calibri,Arial;
	font-size:16px;
}
@font-face {
  font-family: "MS Reference Sans Serif";
  src: url(refsan.ttf) format("truetype");
}
#outer_container {
	width:900px;
	height:auto;
	margin:0 auto;
}
#container {
	width:884px;
	height:auto;
	padding-top:20px;
	margin:0 auto;
	background:#000000;
	border-left:#FFF 2px solid;
	border-right:#FFF 2px solid;
	border-bottom:#FFF 2px solid;
}
#clock {
	position:fixed;
	top:12px;
	right:12px;
	font-size:32px;
	width:122px;
	height:40px;
	padding:10px 10px 10px 10px;
	background:#222222;
	border:#FFF 2px solid;
}
#table {
	margin:0 auto;
	width:100%;
}
.outerbox {
	width:auto;
	height:auto;
	color:white;
	/*-moz-border-radius:15px 15px;*/
	-moz-transition:all 1s ease;
	background:-moz-linear-gradient(top, #2af782, #2fa628);
}
.redbox {
	background:-moz-linear-gradient(top, #f62929, #a70101);
}
.greyedbox {
	background:-moz-linear-gradient(top, #999999, #666666);
}
</style>
<script type="text/javascript">
var tick;
  function stop() {
  clearTimeout(tick);
  }
  function clock() {
  var ut=new Date();
  var h,m,s;
  var time="        ";
  h=ut.getHours();
  m=ut.getMinutes();
  s=ut.getSeconds();
  if(s<=9) s="0"+s;
  if(m<=9) m="0"+m;
  if(h<=9) h="0"+h;
  time+=h+":"+m+"<span style='font-size:15px'>:"+s+"</span>";
  document.getElementById('clock').innerHTML=time;
  tick=setTimeout("clock()",1000); 
  }
</script>
</head>
<body onload="clock();" onunload="stop();">
<div id="clock"><!-- Floating Clock - Se JavaScript --></div>
<div id="outer_container">
<img src="img/server_headL.png" />
<div id="container">
<?php
function HTTPpost($host, $path, $data_to_send,$user,$pass,$agent) {
	$fp = fsockopen($host,8080, @$err_num, @$err_msg, 10);
	if (!$fp) {
		die("$err_msg ($err_num)<br>\n");
	} else {
		$auth = $user.":".$pass ;
		$string=base64_encode($auth);
		echo $string;
		fputs($fp, "POST $path HTTP/1.1\r\n");
		fputs($fp, "Authorization: Basic ".$string."\r\n");
		fputs($fp, "User-Agent: ".$agent."\n");
		fputs($fp, "Host: $host\n");
		fputs($fp, "Content-type: application/x-www-form-urlencoded\n");
		fputs($fp, "Content-length: ".strlen($data_to_send)."\n");
		fputs($fp, "Connection: close\n\n");
		fputs($fp, $data_to_send);
		for ($i = 1; $i < 10; $i++){$reply = fgets($fp, 256);}
			fclose($fp);
	}
}
function alert($msgBOXtext) {
	echo "<script> alert(\"$msgBOXtext\"); </script>";
}
$servere = file("ipliste.csv");
$ltime = file("ping_la.txt");
foreach($servere as $linjenr => $data) {
	if($linjenr!=0) {
		$data = str_replace(";;",";",$data);
		$data = explode(";",$data);
		$server[] = @strtolower($data[0]); //o først ved sortering
		$funksjon[] = @$data[1];
		$os[] = @$data[2];
		$ip[] = @$data[3];
		$plassering[] = @$data[4];
		if(!@stristr($data[5],"iris")) {
			$viretuell[] = @$data[5];
		} else {
			$viretuell[] = "&nbsp;";
		}
		$domene[] = @$data[6];
	}
}
/*$sort = "n";
if($sort=="n") { // etter navn
	$xsx = $oserver;
	$xst = $oserver;
	sort($xst);
} elseif($sort=="i") { // etter IP
	$xsx = $oip;
	$xst = $oip;
	sort($xst);
} else { // etter plassering
	$xsx = $oplassering;
	$xst = $oplassering;
	sort($xst);
}
for($i=0;$i<count($xst);$i++) {
	for($y=0;$y<count($xsx);$y++) {
		if($xsx[$y] == $xst[$i]) {
			if($sort=="n") {
				$ip[$i] = $oip[$y];
			} elseif($sort=="i") {
				$server[$i] = $oserver[$y];
			} else {
				$plassering[$i] = $oplassering[$y];
			}
		}
	}
}
if($sort=="n") {
	$server = $xst;
} elseif($sort=="i") {
	$ip = $xst;
} else {
	$plassering = $xst;
}*/
echo "<table class='contentbox' border='0'>";
$num = 0;
$smslist = file("ping_sms.txt");
$sms_msg = $smslist[1];
for($i=2;$i<count($smslist);$i++) {
	$sx = explode(";;",$smslist[$i]);
	$sms[strtolower($sx[0])] = $sx[1];
}
file_put_contents("ping_la.txt","");
foreach($ip as $num => $ips) {
	if($plassering[$num] != @$plass and strlen($plassering[$num]) > 4) {
		echo "<tr class='outerbox greyedbox'><td colspan='6' style='font-size:18px;'>".$plassering[$num]."</td></tr>";
	}
	if($ips != "") {
		if(ping($ips)) {
			if(@is_file("./sms_sent/".$server[$num]."1.txt")) {
				@unlink("./sms_sent/".$server[$num]."1.txt");
				@unlink("./sms_sent/".$server[$num]."2.txt");
				@unlink("./sms_sent/".$server[$num]."3.txt");
				@unlink("./sms_sent/".$server[$num]."4.txt");
			}
			$ltime[$num] = date("H:i:s d.m-Y");
			file_put_contents("ping_la.txt",file_get_contents("ping_la.txt").$ltime[$num]."\r\n");
			echo "<tr class='outerbox' style='font-size:13px' onclick='alert(\"Sist respons: ".$ltime[$num]."\")'><td width='100px' style='font-size:13px'><b>".$server[$num]."</b></td><td width='350px' style='font-size:9px'>".$funksjon[$num]."</td><td width='250px' style='font-size:9px'>".$os[$num]."</td><td width='80px' style='font-size:10px'>".$ips."</td><td width='70px' colspan='2'>".@$viretuell[$num]." </td></tr>";
		} else {
			if(isset($sms[strtolower($server[$num])])) {
				$sms_nr = explode(",",$sms[strtolower($server[$num])]);
				$sms_msg = str_replace("{!LAST!}",str_replace("\r\n","",$ltime[$num]),$sms_msg);
				$sms_msg = str_replace("{!SERVER!}",$server[$num],$sms_msg);
				if(!is_file("./sms_sent/".$server[$num]."1.txt")) {
					file_put_contents("./sms_sent/".$server[$num]."1.txt",date("nj;;H"));
				} elseif(!is_file("./sms_sent/".$server[$num]."2.txt")) {
					file_put_contents("./sms_sent/".$server[$num]."2.txt",date("nj;;H"));
				} elseif(!is_file("./sms_sent/".$server[$num]."3.txt")) {
					file_put_contents("./sms_sent/".$server[$num]."3.txt",date("nj;;H"));
				} elseif(!is_file("./sms_sent/".$server[$num]."4.txt")) {
					file_put_contents("./sms_sent/".$server[$num]."4.txt",date("nj;;H"));
					for($ix = 0;$ix < count($sms_nr);$ix++) {
						//HTTPpost("firebolt.netcom.no","/sms/send","number={$sms_nr[$ix]}&message={$sms_msg}","username","password","Mozilla/4.0(compatible; MSIE 5.5; Windows NT 5.0)"); // ADD PW AND USER HERE FOR SMS
					}
				}
			}
			echo "<tr class='outerbox redbox' style='font-size:13px' onclick='alert(\"Sist respons: ".str_replace("\r\n","",$ltime[$num])."\")'><td width='100px' style='font-size:16px'><b>".$server[$num]."</b></td><td width='350px' style='font-size:9px'>".$funksjon[$num]."</td><td width='250px' style='font-size:9px'>".$os[$num]."</td><td width='80px' style='font-size:10px'>".$ips."</td><td width='70px'>".@$viretuell[$num]." </td><td>".str_replace("\r\n","",$ltime[$num])."</td></tr>";
			file_put_contents("ping_la.txt",file_get_contents("ping_la.txt").$ltime[$num]);
		}
		$plass = $plassering[$num];
		$num++;
	}
}
echo "</table>";

function ping($host) {
    $package = "\x08\x00\x19\x2f\x00\x00\x00\x00\x70\x69\x6e\x67";

    /* create the socket, the last '1' denotes ICMP */   
    $socket = socket_create(AF_INET, SOCK_RAW, 1);
   
    /* set socket receive timeout to 1 second */
    socket_set_option($socket, SOL_SOCKET, SO_RCVTIMEO, array("sec" => 1, "usec" => 0));
   
    /* connect to socket */
    socket_connect($socket, $host, null);
   
    /* record start time */
    list($start_usec, $start_sec) = explode(" ", microtime());
    $start_time = ((float) $start_usec + (float) $start_sec);
   
    socket_send($socket, $package, strlen($package), 0);
   
    if(@socket_read($socket, 255)) {
        list($end_usec, $end_sec) = explode(" ", microtime());
        $end_time = ((float) $end_usec + (float) $end_sec);
   
        $total_time = $end_time - $start_time;
       
        return true;
		//return $total_time;
    } else {
        return false;
    }
   
    socket_close($socket);
}
?>
</div>
</div>
</body>
</html>