using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour {

	public float ScrollSpeedX = 0.5f;
	public float ScrollSpeedY = 0.5f;

    [HideInInspector] float timeScroll = 0;

	void Update ()
	{
        
            timeScroll += Time.deltaTime;

            float offset_x = timeScroll*ScrollSpeedX;
            float offset_y = timeScroll*ScrollSpeedY;

            GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (offset_x,offset_y);
        
	}
}
