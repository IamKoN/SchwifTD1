<?php
	//vaiables for SQL server
	$server_mainname = $_POST["serverMainName"];
	$server_username = $_POST["serverUserName"];
	$server_password = $_POST["serverPassword"];
	$server_database = $_POST["serverDatabase"];
	
	//Variables for authentication
	$server_key = "defaultkey123";
	$server_auth = $_POST[serverKeycode];
	
	if($server_auth == $server_key)
	{
		//established connection to the SQL server
		$connection = new mysqli($server_mainname, $server_username, $server_password, $server_database);
		
		if($connection)
		{
			//variables for userdatabases
			$username = $_POST["update_username"];
			$password = $_POST["update_password"];
			$email = $_POST["update_email"];
			//getting data from the database
			$sqlCheck = "SELECT identity FROM userinfo WHERE username = '".$username."' ";
			$resultCheck = mysqli_query($connection, $sqlCheck);
			
			if($resultCheck)
			{
				
				if($mysqli_num_rows($resultCheck) > 0)
				{
					//insert data to the database
					$sqlUpdate = "UPDATE userinfo SET
						username = '".$username."',
						password = '".$password."',
						email = '".$email."'
						WHERE username = '".$username."' ";
					$resultUpdate = mysqli_query($connection, $sqlUpdate);
					
					if($resultUpdate)
					{
						echo("Success");
					}
					else
					{
						echo("Failed");
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
			die("connection failed.".mysql_connect_error());
			echo("Failed");
		}
	}
	else
	{
		echo("Error");
	}
?>
				