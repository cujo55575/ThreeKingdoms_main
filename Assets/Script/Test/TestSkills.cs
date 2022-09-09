using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSkills : MonoBehaviour
{
    public RectTransform Content;
    public GameObject skillPrefab;
    public TestBattle testBattle;
    public void RefreshSkills(List<SkillData> skills)
    { 
        for(int i=0;i<skills.Count;i++)
        {
            GameObject ins=Instantiate(skillPrefab);
            ins.transform.SetParent(Content.transform, false);
            ins.GetComponent<RawImage>().texture=skills[i].GetSkillTexture();
            int index = i;
            ins.GetComponent<Button>().onClick.AddListener(delegate { TriggerSpell(index);});
            ins.SetActive(true);
        }
    }
    public void TriggerSpell(int index)
    {
        Debug.Log(index);
        testBattle.firstLeader.TriggerEffect(testBattle.skills[index]);
    }
}
