using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morty : Tower {

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

	[SerializeField]
	private MortySplash splashPrefab;

	[SerializeField]
	private int splashDamage;

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
	public int SplashDamage
	{
		get{ return splashDamage; }
		set{ splashDamage = value; }
	}

    private void Start()
	{
        ElementType = Element.MORTY;
        Upgrades = new TowerUpgrade[]
        {
                 new TowerUpgrade(2,2,.5f,5,-0.1f, 2.5f,1),
                 new TowerUpgrade(5,3,.5f,5,-0.1f, 2.5f,1),
        };

    }
    public override Debuff GetDebuff()
	{
		return new MortyDebuff(tickTime, tickDamage, splashDamage, DebuffDuration, Target); 
	}
    public override void Upgrade()
    {
        //Upgrades the tower
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime += NextUpgrade.TickTime;
        base.Upgrade();

    }
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage);

    }
}
