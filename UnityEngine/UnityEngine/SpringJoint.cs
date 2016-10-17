// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpringJoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The spring joint ties together 2 rigid bodies, spring forces will be automatically applied to keep the object at the given distance.</para>
  /// </summary>
  public sealed class SpringJoint : Joint
  {
    /// <summary>
    ///   <para>The spring force used to keep the two objects together.</para>
    /// </summary>
    public float spring { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The damper force used to dampen the spring force.</para>
    /// </summary>
    public float damper { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The minimum distance between the bodies relative to their initial distance.</para>
    /// </summary>
    public float minDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum distance between the bodies relative to their initial distance.</para>
    /// </summary>
    public float maxDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum allowed error between the current spring length and the length defined by minDistance and maxDistance.</para>
    /// </summary>
    public float tolerance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
