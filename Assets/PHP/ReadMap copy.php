<?php
    //http://matthewhallberg.com/WriteMap.php
    $servername = "localhost";
    $username = "matthew5_matt";
    $password = "password666";
    $dbName = "matthew5_map";

    $mapName = $_POST["mapName"];

    //Make Connection
    $conn = mysqli_connect($servername,$username,$password,$dbName);

    //Check Connection
    if(!$conn){
        die("Connection Failed. " . mysqli_connect_error());
    } else {
        $sql = "SELECT info FROM map WHERE name = '$mapName'";
        $result = mysqli_query($conn,$sql);
        $row = mysqli_fetch_assoc($result);
        echo($row['info']);
    }
?>