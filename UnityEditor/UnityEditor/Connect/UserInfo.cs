// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UserInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Connect
{
  internal struct UserInfo
  {
    private int m_Valid;
    private string m_UserName;
    private string m_DisplayName;
    private string m_PrimaryOrg;
    private string m_AccessToken;
    private int m_AccessTokenValiditySeconds;

    public bool valid
    {
      get
      {
        return this.m_Valid != 0;
      }
    }

    public string userName
    {
      get
      {
        return this.m_UserName;
      }
    }

    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    public string primaryOrg
    {
      get
      {
        return this.m_PrimaryOrg;
      }
    }

    public string accessToken
    {
      get
      {
        return this.m_AccessToken;
      }
    }

    public int accessTokenValiditySeconds
    {
      get
      {
        return this.m_AccessTokenValiditySeconds;
      }
    }
  }
}
