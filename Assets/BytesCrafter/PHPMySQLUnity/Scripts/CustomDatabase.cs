using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CustomDatabase : MonoBehaviour
{
    //server code
	public string serverKeycode = "HashCode123";

	[Header("DATABASE SETTINGS")]
	public string serverHS = "localhost";
	public string serverUN = "account";
	public string serverPW = "1234567890";
	public string serverDB = "account";

    //SQL connector
	public WWWForm mysqlConnect
	{
		get
		{
			WWWForm form = new WWWForm ();
			form.AddField ("serverKeycode", serverKeycode);
			form.AddField ("serverMainName", serverHS);
			form.AddField ("serverUsername", serverUN);
			form.AddField ("serverPassword", serverPW);
			form.AddField ("serverDatabase", serverDB);
			return form;
		}
	}

	[Header("DATABASE SQL LINKS")]
	public string targetPhp = "http://localhost/tutorial/index.php";

	private string DebugResult(string result)
	{
		string[] data = result.Split (new char[] { '|' });
		foreach(string text in data)
		{
			Debug.LogWarning (text);
		}

		return data[0];
	}

	
	// Upload user inputs to your given mySQL server.
	/// This is used as a registration method for the user.
	public void User_Register()
	{
		StartCoroutine (SetValue());
	}

	[Header("REGISTRATION FIELD")]
	public InputField set_username = null;
	public InputField set_password = null;
	public InputField set_email = null;
	public Text set_result = null;

	private IEnumerator SetValue()
	{
		set_result.text = "Accessing Database";

		WWWForm setForm = mysqlConnect;
		setForm.AddField ("methodName", "SetValue");

		setForm.AddField ("set_username", set_username.text);
		setForm.AddField ("set_password", set_password.text);
		setForm.AddField ("set_email", set_email.text);
		

		WWW database = new WWW (targetPhp, setForm);
		yield return database;

		set_result.text = database.text;
		DebugResult (database.text);
	}

	
	// Check if the given username and password exist from the given mySQL server.
	// This is used as authentication method or login method for the user.
	public void User_Authenticate()
	{
		StartCoroutine (CheckValue());
	}

	[Header("AUTHENTICATION FIELD")]
	public InputField check_username = null;
	public InputField check_password = null;
	public Text check_result = null;

	private IEnumerator CheckValue()
	{
		if(!check_username.text.Equals(string.Empty) && !check_password.text.Equals(string.Empty))
		{
			check_result.text = "Checking Database";

			WWWForm checkForm = mysqlConnect;
			checkForm.AddField ("methodName", "CheckValue");

			checkForm.AddField ("check_username", check_username.text);
			checkForm.AddField ("check_password", check_password.text);

			WWW database = new WWW (targetPhp, checkForm);
			yield return database;
            
            string result = DebugResult (database.text);
           
            check_result.text = result;
            if(check_result.text == "Success") SceneManager.LoadScene(1);
        }

		else
		{
			string result = DebugResult ("Failed|Insufficient Inputs");
			check_result.text = result;
		}
	}
	
	[Header("LIST CONTENT DISPLAY")]
	public Transform userLister = null;
	public Text list_result = null;

	

	
	// Get a specific user database in your given mySQL server.
	// Then, update the selected user database with the current inputs.
	public void User_UpdateValue(Transform parent)
	{
		StartCoroutine (UpdateValue(parent));
	}

	public IEnumerator UpdateValue(Transform parent)
	{
		list_result.text = "Synchronizing Database";

		WWWForm updateForm = mysqlConnect;
		updateForm.AddField ("methodName", "UpdateValue");

		updateForm.AddField ("update_username", parent.GetChild(0).GetComponent<InputField>().text);
		updateForm.AddField ("update_password", parent.GetChild(1).GetComponent<InputField>().text);
		updateForm.AddField ("update_email", parent.GetChild(2).GetComponent<InputField>().text);
		

		WWW database = new WWW (targetPhp, updateForm);
		yield return database;

		list_result.text = database.text;
		DebugResult (database.text);
	}


	// Get a specific user database in your given mySQL server.
	// Then delete the selected data row in the Database.
	public void User_DeleteValue(Transform parent)
	{
		StartCoroutine (DeleteValue(parent));
	}

	public IEnumerator DeleteValue(Transform parent)
	{
		WWWForm deleteForm = mysqlConnect;
		deleteForm.AddField ("methodName", "DeleteValue");

		string identity = parent.GetChild (0).GetComponent<InputField> ().text;
		deleteForm.AddField ("delete_identity", identity);

		WWW database = new WWW (targetPhp, deleteForm);
		yield return database;

		string result = DebugResult (database.text);
		list_result.text = result;

		if(result.Equals("Success"))
		{
			list_result.text = "Successfully Deleted Data From Database";
			float height = userLister.GetComponent<RectTransform> ().sizeDelta.y - 30;
			userLister.GetComponent<RectTransform> ().sizeDelta = new Vector2(773f, height);
			Destroy (parent.gameObject);
		}
	}

	
	// Get a specific user database in your given mySQL server.
	// This used to get info which correspond to the given username.
	public void User_GetValue()
	{
		StartCoroutine (GetValue());
	}

	[Header("DATABASE FETCHER FIELD")]
	public InputField get_username = null;
	public Transform get_parent = null;
	public Text get_result = null;
		
	public IEnumerator GetValue()
	{
		WWWForm getForm = mysqlConnect;
		getForm.AddField ("methodName", "GetValue");
		getForm.AddField ("get_username", get_username.text);

		WWW database = new WWW (targetPhp, getForm);
		yield return database;

		string[] result = database.text.Split (new char[] { ':' });

		if(result[0].Equals("Success"))
		{
			get_result.text = "Successfully Fetch All Database";

			for(int index = 1; index < result.Length; index++)
			{
				get_parent.GetChild (index - 1).GetComponent<Text> ().text = result [index];
			}
		}

		get_result.text = result[0];
		DebugResult (database.text);
	}
}