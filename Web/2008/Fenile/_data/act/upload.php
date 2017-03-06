<?php
if ( !@$_FILES['upload_file'] && !$_REQUEST['upload'] ) {
	alert("ERROR!");
	goBACK(-1);
	exit;
}
/*while(list($key,$value) = each($_FILES['upload_file']['name'])) {
	if(!empty($value)){
		$filename = $value;
		$add = $site."/$filename";
		if ( file_exists($add) ) {
			alert('ERROR: The File '.$filename.' Already Exists!');	
			goBACK(-1);
			exit;
		}
		copy($_FILES['upload_file']['tmp_name'][$key], $add) or die("ERROR: Unknown Error!");
		chmod("$add",0777) or alert("Failed chmod");
		redirectURL('?side='.$site);
	}
}*/
$target_path = "./" . $site . basename($_FILES['upload_file']['name']);
if(move_uploaded_file($_FILES['upload_file']['tmp_name'], $target_path)) {
    alert("The file ".  basename($_FILES['upload_file']['name']). 
    " has been uploaded");
	chmod($target_path,0777) or alert("Failed chmod");
	redirectURL('?side='.$site);
} else {
	error(10);
}
?>