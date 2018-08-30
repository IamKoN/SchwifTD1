using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //this will allow us to change the speed of each monster
    [SerializeField]
    private float speed;

    [SerializeField]
    private Element elementType;

    [SerializeField]
    public double bounty;

    [SerializeField]
    private Stat health;

    private Stack<Node> path;

    private List<Debuff> debuffs = new List<Debuff>();

    private List<Debuff> debuffsToRemove = new List<Debuff>();

    private List<Debuff> newDebuffs = new List<Debuff>(); 

    private SpriteRenderer spriteRenderer;

    private int invulnerability = 2;

    private Vector3 destination;

    public float MaxSpeed { get; private set; }

    public Point Gridposition { get; set; }

    public bool IsActive { get; set; }

    public bool Alive
	{
        get { return health.CurrentVal > 0; }
    }

    public Element ElementType
	{
        get { return elementType; }
    }

	public float Speed
    {
		get { return speed; }
		set { speed = value; }
    }

    protected virtual void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        MaxSpeed = Speed;
        health.Initialize();
	}

	protected virtual void Update()
    {
        HandleDebuffs(); 
        Move();
    }

    public void Spawn(int health)
    {
        transform.position = LevelManager.Instance.BluePortal.transform.position;
        this.health.MaxVal = health;
        this.health.CurrentVal = health; 
        //Scales enemies 
        StartCoroutine(Scale(new Vector3(0.25f, 0.25f), new Vector3(1, 1), false));
        SetPath(LevelManager.Instance.Path, false);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime * 1;

            yield return null;
        }

        transform.localScale = to;

        IsActive = true;

        if(remove){
            Release(); 
        }
    }

    private void Move(){
        if (IsActive){
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            if (transform.position == destination){

                if (path != null && path.Count > 0)
                {

                    Gridposition = path.Peek().GridPosition;
                    destination = path.Pop().worldPosition;
                }
                else IsActive = false;
            }
        }

    }

    private void SetPath(Stack<Node> newPath, bool active)
    {
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

    private void Release(){

        debuffs.Clear(); 

        Gridposition = new Point(0, 0);
        Speed = MaxSpeed;
        GameManager.Instance.RemoveEnemy(this);
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        ;
    }

    public void TakeDamage(float damage, Element dmgSource)
	{
        if (IsActive)
		{
            if(dmgSource == ElementType)
			{
                damage = damage / invulnerability;
                invulnerability++; 
            }
            health.CurrentVal -= damage;
            if (health.CurrentVal <= 0)
			{    
                GameManager.Instance.Currency += bounty*EconManager.Instance.BountyWeight;
                GetComponent<SpriteRenderer>().sortingOrder--;
                Invoke("Release", 1);
                IsActive = false;

            }
        }
    }
    public void AddDebuff(Debuff debuff)
	{    
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
        if(newDebuffs.Count > 0)
		{
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear(); 
        }
        foreach(Debuff debuff in debuffsToRemove)
		{
            debuffs.Remove(debuff); 
        }
        debuffsToRemove.Clear(); 
        foreach (Debuff debuff in debuffs )
		{
            debuff.Update(); 
        }
    }
}
