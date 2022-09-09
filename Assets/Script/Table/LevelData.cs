using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class LevelData : TableDataBase
{
    public int ID;
    public string MapName;
    public int ActStringID;
    public int ActInfoID;
    public int ActNameID;
    public int DynastyID;
    public int LevelNameID;
    public int LevelInfoID;
    public int EnergyCost;
    public int Npc1ID;
    public int Npc1PosX;
    public int Npc1PosY;
    public int Npc2ID;
    public int Npc2PosX;
    public int Npc2PosY;
    public int Npc3ID;
    public int Npc3PosX;
    public int Npc3PosY;
    public int Npc4ID;
    public int Npc4PosX;
    public int Npc4PosY;
    public int Npc5ID;
    public int Npc5PosX;
    public int Npc5PosY;
    public int Exp;
    public int BambooRoll;
    public int BambooFragment;
    public int Coin;
    public int Dialogue1ID;
    public int Dialogue2ID;
    public int Dialogue3ID;
    public int Dialogue4ID;
    public int Dialogue5ID;
    public int NextLvlID;

    public override int key()
    {
        return ID;
    }

    public string GetActString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(ActStringID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + ActStringID);
        }
        return result;
    }

    public string GetActInfoString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(ActInfoID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + ActInfoID);
        }
        return result;
    }

    public string GetActNameString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(ActNameID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + ActNameID);
        }
        return result;
    }

    public string GetDynastyString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(DynastyID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + DynastyID);
        }
        return result;
    }

    public string LevelNameString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(LevelNameID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + LevelNameID);
        }
        return result;
    }

    public string GetLevelInfoString()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(LevelInfoID);
        if (string.IsNullOrEmpty(result))
        {
            Debug.LogError("String Cannot be found for ID = " + LevelInfoID);
        }
        return result;
    }

    public List<DialogueData> GetAllDialogues()
    {
        List<DialogueData> result = new List<DialogueData>();
        DialogueData Dialogue1 = TableManager.Instance.DialogueDataTable.GetData(Dialogue1ID);
        DialogueData Dialogue2 = TableManager.Instance.DialogueDataTable.GetData(Dialogue2ID);
        DialogueData Dialogue3 = TableManager.Instance.DialogueDataTable.GetData(Dialogue3ID);
        DialogueData Dialogue4 = TableManager.Instance.DialogueDataTable.GetData(Dialogue4ID);
        DialogueData Dialogue5 = TableManager.Instance.DialogueDataTable.GetData(Dialogue5ID);

        if (Dialogue1 != null)
        {
            result.Add(Dialogue1);
        }

        if (Dialogue2 != null)
        {
            result.Add(Dialogue2);
        }

        if (Dialogue3 != null)
        {
            result.Add(Dialogue3);
        }

        if (Dialogue4 != null)
        {
            result.Add(Dialogue4);
        }

        if (Dialogue5 != null)
        {
            result.Add(Dialogue5);
        }


        return result;
    }
}

[Serializable]
public class LevelDataTable : TableBase<LevelData>
{
}
