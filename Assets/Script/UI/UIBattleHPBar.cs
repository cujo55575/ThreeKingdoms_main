using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleHPBar : UIBase
{
    public GameObject HeroHPStatusPrefab;
    public Transform ScrollView;
    public RectTransform scrollRect;

    public RectTransform BorderIcon;

    private bool tweening=false;

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);

        List<Leader> leaders = (List<Leader>)Objects[0];
        Debug.Log("leaders.Count="+leaders.Count);

        CreateHPCells(leaders);

        tweening = false;
        scrollRect=ScrollView.GetComponent<RectTransform>();
        scrollRect.anchoredPosition = new Vector2(-100,-110);
        CancelInvoke();
        Invoke("TweenStart",2.0f);
    }
    public void TweenStart()
    {
        tweening = true;
    }
    public void CreateHPCells(List<Leader> datas)
    {
        for(int i=0;i<ScrollView.childCount;i++)
        {
            Destroy(ScrollView.GetChild(i).gameObject);
        }
        for(int i=0;i<datas.Count;i++)
        {
            GameObject ins = Instantiate(HeroHPStatusPrefab);
            ins.transform.SetParent(ScrollView, false);
            ins.SetActive(true);
            ins.GetComponent<UIHeroHPStatus>().InitData(datas[i],this);
        }
        
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(tweening)
        {
            scrollRect.anchoredPosition = Vector2.Lerp(scrollRect.anchoredPosition,new Vector2(-100,110),Time.deltaTime*5);
        }
    }
}
public class HeroHPStatusData
{

}
