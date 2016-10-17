// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorClipInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about clip been played and blended by the Animator.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct AnimatorClipInfo
  {
    private int m_ClipInstanceID;
    private float m_Weight;

    /// <summary>
    ///   <para>Returns the animation clip played by the Animator.</para>
    /// </summary>
    public AnimationClip clip
    {
      get
      {
        if (this.m_ClipInstanceID != 0)
          return AnimatorClipInfo.ClipInstanceToScriptingObject(this.m_ClipInstanceID);
        return (AnimationClip) null;
      }
    }

    /// <summary>
    ///   <para>Returns the blending weight used by the Animator to blend this clip.</para>
    /// </summary>
    public float weight
    {
      get
      {
        return this.m_Weight;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AnimationClip ClipInstanceToScriptingObject(int instanceID);
  }
}
