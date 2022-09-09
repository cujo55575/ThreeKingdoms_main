using System;
using System.Collections.Generic;

[Serializable]
public class Deck
{
	public List<CardObject> Cards;

	public Deck()
	{
		Cards = new List<CardObject>();
	}
}