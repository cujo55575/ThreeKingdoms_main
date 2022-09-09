using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public float Damage;
    public Team.TeamType team;
    private BattleManager battleManager;
    private void Start()
    {
        battleManager = GameObject.FindObjectOfType<BattleManager>();
    }
    public void StartFlame(Team.TeamType teamType,float dmg,float duration)
    {
        Damage = dmg;
        team = teamType;
        GameObject.Destroy(this.gameObject,duration);
        InvokeRepeating("DPS",0f,1f);
    }
    void DPS()
    {
        List<Leader> leaders;
        if(team==Team.TeamType.TeamOne)
        {
            leaders=battleManager.teamTwo;
        }
        else
        {
            leaders = battleManager.teamOne;
        }
        for(int i=0;i<leaders.Count;i++)
        {
            if(Vector3.Distance(transform.position,leaders[i].GetCeneterPoint())<=100)
            {
                leaders[i].Hit(Damage);
            }
        }
    }
}
