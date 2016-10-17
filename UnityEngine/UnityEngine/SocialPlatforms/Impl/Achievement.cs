// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Impl.Achievement
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms.Impl
{
  public class Achievement : IAchievement
  {
    private bool m_Completed;
    private bool m_Hidden;
    private DateTime m_LastReportedDate;

    public string id { get; set; }

    public double percentCompleted { get; set; }

    public bool completed
    {
      get
      {
        return this.m_Completed;
      }
    }

    public bool hidden
    {
      get
      {
        return this.m_Hidden;
      }
    }

    public DateTime lastReportedDate
    {
      get
      {
        return this.m_LastReportedDate;
      }
    }

    public Achievement(string id, double percentCompleted, bool completed, bool hidden, DateTime lastReportedDate)
    {
      this.id = id;
      this.percentCompleted = percentCompleted;
      this.m_Completed = completed;
      this.m_Hidden = hidden;
      this.m_LastReportedDate = lastReportedDate;
    }

    public Achievement(string id, double percent)
    {
      this.id = id;
      this.percentCompleted = percent;
      this.m_Hidden = false;
      this.m_Completed = false;
      this.m_LastReportedDate = DateTime.MinValue;
    }

    public Achievement()
      : this("unknown", 0.0)
    {
    }

    public override string ToString()
    {
      return this.id + " - " + (object) this.percentCompleted + " - " + (object) this.completed + " - " + (object) this.hidden + " - " + (object) this.lastReportedDate;
    }

    public void ReportProgress(Action<bool> callback)
    {
      ActivePlatform.Instance.ReportProgress(this.id, this.percentCompleted, callback);
    }

    public void SetCompleted(bool value)
    {
      this.m_Completed = value;
    }

    public void SetHidden(bool value)
    {
      this.m_Hidden = value;
    }

    public void SetLastReportedDate(DateTime date)
    {
      this.m_LastReportedDate = date;
    }
  }
}
