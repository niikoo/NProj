<?php
if ( isset($_REQUEST['download'],$_REQUEST['file']) ) {
	   header("Content-Type: octet/stream");
       header("Content-Disposition: attachment; filename=" . basename($_REQUEST['file']));
       echo file_get_contents(@$_REQUEST['side'].$_REQUEST['file']);
	   exit;
}
?>