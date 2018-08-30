using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Tower
{

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private int slowFactor; 

    //Accessor and mutator methods for debuff parameters
    public float TickTime
    {
        get { return tickTime; }
    }

    public int SlowFactor
    {
        get { return slowFactor; }
    }

    //Init Sauce tower
    private void Start()
    {
        ElementType = Element.SAUCE;
        //Sets up the upgrades
        Upgrades = new TowerUpgrade[]
             {
                 new TowerUpgrade(2, 1.0f, 0.5f, 1.5f, 10),
                 new TowerUpgrade(5, 2.0f, 0.8f, 3.5f, 25),
             };
    }

    //Override Tower methods
    //Upgrade tick time and slowing factor also
    public override void Upgrade()
    {
        //Upgrades the tower
        this.tickTime += NextUpgrade.TickTime;
        this.slowFactor += NextUpgrade.SlowFactor;
        base.Upgrade();

    }

    //Return Sauce Debuff
    public override Debuff GetDebuff()
    {
        return new SauceDebuff(tickTime, slowFactor, DebuffDuration, Target);
    }

    //Return Sauce stats as a string
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSlow factor: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Sauce</b></size>", base.GetStats(), TickTime, SlowFactor, NextUpgrade.TickTime, NextUpgrade.SlowFactor);
        }
        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Sauce</b></size>", base.GetStats(), TickTime, SlowFactor);
    }  
}