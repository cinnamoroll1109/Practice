<?php
$uri = $_SERVER['REQUEST_URI'];
$temp = explode("?", $uri);
$select_file = end($temp);
?>

<html>
<head>
    <meta name="viewpoint" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" type="text/css" href="style1.css">
</head>
<body>
    <form action="rename.php?<?php echo $select_file?>" method="post">
        <p>Set new name:</p>
        <input type=text name="new_name">
        <input type="submit" name="submit" value="Rename">
        <input type="submit" name="back" value="Back">
<!--        <button id="back" name="back" onclick="turn_to_file_operation()">Back</button>-->
    </form>
    
</body>


</html>
