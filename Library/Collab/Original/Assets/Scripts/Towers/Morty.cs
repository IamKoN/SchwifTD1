using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morty : Tower
{
    // Unity input fields
    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    [SerializeField]
    private float splashDamage;

    [SerializeField]
	private MortySplash splashPrefab;

	
    //Accessors and mutators for input fields
    public float TickTime
	{    
        get { return tickTime; }
        set { tickTime = value; }
    }

	public float TickDamage
	{    
		get { return tickDamage; }
		set { tickDamage = value; }
    }

	public float SplashDamage
	{
		get{ return splashDamage; }
		set{ splashDamage = value; }
	}
    
    //Init tower type
    private void Start()
	{
        ElementType = Element.MORTY;     
        //set upgrade stats
        Upgrades = new TowerUpgrade[]
        {
                 new TowerUpgrade(2, 2f, 0.5f, -0.1f, 2.5f, 1f),
                 new TowerUpgrade(5, 3f, 0.5f, -0.1f, 3.0f, 2f),
        };
    }

    // Upgrades the tower
    public override void Upgrade()
    {
        //Upgrades the tower
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime += NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.TickDmg;
        base.Upgrade();

    }

    // Gets the tower's debuff
    public override Debuff GetDebuff()
	{
		return new MortyDebuff(TickTime, TickDamage, SplashDamage, splashPrefab, DebuffDuration, Target); 
	}

    // Returns the towers current stats and upgraded stats
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }
        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage);
    }
}
