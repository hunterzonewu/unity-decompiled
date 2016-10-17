// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.FontUpdateTracker
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Utility class that is used to help with Text update.</para>
  /// </summary>
  public static class FontUpdateTracker
  {
    private static Dictionary<Font, List<Text>> m_Tracked = new Dictionary<Font, List<Text>>();

    /// <summary>
    ///   <para>Register a Text element for receiving texture atlas rebuild calls.</para>
    /// </summary>
    /// <param name="t"></param>
    public static void TrackText(Text t)
    {
      if ((UnityEngine.Object) t.font == (UnityEngine.Object) null)
        return;
      List<Text> textList;
      FontUpdateTracker.m_Tracked.TryGetValue(t.font, out textList);
      if (textList == null)
      {
        if (FontUpdateTracker.m_Tracked.Count == 0)
          Font.textureRebuilt += new Action<Font>(FontUpdateTracker.RebuildForFont);
        textList = new List<Text>();
        FontUpdateTracker.m_Tracked.Add(t.font, textList);
      }
      if (textList.Contains(t))
        return;
      textList.Add(t);
    }

    private static void RebuildForFont(Font f)
    {
      List<Text> textList;
      FontUpdateTracker.m_Tracked.TryGetValue(f, out textList);
      if (textList == null)
        return;
      for (int index = 0; index < textList.Count; ++index)
        textList[index].FontTextureChanged();
    }

    /// <summary>
    ///   <para>Deregister a Text element from receiving texture atlas rebuild calls.</para>
    /// </summary>
    /// <param name="t"></param>
    public static void UntrackText(Text t)
    {
      if ((UnityEngine.Object) t.font == (UnityEngine.Object) null)
        return;
      List<Text> textList;
      FontUpdateTracker.m_Tracked.TryGetValue(t.font, out textList);
      if (textList == null)
        return;
      textList.Remove(t);
      if (textList.Count != 0)
        return;
      FontUpdateTracker.m_Tracked.Remove(t.font);
      if (FontUpdateTracker.m_Tracked.Count != 0)
        return;
      Font.textureRebuilt -= new Action<Font>(FontUpdateTracker.RebuildForFont);
    }
  }
}
