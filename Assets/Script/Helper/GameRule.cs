using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public class GameRule : Singleton<GameRule>
{
    int[,] Round = new int[,] { { 0, 1, 2, 3 }, { 3, 0, 1, 2 }, { 2, 3, 0, 1 }, { 1, 2, 3, 0 } };//Rotation of card (0,90,180,270)  respectively
    List<Grid> BackupGrid = new List<Grid>();

    //****  Debugger Switch  ****//
    private bool ds_CheckGrid = false;            //GenerateCheckGridList
    private bool ds_Restriction = false;          //CheckAvaliableRestriction
    private bool ds_AvaliableRestriction = false; //CheckAvaliableRestriction
    private bool ds_DisableCheck = false; //CheckDisableTile

    public void CalculateAvaliableTile(Vector2Int lastTileIndex,int rotation)
    {
        GenerateCheckGridList(lastTileIndex,rotation);
        CheckAvaliableRestriction(BoardDataManager.Instance.Board.TileList[BoardDataManager.Instance.Turn+1]);
        CheckDisableTile(BoardDataManager.Instance.Turn+1);
    }

    public void ResetDisableTile()
    {
        if (BackupGrid.Count > 0)
        {
            AddToCheckList(BackupGrid[0].BoardIndex);
            for (int i = 1; i < BackupGrid.Count; i++)
            {
                Grid grid = BoardDataManager.Instance.Board.Grid[BackupGrid[i].BoardIndex.x, BackupGrid[i].BoardIndex.y];
                grid.Restriction = BackupGrid[i].Restriction;
                //int count = 0;
                //for (int j = 0; j < 4; j++)
                //{
                //    if (grid.Restriction[j] != E_TerrainType.None)
                //        count++;
                //}
                //if (count <= 1)
                //{ 
                //    RemoveFromCheckList(grid.BoardIndex);
                //}
                
            }
            BackupGrid = new List<Grid>();
        }
    }

    private void GenerateCheckGridList(Vector2Int m_LastTileIndex,int m_rotation,bool m_IsDisableCheck=false)
    {

        Tile currentTile = BoardDataManager.Instance.Board.TileList[BoardDataManager.Instance.Turn];
        Grid currentGrid = BoardDataManager.Instance.Board.Grid[m_LastTileIndex.x, m_LastTileIndex.y];
        Grid topGrid = BoardDataManager.Instance.Board.Grid[m_LastTileIndex.x + 1, m_LastTileIndex.y];
        Grid botGrid = BoardDataManager.Instance.Board.Grid[m_LastTileIndex.x - 1, m_LastTileIndex.y];
        Grid rightGrid = BoardDataManager.Instance.Board.Grid[m_LastTileIndex.x, m_LastTileIndex.y + 1];
        Grid leftGrid = BoardDataManager.Instance.Board.Grid[m_LastTileIndex.x, m_LastTileIndex.y - 1];

        currentGrid.TileIndex = BoardDataManager.Instance.Turn;
        RemoveFromCheckList(m_LastTileIndex);

        Debugger.Log("Rotation "+m_rotation,ds_CheckGrid);
        if (topGrid.TileIndex==(int)E_TileStatus.Free)
        {
            topGrid.Restriction[(int)E_TileDirection.Buttom] = GetRestriction(E_TileDirection.Top, m_rotation, currentTile);
            Debugger.Log("Top"+topGrid.BoardIndex, ds_CheckGrid);
            AddToCheckList(topGrid.BoardIndex);
        }
        if (botGrid.TileIndex == (int)E_TileStatus.Free)
        {
            botGrid.Restriction[(int)E_TileDirection.Top] = GetRestriction(E_TileDirection.Buttom, m_rotation, currentTile);
            Debugger.Log("Bot" + botGrid.BoardIndex, ds_CheckGrid);
            AddToCheckList(botGrid.BoardIndex);
        }
        if (rightGrid.TileIndex == (int)E_TileStatus.Free)
        {
            rightGrid.Restriction[(int)E_TileDirection.Left] = GetRestriction(E_TileDirection.Right, m_rotation, currentTile);
            Debugger.Log("Right" + rightGrid.BoardIndex, ds_CheckGrid);
            AddToCheckList(rightGrid.BoardIndex);
        }
        if (leftGrid.TileIndex == (int)E_TileStatus.Free)
        {
            Debugger.Log("Left" + leftGrid.BoardIndex, ds_CheckGrid);
            leftGrid.Restriction[(int)E_TileDirection.Right] = GetRestriction(E_TileDirection.Left, m_rotation, currentTile);
            AddToCheckList(leftGrid.BoardIndex);
        }
    }

    private E_TerrainType GetRestriction(E_TileDirection tdirection,int rotation,Tile tile)
    {
        
        int direction = (int)tdirection;
        Debugger.Log("Direction "+direction,ds_Restriction);
        switch (rotation)
        {
            case 0://0
                Debugger.Log("Restriction " + tile.Restriction[Round[0, direction]],ds_Restriction);
                return tile.Restriction[Round[0, direction]];
            case 90://1
                Debugger.Log("Restriction " + tile.Restriction[Round[1, direction]], ds_Restriction);
                return tile.Restriction[Round[1, direction]];
            case 180://2
                Debugger.Log("Restriction " + tile.Restriction[Round[2, direction]], ds_Restriction);
                return tile.Restriction[Round[2, direction]];
            case 270://3
                Debugger.Log("Restriction " + tile.Restriction[Round[3, direction]], ds_Restriction);
                return tile.Restriction[Round[3, direction]];
            default:
                break;
        }
        return E_TerrainType.None;
    }

    private void AddToCheckList(Vector2Int gridIndex)
    {
        if (BoardDataManager.Instance.Board.CheckGridList.FindIndex(x => x.GridIndex == gridIndex) < 0)
        {
            BoardDataManager.Instance.Board.CheckGridList.Add(new CheckGrid(gridIndex));
        }
    }

    private void RemoveFromCheckList(Vector2Int gridIndex)
    {
        BoardDataManager.Instance.Board.CheckGridList.Remove(BoardDataManager.Instance.Board.CheckGridList.Find(x => x.GridIndex == gridIndex));
    }

    private void CheckAvaliableRestriction(Tile m_NextTile)
    {
        List<int> PassList = new List<int>();

        Debugger.Log(BoardDataManager.Instance.Turn+"",ds_AvaliableRestriction);
        Debugger.Log(m_NextTile.Restriction[0] + "  " + m_NextTile.Restriction[1] + "  " + m_NextTile.Restriction[2] + "  " + m_NextTile.Restriction[3] + "  ",ds_AvaliableRestriction);
       
        foreach (CheckGrid checkGrid in BoardDataManager.Instance.Board.CheckGridList)
        {
            Grid grid = BoardDataManager.Instance.Board.Grid[checkGrid.GridIndex.x,checkGrid.GridIndex.y];
            checkGrid.AvaliableRotationList = new List<int>();
            checkGrid.Status = E_TileStatus.Free;
            for (int i = 0; i < 4; i++)//  0=0 Degree,1=90 Deg....
            {
                bool isPass = true;
                for (int j = 0; j < 4; j++)
                {
                   
                    if (grid.Restriction[j] != E_TerrainType.None && grid.Restriction[j] != m_NextTile.Restriction[Round[i, j]])
                        isPass = false;
                    Debugger.Log(checkGrid.GridIndex+"(" + j + "," + Round[i, j] + ")", ds_AvaliableRestriction);
                    Debugger.Log(checkGrid.GridIndex +"  "+ grid.Restriction[j] + "   " + m_NextTile.Restriction[Round[i, j]] + "    " + isPass, ds_AvaliableRestriction);
                    
                }
                Debugger.Log("--------------------", ds_AvaliableRestriction);
                if (isPass)
                {
                    checkGrid.Status = E_TileStatus.Avaliable;
                    checkGrid.AvaliableRotationList.Add(i == 0 ? 0 : i == 1 ? 90 : i == 2 ? 180 : 270);
                }
            }
        }
        
    }

    private void CheckDisableTile(int m_lastTileIndex)
    {
        List<CheckGrid> tmpList = BoardDataManager.Instance.Board.CheckGridList.FindAll(x => x.Status != E_TileStatus.NotAvaliable);
        foreach (CheckGrid checkGrid in tmpList)
        {
            Grid grid = BoardDataManager.Instance.Board.Grid[checkGrid.GridIndex.x, checkGrid.GridIndex.y];
            bool isDisable = true;

            for (int index = m_lastTileIndex; index < BoardDataManager.Instance.Board.TileList.Length; index++)
            {
                Tile checkTile = BoardDataManager.Instance.Board.TileList[index];

                for (int i = 0; i < 4; i++)//  0=0 Degree,1=90 Deg....
                {
                    bool isPass = true;
                    for (int j = 0; j < 4; j++)
                    {

                        if (grid.Restriction[j] != E_TerrainType.None && grid.Restriction[j] != checkTile.Restriction[Round[i, j]])
                            isPass = false;
                        Debugger.Log(checkGrid.GridIndex + "(" + j + "," + Round[i, j] + ")", ds_DisableCheck);
                        Debugger.Log(checkGrid.GridIndex + "  " + grid.Restriction[j] + "   " + checkTile.Restriction[Round[i, j]] + "    " + isPass, ds_DisableCheck);
                    }
                    Debugger.Log("--------------------", ds_DisableCheck);
                    if (isPass)
                    {
                        isDisable = false;
                        break;
                    }
                }
                if (!isDisable)
                {
                    break;
                }
            }
            if (isDisable)
            {
                checkGrid.Status = E_TileStatus.NotAvaliable;
            }

        }
    }
}
