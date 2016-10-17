// Decompiled with JetBrains decompiler
// Type: UnityEngine.RemoteNotification
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;

namespace UnityEngine
{
  [Obsolete("RemoteNotification is deprecated. Please use iOS.RemoteNotification instead (UnityUpgradable) -> UnityEngine.iOS.RemoteNotification", true)]
  public sealed class RemoteNotification
  {
    public string alertBody
    {
      get
      {
        return (string) null;
      }
    }

    public bool hasAction
    {
      get
      {
        return false;
      }
    }

    public int applicationIconBadgeNumber
    {
      get
      {
        return 0;
      }
    }

    public string soundName
    {
      get
      {
        return (string) null;
      }
    }

    public IDictionary userInfo
    {
      get
      {
        return (IDictionary) null;
      }
    }
  }
}
