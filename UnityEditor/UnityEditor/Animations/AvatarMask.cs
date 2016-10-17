// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AvatarMask
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>AvatarMask are used to mask out humanoid body parts and transforms.</para>
  /// </summary>
  public sealed class AvatarMask : UnityEngine.Object
  {
    [Obsolete("AvatarMask.humanoidBodyPartCount is deprecated. Use AvatarMaskBodyPart.LastBodyPart instead.")]
    private int humanoidBodyPartCount
    {
      get
      {
        return 13;
      }
    }

    /// <summary>
    ///   <para>Number of transforms.</para>
    /// </summary>
    public int transformCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool hasFeetIK { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Creates a new AvatarMask.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public AvatarMask();

    /// <summary>
    ///   <para>Returns true if the humanoid body part at the given index is active.</para>
    /// </summary>
    /// <param name="index">The index of the humanoid body part.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetHumanoidBodyPartActive(AvatarMaskBodyPart index);

    /// <summary>
    ///   <para>Sets the humanoid body part at the given index to active or not.</para>
    /// </summary>
    /// <param name="index">The index of the humanoid body part.</param>
    /// <param name="value">Active or not.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetHumanoidBodyPartActive(AvatarMaskBodyPart index, bool value);

    /// <summary>
    ///   <para>Returns the path of the transform at the given index.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetTransformPath(int index);

    /// <summary>
    ///   <para>Sets the path of the transform at the given index.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    /// <param name="path">The path of the transform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetTransformPath(int index, string path);

    /// <summary>
    ///   <para>Returns true if the transform at the given index is active.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetTransformActive(int index);

    /// <summary>
    ///   <para>Sets the tranform at the given index to active or not.</para>
    /// </summary>
    /// <param name="index">The index of the transform.</param>
    /// <param name="value">Active or not.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetTransformActive(int index, bool value);

    internal void Copy(AvatarMask other)
    {
      for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart; ++index)
        this.SetHumanoidBodyPartActive(index, other.GetHumanoidBodyPartActive(index));
      this.transformCount = other.transformCount;
      for (int index = 0; index < other.transformCount; ++index)
      {
        this.SetTransformPath(index, other.GetTransformPath(index));
        this.SetTransformActive(index, other.GetTransformActive(index));
      }
    }
  }
}
