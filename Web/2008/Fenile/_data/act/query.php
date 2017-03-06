<?php
/* Section that executes query */
if(@$_GET['form'] == "yes")
{
  $db = @$_POST['database'];
  if ( $db == false ) {
  	$db = $fenile['mysql_db'];
  } else {
  	mysql_select_db($db);
  }
  $query = stripSlashes(@$_POST['query']);
  $result = mysql_query($query);
  echo "<h2 align='center'><strong>MySQL Query Sender</strong></h2><br /><br />";
  echo "<b>Database Selected:</b> ".$db."<br /><br />
        <b>Query:</b><br /><br />$query<br /><br /><h3>Results:</h3><hr /><br />";
  if($result == 0)
     echo "<b>Error ".mysql_errno().": ".mysql_error().
          "</b>";
  elseif (@mysql_num_rows($result) == 0)
     echo("<b>Query completed. No results returned.
           </b><br>");
  else
  {
   echo "<table border='1' bordercolor='#FFF' id='list' width='auto'>
          <thead class='tableHead'>
           <tr class='header'>";
            for($i = 0;$i < mysql_num_fields($result);$i++)
            {
             echo "<th>".mysql_field_name($result,$i).
                  "</th>";
            }
   echo "  </tr>
          </thead>
         <tbody>";
          for ($i = 0; $i < mysql_num_rows($result); $i++)
          {
            echo "<tr>";
             $row = mysql_fetch_row($result);
             for($j = 0;$j<mysql_num_fields($result);$j++) 
             {
               echo("<td>" . $row[$j] . "</td>");
             }
            echo "</tr>";
          }
   echo "</tbody>
        </table>";
  }  //end else
  echo "
   <hr><br>
   <form action=\"?query=1\" method=\"POST\">
     <input type='hidden' name='query' value='$query'>
     <input type='hidden' name='database' 
            value=".@$_POST['database'].">
     <input type='submit' name=\"queryButton\" 
            value=\"New Query\">
     <input type='submit' name=\"queryButton\" 
            value=\"Edit Query\">
	 <input name='Button' type='button' value='   .: Back :.   '
	 		onClick=\"if(confirm('Are You Sure You Want To Quit?')) redirectURL('?');\">
   </form>";
  unset($form);
  exit();
} elseif ( isset($_REQUEST['file']) ) {  // endif form=yes
	$file = @$_REQUEST['file'];
	if ( file_exists($site.$file) ) {
		if ( get_file_extension($file) == ".sql" ) {
			$readfile = true;
		}
	}
}
/* Section that requests user input of query */
@$query=stripSlashes(@$_POST['query']);
if (@$_POST['queryButton'] != "Edit Query")
{
  $query = " ";
}
?>
<style type="text/css">
#querytable {
	font-size:10px;
	font-family:Verdana, Arial, Helvetica, sans-serif;
}
.style1 {
	font-size: 11px;
	font-weight: bold;
}
</style>
<script src="_data/codepress/codepress.js" type="text/javascript" id="cp-script" lang="en-us">
</script>
<h2 align="center"><strong>MySQL Query Sender</strong></h2>
<form action="<?php echo @$_SERVER['PHP_SELF']; ?>?query=1&form=yes" method="POST" onSubmit="codepress.toggleEditor()">
  <div align="center">
    <table id="querytable">
      <tr>
        <td align=right><span class="style1">Type in database name:</span></td>
     <td><input name="database" type="text" 
              value="<?php echo @$_POST['database']; ?>" ></td>
    </tr>
      <tr>
        <td align="right" valign="top">
          <span class="style1">Type in SQL query:</span></td>
     <td><!--<textarea name="query" cols="60" rows="10"><?php echo( @$readfile == true ? readfile(@$site.@$file) : $query ); ?></textarea>-->
	 <textarea name="query" cols="60" rows="20" id="codepress" title="" class="codepress sql"><?php echo( @$readfile == true ? file_get_contents(@$site.@$file) : $query ); ?></textarea></td>
    </tr>
      <tr>
        <td colspan="2" align="center"><input type="submit" value="      .:  Submit Query  :.      ">
        <input name="Button" type="button" value="   .: Back :.   " onClick="if(confirm('Are You Sure You Want To Quit?')) redirectURL('?');"></td>
    </tr>
     </table>
  </div>
</form>
</body></html>
