// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedNativeUnityEngineObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>Description of a C++ unity object in memory.</para>
  /// </summary>
  [Serializable]
  public struct PackedNativeUnityEngineObject
  {
    [SerializeField]
    internal string m_Name;
    [SerializeField]
    internal int m_InstanceId;
    [SerializeField]
    internal int m_Size;
    [SerializeField]
    internal int m_ClassId;
    [SerializeField]
    internal HideFlags m_HideFlags;
    [SerializeField]
    internal PackedNativeUnityEngineObject.ObjectFlags m_Flags;

    /// <summary>
    ///   <para>Is this object persistent? (Assets are persistent, objects stored in scenes are persistent,  dynamically created objects are not)</para>
    /// </summary>
    public bool isPersistent
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsPersistent) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Has this object has been marked as DontDestroyOnLoad?</para>
    /// </summary>
    public bool isDontDestroyOnLoad
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsDontDestroyOnLoad) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Is this native object an internal Unity manager object?</para>
    /// </summary>
    public bool isManager
    {
      get
      {
        return (this.m_Flags & PackedNativeUnityEngineObject.ObjectFlags.IsManager) != (PackedNativeUnityEngineObject.ObjectFlags) 0;
      }
    }

    /// <summary>
    ///   <para>Name of this object.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>InstanceId of this object.</para>
    /// </summary>
    public int instanceId
    {
      get
      {
        return this.m_InstanceId;
      }
    }

    /// <summary>
    ///   <para>Size in bytes of this object.</para>
    /// </summary>
    public int size
    {
      get
      {
        return this.m_Size;
      }
    }

    /// <summary>
    ///   <para>ClassId of this C++ object.  Use this classId to index into PackedMemorySnapshot.nativeTypes.</para>
    /// </summary>
    public int classId
    {
      get
      {
        return this.m_ClassId;
      }
    }

    /// <summary>
    ///   <para>The hideFlags this native object has.</para>
    /// </summary>
    public HideFlags hideFlags
    {
      get
      {
        return this.m_HideFlags;
      }
    }

    internal enum ObjectFlags
    {
      IsDontDestroyOnLoad = 1,
      IsPersistent = 2,
      IsManager = 4,
    }
  }
}
