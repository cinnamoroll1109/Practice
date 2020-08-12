<?php
$new_name = $_POST["new_name"];
$username = $_COOKIE["username"];
$dir = "uploads/".$username."/";

$uri = $_SERVER['REQUEST_URI'];
$temp = explode("?", $uri);
$old_name = end($temp);

if(isset($_POST["submit"])){
    //rename ("/folder/file.ext", "/folder/newfile.ext");
    rename ($dir.$old_name, $dir.$new_name);
    header("Location: file_upload.php?".$username);
}
else if(isset($_POST["back"])){
    header("Location: file_upload.php?".$username);
}
?>