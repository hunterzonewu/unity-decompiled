// Decompiled with JetBrains decompiler
// Type: UnityEngine.LocationService
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into location functionality.</para>
  /// </summary>
  public sealed class LocationService
  {
    /// <summary>
    ///   <para>Specifies whether location service is enabled in user settings.</para>
    /// </summary>
    public bool isEnabledByUser { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns location service status.</para>
    /// </summary>
    public LocationServiceStatus status { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Last measured device geographical location.</para>
    /// </summary>
    public LocationInfo lastData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Starts location service updates.  Last location coordinates could be.</para>
    /// </summary>
    /// <param name="desiredAccuracyInMeters"></param>
    /// <param name="updateDistanceInMeters"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Start([DefaultValue("10f")] float desiredAccuracyInMeters, [DefaultValue("10f")] float updateDistanceInMeters);

    /// <summary>
    ///   <para>Starts location service updates.  Last location coordinates could be.</para>
    /// </summary>
    /// <param name="desiredAccuracyInMeters"></param>
    /// <param name="updateDistanceInMeters"></param>
    [ExcludeFromDocs]
    public void Start(float desiredAccuracyInMeters)
    {
      float updateDistanceInMeters = 10f;
      this.Start(desiredAccuracyInMeters, updateDistanceInMeters);
    }

    [ExcludeFromDocs]
    public void Start()
    {
      this.Start(10f, 10f);
    }

    /// <summary>
    ///   <para>Stops location service updates. This could be useful for saving battery life.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Stop();
  }
}
