// Decompiled with JetBrains decompiler
// Type: UnityEngine.WindZone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Wind Zones add realism to the trees you create by making them wave their branches and leaves as if blown by the wind.</para>
  /// </summary>
  public sealed class WindZone : Component
  {
    /// <summary>
    ///   <para>Defines the type of wind zone to be used (Spherical or Directional).</para>
    /// </summary>
    public WindZoneMode mode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Radius of the Spherical Wind Zone (only active if the WindZoneMode is set to Spherical).</para>
    /// </summary>
    public float radius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The primary wind force.</para>
    /// </summary>
    public float windMain { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The turbulence wind force.</para>
    /// </summary>
    public float windTurbulence { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Defines ow much the wind changes over time.</para>
    /// </summary>
    public float windPulseMagnitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Defines the frequency of the wind changes.</para>
    /// </summary>
    public float windPulseFrequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
