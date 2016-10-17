// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Local
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms
{
  public class Local : ISocialPlatform
  {
    private List<UserProfile> m_Friends = new List<UserProfile>();
    private List<UserProfile> m_Users = new List<UserProfile>();
    private List<AchievementDescription> m_AchievementDescriptions = new List<AchievementDescription>();
    private List<Achievement> m_Achievements = new List<Achievement>();
    private List<Leaderboard> m_Leaderboards = new List<Leaderboard>();
    private static LocalUser m_LocalUser;
    private Texture2D m_DefaultTexture;

    public ILocalUser localUser
    {
      get
      {
        if (Local.m_LocalUser == null)
          Local.m_LocalUser = new LocalUser();
        return (ILocalUser) Local.m_LocalUser;
      }
    }

    void ISocialPlatform.Authenticate(ILocalUser user, Action<bool> callback)
    {
      LocalUser localUser = (LocalUser) user;
      this.m_DefaultTexture = this.CreateDummyTexture(32, 32);
      this.PopulateStaticData();
      localUser.SetAuthenticated(true);
      localUser.SetUnderage(false);
      localUser.SetUserID("1000");
      localUser.SetUserName("Lerpz");
      localUser.SetImage(this.m_DefaultTexture);
      if (callback == null)
        return;
      callback(true);
    }

    void ISocialPlatform.LoadFriends(ILocalUser user, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      ((LocalUser) user).SetFriends((IUserProfile[]) this.m_Friends.ToArray());
      if (callback == null)
        return;
      callback(true);
    }

    void ISocialPlatform.LoadScores(ILeaderboard board, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      Leaderboard board1 = (Leaderboard) board;
      using (List<Leaderboard>.Enumerator enumerator = this.m_Leaderboards.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Leaderboard current = enumerator.Current;
          if (current.id == board1.id)
          {
            board1.SetTitle(current.title);
            board1.SetScores(current.scores);
            board1.SetMaxRange((uint) current.scores.Length);
          }
        }
      }
      this.SortScores(board1);
      this.SetLocalPlayerScore(board1);
      if (callback == null)
        return;
      callback(true);
    }

    bool ISocialPlatform.GetLoading(ILeaderboard board)
    {
      if (!this.VerifyUser())
        return false;
      return ((Leaderboard) board).loading;
    }

    public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
    {
      List<UserProfile> userProfileList = new List<UserProfile>();
      if (!this.VerifyUser())
        return;
      foreach (string userId in userIDs)
      {
        using (List<UserProfile>.Enumerator enumerator = this.m_Users.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UserProfile current = enumerator.Current;
            if (current.id == userId)
              userProfileList.Add(current);
          }
        }
        using (List<UserProfile>.Enumerator enumerator = this.m_Friends.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UserProfile current = enumerator.Current;
            if (current.id == userId)
              userProfileList.Add(current);
          }
        }
      }
      callback((IUserProfile[]) userProfileList.ToArray());
    }

    public void ReportProgress(string id, double progress, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      using (List<Achievement>.Enumerator enumerator = this.m_Achievements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Achievement current = enumerator.Current;
          if (current.id == id && current.percentCompleted <= progress)
          {
            if (progress >= 100.0)
              current.SetCompleted(true);
            current.SetHidden(false);
            current.SetLastReportedDate(DateTime.Now);
            current.percentCompleted = progress;
            if (callback == null)
              return;
            callback(true);
            return;
          }
        }
      }
      using (List<AchievementDescription>.Enumerator enumerator = this.m_AchievementDescriptions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.id == id)
          {
            bool completed = progress >= 100.0;
            this.m_Achievements.Add(new Achievement(id, progress, completed, false, DateTime.Now));
            if (callback == null)
              return;
            callback(true);
            return;
          }
        }
      }
      Debug.LogError((object) "Achievement ID not found");
      if (callback == null)
        return;
      callback(false);
    }

    public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
    {
      if (!this.VerifyUser() || callback == null)
        return;
      callback((IAchievementDescription[]) this.m_AchievementDescriptions.ToArray());
    }

    public void LoadAchievements(Action<IAchievement[]> callback)
    {
      if (!this.VerifyUser() || callback == null)
        return;
      callback((IAchievement[]) this.m_Achievements.ToArray());
    }

    public void ReportScore(long score, string board, Action<bool> callback)
    {
      if (!this.VerifyUser())
        return;
      using (List<Leaderboard>.Enumerator enumerator = this.m_Leaderboards.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Leaderboard current = enumerator.Current;
          if (current.id == board)
          {
            current.SetScores((IScore[]) new List<Score>((IEnumerable<Score>) current.scores)
            {
              new Score(board, score, this.localUser.id, DateTime.Now, score.ToString() + " points", 0)
            }.ToArray());
            if (callback == null)
              return;
            callback(true);
            return;
          }
        }
      }
      Debug.LogError((object) "Leaderboard not found");
      if (callback == null)
        return;
      callback(false);
    }

    public void LoadScores(string leaderboardID, Action<IScore[]> callback)
    {
      if (!this.VerifyUser())
        return;
      using (List<Leaderboard>.Enumerator enumerator = this.m_Leaderboards.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Leaderboard current = enumerator.Current;
          if (current.id == leaderboardID)
          {
            this.SortScores(current);
            if (callback == null)
              return;
            callback(current.scores);
            return;
          }
        }
      }
      Debug.LogError((object) "Leaderboard not found");
      if (callback == null)
        return;
      callback((IScore[]) new Score[0]);
    }

    private void SortScores(Leaderboard board)
    {
      List<Score> scoreList = new List<Score>((IEnumerable<Score>) board.scores);
      scoreList.Sort((Comparison<Score>) ((s1, s2) => s2.value.CompareTo(s1.value)));
      for (int index = 0; index < scoreList.Count; ++index)
        scoreList[index].SetRank(index + 1);
    }

    private void SetLocalPlayerScore(Leaderboard board)
    {
      foreach (Score score in board.scores)
      {
        if (score.userID == this.localUser.id)
        {
          board.SetLocalUserScore((IScore) score);
          break;
        }
      }
    }

    public void ShowAchievementsUI()
    {
      Debug.Log((object) "ShowAchievementsUI not implemented");
    }

    public void ShowLeaderboardUI()
    {
      Debug.Log((object) "ShowLeaderboardUI not implemented");
    }

    public ILeaderboard CreateLeaderboard()
    {
      return (ILeaderboard) new Leaderboard();
    }

    public IAchievement CreateAchievement()
    {
      return (IAchievement) new Achievement();
    }

    private bool VerifyUser()
    {
      if (this.localUser.authenticated)
        return true;
      Debug.LogError((object) "Must authenticate first");
      return false;
    }

    private void PopulateStaticData()
    {
      this.m_Friends.Add(new UserProfile("Fred", "1001", true, UserState.Online, this.m_DefaultTexture));
      this.m_Friends.Add(new UserProfile("Julia", "1002", true, UserState.Online, this.m_DefaultTexture));
      this.m_Friends.Add(new UserProfile("Jeff", "1003", true, UserState.Online, this.m_DefaultTexture));
      this.m_Users.Add(new UserProfile("Sam", "1004", false, UserState.Offline, this.m_DefaultTexture));
      this.m_Users.Add(new UserProfile("Max", "1005", false, UserState.Offline, this.m_DefaultTexture));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement01", "First achievement", this.m_DefaultTexture, "Get first achievement", "Received first achievement", false, 10));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement02", "Second achievement", this.m_DefaultTexture, "Get second achievement", "Received second achievement", false, 20));
      this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement03", "Third achievement", this.m_DefaultTexture, "Get third achievement", "Received third achievement", false, 15));
      Leaderboard leaderboard = new Leaderboard();
      leaderboard.SetTitle("High Scores");
      leaderboard.id = "Leaderboard01";
      leaderboard.SetScores((IScore[]) new List<Score>()
      {
        new Score("Leaderboard01", 300L, "1001", DateTime.Now.AddDays(-1.0), "300 points", 1),
        new Score("Leaderboard01", (long) byte.MaxValue, "1002", DateTime.Now.AddDays(-1.0), "255 points", 2),
        new Score("Leaderboard01", 55L, "1003", DateTime.Now.AddDays(-1.0), "55 points", 3),
        new Score("Leaderboard01", 10L, "1004", DateTime.Now.AddDays(-1.0), "10 points", 4)
      }.ToArray());
      this.m_Leaderboards.Add(leaderboard);
    }

    private Texture2D CreateDummyTexture(int width, int height)
    {
      Texture2D texture2D = new Texture2D(width, height);
      for (int y = 0; y < height; ++y)
      {
        for (int x = 0; x < width; ++x)
        {
          Color color = (x & y) <= 0 ? Color.gray : Color.white;
          texture2D.SetPixel(x, y, color);
        }
      }
      texture2D.Apply();
      return texture2D;
    }
  }
}
