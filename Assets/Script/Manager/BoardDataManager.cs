using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using XEResources;

public class BoardDataManager : Singleton<BoardDataManager>
{
    public Board Board { get; set; }
    public int Turn;
	public DeckObject Player1Deck;
	public DeckObject Player2Deck;
	public override void Initialize()
	{
        base.Initialize();
        BoardObject boardobj = Resources.Load<BoardObject>("ScriptableObject/BoardScriptableObject");
		Tile[] Tiles = new Tile[boardobj.TileList.Length];
		for (int i = 0; i < boardobj.TileList.Length; i++)
		{
			Tiles[i] = boardobj.TileList[i].Tile;
		}
		Board = new Board(Tiles);
        Turn = 0;

		StreamReader reader = new StreamReader("Assets\\Resources\\Player1Deck.txt");
		string JsonString = reader.ReadToEnd();
		reader.Close();
		DeckJson m_Player1Deck = JsonUtility.FromJson<DeckJson>(JsonString);

		List<CardObject> cards = new List<CardObject>();
		for (int i = 0; i < m_Player1Deck.Cards.Count; i++)
		{
			CardJson cJson = m_Player1Deck.Cards[i];
			CardObject card=GLOBALFUNCTION.ToCardFromCardJson(cJson);

			cards.Add(card);
		}

		Player1Deck = new DeckObject();
		Player1Deck.Deck = new Deck();
		Player1Deck.Deck.Cards = cards;

		/*Player1Deck = ResourceManager.Instance.Load<DeckObject>(
		string.Format(GLOBALCONST.FORMAT_DECKSPATH,GLOBALCONST.DECK_1));*/

		Player2Deck = ResourceManager.Instance.Load<DeckObject>(
		string.Format(GLOBALCONST.FORMAT_DECKSPATH,GLOBALCONST.DECK_2));

	}
	public override void RegisterNotices(Action notice)
	{
		base.RegisterNotices(notice);
	}

	public override void Uninitialize()
	{
		base.Uninitialize();
	}

	public override void UnregisterNotices(Action notice)
	{
		base.UnregisterNotices(notice);
	}
}
