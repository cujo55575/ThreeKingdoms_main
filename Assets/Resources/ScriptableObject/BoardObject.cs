using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardObject : ScriptableObject
{
    public Grid[,] Grid;
    public TileObject[] TileList;
    public List<Grid> CheckGridList;
}
