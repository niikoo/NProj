<?php
if ( $filename = @$_REQUEST['add_file'] and $type = @$_REQUEST['add_type'] ) {
	if ( ereg('[a-zA-Z0-9.,-_+$]',$filename) ) {
		if ( file_exists($site.$filename) xor is_dir($site.$filename) ) {
		 	alert("ERROR: The Dir/File Already Exist");
			goBACK(-1);
		} else {
			if ( $type == "dir" ) {
				mkdir($site.$filename);
				redirectURL($_SERVER['PHP_SELF']."?side=".$site);
			} elseif ( $type == "file" ) {
				file_put_contents($site.$filename,"");
				redirectURL($_SERVER['PHP_SELF']."?side=".$site);
			} else {
				alert("ERROR: Unknown Error");
			}
		}
	} else {
		alert("ERROR: Specialchars Allowed: . , - _ + $");
		goBACK(-1);
	}
} else {
	alert("Please Enter A Filename");
	goBACK(-1);
}
?>