using System;
using System.Collections.Generic;

[Serializable]
public abstract class TableBase<T>where T : TableDataBase
{
    protected Dictionary<int, int> m_Keys = new Dictionary<int, int>();
    protected List<T> m_Datas = new List<T>();

    public List<T> Datas
    {
        get { return m_Datas; }
    }

    public void AddData(T Data)
    {
        if (!(Data is T))
            return;
        if (!m_Keys.ContainsKey(Data.key()))
        {
            m_Keys.Add(Data.key(), m_Datas.Count);
            m_Datas.Add(Data as T);            
        }        
    }

    public virtual void Init()
    {

    }

    public T GetData(int Key)
    {
        if (m_Keys.ContainsKey(Key))
        {
            return m_Datas[m_Keys[Key]];
        }        
        return null;
    }
}

[Serializable]
public abstract class TableDataBase
{    
    public virtual int key() { return -1; }
}