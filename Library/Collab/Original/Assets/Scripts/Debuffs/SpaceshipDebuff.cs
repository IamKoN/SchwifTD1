using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipDebuff : Debuff
{
    private float speed;

    //Stop enemy movementS
    public SpaceshipDebuff(Enemy target, float duration) : base(target,duration)
    {
        if(target != null)
        {
            this.speed = target.Speed;
            target.Speed = 0; 
        }
    }

    //remove stun
	public override void Remove()
	{
        //resets the speed
        if (target != null)
		{
            target.Speed = target.MaxSpeed; 
            base.Remove();
        }
	}
}
