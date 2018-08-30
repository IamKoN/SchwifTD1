using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleRick : Tower {

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;


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

    private void Start(){
        
        ElementType = Element.RICK;
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,1,1,2,10),
            new TowerUpgrade(2,1,1,2,20),
        };
    }

    public override Debuff GetDebuff()
    {
        return new RickDebuff(tickTime, tickDamage, DebuffDuration, Target);
    }

    public override void Upgrade()
    {
        this.tickTime += NextUpgrade.TickTime;
        base.Upgrade();
    }
   

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
