<?php
# HUSSYSTEM - NIKOLAI OMMUNDSEN - NIIKOO - 2013/2014
date_default_timezone_set("Europe/Oslo");
$info = array(
	"system_title" => "Husplan",
	"place" => "Mus&eacute;gata 72, 4010 Stavanger, Norway"
);
$ukenr = date("W-o");

/* CONTACT LIST */
$cl = json_decode(trim(file_get_contents("contacts.json")),true); // IMPORT

/* SNAKE HIGHSCORES */
$snake_highscores_save = "snake_highscores.json";
$snake_highscore_limit = 5; // MAX HIGHSCORES
$snhs = json_decode(file_get_contents($snake_highscores_save),true); // GET CONTENTS -> JSON_DECODE -> TO $snhs (snake highscore) [array]
krsort($snhs);

// SANDER'S OFFSHORE ROTASJON - Deltar Sander?????? -- [IKKE I BRUK]
/*$offshore_rotasjon = true; // (bool) Deltar Sander?
$ref_uke = 201402; // REFERANSEUKE - INKL. DENNE 4 UKER HJEMME, S� 2 UKER BORTE
$ref_status = true; // Første uke som true
$denne_uke = date("oW");
// 4 onshore, 2 offshore
$cuke = $ref_uke;
$cstatus = $ref_status;
$cnt = 0;
while($cuke < $denne_uke) {
	$cuke = date("oW",strtotime(substr($cuke,0,4)."W".substr($cuke,4,2)." +7 days"));
	$cnt++;
	if($cnt == 6) {
		$cnt = 0;
	}
	if($cnt == 4 or $cnt == 5) {
		$cstatus = false;
	} else {
		$cstatus = true;
	}
}
$offshore_rotasjon = $cstatus;*/
// END OF SANDER'S OFFSHORE ROTASJON - OUT: $offshore_rotasjon

// MADS SKAL HA HAGE I SOMMERMÅNEDENE
$hagearbeid_sommer = false;
if(date("m") >= 5 && date("m") <= 9) {
	$hagearbeid_sommer = 3; // MADS SIN PID
} 
// END OF MADS SKAL HA HAGE I SOMMERMÅNEDENE

/***
* $tasks
* array
*
* Contents:
*	array[]
*		[0] Oppgave
*		[1] Kan utgå;?
*		[2] Fast oppgave for en person => PERSONID (PID), ellers false - NB! KAN IKKE UTGÅ DA
*/
$tasks = array(
	array("Gulv",false,false),
	array("Kj&oslash;kken",false,false),
	array("Bad",false,false),
	array("St&oslash;vt&oslash;rk, rydding, vaskerom og toalett",true,false),
	array("Uteomr&aring;de",false,$hagearbeid_sommer)
);
$persons = array(
	// array([0] >> NAVN (string), [1] DELTAR DENNE UKEN? (bool), [2] PASSORD (samme som alarm untatt p� id[0]-N.O.)
	0 => array("Nikolai Ommundsen",true,7223),
	1 => array("Sascha Scott Fredriksen",true,8040),
	2 => array("Sander M&aelig;hlum",true,8255),
	3 => array("Mads Emil Kj&aelig;rnes",true,9050),
	4 => array("Harald Lundberg",true,5544)
);
$persons_full = $persons; // FULLSTENDIG UMODIFISERT
/* BEHANDLING AV PERSONER OG OPPGAVER */
// FJERN PERSONER SOM IKKE SKAL DELTA...
$persons_ny = array();
foreach($persons as $aid => $array) {
	if($array[1] === true) { // HVIS DELTAGENE
		$persons_ny[$aid] = $array;
	}
}
$persons = $persons_ny;
unset($persons_ny,$aid,$array);

$tasks_removed = 0; // TRACK NUMBER OF REMOVED TASKS
while(count($persons) < count($tasks)) { // Da m&aring; noen oppgaver utg&aring;...
	$tasks = removeTask($tasks);
}

function removeTask($taskArrayIn) { // FJERNER EN OPPGAVE SOM KAN UTG&aring;
	global $tasks_removed;
	$removed = false;
	$new = array();
	foreach($taskArrayIn as $array) {
		if($array[1] === true and $removed === false) {
			$tasks_removed++;
			$removed = true;
			// SKIP THIS TASK
		} else {
			$new[] = $array;
		}
	}
	return $new;
}
/* SLUTT P&aring; PERSON OG OPPGAVEBEHANDLING */

$laundry_rotation = array(
	// (int) DAY OF WEEK 1-7 => (int/false) $persons index responsible, false for none
	1 => 4,
	2 => 0,
	3 => 1,
	4 => 3,
	5 => 2,
	6 => false,
	7 => false
);

/*
*
* HVEM HAR VASKEDAG?
*
* OUT: $laundry_html
* CONTENT: Navn p&aring; den s&aring; har vaskedagen eller "Ingen - fritt for alle!".
*/
$freeforall = "Ingen - fritt for alle!";
$thisday = date("N");
$pid_today = $laundry_rotation[$thisday];
if($pid_today !== false and isset($persons[$pid_today])) {
	$laundry_html = $persons[$pid_today][0];
} else {
	$laundry_html = $freeforall;
}
unset($pid_today,$freeforall,$thisday);

/*
*
*
* TABELL OPPGAVEFORDELING
*
* UT: $tabell_html
*/

$done_file = "done.json"; // TASKS DONE?
$done = json_decode(file_get_contents($done_file),true);

$pos_file = file("pos.txt");
if(!count($pos_file) == 2) {
	die("Feil i fil pos.txt");
}
$pos_ukenr = trim($pos_file[0]);
$pos = $pos_file[1]-$tasks_removed;
if($pos < 0) $pos = count($tasks)-$tasks_removed; // Gå til andre del av skala

// NY UKE - NY POSISJON
if($pos_ukenr != $ukenr) {
	// ARCHIVE CURRENT FILE
	$archive_path = "archive/";
	$afile = "";
	foreach($done as $pid => $status) {
		$afile .= $persons_full[(int)$pid][0].": ";
		if($status) {
			$afile .= "Utført";
		} else {
			$afile .= "Ikke utført";
		}
		$afile .= "\r\n";
	}
	$afile .= "\r\nPOS: ".$pos;
	// FÅ MED TASK!!!!
	file_put_contents($archive_path.$pos_ukenr.".txt",$afile);
	unset($archive_path,$afile,$pid,$status);
	// RESET DONE FILE
	$done_a = array();
	foreach($persons as $pid => $person) {
		$done_a[(string)$pid.".0"] = false;
	}
	$done = $done_a;
	file_put_contents($done_file,json_encode($done_a)); // WRITE FILE
	// END RESET
	$pos++;
	if($pos == count($tasks)) {
		$pos = "0";
	}
	file_put_contents("pos.txt",$ukenr."\r\n".$pos);
}

$posx = $pos;

$tabell_html = "";

// OPPGAVEUTDELING
$ptask = array();
// FASTE OPPGAVER
foreach($tasks as $taskid => $taska) {
	if($taska[2] !== false) {
		$ptask[$taska[2]] = $tasks[$taskid][0]. "*"; // Get person with that PID and assign task
		unset($tasks[$taskid]);
	}
}
// FORDEL OPPGAVER
$pretask = count($ptask); // Number of preassigned tasks
foreach($persons as $pid => $person) {
	if(!isset($ptask[$pid])) { // CHECK IF PERSON DOES HAVE AN ASSIGNED TASK
		if($posx > count($tasks)-count($pretask)) {
			$posx = 0;
		}
		/*echo "<pre>";
		print_r($ptask);
		print("\r\n\$pid = ".$pid);
		print_r($tasks);
		print("\r\n\$posx = ".$posx);
		print("\r\n".count($tasks)."-".count($pretask)."=". count($tasks)-count($pretask));
		echo "</pre>";*/
		$ptask[$pid] = $tasks[$posx][0];
		$posx++;
	}
}
// END OF OPPGAVEUTDELING -> OUTPUT: $ptask

foreach($persons as $pid => $person) {
	$tabell_html .= "<tr><th class='pid'>".$pid."</th><th>&nbsp;".$person[0]."</th><td>";
	$tabell_html .= $ptask[$pid];
	$tabell_html .= "</td><td>";
	if($done[(string)$pid.".0"] === true) {
		$tabell_html .= "<div class='taskDone'><input type='checkbox' checked='checked' id='taskDone".$pid."' /><label for='taskDone".$pid."'></label></div>";
	} else {
		$tabell_html .= "<div class='taskDone'><input type='checkbox' id='taskDone".$pid."' /><label for='taskDone".$pid."'></label></div>";
	}
	$tabell_html .= "</td></tr>\r\n";
}
unset($pos,$pos_ukenr,$pos_file,$posx,$pid,$done_a);

$pages = array(); // ID => array(DESC,CSS-BGCOLOR-CLASS);

/*

AJAX FUNCTIONS

*/
if(isset($_GET['function'])) {
	switch(addslashes($_GET['function'])) {
		case "getContact":
			if(isset($_GET['term'])) {
				$term = htmlspecialchars(addslashes($_GET['term']));
				if(isset($cl[$term])) {
					echo json_encode($cl[$term]);
					exit;
				}
			}
			unset($term);
			break;
		case "regHighscore":
			if(isset($_GET['tlf']) and isset($_GET['score'])) {
				$tlf = htmlspecialchars(addslashes($_GET['tlf']));
				$score = htmlspecialchars(addslashes($_GET['score']));
				if(isset($cl[$tlf])) {
					if(!(isset($snhs[(string)$score]) and $score == min(array_keys($snhs)) and count($snhs) == $snake_highscore_limit)) {
						while(isset($snhs[(string)$score])) {
							$score += 0.1;
						}
						$snhs[(string)$score] = $tlf;
						krsort($snhs);
						while(count($snhs) > $snake_highscore_limit) {
							array_pop($snhs);
						}
						file_put_contents($snake_highscores_save,json_encode($snhs));
						echo json_encode("REG:OK");
						exit;
					}
				}
			}
			unset($tlf,$score);
			break;
		case "setTaskDone":
			if(isset($_GET['pid']) and isset($_GET['pwd'])) {
				$pid = htmlspecialchars(addslashes($_GET['pid']));
				$pwd = htmlspecialchars(addslashes($_GET['pwd']));
				$pers = $persons[$pid];
				if($pers[2] == trim($pwd)) { // IF PWD == OK
					$regok = "";
					if($done[(string)($pid).".0"] == true) { // TOGGLE STATUS
						$done[(string)($pid).".0"] = false;
						$regok = "REG:OK1";
					} else {
						$done[(string)($pid).".0"] = true;
						$regok = "REG:OK2";
					}
					file_put_contents($done_file,json_encode($done)); // WRITE FILE
					echo json_encode($regok);
					exit;
				}
			}
			unset($pers,$pid,$pwd);
			break;
		default:break;
	}
	die(json_encode(array()));
}

?>
<!html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title><?php echo $info['system_title']." - ".$info['place']; ?></title>
<link href='http://fonts.googleapis.com/css?family=Pacifico' rel='stylesheet' type='text/css'>
<link href='http://fonts.googleapis.com/css?family=Roboto:100,300,500,700,400&subset=latin,latin-ext' rel='stylesheet' type='text/css'>
<link rel="stylesheet" href="_data/style.css" media="all" type="text/css" />
<link rel="stylesheet" href="_data/1140px.css" media="all" type="text/css" />
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" media="all" type="text/css" />
<script src="_data/jquery-2.0.3.min.js"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<script src="_data/ion-sound/ion.sound.min.js"></script>
<!--RADIO--><!--<script src="_data/jQuery.jPlayer.2.5.0/jquery.jplayer.min.js"></script>
<script src="_data/nettradio.js"></script>-->
<link href='_data/fullcalendar/fullcalendar.css' rel='stylesheet' />
<link href='_data/fullcalendar/fullcalendar.print.css' rel='stylesheet' media='print' />
<script src='_data/fullcalendar/fullcalendar.min.js'></script>
<script src="_data/fullcalendar/gcal.js"></script>
<script type="text/javascript">
/* JQUERY SMOOTH ANCHOR SCROLL PLUGIN */
/* http://css-tricks.com/snippets/jquery/smooth-scrolling/ */
$(document).ready(function() {
  function filterPath(string) {
  return string
    .replace(/^\//,'')
    .replace(/(index|default).[a-zA-Z]{3,4}$/,'')
    .replace(/\/$/,'');
  }
  var locationPath = filterPath(location.pathname);
  var scrollElem = scrollableElement('html', 'body');

  $('a[href*=#]').each(function() {
    var thisPath = filterPath(this.pathname) || locationPath;
    if (  locationPath == thisPath
    && (location.hostname == this.hostname || !this.hostname)
    && this.hash.replace(/#/,'') ) {
      var $target = $(this.hash), target = this.hash;
      if (target) {
        var targetOffset = $target.offset().top;
        $(this).click(function(event) {
          event.preventDefault();
          $(scrollElem).animate({scrollTop: targetOffset}, 400, function() {
            location.hash = target;
          });
        });
      }
    }
  });

  // use the first element that is "scrollable"
  function scrollableElement(els) {
    for (var i = 0, argLength = arguments.length; i <argLength; i++) {
      var el = arguments[i],
          $scrollElement = $(el);
      if ($scrollElement.scrollTop()> 0) {
        return el;
      } else {
        $scrollElement.scrollTop(1);
        var isScrollable = $scrollElement.scrollTop()> 0;
        $scrollElement.scrollTop(0);
        if (isScrollable) {
          return el;
        }
      }
    }
    return [];
  }
});
function getKey(event) { // GET KEY OUTPUT (string) FROM EVENT
	var keyCode = event.which;
	var shift = event.shiftKey;
	/* 
	this function converts a keycode to a correct character. 
	the function can deal with shiftkeys aswell, but caps-lock state will be ignored, since the only way (I know of) to check caps-lock state is in a keypress event, and I write this for use in keydown/keyup events.
	*/      
	var myChar;
	myChar = shift ? shiftChars[keyCode] : myChar = chars[keyCode];
	myChar= (typeof myChar== 'undefined') ? '' : myChar;
	return myChar;
}
      
var chars = new Array;
	chars[13]='ENTER'
	chars[32]='SPACE';
	chars[37]='LEFT';
	chars[38]='UP';
	chars[39]='RIGHT';
	chars[40]='DOWN';
	chars[48]='0';
	chars[49]='1';
	chars[50]='2';
	chars[51]='3';
	chars[52]='4';
	chars[53]='5';
	chars[54]='6';
	chars[55]='7';
	chars[56]='8';
	chars[57]='9';
	chars[65]='a';
	chars[66]='b';
	chars[67]='c';
	chars[68]='d';
	chars[69]='e';
	chars[70]='f';
	chars[71]='g';
	chars[72]='h';
	chars[73]='i';
	chars[74]='j';
	chars[75]='k';
	chars[76]='l';
	chars[77]='m';
	chars[78]='n';
	chars[79]='o';
	chars[80]='p';
	chars[81]='q';
	chars[82]='r';
	chars[83]='s';
	chars[84]='t';
	chars[85]='u';
	chars[86]='v';
	chars[87]='w';
	chars[88]='x';
	chars[89]='y';
	chars[90]='z';
	chars[96]='0';
	chars[97]='1';
	chars[98]='2';
	chars[99]='3';
	chars[100]='4';
	chars[101]='5';
	chars[102]='6';
	chars[103]='7';
	chars[104]='8';
	chars[105]='9';
	chars[106]='*';
	chars[107]='+';
	chars[109]='-';
	chars[110]=',';
	chars[111]='/';
	chars[186]=';';
	chars[187]='=';
	chars[188]=',';
	chars[189]='-';
	chars[190]='.';
	chars[191]='/';
	chars[192]='`';
	chars[219]='[';
	chars[220]='\\';
	chars[221]=']';
	chars[222]='\'';

var shiftChars = new Array;
	shiftChars[32]=' ';
	shiftChars[48]=')';
	shiftChars[49]='!';
	shiftChars[50]='@';
	shiftChars[51]='#';
	shiftChars[52]='$';
	shiftChars[53]='%';
	shiftChars[54]='^';
	shiftChars[55]='&';
	shiftChars[56]='*';
	shiftChars[57]='(';
	shiftChars[65]='A';
	shiftChars[66]='B';
	shiftChars[67]='C';
	shiftChars[68]='D';
	shiftChars[69]='E';
	shiftChars[70]='F';
	shiftChars[71]='G';
	shiftChars[72]='H';
	shiftChars[73]='I';
	shiftChars[74]='J';
	shiftChars[75]='K';
	shiftChars[76]='L';
	shiftChars[77]='M';
	shiftChars[78]='N';
	shiftChars[79]='O';
	shiftChars[80]='P';
	shiftChars[81]='Q';
	shiftChars[82]='R';
	shiftChars[83]='S';
	shiftChars[84]='T';
	shiftChars[85]='U';
	shiftChars[86]='V';
	shiftChars[87]='W';
	shiftChars[88]='X';
	shiftChars[89]='Y';
	shiftChars[90]='Z';
	shiftChars[96]='0';
	shiftChars[97]='1';
	shiftChars[98]='2';
	shiftChars[99]='3';
	shiftChars[100]='4';
	shiftChars[101]='5';
	shiftChars[102]='6';
	shiftChars[103]='7';
	shiftChars[104]='8';
	shiftChars[105]='9';
	shiftChars[106]='*';
	shiftChars[107]='+';
	shiftChars[109]='-';
	shiftChars[110]=',';
	shiftChars[111]='/';
	shiftChars[186]=':';
	shiftChars[187]='+';
	shiftChars[188]='<';
	shiftChars[189]='_';
	shiftChars[190]='>';
	shiftChars[191]='?';
	shiftChars[192]='~';
	shiftChars[219]='{';
	shiftChars[220]='|';
	shiftChars[221]='}';
	shiftChars[222]='"';
	
var activePage = 1; // STARTPAGE
function gotoPage(num) {
	$("#scroll-page" + num).trigger("click");
	activePage = num;
	if(activePage == 4) {
		$("#cal_month").fadeOut(300);
		if(snakeRunning == false) {
			startSnake();
			$("#sidebar").animate({
				marginLeft:-120
			},250);
		}
	} else if(activePage == 5) {
		$("#cal_month").html($(".fc-header-title").find("h2").html());
		$("#cal_month").fadeIn(300);
	} else {
		$("#cal_month").fadeOut(300);
		$("#sidebar").animate({
			marginLeft:0
		},250);
	}
}

// ADD LEADING ZERO UNDER 10
function checkTime(i) {
	if (i<10)
	{
		i="0" + i;
	}
	return i + '';
}
// CLOCK
var t;
function updateFancy(jqe,to,dst) {
	if(jqe.html() == to) return; // NO UPDATE
	jqe.animate({
		opacity:0,
		marginTop:-100
	},400,function() {
		jqe.html(to);
		jqe.css("margin-top","100px");
		jqe.animate({
			opacity:1,
			marginTop:0
		},400);
	});
}
function startTime(firstRun) {
	var today=new Date();
	var h=checkTime(today.getHours());
	var m=checkTime(today.getMinutes());
	updateFancy($("#c-hours-d1"),h.substring(0,1));
	updateFancy($("#c-hours-d2"),h.substring(1,2));
	updateFancy($("#c-minutes-d1"),m.substring(0,1));
	updateFancy($("#c-minutes-d2"),m.substring(1,2));
	if((h == "00" && m == "00") || firstRun == true) {
		var mo = checkTime(today.getMonth()+1);
		var yr = today.getFullYear();
		var dy = checkTime(today.getDate());
		updateFancy($("#date"),dy + "." + mo + "-" + yr);
	}
	t=setTimeout(function(){startTime(false)},5000);
}
var snakeKeyFunction = function(event) { return true; };
var keydownEvent = false;
$(function() {
	startTime(true);
	$(document).keydown(function(event) {
		keydownEvent = event;
	});
});
function execKey() {
	if(keydownEvent == false) {
		return false;
	}
	var event = keydownEvent;
	keydownEvent = false;
	key = getKey(event);
	//$(".header").html(new Date().getTime() + ": " + key);
	if(activePage == 1) {
		if(!page1KeyFunction(event)) { // If it returns false -> Don't allow navigation
			return;
		}
	}
	if(activePage == 2) { // STRAFFEPLAN
		if(key == "0") {
			// HVILKEN OPPGAVE BLIR VALGT? :-O ^^
			selected_task = Math.floor(Math.random()*$("#straffeoppgaver_list ul li").length);
			$("#straffeoppgaver_list ul li").animate({
				opacity:0.3
			},500);
			$("#straffeoppgaver_list>ul>li:eq(" + selected_task + ")").delay(300).animate({
				opacity:1
			},500);
		} else if(key == "+") {
			// VIS/SKJUL STRAFFEPLAN
			if($(".straffeplan").is(":hidden")) {
				$(".straffeoppgaver").fadeOut(300);
				$(".straffeplan").delay(300).fadeIn(300);
			} else {
				$(".straffeplan").fadeOut(300);
				$(".straffeoppgaver").delay(300).fadeIn(300);
			}
		}
	} else {
		// RESET OPACITY MED OPPGAVEVELGER
		$("#straffeoppgaver_list ul li").css("opacity",1);
	}
	if(activePage == 4) {
		if(!snakeKeyFunction(event)) { // If it returns false -> Don't allow navigation
			return;
		}
	}
	if(activePage == 5) {
		if(key == "LEFT") $(".fc-button-prev").trigger("click");
		if(key == "RIGHT") $(".fc-button-next").trigger("click");
		if(key == "UP") $(".fc-button-today").trigger("click");
		$("#cal_month").html($(".fc-header-title").find("h2").html());
	}
	if(key == "1") { // NUMPAD 1
		gotoPage(1);
	} else if(key == "2") {
		gotoPage(2);
	} else if(key == "3") {
		gotoPage(3);
	} else if(key == "4") {
		gotoPage(4);
	} else if(key == "5") {
		gotoPage(5);
	}
	event.preventDefault();
}
// REFRESH ON INACTIVITY
(function(seconds) {
    var refresh,       
        intvrefresh = function() {
            clearInterval(refresh);
            refresh = setTimeout(function() {
				if(!radio_playing) { // Don't refresh on radio play
					location.href = location.protocol+'//'+location.host+location.pathname;
				}
			}, seconds * 1000);
        };

    $(document).on('keypress, click', function() { intvrefresh() });
    intvrefresh();
}(600)); // define seconds here
</script>
</head>
<body>
<div style="display:none;" id="scroll_helpers">
	<a href="#page1" id="scroll-page1">1</a>
	<a href="#page2" id="scroll-page2">2</a>
	<a href="#page3" id="scroll-page3x">3</a>
	<a href="#page4" id="scroll-page4">4</a>
	<a href="#page5" id="scroll-page5">5</a>
</div>
<div id="page1" class="page bg-darkgrey">
	<?php $pages[1] = array("Oversikt","bg-darkgrey"); ?>
	<script>
	var pidSelect = false;
	var radio_playing = false;
	page1KeyFunction = function(event) {
		var key = getKey(event);
		if(key == "+") {
			$(".pid").fadeIn(300);
			pidSelect = true;
		}
		if(key == "-") {
			$(".pid").fadeOut(300);
			pidSelect = false;
		}
		/*//RADIO//
		if(key == "SPACE") {
			if(radio_playing) {
				jQuery("#radio-player").jPlayer("pause");
				$("#radio > #control > .pause").fadeIn(300);
				$("#radio > #control > .play").fadeOut(300);
				radio_playing = false;
			} else {
				$("#radio > #control > .pause").fadeOut(300);
				$("#radio > #control > .play").fadeIn(300);
				jQuery("#radio-player").jPlayer("play");
				radio_playing = true;
			}
		}*/
		if(pidSelect == false) {
			return true;
		} else {
			// NUMBER SELECTION
			if(key >= 0 && key <= 9) {
				var pwd = prompt("You need to enter your password to continue");
				$.getJSON( "?function=setTaskDone&pid=" + key + "&pwd=" + pwd, function( data ) {
					if(data != "") {
						if(data == "REG:OK1") {
							$("#taskDone" + key).prop('checked',false);
						} else if(data == "REG:OK2") {
							$("#taskDone" + key).prop('checked',true);
						}
					} else {
						alert("Feil passord eller feil oppgave. NB! Passordene er unike for hver person");
					}
					pidSelect = false;
					$(".pid").delay(400).fadeOut(300);
				});
			} else {
				event.preventDefault();
			}
		}
	}
	$(function() {
		$(".pid").hide();
		$(".taskDone").click(function(event) {
			alert("Vennligst bruk nummertastaturet til navigasjon");
			event.preventDefault(); // NO CLICKING ON THE CHECKBOXES
		});
	});
	</script>
	<a name="page1"></a>
	<div class='container12'>
		<div class='row'>
			<div class="column12 header">
				<span>Oversikt</span>
			</div>
		</div>
		<div class='row'>
			<div class='spacer column12'>&nbsp;</div>
		</div>
		<div class='row'>
			<div class='column7'>
				<h2>Oppgavefordeling</h2>
				<table id="oppgavefordeling">
				<?php echo $tabell_html; ?>
				</table>
				<div style='font-size:9pt;margin-top:5px;'>Oppgave fullf&oslash;rt? Klikk '+' og deretter tallet ditt (person-id). Klikk '-' for &aring; avbryte.<br />Husk &aring; registrere oppgaven som fullf&oslash;rt f&oslash;r slutten av uken!<br />* Faste oppgaver, for eksempel i en gitt sesong.</div>
			</div>
			<div class='column5' id="clock-holder">
				<div id="clock">
					<div id="c-hours">
						<div id='c-hours-d1'>0</div>
						<div id='c-hours-d2'>0</div>
					</div>
					<div id="c-sep">:</div>
					<div id="c-minutes">
						<div id='c-minutes-d1'>0</div>
						<div id='c-minutes-d2'>0</div>
					</div>
				</div>
				<div id="date">01.01-2014</div>
			</div>
		</div>
		<div class='row'>
			<div class='spacer column12'>&nbsp;</div>
		</div>
		<div class='row'>
			<div class='column7'>
				<h2>Dagen i dag</h2>
				<div>
				Vaskedag for: <?php echo $laundry_html; ?>
				<br /><br />
				<?php if(date("H") >= 21 || date("H") <= 1) echo '<h1 style="color:#E74C3C">Husk &aring; t&oslash;m bosset!</h1>'; ?>
				</div>
			</div>
			<div class='column5'>
				<h2>V&aelig;ret som er meldt i dag</h2>
				<style>
				/* ACCUWEATHER STYLING */
				.aw-toggle { display:none !important; }
				div.aw-widget-current-inner div.aw-widget-content {
					border:none !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather div.aw-current-weather-inner {
					margin:0 !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather h3 {
					display:none !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather p {
					padding:0 0 0 5% !important;
					width:65% !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather p span.aw-weather-description {
					font-weight:100 !important;
					font-size:14pt !important;
					font-family: 'Roboto', sans-serif !important;
					margin-top: 20px !important;
					text-align:left !important;
					color:white !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather p span.aw-temperature-today {
					text-align:left !important;
					color:white !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather p span.aw-temperature-today b {
					font-weight:100 !important;
					font-size:48pt !important;
					font-family: 'Roboto', sans-serif !important;
					padding-left:0 !important;
					color:white !important;
				}
				div.aw-widget-current-inner div.aw-widget-content a.aw-current-weather p time {
					display:none !important;
				}
				</style>
				<script>
				$(function() {
					st = setTimeout(function() { $(".aw-widget-content")[0].className = $('.aw-widget-content')[0].className.replace(/\bbg.*?\b/g, ''); },1500);
				});
				</script>
				<a href="http://www.accuweather.com/no/no/stavanger/260665/weather-forecast/260665" class="aw-widget-legal"></a><div id="awcc1389652057921" class="aw-widget-current"  data-locationkey="260665" data-unit="c" data-language="no" data-useip="false" data-uid="awcc1389652057921"></div><script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
			</div>
		</div>
		<?php /* <!-- RADIO --><div class='row'>
			<div class='column7'>
				<div id="radio">
					<div id="radio-player" class="jp-jplayer" style="width: 0px; height: 0px;">
						<img id="jp_poster_0" style="width: 0px; height: 0px; display: none;">
						<audio id="jp_audio_0" preload="metadata" src=""></audio>
					</div>
					<div class="channel">P-</div>
					<div id="control">
						<div class='base'></div>
						<div class='pause'></div>
						<div class='play' style="display:none;"></div>
					</div>
					<div class="info">Klikk mellomrom for &aring; starte eller stoppe radio</div>
				</div>
			</div>
			<div class='column5'>&nbsp;</div>
		</div> */ ?>
	</div>
</div>
<div id="page2" class="page bg-cyan">
	<?php $pages[2] = array("Straffeplan","bg-cyan"); ?>
	<a name="page2"></a>
	<div class='container12'>
		<div class='row'>
			<div class="column12 header">
				<span>Straffeplan</span>
			</div>
		</div>
		<div class='row'>
			<div class='spacer column12'>&nbsp;</div>
		</div>
		<div class='row'>
			<div class="column7">
				<div class="numpadRef straffeoppgaver">
					0&nbsp;
					Velg en straffeoppgave for den skyldige
				</div>
				&nbsp;
			</div>
			<div class="column5">
				<div class='numpadRef'>+&nbsp;
					<span class="straffeoppgaver">Vis straffeplan</span>
					<span class="straffeplan">Vis&nbsp;straffeoppgaver</span>
				</div>
			</div>
		</div>
		<div class='row'>
			<div class='column12 straffeoppgaver' id="straffeoppgaver_list">
				<ul>
					<li>Pante flasker</li>
					<li>Rydde uteomr&aring;de (hage, inngang, trapp og parkering)</li>
					<li>H&oslash;ttrykkspyle terrassen (sommer)</li>
					<li>St&oslash;vsuge kjelleren</li>
					<li>Luke ugress (sommer)</li>
					<li>Rense filter p&aring; oppvaskmaskin<br>og t&oslash;mming av alle sluker.</li>
				<ul>
			</div>
			<div class='column12 straffeplan' id="straffeplan_table">
				<table>
					<thead>
						<tr>
							<th>Brudd&nbsp;p&aring;&nbsp;regel</th>
							<th>Straff</th>
						</tr>
					</thead>
					<tr>
						<th>Ikke&nbsp;deltatt&nbsp;i&nbsp;ryddesyklus</th>
						<td>Straffeoppgave. Oppgaven som ikke er utf&oslash;rt skal tas neste uke. </td>
					</tr>
					<tr>
						<th>&para;3</th>
						<td>Eldster&aring;det informeres + alle straffeoppgavene</td>
					</tr>
					<tr>
						<th>&para;4,&para;5</th>
						<td>Hvis det skjer gjentatte ganger velges en eller flere av straffeoppgavene. Personen skal informeres og det skal bli avgjort at straff er n&oslash;dvendig neste husm&oslash;te (eller kan tas med 2+ personer som er enig i straff). Eventuelt kan de bli enige om erstatning.</td>
					</tr>
					<tr>
						<th>&para;9</th>
						<td>Straffeoppgave - Gjelder kopiering til andre, da skal denne ogs&aring; kreves tilbake. Kan bli godkjent av de andre i huset (alle).</td>
					</tr>
					<tr>
						<th>&para;10, &para;10.1</th>
						<td>Straffeoppgave</td>
					</tr>
					<tr>
						<th>&para;11</th>
						<td>Straffeoppgave (kun ved kontinuitet)</td>
					</tr>
					<tr>
						<th>&para;12</th>
						<td>Blir du fersket for snusking s&aring; er du en drittsekk, og du fortjener alle straffeoppgavene.</td>
					</tr>
					<tr>
						<th>Innesperring av biler</th>
						<td>Alle n&oslash;kler/reserven&oslash;kler til parkerte biler (i g&aring;rdsplassen, i garasjen, eller foran g&aring;rdsplassen) skal ligge i vinduskarm med d&oslash;ren slik at andre kan flytte ut bilene ved behov. Hvis en bil blir innesperret er det opp til den som har parkert bilen &aring; fikse opp i det, hvis ikke n&oslash;kkelen er lagt igjen. Personen blir da ogs&aring; straffet med en straffeoppgave.</td>
					</tr>
					<tr>
						<th>Garasjen&oslash;kkel tas med</th>
						<td>Garasjen&oslash;kkelen skal alltid ligge i vinduskarmen med d&oslash;ren, hvis den tas med resulterer det i straffeoppgave.</td>
					</tr>
				</table>
			</div>
		</div>
	</div>
	<script>
	$(".straffeplan").hide();
	</script>
</div>
<div id="page3" class="page bg-darkblue">
	<?php $pages[3] = array("Ikke i bruk","bg-darkblue"); ?>
	<a name="page3"></a>
	<div class='container12'>
		<div class='row'>
			<div class="column12 header">
				<span>Din straff</span>
			</div>
		</div>
		<div class='row'>
			<div class='column12'>
				<table style='margin-top:50px'>
					<?php
					/*$inv = file("Gjesteliste.csv");
					unset($inv[0]);
					sort($inv);
					$gjest = array();
					$gcnt = 0;
					foreach($inv as $invite) {
						$invite = explode(",",$invite);
						if($gcnt == 0) {
							echo "<tr>";
						}
						echo "<td style='padding:10px !important;'>".$invite[0]."</td>";
						$gcnt++;
						if($gcnt == 5) {
							echo "</tr>";
							$gcnt = 0;
						}
					}
					unset($gcnt,$gjest,$inv,$invite);*/
					?>
				</table>
			</div>
		</div>
	</div>
</div>
<div id="page4" class="page bg-brown">
	<?php $pages[4] = array("Snake","bg-brown"); ?>
	<a name="page4"></a>
	<div class='container12'>
		<div class='row playsnake'>
			<div class="column12 header">
				<span>Snake</span>
			</div>
		</div>
		<div class='row playsnake'>
			<div class="column12 snakeNumpadRef">
				- Avbryt spill og returner til oversikt (side 1)
			</div>
		</div>
		<div id="snake_lost" class="playsnake">
			<div class='row'>
				<div class="column12">
					<div class='header' style="margin-bottom:60px;">
						<span>Game Over!</span>
					</div>
				</div>
			</div>
			<div class='row'>
				<div class="column12">
					<div class='header' style="margin-bottom:60px;">
						<span id="show_score">0 poeng</span>
					</div>
				</div>
			</div>
			<div class='row'>
				<div class="column12 snakeNumpadRef">
					Klikk 'ENTER' for &aring; starte p&aring; nytt!
				</div>
			</div>
		</div>
		<div class='row playsnake'>
			<div class='column12'>
				<div style="width:800px;height:500px;margin:20px auto 0 auto;">
					<canvas id="canvas" width="800" height="500"></canvas>
				</div>
			</div>
			<div id="snake_points" class="column12 snakeNumpadRef">Poengsum: 0</div>
			<!--<div id="snake_debug" class="column12 snakeNumpadRef">&nbsp;</div>-->
		</div>
		<div class='row playsnake'>
			<div id="snake_highscores" class="column12">
				<?php
				foreach($snhs as $score => $tlf) {
					echo "<div class='hs'><span>".round($score)."</span>".$cl[$tlf]."</div>";
				}
				?>
			</div>
		</div>
		<?php /* HIGHSCORE LIST */ ?>
		<div class='row sethighscore'>
			<div class="column12">
				<div class='header' style="margin-bottom:60px;">
					<span>Ny highscore i listen!</span>
				</div>
			</div>
		</div>
		<div class='row sethighscore'>
			<div class="column12">
				<div class='header' style="margin-bottom:60px;">
					<span id="show_highscore">0 poeng</span>
				</div>
			</div>
		</div>
		<div class='row sethighscore'>
			<div class='column12' style="text-align:center;"><span class='snakeNumpadRef'>Skriv inn ditt mobiltelefonnummer: </span>(Klikk '.' for &aring; viske ut, 'Enter' for &aring; s&oslash;ke etter navn)</div>
		</div>
		<div class='row sethighscore'>
			<div class='column12'>
				<div class="ui-widget" id="tlfnr_picker_holder">
					<input id="tlfnr_picker">
				</div>
				<div class="loader">
					<div class="bar"></div>
					<div class="bar"></div>
					<div class="bar"></div>
				</div>
			</div>
		</div>
		<form name="highscore_form" id="highscore_form">
			<input type="hidden" name="tlfnr" id="tlfnr" value="">
			<input type="hidden" name="score" id="score" value="0">
		</form>
		<div class='row sethighscore'>
			<div class='column12'>
				<div id="contact_result">&nbsp;</div>
			</div>
		</div>
		<div class='row sethighscore'>
			<div class='column12 highscore_reg' id="highscore_reg"><span class='snakeNumpadRef'>Klikk '+' for &aring; registrere resultatet i highscore listen</span></div>
		</div>
		<div class='row sethighscore'>
			<div class='spacer column12'>&nbsp;</div>
		</div>
		<div class='row sethighscore highscore_reg'>
			<div class='column12'><span class='snakeNumpadRef'>Klikk '-' for &aring; avbryte registrering (NB! Resultatet blir slettet)</span></div>
		</div>
	</div>
<script>
/* SNAKE CODE */
<?php
// PARSE CSV
/*$f = file("contacts.csv");echo "</script>";
$out = array();
foreach($f as $line) {
	$line = explode(";",$line);
	$a = $line[0];
	$b = $line[1];
	$c = $line[2];
	if(strlen($b) > 0) $a = $a." ".$b;
	$a = $a." ".$c;
	$num = trim($line[3]);
	$num = str_replace("+47","",$num);
	if(strlen($num) == 10 and substr($num,0,2) == "47") $num = substr($num,2);
	$out[$num] = utf8_encode(trim($a));
}
file_put_contents("contacts.json",json_encode($out));echo "<script>";*/

$lowest_snhs = min(array_keys($snhs));
$highest_snhs = max(array_keys($snhs));
echo "var highest_snhs = ".$highest_snhs.";\r\n";
echo "var lowest_snhs = ".$lowest_snhs.";\r\n";
?>

function regHighscore() {
	snakeKeyFunction = function() {
		return true;
	}
	$.getJSON( "?function=regHighscore&tlf=" + $("#tlfnr").val() + "&score=" + $("#score").val(), function( data ) {
		$("#score").val(score);
		$("#tlfnr").val();
		$("#contact_result").fadeOut(50,function() {
			$(this).html("");
		});
		$("#highscore_reg").fadeOut(50);
		if(data != "") {
			$("#tlfnr_picker").val("");
			location.href = location.protocol+'//'+location.host+location.pathname; // reload
			//gotoPage(4);
		} else {
			alert("Beklager! Noe gikk galt en plass :/ Jeg klarer ikke fikse registreringen av highscoren...");
			gotoPage(1);
		}
	});
}

$(function() {
	var highscore_OK = false;
	$(".loader").hide();
	$("#contact_result").hide();
	$("#highscore_reg").hide();
	$(".sethighscore").hide(); // HIDE HIGHSCORE REG.
	$("#tlfnr_picker").change(function(){
		var tlfnr = $(this).val();
		$("#contact_result").fadeOut(250, function() {
			//$(".loader").fadeIn(50);
			$.getJSON( "?function=getContact&term=" + tlfnr, function( data ) {
				if(data != "") {
					$("#contact_result").html("<span class='hit'>" + data + "</span>");
					$("#tlfnr").val(tlfnr);
					highscore_OK = true;
					$("#contact_result").delay(200).fadeIn(250,function() {
						$("#highscore_reg").fadeIn(250);
					});
					snakeKeyFunction = function(event) {
						var key = getKey(event);
						if(key == "-") {
							abortSetHighscore();
							event.preventDefault();
							return false;
						} else if(key == "+") { 
							regHighscore();
							event.preventDefault();
							return false;
						} else if(key == ".") {
							$("#tlfnr_picker").val("").focus();
							event.preventDefault();
						}
						return false; // STRAIGHT THROUGH
					}
				} else {
					snakeKeyFunction = function(event) {
						var key = getKey(event);
						if(key == "-") {
							abortSetHighscore();
							event.preventDefault();
						} else if(key == ".") { // .
							$("#tlfnr_picker").val("").focus();
							event.preventDefault();
						}
						return false; // STRAIGHT THROUGH
					}
					$("#contact_result").html("<span class='no-hit'>Ingen treff!</span>");
					$("#tlfnr").val(0);
					$("#highscore_reg").fadeOut(250,function() {
						$("#contact_result").fadeIn(250);
					});
				}
				//$(".loader").fadeOut(50);
			}).fail(function( jqxhr, textStatus, error ) {
				var err = textStatus + ", " + error;
				console.log( "Request Failed: " + err );
			});
		});
	});
});

//Canvas stuff
var canvas = $("#canvas")[0];
var ctx = canvas.getContext("2d");
var w = $("#canvas").width();
var h = $("#canvas").height();

//Lets save the cell width in a variable for easy control
var cw = 20;
var d;
var food = {x:-1,y:-1};
var poison = {x:-2,y:-2};
var poisonChangedPosOn;
var score;
var startSpeed = 60;
var speed = startSpeed;
var maxSpeed = 30;
var game_loop;
// BONUS
var bonus = {x:-3,y:-3};
var bonusTime;
var bonusChangedPosOn;
// KEYEVENTS
var key_loop;
key_loop = setInterval(execKey, speed); // START KEYLOOP

//Lets create the snake now
var snake_array; //an array of cells to make up the snake
var snakeRunning = false;
var abort_snake = false;

function setHighscore() {
	$("#snake_lost").hide(100);
	abortSnake();
	snakeKeyFunction = function(event) {
		var key = getKey(event);
		if(key == "-") {
			abortSetHighscore();
		} else if(key == "." || key == ",") {
			$("#tlfnr_picker").val("").focus();
			event.preventDefault();
		}
		return false; // STRAIGHT THROUGH
	}
	$(".playsnake").fadeOut(500,function() {
		$(".sethighscore").fadeIn(500, function() {
			$("#tlfnr_picker").focus();
		});
	});
}
function abortSetHighscore() {
	snakeKeyFunction = function(event) {
		return false;
	}
	$(".sethighscore").fadeOut(500,function() {
		gotoPage(4);
	});
}

function abortSnake() {
	// KILL SOUND PLUGIN
	$.ionSound.destroy();
	// RESTORE KEYS
	snakeKeyFunction = function(event) {
		var key = getKey(event);
		if(key == 'ENTER') {
			gotoPage(4); // START SNAKE
		}
		if(key == "-") {
			gotoPage(1); // RETURN TO START PAGE
		}
		return false;
	}
	// ABORT SNAKE
	abort_snake = true;
	clearInterval(game_loop);
	game_loop = false;
	snakeRunning = false;
	$("#snake_points").fadeOut(500);
	$("#canvas").fadeOut(500);
	$("#snake_lost").fadeIn(500);
}
function setPoints(score) {
	$("#snake_points").fadeIn(500);
	$("#show_highscore").html("Du fikk " + score + " poeng!");
	$("#show_score").html("Du fikk " + score + " poeng!");
	$("#score").val(score);
	$("#snake_points").html("Poengsum: " + score);
}

function startSnake()
{
	// SOUND PLUGIN START
	$.ionSound({
		sounds: [                       // set needed sounds names
			"light_bulb_breaking",
			"bell_ring"
		],
		path: "_data/ion-sound/sounds/",// set path to sounds
		multiPlay: true,
		volume: "0.8"                   // not so loud please
	});
	$(".playsnake").show();
	$(".sethighscore").hide();
	$("#snake_lost").hide();
	// SET VARIABLES
	poisonChangedPosOn = 0;
	bonusChangedPosOn = 0;
	bonusTime = 0;
	//foodColor = 2; // START FOOD COLOR (at least 2)
	// KEY SETUP
	snakeKeyFunction = function(event) {
		if(allow_keypress == false) {
			event.which = 0;
			return false; // Don't allow other keys
		}
		var key = getKey(event);
		allow_keypress = false;
		//We will add another clause to prevent reverse gear
		if((key == "LEFT" || key == "4") && d != "right") d = "left";
		else if((key == "UP" || key == "8") && d != "down") d = "up";
		else if((key == "DOWN" || key == "2" || key == "5") && d != "up") d = "down";
		else if((key == "RIGHT" || key == "6") && d != "left") d = "right";
		else if(key == "-") {
			abortSnake();
			gotoPage(1); // RETURN TO START PAGE
		}
		event.which = 0;
		return false; // Don't allow other keys
	}
	$("#canvas").fadeIn(100);
	snakeRunning = true;
	speed = startSpeed
	d = "right"; //default direction
	create_snake();
	create_food(); //Now we can see the food particle
	create_poison(); //Create poison particle. This must be after food
	//finally lets set the score
	score = 0;
	
	//Lets move the snake now using a timer which will trigger the paint function
	//every 60ms
	painter(false);
}

function create_snake()
{
	var length = 5; //Length of the snake
	snake_array = []; //Empty array to start with
	for(var i = length-1; i>=0; i--)
	{
		//This will create a horizontal snake starting from the top left
		snake_array.push({x: i, y:0});
	}
}

//Lets create the food now
function create_food()
{
	food = {
		x: Math.round(Math.random()*(w-cw)/cw), 
		y: Math.round(Math.random()*(h-cw)/cw), 
	};
	//This will create a cell with x/y between 0-44
	//Because there are 45(450/10) positions accross the rows and columns
}
//Lets create the poison
function create_poison()
{
	poison = {
		x: Math.round(Math.random()*(w-cw)/cw), 
		y: Math.round(Math.random()*(h-cw)/cw), 
	};
	if(p_overlap()) { // Don't allow food/poison overlap
		create_poison(); // Redo
		return;
	}
	var p = snake_array[0]; // Don't allow the poison to spawn close to the snake! (min 5 blocks)
	var xDiff = p.x-poison.x;
	var yDiff = p.y-poison.y;
	if((yDiff > 5 || yDiff < -5) || (xDiff > 5 || xDiff < -5)) { 
		poisonChangedPosOn = score; // Update when last position change occured
	} else {
		create_poison(); // REDO
	}
	//This will create a cell with x/y between 0-44
	//Because there are 45(450/10) positions accross the rows and columns
}
//Lets create the bonus
function create_bonus(vsec) // visible seconds
{
	if(bonusTime > 0) return false;
	bonus = {
		x: Math.round(Math.random()*(w-cw)/cw), 
		y: Math.round(Math.random()*(h-cw)/cw), 
	};
	if(p_overlap()) { // Don't allow food/poison overlap
		create_bonus(); // Redo
		return;
	}
	
	var p = snake_array[0]; // Don't allow the poison to spawn close to the snake! (min 5 blocks)
	var xDiff = p.x-bonus.x;
	var yDiff = p.y-bonus.y;
	if((yDiff > 5 || yDiff < -5) || (xDiff > 5 || xDiff < -5)) { 
		bonusTime = vsec;
		bonusChangedPosOn = score; // Update when last position change occured
	} else {
		create_bonus(); // REDO
	}
	//This will create a cell with x/y between 0-44
	//Because there are 45(450/10) positions accross the rows and columns
}
// OVERLAP between food, bonus and poison?
function p_overlap() {
	if(food.x == poison.x || food.x == bonus.x || bonus.x == poison.x || food.y == poison.y || food.y == bonus.y || bonus.y == poison.y) {
		return true;
	} else {
		return false;
	}
}

var allow_keypress = true;

//Lets paint the snake now
function paint()
{
	if(snakeRunning == false) return;
	//To avoid the snake trail we need to paint the BG on every frame
	//Lets paint the canvas now
	ctx.fillStyle = "#ffffff";
	ctx.fillRect(0, 0, w, h);
	ctx.strokeStyle = "#333333";
	ctx.strokeRect(0, 0, w, h);
	
	//The movement code for the snake to come here.
	//The logic is simple
	//Pop out the tail cell and place it infront of the head cell
	var nx = snake_array[0].x;
	var ny = snake_array[0].y;

	//These were the position of the head cell.
	//We will increment it to get the new head position
	//Lets add proper direction based movement now
	if(d == "right") nx++;
	else if(d == "left") nx--;
	else if(d == "up") ny--;
	else if(d == "down") ny++;
	
	allow_keypress = true;
	
	// Does the snake eat the poison?
	var poison_eaten = false;
	if(nx == poison.x && ny == poison.y) {
		poison_eaten = true;
	}
	// Does the snake get the bonus? (+5 points without extra length)
	if(nx == bonus.x && ny == bonus.y) {
		bonus = {x:-3,y:-3}; // To remove it, not just hide it
		score += 5;
		bonusTime = 0;
	}
	
	if((score % 10) == 0 && poisonChangedPosOn != score) { // Move poison every 5 points
		create_poison();
	}
	if((score % 30) == 0 && bonusChangedPosOn != score) { // Create bonus every 30 points
		create_bonus(3); // Last for 3 secs
	}
	
	//Lets add the game over clauses now
	//This will restart the game if the snake hits the wall
	//Lets add the code for body collision
	//Now if the head of the snake bumps into its body, the game will restart
	//Also triggers when poison is eaten
	if(nx == -1 || nx == w/cw || ny == -1 || ny == h/cw || check_collision(nx, ny, snake_array) || poison_eaten)
	{
		$.ionSound.play("light_bulb_breaking");
		// highscore system
		if(score > lowest_snhs) {
			setHighscore(score);
			return;
		}
		//restart game
		//startSnake()
		abortSnake();
		//Lets organize the code a bit now.
		return;
	}
	
	//Lets write the code to make the snake eat the food
	//The logic is simple
	//If the new head position matches with that of the food,
	//Create a new head instead of moving the tail
	var food_eaten = false;
	if(nx == food.x && ny == food.y)
	{
		food_eaten = true;
		$.ionSound.play("bell_ring");
		var tail = {x: nx, y: ny};
		score++;
		//Create new food
		create_food();
	}
	else
	{
		var tail = snake_array.pop(); //pops out the last cell
		tail.x = nx; tail.y = ny;
	}
	//The snake can now eat the food.
	
	snake_array.unshift(tail); //puts back the tail as the first cell
	
	for(var i = 0; i < snake_array.length; i++)
	{
		var c = snake_array[i];
		//Lets paint 10px wide cells
		paint_cell(c.x, c.y, 'tail');
	}
	
	//Lets paint the food
	paint_cell(food.x, food.y, 'food');
	//Lets paint the poison
	paint_cell(poison.x, poison.y, 'poison');
	//Lets pain the bonus
	if(bonusTime > 0) {
		paint_cell(bonus.x, bonus.y, 'bonus');
		bonusTime -= speed/1000;
	}
	//Lets set the score
	setPoints(score);
	//var score_text = "Poengsum: " + score;
	//ctx.fillText(score_text, 5, h-5);
	
	// MAKE THE SNAKE GO FASTER FOR EACH FOOD :-D
	//$("#snake_debug").html("FPS: " + (1000/speed));
	if(food_eaten == true) {
		if(speed > maxSpeed) {
			foodColor += 1;
			speed -= 0.25;
			painter(false);
		}
	}
}
var painter_int;
function painter(cnd) {
	if(typeof key_loop != "undefined") clearInterval(key_loop);
	key_loop = setInterval(execKey, speed);
	painter_int = setInterval(paintSnakeLoop,5);
}
function paintSnakeLoop() {
	if(typeof game_loop != "undefined") clearInterval(game_loop);
	game_loop = setInterval(paint, Math.round(speed));
	clearInterval(painter_int);
}
//var foodColor = 2; // EXTRA FOOD COLOR ^^
//Lets first create a generic function to paint cells
function paint_cell(x, y, type)
{
	if(type == "tail") {
		ctx.fillStyle = "#588c79";
		ctx.strokeStyle = "#1e3859";
	} else if(type == "food") {
		/*var fcolor = (foodColor*500); // DYNAMIC FOOD COLOR
		if(fcolor > 9999 && fcolor < 11000) fcolor = "AAAA";
		if(fcolor > 10999 && fcolor < 12000) fcolor = "BBBB";
		if(fcolor > 11999 && fcolor < 13000) fcolor = "CCCC";
		if(fcolor > 12999 && fcolor < 14000) fcolor = "DDDD";
		if(fcolor > 13999) fcolor = "EEDD";
		ctx.fillStyle = "#FF" + fcolor;
		ctx.strokeStyle = "#588c79";*/
		ctx.drawImage($("#img_food")[0],(x*cw)+2, (y*cw)+2, cw-4, cw-4);
		return;
	} else if(type == "poison") {
		ctx.drawImage($("#img_poison")[0],(x*cw)+2, (y*cw)+2, cw-4, cw-4);
		return;
	} else if(type == "bonus") {
		ctx.drawImage($("#img_bonus")[0],(x*cw)+2, (y*cw)+2, cw-4, cw-4);
		return;
	} else {
		ctx.fillStyle = "#000000";
		ctx.strokeStyle = "#777777";
	}
	ctx.fillRect(x*cw, y*cw, cw, cw);
	ctx.strokeRect(x*cw, y*cw, cw, cw);
}

function check_collision(x, y, array)
{
	//This function will check if the provided x/y coordinates exist
	//in an array of cells or not
	for(var i = 0; i < array.length; i++)
	{
		if(array[i].x == x && array[i].y == y)
		 return true;
	}
	return false;
}
</script>
<img src="_data/img/snake_food.png" id="img_food" style="display:none;" />
<img src="_data/img/snake_poison.png" id="img_poison" style="display:none;" />
<img src="_data/img/snake_bonus.png" id="img_bonus" style="display:none;" />
</div>
<div id="page5" class="page bg-lightblue">
	<?php $pages[5] = array("Kalender","bg-lightblue"); ?>
	<a name="page5"></a>
	<div class='container12'>
		<div class='row'>
			<div class='column12'>
<script>
$(document).ready(function() {

	$('#calendar').fullCalendar({
	
		// US Holidays
		events:{
			url: 'http://www.google.com/calendar/feeds/musegata72@gmail.com/public/basic',
			currentTimezone: 'Europe/Oslo'
		},
		
		weekNumbers: true,
		firstDay: 1,
		timeFormat: 'H:mm{-H:mm}',
		
		/*eventClick: function(event) {
			// opens events in a popup window
			window.open(event.url, 'gcalevent', 'width=700,height=600');
			return false;
		},
		
		loading: function(bool) {
			if (bool) {
				$('#loading').show();
			}else{
				$('#loading').hide();
			}
		}*/
		
	});
});
</script>	
<div id="calendar"></div>
<div id="cal_month"></div>
<?php /*
# CUSTOM GOOGLE CALENDAR
$your_google_calendar = "musegata72%40gmail.com/public/embed";
$your_google_calendar_url = "https://www.google.com/calendar/embed?src=musegata72@gmail.com&showNav=0&showPrint=0&showTabs=0&showCalendars=0&showTz=0&mode=AGENDA&height=600&wkst=2&ctz=Europe%2FOslo&gsessionid=OK";
$chtml=file_get_contents($your_google_calendar_url);
$startfrom = "function _onload() {window._init(";
$chtml = substr(strstr($chtml,$startfrom),strlen($startfrom));
$endwith = ");}";
$chtml = strstr($chtml,$endwith,true);
$cjson = json_decode($chtml,true);
echo "<pre>";
$events = $cjson['cids'][$your_google_calendar]['gdata']['feed']['entry'];
print_r($events);*/
?>
			</div>
		</div>
	</div>
</div>
<div id="sidebar">
	<div id="header-sbi" class="sidebar-item"><span>Sider</span></div>
	<?php
	foreach($pages as $id => $data) {
		echo "<div id='page".$id."-sbi' class='sidebar-item ".$data[1]."'>";
		echo "<span>".$id."</span>".$data[0];
		echo "</div>";
	}
	?>
</div>
</body>
</html>