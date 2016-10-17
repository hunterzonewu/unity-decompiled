// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The AnimationState gives full control over animation blending.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class AnimationState : TrackedReference
  {
    /// <summary>
    ///   <para>Enables / disables the animation.</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The weight of animation.</para>
    /// </summary>
    public float weight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Wrapping mode of the animation.</para>
    /// </summary>
    public WrapMode wrapMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current time of the animation.</para>
    /// </summary>
    public float time { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The normalized time of the animation.</para>
    /// </summary>
    public float normalizedTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The playback speed of the animation. 1 is normal playback speed.</para>
    /// </summary>
    public float speed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The normalized playback speed.</para>
    /// </summary>
    public float normalizedSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The length of the animation clip in seconds.</para>
    /// </summary>
    public float length { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int layer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The clip that is being played by this animation state.</para>
    /// </summary>
    public AnimationClip clip { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The name of the animation.</para>
    /// </summary>
    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which blend mode should be used?</para>
    /// </summary>
    public AnimationBlendMode blendMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Adds a transform which should be animated. This allows you to reduce the number of animations you have to create.</para>
    /// </summary>
    /// <param name="mix">The transform to animate.</param>
    /// <param name="recursive">Whether to also animate all children of the specified transform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void AddMixingTransform(Transform mix, [DefaultValue("true")] bool recursive);

    /// <summary>
    ///   <para>Adds a transform which should be animated. This allows you to reduce the number of animations you have to create.</para>
    /// </summary>
    /// <param name="mix">The transform to animate.</param>
    /// <param name="recursive">Whether to also animate all children of the specified transform.</param>
    [ExcludeFromDocs]
    public void AddMixingTransform(Transform mix)
    {
      bool recursive = true;
      this.AddMixingTransform(mix, recursive);
    }

    /// <summary>
    ///   <para>Removes a transform which should be animated.</para>
    /// </summary>
    /// <param name="mix"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RemoveMixingTransform(Transform mix);
  }
}
