// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.GraphicRebuildTracker
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>EditorOnly class for tracking all Graphics.</para>
  /// </summary>
  public static class GraphicRebuildTracker
  {
    private static IndexedSet<Graphic> m_Tracked = new IndexedSet<Graphic>();
    private static bool s_Initialized;

    /// <summary>
    ///   <para>Track a Graphic.</para>
    /// </summary>
    /// <param name="g"></param>
    public static void TrackGraphic(Graphic g)
    {
      if (!GraphicRebuildTracker.s_Initialized)
      {
        CanvasRenderer.onRequestRebuild += new CanvasRenderer.OnRequestRebuild(GraphicRebuildTracker.OnRebuildRequested);
        GraphicRebuildTracker.s_Initialized = true;
      }
      GraphicRebuildTracker.m_Tracked.AddUnique(g);
    }

    /// <summary>
    ///   <para>Untrack a Graphic.</para>
    /// </summary>
    /// <param name="g"></param>
    public static void UnTrackGraphic(Graphic g)
    {
      GraphicRebuildTracker.m_Tracked.Remove(g);
    }

    private static void OnRebuildRequested()
    {
      StencilMaterial.ClearAll();
      for (int index = 0; index < GraphicRebuildTracker.m_Tracked.Count; ++index)
        GraphicRebuildTracker.m_Tracked[index].OnRebuildRequested();
    }
  }
}
