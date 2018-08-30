using UnityEngine;
using UnityEngine.SceneManagement;

// Handles all menu interaction
// The class inherits from Singleton to make it a singleton
public class MenuManager : Singleton<MenuManager>
{
    /// A reference to the MainMenu
    [SerializeField]
    private GameObject mainMenu;

   
    /// A reference to the options menu
    [SerializeField]
    private GameObject optionMenu;

    /// Shows the options menu
    public void ShowOptions()
    {
        //mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    /// Shows the main menu
    public void ShowMain()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// Loads the scene
    public void Play()
    {
        //Loads the ingame level
        SceneManager.LoadScene(2);
    }

    /// Quits to the main menu
    public void QuitToMain()
    {
        //Sets timescale to 1 to unpause
        Time.timeScale = 1;

        //Loads the scene manager
        SceneManager.LoadScene(0);
    }

    /// Shows the inagame menu
    public void ShowIngameMenu()
    {
        //checks if the menu is active
        if (optionMenu.activeSelf)
        {
            //Shows the main menu
            ShowMain();
        }
        else//If the options menu isn't active
        {
            //Sets the menu to active
            mainMenu.SetActive(!mainMenu.activeSelf);

            if (!mainMenu.activeSelf) //if we deactivate we unpause
            {
                Time.timeScale = 1;
            }
            else //If we activate we pause
            {
                Time.timeScale = 0;
            }
        }

    }
}
