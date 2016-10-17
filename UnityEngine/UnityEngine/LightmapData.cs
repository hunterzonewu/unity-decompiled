// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmapData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Data of a lightmap.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class LightmapData
  {
    internal Texture2D m_Light;
    internal Texture2D m_Dir;

    /// <summary>
    ///   <para>Lightmap storing the full incoming light.</para>
    /// </summary>
    public Texture2D lightmapFar
    {
      get
      {
        return this.m_Light;
      }
      set
      {
        this.m_Light = value;
      }
    }

    /// <summary>
    ///   <para>Lightmap storing only the indirect incoming light.</para>
    /// </summary>
    public Texture2D lightmapNear
    {
      get
      {
        return this.m_Dir;
      }
      set
      {
        this.m_Dir = value;
      }
    }

    [Obsolete("Property LightmapData.lightmap has been deprecated. Use LightmapData.lightmapFar instead (UnityUpgradable) -> lightmapFar", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Texture2D lightmap
    {
      get
      {
        return (Texture2D) null;
      }
      set
      {
      }
    }
  }
}
