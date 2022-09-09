using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceListData : TableDataBase
{
    public int ID;
    public string Assetbundle;
    public byte AssetbundlePathKind;
    public string ResourceName;

    public override int key()
    {
        return ID;
    }
}

[Serializable]
public class ResourceListDataTable : TableBase<ResourceListData>
{
    Dictionary<string, int> m_Map = new Dictionary<string, int>();

    public override void Init()
    {
        base.Init();            

        for(int i = 0; i < m_Datas.Count; i++)
        {
            ResourceListData Data = m_Datas[i];

            if (!m_Map.ContainsKey(Data.ResourceName))
                m_Map.Add(Data.ResourceName, Data.ID);
            else
				Debug.LogError(string.Format( "duplicate file name {0}" , Data.ResourceName));
        }
    }

    public ResourceListData GetData(string resourceName)
    {
        if (m_Map.ContainsKey(resourceName))            
            return GetData(m_Map[resourceName]);            
        
        return null;                
    }

    public List<string> GetAllNamesByType(byte kind)
    {
        List<string> resources = new List<string>();

        foreach(ResourceListData data in Datas)
        {
            if (kind == data.AssetbundlePathKind)
                resources.Add(data.Assetbundle);
        }

        return resources;
    }

}
