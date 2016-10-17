// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.MemorySection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A dump of a piece of memory from the player that's being profiled.</para>
  /// </summary>
  [Serializable]
  public struct MemorySection
  {
    [SerializeField]
    internal byte[] m_Bytes;
    [SerializeField]
    internal ulong m_StartAddress;

    /// <summary>
    ///   <para>The actual bytes of the memory dump.</para>
    /// </summary>
    public byte[] bytes
    {
      get
      {
        return this.m_Bytes;
      }
    }

    /// <summary>
    ///   <para>The start address of this piece of memory.</para>
    /// </summary>
    public ulong startAddress
    {
      get
      {
        return this.m_StartAddress;
      }
    }
  }
}
