using UnityEngine;
using System.Collections;

public class TweenRotation : MonoBehaviour {

    private bool m_Finish = true;
    private bool m_Front;
    private float m_GoalTime;
    private Vector3 m_NewPos;

    public Vector3 From;
    public Vector3 To;
    public float Duration;
    public bool WorldSpace;
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

    void FixedUpdate()
    {
        if (m_Finish)
            return;
        if (Time.time > m_GoalTime)
            Goal();
        else
            SetObjectRotation();
    }

    //開始
    public void TweenStart()
    {
        enabled = true;
        m_Finish = false;
        m_Front = true;
        //m_StartTime = Time.time;
        m_GoalTime = Time.time + Duration;

        if (WorldSpace)
            gameObject.transform.eulerAngles = From;
        else
            gameObject.transform.localEulerAngles = From;
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

    private void SetObjectRotation()
    {
        float T = Mathf.Clamp01((m_GoalTime - Time.time) / Duration);

        if (m_Front)
        {
            float x = Mathf.Lerp(To.x, From.x, T);
            float y = Mathf.Lerp(To.y, From.y, T);
            float z = Mathf.Lerp(To.z, From.z, T);
            m_NewPos.Set(x, y, z);
        }
        else
        {
            float x = Mathf.Lerp(From.x, To.x, T);
            float y = Mathf.Lerp(From.y, To.y, T);
            float z = Mathf.Lerp(From.z, To.z, T);
            m_NewPos.Set(x, y, z);
        }

        if (WorldSpace)
            gameObject.transform.eulerAngles = m_NewPos;
        else
            gameObject.transform.localEulerAngles = m_NewPos;
    }

    private void SetObjectGoal()
    {
        if (m_Front)
        {
            if (WorldSpace)
                gameObject.transform.eulerAngles = To;
            else
                gameObject.transform.localEulerAngles = To;
        }
        else
        {
            if (WorldSpace)
                gameObject.transform.eulerAngles = From;
            else
                gameObject.transform.localEulerAngles = From;
        }

    }

}
