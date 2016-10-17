// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ClipperRegistry
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Registry class to keep track of all IClippers that exist in the scene.</para>
  /// </summary>
  public class ClipperRegistry
  {
    private readonly IndexedSet<IClipper> m_Clippers = new IndexedSet<IClipper>();
    private static ClipperRegistry s_Instance;

    /// <summary>
    ///   <para>Singleton instance.</para>
    /// </summary>
    public static ClipperRegistry instance
    {
      get
      {
        if (ClipperRegistry.s_Instance == null)
          ClipperRegistry.s_Instance = new ClipperRegistry();
        return ClipperRegistry.s_Instance;
      }
    }

    protected ClipperRegistry()
    {
    }

    /// <summary>
    ///   <para>Perform the clipping on all registered IClipper.</para>
    /// </summary>
    public void Cull()
    {
      for (int index = 0; index < this.m_Clippers.Count; ++index)
        this.m_Clippers[index].PerformClipping();
    }

    /// <summary>
    ///   <para>Register an IClipper.</para>
    /// </summary>
    /// <param name="c"></param>
    public static void Register(IClipper c)
    {
      if (c == null)
        return;
      ClipperRegistry.instance.m_Clippers.AddUnique(c);
    }

    /// <summary>
    ///   <para>Unregister an IClipper.</para>
    /// </summary>
    /// <param name="c"></param>
    public static void Unregister(IClipper c)
    {
      ClipperRegistry.instance.m_Clippers.Remove(c);
    }
  }
}
