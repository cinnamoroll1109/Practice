<?php
$uri = $_SERVER['REQUEST_URI'];
$temp = explode("?", $uri);
$filename = end($temp);

//$filepath = "download/".$filename;

//header("Cache-Control: public");
//header("Content=Description: File Transfer");
header("Content-Disposition: attachment; filename=$filename");
//header("Content-Type: application/zip");
//header("Content-Transfer-Encoding: binary");

readfile($filename);
//echo "hi";
exit;
?>