<?php
function get_icon($extension,$type) { // TYPE: dir or file
	if ($type == "file") {
		if ($dir1 = @opendir("./_data/icons")) {
			while (($file1 = readdir ($dir1)) !== false) { 
				$file_names1[] = $file1;
			}
			closedir ($dir1); 
			ksort($file_names1);
			unset($file_names1['0']);
			unset($file_names1['1']);
		} else {
			echo "<br><br><strong>ERROR: The Directory Doesn't Exist or Unreadable</strong><br><br>";
			exit;
		}
		if ( @$file_names1 ) {
			foreach ( $file_names1 as $ipa ) {
				$ipa_ext = get_file_extension($ipa);
				$ipa1 = explode(".",$ipa);	
				$ipa2 = $ipa1['0'];
				if ( strlen($ipa2) <= 4 ) {
				
					if ( strtolower($extension) == ".".strtolower($ipa2) ) {
						$output = "_data/icons/{$ipa2}{$ipa_ext}";
					}
				}
			}
		}
	} elseif ( $type == "dir" ) {
		$output = "_data/icons/directory.png";
	} else {
		echo "ERROR: Icon Type Error!";
		exit;
	}
	if ( @!$output ) {
	$output = "_data/icons/standardfile.png";
	}
	return $output;
}
?>