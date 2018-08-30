using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RickDebuff : Debuff
{
    private float timeSinceTick;
    private float tickTime;
    private float tickDamage;

    public RickDebuff(float tickTime, float tickDamage, float duration, Enemy target) : base(target, duration)
	{
        this.tickTime = tickTime;
        this.tickDamage = tickDamage;
    }

    //Scales up tick damage over time
	public override void Update()
	{
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= tickTime)
            {              
                tickDamage *= (1 + timeSinceTick);
                target.TakeDamage(tickDamage, Element.RICK);
                timeSinceTick = 0;
            }         
        }
        base.Update();
    }
}
