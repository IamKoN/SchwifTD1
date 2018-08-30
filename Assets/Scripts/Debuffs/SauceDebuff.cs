using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceDebuff : Debuff
{
    // How often the debuff will tick in seconds
    private float tickTime;
    // Amount of damage per tick
    private float tickDamage;
    // Time since last tick, determine if the debuff should tick again
    private float elpasedTickTime;
    //1-100 target slowing amount
    private int slowingFactor;
    private bool slowApplied;

    public SauceDebuff(float tickTime, int slowingFactor, float duration, Enemy target) : base(target, duration)
    {
        this.tickTime = tickTime;
        this.slowingFactor = slowingFactor;  
    }

    //Slow target for tickTime duration
	public override void Update()
	{
        target.Speed -= (target.MaxSpeed * slowingFactor) / 100;
        if (target != null)
        {
            elpasedTickTime += Time.deltaTime;
            
            if ((elpasedTickTime >= tickTime) || !slowApplied)
            {      
                elpasedTickTime = 0;  
                slowApplied = true;
                target.Speed -= (target.MaxSpeed * slowingFactor) / 100;               
            }
        }
        base.Update();
    }

    //remove slow
    public override void Remove()
	{    
		if (target != null)
		{
			target.Speed = target.MaxSpeed; 
			base.Remove();
        }
	}
}
