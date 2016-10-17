// Decompiled with JetBrains decompiler
// Type: UnityEngine.RaycastHit2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information returned about an object detected by a raycast in 2D physics.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct RaycastHit2D
  {
    private Vector2 m_Centroid;
    private Vector2 m_Point;
    private Vector2 m_Normal;
    private float m_Distance;
    private float m_Fraction;
    private Collider2D m_Collider;

    /// <summary>
    ///   <para>The centroid of the primitive used to perform the cast.</para>
    /// </summary>
    public Vector2 centroid
    {
      get
      {
        return this.m_Centroid;
      }
      set
      {
        this.m_Centroid = value;
      }
    }

    /// <summary>
    ///   <para>The point in world space where the ray hit the collider's surface.</para>
    /// </summary>
    public Vector2 point
    {
      get
      {
        return this.m_Point;
      }
      set
      {
        this.m_Point = value;
      }
    }

    /// <summary>
    ///   <para>The normal vector of the surface hit by the ray.</para>
    /// </summary>
    public Vector2 normal
    {
      get
      {
        return this.m_Normal;
      }
      set
      {
        this.m_Normal = value;
      }
    }

    /// <summary>
    ///   <para>The distance from the ray origin to the impact point.</para>
    /// </summary>
    public float distance
    {
      get
      {
        return this.m_Distance;
      }
      set
      {
        this.m_Distance = value;
      }
    }

    /// <summary>
    ///   <para>Fraction of the distance along the ray that the hit occurred.</para>
    /// </summary>
    public float fraction
    {
      get
      {
        return this.m_Fraction;
      }
      set
      {
        this.m_Fraction = value;
      }
    }

    /// <summary>
    ///   <para>The collider hit by the ray.</para>
    /// </summary>
    public Collider2D collider
    {
      get
      {
        return this.m_Collider;
      }
    }

    /// <summary>
    ///   <para>The Rigidbody2D attached to the object that was hit.</para>
    /// </summary>
    public Rigidbody2D rigidbody
    {
      get
      {
        if ((Object) this.collider != (Object) null)
          return this.collider.attachedRigidbody;
        return (Rigidbody2D) null;
      }
    }

    /// <summary>
    ///   <para>The Transform of the object that was hit.</para>
    /// </summary>
    public Transform transform
    {
      get
      {
        Rigidbody2D rigidbody = this.rigidbody;
        if ((Object) rigidbody != (Object) null)
          return rigidbody.transform;
        if ((Object) this.collider != (Object) null)
          return this.collider.transform;
        return (Transform) null;
      }
    }

    public static implicit operator bool(RaycastHit2D hit)
    {
      return (Object) hit.collider != (Object) null;
    }

    public int CompareTo(RaycastHit2D other)
    {
      if ((Object) this.collider == (Object) null)
        return 1;
      if ((Object) other.collider == (Object) null)
        return -1;
      return this.fraction.CompareTo(other.fraction);
    }
  }
}
