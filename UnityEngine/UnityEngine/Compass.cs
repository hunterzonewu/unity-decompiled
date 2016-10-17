// Decompiled with JetBrains decompiler
// Type: UnityEngine.Compass
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into compass functionality.</para>
  /// </summary>
  public sealed class Compass
  {
    /// <summary>
    ///   <para>The heading in degrees relative to the magnetic North Pole. (Read Only)</para>
    /// </summary>
    public float magneticHeading { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The heading in degrees relative to the geographic North Pole. (Read Only)</para>
    /// </summary>
    public float trueHeading { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Accuracy of heading reading in degrees.</para>
    /// </summary>
    public float headingAccuracy { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The raw geomagnetic data measured in microteslas. (Read Only)</para>
    /// </summary>
    public Vector3 rawVector
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_rawVector(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>Timestamp (in seconds since 1970) when the heading was last time updated. (Read Only)</para>
    /// </summary>
    public double timestamp { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Used to enable or disable compass. Note, that if you want Input.compass.trueHeading property to contain a valid value, you must also enable location updates by calling Input.location.Start().</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rawVector(out Vector3 value);
  }
}
