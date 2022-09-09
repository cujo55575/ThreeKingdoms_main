using UnityEngine;
using UnityEngine.UI;
public class ClickOn : MonoBehaviour
{
    public Transform parent;
    public GameObject Lock;
    public GameObject armyUnlockLevel;
    public TweenScale Scaler;
    private HeroArmyData m_Data;

    private GameObject model;
    void Update()
    {
        transform.forward = Vector3.forward;
        transform.rotation = Quaternion.Euler(-30, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    Vector3 rotation;
    public void InstantiateArmyObject(HeroArmyData data)
    {
        model = data.GetArmyData().GetModelObject();
        model = Instantiate(model, transform.position, Quaternion.identity);
        model.transform.SetParent(parent, false);
        model.transform.localRotation = Quaternion.Euler(-15, 135, 15);
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = Vector3.one * 1.1f;
        Refresh(data);
    }

    public void Refresh(HeroArmyData data)
    {
        m_Data = data;
        armyUnlockLevel.GetComponentInChildren<Text>().text = string.Format("Lv.{0}", data.ArmyLevel.ToString());
        bool IsLock = E_EquipableStatus.Locked == (E_EquipableStatus)data.HeroArmyStatusType;
        Lock.SetActive(IsLock);
        if (IsLock)
        {
            model.GetComponent<AlternativeMaterials>().ChangeShaderWithType(E_ShaderType.ModelGreyScale);
        }
        else
        {
            model.GetComponent<AlternativeMaterials>().ChangeShaderWithType(E_ShaderType.Normal);
        }
        armyUnlockLevel.SetActive(!IsLock);
    }

    public void DecreaseModelSizeToNormal()
    {
        // Scaler.From = model.transform.localScale;
        // Scaler.To = Vector3.one * 1.1f;
        // Scaler.Duration = 0.3f;
        // Scaler.TweenStart();
    }

    public void IncreaseModelSize()
    {
        // Scaler.From = model.transform.localScale;
        // Scaler.To = Vector3.one * 1.3f;
        // Scaler.Duration = 0.3f;
        // Scaler.TweenStart();
    }

}
