<?php
//disallowdots($site);
/*
if ($dir = @opendir("./".$site)) {
	while (($file = readdir ($dir)) !== false) { 
		$file_names[] = $file;
	}
	closedir ($dir); 
	ksort($file_names);
	$i = 0;
	foreach( $file_names as $part1 ) {
		if ( ereg("^\.\.$",$part1) ) {
			unset($file_names[$i]);
		}
		if ( ereg("^\.$",$part1) ) {
			unset($file_names[$i]);
		}
		$i++;
	}
	//unset($file_names['0']);
	//unset($file_names['1']);
} else {
	echo "<br><br><strong>ERROR: The Directory Doesn't Exist or Unreadable</strong><br><br>";
	exit;
} // OLD */

$pos = strpos(@$site,".."); // Disable '../'
if ( $pos !== false ) {
	$site = ""; // GO TO ROOT
}
$file_names = map_dirs("./".$site);
function map_dirs($path) {
        //global $file_names;
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

function sdirs($path,$level=0) {
	   global $fenile;
       if(is_dir($path)) {
               if($contents = opendir($path)) {
                       while(($node = readdir($contents)) !== false) {
                               if($node!="." && $node!="..") {
									   $path = str_replace("//","/",$path);
									   if ( $level > 0 ) {
									   		$path = str_replace("./","",$path);
								       }
									   if ( @$hide_data != true and @$node != "_data" ) {
									   		$file_names['path'][] = $path;
									   		$file_names['node'][] = $node;
									   		//echo($path.$node."   LEVEL: ".$level."<br>");
                                       		if ( is_dir($path.$node) ) {
									   			$newlevel = $level + 1;
									   			$files3 = sdirs("{$path}{$node}",$newlevel);
									   			for($n = 0;$n != count($files3['node']); $n++) {
													$file_names['path'][] = $files3['path'][$n];
													$file_names['node'][] = $files3['node'][$n];
												}
												
									   		} elseif ( is_dir($path."/".$node) ) {
									   			$newlevel = $level + 1;
									   			$files3 = sdirs("{$path}/{$node}",$newlevel);
									   			for($n = 0;$n != count($files3['node']); $n++) {
													$file_names['path'][] = $files3['path'][$n]."/";
													$file_names['node'][] = $files3['node'][$n];
												}
									   		}
									   }
                               }
                       }
               }
       }
	   if ( isset( $file_names ) ) {
	   	  return @$file_names;
	   } else {
	   	  return false;
	   }
}
?>