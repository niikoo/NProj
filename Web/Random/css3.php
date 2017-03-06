<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head profile="http://www.w3.org/2005/10/profile">
<title>IRIS - Timeliste</title>
<meta name="Author" content="International Research Institute of Stavanger [Nikolai Ommundsen]"/>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="icon" type="image/vnd.microsoft.icon" href="favicon.ico" />
<style>
/* Shorthand version */
.menu-item {
	position: relative;
	display: inline-block;
	border: 1px dashed #000;
	padding: 10px;
	background: #ffffa2;
	height: 20px;
	opacity: 0.3;
	text-decoration: none;
	
	/* Firefox */
	-moz-transition: all 1s ease;
	/* WebKit */
	-webkit-transition: all 1s ease;
	/* Opera */
	-o-transition: all 1s ease;
	/* Standard */
	transition: all 1s ease;
}

.menu-item:hover {
	opacity: 1;
	background:#F00;
	/* Firefox */
	-moz-transform: scale(2) rotate(30deg) translate(50px);
	/* WebKit */
	-webkit-transform: scale(1.2) rotate(30deg) translate(50px);
	/* Opera */
	-o-transform: scale(2) rotate(30deg) translate(50px);
	/* Standard */
	transform: scale(2) rotate(30deg) translate(50px);
	
	z-index: 1000;
}
#mover {
	position:relative;
	height:50px;
	width:300px;
	left:0;
	background:#ff0000;
	-moz-transition: all 1s ease;
}
#mover:hover {
	left:800px;
	-moz-transform: scale(2);
}
</style>
</head>
<body>
<div class="menu-item">Item 1</div>
<div class="menu-item">Item 2</div>
<div class="menu-item">Item 3</div>
<div class="menu-item">Item 4</div>
<br /><br />
<div id="mover">MOVE</div>
</body>
</html>