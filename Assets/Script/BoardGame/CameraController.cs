using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera OrthoCamera;

	public bool IsTopDownCamera=false;
	public float LastDistance=-1.0f;
	public bool InitControl=false;
	public bool SystemControl=false;
	public Transform Target;
	public Team.TeamType TargetTeam;

	public List<Leader> teamOne;
	public List<Leader> teamTwo;
	public GameObject SwipeScreeen;
    private SwipeScreen m_SwipeScript;

    private AudioSource ac;

    public Vector3 OwnBattleFieldPos;
    public Vector3 EnemyBattleFieldPos;

    public Vector3 LerpPos;

    public void ChangeToEnemyBattleField()
    {
        LerpPos = EnemyBattleFieldPos;
    }
    public void BackToOwnBattleField()
    {
        LerpPos = OwnBattleFieldPos;
    }
    private void Start()
    {
        ac = GetComponent<AudioSource>();
        m_SwipeScript = SwipeScreeen.GetComponent<SwipeScreen>();
        LerpPos = OwnBattleFieldPos;
    }
    public void StartControl()
	{
		InitControl = false;
		SystemControl = false;
		SwipeScreeen.SetActive(true);
	}
	public void Init()
	{
		teamOne = new List<Leader>();
		teamTwo = new List<Leader>();

		Leader[] leaders = GameObject.FindObjectsOfType<Leader>();

		for (int i = 0; i < leaders.Length; i++)
		{
			if (leaders[i].team.team == Team.TeamType.TeamOne)
			{
				teamOne.Add(leaders[i]);
			}
			else
			{
				teamTwo.Add(leaders[i]);
			}
		}
        Invoke("StartInitCam", 1f);

        ac.enabled = true;
        ac.Play();
	}
    void StartInitCam()
    {
        InitControl = true;
    }
	private Vector3 GetCenterOfTargets()
	{
		Vector3 center = Vector3.zero;

		for (int i = 0; i < teamOne.Count; i++)
		{
			center += teamOne[i].transform.position;
		}
		center = center / teamOne.Count;

        center = new Vector3(center.x, 0, center.z);

        return center;
	}
    private Vector3 GetCenterOfTargetsAll()
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < teamOne.Count; i++)
        {
            center += teamOne[i].transform.position;
        }
        for (int i = 0; i < teamTwo.Count; i++)
        {
            center += teamTwo[i].transform.position;
        }
        center = center / (teamOne.Count+teamTwo.Count);

        center = new Vector3(center.x,0,center.z);

        return center;
    }
    private void Update()
	{
        ac.enabled = InitControl;
		if (InitControl)
		{
			if (Target != null)
			{
				Target.transform.position = GetCenterOfTargets();
				float height =100f;
				transform.position = Vector3.Lerp(transform.position,Target.position + new Vector3(120,height,-120),Time.deltaTime*0.5f);
			}
            Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(20, -45, 0), Time.deltaTime*0.5f);
            return;
            
        }
		else if (SystemControl)
		{
			if (Target != null)
			{
				transform.position = Vector3.Lerp(transform.position,Target.position + new Vector3(150,170f,-150),Time.deltaTime*10);
			}
            Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(45, -45, 0), Time.deltaTime);
            return; 
        }
       else if(!m_SwipeScript.gameObject.activeSelf)
        {
            transform.position = Vector3.Lerp(transform.position, LerpPos, Time.deltaTime*10f);
			return;
        }
		 else
		 {
			if (m_SwipeScript.PointerID == -10000)
			{
				if (Target != null)
				{
					Target.transform.position = GetCenterOfTargetsAll();
					float height = Mathf.Lerp(transform.position.y,170f,Time.deltaTime * 10);
					transform.position = Vector3.Lerp(transform.position,Target.position + new Vector3(120,height,-120),Time.deltaTime*2f);
				}
				Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation,Quaternion.Euler(45,-45,0),Time.deltaTime);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x,170f,transform.position.z),Time.deltaTime);
				Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation,Quaternion.Euler(45,-45,0),Time.deltaTime);
			}
			
		 }
		/*   //OrthoCamera.orthographicSize -= Input.mouseScrollDelta.y*Time.deltaTime*250;
		   Vector3 offset= OrthoCamera.transform.forward * Input.mouseScrollDelta.y * Time.deltaTime * 500;
		   Vector3 futurePos= transform.position+offset;

		   if(futurePos.y>m_SwipeScript.Height.x && futurePos.y<m_SwipeScript.Height.y)
		   {
				  transform.position = futurePos;
		   }
		   m_SwipeScript.Clamp();

		   if (Input.touchCount == 2)
		   {
			   Touch t1 = Input.touches[0];
			   Touch t2 = Input.touches[1];
			   if (LastDistance > 0)
			   {
				   float increment = Vector2.Distance(t1.position,t2.position) - LastDistance;
				   OrthoCamera.orthographicSize +=increment;
			   }
			   LastDistance = Vector2.Distance(t1.position,t2.position);
		   }
		   else
		   {
			   LastDistance = -1.0f;
		   }

		   //OrthoCamera.orthographicSize = Mathf.Clamp(OrthoCamera.orthographicSize,clamp.x,clamp.y);*/
	}

	public void ChangeCamera()
	{
		IsTopDownCamera = !IsTopDownCamera;

		if (IsTopDownCamera)
		{
			OrthoCamera.transform.rotation = Quaternion.Euler(90,0,0);
			OrthoCamera.transform.localPosition = new Vector3(0,100,0);
		}
		else
		{
			OrthoCamera.transform.rotation = Quaternion.Euler(45,45,0);
			OrthoCamera.transform.localPosition = new Vector3(-70,100,-70);
		}
	}
}
