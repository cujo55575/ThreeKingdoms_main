using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
	public GameObject Arrow;
	public Transform ShootPoint;
	public Team.TeamType MyTeam;

	private Unit unit;
	private void Start()
	{
		unit = GetComponent<Unit>();
		MyTeam = unit.Leader.team.team;
	}
	public void Shoot()
	{
		GameObject ins = Instantiate(Arrow);
		ins.transform.position = ShootPoint.position;

		ins.transform.rotation = transform.rotation;
		ins.GetComponentInChildren<ArrowDetect>().MyTeam=MyTeam;

		if (unit.Leader.SecondTarget != null)
		{
			ins = Instantiate(Arrow);
			ins.transform.position = ShootPoint.position;

			Vector3 directiion = (unit.Leader.SecondTarget.position - unit.Leader.transform.position).normalized;
			ins.transform.forward = directiion.normalized;
			ins.GetComponentInChildren<ArrowDetect>().MyTeam = MyTeam;
		}

		if (unit.Leader.ThirdTarget != null)
		{
			ins = Instantiate(Arrow);
			ins.transform.position = ShootPoint.position;

			Vector3 directiion= (unit.Leader.ThirdTarget.position - unit.Leader.transform.position).normalized;
			ins.transform.forward = directiion.normalized;
			ins.GetComponentInChildren<ArrowDetect>().MyTeam = MyTeam;
		}
	}
}
