// Decompiled with JetBrains decompiler
// Type: UnityEditor.LayerVisibilityWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class LayerVisibilityWindow : EditorWindow
  {
    private const float kScrollBarWidth = 14f;
    private const float kFrameWidth = 1f;
    private const float kToggleSize = 17f;
    private const float kSeparatorHeight = 6f;
    private const string kLayerVisible = "Show/Hide Layer";
    private const string kLayerLocked = "Lock Layer for Picking";
    private static LayerVisibilityWindow s_LayerVisibilityWindow;
    private static long s_LastClosedTime;
    private static LayerVisibilityWindow.Styles s_Styles;
    private List<string> s_LayerNames;
    private List<int> s_LayerMasks;
    private int m_AllLayersMask;
    private float m_ContentHeight;
    private Vector2 m_ScrollPosition;

    private LayerVisibilityWindow()
    {
      this.hideFlags = HideFlags.DontSave;
      this.wantsMouseMove = true;
    }

    private void CalcValidLayers()
    {
      this.s_LayerNames = new List<string>();
      this.s_LayerMasks = new List<int>();
      this.m_AllLayersMask = 0;
      for (int layer = 0; layer < 32; ++layer)
      {
        string layerName = InternalEditorUtility.GetLayerName(layer);
        if (!(layerName == string.Empty))
        {
          this.s_LayerNames.Add(layerName);
          this.s_LayerMasks.Add(layer);
          this.m_AllLayersMask |= 1 << layer;
        }
      }
    }

    internal void OnDisable()
    {
      LayerVisibilityWindow.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      LayerVisibilityWindow.s_LayerVisibilityWindow = (LayerVisibilityWindow) null;
    }

    internal static bool ShowAtPosition(Rect buttonRect)
    {
      if (DateTime.Now.Ticks / 10000L < LayerVisibilityWindow.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) LayerVisibilityWindow.s_LayerVisibilityWindow == (UnityEngine.Object) null)
        LayerVisibilityWindow.s_LayerVisibilityWindow = ScriptableObject.CreateInstance<LayerVisibilityWindow>();
      LayerVisibilityWindow.s_LayerVisibilityWindow.Init(buttonRect);
      return true;
    }

    private void Init(Rect buttonRect)
    {
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      this.CalcValidLayers();
      float num = (float) ((double) (this.s_LayerNames.Count + 2 + 1 + 1) * 16.0 + 6.0);
      int sortingLayerCount = InternalEditorUtility.GetSortingLayerCount();
      if (sortingLayerCount > 1)
        num = num + 22f + (float) sortingLayerCount * 16f;
      this.m_ContentHeight = num;
      Vector2 windowSize = new Vector2(180f, Mathf.Min(num + 2f, 600f));
      this.ShowAsDropDown(buttonRect, windowSize);
    }

    internal void OnGUI()
    {
      if (Event.current.type == EventType.Layout)
        return;
      if (LayerVisibilityWindow.s_Styles == null)
        LayerVisibilityWindow.s_Styles = new LayerVisibilityWindow.Styles();
      Rect position = new Rect(1f, 1f, this.position.width - 2f, this.position.height - 2f);
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, this.m_ContentHeight);
      bool flag = (double) this.m_ContentHeight > (double) position.height;
      float width = position.width;
      if (flag)
        width -= 14f;
      this.m_ScrollPosition = GUI.BeginScrollView(position, this.m_ScrollPosition, viewRect);
      this.Draw(width);
      GUI.EndScrollView();
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, LayerVisibilityWindow.s_Styles.background);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.Close();
      GUIUtility.ExitGUI();
    }

    private void DrawListBackground(Rect rect, bool even)
    {
      GUIStyle style = !even ? LayerVisibilityWindow.s_Styles.listOddBg : LayerVisibilityWindow.s_Styles.listEvenBg;
      GUI.Label(rect, GUIContent.none, style);
    }

    private void DrawHeader(ref Rect rect, string text, ref bool even)
    {
      this.DrawListBackground(rect, even);
      GUI.Label(rect, GUIContent.Temp(text), LayerVisibilityWindow.s_Styles.listHeaderStyle);
      rect.y += 16f;
      even = !even;
    }

    private void DrawSeparator(ref Rect rect, bool even)
    {
      this.DrawListBackground(new Rect(rect.x + 1f, rect.y, rect.width - 2f, 6f), even);
      GUI.Label(new Rect(rect.x + 5f, rect.y + 3f, rect.width - 10f, 3f), GUIContent.none, LayerVisibilityWindow.s_Styles.separator);
      rect.y += 6f;
    }

    private void Draw(float listElementWidth)
    {
      Rect rect = new Rect(0.0f, 0.0f, listElementWidth, 16f);
      bool even = false;
      this.DrawHeader(ref rect, "Layers", ref even);
      this.DoSpecialLayer(rect, true, ref even);
      rect.y += 16f;
      this.DoSpecialLayer(rect, false, ref even);
      rect.y += 16f;
      for (int index = 0; index < this.s_LayerNames.Count; ++index)
      {
        this.DoOneLayer(rect, index, ref even);
        rect.y += 16f;
      }
      int sortingLayerCount = InternalEditorUtility.GetSortingLayerCount();
      if (sortingLayerCount > 1)
      {
        this.DrawSeparator(ref rect, even);
        this.DrawHeader(ref rect, "Sorting Layers", ref even);
        for (int index = 0; index < sortingLayerCount; ++index)
        {
          this.DoOneSortingLayer(rect, index, ref even);
          rect.y += 16f;
        }
      }
      this.DrawSeparator(ref rect, even);
      this.DrawListBackground(rect, even);
      if (!GUI.Button(rect, EditorGUIUtility.TempContent("Edit Layers..."), LayerVisibilityWindow.s_Styles.menuItem))
        return;
      this.Close();
      Selection.activeObject = EditorApplication.tagManager;
      GUIUtility.ExitGUI();
    }

    private void DoSpecialLayer(Rect rect, bool all, ref bool even)
    {
      bool visible = (Tools.visibleLayers & this.m_AllLayersMask) == (!all ? 0 : this.m_AllLayersMask);
      bool visibleChanged;
      bool lockedChanged;
      this.DoLayerEntry(rect, !all ? "Nothing" : "Everything", even, true, false, visible, false, out visibleChanged, out lockedChanged);
      if (visibleChanged)
      {
        Tools.visibleLayers = !all ? 0 : -1;
        LayerVisibilityWindow.RepaintAllSceneViews();
      }
      even = !even;
    }

    private void DoOneLayer(Rect rect, int index, ref bool even)
    {
      int visibleLayers = Tools.visibleLayers;
      int lockedLayers = Tools.lockedLayers;
      int num = 1 << this.s_LayerMasks[index];
      bool visible = (visibleLayers & num) != 0;
      bool locked = (lockedLayers & num) != 0;
      bool visibleChanged;
      bool lockedChanged;
      this.DoLayerEntry(rect, this.s_LayerNames[index], even, true, true, visible, locked, out visibleChanged, out lockedChanged);
      if (visibleChanged)
      {
        Tools.visibleLayers ^= num;
        LayerVisibilityWindow.RepaintAllSceneViews();
      }
      if (lockedChanged)
        Tools.lockedLayers ^= num;
      even = !even;
    }

    private void DoOneSortingLayer(Rect rect, int index, ref bool even)
    {
      bool sortingLayerLocked = InternalEditorUtility.GetSortingLayerLocked(index);
      bool visibleChanged;
      bool lockedChanged;
      this.DoLayerEntry(rect, InternalEditorUtility.GetSortingLayerName(index), even, false, true, true, sortingLayerLocked, out visibleChanged, out lockedChanged);
      if (lockedChanged)
        InternalEditorUtility.SetSortingLayerLocked(index, !sortingLayerLocked);
      even = !even;
    }

    private void DoLayerEntry(Rect rect, string layerName, bool even, bool showVisible, bool showLock, bool visible, bool locked, out bool visibleChanged, out bool lockedChanged)
    {
      this.DrawListBackground(rect, even);
      EditorGUI.BeginChangeCheck();
      Rect position1 = rect;
      position1.width -= 34f;
      visible = GUI.Toggle(position1, visible, EditorGUIUtility.TempContent(layerName), LayerVisibilityWindow.s_Styles.listTextStyle);
      Rect position2 = new Rect(rect.width - 34f, rect.y + (float) (((double) rect.height - 17.0) * 0.5), 17f, 17f);
      visibleChanged = false;
      if (showVisible)
      {
        Color color1 = GUI.color;
        Color color2 = color1;
        color2.a = !visible ? 0.4f : 0.6f;
        GUI.color = color2;
        Rect position3 = position2;
        position3.y += 3f;
        GUIContent content = new GUIContent(string.Empty, !visible ? (Texture) LayerVisibilityWindow.s_Styles.visibleOff : (Texture) LayerVisibilityWindow.s_Styles.visibleOn, "Show/Hide Layer");
        GUI.Toggle(position3, visible, content, GUIStyle.none);
        GUI.color = color1;
        visibleChanged = EditorGUI.EndChangeCheck();
      }
      lockedChanged = false;
      if (!showLock)
        return;
      position2.x += 17f;
      EditorGUI.BeginChangeCheck();
      Color backgroundColor = GUI.backgroundColor;
      Color color = backgroundColor;
      if (!locked)
        color.a *= 0.4f;
      GUI.backgroundColor = color;
      GUI.Toggle(position2, locked, new GUIContent(string.Empty, "Lock Layer for Picking"), LayerVisibilityWindow.s_Styles.lockButton);
      GUI.backgroundColor = backgroundColor;
      lockedChanged = EditorGUI.EndChangeCheck();
    }

    private static void RepaintAllSceneViews()
    {
      foreach (EditorWindow editorWindow in Resources.FindObjectsOfTypeAll(typeof (SceneView)))
        editorWindow.Repaint();
    }

    private class Styles
    {
      public readonly GUIStyle background = (GUIStyle) "grey_border";
      public readonly GUIStyle menuItem = (GUIStyle) "MenuItem";
      public readonly GUIStyle listEvenBg = (GUIStyle) "ObjectPickerResultsOdd";
      public readonly GUIStyle listOddBg = (GUIStyle) "ObjectPickerResultsEven";
      public readonly GUIStyle separator = (GUIStyle) "sv_iconselector_sep";
      public readonly GUIStyle lockButton = (GUIStyle) "IN LockButton";
      public readonly Texture2D visibleOn = EditorGUIUtility.LoadIcon("animationvisibilitytoggleon");
      public readonly Texture2D visibleOff = EditorGUIUtility.LoadIcon("animationvisibilitytoggleoff");
      public readonly GUIStyle listTextStyle;
      public readonly GUIStyle listHeaderStyle;

      public Styles()
      {
        this.listTextStyle = new GUIStyle(EditorStyles.label);
        this.listTextStyle.alignment = TextAnchor.MiddleLeft;
        this.listTextStyle.padding.left = 10;
        this.listHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
        this.listHeaderStyle.padding.left = 5;
      }
    }
  }
}
