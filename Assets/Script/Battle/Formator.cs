using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	public float offset;

	public GameObject Place;
	public Leader Leader;
	public GameObject Unit;

	public List<GameObject> Units;
	public List<GameObject> Deads;
	public Formation SquadFormation = Formation.FourByFour;

	public enum Formation
	{
		FourByFour,
		FourThreeFour,
	};

	private void Awake()
	{
		Leader = GetComponentInParent<Leader>();
		Deads =new List<GameObject>();
	}
	private void Start()
	{
		Units = new List<GameObject>();
		switch (SquadFormation)
		{
			case Formation.FourByFour:
			MakeFourByFourSquad();
			break;
			case Formation.FourThreeFour:
			MakeFourThreeFourSquad();
			break;
			default:
			MakeFourByFourSquad();
			break;
		}
	}

	private void MakeFourByFourSquad()
	{
		for (int i = 0; i < 4; i++)
		{
			float StartX =1.5f* offset * -1;
			float StartY = (1.5f-i) * offset;
			MakeSquadRow(4,StartX,StartY);
		}
	}

	private void MakeFourThreeFourSquad()
	{
		for (int i = 0; i < 3; i++)
		{
			if (i == 1)
			{
				float StartX = 1f * offset * -1;
				float StartY = (1-i) * offset;
				MakeSquadRow(3,StartX,StartY);
			}
			else
			{
				float StartX = 1.5f * offset * -1;
				float StartY = (1-i) * offset;
				MakeSquadRow(4,StartX,StartY);
			}
		}
	}

	private void MakeSquadRow(int RowCount, float StartX, float YPos)
	{
		for (int j = 0; j < RowCount; j++)
		{
			GameObject place = Instantiate(Place,transform.position,Quaternion.identity);
			float XPos = StartX + (j * offset);

			place.transform.SetParent(transform,true);

			place.transform.localPosition = new Vector3(XPos,0,YPos);
			place.transform.localRotation = Quaternion.Euler(0,0,0);

			GameObject ins = Instantiate(Unit,transform.position,Quaternion.identity);
			ins.transform.position = place.transform.position;
			ins.transform.rotation = place.transform.rotation;

			ins.transform.SetParent(Leader.transform,true);

			Unit unit = ins.GetComponent<Unit>();
			unit.Leader = Leader;
			unit.FollowTarget = place.transform;

            unit.GetComponent<AlternativeMaterials>().ChangeSkin(Leader.ArmyModelString);

			Units.Add(ins);
		}
	}
    public List<string> GetAttackSFXNames()
    {
        List<string> Names = new List<string>();
        for(int i=0;i<Units.Count;i++)
        {
            string sfxName = Units[i].GetComponent<Unit>().SFXName;
            if(!Names.Contains(sfxName))
            {
                Names.Add(sfxName);
            }
        }
        return Names;
    }
    public void ChangeShader()
    {
        for(int i=0;i<Units.Count;i++)
        {
            Units[i].GetComponent<AlternativeMaterials>().ChangeShader();
        }
    }
}
