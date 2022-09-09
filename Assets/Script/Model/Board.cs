using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	public Grid[,] Grid;
    public Tile[] TileList { get; private set; }
    public  List<CheckGrid> CheckGridList;
	public Vector2Int StartingIndex{ get; private set;}

	public Board(Tile[] tileList)
	{
		//TileList = tileList ?? throw new ArgumentNullException(nameof(tileList));
		Grid = new Grid[TileList.Length,TileList.Length];
		StartingIndex= Vector2Int.one* (tileList.Length / 2);
		CheckGridList = new List<CheckGrid>();
	}
}