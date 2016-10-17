// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeAnimatorController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Runtime representation of the AnimatorController. It can be used to change the Animator's controller during runtime.</para>
  /// </summary>
  public class RuntimeAnimatorController : Object
  {
    /// <summary>
    ///   <para>Retrieves all AnimationClip used by the controller.</para>
    /// </summary>
    public AnimationClip[] animationClips { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
