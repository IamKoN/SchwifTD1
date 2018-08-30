using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

//struct Point{
//	public int x, y;
//}

public class LevelManager : Singleton<LevelManager> {

	[SerializeField]
	private GameObject[] tilePrefabs;  

	private Point blueSpawn, redSpawn; 

    float cameraX = 17.68f;

    float cameraY = -7.37f;

    public Point BlueSpawn
    {
        get{ return blueSpawn; }
    }
    public Point RedSpawn
    {
        get { return redSpawn; }
    }

    /// <summary>
    /// A bool that indicates if we are loading or not
    /// </summary>
    //public bool Loading { get; private set; }

    [SerializeField]
	private GameObject bluePortalPrefab;

	[SerializeField]
	private GameObject redPortalPrefab;

    /// <summary>
    /// A reference to the loading screen
    /// </summary>
    //[SerializeField]
    //private Canvas loadScreen;

    public Portal BluePortal { get; set; }

	//public Portal redPortal { get; set; }

	[SerializeField]
	private CameraMovement cameraMovement; 

	[SerializeField]
	private Transform map; 

	private Point mapSize;

	private Stack<Node> fullPath; 

	public Stack<Node> Path
	{
		get
        { 
			if (fullPath == null)
            {
				GeneratePath(); 
				//path = Astar.getPath(blueSpawn, redSpawn); 
			}
			//makes sure each enemy has its own stack of nodes so that it can pop as it pleases 
			return new Stack<Node> (new Stack<Node> (fullPath)); 
		}
	}

	public Dictionary<Point, Tilescript> Tiles { get; set; } 

	public float TileSize
    {
		get{ return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * tilePrefabs[0].GetComponent<Transform>().localScale.x; }
	}
	// Use this for initialization
	void Start ()
    {
		CreateLevel ();
	}
	// Update is called once per frame
	void Update ()
    {	
	}
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        }
    }
    private void CreateLevel()
    {
		Tiles = new Dictionary<Point, Tilescript> (); 
		String[] mapData = new string[]
          {
              "111111111111111111111111",
              "441444144441111111111111",
              "141414141141111111111111",
              "141414141141111111111111",
              "141414141141111111111111",
              "141414141141111111111111",
              "144414441144111111111111",
              "111111111111111111111111",
              "111111111111111111111111",
              "111111111111111111111111",
              "111111111111111111111111",
              "111111111111111111111111",
        };
        //Sets the map size based on the map data
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData [0].ToCharArray ().Length; 
		int mapY = mapData.Length;

		Vector3 maxTile = Vector3.zero;
		Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
		for (int y = 0; y < mapSize.Y; y++)
        {
			char[] horizontalTiles = mapData[y].ToCharArray();				
			for (int x = 0; x < mapSize.X; x++)
            {
				PlaceTile (horizontalTiles[x].ToString(), x, y, worldStart); 
			}
		}
		maxTile = Tiles [new Point (mapX - 1, mapY - 1)].transform.position; 
		cameraMovement.SetLimits (new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
		SpawnPortals ();
        //set camera 
        Camera.main.transform.position = new Vector3(cameraX, cameraY, -10f); 
        //Removes the loading screen
        //loadScreen.enabled = false;
    }

	private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
		int tileIndex = int.Parse (tileType); 
		Tilescript newTile = Instantiate (tilePrefabs[tileIndex]).GetComponent<Tilescript>();
        newTile.Setup(new Point(x, y), new Vector3(WorldStartPos.x + (TileSize * x), WorldStartPos.y - (TileSize * y), 0), tileIndex, map);

        //Tiles.Add (new Point (x, y), newTile); 

    }
    private string[] ReadLevelText()
    {
		TextAsset bindData = Resources.Load ("Level") as TextAsset;
		string data = bindData.text.Replace (Environment.NewLine, string.Empty); 
		return data.Split('-');
	}
	private void SpawnPortals()
    {
		blueSpawn = new Point (0, 1); 
		GameObject tmp = (GameObject)Instantiate (bluePortalPrefab, Tiles[blueSpawn].GetComponent<Tilescript>().WorldPosition, Quaternion.identity);
		BluePortal = tmp.GetComponent<Portal>(); 
		BluePortal.name = "bluePortal"; 

		redSpawn = new Point (22, 10); //15,6
		Instantiate (redPortalPrefab, Tiles[redSpawn].GetComponent<Tilescript>().WorldPosition, Quaternion.identity);
		//redPortal = tmp.GetComponent<Portal> (); 
		//redPortal.name = "RedPortal"; 
	}
	public bool InBounds(Point position)
    {
		return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
	}
	public void GeneratePath()
    {
		fullPath = Astar.GetPath (blueSpawn, redSpawn); 
	}
    public bool CheckPath(Point checkPoint)
    {
        //A temp reference to the clicked tile
        Tilescript tmp = Tiles[checkPoint];
        if (tmp.IsEmpty) //If the tile is empty then we can move there
        {
            tmp.IsEmpty = false;

            if (Astar.GetPath(RedSpawn, blueSpawn) == null) //If there isn't an available path between the tiles
            {
                tmp.IsEmpty = true;
                return false;
            }
            else //If there is an available path
            {
                tmp.IsEmpty = true;
                return true;
            }

        }
        else //If the tile isn't empty
        {
            return false;
        }
    }
    private void SetCamera()
    {
    }
}
