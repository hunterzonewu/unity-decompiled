// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColliderUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class ColliderUtil
  {
    public static Vector3 GetCapsuleExtents(CapsuleCollider cc)
    {
      Vector3 vector3;
      ColliderUtil.INTERNAL_CALL_GetCapsuleExtents(cc, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetCapsuleExtents(CapsuleCollider cc, out Vector3 value);

    public static Matrix4x4 CalculateCapsuleTransform(CapsuleCollider cc)
    {
      Matrix4x4 matrix4x4;
      ColliderUtil.INTERNAL_CALL_CalculateCapsuleTransform(cc, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateCapsuleTransform(CapsuleCollider cc, out Matrix4x4 value);
  }
}
