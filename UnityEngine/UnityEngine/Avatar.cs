// Decompiled with JetBrains decompiler
// Type: UnityEngine.Avatar
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Avatar definition.</para>
  /// </summary>
  public sealed class Avatar : Object
  {
    /// <summary>
    ///   <para>Return true if this avatar is a valid mecanim avatar. It can be a generic avatar or a human avatar.</para>
    /// </summary>
    public bool isValid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Return true if this avatar is a valid human avatar.</para>
    /// </summary>
    public bool isHuman { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private Avatar()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetMuscleMinMax(int muscleId, float min, float max);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetParameter(int parameterId, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal float GetAxisLength(int humanId);

    internal Quaternion GetPreRotation(int humanId)
    {
      Quaternion quaternion;
      Avatar.INTERNAL_CALL_GetPreRotation(this, humanId, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPreRotation(Avatar self, int humanId, out Quaternion value);

    internal Quaternion GetPostRotation(int humanId)
    {
      Quaternion quaternion;
      Avatar.INTERNAL_CALL_GetPostRotation(this, humanId, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPostRotation(Avatar self, int humanId, out Quaternion value);

    internal Quaternion GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
    {
      Quaternion quaternion;
      Avatar.INTERNAL_CALL_GetZYPostQ(this, humanId, ref parentQ, ref q, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetZYPostQ(Avatar self, int humanId, ref Quaternion parentQ, ref Quaternion q, out Quaternion value);

    internal Quaternion GetZYRoll(int humanId, Vector3 uvw)
    {
      Quaternion quaternion;
      Avatar.INTERNAL_CALL_GetZYRoll(this, humanId, ref uvw, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetZYRoll(Avatar self, int humanId, ref Vector3 uvw, out Quaternion value);

    internal Vector3 GetLimitSign(int humanId)
    {
      Vector3 vector3;
      Avatar.INTERNAL_CALL_GetLimitSign(this, humanId, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLimitSign(Avatar self, int humanId, out Vector3 value);
  }
}
