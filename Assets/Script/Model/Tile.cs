using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile 
{
    public int TileID;
    public int Rotation;
    public Texture Texture;
	public GameObject Model;
    public E_TerrainType[] Restriction;
    public HeroPosition HeroPosition;

    public Tile()
    {
        TileID = -1;
        Rotation = 0;
        Restriction = new E_TerrainType[4];
        HeroPosition = new HeroPosition();
    }
}
[Serializable]
public class HeroPosition
{
    public bool TopLeft;
    public bool TopCenter;
    public bool TopRight;
    public bool CenterRight;
    public bool BottomRight;
    public bool BottomCenter;
    public bool BottomLeft;
    public bool CenterLeft;
    public bool CenterCenter;
}