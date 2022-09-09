using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattle : MonoBehaviour
{
    public Leader firstLeader;
    public Leader secondLeader;
    // Start is called before the first frame update
    public List<SkillData> skills;
    public TestSkills TestSkill;
    public int index = 0;
    void Start()
    {
        skills=TableManager.Instance.SkillDataTable.Datas;
        TestSkill.RefreshSkills(skills);
        Invoke("StartGame",2.0f);
    }
    public void StartGame()
    {
        Leader[] leaders=GameObject.FindObjectsOfType<Leader>();
        for(int i=0;i<leaders.Length;i++)
        {
            leaders[i].GameStart();
        }
    }
}
