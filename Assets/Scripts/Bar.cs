using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour{

    //Movement speed of the health bar
    [SerializeField]
    private float lerpSpeed; 

    //reference to content of the bar
    [SerializeField]
    private Image content;


    [SerializeField]
    private Color fullColor;


    [SerializeField]
    private Color lowColor;


    [SerializeField]
    private bool lerpColors;

    private float fillAmount;

    //Max value of bar. Reflects the players max health
    public float MaxValue { get; set; }

    //sets the bars value
    public float Value
    {
        set { fillAmount = Map(value, 0, MaxValue, 0, 1); }
    }

	// Use this for initialization
	void Start ()
    {
        if(lerpColors) { content.color = fullColor; }
	}

    // Update is called once per frame
    void Update() { HandleBar(); }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime*lerpSpeed); 
        }
        else
        {
            content.fillAmount = fillAmount;
        }
        if(lerpColors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
    }

    //Maps a range of number into another range
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin; 
    }
}
