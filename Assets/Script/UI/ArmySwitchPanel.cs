using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmySwitchPanel : MonoBehaviour
{
    public Transform viewPort;
    public GameObject ArmyItem;

    public UIBattleCardInfo battleCardInfo;
    private void Awake()
    {
        battleCardInfo = GetComponentInParent<UIBattleCardInfo>();
    }
    private void OnEnable()
    {
        RefreshArmies();
    }
    public void RefreshArmies()
    {
        for(int i=0;i<viewPort.childCount;i++)
        {
            GameObject.Destroy(viewPort.GetChild(i).gameObject);
        }
        if(battleCardInfo.bcm!=null) {
            List<HeroArmyData> armies = battleCardInfo.bcm.data.GetAllArmiesSelf();
            if(armies!=null) {
                for(int i=0;i<armies.Count;i++)
                {
                    if(armies[i].HeroArmyStatusType==(byte)E_EquipableStatus.Locked)
                    {
                        continue;
                    }
                    GameObject ins = Instantiate(ArmyItem);
                    ins.transform.SetParent(viewPort,false);

                    ins.GetComponent<ArmyItem>().RefreshData(battleCardInfo.bcm.data,i);

                    ins.gameObject.SetActive(true);
                }
            }
        }
    }
}
