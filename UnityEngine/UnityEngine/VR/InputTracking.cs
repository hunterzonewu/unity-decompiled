// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.InputTracking
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.VR
{
  /// <summary>
  ///   <para>VR Input tracking data.</para>
  /// </summary>
  public sealed class InputTracking
  {
    /// <summary>
    ///   <para>The current position of the requested VRNode.</para>
    /// </summary>
    /// <param name="node">Node index.</param>
    /// <returns>
    ///   <para>Position of node local to its tracking space.</para>
    /// </returns>
    public static Vector3 GetLocalPosition(VRNode node)
    {
      Vector3 vector3;
      InputTracking.INTERNAL_CALL_GetLocalPosition(node, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLocalPosition(VRNode node, out Vector3 value);

    /// <summary>
    ///   <para>The current rotation of the requested VRNode.</para>
    /// </summary>
    /// <param name="node">Node index.</param>
    /// <returns>
    ///   <para>Rotation of node local to its tracking space.</para>
    /// </returns>
    public static Quaternion GetLocalRotation(VRNode node)
    {
      Quaternion quaternion;
      InputTracking.INTERNAL_CALL_GetLocalRotation(node, out quaternion);
      return quaternion;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLocalRotation(VRNode node, out Quaternion value);

    /// <summary>
    ///   <para>Center tracking on the current pose.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Recenter();
  }
}
