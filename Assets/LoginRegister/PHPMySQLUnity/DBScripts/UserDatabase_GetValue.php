<?php
    //Variables for Authentication.
    $server_key = "defaultkey123";
    $server_auth = $_POST["serverKeycode"];
   
    if($server_auth == $server_key)
    {
        //Variables for SQL Server.
        $server_mainname = $_POST["serverMainName"];
        $server_username = $_POST["serverUsername"];
        $server_password = $_POST["serverPassword"];
        $server_database = $_POST["serverDatabase"];
       
        //Established the connection to the mySQL server.
        $connection = new mysqli($server_mainname, $server_username, $server_password, $server_database);
 
        if($connection)
        {
            //Variables for userdatabase.
            $username = $_POST["get_username"];
       
            //Getting data from the database.
            $sql = "SELECT username, password, email, firstname, lastname, gender, age FROM userinfo WHERE username = '".$username."' ";
            $result = mysqli_query($connection, $sql);
           
            if($result)
            {
                if(mysqli_num_rows($result) > 0)
                {
                    while($row = mysqli_fetch_assoc($result))
                    {
                        echo "Success:".$row['username'].":".$row['password'].":".$row['email'].":".$row['firstname'].":".$row['lastname'].":".$row['gender'].":".$row['age']."";
                    }
                }
       
                else
                {
                    echo("Unknown");
                }
            }
       
            else
            {
                echo("Failed");
            }
        }
       
        else
        {
            die("Coonection Failed.".mysql_connect_error());
            echo("Failed");
        }
    }
   
    else
    {
        echo("Error");
    }  

?>