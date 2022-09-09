using UnityEngine;

[CreateAssetMenu]
public class CardObject : ScriptableObject
{
	public Card Card;

	public CardObject()
	{
		Card = new Card();
	}
}
