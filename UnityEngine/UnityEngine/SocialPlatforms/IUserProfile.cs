// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IUserProfile
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  public interface IUserProfile
  {
    /// <summary>
    ///   <para>This user's username or alias.</para>
    /// </summary>
    string userName { get; }

    /// <summary>
    ///   <para>This users unique identifier.</para>
    /// </summary>
    string id { get; }

    /// <summary>
    ///   <para>Is this user a friend of the current logged in user?</para>
    /// </summary>
    bool isFriend { get; }

    /// <summary>
    ///   <para>Presence state of the user.</para>
    /// </summary>
    UserState state { get; }

    /// <summary>
    ///   <para>Avatar image of the user.</para>
    /// </summary>
    Texture2D image { get; }
  }
}
