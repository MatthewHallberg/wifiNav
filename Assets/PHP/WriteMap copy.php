<?php
    //http://matthewhallberg.com/WriteMap.php
    $servername = "localhost";
    $username = "matthew5_matt";
    $password = "password666";
    $dbName = "matthew5_map";

    $mapName = $_POST["mapName"];
    $mapInfo = $_POST["mapInfo"];

    //Make Connection
    $conn = mysqli_connect($servername,$username,$password,$dbName);

    //Check Connection
    if(!$conn){
        die("Connection Failed. " . mysqli_connect_error());
    } else {
        $sql = "UPDATE map SET info = '$mapInfo' WHERE name = '$mapName'";
        $result = mysqli_query($conn,$sql);
        echo("Success!");
    }
?>