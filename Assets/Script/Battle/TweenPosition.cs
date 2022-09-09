using UnityEngine;
using System.Collections;

public enum E_TweenType
{
    Linear = 0,
    Loop,
    Pingpong
}

public class TweenPosition : MonoBehaviour {

    private bool m_Finish = true;
    private bool m_Front;    
    private float m_GoalTime;
    private float m_StartTime;
    private Vector3 m_NewPos;    

    public Vector3 From;
    public Vector3 To;
    public float Delay = 0;
    public float Duration;
    public bool WorldSpace;
    public bool AutoStart = false;
    public bool Inverse = false;
    public E_TweenType Type = E_TweenType.Linear;
    public System.Action<GameObject> GoalMethod;
    public System.Action<GameObject> FinishMethod;

    private Vector3 m_From
    {
        get
        {
            if (!Inverse)
                return From;
            else
                return To;
        }
    }

    private Vector3 m_To
    {
        get
        {
            if (!Inverse)
                return To;
            else
                return From;
        }
    }


    // Use this for initialization
    void Start () {
        if (AutoStart && m_Finish)
            TweenStart();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if (m_Finish)
            return;
        if (Time.time < m_StartTime)
            return;
        if (Time.time > m_GoalTime)        
            Goal();        
        else
            SetObjectPosition();
    }

    //開始
    public void TweenStart()
    {
        enabled = true;
        m_Finish = false;
        m_Front = true;
        m_StartTime = Time.time + Delay;
        m_GoalTime = m_StartTime + Duration;

        if (WorldSpace)
            gameObject.transform.position = m_From;
        else
            gameObject.transform.localPosition = m_From;
    }
    //結束
    public void TweenFinish()
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

    private void SetObjectPosition()
    {
        float T = Mathf.Clamp01((m_GoalTime - Time.time) / Duration);

        if (m_Front)
        {
            float x = Mathf.Lerp(m_To.x, m_From.x, T);
            float y = Mathf.Lerp(m_To.y, m_From.y, T);
            float z = Mathf.Lerp(m_To.z, m_From.z, T);
            m_NewPos.Set(x, y, z);
        }
        else
        {
            float x = Mathf.Lerp(m_From.x, m_To.x,  T);
            float y = Mathf.Lerp(m_From.y, m_To.y,  T);
            float z = Mathf.Lerp(m_From.z, m_To.z,  T);
            m_NewPos.Set(x, y, z);
        }

        if (WorldSpace)
            gameObject.transform.position = m_NewPos;
        else
            gameObject.transform.localPosition = m_NewPos;
    }

    private void SetObjectGoal()
    {
        if (m_Front)
        {
            if (WorldSpace)
                gameObject.transform.position = m_To;
            else
                gameObject.transform.localPosition = m_To;
        }
        else
        {
            if (WorldSpace)
                gameObject.transform.position = m_From;
            else
                gameObject.transform.localPosition = m_From;
        }

    }



}
