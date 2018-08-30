using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

//script for all tiles
public class Tilescript : MonoBehaviour
{
    //Tiles position on grid
    public Point GridPosition { get; private set; }

    //Is tile empty
    public bool IsEmpty { get; set; }

    private Tower myTower;

    private SpriteRenderer spriteRenderer;

    //Is tile walkable or not
    public bool WalkAble { get; set; }

    public bool Debugging { get; set; }

    private Color32 fullColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    //The tiles world position
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    // Use this for initialization
    void Start()
    {
        IsEmpty = true;
        WalkAble = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEmpty)
        {
            ColorTile(Color.white);
        }
        else
        {
            ColorTile(Color.grey);
        }
    }
    
    /// <param name="gridPosition">The tile's grid position</param>
    /// <param name="worldPosition">The tile's world position</param>
    /// <param name="parent">The tiles parent</param>
    public void Setup(Point gridPosition, Vector3 worldPosition, int tileIndex, Transform parent)
    {
        //Sets the values
        this.GridPosition = gridPosition;
        transform.position = worldPosition;
        transform.SetParent(parent);
        //if (tileIndex == 4) walkAble = true;      //If Sand tile, allow walking

        //Adds the tile to the levelmanager
        LevelManager.Instance.Tiles.Add(GridPosition, this);
    }

    //When mouse is over a tile
    public void OnMouseOver()
    {
        //If mouse isn't hitting a gameObject and no button has been clicked
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            //Tile empty
            if (IsEmpty)
            {
                ColorTile(emptyColor);
            }
            if (!LevelManager.Instance.CheckPath(GridPosition) || GridPosition == LevelManager.Instance.RedSpawn)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (!GameManager.Instance.WaveActive)
                    //Places the tower
                    PlaceTower();
            }
        }
        //If we click a tile
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0) && myTower != null)
            {
                //Selected tower
                GameManager.Instance.SelectTower(myTower);
            }
            if (Input.GetMouseButtonDown(0) && myTower == null)
            {
                //If an empty tile has been clicked, deselect it
                GameManager.Instance.DeselectTower();
            }
        }
    }

    //When mouse exits from a tile
    private void OnMouseExit()
    {
        Update();
    }

    //Places tower on a tile
    private void PlaceTower()
    {
        //Creates tower
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        //Creates reference to the tower
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        //releases the tower after placed
        //Removes the hover icon
        Hover.Instance.Deactive(); 

        //tile is now not empty
		IsEmpty = false; 
		ColorTile (Color.white); 

        //Enemies can no longer walk through this tile
		WalkAble = false;

        //can only place one tower per click
        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        //Buys tower
		GameManager.Instance.BuyTower();
	}

	private void ColorTile(Color newColor)
    {
		spriteRenderer.color = newColor; 
	}
}
