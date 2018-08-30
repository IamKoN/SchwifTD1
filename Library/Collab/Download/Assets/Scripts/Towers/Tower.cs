using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum Element {MORTY, SAUCE, RICK, SPACESHIP, NONE}

public abstract class Tower : MonoBehaviour {

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;

    [SerializeField]
    private float attackCooldown;


    private Queue<Enemy> Enemy = new Queue<Enemy>();

    private bool canAttack = true;

    private float attackTimer = 0 ;

    //change this in Unity to increase the time between shots
    

    private SpriteRenderer mySpriteRenderer;

    private Enemy target;

    //private bool canSee = true; 

    public Element ElementType { get; protected set; }

    public TowerUpgrade[] Upgrades { get; protected set; }

    public int Level { get; protected set; }

    public int Price { get; set; }

    public float ProjectileSpeed
    {       
        get{ return projectileSpeed; }
    }
    public float AttackCD
    {
        get { return attackCooldown; }
    }
    public Enemy Target
    {      
        get{ return target; }
    }
    public int Damage
    {      
        get{ return damage; }
    }
    public float Proc
    {    
        get { return proc; }

        set{ this.proc = value; }
    }

    public float DebuffDuration
    {       
        get{ return debuffDuration;}
        set{ this.debuffDuration = value;}
    }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level - 1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
    }
    
    private void Awake ()
    {
        mySpriteRenderer = transform.GetComponent<SpriteRenderer>();
        Level = 1;
    }	
	void Update ()// Update is called once per frame
    {
        Attack();
	}

    public void Select()
    {      
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateTooltip();
    }

    public void Attack(){
        if (!canAttack){
            
            attackTimer += Time.deltaTime;

            if (attackTimer >= AttackCD){
                
                canAttack = true;
                attackTimer = 0;
            }
        }

        if (Target != null && Target.IsActive){
      
        
            //if(target.name == "tinkles" && projectileType != "lightning"){
            //    canAttack = false;
            //}
            if (canAttack){
                
                Shoot();
                canAttack = false;
            }
        }
        else if (Enemy.Count > 0){
            
            target = Enemy.Dequeue();
        }

        if (target != null && !target.Alive){
            
            target = null;
        }
    } 


    private void Shoot()
    {
        
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        //Projectile projectile = GameManager.Instance.Pool.GetObject(projectilePrefab.name).GetComponent<Projectile>();
        //spawns in the tower position
        projectile.transform.position = transform.position;
        projectile.Initialize(this); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
          
            Enemy.Enqueue(other.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            target = null;
        }
    }
   
    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec <color=#00ff00ff>+{6}</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }

        return string.Format("\nLevel: {0} \nDamage: {1}\nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);

    }
    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.DebuffDuration += NextUpgrade.DebuffDuration;
        Level++;
        GameManager.Instance.UpdateTooltip();
    }
}
