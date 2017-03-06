<?php
if ( $theme = @$_REQUEST['theme_select'] ) {
	mysql_query("UPDATE fenile_config SET value='{$theme}' WHERE id='theme'") or die(error('16'));
	redirectURL("index.php?side=".$site);
} else {
error('15');
exit;
}
?>