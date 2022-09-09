using UnityEngine;
using UnityEngine.UI;

public class TweenColor : MonoBehaviour
{

	private bool m_Finish = true;
	private bool m_Front;
	private float m_GoalTime;
	private Color m_NewPos;
	private Graphic m_Color;

	public Color From;
	public Color To;
	public float Duration;
	public E_TweenType Type = E_TweenType.Linear;
	public bool AutoStart = false;
	public System.Action<GameObject> GoalMethod;
	public System.Action<GameObject> FinishMethod;


	void Awake()
	{
		m_Color = GetComponent<Graphic>();
	}

	// Use this for initialization
	void Start()
	{
		if (AutoStart && m_Finish)
			TweenStart();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void LateUpdate()
	{
		if (m_Finish)
			return;
		if (Time.time > m_GoalTime)
			Goal();
		else
			SetObjectScale();
	}

	//開始
	public void TweenStart()
	{
		enabled = true;
		m_Finish = false;
		m_Front = true;
		//m_StartTime = Time.time;
		m_GoalTime = Time.time + Duration;
		m_Color.color = From;
	}

	//結束
	private void TweenFinish()
	{
		m_Finish = true;
		enabled = false;
		if (FinishMethod != null)
			FinishMethod(gameObject);
	}

	//抵達目標
	private void Goal()
	{
		SetObjectGoal();
		if (GoalMethod != null)
			GoalMethod(gameObject);
		switch (Type)
		{
			case E_TweenType.Linear:
			TweenFinish();
			break;
			case E_TweenType.Loop:
			TweenStart();
			break;
			case E_TweenType.Pingpong:
			m_Front = !m_Front;
			m_GoalTime = Time.time + Duration;
			break;
		}
	}

	private void SetObjectScale()
	{
		float T = Mathf.Clamp01((m_GoalTime - Time.time) / Duration);

		if (m_Front)
		{
			m_NewPos.r = Mathf.Lerp(To.r,From.r,T);
			m_NewPos.g = Mathf.Lerp(To.g,From.g,T);
			m_NewPos.b = Mathf.Lerp(To.b,From.b,T);
			m_NewPos.a = Mathf.Lerp(To.a,From.a,T);

		}
		else
		{
			m_NewPos.r = Mathf.Lerp(From.r,To.r,T);
			m_NewPos.g = Mathf.Lerp(From.g,To.g,T);
			m_NewPos.b = Mathf.Lerp(From.b,To.b,T);
			m_NewPos.a = Mathf.Lerp(From.a,To.a,T);

		}
		m_Color.color = m_NewPos;
	}

	private void SetObjectGoal()
	{
		if (m_Front)
		{
			m_Color.color = To;
		}
		else
		{
			m_Color.color = From;
		}

	}
}
