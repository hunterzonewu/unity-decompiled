// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneTouch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneTouch struct is deprecated. Please use Touch instead (UnityUpgradable) -> Touch", true)]
  public struct iPhoneTouch
  {
    [Obsolete("positionDelta property is deprecated. Please use Touch.deltaPosition instead (UnityUpgradable) -> Touch.deltaPosition", true)]
    public Vector2 positionDelta
    {
      get
      {
        return new Vector2();
      }
    }

    [Obsolete("timeDelta property is deprecated. Please use Touch.deltaTime instead (UnityUpgradable) -> Touch.deltaTime", true)]
    public float timeDelta
    {
      get
      {
        return 0.0f;
      }
    }

    public int fingerId
    {
      get
      {
        return 0;
      }
    }

    public Vector2 position
    {
      get
      {
        return new Vector2();
      }
    }

    public Vector2 deltaPosition
    {
      get
      {
        return new Vector2();
      }
    }

    public float deltaTime
    {
      get
      {
        return 0.0f;
      }
    }

    public int tapCount
    {
      get
      {
        return 0;
      }
    }

    public iPhoneTouchPhase phase
    {
      get
      {
        return iPhoneTouchPhase.Began;
      }
    }
  }
}
