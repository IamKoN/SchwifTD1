using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script attached to all tower buttons used to buy towers
public class TowerBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject towerPrefab;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text txtPrice;

	[SerializeField]
	private Sprite sprite;

    //Accesses the button prefab
	public GameObject TowerPrefab
    {
		get{ 
			return towerPrefab; 
		}

	}

    //Accesses button's price
    public int Price
    {
        get
        {
            return price;
        }
    }

    public Text TxtPrice
    {
        get
        {
            return txtPrice;
        }
    }

	public Sprite Sprite { 
		get{ 
			return sprite; 
		} 
	}
    public void ShowInfo(string type)
    {

        string tooltip = string.Empty;
        switch (type)
        {
            case "Morty":
                Morty morty = towerPrefab.GetComponentInChildren<Morty>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Morty</b></size></color>      \nLook who's purging now    \nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nTick time: {3} sec \nTick damage: {4}\nUpgrade to burn enemies",morty.Damage, morty.Proc, morty.DebuffDuration, morty.TickTime, morty.TickDamage);
                break;
            case "PickleRick":
                PickleRick pr = towerPrefab.GetComponentInChildren<PickleRick>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Pickle Rick</b></size></color>\n'I'M PICKLE RICK, HAHA!'  \nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nTick damage: {3}%\n", pr.Damage, pr.Proc, pr.DebuffDuration, pr.TickDamage);
                break;
            case "Sauce":
                Sauce s = towerPrefab.GetComponentInChildren<Sauce>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Sauce</b></size></color>      \nSzechuan sauce kicks back \nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nTick time: {3} sec \nSplash damage: {4}\nChance to slow enemies: {5}", s.Damage, s.Proc, s.DebuffDuration, s.TickTime, s.SplashDamage, s.SlowFactor);
                break;
            case "Spaceship":
                Spaceship ship = towerPrefab.GetComponentInChildren<Spaceship>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Spacecar</b></size></color>   \nSpacecar keep player safe \nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nChance to stun enemies", ship.Damage, ship.Proc, ship.DebuffDuration);
                break;
        }
        GameManager.Instance.SetToolTipText(tooltip);
        GameManager.Instance.Showstats();
    }
}
