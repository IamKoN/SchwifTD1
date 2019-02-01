<?php
	//Callbacks(text):
		//"Success" - sucessfull adding inputs to database
		//"Existed" - Username existed from the database
		//"failed" - failed adding inputs to database
		//Error - User authenticatoin is not valid
		
	//Variables for SQL server
	$server_mainname = $_POST["serverMainName"];
	$server_username = $_POST["serverUsername"];
	$server_password = $_POST["serverPassword"];
	$server_database = $_POST["serverDatabase"];
	
	//Variables for authentication
	$server_key = "defaultkey123";
	$server_auth = $_POST["serverKeycode"];
	
	if($server_auth == $server_key)
	{
		//established the connection to mySQL server
		$connection = new mysqli($server_mainname, $server_username, $server_password, $server_database);
		
		//variables for userdatabase
		$username = $_POST["set_username"];
		$password = $_POST["set_password"];
		$email = $_POST["set_email"];
		
		//Getting fata from the database
		$sqlCheck = "SELECT identity FROM userinfo WHERE username = '".$username."' ";
		$resultCheck = mysqli_query($connection, $sqlCheck);
		
		if(!$connection){
			die("Connection Failed.".mysql_connect_error());
			echo("failed");
		}
		
		else
		{
			if($resultCheck)
			{
				if(mysqli_num_rows($resultCheck) < 1)
				{
				    //hash password
				    $hashed= password_hash($password, PASSWORD_DEFAULT);
					//Insert data to the database
					$sqlSet = "INSERT INTO userinfo (username, password, email) VALUES ('".$username."','".$hashed."','".$email."');";
					$resultSet = mysqli_query($connection, $sqlSet);
					
					if($resultSet)
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
					echo("Existed");
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
	