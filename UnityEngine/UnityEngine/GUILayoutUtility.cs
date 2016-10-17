// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngineInternal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Utility functions for implementing and extending the GUILayout class.</para>
  /// </summary>
  public class GUILayoutUtility
  {
    private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredLayouts = new Dictionary<int, GUILayoutUtility.LayoutCache>();
    private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredWindows = new Dictionary<int, GUILayoutUtility.LayoutCache>();
    internal static GUILayoutUtility.LayoutCache current = new GUILayoutUtility.LayoutCache();
    private static readonly Rect kDummyRect = new Rect(0.0f, 0.0f, 1f, 1f);
    private static GUIStyle s_SpaceStyle;

    internal static GUILayoutGroup topLevel
    {
      get
      {
        return GUILayoutUtility.current.topLevel;
      }
    }

    internal static GUIStyle spaceStyle
    {
      get
      {
        if (GUILayoutUtility.s_SpaceStyle == null)
          GUILayoutUtility.s_SpaceStyle = new GUIStyle();
        GUILayoutUtility.s_SpaceStyle.stretchWidth = false;
        return GUILayoutUtility.s_SpaceStyle;
      }
    }

    internal static GUILayoutUtility.LayoutCache SelectIDList(int instanceID, bool isWindow)
    {
      Dictionary<int, GUILayoutUtility.LayoutCache> dictionary = !isWindow ? GUILayoutUtility.s_StoredLayouts : GUILayoutUtility.s_StoredWindows;
      GUILayoutUtility.LayoutCache layoutCache;
      if (!dictionary.TryGetValue(instanceID, out layoutCache))
      {
        layoutCache = new GUILayoutUtility.LayoutCache();
        dictionary[instanceID] = layoutCache;
      }
      GUILayoutUtility.current.topLevel = layoutCache.topLevel;
      GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
      GUILayoutUtility.current.windows = layoutCache.windows;
      return layoutCache;
    }

    internal static void Begin(int instanceID)
    {
      GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(instanceID, false);
      if (Event.current.type == EventType.Layout)
      {
        GUILayoutUtility.current.topLevel = layoutCache.topLevel = new GUILayoutGroup();
        GUILayoutUtility.current.layoutGroups.Clear();
        GUILayoutUtility.current.layoutGroups.Push((object) GUILayoutUtility.current.topLevel);
        GUILayoutUtility.current.windows = layoutCache.windows = new GUILayoutGroup();
      }
      else
      {
        GUILayoutUtility.current.topLevel = layoutCache.topLevel;
        GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
        GUILayoutUtility.current.windows = layoutCache.windows;
      }
    }

    internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
    {
      GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(windowID, true);
      if (Event.current.type == EventType.Layout)
      {
        GUILayoutUtility.current.topLevel = layoutCache.topLevel = new GUILayoutGroup();
        GUILayoutUtility.current.topLevel.style = style;
        GUILayoutUtility.current.topLevel.windowID = windowID;
        if (options != null)
          GUILayoutUtility.current.topLevel.ApplyOptions(options);
        GUILayoutUtility.current.layoutGroups.Clear();
        GUILayoutUtility.current.layoutGroups.Push((object) GUILayoutUtility.current.topLevel);
        GUILayoutUtility.current.windows = layoutCache.windows = new GUILayoutGroup();
      }
      else
      {
        GUILayoutUtility.current.topLevel = layoutCache.topLevel;
        GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
        GUILayoutUtility.current.windows = layoutCache.windows;
      }
    }

    public static void BeginGroup(string GroupName)
    {
    }

    public static void EndGroup(string groupName)
    {
    }

    internal static void Layout()
    {
      if (GUILayoutUtility.current.topLevel.windowID == -1)
      {
        GUILayoutUtility.current.topLevel.CalcWidth();
        GUILayoutUtility.current.topLevel.SetHorizontal(0.0f, Mathf.Min((float) Screen.width / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxWidth));
        GUILayoutUtility.current.topLevel.CalcHeight();
        GUILayoutUtility.current.topLevel.SetVertical(0.0f, Mathf.Min((float) Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
        GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
      }
      else
      {
        GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
        GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
      }
    }

    internal static void LayoutFromEditorWindow()
    {
      GUILayoutUtility.current.topLevel.CalcWidth();
      GUILayoutUtility.current.topLevel.SetHorizontal(0.0f, (float) Screen.width / GUIUtility.pixelsPerPoint);
      GUILayoutUtility.current.topLevel.CalcHeight();
      GUILayoutUtility.current.topLevel.SetVertical(0.0f, (float) Screen.height / GUIUtility.pixelsPerPoint);
      GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
    }

    internal static float LayoutFromInspector(float width)
    {
      if (GUILayoutUtility.current.topLevel != null && GUILayoutUtility.current.topLevel.windowID == -1)
      {
        GUILayoutUtility.current.topLevel.CalcWidth();
        GUILayoutUtility.current.topLevel.SetHorizontal(0.0f, width);
        GUILayoutUtility.current.topLevel.CalcHeight();
        GUILayoutUtility.current.topLevel.SetVertical(0.0f, Mathf.Min((float) Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
        float minHeight = GUILayoutUtility.current.topLevel.minHeight;
        GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
        return minHeight;
      }
      if (GUILayoutUtility.current.topLevel != null)
        GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
      return 0.0f;
    }

    internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
    {
      using (List<GUILayoutEntry>.Enumerator enumerator = toplevel.entries.GetEnumerator())
      {
        while (enumerator.MoveNext())
          GUILayoutUtility.LayoutSingleGroup((GUILayoutGroup) enumerator.Current);
      }
      toplevel.ResetCursor();
    }

    private static void LayoutSingleGroup(GUILayoutGroup i)
    {
      if (!i.isWindow)
      {
        float minWidth = i.minWidth;
        float maxWidth = i.maxWidth;
        i.CalcWidth();
        i.SetHorizontal(i.rect.x, Mathf.Clamp(i.maxWidth, minWidth, maxWidth));
        float minHeight = i.minHeight;
        float maxHeight = i.maxHeight;
        i.CalcHeight();
        i.SetVertical(i.rect.y, Mathf.Clamp(i.maxHeight, minHeight, maxHeight));
      }
      else
      {
        i.CalcWidth();
        Rect windowRect = GUILayoutUtility.Internal_GetWindowRect(i.windowID);
        i.SetHorizontal(windowRect.x, Mathf.Clamp(windowRect.width, i.minWidth, i.maxWidth));
        i.CalcHeight();
        i.SetVertical(windowRect.y, Mathf.Clamp(windowRect.height, i.minHeight, i.maxHeight));
        GUILayoutUtility.Internal_MoveWindow(i.windowID, i.rect);
      }
    }

    [SecuritySafeCritical]
    private static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(System.Type LayoutType)
    {
      if (!typeof (GUILayoutGroup).IsAssignableFrom(LayoutType))
        throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup");
      return (GUILayoutGroup) Activator.CreateInstance(LayoutType);
    }

    internal static GUILayoutGroup BeginLayoutGroup(GUIStyle style, GUILayoutOption[] options, System.Type layoutType)
    {
      GUILayoutGroup guiLayoutGroup;
      switch (Event.current.type)
      {
        case EventType.Layout:
        case EventType.Used:
          guiLayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
          guiLayoutGroup.style = style;
          if (options != null)
            guiLayoutGroup.ApplyOptions(options);
          GUILayoutUtility.current.topLevel.Add((GUILayoutEntry) guiLayoutGroup);
          break;
        default:
          guiLayoutGroup = GUILayoutUtility.current.topLevel.GetNext() as GUILayoutGroup;
          if (guiLayoutGroup == null)
            throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + (object) Event.current.type);
          guiLayoutGroup.ResetCursor();
          break;
      }
      GUILayoutUtility.current.layoutGroups.Push((object) guiLayoutGroup);
      GUILayoutUtility.current.topLevel = guiLayoutGroup;
      return guiLayoutGroup;
    }

    internal static void EndLayoutGroup()
    {
      EventType type = Event.current.type;
      GUILayoutUtility.current.layoutGroups.Pop();
      GUILayoutUtility.current.topLevel = (GUILayoutGroup) GUILayoutUtility.current.layoutGroups.Peek();
    }

    internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, System.Type layoutType)
    {
      GUILayoutGroup guiLayoutGroup;
      switch (Event.current.type)
      {
        case EventType.Layout:
        case EventType.Used:
          guiLayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
          guiLayoutGroup.style = style;
          GUILayoutUtility.current.windows.Add((GUILayoutEntry) guiLayoutGroup);
          break;
        default:
          guiLayoutGroup = GUILayoutUtility.current.windows.GetNext() as GUILayoutGroup;
          if (guiLayoutGroup == null)
            throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + (object) Event.current.type);
          guiLayoutGroup.ResetCursor();
          break;
      }
      GUILayoutUtility.current.layoutGroups.Push((object) guiLayoutGroup);
      GUILayoutUtility.current.topLevel = guiLayoutGroup;
      return guiLayoutGroup;
    }

    internal static GUILayoutGroup DoBeginLayoutArea(GUIStyle style, System.Type layoutType)
    {
      return GUILayoutUtility.BeginLayoutArea(style, layoutType);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle for displaying some contents with a specific style.</para>
    /// </summary>
    /// <param name="content">The content to make room for displaying.</param>
    /// <param name="style">The GUIStyle to layout for.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle that is large enough to contain content when rendered in style.</para>
    /// </returns>
    public static Rect GetRect(GUIContent content, GUIStyle style)
    {
      return GUILayoutUtility.DoGetRect(content, style, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle for displaying some contents with a specific style.</para>
    /// </summary>
    /// <param name="content">The content to make room for displaying.</param>
    /// <param name="style">The GUIStyle to layout for.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle that is large enough to contain content when rendered in style.</para>
    /// </returns>
    public static Rect GetRect(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetRect(content, style, options);
    }

    private static Rect DoGetRect(GUIContent content, GUIStyle style, GUILayoutOption[] options)
    {
      GUIUtility.CheckOnGUI();
      switch (Event.current.type)
      {
        case EventType.Layout:
          if (style.isHeightDependantOnWidth)
          {
            GUILayoutUtility.current.topLevel.Add((GUILayoutEntry) new GUIWordWrapSizer(style, content, options));
          }
          else
          {
            Vector2 constraints = new Vector2(0.0f, 0.0f);
            if (options != null)
            {
              foreach (GUILayoutOption option in options)
              {
                switch (option.type)
                {
                  case GUILayoutOption.Type.maxWidth:
                    constraints.x = (float) option.value;
                    break;
                  case GUILayoutOption.Type.maxHeight:
                    constraints.y = (float) option.value;
                    break;
                }
              }
            }
            Vector2 vector2 = style.CalcSizeWithConstraints(content, constraints);
            GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(vector2.x, vector2.x, vector2.y, vector2.y, style, options));
          }
          return GUILayoutUtility.kDummyRect;
        case EventType.Used:
          return GUILayoutUtility.kDummyRect;
        default:
          return GUILayoutUtility.current.topLevel.GetNext().rect;
      }
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
    /// </summary>
    /// <param name="width">The width of the area you want.</param>
    /// <param name="height">The height of the area you want.</param>
    /// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rectanlge to put your control in.</para>
    /// </returns>
    public static Rect GetRect(float width, float height)
    {
      return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
    /// </summary>
    /// <param name="width">The width of the area you want.</param>
    /// <param name="height">The height of the area you want.</param>
    /// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rectanlge to put your control in.</para>
    /// </returns>
    public static Rect GetRect(float width, float height, GUIStyle style)
    {
      return GUILayoutUtility.DoGetRect(width, width, height, height, style, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
    /// </summary>
    /// <param name="width">The width of the area you want.</param>
    /// <param name="height">The height of the area you want.</param>
    /// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rectanlge to put your control in.</para>
    /// </returns>
    public static Rect GetRect(float width, float height, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, options);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
    /// </summary>
    /// <param name="width">The width of the area you want.</param>
    /// <param name="height">The height of the area you want.</param>
    /// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rectanlge to put your control in.</para>
    /// </returns>
    public static Rect GetRect(float width, float height, GUIStyle style, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetRect(width, width, height, height, style, options);
    }

    /// <summary>
    ///   <para>Reserve layout space for a flexible rect.</para>
    /// </summary>
    /// <param name="minWidth">The minimum width of the area passed back.</param>
    /// <param name="maxWidth">The maximum width of the area passed back.</param>
    /// <param name="minHeight">The minimum width of the area passed back.</param>
    /// <param name="maxHeight">The maximum width of the area passed back.</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
    /// </returns>
    public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight)
    {
      return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a flexible rect.</para>
    /// </summary>
    /// <param name="minWidth">The minimum width of the area passed back.</param>
    /// <param name="maxWidth">The maximum width of the area passed back.</param>
    /// <param name="minHeight">The minimum width of the area passed back.</param>
    /// <param name="maxHeight">The maximum width of the area passed back.</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
    /// </returns>
    public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style)
    {
      return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a flexible rect.</para>
    /// </summary>
    /// <param name="minWidth">The minimum width of the area passed back.</param>
    /// <param name="maxWidth">The maximum width of the area passed back.</param>
    /// <param name="minHeight">The minimum width of the area passed back.</param>
    /// <param name="maxHeight">The maximum width of the area passed back.</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
    /// </returns>
    public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
    }

    /// <summary>
    ///   <para>Reserve layout space for a flexible rect.</para>
    /// </summary>
    /// <param name="minWidth">The minimum width of the area passed back.</param>
    /// <param name="maxWidth">The maximum width of the area passed back.</param>
    /// <param name="minHeight">The minimum width of the area passed back.</param>
    /// <param name="maxHeight">The maximum width of the area passed back.</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
    /// </returns>
    public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
    }

    private static Rect DoGetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options)
    {
      switch (Event.current.type)
      {
        case EventType.Layout:
          GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
          return GUILayoutUtility.kDummyRect;
        case EventType.Used:
          return GUILayoutUtility.kDummyRect;
        default:
          return GUILayoutUtility.current.topLevel.GetNext().rect;
      }
    }

    /// <summary>
    ///   <para>Get the rectangle last used by GUILayout for a control.</para>
    /// </summary>
    /// <returns>
    ///   <para>The last used rectangle.</para>
    /// </returns>
    public static Rect GetLastRect()
    {
      switch (Event.current.type)
      {
        case EventType.Layout:
          return GUILayoutUtility.kDummyRect;
        case EventType.Used:
          return GUILayoutUtility.kDummyRect;
        default:
          return GUILayoutUtility.current.topLevel.GetLast();
      }
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
    /// </summary>
    /// <param name="aspect">The aspect ratio of the element (width / height).</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rect for the control.</para>
    /// </returns>
    public static Rect GetAspectRect(float aspect)
    {
      return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
    /// </summary>
    /// <param name="aspect">The aspect ratio of the element (width / height).</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rect for the control.</para>
    /// </returns>
    public static Rect GetAspectRect(float aspect, GUIStyle style)
    {
      return GUILayoutUtility.DoGetAspectRect(aspect, style, (GUILayoutOption[]) null);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
    /// </summary>
    /// <param name="aspect">The aspect ratio of the element (width / height).</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rect for the control.</para>
    /// </returns>
    public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, options);
    }

    /// <summary>
    ///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
    /// </summary>
    /// <param name="aspect">The aspect ratio of the element (width / height).</param>
    /// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The rect for the control.</para>
    /// </returns>
    public static Rect GetAspectRect(float aspect, GUIStyle style, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, options);
    }

    private static Rect DoGetAspectRect(float aspect, GUIStyle style, GUILayoutOption[] options)
    {
      switch (Event.current.type)
      {
        case EventType.Layout:
          GUILayoutUtility.current.topLevel.Add((GUILayoutEntry) new GUIAspectSizer(aspect, options));
          return GUILayoutUtility.kDummyRect;
        case EventType.Used:
          return GUILayoutUtility.kDummyRect;
        default:
          return GUILayoutUtility.current.topLevel.GetNext().rect;
      }
    }

    private static Rect Internal_GetWindowRect(int windowID)
    {
      Rect rect;
      GUILayoutUtility.INTERNAL_CALL_Internal_GetWindowRect(windowID, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_GetWindowRect(int windowID, out Rect value);

    private static void Internal_MoveWindow(int windowID, Rect r)
    {
      GUILayoutUtility.INTERNAL_CALL_Internal_MoveWindow(windowID, ref r);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_MoveWindow(int windowID, ref Rect r);

    internal static Rect GetWindowsBounds()
    {
      Rect rect;
      GUILayoutUtility.INTERNAL_CALL_GetWindowsBounds(out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetWindowsBounds(out Rect value);

    internal sealed class LayoutCache
    {
      internal GUILayoutGroup topLevel = new GUILayoutGroup();
      internal GenericStack layoutGroups = new GenericStack();
      internal GUILayoutGroup windows = new GUILayoutGroup();

      internal LayoutCache()
      {
        this.layoutGroups.Push((object) this.topLevel);
      }

      internal LayoutCache(GUILayoutUtility.LayoutCache other)
      {
        this.topLevel = other.topLevel;
        this.layoutGroups = other.layoutGroups;
        this.windows = other.windows;
      }
    }
  }
}
