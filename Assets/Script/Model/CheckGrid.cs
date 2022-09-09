using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrid
{
    public Vector2Int GridIndex;
    public List<int> AvaliableRotationList;
    public E_TileStatus Status;

    public CheckGrid(Vector2Int gridIndex)
    {
        GridIndex = gridIndex;
        AvaliableRotationList = new List<int>();
        Status = E_TileStatus.Free;
    }
}
