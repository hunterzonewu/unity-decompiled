// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneInput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneInput class is deprecated. Please use Input instead (UnityUpgradable) -> Input", true)]
  public class iPhoneInput
  {
    [Obsolete("orientation property is deprecated. Please use Input.deviceOrientation instead (UnityUpgradable) -> Input.deviceOrientation", true)]
    public static iPhoneOrientation orientation
    {
      get
      {
        return iPhoneOrientation.Unknown;
      }
    }

    [Obsolete("lastLocation property is deprecated. Please use Input.location.lastData instead.", true)]
    public static LocationInfo lastLocation
    {
      get
      {
        return new LocationInfo();
      }
    }

    public static iPhoneAccelerationEvent[] accelerationEvents
    {
      get
      {
        return (iPhoneAccelerationEvent[]) null;
      }
    }

    public static iPhoneTouch[] touches
    {
      get
      {
        return (iPhoneTouch[]) null;
      }
    }

    public static int touchCount
    {
      get
      {
        return 0;
      }
    }

    public static bool multiTouchEnabled
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public static int accelerationEventCount
    {
      get
      {
        return 0;
      }
    }

    public static Vector3 acceleration
    {
      get
      {
        return new Vector3();
      }
    }

    public static iPhoneTouch GetTouch(int index)
    {
      return new iPhoneTouch();
    }

    public static iPhoneAccelerationEvent GetAccelerationEvent(int index)
    {
      return new iPhoneAccelerationEvent();
    }
  }
}
