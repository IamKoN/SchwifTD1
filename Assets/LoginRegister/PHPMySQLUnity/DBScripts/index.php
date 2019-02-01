<?php


	//Security Check on server.
	$serverKeycode = "defaultkey123";

	//Check and assign the target method to execute on server.
	$methodName = $_POST['methodName'];

	if($serverKeycode == $_POST['serverKeycode'])
	{
		if($methodName == "SetValue")
		{
			include 'UserDatabase_SetValue.php';
		}

		else if($methodName == "CheckValue")
		{
			include 'UserDatabase_CheckValue.<?php


	//Security Check on server.
	$serverKeycode = "defaultkey123";

	//Check and assign the target method to execute on server.
	$methodName = $_POST['methodName'];

	if($serverKeycode == $_POST['serverKeycode'])
	{
		if($methodName == "SetValue")
		{
			include 'UserDatabase_SetValue.php';
		}

		else if($methodName == "CheckValue")
		{
			include 'UserDatabase_CheckValue.php';
		}

		else if($methodName == "ListValue")
		{
			include 'UserDatabase_ListValue.php';
		}

		else if($methodName == "UpdateValue")
		{
			include 'UserDatabase_UpdateValue.php';
		}

		else if($methodName == "DeleteValue")
		{
			include 'UserDatabase_DeleteValue.php';
		}

		else if($methodName == "GetValue")
		{
			include 'account/GetValue.php';
		}

		else
		{
			echo ("Error");
		}
	}

	else
	{
		echo ("Error");
	}

	
?>php';
		}

		else if($methodName == "ListValue")
		{
			include 'UserDatabase_ListValue.php';
		}

		else if($methodName == "UpdateValue")
		{
			include 'UserDatabase_UpdateValue.php';
		}

		else if($methodName == "DeleteValue")
		{
			include 'UserDatabase_DeleteValue.php';
		}

		else if($methodName == "GetValue")
		{
			include 'account/GetValue.php';
		}

		else
		{
			echo ("Error");
		}
	}

	else
	{
		echo ("Error");
	}

	
?>