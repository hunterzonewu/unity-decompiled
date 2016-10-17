// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collision2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information returned by a collision in 2D physics.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class Collision2D
  {
    internal Rigidbody2D m_Rigidbody;
    internal Collider2D m_Collider;
    internal ContactPoint2D[] m_Contacts;
    internal Vector2 m_RelativeVelocity;
    internal bool m_Enabled;

    /// <summary>
    ///   <para>Whether the collision was disabled or not.</para>
    /// </summary>
    public bool enabled
    {
      get
      {
        return this.m_Enabled;
      }
    }

    /// <summary>
    ///   <para>The incoming Rigidbody2D involved in the collision.</para>
    /// </summary>
    public Rigidbody2D rigidbody
    {
      get
      {
        return this.m_Rigidbody;
      }
    }

    /// <summary>
    ///   <para>The incoming Collider2D involved in the collision.</para>
    /// </summary>
    public Collider2D collider
    {
      get
      {
        return this.m_Collider;
      }
    }

    /// <summary>
    ///   <para>The Transform of the incoming object involved in the collision.</para>
    /// </summary>
    public Transform transform
    {
      get
      {
        if ((Object) this.rigidbody != (Object) null)
          return this.rigidbody.transform;
        return this.collider.transform;
      }
    }

    /// <summary>
    ///   <para>The incoming GameObject involved in the collision.</para>
    /// </summary>
    public GameObject gameObject
    {
      get
      {
        if ((Object) this.m_Rigidbody != (Object) null)
          return this.m_Rigidbody.gameObject;
        return this.m_Collider.gameObject;
      }
    }

    /// <summary>
    ///   <para>The specific points of contact with the incoming Collider2D.</para>
    /// </summary>
    public ContactPoint2D[] contacts
    {
      get
      {
        return this.m_Contacts;
      }
    }

    /// <summary>
    ///   <para>The relative linear velocity of the two colliding objects (Read Only).</para>
    /// </summary>
    public Vector2 relativeVelocity
    {
      get
      {
        return this.m_RelativeVelocity;
      }
    }
  }
}
