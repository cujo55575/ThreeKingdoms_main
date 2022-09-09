using System;

public class Singleton<T>
{
  protected static readonly T m_instance = Activator.CreateInstance<T>();
  protected static Action m_Notices;
  protected bool m_hasInit = false;
  public static T Instance { get { return m_instance; } }

    protected Singleton() { }

  public virtual void Initialize()
  {
    m_hasInit = true;
  }
  public virtual void Uninitialize()
  {
    m_hasInit = false;
  }

    public virtual void RegisterNotices(Action notice)
    {
        m_Notices += notice;
    }
    public virtual void UnregisterNotices(Action notice)
    {
        m_Notices -= notice;
    }
}
