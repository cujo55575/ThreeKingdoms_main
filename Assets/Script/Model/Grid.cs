using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Grid : ICloneable
{
    public int TileIndex;
    public Vector2 Position;
	public Vector2Int BoardIndex;
    public E_TerrainType[] Restriction;
    public int Rotation;

    public Grid()
    {
        TileIndex = -1;
        Rotation = 0;
        Restriction = new E_TerrainType[4];
    }

    public Grid(int tileIndex, Vector2 position, Vector2Int boardIndex, E_TerrainType[] restriction, int rotation)
    {
        TileIndex = tileIndex;
        Position = position;
        BoardIndex = boardIndex;
        Restriction = restriction;
        Rotation = rotation;
    }

    public object Clone()
    {
        return new Grid(TileIndex, Position, BoardIndex, Restriction, Rotation);
    }

}


