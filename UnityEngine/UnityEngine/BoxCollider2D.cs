// Decompiled with JetBrains decompiler
// Type: UnityEngine.BoxCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Collider for 2D physics representing an axis-aligned rectangle.</para>
  /// </summary>
  public sealed class BoxCollider2D : Collider2D
  {
    /// <summary>
    ///   <para>The width and height of the rectangle.</para>
    /// </summary>
    public Vector2 size
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_size(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_size(ref value);
      }
    }

    /// <summary>
    ///   <para>The center point of the collider in local space.</para>
    /// </summary>
    [Obsolete("BoxCollider2D.center has been deprecated. Use BoxCollider2D.offset instead (UnityUpgradable) -> offset", true)]
    public Vector2 center
    {
      get
      {
        return Vector2.zero;
      }
      set
      {
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_size(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_size(ref Vector2 value);
  }
}
