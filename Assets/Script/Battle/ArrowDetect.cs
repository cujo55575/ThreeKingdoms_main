using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetect : MonoBehaviour
{
	public Team.TeamType MyTeam;
	private void OnTriggerEnter(Collider other)
	{
		if (other.name != "Arrow")
		{
			Unit unit = other.GetComponent<Unit>();
			if (unit != null)
			{
				if (unit.Leader.team.team != MyTeam)
				{
					Destroy(this.gameObject);
				}
			}	
		}	
	}
}
