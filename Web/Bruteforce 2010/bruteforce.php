<?php
if(isset($argv)) {
	$console=1;
function arguments ( $args )
{
    array_shift( $args );
    $args = join( $args, ' ' );

    preg_match_all('/ (--\w+ (?:[= ] [^-]+ [^\s-] )? ) | (-\w+) | (\w+) /x', $args, $match );
    $args = array_shift( $match );

    $ret = array(
        'input'    => array(),
        'commands' => array(),
        'flags'    => array()
    );

    foreach ( $args as $arg ) {

        // Is it a command? (prefixed with --)
        if ( substr( $arg, 0, 2 ) === '--' ) {

            $value = preg_split( '/[= ]/', $arg, 2 );
            $com   = substr( array_shift($value), 2 );
            $value = join($value);

            $ret['commands'][$com] = !empty($value) ? $value : true;
            continue;

        }

        // Is it a flag? (prefixed with -)
        if ( substr( $arg, 0, 1 ) === '-' ) {
            $ret['flags'][] = substr( $arg, 1 );
            continue;
        }

        $ret['input'][] = $arg;
        continue;

    }

    return $ret;
}
exec("@echo off");
exec("cls");
$arg = arguments($argv);
if (count($argv) <= 4) {
echo "\r\n\r\nMD5 Bruteforce - Command line use:\r\n\r\nphp bruteforce.php <hash> <hash algorithm> <max lenght> [options]\r\n\r\nOptions:\r\n-s: Small characters a-z\r\n-c: Capitalized letters A-Z\r\n-x: Special chars (&!;:. etc.)\r\n-n: Numbers 0-9\r\n\r\nAvailable algorithms:\r\n";
#foreach(hash_algos() as $num => $alg) {
#	echo $alg.", ";
#}
echo "md5 ";
echo "\r\n\r\nPHP Bruteforce script by Nikolai Ommundsen, thanks to Robert Green.\r\n\r\n";
fgets(STDIN); //Read line
exit;
}
/*
 * Thanks to Robert Green for this script he wrote in python
 * http://www.rbgrn.net/blog/2007/09/how-to-write-a-brute-force-password-cracker.html
 * I took what we wrote and ported this to PHP
 * Modified by Nikolai Ommundsen for graphical and command line user interface.
 * 
 * This script was written for PHP 5, but should work with
 * PHP 4 if the hash() function is replaced with md5() or something else
 */

#########################################################
/*                   Configuration                     */
// this is the hash we are trying to crack
define('HASH', stripslashes(htmlspecialchars($arg['input'][0])));

// algorithm of hash
// see http://php.net/hash_algos for available algorithms
define('HASH_ALGO', stripslashes(htmlspecialchars($arg['input'][1])));

// max length of password to try
define('PASSWORD_MAX_LENGTH', stripslashes(htmlspecialchars($arg['input'][2])));
echo HASH.HASH_ALGO.PASSWORD_MAX_LENGHT;
// available characters to try for password
// uncomment additional charsets for more complex passwords
$charset = "";
foreach($arg['flags'] as $num => $flag) {
	if($flag == "s") {
		$charset .= 'abcdefghijklmnopqrstuvwxyz';
	}
	if($flag == "c") {
		$charset .= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
	}
	if($flag == "n") {
		$charset .= '0123456789';
	}
	if($flag == "x") {
		$charset .= '~`!@#$%^&*()-_\/\'";:,.+=<>? ';
	}
}
#########################################################
$charset_length = strlen($charset);
$i = 0;
function check($password)
{	
	if (hash(HASH_ALGO, $password) == HASH) {
		echo "\r\nFOUND MATCH, password: ".$password." Try number: {$i} \r\n";
		exit;
	}
}


function recurse($width, $position, $base_string)
{
	global $charset, $charset_length;
	
	for ($i = 0; $i < $charset_length; ++$i) {
		if ($position  < $width - 1) {
			recurse($width, $position + 1, $base_string . $charset[$i]);
		}
		$i++;
		check($base_string . $charset[$i]);
	}
}

echo "\r\nTarget hash: ".HASH."\r\n";
recurse(PASSWORD_MAX_LENGTH, 0, '');

echo "\r\nExecution complete, no password found\r\n";

exec("@echo on");

//end command line
} elseif(isset($_REQUEST['hash'])) {

/*
 * Thanks to Robert Green for this script he wrote in python
 * http://www.rbgrn.net/blog/2007/09/how-to-write-a-brute-force-password-cracker.html
 * I took what we wrote and ported this to PHP
 * Modified by Nikolai Ommundsen for graphical user interface.
 * 
 * This script was written for PHP 5, but should work with
 * PHP 4 if the hash() function is replaced with md5() or something else
 */

#########################################################
/*                   Configuration                     */

// this is the hash we are trying to crack
define('HASH', stripslashes(htmlspecialchars($_REQUEST['hash'])));

// algorithm of hash
// see http://php.net/hash_algos for available algorithms
define('HASH_ALGO', stripslashes(htmlspecialchars($_REQUEST['alg'])));

// max length of password to try
define('PASSWORD_MAX_LENGTH', stripslashes(htmlspecialchars($_REQUEST['lenght'])));
// available characters to try for password
// uncomment additional charsets for more complex passwords
$charset = "";
if(isset($_REQUEST['comp1'])) {
	$charset .= 'abcdefghijklmnopqrstuvwxyz';
}
if(isset($_REQUEST['comp2'])) {
	$charset .= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
}
if(isset($_REQUEST['comp3'])) {
	$charset .= '0123456789';
}
if(isset($_REQUEST['comp4'])) {
	$charset .= '~`!@#$%^&*()-_\/\'";:,.+=<>? ';
}
#########################################################
$charset_length = strlen($charset);

function check($password)
{	
	if (hash(HASH_ALGO, $password) == HASH) {
		echo '<br />FOUND MATCH, password: '.$password."\r\n";
		exit;
	}
}


function recurse($width, $position, $base_string)
{
	global $charset, $charset_length;
	
	for ($i = 0; $i < $charset_length; ++$i) {
		if ($position  < $width - 1) {
			recurse($width, $position + 1, $base_string . $charset[$i]);
		}
		check($base_string . $charset[$i]);
	}
}

echo 'target hash: '.HASH."\r\n";
recurse(PASSWORD_MAX_LENGTH, 0, '');

echo "<br />Execution complete, no password found\r\n";

} elseif(!isset($console)) {
?>
<form id="form1" name="form1" method="post" action="">
  <p>BRUTEFORCE vX SUPERIOR 2010 v1.2</p>
  <p>Hash:
    <label>
      <input type="text" name="hash" id="hash" value="e2fc714c4727ee9395f324cd2e7f331f" />
      <br />
    </label>
    Algorithm: 
    <label>
    <select name="alg" id="alg">
  <?php
  	foreach(hash_algos() as $num => $alg) {
		echo "<option value='{$alg}'";
		if($alg == "md5") {
			echo " selected='selected' ";
		}
		echo ">".$alg."</option>";
	}
  ?>
    </select>
    </label>
    <br />
  Max lenght: 
  <label>
    <input type="text" name="lenght" id="lenght" value="4" />
  </label>
  </p>
  <p>
    <label>
      <input type="checkbox" name="comp1" value="2" id="compl_0" checked="checked" />
      abcdefg....</label>
    <br />
    <label>
      <input type="checkbox" name="comp2" value="2" id="compl_1" />
      ABCDEFG....</label>
    <br />
    <label>
      <input type="checkbox" name="comp3" value="2" id="compl_2" />
      0123456....</label>
    <br />
    <label>
      <input type="checkbox" name="comp4" value="2" id="compl_3" />
      #¤()%!"....</label>
  </p>
  <p>
    <label>
      <input type="submit" name="exec" id="exec" value="exec" />
    </label>
  </p>
</form>
<?php } ?>
