<!DOCTYPE HTML>
<html>

<head>
    <title>Register Form</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" type="text/css" href="style1.css">
    <script type="text/javascript" src="button_operation.js"></script>
    <meta name="viewpoint" content="width=device-width, initial-scale=1.0">
</head>

<body>
    <h1 id="welcome_word"></h1>

    <script>
        //var name = localStorage.getItem("USERNAME");     
        //document.getElementById("demo1").innerHTML = "Welcome " + name;

        var this_url = window.location.href;
        var res = this_url.split("?");
        document.getElementById("welcome_word").innerHTML = "Welcome " + res[res.length - 1];

    </script>

    <div class="buttons">
        <h2><a href="login.html" target="_self">登出</a></h2>
        <button type="button" name="member_list" onclick=turn_to_memberlist()>會員清單</button>
        <button type="button" name="change_profile" onclick=turn_to_change_profile()>編輯個檔</button>
        <button type="button" name="file_operation" onclick=turn_to_file_operation()>檔案上傳/下載</button>
        <p><b>會員清單</b></p>


    </div>


    <table>
        <tr>
            <th>Name</th>
            <th>Gender</th>
            <th>Color</th>
            <th>E-mail</th>
        </tr>

        <?php
            $conn = mysqli_connect("localhost", "root", "", "web");
            if($conn -> connect_error){
                die("Connect failed");
            }
            
            $sql = "SELECT * FROM register";
            $result = $conn -> query($sql);
            
            if($result -> num_rows > 0){
                while($row = $result -> fetch_assoc()){
                    echo "<tr><td>".$row["username"]."</td><td>".$row["gender"]."</td><td>".$row["color"]."</td><td>".$row["email"]."></td></tr>";
                }
                echo "</table>";
            }
            else{
                echo "0 result";
            }
            ?>
    </table>


    <script>
        function turn_to_memberlist() {
            //const username = document.getElementById('username').value;

            //localStorage.setItem("USERNAME", username);

            var this_url = window.location.href;
            var res = this_url.split("?");
            var new_url = "memberlist.php?";
            window.location.href = new_url.concat(res[res.length - 1]);

            return;
        }

        function turn_to_change_profile() {
            //const username = document.getElementById('username').value;
            //localStorage.setItem("USERNAME", username);

            var this_url = window.location.href;
            var res = this_url.split("?");
            var new_url = "change_profile.php?";
            window.location.href = new_url.concat(res[res.length - 1]);

            return;
        }

        function turn_to_file_operation() {
            //const username = document.getElementById('username').value;
            //localStorage.setItem("USERNAME", username);

            var this_url = window.location.href;
            var res = this_url.split("?");
            var new_url = "file_upload.php?";
            window.location.href = new_url.concat(res[res.length - 1]);

            return;
        }

    </script>


</body>


</html>
