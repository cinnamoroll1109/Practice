<?php
$username = $_COOKIE["username"];

if(isset($_POST["upload_file"])) {
    $file = $_FILES["fileToUpload"];
    $dir = "uploads/".$username."/".$file["name"];
    
    if(!file_exists($dir)){
        move_uploaded_file($file["tmp_name"], $dir);
        header("Location: file_upload.php?".$username);
    }
    else{
        echo "duplicate";
        header("Location: file_upload.php?".$username);
    }   
}


?>
