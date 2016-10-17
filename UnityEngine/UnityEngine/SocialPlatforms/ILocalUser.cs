// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ILocalUser
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface ILocalUser : IUserProfile
  {
    /// <summary>
    ///   <para>The users friends list.</para>
    /// </summary>
    IUserProfile[] friends { get; }

    /// <summary>
    ///   <para>Checks if the current user has been authenticated.</para>
    /// </summary>
    bool authenticated { get; }

    /// <summary>
    ///   <para>Is the user underage?</para>
    /// </summary>
    bool underage { get; }

    void Authenticate(Action<bool> callback);

    void LoadFriends(Action<bool> callback);
  }
}
