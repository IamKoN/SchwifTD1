using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum Element {MORTY, SAUCE, RICK, SPACESHIP, NONE}

public abstract class Tower : MonoBehaviour
{
    //private bool canSee = true;

    private bool canAttack = true;

    private float attackTimer = 0;

    private SpriteRenderer mySpriteRenderer;

    private Enemy target;

    private Queue<Enemy> Enemy = new Queue<Enemy>();

    /// <summary>
    /// Unity variable fields
    /// </summary>
    [SerializeField]
    private float damage;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private string projectileType;

    /// <summary>
    /// Accessors and mutators for serialized fields 
    /// </summary>
    public float Damage
    {
        get { return damage; }
    }

    public float DebuffDuration
    {
        get { return debuffDuration; }
        set { this.debuffDuration = value; }
    }

    public float Proc
    {
        get { return proc; }

        set { this.proc = value; }
    }

    public float AttackCD
    {
        get { return attackCooldown; }
    }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    /// <summary>
    /// Accessors and mutators for private variables
    /// </summary>
    public int Level
    {
        get; protected set;
    }

    public int Price
    {
        get; set;
    }

    public Element ElementType
    {
        get; protected set;
    }

    public Enemy Target
    {
        get { return target; }
    }

    //Stores array of upgrades
    public TowerUpgrade[] Upgrades
    {
        get; protected set;
    }    

    //Accessor for upgrades based on tower level
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

    /// <summary>
    /// Methods fpr manipulating tower objects
    /// </summary>
    //Disable sprite renderer and update tooltip for tower object
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateTooltip();
    }
    //Initializes tower sprite and level
    private void Awake ()
    {
        mySpriteRenderer = transform.GetComponent<SpriteRenderer>();
        Level = 1;
    }

    //Update is called once per frame
    void Update ()
    {
        Attack();
	}

    //Check if tower can attack a target or not
    public void Attack()
    {
        //Check if attack cooldown is less than the attack timer
        if (!canAttack)
        {          
            attackTimer += Time.deltaTime;
            if (attackTimer >= AttackCD)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        //Check if can attack target, then shoot target
        if (Target != null && Target.IsActive)
        {
            if (canAttack)
            {              
                Shoot();
                canAttack = false;
            }
        }
        //check if ther are any targets left to attack
        else if (Enemy.Count > 0)
        {           
            target = Enemy.Dequeue();
        }
        if (target != null && !target.Alive)
        {            
            target = null;
        }
    } 

    //Create projectile object and initialize its position on tower
    private void Shoot()
    {       
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this); 
    }

    //Acquire target lock: add enemy to queue
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {       
            Enemy.Enqueue(other.GetComponent<Enemy>());
        }
    }

    //Dequeue target
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            target = null;
        }
    }
   
    //Print tower stats
    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec <color=#00ff00ff>+{6}</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }

        return string.Format("\nLevel: {0} \nDamage: {1}\nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);

    }

    //Iterate money and tower changes upon upgrading tower
    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.DebuffDuration += NextUpgrade.DebuffDuration;
        this.proc += NextUpgrade.ProcChance;
        Level++;
        GameManager.Instance.UpdateTooltip();
    }

    public abstract Debuff GetDebuff();
}
