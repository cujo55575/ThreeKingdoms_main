using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	public float speed = 50;
	private TextMesh tm;
	private void Awake()
	{
		tm = GetComponent<TextMesh>();
		Destroy(this.gameObject,3.5f);
		StartCoroutine("Fade");
	}
	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);

		transform.Translate(transform.up*speed*Time.deltaTime);		
	}
	IEnumerator Fade()
	{
		yield return new WaitForSeconds(1.25f);
		float timer = 0;
		while (timer <1)
		{
			
			tm.color=Color.Lerp(tm.color,Color.clear,timer);
			timer += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		tm.color = Color.clear;
		yield return new WaitForSeconds(0f);

	}
	
}
