// Decompiled with JetBrains decompiler
// Type: UnityEngine.RaycastHit
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Structure used to get information back from a raycast.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct RaycastHit
  {
    private Vector3 m_Point;
    private Vector3 m_Normal;
    private int m_FaceID;
    private float m_Distance;
    private Vector2 m_UV;
    private Collider m_Collider;

    /// <summary>
    ///   <para>The impact point in world space where the ray hit the collider.</para>
    /// </summary>
    public Vector3 point
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
    ///   <para>The normal of the surface the ray hit.</para>
    /// </summary>
    public Vector3 normal
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
    ///   <para>The barycentric coordinate of the triangle we hit.</para>
    /// </summary>
    public Vector3 barycentricCoordinate
    {
      get
      {
        return new Vector3((float) (1.0 - ((double) this.m_UV.y + (double) this.m_UV.x)), this.m_UV.x, this.m_UV.y);
      }
      set
      {
        this.m_UV = (Vector2) value;
      }
    }

    /// <summary>
    ///   <para>The distance from the ray's origin to the impact point.</para>
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
    ///   <para>The index of the triangle that was hit.</para>
    /// </summary>
    public int triangleIndex
    {
      get
      {
        return this.m_FaceID;
      }
    }

    /// <summary>
    ///   <para>The uv texture coordinate at the impact point.</para>
    /// </summary>
    public Vector2 textureCoord
    {
      get
      {
        Vector2 output;
        RaycastHit.CalculateRaycastTexCoord(out output, this.collider, this.m_UV, this.m_Point, this.m_FaceID, 0);
        return output;
      }
    }

    /// <summary>
    ///   <para>The secondary uv texture coordinate at the impact point.</para>
    /// </summary>
    public Vector2 textureCoord2
    {
      get
      {
        Vector2 output;
        RaycastHit.CalculateRaycastTexCoord(out output, this.collider, this.m_UV, this.m_Point, this.m_FaceID, 1);
        return output;
      }
    }

    [Obsolete("Use textureCoord2 instead")]
    public Vector2 textureCoord1
    {
      get
      {
        Vector2 output;
        RaycastHit.CalculateRaycastTexCoord(out output, this.collider, this.m_UV, this.m_Point, this.m_FaceID, 1);
        return output;
      }
    }

    /// <summary>
    ///   <para>The uv lightmap coordinate at the impact point.</para>
    /// </summary>
    public Vector2 lightmapCoord
    {
      get
      {
        Vector2 output;
        RaycastHit.CalculateRaycastTexCoord(out output, this.collider, this.m_UV, this.m_Point, this.m_FaceID, 1);
        if ((Object) this.collider.GetComponent<Renderer>() != (Object) null)
        {
          Vector4 lightmapScaleOffset = this.collider.GetComponent<Renderer>().lightmapScaleOffset;
          output.x = output.x * lightmapScaleOffset.x + lightmapScaleOffset.z;
          output.y = output.y * lightmapScaleOffset.y + lightmapScaleOffset.w;
        }
        return output;
      }
    }

    /// <summary>
    ///   <para>The Collider that was hit.</para>
    /// </summary>
    public Collider collider
    {
      get
      {
        return this.m_Collider;
      }
    }

    /// <summary>
    ///   <para>The Rigidbody of the collider that was hit. If the collider is not attached to a rigidbody then it is null.</para>
    /// </summary>
    public Rigidbody rigidbody
    {
      get
      {
        if ((Object) this.collider != (Object) null)
          return this.collider.attachedRigidbody;
        return (Rigidbody) null;
      }
    }

    /// <summary>
    ///   <para>The Transform of the rigidbody or collider that was hit.</para>
    /// </summary>
    public Transform transform
    {
      get
      {
        Rigidbody rigidbody = this.rigidbody;
        if ((Object) rigidbody != (Object) null)
          return rigidbody.transform;
        if ((Object) this.collider != (Object) null)
          return this.collider.transform;
        return (Transform) null;
      }
    }

    private static void CalculateRaycastTexCoord(out Vector2 output, Collider col, Vector2 uv, Vector3 point, int face, int index)
    {
      RaycastHit.INTERNAL_CALL_CalculateRaycastTexCoord(out output, col, ref uv, ref point, face, index);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateRaycastTexCoord(out Vector2 output, Collider col, ref Vector2 uv, ref Vector3 point, int face, int index);
  }
}
