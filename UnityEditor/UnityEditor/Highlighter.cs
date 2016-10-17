// Decompiled with JetBrains decompiler
// Type: UnityEditor.Highlighter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Use this class to highlight elements in the editor for use in in-editor tutorials and similar.</para>
  /// </summary>
  public sealed class Highlighter
  {
    private const float kPulseSpeed = 0.45f;
    private const float kPopupDuration = 0.33f;
    private const int kExpansionMovementSize = 5;
    private static GUIView s_View;
    private static HighlightSearchMode s_SearchMode;
    private static float s_HighlightElapsedTime;
    private static float s_LastTime;
    private static Rect s_RepaintRegion;
    private static GUIStyle s_HighlightStyle;

    internal static extern HighlightSearchMode searchMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool searching { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private static GUIStyle highlightStyle
    {
      get
      {
        if (Highlighter.s_HighlightStyle == null)
          Highlighter.s_HighlightStyle = new GUIStyle((GUIStyle) "ControlHighlight");
        return Highlighter.s_HighlightStyle;
      }
    }

    /// <summary>
    ///   <para>Is there currently an active highlight?</para>
    /// </summary>
    public static bool active { get; private set; }

    /// <summary>
    ///   <para>Is the current active highlight visible yet?</para>
    /// </summary>
    public static bool activeVisible
    {
      get
      {
        return Highlighter.internal_get_activeVisible();
      }
      private set
      {
        Highlighter.internal_set_activeVisible(value);
      }
    }

    /// <summary>
    ///   <para>The text of the current active highlight.</para>
    /// </summary>
    public static string activeText
    {
      get
      {
        return Highlighter.internal_get_activeText();
      }
      private set
      {
        Highlighter.internal_set_activeText(value);
      }
    }

    /// <summary>
    ///   <para>The rect in screenspace of the current active highlight.</para>
    /// </summary>
    public static Rect activeRect
    {
      get
      {
        return Highlighter.internal_get_activeRect();
      }
      private set
      {
        Highlighter.internal_set_activeRect(value);
      }
    }

    internal static void Handle(Rect position, string text)
    {
      Highlighter.INTERNAL_CALL_Handle(ref position, text);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Handle(ref Rect position, string text);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string internal_get_activeText();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void internal_set_activeText(string value);

    internal static Rect internal_get_activeRect()
    {
      Rect rect;
      Highlighter.INTERNAL_CALL_internal_get_activeRect(out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_internal_get_activeRect(out Rect value);

    internal static void internal_set_activeRect(Rect value)
    {
      Highlighter.INTERNAL_CALL_internal_set_activeRect(ref value);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_internal_set_activeRect(ref Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool internal_get_activeVisible();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void internal_set_activeVisible(bool value);

    /// <summary>
    ///   <para>Stops the active highlight.</para>
    /// </summary>
    public static void Stop()
    {
      Highlighter.active = false;
      Highlighter.activeVisible = false;
      Highlighter.activeText = string.Empty;
      Highlighter.activeRect = new Rect();
      Highlighter.s_LastTime = 0.0f;
      Highlighter.s_HighlightElapsedTime = 0.0f;
    }

    /// <summary>
    ///   <para>Highlights an element in the editor.</para>
    /// </summary>
    /// <param name="windowTitle">The title of the window the element is inside.</param>
    /// <param name="text">The text to identify the element with.</param>
    /// <param name="mode">Optional mode to specify how to search for the element.</param>
    /// <returns>
    ///   <para>true if the requested element was found; otherwise false.</para>
    /// </returns>
    public static bool Highlight(string windowTitle, string text)
    {
      return Highlighter.Highlight(windowTitle, text, HighlightSearchMode.Auto);
    }

    /// <summary>
    ///   <para>Highlights an element in the editor.</para>
    /// </summary>
    /// <param name="windowTitle">The title of the window the element is inside.</param>
    /// <param name="text">The text to identify the element with.</param>
    /// <param name="mode">Optional mode to specify how to search for the element.</param>
    /// <returns>
    ///   <para>true if the requested element was found; otherwise false.</para>
    /// </returns>
    public static bool Highlight(string windowTitle, string text, HighlightSearchMode mode)
    {
      Highlighter.Stop();
      Highlighter.active = true;
      if (!Highlighter.SetWindow(windowTitle))
      {
        Debug.LogWarning((object) ("Window " + windowTitle + " not found."));
        return false;
      }
      Highlighter.activeText = text;
      Highlighter.s_SearchMode = mode;
      Highlighter.s_LastTime = Time.realtimeSinceStartup;
      bool flag = Highlighter.Search();
      if (flag)
      {
        EditorApplication.update -= new EditorApplication.CallbackFunction(Highlighter.Update);
        EditorApplication.update += new EditorApplication.CallbackFunction(Highlighter.Update);
      }
      else
      {
        Debug.LogWarning((object) ("Item " + text + " not found in window " + windowTitle + "."));
        Highlighter.Stop();
      }
      InternalEditorUtility.RepaintAllViews();
      return flag;
    }

    /// <summary>
    ///   <para>Call this method to create an identifiable rect that the Highlighter can find.</para>
    /// </summary>
    /// <param name="position">The position to make highlightable.</param>
    /// <param name="identifier">The identifier text of the rect.</param>
    public static void HighlightIdentifier(Rect position, string identifier)
    {
      if (Highlighter.searchMode != HighlightSearchMode.Identifier && Highlighter.searchMode != HighlightSearchMode.Auto)
        return;
      Highlighter.Handle(position, identifier);
    }

    private static void Update()
    {
      Rect activeRect = Highlighter.activeRect;
      if ((double) Highlighter.activeRect.width == 0.0 || (Object) Highlighter.s_View == (Object) null)
      {
        EditorApplication.update -= new EditorApplication.CallbackFunction(Highlighter.Update);
        Highlighter.Stop();
        InternalEditorUtility.RepaintAllViews();
      }
      else
        Highlighter.Search();
      if (Highlighter.activeVisible)
        Highlighter.s_HighlightElapsedTime += Time.realtimeSinceStartup - Highlighter.s_LastTime;
      Highlighter.s_LastTime = Time.realtimeSinceStartup;
      Rect rect = Highlighter.activeRect;
      if ((double) activeRect.width > 0.0)
      {
        rect.xMin = Mathf.Min(rect.xMin, activeRect.xMin);
        rect.xMax = Mathf.Max(rect.xMax, activeRect.xMax);
        rect.yMin = Mathf.Min(rect.yMin, activeRect.yMin);
        rect.yMax = Mathf.Max(rect.yMax, activeRect.yMax);
      }
      rect = Highlighter.highlightStyle.padding.Add(rect);
      rect = Highlighter.highlightStyle.overflow.Add(rect);
      rect = new RectOffset(7, 7, 7, 7).Add(rect);
      if ((double) Highlighter.s_HighlightElapsedTime < 0.430000007152557)
        rect = new RectOffset((int) rect.width / 2, (int) rect.width / 2, (int) rect.height / 2, (int) rect.height / 2).Add(rect);
      Highlighter.s_RepaintRegion = rect;
      foreach (GUIView guiView in Resources.FindObjectsOfTypeAll(typeof (GUIView)))
      {
        if ((Object) guiView.window == (Object) Highlighter.s_View.window)
          guiView.SendEvent(EditorGUIUtility.CommandEvent("HandleControlHighlight"));
      }
    }

    private static bool SetWindow(string windowTitle)
    {
      Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (GUIView));
      GUIView guiView1 = (GUIView) null;
      foreach (GUIView guiView2 in objectsOfTypeAll)
      {
        if (guiView2 is HostView)
        {
          if ((guiView2 as HostView).actualView.titleContent.text == windowTitle)
          {
            guiView1 = guiView2;
            break;
          }
        }
        else if ((bool) ((Object) guiView2.window) && guiView2.GetType().Name == windowTitle)
        {
          guiView1 = guiView2;
          break;
        }
      }
      Highlighter.s_View = guiView1;
      return (Object) guiView1 != (Object) null;
    }

    private static bool Search()
    {
      Highlighter.searchMode = Highlighter.s_SearchMode;
      Highlighter.s_View.RepaintImmediately();
      if (Highlighter.searchMode == HighlightSearchMode.None)
        return true;
      Highlighter.searchMode = HighlightSearchMode.None;
      Highlighter.Stop();
      return false;
    }

    internal static void ControlHighlightGUI(GUIView self)
    {
      if ((Object) Highlighter.s_View == (Object) null || (Object) self.window != (Object) Highlighter.s_View.window || (!Highlighter.activeVisible || Highlighter.searching))
        return;
      if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "HandleControlHighlight")
      {
        if (!self.screenPosition.Overlaps(Highlighter.s_RepaintRegion))
          return;
        self.Repaint();
      }
      else
      {
        if (Event.current.type != EventType.Repaint)
          return;
        Rect position = Highlighter.highlightStyle.padding.Add(GUIUtility.ScreenToGUIRect(Highlighter.activeRect));
        float num1 = (float) (((double) Mathf.Cos((float) ((double) Highlighter.s_HighlightElapsedTime * 3.14159274101257 * 2.0 * 0.449999988079071)) + 1.0) * 0.5);
        float num2 = Mathf.Min(1f, (float) (0.00999999977648258 + (double) Highlighter.s_HighlightElapsedTime / 0.330000013113022));
        float num3 = num2 + Mathf.Sin(num2 * 3.141593f) * 0.5f;
        Vector2 scale = (Vector2.one + new Vector2((float) (((double) position.width + 5.0) / (double) position.width - 1.0), (float) (((double) position.height + 5.0) / (double) position.height - 1.0)) * num1) * num3;
        Matrix4x4 matrix = GUI.matrix;
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, (float) (0.800000011920929 - 0.300000011920929 * (double) num1));
        GUIUtility.ScaleAroundPivot(scale, position.center);
        Highlighter.highlightStyle.Draw(position, false, false, false, false);
        GUI.color = color;
        GUI.matrix = matrix;
      }
    }
  }
}
