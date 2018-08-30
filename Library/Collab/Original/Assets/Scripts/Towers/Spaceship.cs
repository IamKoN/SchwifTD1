using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : Tower
{
    //init Spaceship tower
    private void Start(){
        ElementType = Element.SPACESHIP;
        //Creates the upgrades
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,2,1,15),
            new TowerUpgrade(5,3,2,35),
        };
    }
    //Return debuff
    public override Debuff GetDebuff()
    {
        return new SpaceshipDebuff(Target, DebuffDuration); 
    }

    //Return stats
    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Storm</b></size>", base.GetStats());
    }
}
