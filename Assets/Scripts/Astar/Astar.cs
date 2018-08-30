using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 

//Contains all Astar functionality
//Returns a walkable path for the enemies
public static class Astar
{ 
    //Contains all nodes in the grid
	private static Dictionary<Point, Node> nodes; 

    //Creates all nodes in the grid
	private static void CreateNodes(){
		nodes = new Dictionary<Point, Node> ();
		foreach (Tilescript tile in LevelManager.Instance.Tiles.Values) {
                //Creates node based on tile
                nodes.Add (tile.GridPosition, new Node (tile));  
		}
	}

	//Returns a walkable path
	public static Stack<Node> GetPath(Point start, Point goal){
        //If there are no nodes we create them
		if (nodes == null) {
			CreateNodes (); 
		}

        //creates OpenList of nodes
		HashSet<Node> openList = new HashSet<Node>();

        //Creates CLosedList of nodes
		HashSet<Node> closedList = new HashSet<Node>();

		Stack<Node> finalPath = new Stack<Node> (); 

        //sets current node at start node.
		Node currentNode = nodes [start]; 

        //adds current node to open list
		openList.Add (currentNode); 

        //as long as onpenlist has nodes we keep searching for path
		while (openList.Count > 0) {
			for(int x = -1; x <= 1; x++){
				for (int y = -1; y <= 1; y++) {
                    //Stores pos of current neighbor node
					Point neighborPos = new Point (currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y); 
					//Node neighbor = nodes [new Point(neighborPos.X, neighborPos.Y)]; 
					if (LevelManager.Instance.InBounds(neighborPos) &&  neighborPos != start && LevelManager.Instance.Tiles [neighborPos].IsEmpty && neighborPos != currentNode.GridPosition) {
                        //store reference to node
						Node neighbor = nodes [new Point(neighborPos.X, neighborPos.Y)]; 

						int gCost = 0; 

                        //if the node is veritcal or horizontal in reference to current node
						if(Math.Abs(x-y) == 1){
							gCost = 10; 
						}
						else{ //diagonal move

							if (!ConnectedDiagonally (currentNode, nodes[neighborPos])) {
								continue;
							}
							gCost = 14; 

						}
						//Node neighbor = nodes [neighborPos]; 
						//Only for debuggin 
						//neighbor.TileRef.SpriteRenderer.color = Color.black;

						if (openList.Contains (neighbor)) {
							if (currentNode.G + gCost < neighbor.G) {
								neighbor.CalcValues (currentNode, nodes[goal], gCost); 
							}
						}
						else if (!closedList.Contains(neighbor)){
							openList.Add (neighbor); 
							neighbor.CalcValues (currentNode, nodes[goal], gCost); 
						}
					}
				}
			}
            //current node removed from the OpenList
			openList.Remove (currentNode);

            //Current node added to closed list
			closedList.Add (currentNode); 

			if (openList.Count > 0) {
				//sorts the list by the f value and selects the first one
				currentNode = openList.OrderBy (n => n.F).First (); 
			}

            //if current node is the goal, path found
			if (currentNode == nodes [goal]) {

                //creates stack to hold final path
				finalPath = new Stack<Node>(); 

                //adds nodes to final path
				while (currentNode.GridPosition != start) {
                    //add current node to final path
					finalPath.Push (currentNode); 
					currentNode = currentNode.Parent; 
				}
				//returns complete path
				return finalPath; 
			}
		}
	    //if path not found returns null
		return null; 
		//This is only for debugging, remove later 
		//GameObject.Find("Debugger").GetComponent<AstarDEBUG>().debugPath(openList, closedList, finalPath); 
	}

    //Determines if two nodes are connected diagnolly without blocking the path
	private static bool ConnectedDiagonally(Node currentNode, Node neighbor){
        //gets direction
		Point direction = currentNode.GridPosition - neighbor.GridPosition; 

		Point first = new Point (currentNode.GridPosition.X + (direction.X * -1), currentNode.GridPosition.Y);
		Point second = new Point (currentNode.GridPosition.X, currentNode.GridPosition.Y + (direction.Y * -1)); 

        //Checks if both nodes are empty
		if (LevelManager.Instance.InBounds (first) && !LevelManager.Instance.Tiles [first].IsEmpty) {
			return false; 
		}

		if (LevelManager.Instance.InBounds (second) && !LevelManager.Instance.Tiles [second].IsEmpty) {
			return false; 
		} 
        //Nodes are empty
		return true; 
	}
}
