using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EconManager : Singleton<EconManager>
{
    /// A reference to the upgrade panel
    [SerializeField]
    private GameObject econPanel;

    /// A reference to the options menu
    [SerializeField]
    private GameObject econMenu;

    /// A reference to the economy level text
    [SerializeField]
    private Text econLevelText;

    /// The player's Economy Level
    private int econLevel = 0;

    /// The player's bountyWeight
    private double bountyWeight = 1;

    /// Shows the econ menu
    public void ShowEcon()
    {
        econMenu.SetActive(true);
    }

    public int EconLevel
    {
        get
        {
            return econLevel;
        }
        set
        {
            this.econLevel = value;
            this.econLevelText.text = value.ToString() + " <color=lime>Passive Income</color>";
        }
    }

    public double BountyWeight
    {
        get
        {
            return bountyWeight;
        }
        set
        {
            this.bountyWeight = value;
        }
    }

    public void ShowEconMenu()
    {
        //checks if the menu is active
        if (econMenu.activeSelf)
        {
            //Shows the main menu
            ShowEcon();
        }
        else//If the options menu isn't active
        {
            //Sets the menu to active
            econMenu.SetActive(!econMenu.activeSelf);

            if (!econMenu.activeSelf) //if we deactivate we unpause
            {
                Time.timeScale = 1;
            }
            else //If we activate we pause
            {
                Time.timeScale = 0;
            }
        }
    }

    //Increments passive currency awarded per round by 1
    public void UpEconomy()
    {
        if (GameManager.Instance.Currency > 0)
        {
            GameManager.Instance.Currency--;
            EconLevel++;
        }
    }

    //Not currently used, the variable bountyweight is multiplied by the earned bounty to calculate the player's currency reward
    public void UpBountyWeight()
    {
        //Upgrade cost is 5
        if (GameManager.Instance.Currency >= 5)
        {
            GameManager.Instance.Currency -= 5;
            BountyWeight = BountyWeight * 1.5;
        }
    }
}
