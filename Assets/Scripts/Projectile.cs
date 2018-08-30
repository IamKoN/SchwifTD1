using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Enemy target;

    private Tower Tower;

    private Element elementType; 

	// Use this for initialization
	void Start ()
    {
	}
	// Update is called once per frame
	void Update ()
    {
        MoveToTarget(); 
	}

	public void Initialize(Tower tower){
        this.target = tower.Target; 
        this.Tower = tower;
        this.elementType = tower.ElementType; 
	}

    private void MoveToTarget()
    {
        if(target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * Tower.ProjectileSpeed);
            //possible use this so for the tower rotation
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        }
        else if (!target.IsActive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject); 
        }
    }

	private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            if (target.gameObject == other.gameObject)
            {
                Enemy target = other.GetComponent<Enemy>();
                target.TakeDamage(Tower.Damage, Tower.ElementType);
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }
        }
	}
}
