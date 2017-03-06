<?php
function create_upst1_back() {
	global $site;
	if ( @$site ) {
		$upst = explode("/",strtolower($site));
		$lastchar1 = count($upst);
		unset($upst[$lastchar1]);
		unset($upst[$lastchar1-1]);
		unset($upst[$lastchar1-2]);
		$upst1 = "";
		foreach ( $upst as $partofupst ) {
			$upst1 .= $partofupst."/";
		}
		return $upst1;
	/*echo "<br />
	<div id='goup' align='left' style='padding-left:30px'>
	<a href='?side=".@$upst1."'>";
	echo "<span class='style13'>Back</span>";
	echo "</a></div>";*/
	
	#unset($upst1);
	}
}

function generate($format,$file,$type,$site="",$sysfile=false) {
	$upst1 = create_upst1_back();
	if ( $type == "dir" ) {
		$img_path = get_icon("","dir");
	} else {
		$img_path = get_icon(get_file_extension($file),'file');
	}
	if ( $format == 2 || $format == 3 ) {
		if ( $format == "2" ) {
			$a = 20; //Max Filename Lenght
		} elseif ( $format == "3" ) {
			$a = 15;
		}
		if ( strlen($file) >= $a ) {
			$filelenght = strlen($file);
			$newfiletitle = "";
			for($i = 0; $i < $a; $i++) {
				$newfiletitle .= $file[$i];
			}
			$newfiletitle .= "...";
		}
	}
	switch (@$format) {
		case "1":
			//LIST
			global $donestart;
			global $totalsize;
			if ( @$donestart != 1) {
			echo '<table width="100%" height="auto" border="1" id="list">';
    		echo '<thead class="tableHead">';
		    echo '<tr class="header">';
      		echo '<th><div align="center">Select</div></th>';
    		echo '<th><div align="center">Name</div></th>';
			echo '<th><div align="center">Type</div></th>';
     		echo '<th><div align="center">Size</div></th>';
			echo '<th><div align="center">Last Modified</div></th>';
     		echo '<th><div align="center">Read Only</div></th>';
			echo '<th><div align="center">Actions</div></th>';
			echo '</tr>';
    		echo '</thead>';
			$donestart = 1;
			
			#------------------#
			# Go Back Function #
			#------------------#
			if ( @$site ) {
				echo "
				<tr id='goback' style='background-color:#efefef'>
				<td></td>
				";
				echo "<td><div align='center'><a href='?side=".@$upst1."'>";
				echo "<span style='font-weight:bold'>    Go Back    </span></a></div></td>";
				echo "
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				";
				$donestart = 1;
			}
			#----------------
			
			} //END OF STARTING SECTION
			echo "<tr id='".$file."'>";
      		echo '<td><div align="center">';
		   echo '<input type="checkbox" name="filename[]" value="'.$file.'" onClick="selxt(\''.$file.'\',this.checked)" /></div></td>';
    		echo '<td><div align="center">';
			if ( $site == "" && $type != "dir" ) { //Name
				if($sysfile==true){
					echo "<a href='".$file."' style='color:#FF0000'>";
					echo $file.'</a></div></td>';
				} else {
					echo "<a href='".$file."'>";
					echo $file.'</a></div></td>';
				}
			} elseif ( $type == "file" ) {
				if($sysfile==true){
					echo "<a href='".@$site.$file."' style='color:#FF0000'>";
					echo $file.'</a></div></td>';
				} else {
					echo "<a href='".@$site.$file."'>";
					echo $file.'</a></div></td>';
				}
			} elseif ( $type == "dir" ) {
				if($sysfile==true){
					echo "<a href='?side=".@$site.$file."/' style='color:#FF0000'><strong>";
					echo $file.'</strong></a></div></td>'; 
				} else {
					echo "<a href='?side=".@$site.$file."/'><strong>";
					echo $file.'</strong></a></div></td>'; 
				}
			}
			echo '<td><div align="center">';
			if($sysfile==true) {
				$type1 = str_replace("dir","folder",$type);
				echo "System {$type1}</div></td>";
			} else {
				if ( $type != 'dir' ) {
					echo get_file_extension($file)." ";
				}
				echo $type.'</div></td>'; //Type
			}
			
      		if ( $type == "dir" ) { // Filesize
				echo '<td><div align="center">'.size_hum_read(dirsize($site.$file)).'</div></td>';
				$totalsize += dirsize($site.$file);
			} else {
				if (!$filesize = @size_hum_read(filesize($site.$file)) ) {
					$filesize = "0 B";
				}
				echo '<td><div align="center">'.$filesize.'</div></td>';
				$totalsize += filesize($site.$file);
			}
			echo '<td><div align="center">'.date("F d Y H:i",@filemtime($site.$file)); //Last Modified
			echo '</div></td>';
			$currentchmod = substr(sprintf('%o',@fileperms($site.$file)), -4);
			$currentchmod = str_replace("0","",$currentchmod);
			$currentchmod = "CHMOD: ".$currentchmod;
     		echo '<td><div align="center" alt="'.$currentchmod.'" title="'.$currentchmod.'">';
			if ( if_read_only($file,$site) ) { echo 'Yes'; } else { echo 'No'; }
			echo '</div></td>'; //File Permissions
			echo '<td align="center">';
			dispact($file,$type);
			if ( @$type == "file" ) {
				echo '<a href="javascript:void(0)" onclick="prompt(\''.($type=="file"?"{$file} -> MD5:', '".md5_file($site.$file):"").'\')">MD5</div>';
			}
			echo '</tr>';
		break;
		
		case "2":
			//ICONs
			
			global $donestart; # STARTING PART
			if (@$donestart != 1) {
			#------------------#
			# Go Back Function #
			#------------------#
			if ( @$site ) {
				echo "
				<div class='icons'><a href='?side=".@$upst1."'>
				<img src='_data/img/back_48x48.png' width='48' height='48' class='icon' alt='Back' />
				<br /><strong>Go Back</strong></a></div>
				";
				$donestart = 1;
			}
			#----------------
			} # END OF STARTING PART
			
			echo '<div class="icons">';
			if ( $site == "" && $type != "dir" ) {
				if($sysfile==true){
					echo "<a href='".$file."' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='".$file."' title='{$file}' alt='{$file}'>";
				}
			} elseif ( $type == "file" ) {
				if($sysfile==true){
					echo "<a href='".@$site.$file."' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='".@$site.$file."' title='{$file}' alt='{$file}'>";
				}
			} else {
				if($sysfile==true){
					echo "<a href='?side=".@$site.$file."/' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='?side=".@$site.$file."/' title='{$file}' alt='{$file}'>";
				}
			}
			echo '<img src="'.$img_path.'" width="48" height="48" class="icon" alt="'.$file.'" />';
			if (isset($newfiletitle)) {
				echo '<br />'.strtolower($newfiletitle).'</a><br />';
			} else {
				echo '<br />'.strtolower($file).'</a><br />';
			}
			echo '<div id="blb"><input type="checkbox" name="filename[]" value="'.$file.'" />&nbsp;';
			strtoupper(dispact($file,$type));
			echo '</div></div>';
		break;
		
		case "3":
			//Thumbnails
			
			global $donestart; # STARTING PART
			if (@$donestart != 1) {
			#------------------#
			# Go Back Function #
			#------------------#
			if ( @$site ) {
				echo "
				<div class='thumbnails'><a href='?side=".@$upst1."'>
				<img src='_data/img/back.png' width='110' height='110' class='icon' alt='Back' />
				<br /><strong>Go Back</strong></a></div>
				";
				$donestart = 1;
			}
			#----------------
			} # END OF STARTING PART
			
			echo '<div class="thumbnails">';
			if ( $site == "" && $type != "dir" ) {
				if($sysfile==true){
					echo "<a href='".$file."' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='".$file."' title='{$file}' alt='{$file}'>";
				}
			} elseif ( $type == "file" ) {
				if($sysfile==true){
					echo "<a href='".@$site.$file."' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='".@$site.$file."' title='{$file}' alt='{$file}'>";
				}
			} else {
				if($sysfile==true){
					echo "<a href='?side=".@$site.$file."/' title='{$file}' alt='{$file}' style='color:#FF0000'>";
				} else {
					echo "<a href='?side=".@$site.$file."/' title='{$file}' alt='{$file}'>";
				}
			}
			$imgtypes = array("jpg","png","gif","jpeg","bmp");
			foreach( $imgtypes as $imgtyp ) {
				if ( strtolower(get_file_extension($file)) == ".".strtolower($imgtyp) ) {
					$img_path = @$site.$file;
				}
			}
			$imgsize_n = getimagesize($img_path);
			if ( $imgsize_n[0] <= 110 and $imgsize_n[1] <= 110 ) {
				$imgsize = array($imgsize_n[0],$imgsize_n[1]);
			} else {
				$imgsize = array("110","110");
			}
			echo "<div class='thumbimgborder'>";
			echo '<img src="'.$img_path.'" width="'.$imgsize['0'].'" height="'.$imgsize['1'].'" class="icon" alt="'.$file.'"  style="padding:1px" />';
			echo "</div>";
			if (isset($newfiletitle)) {
				echo strtolower($newfiletitle).'</a><br />';
			} else {
				echo strtolower($file).'</a><br />';
			}
			echo '<div id="blb"><input type="checkbox" name="filename[]" value="'.$file.'" />';
			strtoupper(dispact($file,$type));
			echo '</div></div>';
		break;
	}
}
?>