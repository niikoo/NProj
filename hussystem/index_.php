<?php
# HUSSYSTEM - NIKOLAI OMMUNDSEN - NIIKOO - 2013/2014
$info = array(
	"system_title" => "Husplan",
	"place" => "Mus&eacute;gata 72"
);
$ukenr = date("W-Y");

/*
*
*
* TABELL OPPGAVEFORDELING
*
* UT: $tabell_html
*/

// SANDER'S OFFSHORE ROTASJON - Deltar Sander
$offshore_rotasjon = true; // (bool) Deltar Sander?
$ref_uke = 201349; // REFERANSEUKE - INKL. DENNE 4 UKER HJEMME, SÅ 2 UKER BORTE
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
$offshore_rotasjon = $cstatus;
$persons = array(
	// array([0] >> NAVN (string), [1] DELTAR DENNE UKEN? (bool)
	array("Nikolai Ommundsen",true),
	array("Sascha Scott Fredriksen",true),
	array("Sander M&aelig;hlum",$offshore_rotasjon),
	array("Mads Emil Kj&aelig;rnes",true),
	array("Sebastian R&oslash;the",true)
);
// FJERN PERSONER SOM IKKE SKAL DELTA...
$persons_ny = array();
foreach($persons as $array) {
	if($array[1] == true) { // HVIS DELTAGENE
		$persons_ny[] = $array;
	}
}
$persons = $persons_ny;
unset($persons_ny);
/***
* $tasks
* array
*
* Contents:
*	array[]
*		[0] Oppgave
*		[1] Kan utgå?
*/
$tasks = array(
	array("Gulv",false),
	array("Kj&oslash;kken",false),
	array("Bad",false),
	array("Toalett og vaskerom",true),
	array("St&oslash;vt&oslash;rk og rydding av fellesarealer",true)
);
while(count($persons) < count($tasks)) { // Da må noen oppgaver utgå...
	$persons = removeTask($tasks);
}

function removeTask($taskArrayIn) { // FJERNER EN OPPGAVE SOM KAN UTGÅ
	$removed = false;
	$new = array();
	foreach($taskArrayIn as $task => $skipable) {
		if($skipable == true and $removed == false) {
			$removed = true;
			// SKIP THIS TASK
		} else {
			$new[] = array($task,$skipable);
		}
	}
	return $new;
}

$pos_file = file("pos.txt");
if(!count($pos_file) == 2) {
	die("Feil i fil pos.txt");
}
$pos_ukenr = trim($pos_file[0]);
$pos = $pos_file[1];

$posx = $pos;

$tabell_html = "";

foreach($persons as $person) {
	$tabell_html .= "<tr><th>".$person[0]."</th><td>";
	$tabell_html .= $tasks[$posx][0];
	$posx++;
	if($posx == count($tasks)) {
		$posx = 0;
	}
	$tabell_html .= "</td></tr>\r\n";
}

// NY UKE - NY POSISJON
if($pos_ukenr != $ukenr) {
	$pos++;
	if($pos == count($tasks)) {
		$pos = "0";
	}
	file_put_contents("pos.txt",$ukenr."\r\n".$pos);
}
unset($pos,$pos_ukenr,$pos_file,$posx);
?>
<!html>
<head>
	<link rel="stylesheet" href="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.css" />
	<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
	<script src="http://code.jquery.com/mobile/1.3.2/jquery.mobile-1.3.2.min.js"></script>
	<style>
	table thead {
		display:none;
		color:#396b9e;
	}
	</style>
</head>
<div data-role="header">
    <h1><?php echo $info['system_title']; ?> - <?php echo $info['place']; ?> - Uke: <?php echo $ukenr; ?></h1>
</div>
<ul class="subheader" data-role="listview" data-divider-theme="d" data-inset="false">
   <li data-role="list-divider" role="heading">Ukentlige oppgaver</li>
</ul>
<fieldset class="ui-grid-b">
	<div class="ui-block-a">
		<table data-role="table" id="oppgavetabell" data-mode="reflow" class="ui-responsive table-stroke">
			<thead>
				<tr>
					<th data-priority="1">Navn</th>
					<th data-priority="2">Oppgave</th>
				</tr>
			</thead>
			<tbody>
			<?php
			echo $tabell_html;
			?>
			</tbody>
		</table>
	</div>
	<div class="ui-block-b">&nbsp;</div>
	<div class="ui-block-c">
<?php
# CUSTOM GOOGLE CALENDAR
$your_google_calendar="https://www.google.com/calendar/embed?src=musegata72@gmail.com&showNav=0&showPrint=0&showTabs=0&showCalendars=0&showTz=0&mode=AGENDA&height=600&wkst=2&ctz=Europe%2FOslo&gsessionid=OK";
$url= parse_url($your_google_calendar);
$google_domain = $url['scheme'].'://'.$url['host'].dirname($url['path']).'/';
// Load and parse Google's raw calendar
$dom = new DOMDocument;
$dom->loadHTMLfile($your_google_calendar);
if($dom === false) die("ERROR!");
// Change Google's CSS file to use absolute URLs (assumes there's only one element)
$css = $dom->getElementsByTagName('link')->item(0);
$css_href = $css->getAttribute('href');
$css->setAttribute('href', $google_domain . $css_href);
// Change Google's JS file to use absolute URLs
$script = $dom->getElementsByTagName('script')->item(1);
$js_src = $script->getAttribute('src');
$script->setAttribute('src', $google_domain . $js_src);
// Create a link to a new CSS file called custom_calendar.css
$element = $dom->createElement('link');
$element->setAttribute('type', 'text/css');
$element->setAttribute('rel', 'stylesheet');
$element->setAttribute('href', 'custom_calendar.css');
// Append this link at the end of the element
$head = $dom->getElementsByTagName('head')->item(0);
$head->appendChild($element);
// Export the HTML
echo $dom->saveHTML();
?>
	</div>
</div>