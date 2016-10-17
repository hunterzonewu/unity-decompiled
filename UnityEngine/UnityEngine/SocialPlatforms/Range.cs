// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Range
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  /// <summary>
  ///   <para>The score range a leaderboard query should include.</para>
  /// </summary>
  public struct Range
  {
    /// <summary>
    ///   <para>The rank of the first score which is returned.</para>
    /// </summary>
    public int from;
    /// <summary>
    ///   <para>The total amount of scores retreived.</para>
    /// </summary>
    public int count;

    /// <summary>
    ///   <para>Constructor for a score range, the range starts from a specific value and contains a maxium score count.</para>
    /// </summary>
    /// <param name="fromValue">The minimum allowed value.</param>
    /// <param name="valueCount">The number of possible values.</param>
    public Range(int fromValue, int valueCount)
    {
      this.from = fromValue;
      this.count = valueCount;
    }
  }
}
