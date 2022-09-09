using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public string SFXName;
	public Leader Leader;
	private Animator anim;

	public Transform FollowTarget;
	public Transform AttackTarget;

	private float timer;
	private NavMeshAgent agent;


    public GameObject StunObject;

	bool warmed=false;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();

		agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.radius = 0.2f;
		agent.autoBraking = false;

        //CreatingStunObject
       GameObject stunObj=Resources.Load("Prefab/StunVFX") as GameObject;
       GameObject ins=Instantiate(stunObj);
        ins.layer = transform.gameObject.layer;

        ins.transform.SetParent(transform,false);
        ins.transform.localPosition = new Vector3(0,2,0);
        ins.transform.localScale = new Vector3(0.6f,1,0.6f);

        StunObject = ins;
        StunObject.SetActive(false);
	}
	void Start()
    {
		timer = 0;
		if (Leader.team.team == Team.TeamType.TeamOne)
		{
			//GetComponentInChildren<SkinnedMeshRenderer>().material = GetComponent<AlternativeMaterials>().TeamOneMaterial;
		}
		else
		{
			//GetComponentInChildren<SkinnedMeshRenderer>().material = GetComponent<AlternativeMaterials>().TeamTwoMaterial;
		}
        anim.SetFloat(Leader.SpeedParam,Leader.TotalSpeed/50f);
    }
	// Update is called once per frame
	void Update()
    {
		if (Leader != null)
		{
            if(!Leader.GameAlreadyStarted)
            {
                transform.position = new Vector3(FollowTarget.position.x,transform.position.y,FollowTarget.position.z);
            }
			anim.enabled = !Leader.Pause;

			if (Leader.IsKnocking)
            {
                if(agent.enabled)
                {
                    AttackTarget = null;
                    warmed = false;
                    agent.angularSpeed = 0;
                    anim.SetBool(Leader.AttackingParam, false);
                    anim.SetBool(Leader.MovingParam, false);
					transform.position = FollowTarget.transform.position;
                }   
                return;
            }
            if(Leader.IsRooting)
            {
                warmed = false;
                agent.speed = 0;
            }
            else if(Leader.IsStunning)
            {
                warmed = false;
                anim.SetBool(Leader.AttackingParam, false);
                anim.SetBool(Leader.MovingParam, false);
                agent.speed = 0;
                StunObject.SetActive(true);
                return;
            }
            else
            {
                agent.speed=Leader.TotalSpeed;
				StunObject.SetActive(false);
            }
            agent.angularSpeed = Leader.m_AgentAngularSpeed;
            if (AttackTarget != null)
			{
				if (!AttackTarget.GetComponent<NavMeshAgent>().enabled)
				{
					AttackTarget = null;
					anim.SetBool(Leader.AttackingParam,false);
				}
			}
			if (!agent.enabled)
			{
				anim.SetBool(Leader.MovingParam,false);
				anim.SetBool(Leader.AttackingParam,false);
				return;
			}

			if (Leader.IsMoving && !Leader.IsAttacking)
			{
				agent.Warp(transform.position);
				if (!Leader.Pause)
				{
					transform.position = Vector3.Lerp(transform.position,FollowTarget.position,Time.deltaTime * 5f);
					transform.rotation = Quaternion.Lerp(transform.rotation,Leader.transform.rotation,Time.deltaTime);
				}	
				if (Leader.IsRooting)
                {
                    anim.SetBool(Leader.MovingParam,false);
                }
                else
                {
                    anim.SetBool(Leader.MovingParam, true);
                }
				
				anim.SetBool(Leader.AttackingParam,false);
			}
			else if (Leader.IsAttacking)
			{
				if (AttackTarget == null)
				{
					if (Leader.Target == null)
					{
						return;
					}
					List<GameObject> targets= Leader.Target.GetComponent<Leader>().formator.Units;
					if (targets.Count != 0)
					{
						int rnd = Random.Range(0,targets.Count);
						AttackTarget = targets[rnd].transform;
					}
					
				}
				if (AttackTarget != null)
				{		
					if (Vector3.Distance(transform.position,AttackTarget.position) <= Leader.TotalRange)
					{
                        warmed = true;
                        agent.SetDestination(transform.position);
                        //agent.Warp(transform.position);
						anim.SetBool(Leader.AttackingParam, true);
						transform.LookAt(AttackTarget.position);
                        anim.SetBool(Leader.MovingParam, false);
                    }
					else
					{
                        warmed = false;
                        agent.SetDestination(AttackTarget.position);
                        //agent.Warp(AttackTarget.position);
						anim.SetBool(Leader.AttackingParam,false);
                        if (Leader.IsRooting)
                        {
                            anim.SetBool(Leader.MovingParam, false);
                        }
                        else
                        {
                            anim.SetBool(Leader.MovingParam, true);
                        }
                    }
				}
			}
			else
			{
				//agent.SetDestination(transform.position);
				agent.Warp(transform.position);
				anim.SetBool(Leader.MovingParam,false);
			}
		}
        if(Leader.Pause)
        {
            agent.SetDestination(transform.position);
            //agent.Warp(transform.position);
        }
	}
    public void DispelTarget()
    {
        AttackTarget = null;
    }
	public void Attack()
	{

	}
	public void Die()
	{
		anim.SetBool("IsAlive",false);

		StartCoroutine("SelfDestroy");
		agent.enabled = false;
	}
    public void Revive()
    {
        gameObject.SetActive(true);
        anim.CrossFade("Idle",0.01f);
        transform.position = FollowTarget.position;
        anim.SetBool("IsAlive",true);

        StopCoroutine("SelfDestroy");
        agent.enabled = true;
    }

	IEnumerator SelfDestroy()
	{
		bool b = true;
		float time = 0.0f;
		while (b)
		{
			if (!Leader.Pause || !Leader.gameObject.activeSelf)
			{
				time += 0.1f;
			}
			if (time >= 3)
			{
				b = false;
				gameObject.SetActive(false);
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.0f);
	}
}
