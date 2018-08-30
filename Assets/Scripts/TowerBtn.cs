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
                tooltip = string.Format("<color=#00ffffff><size=20><b>Morty</b></size></color>      \nLook who's purging now    \nDamage: {0} \nDebuff duration: {1}sec \nSplash damage: {2}\nUpgrade to burn enemies\nTick time: {3}sec \nTick damage: {4}", morty.Damage, morty.DebuffDuration, morty.SplashDamage, morty.TickTime, morty.TickDamage);
                break;
            case "PickleRick":
                PickleRick pr = towerPrefab.GetComponentInChildren<PickleRick>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Pickle Rick</b></size></color>\n'I'M PICKLE RICK, HAHA!'  \nDamage: {0} \nDebuff duration: {1}sec \nTick time: {2}sec\nTick damage: {3}", pr.Damage, pr.DebuffDuration, pr.TickTime, pr.TickDamage);
                break;
            case "Sauce":
                Sauce s = towerPrefab.GetComponentInChildren<Sauce>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Sauce</b></size></color>      \nSzechuan sauce kicks back \nDamage: {0} \nDebuff duration: {1}sec \nTick time: {2}sec\nSlow ratio: {3}%", s.Damage, s.DebuffDuration, s.TickTime, s.SlowFactor);
                break;
            case "Spaceship":
                Spaceship ship = towerPrefab.GetComponentInChildren<Spaceship>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Spacecar</b></size></color>   \nSpacecar keep player safe \nDamage: {0} \nDebuff duration: {1}sec \nChance to stun enemies: {2}% ", ship.Damage, ship.DebuffDuration, ship.Proc);
                break;
        }
        GameManager.Instance.SetToolTipText(tooltip);
        GameManager.Instance.Showstats();
    }
}
