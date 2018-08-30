using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class MortySplash : MonoBehaviour
{
	public float Damage
    {
        get; set;
    }

    //Acquire target to deal damage to
	private void OnTriggerEnter2D(Collider2D other)
    {
		if(other.tag == "Enemy")
		{
			other.GetComponent<Enemy>().TakeDamage(Damage, Element.MORTY);
			Destroy(gameObject); 
		}
	}
}

