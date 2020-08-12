<?php
$username = $_POST['username'];
$password = $_POST['password'];


if(isset($_POST['register'])){
    Registration($username, $password);
}
else if(isset($_POST['login'])){
    Login($username, $password);
}
else{
    echo 'sorry';
}

function Registration($username, $password){
    $email = $_POST['email'];
    $gender = $_POST['gender'];
    $favcolor = $_POST['favcolor'];
    if (!empty($username) || !empty($password) || !empty($email) || !empty($gender) || !empty($favcolor) ) {
	    $host = "localhost";
        $dbUsername = "root";
        $dbPassword = "";
        $dbname = "web";

        //create connection
        $conn = new mysqli($host, $dbUsername, $dbPassword, $dbname);

        if (mysqli_connect_error()) {
            die('Connect Error('. mysqli_connect_errno().')'. mysqli_connect_error());
        } 
        else {

            $SELECT = "SELECT username From register WHERE username = ? Limit 1";
            $INSERT = "INSERT INTO register (username, password, email, gender, color) VALUES (?, ?, ?, ?, ?)";
            //Prepare statement
            $stmt = $conn->prepare($SELECT);
            $stmt->bind_param("s", $username);
            $stmt->execute();

            $stmt->bind_result($username);
            $stmt->store_result();
            $rnum = $stmt->num_rows;
            if ($rnum==0) {
                $stmt->close();
                $stmt = $conn->prepare($INSERT);
                $stmt->bind_param("sssss", $username, $password, $email, $gender, $favcolor);
                $stmt->execute();

                header("LOCATION: memberlist.php?".$username);

            } 
            else {
                echo "Someone already register using this email";
            }
            $stmt->close();
            $conn->close();				
        }
    } 
    else {
        echo "All field are required";
        die();
    }
}

function Login($username, $password){
    if(!empty($username) || !empty($password)){
        $host = "localhost";
        $dbUsername = "root";
        $dbPassword = "";
        $dbname = "web";
        
        $connection = mysqli_connect($host, $dbUsername, $dbPassword, $dbname); 
      
        // Check connection 
        if (mysqli_connect_errno()) 
        { 
            echo "Database connection failed."; 
        } 
      
        $query = "SELECT * From register WHERE username = '$username' and password = '$password'"; 

        // Execute the query and store the result set 
        $result = mysqli_query($connection, $query); 
        $num_rows = mysqli_num_rows($result);
        
        if($num_rows == 0){
            echo "Username or Password is wrong!!";
        }
        else{            
            header("LOCATION: memberlist.php?".$username);
        }
        
        $connection -> close();        
    }
    else {
        echo "Please enter 'Username' and 'Password'!!";
        die();
    }
    //echo "hi";
}
?>
