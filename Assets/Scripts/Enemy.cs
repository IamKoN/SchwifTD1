using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script attached to all enemies
public class Enemy : MonoBehaviour
{
    //this will allow us to change the speed of each enemy
    [SerializeField]
    private float speed;

    //Enemy Type
    [SerializeField]
    private Element elementType;

    //Number of currency gained per kill
    [SerializeField]
    public double bounty;

    // Enemy health
    [SerializeField]
    private Stat health;

    public Point GridPosition { get; set; }

    private Stack<Node> path;

    private List<Debuff> debuffs = new List<Debuff>();

    public List<Debuff> debuffsToRemove { get; private set; }

    public List<Debuff> newDebuffs = new List<Debuff>(); 

    private SpriteRenderer spriteRenderer;

    //amount of damage reduction enemy will take from its own type
    private int invulnerability = 2;

    private Vector3 destination;

    public float MaxSpeed { get; private set; }

    public Point Gridposition { get; set; }

    public bool IsActive { get; set; }

    //Checks if enemy is alive
    public bool Alive
	{
        get { return health.CurrentVal > 0; }
    }

    //gets element type of the enemy
    public Element ElementType
	{
        get { return elementType; }
    }

	public float Speed
    {
		get { return speed; }
		set { speed = value; }
    }

    //On enemy spawn
    protected virtual void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        MaxSpeed = Speed;
        debuffsToRemove = new List<Debuff>();
        health.Initialize();
	}

	protected virtual void Update()
    {
        HandleDebuffs(); 
        Move();
    }

    //spawns enemy in the world
    public void Spawn(int health)
    {
        //resets the health
        this.health.MaxVal = health;
        this.health.CurrentVal = health;

        //removes all debuffs
        debuffs.Clear();

        //Sets the position
        transform.position = LevelManager.Instance.BluePortal.transform.position;

        //Scales enemies 
        StartCoroutine(Scale(new Vector3(0.25f, 0.25f), new Vector3(1, 1), false));
        SetPath(LevelManager.Instance.Path, false);
    }

    //Scales enemy to fit portal or world
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime * 1;

            yield return null;
        }
        //makes sure that it has correct scale
        transform.localScale = to;

        IsActive = true;

        if(remove)
        {
            Release(); 
        }
    }

    private void Move()
    {
        //Checks if unit is active
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            if (transform.position == destination)
            {

                //If there is a path with more nodes, keep moving
                if (path != null && path.Count > 0)
                {

                    Gridposition = path.Peek().GridPosition;
                    destination = path.Pop().worldPosition;
                }
                //No Path.. done
                else
                    IsActive = false;
            }
        }

    }

    //Gives enemy path to walk
    private void SetPath(Stack<Node> newPath, bool active)
    {
        //If there is a path
        if (newPath != null)
        {
            this.path = newPath;
            Gridposition = path.Peek().GridPosition;
            destination = path.Pop().worldPosition;
            this.IsActive = active;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //despawn enemy if it enters red portal
        if(collision.tag == "redPortal")
		{
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));

            GameManager.Instance.Lives--; 
        }
        if (collision.tag == "Tile")
		{
            spriteRenderer.sortingOrder = collision.GetComponent<Tilescript>().GridPosition.Y;
        }
	}

    private void Release()
    {
        //removes all debuffs
        foreach (Debuff debuff in debuffs)
        {
            debuff.Remove();
        }

        //resets enemy position
        Gridposition = new Point(0, 0);
        Speed = MaxSpeed;

        //removes enemy from the game
        GameManager.Instance.RemoveEnemy(this);
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        
    }

    public void TakeDamage(float damage, Element dmgSource)
	{
        //Checks if enemy is in play
        if (IsActive)
		{
            //Calculates amount of damage enemy takes
            if(dmgSource == ElementType)
			{
                damage = damage / invulnerability;
                invulnerability++; 
            }
            //Reduces enemy health
            health.CurrentVal -= damage;

            //Checks if enemy is dead
            if (health.CurrentVal <= 0)
			{    
                GameManager.Instance.Currency += bounty*EconManager.Instance.BountyWeight;
                GetComponent<SpriteRenderer>().sortingOrder--;
                Invoke("Release", 1);
                IsActive = false;

            }
        }
    }

    //Adds debuff to enemy
    public void AddDebuff(Debuff debuff)
	{    
        //Checks if dedbuff exists currently
        if(!debuffs.Exists(x => x.GetType() == debuff.GetType()))
		{    
            newDebuffs.Add(debuff); 
        }
    }

    public void RemoveDebuff(Debuff debuff)
	{   
        debuffsToRemove.Add(debuff); 
    }

    private void HandleDebuffs()
	{
        //aif enemy has new debuffs
        if(newDebuffs.Count > 0)
		{
            //Add to debuff list
            debuffs.AddRange(newDebuffs);

            //Clear new debuffs so they will only be added once
            newDebuffs.Clear(); 
        }

        //checks for debuff removal
        foreach(Debuff debuff in debuffsToRemove)
		{
            debuffs.Remove(debuff); 
        }
        //Clears debuffs to remove
        debuffsToRemove.Clear(); 

        //Updates all debuffs
        foreach (Debuff debuff in debuffs )
		{
            debuff.Update(); 
        }
    }
}
