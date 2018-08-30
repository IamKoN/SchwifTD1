using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortyDebuff : Debuff
{
    
    // How often the debuff will tick in seconds
    private float tickTime;
    // Amount of damage per tick
	private float tickDamage;
    // Time since last tick, determine if the debuff should tick again
    private float elpasedTickTime;

    private float splashDamage;
    private MortySplash splashPrefab;
	

    //Setter method for morty tower debuff
	public MortyDebuff(float tickTime, float tickDamage, float splashDamage, MortySplash splashPrefab, float duration, Enemy target) : base(target, duration)
	{
        this.tickTime = tickTime;
        this.tickDamage = tickDamage;
		this.splashDamage = splashDamage;
        this.splashPrefab = splashPrefab;
    }

    //Checks elapsed time for debuff tick
	public override void Update()
	{
        if(target != null)
		{
            elpasedTickTime += Time.deltaTime; 
            if(elpasedTickTime >= tickTime)
			{
				elpasedTickTime = 0;

                //Dodes a tick
                target.TakeDamage(tickDamage, Element.MORTY);

                //apply splash damage
                Splash();
            }
        }
        base.Update();
	}

    /// Spawns a splash on the ground
    private void Splash()
    {
        //Spawns the splash at the monster's position
        MortySplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity) as MortySplash;

        //Sets the damage equal to the splash damage
        tmp.Damage = splashDamage;

        //Ensure that the target that spawned the splash can't collide with it
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
