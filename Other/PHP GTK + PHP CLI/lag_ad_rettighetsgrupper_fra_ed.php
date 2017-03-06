<?php
function cmdnor($string) {
    $string = str_replace(array("æ","ø","å","Æ","Ø","Å"),array("‘","›","†","‘","›","†"),$string);
    return $string;
}
echo cmdnor("eD til AD\n");

// CONFIG
$config = array();
$config['ad_server']	= "*ad_server*";						// Active Directory Server (ldaps) - example: svg-dc1.iris.no
$config['ad_base-dn']	= "*BASEDN*";					// Active Directory BaseDN
$config['ad_users_ou']	= "OU=Users,OU=!L-!OU=iris";			// Active Directory - Users locations
$config['ad_account-suffix'] = "@*.local";					// Active Directory Account Suffix, example: '@iris.local'
$config['ad_domain']	= "*";								// Active Directory - Domenenavn, trengs for å koble til datamaskiner knyttet til domenet
$config['ad_user']		= "administrator";						// Active Directory - Koble til AD med dette brukernavnet > Må være domene-admin
$config['ad_home-drive'] = "H:";        // Active Directory - Standard 'Home drive' for brukere
$config['ad_password']	= "PWD"; // Active Directory - Passord til brukeren i $config['ad_user'], enkodet i base64
$config['ad_default_profile_path'] = "\\\\!L#!-fil1\\Profile\\"; // Active Directory - Standard plassering der profilen blir lagt (husk \\ > \), slutter med (dobbel)backslash. Eks: '\\\\svg-fil1\\Profiles\\'
$config['ad_default_home-directory'] = "\\\\!L#!-fil1\\Home\\";  // Active Directory - Standard plassering for home-directory (husk dobbel-backslash \\ blir \), slutter med (dobbel)backslash. Eks: '\\\\svg-fil1\\Home\\'

$config['locations'] = array(									// Fysiske lokasjoner - kan brukes i ad_default_profile_path, ad_default_home-directory og ad_users_ou ved bruk av !L-! for key og !L#! for value
						"svg"	=>	"Ullandhaug",				// Format: array("lokasjon kort navn"=>"lokasjon fullt navn");
						"brg"	=>	"Bergen");


function map_dirs($path) { //Gir alle filene og mappene i $path mappen.
		if(is_dir($path)) {
                if($contents = opendir($path)) {
                        while(($node = readdir($contents)) !== false) {
                                if($node!="." && $node!="..") {
                                      $file_names[] = $node;
                                }
                        }
                }
        }
		return @$file_names;
}
function ad_chgrp($dir,$group,$rights="F") {
    /*$exec = "icacls \"".$dir."\" /reset";
    echo "EXEC: ".$exec."\n";
    $result = exec($exec);*/
    $exec = "icacls \"".$dir."\" /grant:r \"".$group."\":(OI)(CI)".$rights;
    $result = "\n".exec($exec);
    $exec = "icacls \"".$dir."\" /inheritance:e";
    return $result." - ".exec($exec);
}

$location = "SVG";
$P = "E:\\Projects\\";
$Pr = map_dirs($P);
$output = array();
foreach($Pr as $df) {
    $Pdf = $P.$df;
    if(is_file($Pdf)) {
		//echo "File found: ".$Pdf.", action: SKIP\n";
	} elseif(is_dir($Pdf)) {
		echo "\n".$df.":";
		if(preg_match("/[0-9]{3}/",$df)) {
			$srdf = map_dirs($Pdf);
			if(!is_array($srdf)) {
				continue;
			}
			$sdfc = array();
			foreach($srdf as $sdf) {
				if(preg_match("/^[P]{1}[0-9]{7,8}/",$sdf,$sdfm)) {
					if(is_dir($Pdf."\\".$sdf)) {
						$sdfc[$sdf] = ad_chgrp($Pdf."\\".$sdf,$sdfm[0],"MRXWD");
						echo ".";
					}
				} else { // UTFORSK MER
					$thisdir = $Pdf."\\".$sdf;
					if(is_dir($thisdir)) {
						$thisdir_explored = map_dirs($thisdir);
						if(is_array($thisdir_explored)) {
							foreach($thisdir_explored as $thisdir_ff) {
								if(is_dir($thisdir."\\".$thisdir_ff)) {
									if(preg_match("/^[P]{1}[0-9]{7,8}/",$thisdir_ff,$thisdir_match)) {
										//echo "ADD: ".$sdf."\\".$thisdir_ff."\n";
										$sdfc[$sdf."\\".$thisdir_ff] = ad_chgrp($thisdir."\\".$thisdir_ff,$thisdir_match[0],"MRXWD");
										echo ".";
									}
								}
							}
						}
					}
				}
			}
			$output[$df] = $sdfc;
			//echo ", action: Add and explore\n";
		} else {
			//echo ", action: skip\n";
		}
    } else {
		//echo $Pdf." ???\n";
    }
}

// OUTPUT BUFFERING
ob_start();
print_r($output);
$cnt = ob_get_contents();
file_put_contents("ob.txt",$cnt);
ob_end_flush();
exit;
?>