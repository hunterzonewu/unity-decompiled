// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.Connection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A pair of from and to indices describing what thing keeps what other thing alive.</para>
  /// </summary>
  [Serializable]
  public struct Connection
  {
    [SerializeField]
    internal int m_From;
    [SerializeField]
    internal int m_To;

    /// <summary>
    ///   <para>Index into a virtual list of all GC handles, followed by all native objects.</para>
    /// </summary>
    public int from
    {
      get
      {
        return this.m_From;
      }
      set
      {
        this.m_From = value;
      }
    }

    /// <summary>
    ///   <para>Index into a virtual list of all GC handles, followed by all native objects.</para>
    /// </summary>
    public int to
    {
      get
      {
        return this.m_To;
      }
      set
      {
        this.m_To = value;
      }
    }
  }
}
