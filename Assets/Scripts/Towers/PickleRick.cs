using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleRick : Tower
{ 
    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    //Mutator and Accessor for debuff parameters
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

    //Override Tower methods
    private void Start()
    {    
        ElementType = Element.RICK;
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2, 1.0f, 0.5f, 2.0f, 2.0f),
            new TowerUpgrade(5, 2.5f, 0.5f, 3.0f, 6.0f),
        };
    }

    public override Debuff GetDebuff()
    {
        return new RickDebuff(tickTime, tickDamage, DebuffDuration, Target);
    }

    //Upgrade tick time and damage also
    public override void Upgrade()
    {
        this.tickTime += NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.TickDmg;
        base.Upgrade();
    }
   
    //Return tower stats as string
    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return String.Format("<color=#00ffffff>{0}</color>{1} \nTick time:  {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>TickTime</b></size>", base.GetStats(), TickTime, NextUpgrade.TickTime);
        }
        //Returns the current upgrade
        return String.Format("<color=#00ffffff>{0}</color>{1} \nTick time: {2}%", "<size=20><b>TickTime</b></size>", base.GetStats(), TickTime);
    }
}
