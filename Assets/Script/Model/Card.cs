using System;
using UnityEngine;

[Serializable]
public class Card
{
	public string CardName;
	public Sprite CardTexture;
	public int RareUp;
	public int Atk;
	public int Def;
	public bool IsUsed;

	public float PosX;
	public float PosY;
	public float Scale;

    public E_CardType Type;
    public E_KingdomType KDType;
    public int TotalAmount;

	public Card()
	{
		RareUp = 1;
		Atk = 1;
		Def = 1;
		IsUsed = false;    
	}
}
