// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneAccelerationEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneAccelerationEvent struct is deprecated. Please use AccelerationEvent instead (UnityUpgradable) -> AccelerationEvent", true)]
  public struct iPhoneAccelerationEvent
  {
    [Obsolete("timeDelta property is deprecated. Please use AccelerationEvent.deltaTime instead (UnityUpgradable) -> AccelerationEvent.deltaTime", true)]
    public float timeDelta
    {
      get
      {
        return 0.0f;
      }
    }

    public Vector3 acceleration
    {
      get
      {
        return new Vector3();
      }
    }

    public float deltaTime
    {
      get
      {
        return -1f;
      }
    }
  }
}
