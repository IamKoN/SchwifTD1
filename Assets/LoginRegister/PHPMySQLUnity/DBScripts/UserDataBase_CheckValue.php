<?php
	//vaiables for SQL server
	$server_mainname = $_POST["serverMainName"];
	$server_username = $_POST["serverUsername"];
	$server_password = $_POST["serverPassword"];
	$server_database = $_POST["serverDatabase"];
	
	//Variables for authentication
	$server_key = "defaultkey123";
	$server_auth = $_POST["serverKeycode"];
	
	if($server_auth == $server_key)
	{
		//established connection to the SQL server
		$connection = new mysqli($server_mainname, $server_username, $server_password, $server_database);
		
		//variables for userdatabase
		$username = $_POST["check_username"];
		$password = $_POST["check_password"];
		
		//check data to the databse
		$sql = "SELECT password FROM userinfo WHERE username = '".$username."' ";
		$result = mysqli_query($connection, $sql);
		
		if(!$connection)
		{
			die("Connection failed.".mysql_connect_error());
			echo("Failed");
		}
		else
		{
		    
			if($result)
			{
				if(mysqli_num_rows($result) > 0)
				{
					while($row = mysqli_fetch_assoc($result))
					{
					
						//if($row['password'] == $hashed)
						if(password_verify($password, $row['password']))
						{
							echo("Success");
						}
						else
						{
							echo("Incorrect");
						}
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
	}
	else
	{
		echo("Error");
	}
?>