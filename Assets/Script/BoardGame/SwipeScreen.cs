using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeScreen : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
	public Transform CameraHolder;
	public Camera cam;
	public Transform directionObj;
	public int PointerID = -10000;
	private CameraController CameraController;

	void Start()
	{
		CameraController = CameraHolder.GetComponent<CameraController>();
	}
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (PointerID == -10000)
		{
			PointerID = eventData.pointerId;
		}
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if (PointerID == eventData.pointerId)
		{
			PointerID = -10000;
		}
	}
	public void OnDrag(PointerEventData data)
	{
		if (CameraController.SystemControl)
		{
			return;
		}
		if (PointerID != data.pointerId)
		{
			return;
		}

		Vector3 direction = new Vector3(data.delta.x,0,data.delta.y) * -1;
		directionObj.forward = direction;
		directionObj.transform.Rotate(new Vector3(0,cam.transform.rotation.eulerAngles.y,0));
		CameraHolder.Translate(directionObj.transform.forward * direction.magnitude * Time.deltaTime * cam.transform.position.y * 0.1f);
		CameraHolder.transform.position =
		new Vector3
		(
		Mathf.Clamp(CameraHolder.transform.position.x,Horizontal.x - 20,Horizontal.y + 20),
		CameraHolder.transform.position.y,
		Mathf.Clamp(CameraHolder.transform.position.z,Vertical.x - 20,Vertical.y + 20)
		);
	}
	public Vector2 Vertical;
	public Vector2 Horizontal;
	public Vector2 Height;
	public void UpdateClampSize(Vector3 pos)
	{
		if (pos.x < Horizontal.x)
		{
			Horizontal = new Vector2(pos.x,Horizontal.y);
		}
		if (pos.x > Horizontal.y)
		{
			Horizontal = new Vector2(Horizontal.x,pos.x);
		}
		if (pos.z < Vertical.x)
		{
			Vertical = new Vector2(pos.z,Vertical.y);
		}
		if (pos.z > Vertical.y)
		{
			Vertical = new Vector2(Vertical.x,pos.z);
		}
	}

	void Update()
	{
        Clamp();
	}
    public void Clamp()
    {
        CameraHolder.transform.position =
           new Vector3
           (
               Mathf.Clamp(CameraHolder.transform.position.x, Horizontal.x - 20, Horizontal.y + 20),
               Mathf.Clamp(CameraHolder.transform.position.y, Height.x, Height.y),
               Mathf.Clamp(CameraHolder.transform.position.z, Vertical.x - 20, Vertical.y + 20)
           );

    }

}
