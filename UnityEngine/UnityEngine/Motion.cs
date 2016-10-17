// Decompiled with JetBrains decompiler
// Type: UnityEngine.Motion
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for AnimationClips and BlendTrees.</para>
  /// </summary>
  public class Motion : Object
  {
    public float averageDuration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public float averageAngularSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Vector3 averageSpeed
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_averageSpeed(out vector3);
        return vector3;
      }
    }

    public float apparentSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isLooping { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool legacy { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isHumanMotion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("isAnimatorMotion is not supported anymore. Use !legacy instead.", true)]
    public bool isAnimatorMotion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_averageSpeed(out Vector3 value);

    [Obsolete("ValidateIfRetargetable is not supported anymore. Use isHumanMotion instead.", true)]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool ValidateIfRetargetable(bool val);
  }
}
