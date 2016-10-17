// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneKeyboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneKeyboard class is deprecated. Please use TouchScreenKeyboard instead (UnityUpgradable) -> TouchScreenKeyboard", true)]
  public class iPhoneKeyboard
  {
    public string text
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    public static bool hideInput
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool active
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool done
    {
      get
      {
        return false;
      }
    }

    public static Rect area
    {
      get
      {
        return new Rect();
      }
    }

    public static bool visible
    {
      get
      {
        return false;
      }
    }
  }
}
