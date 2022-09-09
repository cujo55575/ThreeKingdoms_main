using System;

[Serializable]
public class CardCountData : TableDataBase
{
	public int ID;
	public int Level1Cost;
	public int Level2Cost;
	public int Level3Cost;
	public int Level4Cost;
	public int Level5Cost;

	public override int key()
	{
		return ID;
	}
	public int GetCostByLevel(int CardLevel)
    {
        switch(CardLevel)
        {
            case 1:
                return Level1Cost;
            case 2:
                return Level2Cost;
            case 3:
                return Level3Cost;
            case 4:
                return Level4Cost;
            case 5:
                return Level5Cost;
            case 6:
                return Level5Cost;
            default:
                return -1;
        }
    }
}

[Serializable]
public class CardCountDataTable : TableBase<CardCountData>
{
}
