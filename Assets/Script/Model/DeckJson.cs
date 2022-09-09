using System.Collections.Generic;

[System.Serializable]
public class DeckJson
{
	public List<CardJson> Cards;

	public DeckJson()
	{
		Cards = new List<CardJson>();
	}
}
