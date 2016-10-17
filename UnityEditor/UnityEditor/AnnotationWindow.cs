// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnnotationWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AnnotationWindow : EditorWindow
  {
    private GUIContent iconToggleContent = new GUIContent(string.Empty, "Show/Hide Icon");
    private GUIContent iconSelectContent = new GUIContent(string.Empty, "Select Icon");
    private GUIContent icon3dGizmoContent = new GUIContent("3D Icons");
    private GUIContent showGridContent = new GUIContent("Show Grid");
    private const float kWindowWidth = 270f;
    private const float scrollBarWidth = 14f;
    private const float listElementHeight = 18f;
    private const float gizmoRightAlign = 23f;
    private const float iconRightAlign = 64f;
    private const float frameWidth = 1f;
    private const float k_AnimDuration = 0.4f;
    private const int maxShowRecent = 5;
    private const string textGizmoVisible = "Show/Hide Gizmo";
    private const float exponentStart = -3f;
    private const float exponentRange = 3f;
    private const string kAlwaysFullSizeText = "Always Full Size";
    private const string kHideAllIconsText = "Hide All Icons";
    private static bool s_Debug;
    private static AnnotationWindow s_AnnotationWindow;
    private static long s_LastClosedTime;
    private static AnnotationWindow.Styles m_Styles;
    private List<AInfo> m_RecentAnnotations;
    private List<AInfo> m_BuiltinAnnotations;
    private List<AInfo> m_ScriptAnnotations;
    private Vector2 m_ScrollPosition;
    private bool m_SyncWithState;
    private string m_LastScriptThatHasShownTheIconSelector;
    private List<MonoScript> m_MonoScriptIconsChanged;
    private bool m_IsGameView;

    private AnnotationWindow()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    private static float ConvertTexelWorldSizeTo01(float texelWorldSize)
    {
      if ((double) texelWorldSize == -1.0)
        return 1f;
      if ((double) texelWorldSize == 0.0)
        return 0.0f;
      return (float) (((double) Mathf.Log10(texelWorldSize) - -3.0) / 3.0);
    }

    private static float Convert01ToTexelWorldSize(float value01)
    {
      if ((double) value01 <= 0.0)
        return 0.0f;
      return Mathf.Pow(10f, (float) (3.0 * (double) value01 - 3.0));
    }

    private static string ConvertTexelWorldSizeToString(float texelWorldSize)
    {
      if ((double) texelWorldSize == -1.0)
        return "Always Full Size";
      if ((double) texelWorldSize == 0.0)
        return "Hide All Icons";
      float num = texelWorldSize * 32f;
      int minimumDifference = MathUtils.GetNumberOfDecimalsForMinimumDifference(num * 0.1f);
      return num.ToString("N" + (object) minimumDifference);
    }

    public void MonoScriptIconChanged(MonoScript monoScript)
    {
      if ((UnityEngine.Object) monoScript == (UnityEngine.Object) null)
        return;
      bool flag = true;
      using (List<MonoScript>.Enumerator enumerator = this.m_MonoScriptIconsChanged.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.GetInstanceID() == monoScript.GetInstanceID())
            flag = false;
        }
      }
      if (!flag)
        return;
      this.m_MonoScriptIconsChanged.Add(monoScript);
    }

    public static void IconChanged()
    {
      if (!((UnityEngine.Object) AnnotationWindow.s_AnnotationWindow != (UnityEngine.Object) null))
        return;
      AnnotationWindow.s_AnnotationWindow.IconHasChanged();
    }

    private float GetTopSectionHeight()
    {
      return 50f;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
      using (List<MonoScript>.Enumerator enumerator = this.m_MonoScriptIconsChanged.GetEnumerator())
      {
        while (enumerator.MoveNext())
          MonoImporter.CopyMonoScriptIconToImporters(enumerator.Current);
      }
      AnnotationWindow.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      AnnotationWindow.s_AnnotationWindow = (AnnotationWindow) null;
    }

    internal static bool ShowAtPosition(Rect buttonRect, bool isGameView)
    {
      if (DateTime.Now.Ticks / 10000L < AnnotationWindow.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) AnnotationWindow.s_AnnotationWindow == (UnityEngine.Object) null)
        AnnotationWindow.s_AnnotationWindow = ScriptableObject.CreateInstance<AnnotationWindow>();
      AnnotationWindow.s_AnnotationWindow.Init(buttonRect, isGameView);
      return true;
    }

    private void Init(Rect buttonRect, bool isGameView)
    {
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      this.m_MonoScriptIconsChanged = new List<MonoScript>();
      this.m_SyncWithState = true;
      this.m_IsGameView = isGameView;
      this.SyncToState();
      Vector2 windowSize = new Vector2(270f, Mathf.Min(2f + this.GetTopSectionHeight() + this.DrawNormalList(false, 100f, 0.0f, 10000f), 900f));
      this.ShowAsDropDown(buttonRect, windowSize);
    }

    private void IconHasChanged()
    {
      if (string.IsNullOrEmpty(this.m_LastScriptThatHasShownTheIconSelector))
        return;
      using (List<AInfo>.Enumerator enumerator = this.m_ScriptAnnotations.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AInfo current = enumerator.Current;
          if (current.m_ScriptClass == this.m_LastScriptThatHasShownTheIconSelector && !current.m_IconEnabled)
          {
            current.m_IconEnabled = true;
            this.SetIconState(current);
            break;
          }
        }
      }
      this.Repaint();
    }

    private void Cancel()
    {
      this.Close();
      GUI.changed = true;
      GUIUtility.ExitGUI();
    }

    private AInfo GetAInfo(int classID, string scriptClass)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnnotationWindow.\u003CGetAInfo\u003Ec__AnonStorey4B ainfoCAnonStorey4B = new AnnotationWindow.\u003CGetAInfo\u003Ec__AnonStorey4B();
      // ISSUE: reference to a compiler-generated field
      ainfoCAnonStorey4B.scriptClass = scriptClass;
      // ISSUE: reference to a compiler-generated field
      ainfoCAnonStorey4B.classID = classID;
      // ISSUE: reference to a compiler-generated field
      if (ainfoCAnonStorey4B.scriptClass != string.Empty)
      {
        // ISSUE: reference to a compiler-generated method
        return this.m_ScriptAnnotations.Find(new Predicate<AInfo>(ainfoCAnonStorey4B.\u003C\u003Em__84));
      }
      // ISSUE: reference to a compiler-generated method
      return this.m_BuiltinAnnotations.Find(new Predicate<AInfo>(ainfoCAnonStorey4B.\u003C\u003Em__85));
    }

    private void SyncToState()
    {
      Annotation[] annotations = AnnotationUtility.GetAnnotations();
      string str = string.Empty;
      if (AnnotationWindow.s_Debug)
        str += "AnnotationWindow: SyncToState\n";
      this.m_BuiltinAnnotations = new List<AInfo>();
      this.m_ScriptAnnotations = new List<AInfo>();
      for (int index = 0; index < annotations.Length; ++index)
      {
        if (AnnotationWindow.s_Debug)
          str = str + "   same as below: icon " + (object) annotations[index].iconEnabled + " gizmo " + (object) annotations[index].gizmoEnabled + "\n";
        AInfo ainfo = new AInfo(annotations[index].gizmoEnabled == 1, annotations[index].iconEnabled == 1, annotations[index].flags, annotations[index].classID, annotations[index].scriptClass);
        if (ainfo.m_ScriptClass == string.Empty)
        {
          this.m_BuiltinAnnotations.Add(ainfo);
          if (AnnotationWindow.s_Debug)
            str = str + "   " + BaseObjectTools.ClassIDToString(ainfo.m_ClassID) + ": icon " + (object) ainfo.m_IconEnabled + " gizmo " + (object) ainfo.m_GizmoEnabled + "\n";
        }
        else
        {
          this.m_ScriptAnnotations.Add(ainfo);
          if (AnnotationWindow.s_Debug)
            str = str + "   " + annotations[index].scriptClass + ": icon " + (object) ainfo.m_IconEnabled + " gizmo " + (object) ainfo.m_GizmoEnabled + "\n";
        }
      }
      this.m_BuiltinAnnotations.Sort();
      this.m_ScriptAnnotations.Sort();
      this.m_RecentAnnotations = new List<AInfo>();
      Annotation[] changedAnnotations = AnnotationUtility.GetRecentlyChangedAnnotations();
      for (int index = 0; index < changedAnnotations.Length && index < 5; ++index)
      {
        AInfo ainfo = this.GetAInfo(changedAnnotations[index].classID, changedAnnotations[index].scriptClass);
        if (ainfo != null)
          this.m_RecentAnnotations.Add(ainfo);
      }
      this.m_SyncWithState = false;
      if (!AnnotationWindow.s_Debug)
        return;
      Debug.Log((object) str);
    }

    internal void OnGUI()
    {
      if (Event.current.type == EventType.Layout)
        return;
      if (AnnotationWindow.m_Styles == null)
        AnnotationWindow.m_Styles = new AnnotationWindow.Styles();
      if (this.m_SyncWithState)
        this.SyncToState();
      float topSectionHeight = this.GetTopSectionHeight();
      this.DrawTopSection(topSectionHeight);
      this.DrawAnnotationList(topSectionHeight, this.position.height - topSectionHeight);
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, AnnotationWindow.m_Styles.background);
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.Cancel();
    }

    private void DrawTopSection(float topSectionHeight)
    {
      GUI.Label(new Rect(1f, 0.0f, this.position.width - 2f, topSectionHeight), string.Empty, EditorStyles.inspectorBig);
      float num1 = 7f;
      float num2 = 11f;
      float y1 = num1;
      Rect position = new Rect(num2 - 2f, y1, 80f, 20f);
      AnnotationUtility.use3dGizmos = GUI.Toggle(position, AnnotationUtility.use3dGizmos, this.icon3dGizmoContent);
      float iconSize = AnnotationUtility.iconSize;
      if (AnnotationWindow.s_Debug)
        GUI.Label(new Rect(0.0f, y1 + 10f, this.position.width - num2, 20f), AnnotationWindow.ConvertTexelWorldSizeToString(iconSize), AnnotationWindow.m_Styles.texelWorldSizeStyle);
      EditorGUI.BeginDisabledGroup(!AnnotationUtility.use3dGizmos);
      float width = 160f;
      float num3 = AnnotationWindow.ConvertTexelWorldSizeTo01(iconSize);
      float num4 = GUI.HorizontalSlider(new Rect(this.position.width - num2 - width, y1, width, 20f), num3, 0.0f, 1f);
      if (GUI.changed)
      {
        AnnotationUtility.iconSize = AnnotationWindow.Convert01ToTexelWorldSize(num4);
        SceneView.RepaintAll();
      }
      EditorGUI.EndDisabledGroup();
      float y2 = y1 + 20f;
      EditorGUI.BeginDisabledGroup(this.m_IsGameView);
      position = new Rect(num2 - 2f, y2, 80f, 20f);
      AnnotationUtility.showGrid = GUI.Toggle(position, AnnotationUtility.showGrid, this.showGridContent);
      EditorGUI.EndDisabledGroup();
    }

    private void DrawAnnotationList(float startY, float height)
    {
      Rect position = new Rect(1f, startY + 1f, this.position.width - 2f, (float) ((double) height - 1.0 - 1.0));
      float height1 = this.DrawNormalList(false, 0.0f, 0.0f, 100000f);
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, height1);
      bool flag = (double) height1 > (double) position.height;
      float width = position.width;
      if (flag)
        width -= 14f;
      this.m_ScrollPosition = GUI.BeginScrollView(position, this.m_ScrollPosition, viewRect);
      double num = (double) this.DrawNormalList(true, width, this.m_ScrollPosition.y - 18f, this.m_ScrollPosition.y + height1);
      GUI.EndScrollView();
    }

    private void Flip(ref bool even)
    {
      even = !even;
    }

    private float DrawNormalList(bool doDraw, float listElementWidth, float startY, float endY)
    {
      bool even = true;
      float y = 0.0f;
      bool headerDrawn = false;
      return this.DrawListSection(this.DrawListSection(this.DrawListSection(y, "Recently Changed", this.m_RecentAnnotations, doDraw, listElementWidth, startY, endY, ref even, true, ref headerDrawn), "Scripts", this.m_ScriptAnnotations, doDraw, listElementWidth, startY, endY, ref even, false, ref headerDrawn), "Built-in Components", this.m_BuiltinAnnotations, doDraw, listElementWidth, startY, endY, ref even, false, ref headerDrawn);
    }

    private float DrawListSection(float y, string sectionHeader, List<AInfo> listElements, bool doDraw, float listElementWidth, float startY, float endY, ref bool even, bool useSeperator, ref bool headerDrawn)
    {
      float y1 = y;
      if (listElements.Count > 0)
      {
        if (doDraw)
        {
          Rect position = new Rect(1f, y1, listElementWidth - 2f, 30f);
          this.Flip(ref even);
          GUIStyle style = !even ? AnnotationWindow.m_Styles.listOddBg : AnnotationWindow.m_Styles.listEvenBg;
          GUI.Label(position, GUIContent.Temp(string.Empty), style);
        }
        float y2 = y1 + 10f;
        if (doDraw)
          this.DrawListHeader(sectionHeader, new Rect(3f, y2, listElementWidth, 20f), ref headerDrawn);
        y1 = y2 + 20f;
        for (int index = 0; index < listElements.Count; ++index)
        {
          this.Flip(ref even);
          if ((double) y1 > (double) startY && (double) y1 < (double) endY)
          {
            Rect rect = new Rect(1f, y1, listElementWidth - 2f, 18f);
            if (doDraw)
              this.DrawListElement(rect, even, listElements[index]);
          }
          y1 += 18f;
        }
        if (useSeperator)
        {
          float height = 6f;
          if (doDraw)
          {
            GUIStyle style = !even ? AnnotationWindow.m_Styles.listOddBg : AnnotationWindow.m_Styles.listEvenBg;
            GUI.Label(new Rect(1f, y1, listElementWidth - 2f, height), GUIContent.Temp(string.Empty), style);
            GUI.Label(new Rect(10f, y1 + 3f, listElementWidth - 15f, 3f), GUIContent.Temp(string.Empty), AnnotationWindow.m_Styles.seperator);
          }
          y1 += height;
        }
      }
      return y1;
    }

    private void DrawListHeader(string header, Rect rect, ref bool headerDrawn)
    {
      GUI.Label(rect, GUIContent.Temp(header), AnnotationWindow.m_Styles.listHeaderStyle);
      if (headerDrawn)
        return;
      headerDrawn = true;
      GUI.color = new Color(1f, 1f, 1f, 0.65f);
      Rect position = rect;
      position.y += -10f;
      position.x = rect.width - 32f;
      GUI.Label(position, "gizmo", AnnotationWindow.m_Styles.columnHeaderStyle);
      position.x = rect.width - 64f;
      GUI.Label(position, "icon", AnnotationWindow.m_Styles.columnHeaderStyle);
      GUI.color = Color.white;
    }

    private void DrawListElement(Rect rect, bool even, AInfo ainfo)
    {
      if (ainfo == null)
      {
        Debug.LogError((object) "DrawListElement: AInfo not valid!");
      }
      else
      {
        float num1 = 17f;
        float a = 0.3f;
        bool changed = GUI.changed;
        bool enabled = GUI.enabled;
        Color color = GUI.color;
        GUI.changed = false;
        GUI.enabled = true;
        GUIStyle style = !even ? AnnotationWindow.m_Styles.listOddBg : AnnotationWindow.m_Styles.listEvenBg;
        GUI.Label(rect, GUIContent.Temp(string.Empty), style);
        Rect position1 = rect;
        position1.width = (float) ((double) rect.width - 64.0 - 22.0);
        GUI.Label(position1, ainfo.m_DisplayText, AnnotationWindow.m_Styles.listTextStyle);
        float num2 = 16f;
        Rect position2 = new Rect(rect.width - 64f, rect.y + (float) (((double) rect.height - (double) num2) * 0.5), num2, num2);
        Texture texture = (Texture) null;
        if (ainfo.m_ScriptClass != string.Empty)
        {
          texture = (Texture) EditorGUIUtility.GetIconForObject(EditorGUIUtility.GetScript(ainfo.m_ScriptClass));
          Rect position3 = position2;
          position3.x += 18f;
          ++position3.y;
          position3.width = 1f;
          position3.height = 12f;
          GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.13f) : new Color(0.0f, 0.0f, 0.0f, 0.33f);
          GUI.DrawTexture(position3, (Texture) EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
          GUI.color = Color.white;
          Rect rect1 = position2;
          rect1.x += 18f;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Rect& local = @rect1;
          // ISSUE: explicit reference operation
          double y = (double) (^local).y;
          // ISSUE: explicit reference operation
          (^local).y = (float) y;
          rect1.width = 9f;
          if (GUI.Button(rect1, this.iconSelectContent, AnnotationWindow.m_Styles.iconDropDown))
          {
            UnityEngine.Object script = EditorGUIUtility.GetScript(ainfo.m_ScriptClass);
            if (script != (UnityEngine.Object) null)
            {
              this.m_LastScriptThatHasShownTheIconSelector = ainfo.m_ScriptClass;
              if (IconSelector.ShowAtPosition(script, rect1, true))
              {
                IconSelector.SetMonoScriptIconChangedCallback(new IconSelector.MonoScriptIconChangedCallback(this.MonoScriptIconChanged));
                GUIUtility.ExitGUI();
              }
            }
          }
        }
        else if (ainfo.HasIcon())
          texture = (Texture) AssetPreview.GetMiniTypeThumbnailFromClassID(ainfo.m_ClassID);
        if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
        {
          if (!ainfo.m_IconEnabled)
          {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
            string empty = string.Empty;
          }
          this.iconToggleContent.image = texture;
          if (GUI.Button(position2, this.iconToggleContent, GUIStyle.none))
          {
            ainfo.m_IconEnabled = !ainfo.m_IconEnabled;
            this.SetIconState(ainfo);
          }
          GUI.color = color;
        }
        if (GUI.changed)
        {
          this.SetIconState(ainfo);
          GUI.changed = false;
        }
        GUI.enabled = true;
        GUI.color = color;
        if (ainfo.HasGizmo())
        {
          string tooltip = "Show/Hide Gizmo";
          Rect position3 = new Rect(rect.width - 23f, rect.y + (float) (((double) rect.height - (double) num1) * 0.5), num1, num1);
          ainfo.m_GizmoEnabled = GUI.Toggle(position3, ainfo.m_GizmoEnabled, new GUIContent(string.Empty, tooltip), AnnotationWindow.m_Styles.toggle);
          if (GUI.changed)
            this.SetGizmoState(ainfo);
        }
        GUI.enabled = enabled;
        GUI.changed = changed;
        GUI.color = color;
      }
    }

    private void SetIconState(AInfo ainfo)
    {
      AnnotationUtility.SetIconEnabled(ainfo.m_ClassID, ainfo.m_ScriptClass, !ainfo.m_IconEnabled ? 0 : 1);
      SceneView.RepaintAll();
    }

    private void SetGizmoState(AInfo ainfo)
    {
      AnnotationUtility.SetGizmoEnabled(ainfo.m_ClassID, ainfo.m_ScriptClass, !ainfo.m_GizmoEnabled ? 0 : 1);
      SceneView.RepaintAll();
    }

    private class Styles
    {
      public GUIStyle toolbar = (GUIStyle) "toolbar";
      public GUIStyle toggle = (GUIStyle) "OL Toggle";
      public GUIStyle listEvenBg = (GUIStyle) "ObjectPickerResultsOdd";
      public GUIStyle listOddBg = (GUIStyle) "ObjectPickerResultsEven";
      public GUIStyle listSectionHeaderBg = (GUIStyle) "ObjectPickerResultsEven";
      public GUIStyle background = (GUIStyle) "grey_border";
      public GUIStyle seperator = (GUIStyle) "sv_iconselector_sep";
      public GUIStyle iconDropDown = (GUIStyle) "IN dropdown";
      public GUIStyle listTextStyle;
      public GUIStyle listHeaderStyle;
      public GUIStyle texelWorldSizeStyle;
      public GUIStyle columnHeaderStyle;

      public Styles()
      {
        this.listTextStyle = new GUIStyle(EditorStyles.label);
        this.listTextStyle.alignment = TextAnchor.MiddleLeft;
        this.listTextStyle.padding.left = 10;
        this.listHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
        this.listHeaderStyle.padding.left = 5;
        this.texelWorldSizeStyle = new GUIStyle(EditorStyles.label);
        this.texelWorldSizeStyle.alignment = TextAnchor.UpperRight;
        this.texelWorldSizeStyle.font = EditorStyles.miniLabel.font;
        this.texelWorldSizeStyle.fontSize = EditorStyles.miniLabel.fontSize;
        this.texelWorldSizeStyle.padding.right = 0;
        this.columnHeaderStyle = new GUIStyle(EditorStyles.miniLabel);
      }
    }
  }
}
