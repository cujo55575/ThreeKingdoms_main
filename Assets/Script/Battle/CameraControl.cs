using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public Transform NewCameraTransform;
	public float MoveDelay;
	public float MoveDuration;

	public float RotateDelay;
	public float RotateDuration;

	private TweenPosition m_PositionControl;
	private TweenRotation m_RotationControl;


    // Start is called before the first frame update
    void Start()
    {
		m_PositionControl = GetComponent<TweenPosition>();
		if (m_PositionControl == null)
		{
			m_PositionControl = gameObject.AddComponent<TweenPosition>();
		}

		m_RotationControl = GetComponent<TweenRotation>();
		if (m_RotationControl == null)
		{
			m_RotationControl = gameObject.AddComponent<TweenRotation>();
		}
		m_PositionControl.From = transform.localPosition;
		m_PositionControl.To = NewCameraTransform.localPosition;
		m_PositionControl.Duration = MoveDuration;
		m_PositionControl.Delay = MoveDelay;
		m_PositionControl.TweenStart();

		m_RotationControl.From = transform.localEulerAngles;
		m_RotationControl.To = transform.localEulerAngles;
		m_RotationControl.Duration = RotateDuration;
		m_RotationControl.TweenStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
