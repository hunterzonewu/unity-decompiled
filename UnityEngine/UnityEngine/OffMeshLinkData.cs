// Decompiled with JetBrains decompiler
// Type: UnityEngine.OffMeshLinkData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>State of OffMeshLink.</para>
  /// </summary>
  public struct OffMeshLinkData
  {
    private int m_Valid;
    private int m_Activated;
    private int m_InstanceID;
    private OffMeshLinkType m_LinkType;
    private Vector3 m_StartPos;
    private Vector3 m_EndPos;

    /// <summary>
    ///   <para>Is link valid (Read Only).</para>
    /// </summary>
    public bool valid
    {
      get
      {
        return this.m_Valid != 0;
      }
    }

    /// <summary>
    ///   <para>Is link active (Read Only).</para>
    /// </summary>
    public bool activated
    {
      get
      {
        return this.m_Activated != 0;
      }
    }

    /// <summary>
    ///   <para>Link type specifier (Read Only).</para>
    /// </summary>
    public OffMeshLinkType linkType
    {
      get
      {
        return this.m_LinkType;
      }
    }

    /// <summary>
    ///   <para>Link start world position (Read Only).</para>
    /// </summary>
    public Vector3 startPos
    {
      get
      {
        return this.m_StartPos;
      }
    }

    /// <summary>
    ///   <para>Link end world position (Read Only).</para>
    /// </summary>
    public Vector3 endPos
    {
      get
      {
        return this.m_EndPos;
      }
    }

    /// <summary>
    ///   <para>The OffMeshLink if the link type is a manually placed Offmeshlink (Read Only).</para>
    /// </summary>
    public OffMeshLink offMeshLink
    {
      get
      {
        return this.GetOffMeshLinkInternal(this.m_InstanceID);
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal OffMeshLink GetOffMeshLinkInternal(int instanceID);
  }
}
