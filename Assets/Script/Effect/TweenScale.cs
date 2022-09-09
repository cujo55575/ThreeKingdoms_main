using UnityEngine;

public class TweenScale : MonoBehaviour
{

	private bool m_Finish = true;
	private bool m_Front;
	private float m_GoalTime;
    private float m_StartTime;
    private Vector3 m_NewPos;

	public Vector3 From;
	public Vector3 To;
    public float Delay = 0;
    public float Duration;
	public bool AutoStart = false;
	public E_TweenType Type = E_TweenType.Linear;
	public System.Action<GameObject> GoalMethod;
	public System.Action<GameObject> FinishMethod;


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
        if (Time.time < m_StartTime)
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
		m_StartTime = Time.time+Delay;
		m_GoalTime = m_StartTime + Duration;
		gameObject.transform.localScale = From;
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
			float x = Mathf.Lerp(To.x,From.x,T);
			float y = Mathf.Lerp(To.y,From.y,T);
			float z = Mathf.Lerp(To.z,From.z,T);
			m_NewPos.Set(x,y,z);
		}
		else
		{
			float x = Mathf.Lerp(From.x,To.x,T);
			float y = Mathf.Lerp(From.y,To.y,T);
			float z = Mathf.Lerp(From.z,To.z,T);
			m_NewPos.Set(x,y,z);
		}
		gameObject.transform.localScale = m_NewPos;
	}

	private void SetObjectGoal()
	{
		if (m_Front)
		{
			gameObject.transform.localScale = To;
		}
		else
		{
			gameObject.transform.localScale = From;
		}

	}
}
