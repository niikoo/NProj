<?php
/* FENILE FILE EDITOR */
if ( @$_REQUEST['file'] ) {
	$file3 = $_REQUEST['file'];
} else {
	$file3 = "";
}
if ( @$_REQUEST['sub'] == "source" ) {
	$source = true;
} else {
	$source = false;
}

if ( $fp = @fopen("./".$site.$file3,"r") ) {
	if (@file_get_contents) {
	$filecontents = file_get_contents("./".$site."/".$file3);
	} else {
	$filecontents = readfile("./".$site."/".$file3);
	}
	$filecontents = htmlspecialchars($filecontents);
} else {
	$file3 = false;
}

/*foreach ( $wysiwyg_editor as $extensions ) {
	if ( get_file_extension($file3) == ".".$extensions ) {
	$wysiwyg = true;
	}
}*/

if ( @$_REQUEST['code'] or @$wysiwyg == false ) {
	$code = true;
} else {
	$code = false;
}

/*if ( @!$code ) {
?>
<script>
if (!confirm('Click OK for Design Mode and Cancel for Code Mode')) {
	redirectURL("?file=<?php echo $file3; ?>&act=edit&side=<?php echo $site; ?>&code=1");
}
</script>
<?php
}*/
if ( @$wysiwyg == true & @!$code ) {
require "_data/editor.htm";
}
?>
<style type="text/css">
   body {
    FONT-FAMILY: Verdana;
    FONT-SIZE: 10;
   }
   #endforms {
    border-left:2px;
	border-color:#000;
	backgorund-color:#a1a1a1;
	width:500px;
	height:auto;
	margin:0 auto;
   }
   .cp {
   	margin:0 auto;
	text-align:left;
	display:block;
	padding:5px;
	text-decoration:none;
   }
</style>
<script language="javascript">
<!--
/* OLD
function formsubmit(type) {
	switch(type) {
		case "save":
		if (confirm('Do You Want To Save')) {
			if (confirm('Overwrite The Old File?')) {
				document.submitf.submit();
			} else {
				newfilename = '<?php echo @$site.$file3; ?>';
				while ( newfilename == '<?php echo @$site.$file3; ?>' ) {
					newfilename = prompt('New Filename:','<?php echo @$site.$file3; ?>');
					if ( newfilename == '<?php echo @$site.$file3; ?>' ) {
						alert("This New Filename Is Equal To The Old One!");
						newfilename = false;
					}
				}
				if ( newfilename == "null" ) {
					newfilename = false;
				}
				if ( newfilename != false ) {
					if ( newfilename != "" ) {
						document.submitf.file.value = newfilename;
					}
					document.submitf.submit = true;
				}
			}
		}
		break;
		case "back":
			if (confirm('Do You Want To Go Back')) {
				redirectURL('?side=<?php echo $site; ?>');
			}
		break;
	}
}
*/
function codetotext() {
	<?php if ( @$code ) { ?>
	document.submitf.text.value = codepress.getCode();
	<?php } else { ?>
	document.submitf.text.value = document.submitf.codpress.value;
	alert(document.submitf.text.value);
	alert(document.submitf.codpress.value);
	<?php } ?>
}
-->
</script>
<?php if ( @$code ) echo '<script src="_data/codepress/codepress.js" type="text/javascript" id="cp-script" lang="en-us"></script>'; ?>
<title>Fenile - <?php echo ($source == true ? "Source View" : "File Editor"); ?></title>
<h2 align="center"><em><strong><?php echo ($source == true ? "Source View" : "File Editor"); ?> </strong></em>
- Current File: <?php echo $site.$file3; ?><br />
</h2>
<body onLoad="<?php if ($source == true) echo "codepress.toggleReadOnly();"; ?>">
<form id="submitf" onSubmit="codetotext()" name="submitf" method="post" onSubmit="<?php $_SERVER['PHP_SELF']; ?>?edit=1">
<p align="center">
	  <input type="hidden" name="side" value="<?php echo $site; ?>" />
	  <input type="hidden" name="file" value="<?php echo $file3; ?>" />
	  <input type="hidden" name="text" value="" />
      <br>
      <label>
	  <script>
	  function toggleAutoComplete() {
	  		if (navigator.appName == "Netscape") {
	  			codepress.toggleEditor()
	  			codepress.toggleAutoComplete()
			} else {
				codepress.toggleAutoComplete()
			}
	  }
	  </script>
      <input type="button" name="Eonoff" value="Syntax Highlighting On/Off" onClick="codepress.toggleEditor()">
	  <input type="button" name="Aonoff" value="Autocomplete On/Off" onClick="toggleAutoComplete()">
      </label>
</p>
      <p align="center">
        <textarea name="codepress" cols="68" rows="25" style="width: 100%" id="codepress" title="<?php echo $site.$file3; ?>" class="codepress <?php echo substr(get_file_extension($site.$file3),1); ?>">
<?php
if ( @$_REQUEST['text'] && $file3 == "" ) {
echo @$_REQUEST['text'];
} elseif ( $file3 == true ) {
	echo $filecontents;
}
?>
    </textarea>
        <br />
        <br>
        <?php if ( !$source ) { ?>
        <input name="saveas_name" type="text" value="<?php echo @$site.$file3; ?>" size="" />
      <input name="saveas" type="submit" value=" .: Save As... :. " border="1" onClick="codetotext()" />
      <br />
        <br />
        <?php if ( @$file3 ) { ?>
        <input name="save" onClick="codetotext()" type="submit" value="           .: Save :.            " border="1" />
        <?php } ?>
          </p>
      <?php } ?>
  <p align="center">
      <input name="back" type="button" onClick="javascript:if(confirm('Do You Want To Go Back?')) redirectURL('?side=<?php echo $site; ?>')" value="          .: Back :.          " border="1" />
  </p>
  <p align="center">
    <?php //<input name="back2" type="button" onclick="codetotext()" value="          .: CodeToText :.          " border="1" /> ?>
  </p>
  <p align="center">
  </p>
  <div id="saveid"></div>
</form>
