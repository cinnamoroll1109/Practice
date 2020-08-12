<?php
$conn = mysqli_connect("localhost", "root", "", "web") or die('Error with MySQL connection');
$conn -> set_charset("UTF8");

$username = $_POST['username'];
$password = $_POST['password'];
$gender = $_POST['gender'];
$color = $_POST['favcolor'];
$email = $_POST['email'];

$result = 	
	$conn-> query("UPDATE register SET password = '$password', gender = '$gender', color = '$color', email = '$email' WHERE username = '$username'");
    $conn -> close();
//echo "update finish";
header("Location: change_profile.php?".$username);
?>