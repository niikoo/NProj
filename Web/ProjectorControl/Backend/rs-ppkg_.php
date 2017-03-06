<?php
session_start();
$errorno = 0;
$errorstr = "";

class PPKG {
	public $error = array(
		"no" => 0,
		"str" => ""
	);
	
	private function error($errorno,$errorstr,$die=false) {
		$this->error['no'] = $errorno;
		$this->error['str'] = $errorstr;
		echo "<br><h2>ERROR ".$errorno.": ".$errorstr."</h2><br>";
		if($die) die();
	}
	
	private $socket = false;
	
	function __construct($address,$port) {
		if (!$this->socket=stream_socket_client("tcp://".$address.":".$port, $errno, $errstr)) {
			$this->error($errno,$errstr,true);
		}
		$this->getInfo();
	}
	function __destruct() {
		stream_socket_shutdown($this->socket,STREAM_SHUT_RDWR);
	}
	
	function sendCommand($cmd,$decode=true,$outputJSON=false) {
		$sent = stream_socket_sendto($this->socket,$cmd['send']);
		if($sent > 0) {
			$server_response = fread($this->socket,1500);
			$server_response = unpack("H*", $server_response);
			$server_response = current($server_response);
			if($decode == true) {
				$this->getInfo();
				$result = $this->decodeResponseFromNEC($cmd,$server_response);
				if($outputJSON == true) {
					echo json_encode(array(
						"status" => $this->status,
						"result" => $result
					));
					exit;
				}
			} else {
				return $server_response;
			}
		} else {
			return $this->output(true,"Unable to send");
		}
	}
	
	/***
	* Reads an NEC response -> Should be strlen(12) hex unpacked
	*/
	function decodeResponseFromNEC($cmd,$response) {
		//var_dump($response);
		// EXAMPLE NEC_CMD::ON == SUCCESS: 220001400063
		// 22 static 00 static 01 ID1 40 ID2 00 DATA1 63 CHK
		if($response != false and strlen($response) >= 12) {
			return $this->output(true,"sent successfully (hopefully working :])");
			/*$j = 0;
			for($i=0;$i<=strlen($response);$i+=2) {
				//if(substr($response,$i,2) == $cmd['success'][$j]) {
					
				//}
				$j++;
			}*/
			// BREAK UP STRING TO 2+2+2+2+2 array
		} else {
			return $this->output(true,"Sending failed");
		}
	}
	
	/***
	* Get info from the NEC projector
	*/
	public function getInfo() {
		$response = $this->sendCommand(NEC_CMD::$getInfo,false);
		$response = str_split($response,2);
		//echo "<pre>";
		$data = array();
		$data["all"] = $response;
		$data[1] = $response[6]; // Status - On or standby
		$data[4] = $response[9]; // Input
		$data[6] = $response[11]; // Video mute
		$data[9] = $response[14]; // Freeze
		if($data[1] == "04") {
			$this->status['on'] = true;
		} else {
			$this->status['on'] = false;
		}
		if($data[6] == "01") {
			$this->status['muted'] = true;
		} else { //00
			$this->status['muted'] = false;
		}
		if($data[9] == "01") {
			$this->status['frozen'] = true;
		} else {
			$this->status['frozen'] = false;
		}
		return $data;
	}
	
	public $status = array(
		"on" => false,
		"frozen" => false,
		"muted" => false
	);
	
	private function output($result, $description, $returnData = array()) {
		return array(
			"result" => $result,
			"description" => $description,
			"data" => $returnData
		);
	}
	
	static $specialTags = array(
		"ID1" => array(
			"description" => "Identification part 1",
			"handler" => false
		),
		"ID2" => array(
			"description" => "Identification part 2",
			"handler" => false
		)
	);
}

class NEC_CMD {
	// Unpacked hex: $err1.$err2 as key -> Value as description
	static $errorList = array(
		"0000" => "The command cannot be recognized.",
		"0001" => "The command is not supported by the model in use.",
		"0100" => "The specified value is invalid.",
		"0101" => "The specified input terminal is invalid.",
		"0102" => "The specified language is invalid.",
		"0200" => "Memory allocation error",
		"0202" => "Memory in use",
		"0203" => "The specified value cannot be set.",
		"0204" => "Forced onscreen mute on",
		"0206" => "Viewer error",
		"0207" => "No signal",
		"0208" => "A test pattern or filer is displayed.",
		"0209" => "No PC card is inserted.",
		"020A" => "Memory operation error",
		"020C" => "An entry list is displayed.",
		"020D" => "The command cannot be accepted because the power is off.",
		"020E" => "The command execution failed.",
		"020F" => "There is no authority necessary for the operation.",
		"0300" => "The specified gain number is incorrect.",
		"0301" => "The specified gain is invalid.",
		"0302" => "Adjustment failed."
	);
	
	// Arrays
	// send: hex to send
	// recv: hex to receive for if successful
	static $on = array(
		"send" => "\x02\x00\x00\x00\x00\x02",
		// SPECIAL TAGS: ERR1, ERR2, DATA1 - DATA12, CKS, ID1, ID2
		"success" => array("A2","00","ID1","ID2","00","CKS"), // How to understand the response in case of successs > empty data (unpakced hex)
		"fail" => array("A2","00","ID1","ID2","02","ERR1","ERR2","CKS") // How to understand the response in case of failure > error message (unpakced hex)
	);
	
	static $off = array(
		"send" => "\x02\x01\x00\x00\x00\x03"
	);
	
	static $input = array(
		"computer1" => array( // COMPUTER 1 - VGA
			"send" => "\x02\x03\x00\x00\x02\x01\x01\x09",
		),
		"computer2" => array( // COMPUTER 2 - HDMI
			"send" => "",
		)
	);
	
	static $freeze = array(
		"on" => array(
			"send" => "\x01\x98\x00\x00\x01\x01\x9B" // freeze on
		),
		"off" => array(
			"send" => "\x01\x98\x00\x00\x01\x02\x9C" // freeze off
		)
	);

	static $videoMute = array(
		"on" => array(
			"send" => "\x02\x10\x00\x00\x00\x12" // vidmute on
		),
		"off" => array(
			"send" => "\x02\x11\x00\x00\x00\x13" // vidmute off
		)
	);	
	
	static $getInfo = array(
		"send" => "\x00\xBF\x00\x00\x01\x02\xC2" // Get info from the projector
	);
	
}
$projIP = "192.168.10.21"; // Default IP >> Can be overrided by ip.conf
if(is_file("./ip.conf")) {
	$projIP = htmlspecialchars(addslashes(trim(file_get_contents("./ip.conf"))));
}
$nec = new PPKG($projIP,"7142");
if(isset($_GET['cmd'])) {
	switch($_GET['cmd']) {
		case "on":
			if(!$nec->status['on'])
				$nec->sendCommand(NEC_CMD::$on,true,true);
			break;
		case "off":
			if($nec->status['on']) 
				$nec->sendCommand(NEC_CMD::$off,true,true);
			break;
		case "mute-on":
			if(!$nec->status['muted'])
				$nec->sendCommand(NEC_CMD::$videoMute['on'],true,true);
			break;
		case "mute-off":
			if($nec->status['muted'])
				$nec->sendCommand(NEC_CMD::$videoMute['off'],true,true);
			break;
		case "mute":
			if($nec->status['muted']) {
				$nec->sendCommand(NEC_CMD::$videoMute['off'],true,true);
			} else {
				$nec->sendCommand(NEC_CMD::$videoMute['on'],true,true);
			}
			break;
		case "freeze-on":
			if(!$nec->status['frozen'])
				$nec->sendCommand(NEC_CMD::$freeze['on'],true,true);
			break;
		case "freeze-off":
			if($nec->status['frozen'])
				$nec->sendCommand(NEC_CMD::$freeze['off'],true,true);
			break;
		case "freeze":
			if($nec->status['frozen']) {
				$nec->sendCommand(NEC_CMD::$freeze['off'],true,true);
			} else {
				$nec->sendCommand(NEC_CMD::$freeze['on'],true,true);
			}
			break;
		case "status":
			$nec->getInfo(); // RELOAD INFO
			echo json_encode(array(
				"status" => $nec->status,
				"result" => "status info"
			));
			exit;
			break;
		case "info":
			echo "<pre>";
			var_dump($nec->getInfo());
			echo "</pre>";
			echo "<br><h1>Current status</h1><pre>";
			print_r($nec->status);
			break;
	}
}
// IF NOT EXITED ALREADY > SHOW STATUS
$nec->getInfo(); // RELOAD INFO
echo json_encode(array(
	"status" => $nec->status,
	"result" => "No cmd runned!"
));
exit;
?>