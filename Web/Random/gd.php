<?php
// Create a blank image
$image = imagecreatetruecolor(400, 300);

// Allocate a color for the polygon
$col_poly = imagecolorallocate($image, 255, 255, 255);

// Draw the polygon
imagepolygon($image, array(
        80,   30,
		86,	  60,
		95,	 160,
        100, 200,
		150, rand(100,200),
        rand(300,350), rand(200,250)
    ),
    6,
    $col_poly);

// Allocate colors
$black = imagecolorallocate($image, 0, 0, 0);
$white = imagecolorallocate($image, 255, 255, 255);
$yellow= imagecolorallocate($image,255,255,0);

// Path to our ttf font file
$font_file = './REFSAN.TTF';

// Draw the text 'PHP Manual' using font size 13
imagefttext($image, 16, 30, 30, 270, $yellow, $font_file, 'Logget inn på timelisten som: '.@base64_decode($_COOKIE['login_initial']));
imagefttext($image,12,0,10,20,$white,$font_file,"IP: ".$_SERVER['REMOTE_ADDR']);

// Output the picture to the browser
header('Content-type: image/png');

imagepng($image);
imagedestroy($image);
?>