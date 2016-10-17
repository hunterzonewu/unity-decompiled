// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms.GameCenter
{
  /// <summary>
  ///   <para>iOS GameCenter implementation for network services.</para>
  /// </summary>
  public class GameCenterPlatform : Local
  {
    public static void ResetAllAchievements(Action<bool> callback)
    {
      Debug.Log((object) "ResetAllAchievements - no effect in editor");
      callback(true);
    }

    /// <summary>
    ///   <para>Show the default iOS banner when achievements are completed.</para>
    /// </summary>
    /// <param name="value"></param>
    public static void ShowDefaultAchievementCompletionBanner(bool value)
    {
      Debug.Log((object) "ShowDefaultAchievementCompletionBanner - no effect in editor");
    }

    /// <summary>
    ///   <para>Show the leaderboard UI with a specific leaderboard shown initially with a specific time scope selected.</para>
    /// </summary>
    /// <param name="leaderboardID"></param>
    /// <param name="timeScope"></param>
    public static void ShowLeaderboardUI(string leaderboardID, TimeScope timeScope)
    {
      Debug.Log((object) "ShowLeaderboardUI - no effect in editor");
    }
  }
}
