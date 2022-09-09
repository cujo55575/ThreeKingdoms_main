using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsHandPositioner : MonoBehaviour
{
	public RectTransform StartTransform;
	public RectTransform EndTransform;
	public float YPeak = 5f;
	public int Count = 7;

	public GameObject Prefab;

	private void Update()
	{

	}

	private void Start()
	{
		Vector2 startPos = new Vector2(StartTransform.localPosition.x,StartTransform.localPosition.y);
		Vector2 endPos = new Vector2(EndTransform.localPosition.x,EndTransform.localPosition.y);

		List<Vector2> results = GetPositions(startPos,endPos,YPeak,Count);
		for (int i = 0; i < results.Count; i++)
		{
			GameObject go = GameObject.Instantiate(Prefab,this.transform);
			go.transform.localPosition = new Vector3(results[i].x,results[i].y,0);
		}
	}
	public List<Vector2> GetPositions(Vector2 startPosition, Vector2 endPosition, float yPeak, int count)
	{
		List<Vector2> resultList = new List<Vector2>();

		float xSpan = (endPosition.x - startPosition.x) / (count - 1);

		int columnCount = (count / 2) + (count % 2);
		float ySpan = (endPosition.y - startPosition.y + yPeak) / columnCount;

		for (int i = 0; i < count; i++)
		{
			float x = startPosition.x + (xSpan * i);
			float y = startPosition.y + (ySpan * i);
			if (i > columnCount - 1)
			{
				y = startPosition.y + (ySpan * (count - i - 1));
			}
		
			Vector2 position = new Vector2(x,y);
			resultList.Add(position);
		}
		
		return resultList;
	}
}
