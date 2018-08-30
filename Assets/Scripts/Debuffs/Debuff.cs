using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent of tower attack debuffs
public abstract class Debuff
{
    //target of debuff
    protected Enemy target;
    /// The duration of the debuff
    private float duration;
    /// Time elapsed since the debuff was applied
    private float elapsedTime; 

    //Constructor for debuff, every debuff requires a target and has a fixed duration
    public Debuff(Enemy target, float duration)
	{    
        this.target = target;
        this.duration = duration; 
    }

    //Check if current debuff has exceeded alotted duration
    public virtual void Update()
	{
		elapsedTime += Time.deltaTime; 
		if(elapsedTime >= duration)
		{
            Remove();
        }
    }

    //Remove current debuff if target is still alive
    public virtual void Remove()
	{
        if(target != null)
		{
            target.debuffsToRemove.Add(this);
        }
    }
}
