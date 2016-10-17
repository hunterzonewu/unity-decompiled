// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.Impl.LocalUser
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms.Impl
{
  public class LocalUser : UserProfile, ILocalUser, IUserProfile
  {
    private IUserProfile[] m_Friends;
    private bool m_Authenticated;
    private bool m_Underage;

    public IUserProfile[] friends
    {
      get
      {
        return this.m_Friends;
      }
    }

    public bool authenticated
    {
      get
      {
        return this.m_Authenticated;
      }
    }

    public bool underage
    {
      get
      {
        return this.m_Underage;
      }
    }

    public LocalUser()
    {
      this.m_Friends = (IUserProfile[]) new UserProfile[0];
      this.m_Authenticated = false;
      this.m_Underage = false;
    }

    public void Authenticate(Action<bool> callback)
    {
      ActivePlatform.Instance.Authenticate((ILocalUser) this, callback);
    }

    public void LoadFriends(Action<bool> callback)
    {
      ActivePlatform.Instance.LoadFriends((ILocalUser) this, callback);
    }

    public void SetFriends(IUserProfile[] friends)
    {
      this.m_Friends = friends;
    }

    public void SetAuthenticated(bool value)
    {
      this.m_Authenticated = value;
    }

    public void SetUnderage(bool value)
    {
      this.m_Underage = value;
    }
  }
}
