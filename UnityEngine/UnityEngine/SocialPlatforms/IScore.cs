// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IScore
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface IScore
  {
    /// <summary>
    ///   <para>The ID of the leaderboard this score belongs to.</para>
    /// </summary>
    string leaderboardID { get; set; }

    /// <summary>
    ///   <para>The score value achieved.</para>
    /// </summary>
    long value { get; set; }

    /// <summary>
    ///   <para>The date the score was achieved.</para>
    /// </summary>
    DateTime date { get; }

    /// <summary>
    ///   <para>The correctly formatted value of the score, like X points or X kills.</para>
    /// </summary>
    string formattedValue { get; }

    /// <summary>
    ///   <para>The user who owns this score.</para>
    /// </summary>
    string userID { get; }

    /// <summary>
    ///   <para>The rank or position of the score in the leaderboard. </para>
    /// </summary>
    int rank { get; }

    void ReportScore(Action<bool> callback);
  }
}
