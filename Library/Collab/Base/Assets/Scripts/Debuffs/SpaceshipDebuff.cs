using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipDebuff : Debuff {
    private float speed;
    public SpaceshipDebuff(Enemy target, float duration) : base(target,duration){

        if(target != null){
            this.speed = target.Speed;
            target.Speed = 0; 
        }
    }

	public override void Remove()
	{
        if(target != null)
		{
            target.Speed = target.MaxSpeed; 
            base.Remove();
        }
	}
}
