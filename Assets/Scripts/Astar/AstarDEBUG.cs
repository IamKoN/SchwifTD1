using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarDEBUG : MonoBehaviour {

	[SerializeField]
	private Tilescript start, goal; 

	[SerializeField]
	private GameObject arrowPrefab;

	[SerializeField]
	private GameObject debugTilePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	//Update is called once per frame
	//void Update () {
	//	ClickTile (); 
	//	if (Input.GetKeyDown (KeyCode.Space)) {
	//		Astar.getPath (start.GridPosition, goal.GridPosition); 
	//	}
	//}

	private void ClickTile(){
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero); 

			if (hit.collider != null) {
				Tilescript tmp = hit.collider.GetComponent<Tilescript> (); 

				if (tmp != null) {
					if (start == null) {
						start = tmp;
						CreateDebugTile (start.WorldPosition, new Color32 (255, 135, 0, 255));
					} 
					else if (goal == null) {
						goal = tmp; 
						CreateDebugTile (goal.WorldPosition, new Color32 (255, 0, 0, 255));
					}
				}
			}

		}
	}

	public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path){
		foreach (Node node in openList) {

			if(node.TileRef != start && node.TileRef != goal){
				CreateDebugTile (node.TileRef.WorldPosition, Color.yellow, node); 
			}

			Point2Parent (node, node.TileRef.WorldPosition); 
		}
		foreach (Node node in closedList) {

			if(node.TileRef != start && node.TileRef != goal && !path.Contains(node)){
				CreateDebugTile (node.TileRef.WorldPosition, Color.grey, node); 
			}

			Point2Parent (node, node.TileRef.WorldPosition); 
		}
		foreach (Node node in path) {
			if (node.TileRef != start && node.TileRef != goal) {
				CreateDebugTile (node.TileRef.WorldPosition, Color.green, node);
			}
		}
	}


	private void Point2Parent(Node node, Vector2 position){
		

		if (node.Parent != null) {
			GameObject arrow = (GameObject)Instantiate (arrowPrefab, position, Quaternion.identity); 
			arrow.GetComponent<SpriteRenderer> ().sortingOrder = 2; 
			//point to the right
			if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 0); 
			}
			//point to the top right
			else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 45); 
			}
			//point up
			else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 90); 
			}
			//point to the top left 
			else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 135); 
			}
			//point to the left 
			else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 180); 
			}
			//point to the bottom left
			else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 225); 
			}
			//point below
			else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 270); 
			}
			//point below
			else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y) {
				arrow.transform.eulerAngles = new Vector3 (0, 0, 315); 
			}
		}
			
	}

	private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null){
		GameObject debugTile = (GameObject)Instantiate (debugTilePrefab,worldPos,Quaternion.identity); 
		debugTile.transform.position = worldPos; 
		if (node != null) {
			DebugTile tmp = debugTile.GetComponent<DebugTile> (); 

            tmp.G.text += node.G; 
            tmp.H.text += node.H; 
            tmp.F.text += node.F; 
		}
		debugTile.GetComponent<SpriteRenderer> ().color = color; 
	}
}
