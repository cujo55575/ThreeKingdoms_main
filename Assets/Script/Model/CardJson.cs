
[System.Serializable]
public class CardJson
{
	public string Name;
	public string TextureName;
	public int RareUp;
	public int Atk;
	public int Def;
	public bool IsUsed;

	public float PosX;
	public float PosY;
	public float Scale;

	public CardJson()
	{
		Name = string.Empty;
		TextureName = string.Empty;
		RareUp = 1;
		Atk = 1;
		Def = 1;
		IsUsed = false;
	}
}
