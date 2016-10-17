// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneHierarchySortingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneHierarchySortingWindow : EditorWindow
  {
    private const float kFrameWidth = 1f;
    private static SceneHierarchySortingWindow s_SceneHierarchySortingWindow;
    private static long s_LastClosedTime;
    private static SceneHierarchySortingWindow.Styles s_Styles;
    private List<SceneHierarchySortingWindow.InputData> m_Data;
    private SceneHierarchySortingWindow.OnSelectCallback m_Callback;

    private SceneHierarchySortingWindow()
    {
      this.hideFlags = HideFlags.DontSave;
      this.wantsMouseMove = true;
    }

    private float GetHeight()
    {
      return 16f * (float) this.m_Data.Count;
    }

    private float GetWidth()
    {
      float num = 0.0f;
      using (List<SceneHierarchySortingWindow.InputData>.Enumerator enumerator = this.m_Data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SceneHierarchySortingWindow.InputData current = enumerator.Current;
          float x = SceneHierarchySortingWindow.s_Styles.menuItem.CalcSize(GUIContent.Temp(current.m_Name)).x;
          if ((double) x > (double) num)
            num = x;
        }
      }
      return num;
    }

    private void OnDisable()
    {
      SceneHierarchySortingWindow.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
    }

    internal static bool ShowAtPosition(Vector2 pos, List<SceneHierarchySortingWindow.InputData> data, SceneHierarchySortingWindow.OnSelectCallback callback)
    {
      if (DateTime.Now.Ticks / 10000L < SceneHierarchySortingWindow.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) SceneHierarchySortingWindow.s_SceneHierarchySortingWindow == (UnityEngine.Object) null)
        SceneHierarchySortingWindow.s_SceneHierarchySortingWindow = ScriptableObject.CreateInstance<SceneHierarchySortingWindow>();
      SceneHierarchySortingWindow.s_SceneHierarchySortingWindow.Init(pos, data, callback);
      return true;
    }

    private void Init(Vector2 pos, List<SceneHierarchySortingWindow.InputData> data, SceneHierarchySortingWindow.OnSelectCallback callback)
    {
      Rect screenRect = GUIUtility.GUIToScreenRect(new Rect(pos.x, pos.y - 16f, 16f, 16f));
      data.Sort((Comparison<SceneHierarchySortingWindow.InputData>) ((lhs, rhs) => lhs.m_Name.CompareTo(rhs.m_Name)));
      this.m_Data = data;
      this.m_Callback = callback;
      if (SceneHierarchySortingWindow.s_Styles == null)
        SceneHierarchySortingWindow.s_Styles = new SceneHierarchySortingWindow.Styles();
      Vector2 windowSize = new Vector2(2f + this.GetWidth(), 2f + this.GetHeight());
      this.ShowAsDropDown(screenRect, windowSize);
    }

    internal void OnGUI()
    {
      if (Event.current.type == EventType.Layout)
        return;
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      this.Draw();
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, SceneHierarchySortingWindow.s_Styles.background);
    }

    private void Draw()
    {
      Rect rect = new Rect(1f, 1f, this.position.width - 2f, 16f);
      using (List<SceneHierarchySortingWindow.InputData>.Enumerator enumerator = this.m_Data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SceneHierarchySortingWindow.InputData current = enumerator.Current;
          this.DrawListElement(rect, current);
          rect.y += 16f;
        }
      }
    }

    private void DrawListElement(Rect rect, SceneHierarchySortingWindow.InputData data)
    {
      EditorGUI.BeginChangeCheck();
      GUI.Toggle(rect, data.m_Selected, EditorGUIUtility.TempContent(data.m_Name), SceneHierarchySortingWindow.s_Styles.menuItem);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_Callback(data);
      this.Close();
    }

    private class Styles
    {
      public GUIStyle background = (GUIStyle) "grey_border";
      public GUIStyle menuItem = (GUIStyle) "MenuItem";
    }

    public class InputData
    {
      public string m_TypeName;
      public string m_Name;
      public bool m_Selected;
    }

    public delegate void OnSelectCallback(SceneHierarchySortingWindow.InputData element);
  }
}
