using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : Tower {

    private void Start(){
        ElementType = Element.SPACESHIP;
        //Creates the upgrades
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,2,1,2),
            new TowerUpgrade(5,3,1,2),
        };
    }

    public override string GetStats()
    {
        return String.Format("<color=#add8e6ff>{0}</color>{1}", "<size=20><b>Storm</b></size>", base.GetStats());
    }
}
