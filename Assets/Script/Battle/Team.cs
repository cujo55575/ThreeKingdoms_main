using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
	public TeamType team;
	public Leader leader;
	public void Awake()
	{
		leader = GetComponent<Leader>();
	}
	public enum TeamType
	{
	TeamOne,
	TeamTwo
	}
}
