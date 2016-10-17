// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedNativeType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A description of a C++ unity type.</para>
  /// </summary>
  [Serializable]
  public struct PackedNativeType
  {
    [SerializeField]
    internal string m_Name;
    [SerializeField]
    internal int m_BaseClassId;

    /// <summary>
    ///   <para>Name of this C++ unity type.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>ClassId of the base class of this C++ class.</para>
    /// </summary>
    public int baseClassId
    {
      get
      {
        return this.m_BaseClassId;
      }
    }
  }
}
