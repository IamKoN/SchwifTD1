using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class Stat
{
    //Reference to bar that stat is controlling
    [SerializeField]
    private Bar bar;

    //Max value of stat
    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal; 

    //Accessing and setting val
    public float CurrentVal
	{
        get{ return currentVal; }
        set
		{
            this.currentVal = Mathf.Clamp(value, 0, maxVal);
            bar.Value = currentVal; 
        }
    }

    //Accessing and setting val
    public float MaxVal
	{
        get{ return maxVal; }
        set
		{
            this.maxVal = value;
            bar.MaxValue = maxVal; 
        }
    }

    //Initialized stat
    public void Initialize()
	{
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal; 
    }
}
