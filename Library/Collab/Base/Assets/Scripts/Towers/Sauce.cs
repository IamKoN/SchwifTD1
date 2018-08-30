using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Tower
{

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float slowFactor;


    [SerializeField]
    private int splashDamage;

   

    public float TickTime
    {
        get { return tickTime; }
    }
    public float SlowFactor
    {
        get { return slowFactor; }
    }

    public int SplashDamage
    {
        get { return splashDamage; }
    }
    private void Start()
    {
        ElementType = Element.SAUCE;
        //Sets up the upgrades
        Upgrades = new TowerUpgrade[]
             {
                 new TowerUpgrade(2,1,.5f,5,-0.1f,2.5f,1),
                 new TowerUpgrade(5,1,.5f,5,-0.1f,2.5f,1),
             };
    }
    public override Debuff GetDebuff()
    {
        return new SauceDebuff(tickTime, slowFactor, splashDamage, DebuffDuration, Target);
    }
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage);

    }
    public override void Upgrade()
    {
        //Upgrades the tower
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime += NextUpgrade.TickTime;
        this.slowFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();

    }
}