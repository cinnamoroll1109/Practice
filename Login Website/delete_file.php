
<?php
$username = $_COOKIE["username"];

$uri = $_SERVER['REQUEST_URI'];
$temp = explode("?", $uri);
$select_file = end($temp);

$path = "uploads/".$username."/".$select_file;

if(unlink($path)){
    header("Location: file_upload.php?".$username);
}
else{
    echo "delete error!!";
}
?>