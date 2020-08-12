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
        <p><b>檔案上傳/下載</b></p>
    </div>

    <form action="upload.php" method="post" enctype="multipart/form-data">
        <!--        Select image to upload:-->
        <table>
            <tr>
                <td colspan="2">
                    <input type="file" name="fileToUpload" id="fileToUpload">

                </td>
                <td colspan="2">
                    <input type="submit" name="upload_file" value="Upload">
                </td>
            </tr>
            <tr>
                <th>File Name</th>
                <th>File Size</th>
                <th>Upload Timestamp</th>
                <th>Edit</th>
            </tr>

            <?php
            $uri = $_SERVER['REQUEST_URI'];
            $temp = explode("?", $uri);
            $name = end($temp);
            setcookie("username", $name);
            
            $dir = "uploads/".$name."/";
            if(!file_exists($dir)){
                mkdir($dir);
            }
                        
            //Display all uploaded files
            $files = scandir($dir);
            //print_r($files);
            for($i = 2; $i < count($files); $i++){
                //Display links to download
                ?>
            <tr>
                <!--file name-->
                <td>
                    <a href="<?php echo $dir.$files[$i] ?>"><?php echo $files[$i]?></a>
                </td>
                <!--file size-->
                <td>
                    <p>
                        <?php           
                        $fileSizeBytes = filesize($dir.$files[$i]);
                        if($fileSizeBytes < 1024){
                            echo $fileSizeBytes."B";  
                        }
                        else{
                            $fileSizeKB = round($fileSizeBytes / 1024);
                            echo $fileSizeKB."KB";   
                        }                                                       
                        ?>
                    </p>
                </td>
                <!--timestamp-->
                <td>
                    <p>
                        <?php
                        if (file_exists($files[$i])) {
                            date_default_timezone_set("Asia/Taipei");
                            echo date ("Y-m-d H:i:s", filemtime($dir.$files[$i]));
                        }
                        ?>
                    </p>
                </td>
                <!--edit-->
                <td>
                    <!--                    <button type="submit" name="delete_file" onclick="">Delete</button>                   -->
                    <!--                    <button type="submit" name="rename_file">Rename</button>-->

                    <a href="new_filename.php?<?php echo $files[$i]?>">[Rename]</a>
                    <a href="delete_file.php?<?php echo $files[$i]?>">[Delete]</a>
                    <a href="download.php?<?php echo $files[$i]?>">[Download]</a>
                </td>
            </tr>


            <?php
            }
            ?>
            <tr>

            </tr>
        </table>

    </form>



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
