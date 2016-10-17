// Decompiled with JetBrains decompiler
// Type: UnityEngine.TreePrototype
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Simple class that contains a pointer to a tree prototype.</para>
  /// </summary>
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class TreePrototype
  {
    internal GameObject m_Prefab;
    internal float m_BendFactor;

    /// <summary>
    ///   <para>Retrieves the actual GameObect used by the tree.</para>
    /// </summary>
    public GameObject prefab
    {
      get
      {
        return this.m_Prefab;
      }
      set
      {
        this.m_Prefab = value;
      }
    }

    /// <summary>
    ///   <para>Bend factor of the tree prototype.</para>
    /// </summary>
    public float bendFactor
    {
      get
      {
        return this.m_BendFactor;
      }
      set
      {
        this.m_BendFactor = value;
      }
    }
  }
}
