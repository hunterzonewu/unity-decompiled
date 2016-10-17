// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedGCHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>A description of a GC handle used by the virtual machine.</para>
  /// </summary>
  [Serializable]
  public struct PackedGCHandle
  {
    [SerializeField]
    internal ulong m_Target;

    /// <summary>
    ///   <para>The address of the managed object that the GC handle is referencing.</para>
    /// </summary>
    public ulong target
    {
      get
      {
        return this.m_Target;
      }
    }
  }
}
