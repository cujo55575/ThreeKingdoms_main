using System;
[Serializable]
public class AttributeLinkData : TableDataBase
{
	public int ID;
	public int Level1AttributeTableID;
	public int Level2AttributeTableID;
	public int Level3AttributeTableID;
	public int Level4AttributeTableID;
	public int Level5AttributeTableID;
	public int Level6AttributeTableID;
	public int Level7AttributeTableID;
	public int Level8AttributeTableID;
	public int Level9AttributeTableID;
	public int Level10AttributeTableID;
	public int Level11AttributeTableID;
	public int Level12AttributeTableID;
	public int Level13AttributeTableID;
	public int Level14AttributeTableID;
	public int Level15AttributeTableID;
	public int Level16AttributeTableID;
	public int Level17AttributeTableID;
	public int Level18AttributeTableID;
	public int Level19AttributeTableID;
	public int Level20AttributeTableID;
	public int Level21AttributeTableID;
	public int Level22AttributeTableID;
	public int Level23AttributeTableID;
	public int Level24AttributeTableID;
	public int Level25AttributeTableID;
	public int Level26AttributeTableID;
	public int Level27AttributeTableID;
	public int Level28AttributeTableID;
	public int Level29AttributeTableID;
	public int Level30AttributeTableID;
	public int Level31AttributeTableID;
	public int Level32AttributeTableID;
	public int Level33AttributeTableID;
	public int Level34AttributeTableID;
	public int Level35AttributeTableID;
	public int Level36AttributeTableID;
	public int Level37AttributeTableID;
	public int Level38AttributeTableID;
	public int Level39AttributeTableID;
	public int Level40AttributeTableID;
	public int Level41AttributeTableID;
	public int Level42AttributeTableID;
	public int Level43AttributeTableID;
	public int Level44AttributeTableID;
	public int Level45AttributeTableID;
	public int Level46AttributeTableID;
	public int Level47AttributeTableID;
	public int Level48AttributeTableID;
	public int Level49AttributeTableID;
	public int Level50AttributeTableID;
	public int Level51AttributeTableID;
	public int Level52AttributeTableID;
	public int Level53AttributeTableID;
	public int Level54AttributeTableID;
	public int Level55AttributeTableID;
	public int Level56AttributeTableID;
	public int Level57AttributeTableID;
	public int Level58AttributeTableID;
	public int Level59AttributeTableID;
	public int Level60AttributeTableID;
	public int Level61AttributeTableID;
	public int Level62AttributeTableID;
	public int Level63AttributeTableID;
	public int Level64AttributeTableID;
	public int Level65AttributeTableID;
	public int Level66AttributeTableID;
	public int Level67AttributeTableID;
	public int Level68AttributeTableID;
	public int Level69AttributeTableID;
	public int Level70AttributeTableID;
	public int Level71AttributeTableID;
	public int Level72AttributeTableID;
	public int Level73AttributeTableID;
	public int Level74AttributeTableID;
	public int Level75AttributeTableID;
	public int Level76AttributeTableID;
	public int Level77AttributeTableID;
	public int Level78AttributeTableID;
	public int Level79AttributeTableID;
	public int Level80AttributeTableID;
	public int Level81AttributeTableID;
	public int Level82AttributeTableID;
	public int Level83AttributeTableID;
	public int Level84AttributeTableID;
	public int Level85AttributeTableID;
	public int Level86AttributeTableID;
	public int Level87AttributeTableID;
	public int Level88AttributeTableID;
	public int Level89AttributeTableID;
	public int Level90AttributeTableID;
	public int Level91AttributeTableID;
	public int Level92AttributeTableID;
	public int Level93AttributeTableID;
	public int Level94AttributeTableID;
	public int Level95AttributeTableID;
	public int Level96AttributeTableID;
	public int Level97AttributeTableID;
	public int Level98AttributeTableID;
	public int Level99AttributeTableID;

	public override int key()
	{
		return ID;
	}

	public AttributeData GetAttribute(int level)
	{
		int attributeTableID = -1;
		switch (level)
		{
			case 1: 
			attributeTableID = Level1AttributeTableID;
			break;
			case 2:
			attributeTableID = Level2AttributeTableID;
			break;
			case 3:
			attributeTableID = Level3AttributeTableID;
			break;
			case 4:
			attributeTableID = Level4AttributeTableID;
			break;
			case 5:
			attributeTableID = Level5AttributeTableID;
			break;
			case 6:
			attributeTableID = Level6AttributeTableID;
			break;
			case 7:
			attributeTableID = Level7AttributeTableID;
			break;
			case 8:
			attributeTableID = Level8AttributeTableID;
			break;
			case 9:
			attributeTableID = Level9AttributeTableID;
			break;
			case 10:
			attributeTableID = Level10AttributeTableID;
			break;
			case 11:
			attributeTableID = Level11AttributeTableID;
			break;
			case 12:
			attributeTableID = Level12AttributeTableID;
			break;
			case 13:
			attributeTableID = Level13AttributeTableID;
			break;
			case 14:
			attributeTableID = Level14AttributeTableID;
			break;
			case 15:
			attributeTableID = Level15AttributeTableID;
			break;
			case 16:
			attributeTableID = Level16AttributeTableID;
			break;
			case 17:
			attributeTableID = Level17AttributeTableID;
			break;
			case 18:
			attributeTableID = Level18AttributeTableID;
			break;
			case 19:
			attributeTableID = Level19AttributeTableID;
			break;
			case 20:
			attributeTableID = Level20AttributeTableID;
			break;
			case 21:
			attributeTableID = Level21AttributeTableID;
			break;
			case 22:
			attributeTableID = Level22AttributeTableID;
			break;
			case 23:
			attributeTableID = Level23AttributeTableID;
			break;
			case 24:
			attributeTableID = Level24AttributeTableID;
			break;
			case 25:
			attributeTableID = Level25AttributeTableID;
			break;
			case 26:
			attributeTableID = Level26AttributeTableID;
			break;
			case 27:
			attributeTableID = Level27AttributeTableID;
			break;
			case 28:
			attributeTableID = Level28AttributeTableID;
			break;
			case 29:
			attributeTableID = Level29AttributeTableID;
			break;
			case 30:
			attributeTableID = Level30AttributeTableID;
			break;
			case 31:
			attributeTableID = Level31AttributeTableID;
			break;
			case 32:
			attributeTableID = Level32AttributeTableID;
			break;
			case 33:
			attributeTableID = Level33AttributeTableID;
			break;
			case 34:
			attributeTableID = Level34AttributeTableID;
			break;
			case 35:
			attributeTableID = Level35AttributeTableID;
			break;
			case 36:
			attributeTableID = Level36AttributeTableID;
			break;
			case 37:
			attributeTableID = Level37AttributeTableID;
			break;
			case 38:
			attributeTableID = Level38AttributeTableID;
			break;
			case 39:
			attributeTableID = Level39AttributeTableID;
			break;
			case 40:
			attributeTableID = Level40AttributeTableID;
			break;
			case 41:
			attributeTableID = Level41AttributeTableID;
			break;
			case 42:
			attributeTableID = Level42AttributeTableID;
			break;
			case 43:
			attributeTableID = Level43AttributeTableID;
			break;
			case 44:
			attributeTableID = Level44AttributeTableID;
			break;
			case 45:
			attributeTableID = Level45AttributeTableID;
			break;
			case 46:
			attributeTableID = Level46AttributeTableID;
			break;
			case 47:
			attributeTableID = Level47AttributeTableID;
			break;
			case 48:
			attributeTableID = Level48AttributeTableID;
			break;
			case 49:
			attributeTableID = Level49AttributeTableID;
			break;
			case 50:
			attributeTableID = Level50AttributeTableID;
			break;
			case 51:
			attributeTableID = Level51AttributeTableID;
			break;
			case 52:
			attributeTableID = Level52AttributeTableID;
			break;
			case 53:
			attributeTableID = Level53AttributeTableID;
			break;
			case 54:
			attributeTableID = Level54AttributeTableID;
			break;
			case 55:
			attributeTableID = Level55AttributeTableID;
			break;
			case 56:
			attributeTableID = Level56AttributeTableID;
			break;
			case 57:
			attributeTableID = Level57AttributeTableID;
			break;
			case 58:
			attributeTableID = Level58AttributeTableID;
			break;
			case 59:
			attributeTableID = Level59AttributeTableID;
			break;
			case 60:
			attributeTableID = Level60AttributeTableID;
			break;
			case 61:
			attributeTableID = Level61AttributeTableID;
			break;
			case 62:
			attributeTableID = Level62AttributeTableID;
			break;
			case 63:
			attributeTableID = Level63AttributeTableID;
			break;
			case 64:
			attributeTableID = Level64AttributeTableID;
			break;
			case 65:
			attributeTableID = Level65AttributeTableID;
			break;
			case 66:
			attributeTableID = Level66AttributeTableID;
			break;
			case 67:
			attributeTableID = Level67AttributeTableID;
			break;
			case 68:
			attributeTableID = Level68AttributeTableID;
			break;
			case 69:
			attributeTableID = Level69AttributeTableID;
			break;
			case 70:
			attributeTableID = Level70AttributeTableID;
			break;
			case 71:
			attributeTableID = Level71AttributeTableID;
			break;
			case 72:
			attributeTableID = Level72AttributeTableID;
			break;
			case 73:
			attributeTableID = Level73AttributeTableID;
			break;
			case 74:
			attributeTableID = Level74AttributeTableID;
			break;
			case 75:
			attributeTableID = Level75AttributeTableID;
			break;
			case 76:
			attributeTableID = Level76AttributeTableID;
			break;
			case 77:
			attributeTableID = Level77AttributeTableID;
			break;
			case 78:
			attributeTableID = Level78AttributeTableID;
			break;
			case 79:
			attributeTableID = Level79AttributeTableID;
			break;
			case 80:
			attributeTableID = Level80AttributeTableID;
			break;
			case 81:
			attributeTableID = Level81AttributeTableID;
			break;
			case 82:
			attributeTableID = Level82AttributeTableID;
			break;
			case 83:
			attributeTableID = Level83AttributeTableID;
			break;
			case 84:
			attributeTableID = Level84AttributeTableID;
			break;
			case 85:
			attributeTableID = Level85AttributeTableID;
			break;
			case 86:
			attributeTableID = Level86AttributeTableID;
			break;
			case 87:
			attributeTableID = Level87AttributeTableID;
			break;
			case 88:
			attributeTableID = Level88AttributeTableID;
			break;
			case 89:
			attributeTableID = Level89AttributeTableID;
			break;
			case 90:
			attributeTableID = Level90AttributeTableID;
			break;
			case 91:
			attributeTableID = Level91AttributeTableID;
			break;
			case 92:
			attributeTableID = Level92AttributeTableID;
			break;
			case 93:
			attributeTableID = Level93AttributeTableID;
			break;
			case 94:
			attributeTableID = Level94AttributeTableID;
			break;
			case 95:
			attributeTableID = Level95AttributeTableID;
			break;
			case 96:
			attributeTableID = Level96AttributeTableID;
			break;
			case 97:
			attributeTableID = Level97AttributeTableID;
			break;
			case 98:
			attributeTableID = Level98AttributeTableID;
			break;
			case 99:
			attributeTableID = Level99AttributeTableID;
			break;
			default:
			attributeTableID = Level11AttributeTableID;
			break;

		}

		return TableManager.Instance.AttributeDataTable.GetData(attributeTableID);
	}
}

[Serializable]
public class AttributeLinkDataTable : TableBase<AttributeLinkData>
{
}
