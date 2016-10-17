// Decompiled with JetBrains decompiler
// Type: UnityEditor.IconSelector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class IconSelector : EditorWindow
  {
    private static IconSelector s_IconSelector = (IconSelector) null;
    private static long s_LastClosedTime = 0;
    private static int s_LastInstanceID = -1;
    private static int s_HashIconSelector = "IconSelector".GetHashCode();
    private static IconSelector.Styles m_Styles;
    private UnityEngine.Object m_TargetObject;
    private Texture2D m_StartIcon;
    private bool m_ShowLabelIcons;
    private GUIContent[] m_LabelLargeIcons;
    private GUIContent[] m_LabelIcons;
    private GUIContent[] m_LargeIcons;
    private GUIContent[] m_SmallIcons;
    private GUIContent m_NoneButtonContent;
    private IconSelector.MonoScriptIconChangedCallback m_MonoScriptIconChangedCallback;

    private IconSelector()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    private GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
    {
      GUIContent[] guiContentArray = new GUIContent[count];
      for (int index = 0; index < count; ++index)
        guiContentArray[index] = EditorGUIUtility.IconContent(baseName + (object) (startIndex + index) + postFix);
      return guiContentArray;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
      this.SaveIconChanges();
      IconSelector.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      IconSelector.s_IconSelector = (IconSelector) null;
    }

    private void SaveIconChanges()
    {
      if (!((UnityEngine.Object) EditorGUIUtility.GetIconForObject(this.m_TargetObject) != (UnityEngine.Object) this.m_StartIcon))
        return;
      MonoScript targetObject = this.m_TargetObject as MonoScript;
      if (!((UnityEngine.Object) targetObject != (UnityEngine.Object) null))
        return;
      if (this.m_MonoScriptIconChangedCallback != null)
        this.m_MonoScriptIconChangedCallback(targetObject);
      else
        MonoImporter.CopyMonoScriptIconToImporters(targetObject);
    }

    internal static bool ShowAtPosition(UnityEngine.Object targetObj, Rect activatorRect, bool showLabelIcons)
    {
      int instanceId = targetObj.GetInstanceID();
      bool flag = DateTime.Now.Ticks / 10000L < IconSelector.s_LastClosedTime + 50L;
      if (instanceId == IconSelector.s_LastInstanceID && flag)
        return false;
      Event.current.Use();
      IconSelector.s_LastInstanceID = instanceId;
      if ((UnityEngine.Object) IconSelector.s_IconSelector == (UnityEngine.Object) null)
        IconSelector.s_IconSelector = ScriptableObject.CreateInstance<IconSelector>();
      IconSelector.s_IconSelector.Init(targetObj, activatorRect, showLabelIcons);
      return true;
    }

    internal static void SetMonoScriptIconChangedCallback(IconSelector.MonoScriptIconChangedCallback callback)
    {
      if ((UnityEngine.Object) IconSelector.s_IconSelector != (UnityEngine.Object) null)
        IconSelector.s_IconSelector.m_MonoScriptIconChangedCallback = callback;
      else
        Debug.Log((object) "ERROR: setting callback on hidden IconSelector");
    }

    private void Init(UnityEngine.Object targetObj, Rect activatorRect, bool showLabelIcons)
    {
      this.m_TargetObject = targetObj;
      this.m_StartIcon = EditorGUIUtility.GetIconForObject(this.m_TargetObject);
      this.m_ShowLabelIcons = showLabelIcons;
      Rect screenRect = GUIUtility.GUIToScreenRect(activatorRect);
      GUIUtility.keyboardControl = 0;
      this.m_LabelLargeIcons = this.GetTextures("sv_label_", string.Empty, 0, 8);
      this.m_LabelIcons = this.GetTextures("sv_icon_name", string.Empty, 0, 8);
      this.m_SmallIcons = this.GetTextures("sv_icon_dot", "_sml", 0, 16);
      this.m_LargeIcons = this.GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
      this.m_NoneButtonContent = EditorGUIUtility.IconContent("sv_icon_none");
      this.m_NoneButtonContent.text = "None";
      float x = 140f;
      float y = 86f;
      if (this.m_ShowLabelIcons)
        y = 126f;
      this.ShowAsDropDown(screenRect, new Vector2(x, y));
    }

    private Texture2D ConvertLargeIconToSmallIcon(Texture2D largeIcon, ref bool isLabelIcon)
    {
      if ((UnityEngine.Object) largeIcon == (UnityEngine.Object) null)
        return (Texture2D) null;
      isLabelIcon = true;
      for (int index = 0; index < this.m_LabelLargeIcons.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_LabelLargeIcons[index].image == (UnityEngine.Object) largeIcon)
          return (Texture2D) this.m_LabelIcons[index].image;
      }
      isLabelIcon = false;
      for (int index = 0; index < this.m_LargeIcons.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_LargeIcons[index].image == (UnityEngine.Object) largeIcon)
          return (Texture2D) this.m_SmallIcons[index].image;
      }
      return largeIcon;
    }

    private Texture2D ConvertSmallIconToLargeIcon(Texture2D smallIcon, bool labelIcon)
    {
      if (labelIcon)
      {
        for (int index = 0; index < this.m_LabelIcons.Length; ++index)
        {
          if ((UnityEngine.Object) this.m_LabelIcons[index].image == (UnityEngine.Object) smallIcon)
            return (Texture2D) this.m_LabelLargeIcons[index].image;
        }
      }
      else
      {
        for (int index = 0; index < this.m_SmallIcons.Length; ++index)
        {
          if ((UnityEngine.Object) this.m_SmallIcons[index].image == (UnityEngine.Object) smallIcon)
            return (Texture2D) this.m_LargeIcons[index].image;
        }
      }
      return smallIcon;
    }

    private void DoButton(GUIContent content, Texture2D selectedIcon, bool labelIcon)
    {
      int controlId = GUIUtility.GetControlID(IconSelector.s_HashIconSelector, FocusType.Keyboard);
      if ((UnityEngine.Object) content.image == (UnityEngine.Object) selectedIcon)
      {
        Rect position = GUILayoutUtility.topLevel.PeekNext();
        float num = 2f;
        position.x -= num;
        position.y -= num;
        position.width = (float) selectedIcon.width + 2f * num;
        position.height = (float) selectedIcon.height + 2f * num;
        GUI.Label(position, GUIContent.none, !labelIcon ? IconSelector.m_Styles.selection : IconSelector.m_Styles.selectionLabel);
      }
      if (!EditorGUILayout.IconButton(controlId, content, GUIStyle.none))
        return;
      EditorGUIUtility.SetIconForObject(this.m_TargetObject, this.ConvertSmallIconToLargeIcon((Texture2D) content.image, labelIcon));
      EditorUtility.ForceReloadInspectors();
      AnnotationWindow.IconChanged();
      if (Event.current.clickCount != 2)
        return;
      this.CloseWindow();
    }

    private void DoTopSection(bool anySelected)
    {
      GUI.Label(new Rect(6f, 4f, 110f, 20f), "Select Icon");
      EditorGUI.BeginDisabledGroup(!anySelected);
      if (GUI.Button(new Rect(93f, 6f, 43f, 12f), this.m_NoneButtonContent, IconSelector.m_Styles.noneButton))
      {
        EditorGUIUtility.SetIconForObject(this.m_TargetObject, (Texture2D) null);
        EditorUtility.ForceReloadInspectors();
        AnnotationWindow.IconChanged();
      }
      EditorGUI.EndDisabledGroup();
    }

    private void CloseWindow()
    {
      this.Close();
      GUI.changed = true;
      GUIUtility.ExitGUI();
    }

    internal void OnGUI()
    {
      if (IconSelector.m_Styles == null)
        IconSelector.m_Styles = new IconSelector.Styles();
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        this.CloseWindow();
      Texture2D texture2D = EditorGUIUtility.GetIconForObject(this.m_TargetObject);
      bool isLabelIcon = false;
      if (Event.current.type == EventType.Repaint)
        texture2D = this.ConvertLargeIconToSmallIcon(texture2D, ref isLabelIcon);
      Event current = Event.current;
      EventType type = current.type;
      GUI.BeginGroup(new Rect(0.0f, 0.0f, this.position.width, this.position.height), IconSelector.m_Styles.background);
      this.DoTopSection((UnityEngine.Object) texture2D != (UnityEngine.Object) null);
      GUILayout.Space(22f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(1f);
      GUI.enabled = false;
      GUILayout.Label(string.Empty, IconSelector.m_Styles.seperator, new GUILayoutOption[0]);
      GUI.enabled = true;
      GUILayout.Space(1f);
      GUILayout.EndHorizontal();
      GUILayout.Space(3f);
      if (this.m_ShowLabelIcons)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Space(6f);
        for (int index = 0; index < this.m_LabelIcons.Length / 2; ++index)
          this.DoButton(this.m_LabelIcons[index], texture2D, true);
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(6f);
        for (int index = this.m_LabelIcons.Length / 2; index < this.m_LabelIcons.Length; ++index)
          this.DoButton(this.m_LabelIcons[index], texture2D, true);
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(1f);
        GUI.enabled = false;
        GUILayout.Label(string.Empty, IconSelector.m_Styles.seperator, new GUILayoutOption[0]);
        GUI.enabled = true;
        GUILayout.Space(1f);
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
      }
      GUILayout.BeginHorizontal();
      GUILayout.Space(9f);
      for (int index = 0; index < this.m_SmallIcons.Length / 2; ++index)
        this.DoButton(this.m_SmallIcons[index], texture2D, false);
      GUILayout.Space(3f);
      GUILayout.EndHorizontal();
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(9f);
      for (int index = this.m_SmallIcons.Length / 2; index < this.m_SmallIcons.Length; ++index)
        this.DoButton(this.m_SmallIcons[index], texture2D, false);
      GUILayout.Space(3f);
      GUILayout.EndHorizontal();
      GUILayout.Space(6f);
      GUI.backgroundColor = new Color(1f, 1f, 1f, 0.7f);
      bool flag = false;
      int controlId = GUIUtility.GetControlID(IconSelector.s_HashIconSelector, FocusType.Keyboard);
      if (GUILayout.Button(EditorGUIUtility.TempContent("Other...")))
      {
        GUIUtility.keyboardControl = controlId;
        flag = true;
      }
      GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
      GUI.EndGroup();
      if (flag)
      {
        ObjectSelector.get.Show(this.m_TargetObject, typeof (Texture2D), (SerializedProperty) null, false);
        ObjectSelector.get.objectSelectorID = controlId;
        GUI.backgroundColor = new Color(1f, 1f, 1f, 0.7f);
        current.Use();
        GUIUtility.ExitGUI();
      }
      if (type != EventType.ExecuteCommand || !(current.commandName == "ObjectSelectorUpdated") || (ObjectSelector.get.objectSelectorID != controlId || GUIUtility.keyboardControl != controlId))
        return;
      EditorGUIUtility.SetIconForObject(this.m_TargetObject, ObjectSelector.GetCurrentObject() as Texture2D);
      GUI.changed = true;
      current.Use();
    }

    private class Styles
    {
      public GUIStyle background = (GUIStyle) "sv_iconselector_back";
      public GUIStyle seperator = (GUIStyle) "sv_iconselector_sep";
      public GUIStyle selection = (GUIStyle) "sv_iconselector_selection";
      public GUIStyle selectionLabel = (GUIStyle) "sv_iconselector_labelselection";
      public GUIStyle noneButton = (GUIStyle) "sv_iconselector_button";
    }

    public delegate void MonoScriptIconChangedCallback(MonoScript monoScript);
  }
}
