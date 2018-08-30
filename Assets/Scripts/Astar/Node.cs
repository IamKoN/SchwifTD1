using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

//Used by the Astar algorithm.
//Each tile has a node connected to it
public class Node {

	public Point GridPosition { get; private set; }

	public Tilescript TileRef { get; private set; }

	public Node Parent { get; private set; }

	public int G { get; set; }

	public int H { get; set; }

	public int F { get; set; }

	public Vector2 worldPosition { get; set; }

    //Node Constructor
	public Node(Tilescript tileRef){
		this.TileRef = tileRef; 
		this.GridPosition = tileRef.GridPosition; 
		this.worldPosition = tileRef.WorldPosition; 
	}

    //Calculate values that are used by Astar
	public void CalcValues(Node parent, Node goal, int gCost){
		this.Parent = parent; 
		this.G = parent.G + gCost; 
		this.H = ((Math.Abs(GridPosition.X - goal.GridPosition.X)) + (Math.Abs(goal.GridPosition.Y - GridPosition.Y))) * 10; 
		this.F = G + H; 
	}
}
