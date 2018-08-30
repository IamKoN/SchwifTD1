using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic; 

//Class that handles the level generation
public class LevelManager : Singleton<LevelManager>
{
    //Location of blue and red spawn
    private Point blueSpawn, redSpawn;

    private Point mapSize;

    private Stack<Node> fullPath;

    //Array of tile prefabs
    [SerializeField]
	private GameObject[] tilePrefabs;  

    [SerializeField]
	private GameObject bluePortalPrefab;

	[SerializeField]
	private GameObject redPortalPrefab;
    
	[SerializeField]
	private CameraMovement cameraMovement; 

	[SerializeField]
	private Transform map; 

    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        }
    }

    public Point BlueSpawn
    {
        get { return blueSpawn; }
    }

    public Point RedSpawn
    {
        get { return redSpawn; }
    }

    public Portal BluePortal { get; set; }

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

	void Start ()	// Use this for initialization
    {
		CreateLevel ();
	}

	void Update () // Update is called once per frame
    {	
	}

    private void CreateLevel()
    {
		Tiles = new Dictionary<Point, Tilescript> (); 

        //generates map
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
        SetCamera();
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
        //Loads text asset from resources
		TextAsset bindData = Resources.Load ("Level") as TextAsset;
		string data = bindData.text.Replace (Environment.NewLine, string.Empty); 

        //Splits string into an array
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

    //Checks if position is in bounds of map
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
        float cameraX = 9.22f;
        float cameraY = -2.07f;
        Camera.main.transform.position = new Vector3(cameraX, cameraY, -10f);
        Camera.main.orthographicSize = 11.27585f;
    }
}
