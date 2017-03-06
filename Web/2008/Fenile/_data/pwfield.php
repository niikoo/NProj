<?php if ( @$fenile['login_r'] == true ) { ?>
<body>
<!--<h2><strong>Fenile Login: </strong></h2>-->
<div id="loginback">
<br />
<div id="loginbox"><br /><br /><br /><br /><br /><br /><br /><br />
<form id="login" name="login" method="post" action="">
  <div align="center">
    Username: 
      <input name="l_username" type="text" id="l_username" />
        <br />
        <br />
      Password: 
      <input name="l_password" type="password" id="l_password" maxlength="15" />
      <br />
      <br />
       <input name="l_submit" type="submit" id="l_submit" value="     .:   Login   :.     " />
    <br /><br /><br /><br /><br />
    <p>Fenile project - v<?php echo $fenile['version']; ?><br />
    </p>
  </div>
</form>
</div>
</div>
</body>
</html>
<?php exit; } ?>