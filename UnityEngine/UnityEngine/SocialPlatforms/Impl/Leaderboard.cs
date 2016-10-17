// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Impl.Leaderboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms.Impl
{
  public class Leaderboard : ILeaderboard
  {
    private bool m_Loading;
    private IScore m_LocalUserScore;
    private uint m_MaxRange;
    private IScore[] m_Scores;
    private string m_Title;
    private string[] m_UserIDs;

    public bool loading
    {
      get
      {
        return ActivePlatform.Instance.GetLoading((ILeaderboard) this);
      }
    }

    public string id { get; set; }

    public UserScope userScope { get; set; }

    public Range range { get; set; }

    public TimeScope timeScope { get; set; }

    public IScore localUserScore
    {
      get
      {
        return this.m_LocalUserScore;
      }
    }

    public uint maxRange
    {
      get
      {
        return this.m_MaxRange;
      }
    }

    public IScore[] scores
    {
      get
      {
        return this.m_Scores;
      }
    }

    public string title
    {
      get
      {
        return this.m_Title;
      }
    }

    public Leaderboard()
    {
      this.id = "Invalid";
      this.range = new Range(1, 10);
      this.userScope = UserScope.Global;
      this.timeScope = TimeScope.AllTime;
      this.m_Loading = false;
      this.m_LocalUserScore = (IScore) new Score("Invalid", 0L);
      this.m_MaxRange = 0U;
      this.m_Scores = (IScore[]) new Score[0];
      this.m_Title = "Invalid";
      this.m_UserIDs = new string[0];
    }

    public void SetUserFilter(string[] userIDs)
    {
      this.m_UserIDs = userIDs;
    }

    public override string ToString()
    {
      return "ID: '" + this.id + "' Title: '" + this.m_Title + "' Loading: '" + (object) this.m_Loading + "' Range: [" + (object) this.range.from + "," + (object) this.range.count + "] MaxRange: '" + (object) this.m_MaxRange + "' Scores: '" + (object) this.m_Scores.Length + "' UserScope: '" + (object) this.userScope + "' TimeScope: '" + (object) this.timeScope + "' UserFilter: '" + (object) this.m_UserIDs.Length;
    }

    public void LoadScores(Action<bool> callback)
    {
      ActivePlatform.Instance.LoadScores((ILeaderboard) this, callback);
    }

    public void SetLocalUserScore(IScore score)
    {
      this.m_LocalUserScore = score;
    }

    public void SetMaxRange(uint maxRange)
    {
      this.m_MaxRange = maxRange;
    }

    public void SetScores(IScore[] scores)
    {
      this.m_Scores = scores;
    }

    public void SetTitle(string title)
    {
      this.m_Title = title;
    }

    public string[] GetUserFilter()
    {
      return this.m_UserIDs;
    }
  }
}
