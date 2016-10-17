// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnchoredJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Parent class for all joints that have anchor points.</para>
  /// </summary>
  public class AnchoredJoint2D : Joint2D
  {
    /// <summary>
    ///   <para>The joint's anchor point on the object that has the joint component.</para>
    /// </summary>
    public Vector2 anchor
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_anchor(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_anchor(ref value);
      }
    }

    /// <summary>
    ///   <para>The joint's anchor point on the second object (ie, the one which doesn't have the joint component).</para>
    /// </summary>
    public Vector2 connectedAnchor
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_connectedAnchor(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_connectedAnchor(ref value);
      }
    }

    /// <summary>
    ///   <para>Should the connectedAnchor be calculated automatically?</para>
    /// </summary>
    public bool autoConfigureConnectedAnchor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_anchor(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_anchor(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_connectedAnchor(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_connectedAnchor(ref Vector2 value);
  }
}
