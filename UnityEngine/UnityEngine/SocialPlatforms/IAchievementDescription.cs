// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IAchievementDescription
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  public interface IAchievementDescription
  {
    /// <summary>
    ///   <para>Unique identifier for this achievement description.</para>
    /// </summary>
    string id { get; set; }

    /// <summary>
    ///   <para>Human readable title.</para>
    /// </summary>
    string title { get; }

    /// <summary>
    ///   <para>Image representation of the achievement.</para>
    /// </summary>
    Texture2D image { get; }

    /// <summary>
    ///   <para>Description when the achivement is completed.</para>
    /// </summary>
    string achievedDescription { get; }

    /// <summary>
    ///   <para>Description when the achivement has not been completed.</para>
    /// </summary>
    string unachievedDescription { get; }

    /// <summary>
    ///   <para>Hidden achievement are not shown in the list until the percentCompleted has been touched (even if it's 0.0).</para>
    /// </summary>
    bool hidden { get; }

    /// <summary>
    ///   <para>Point value of this achievement.</para>
    /// </summary>
    int points { get; }
  }
}
