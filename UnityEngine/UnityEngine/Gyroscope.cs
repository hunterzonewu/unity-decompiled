// Decompiled with JetBrains decompiler
// Type: UnityEngine.Gyroscope
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into the Gyroscope.</para>
  /// </summary>
  public sealed class Gyroscope
  {
    private int m_GyroIndex;

    /// <summary>
    ///   <para>Returns rotation rate as measured by the device's gyroscope.</para>
    /// </summary>
    public Vector3 rotationRate
    {
      get
      {
        return Gyroscope.rotationRate_Internal(this.m_GyroIndex);
      }
    }

    /// <summary>
    ///   <para>Returns unbiased rotation rate as measured by the device's gyroscope.</para>
    /// </summary>
    public Vector3 rotationRateUnbiased
    {
      get
      {
        return Gyroscope.rotationRateUnbiased_Internal(this.m_GyroIndex);
      }
    }

    /// <summary>
    ///   <para>Returns the gravity acceleration vector expressed in the device's reference frame.</para>
    /// </summary>
    public Vector3 gravity
    {
      get
      {
        return Gyroscope.gravity_Internal(this.m_GyroIndex);
      }
    }

    /// <summary>
    ///   <para>Returns the acceleration that the user is giving to the device.</para>
    /// </summary>
    public Vector3 userAcceleration
    {
      get
      {
        return Gyroscope.userAcceleration_Internal(this.m_GyroIndex);
      }
    }

    /// <summary>
    ///   <para>Returns the attitude (ie, orientation in space) of the device.</para>
    /// </summary>
    public Quaternion attitude
    {
      get
      {
        return Gyroscope.attitude_Internal(this.m_GyroIndex);
      }
    }

    /// <summary>
    ///   <para>Sets or retrieves the enabled status of this gyroscope.</para>
    /// </summary>
    public bool enabled
    {
      get
      {
        return Gyroscope.getEnabled_Internal(this.m_GyroIndex);
      }
      set
      {
        Gyroscope.setEnabled_Internal(this.m_GyroIndex, value);
      }
    }

    /// <summary>
    ///   <para>Sets or retrieves gyroscope interval in seconds.</para>
    /// </summary>
    public float updateInterval
    {
      get
      {
        return Gyroscope.getUpdateInterval_Internal(this.m_GyroIndex);
      }
      set
      {
        Gyroscope.setUpdateInterval_Internal(this.m_GyroIndex, value);
      }
    }

    internal Gyroscope(int index)
    {
      this.m_GyroIndex = index;
    }

    private static Vector3 rotationRate_Internal(int idx)
    {
      Vector3 vector3;
      Gyroscope.INTERNAL_CALL_rotationRate_Internal(idx, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_rotationRate_Internal(int idx, out Vector3 value);

    private static Vector3 rotationRateUnbiased_Internal(int idx)
    {
      Vector3 vector3;
      Gyroscope.INTERNAL_CALL_rotationRateUnbiased_Internal(idx, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_rotationRateUnbiased_Internal(int idx, out Vector3 value);

    private static Vector3 gravity_Internal(int idx)
    {
      Vector3 vector3;
      Gyroscope.INTERNAL_CALL_gravity_Internal(idx, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_gravity_Internal(int idx, out Vector3 value);

    private static Vector3 userAcceleration_Internal(int idx)
    {
      Vector3 vector3;
      Gyroscope.INTERNAL_CALL_userAcceleration_Internal(idx, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_userAcceleration_Internal(int idx, out Vector3 value);

    private static Quaternion attitude_Internal(int idx)
    {
      Quaternion quaternion;
      Gyroscope.INTERNAL_CALL_attitude_Internal(idx, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_attitude_Internal(int idx, out Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool getEnabled_Internal(int idx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void setEnabled_Internal(int idx, bool enabled);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float getUpdateInterval_Internal(int idx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void setUpdateInterval_Internal(int idx, float interval);
  }
}
