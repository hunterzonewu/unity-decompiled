// Decompiled with JetBrains decompiler
// Type: UnityEngine.Projector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A script interface for a.</para>
  /// </summary>
  public sealed class Projector : Behaviour
  {
    /// <summary>
    ///   <para>The near clipping plane distance.</para>
    /// </summary>
    public float nearClipPlane { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The far clipping plane distance.</para>
    /// </summary>
    public float farClipPlane { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The field of view of the projection in degrees.</para>
    /// </summary>
    public float fieldOfView { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The aspect ratio of the projection.</para>
    /// </summary>
    public float aspectRatio { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the projection orthographic (true) or perspective (false)?</para>
    /// </summary>
    public bool orthographic { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Projection's half-size when in orthographic mode.</para>
    /// </summary>
    public float orthographicSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which object layers are ignored by the projector.</para>
    /// </summary>
    public int ignoreLayers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The material that will be projected onto every object.</para>
    /// </summary>
    public Material material { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property isOrthoGraphic has been deprecated. Use orthographic instead (UnityUpgradable) -> orthographic", true)]
    public bool isOrthoGraphic
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    [Obsolete("Property orthoGraphicSize has been deprecated. Use orthographicSize instead (UnityUpgradable) -> orthographicSize", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public float orthoGraphicSize
    {
      get
      {
        return -1f;
      }
      set
      {
      }
    }
  }
}
