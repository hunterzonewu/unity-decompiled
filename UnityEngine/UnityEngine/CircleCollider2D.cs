// Decompiled with JetBrains decompiler
// Type: UnityEngine.CircleCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Collider for 2D physics representing an circle.</para>
  /// </summary>
  public sealed class CircleCollider2D : Collider2D
  {
    /// <summary>
    ///   <para>Radius of the circle.</para>
    /// </summary>
    public float radius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The center point of the collider in local space.</para>
    /// </summary>
    [Obsolete("CircleCollider2D.center has been deprecated. Use CircleCollider2D.offset instead (UnityUpgradable) -> offset", true)]
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
  }
}
