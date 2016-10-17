// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class SpriteEditorMenu : EditorWindow
  {
    public readonly string[] spriteAlignmentOptions = new string[10]{ EditorGUIUtility.TextContent("Center").text, EditorGUIUtility.TextContent("Top Left").text, EditorGUIUtility.TextContent("Top").text, EditorGUIUtility.TextContent("Top Right").text, EditorGUIUtility.TextContent("Left").text, EditorGUIUtility.TextContent("Right").text, EditorGUIUtility.TextContent("Bottom Left").text, EditorGUIUtility.TextContent("Bottom").text, EditorGUIUtility.TextContent("Bottom Right").text, EditorGUIUtility.TextContent("Custom").text };
    public readonly string[] slicingMethodOptions = new string[3]{ EditorGUIUtility.TextContent("Delete Existing|Delete all existing sprite assets before the slicing operation").text, EditorGUIUtility.TextContent("Smart|Try to match existing sprite rects to sliced rects from the slicing operation").text, EditorGUIUtility.TextContent("Safe|Keep existing sprite rects intact").text };
    public static SpriteEditorMenu s_SpriteEditorMenu;
    private static SpriteEditorMenu.Styles s_Styles;
    private static long s_LastClosedTime;
    private static int s_Selected;
    private static SpriteEditorMenuSetting s_Setting;
    public static SpriteEditorWindow s_SpriteEditor;

    private void Init(Rect buttonRect)
    {
      if ((UnityEngine.Object) SpriteEditorMenu.s_Setting == (UnityEngine.Object) null)
        SpriteEditorMenu.s_Setting = ScriptableObject.CreateInstance<SpriteEditorMenuSetting>();
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      Vector2 windowSize = new Vector2(300f, 145f);
      this.ShowAsDropDown(buttonRect, windowSize);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void UndoRedoPerformed()
    {
      this.Repaint();
    }

    private void OnDisable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      SpriteEditorMenu.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      SpriteEditorMenu.s_SpriteEditorMenu = (SpriteEditorMenu) null;
    }

    internal static bool ShowAtPosition(Rect buttonRect)
    {
      if (DateTime.Now.Ticks / 10000L < SpriteEditorMenu.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditorMenu == (UnityEngine.Object) null)
        SpriteEditorMenu.s_SpriteEditorMenu = ScriptableObject.CreateInstance<SpriteEditorMenu>();
      SpriteEditorMenu.s_SpriteEditorMenu.Init(buttonRect);
      return true;
    }

    private void OnGUI()
    {
      if (SpriteEditorMenu.s_Styles == null)
        SpriteEditorMenu.s_Styles = new SpriteEditorMenu.Styles();
      GUILayout.Space(4f);
      EditorGUIUtility.labelWidth = 124f;
      EditorGUIUtility.wideMode = true;
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, SpriteEditorMenu.s_Styles.background);
      EditorGUI.BeginChangeCheck();
      SpriteEditorMenuSetting.SlicingType slicingType = (SpriteEditorMenuSetting.SlicingType) EditorGUILayout.EnumPopup("Type", (Enum) SpriteEditorMenu.s_Setting.slicingType, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change slicing type");
        SpriteEditorMenu.s_Setting.slicingType = slicingType;
      }
      switch (slicingType)
      {
        case SpriteEditorMenuSetting.SlicingType.Automatic:
          this.OnAutomaticGUI();
          break;
        case SpriteEditorMenuSetting.SlicingType.GridByCellSize:
        case SpriteEditorMenuSetting.SlicingType.GridByCellCount:
          this.OnGridGUI();
          break;
      }
      GUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.labelWidth + 4f);
      if (GUILayout.Button("Slice"))
        this.DoSlicing();
      GUILayout.EndHorizontal();
    }

    private void DoSlicing()
    {
      this.DoAnalytics();
      switch (SpriteEditorMenu.s_Setting.slicingType)
      {
        case SpriteEditorMenuSetting.SlicingType.Automatic:
          this.DoAutomaticSlicing();
          break;
        case SpriteEditorMenuSetting.SlicingType.GridByCellSize:
        case SpriteEditorMenuSetting.SlicingType.GridByCellCount:
          this.DoGridSlicing();
          break;
      }
    }

    private void DoAnalytics()
    {
      Analytics.Event("Sprite Editor", "Slice", "Type", (int) SpriteEditorMenu.s_Setting.slicingType);
      if ((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.originalTexture != (UnityEngine.Object) null)
      {
        Analytics.Event("Sprite Editor", "Slice", "Texture Width", SpriteEditorMenu.s_SpriteEditor.originalTexture.width);
        Analytics.Event("Sprite Editor", "Slice", "Texture Height", SpriteEditorMenu.s_SpriteEditor.originalTexture.height);
      }
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.Automatic)
      {
        Analytics.Event("Sprite Editor", "Slice", "Auto Slicing Method", SpriteEditorMenu.s_Setting.autoSlicingMethod);
      }
      else
      {
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Size X", (int) SpriteEditorMenu.s_Setting.gridSpriteSize.x);
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Size Y", (int) SpriteEditorMenu.s_Setting.gridSpriteSize.y);
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Offset X", (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.x);
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Offset Y", (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.y);
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Padding X", (int) SpriteEditorMenu.s_Setting.gridSpritePadding.x);
        Analytics.Event("Sprite Editor", "Slice", "Grid Slicing Padding Y", (int) SpriteEditorMenu.s_Setting.gridSpritePadding.y);
      }
    }

    private void TwoIntFields(string label, string labelX, string labelY, ref int x, ref int y)
    {
      float num = 16f;
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, num, num, EditorStyles.numberField);
      Rect position1 = rect;
      position1.width = EditorGUIUtility.labelWidth;
      position1.height = 16f;
      GUI.Label(position1, label);
      Rect position2 = rect;
      position2.width -= EditorGUIUtility.labelWidth;
      position2.height = 16f;
      position2.x += EditorGUIUtility.labelWidth;
      position2.width /= 2f;
      position2.width -= 2f;
      EditorGUIUtility.labelWidth = 12f;
      x = EditorGUI.IntField(position2, labelX, x);
      position2.x += position2.width + 3f;
      y = EditorGUI.IntField(position2, labelY, y);
      EditorGUIUtility.labelWidth = position1.width;
    }

    private void OnGridGUI()
    {
      int max1 = !((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.previewTexture != (UnityEngine.Object) null) ? 4096 : SpriteEditorMenu.s_SpriteEditor.previewTexture.width;
      int max2 = !((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.previewTexture != (UnityEngine.Object) null) ? 4096 : SpriteEditorMenu.s_SpriteEditor.previewTexture.height;
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.GridByCellCount)
      {
        int x = (int) SpriteEditorMenu.s_Setting.gridCellCount.x;
        int y = (int) SpriteEditorMenu.s_Setting.gridCellCount.y;
        EditorGUI.BeginChangeCheck();
        this.TwoIntFields("Column & Row", "C", "R", ref x, ref y);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change column & row");
          SpriteEditorMenu.s_Setting.gridCellCount.x = (float) Mathf.Clamp(x, 1, max1);
          SpriteEditorMenu.s_Setting.gridCellCount.y = (float) Mathf.Clamp(y, 1, max2);
        }
      }
      else
      {
        int x = (int) SpriteEditorMenu.s_Setting.gridSpriteSize.x;
        int y = (int) SpriteEditorMenu.s_Setting.gridSpriteSize.y;
        EditorGUI.BeginChangeCheck();
        this.TwoIntFields("Pixel Size", "X", "Y", ref x, ref y);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid size");
          SpriteEditorMenu.s_Setting.gridSpriteSize.x = (float) Mathf.Clamp(x, 1, max1);
          SpriteEditorMenu.s_Setting.gridSpriteSize.y = (float) Mathf.Clamp(y, 1, max2);
        }
      }
      int x1 = (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.x;
      int y1 = (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.y;
      EditorGUI.BeginChangeCheck();
      this.TwoIntFields("Offset", "X", "Y", ref x1, ref y1);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid offset");
        SpriteEditorMenu.s_Setting.gridSpriteOffset.x = Mathf.Clamp((float) x1, 0.0f, (float) max1 - SpriteEditorMenu.s_Setting.gridSpriteSize.x);
        SpriteEditorMenu.s_Setting.gridSpriteOffset.y = Mathf.Clamp((float) y1, 0.0f, (float) max2 - SpriteEditorMenu.s_Setting.gridSpriteSize.y);
      }
      int x2 = (int) SpriteEditorMenu.s_Setting.gridSpritePadding.x;
      int y2 = (int) SpriteEditorMenu.s_Setting.gridSpritePadding.y;
      EditorGUI.BeginChangeCheck();
      this.TwoIntFields("Padding", "X", "Y", ref x2, ref y2);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid padding");
        SpriteEditorMenu.s_Setting.gridSpritePadding.x = (float) Mathf.Clamp(x2, 0, max1);
        SpriteEditorMenu.s_Setting.gridSpritePadding.y = (float) Mathf.Clamp(y2, 0, max2);
      }
      this.DoPivotGUI();
      GUILayout.Space(2f);
    }

    private void OnAutomaticGUI()
    {
      float pixels = 38f;
      if ((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.originalTexture != (UnityEngine.Object) null && TextureUtil.IsCompressedTextureFormat(SpriteEditorMenu.s_SpriteEditor.originalTexture.format))
      {
        EditorGUILayout.LabelField("To obtain more accurate slicing results, manual slicing is recommended!", SpriteEditorMenu.s_Styles.notice, new GUILayoutOption[0]);
        pixels -= 31f;
      }
      this.DoPivotGUI();
      EditorGUI.BeginChangeCheck();
      int num = EditorGUILayout.Popup("Method", SpriteEditorMenu.s_Setting.autoSlicingMethod, this.slicingMethodOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change Slicing Method");
        SpriteEditorMenu.s_Setting.autoSlicingMethod = num;
      }
      GUILayout.Space(pixels);
    }

    private void DoPivotGUI()
    {
      EditorGUI.BeginChangeCheck();
      int num = EditorGUILayout.Popup("Pivot", SpriteEditorMenu.s_Setting.spriteAlignment, this.spriteAlignmentOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change Alignment");
        SpriteEditorMenu.s_Setting.spriteAlignment = num;
        SpriteEditorMenu.s_Setting.pivot = SpriteEditorUtility.GetPivotValue((SpriteAlignment) num, SpriteEditorMenu.s_Setting.pivot);
      }
      Vector2 pivot = SpriteEditorMenu.s_Setting.pivot;
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginDisabledGroup(num != 9);
      Vector2 vector2 = EditorGUILayout.Vector2Field("Custom Pivot", pivot);
      EditorGUI.EndDisabledGroup();
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change custom pivot");
      SpriteEditorMenu.s_Setting.pivot = vector2;
    }

    private void DoAutomaticSlicing()
    {
      SpriteEditorMenu.s_SpriteEditor.DoAutomaticSlicing(4, SpriteEditorMenu.s_Setting.spriteAlignment, SpriteEditorMenu.s_Setting.pivot, (SpriteEditorWindow.AutoSlicingMethod) SpriteEditorMenu.s_Setting.autoSlicingMethod);
    }

    private void DoGridSlicing()
    {
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.GridByCellCount)
        this.DetemineGridCellSizeWithCellCount();
      SpriteEditorMenu.s_SpriteEditor.DoGridSlicing(SpriteEditorMenu.s_Setting.gridSpriteSize, SpriteEditorMenu.s_Setting.gridSpriteOffset, SpriteEditorMenu.s_Setting.gridSpritePadding, SpriteEditorMenu.s_Setting.spriteAlignment, SpriteEditorMenu.s_Setting.pivot);
    }

    private void DetemineGridCellSizeWithCellCount()
    {
      int num1 = !((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.previewTexture != (UnityEngine.Object) null) ? 4096 : SpriteEditorMenu.s_SpriteEditor.previewTexture.width;
      int num2 = !((UnityEngine.Object) SpriteEditorMenu.s_SpriteEditor.previewTexture != (UnityEngine.Object) null) ? 4096 : SpriteEditorMenu.s_SpriteEditor.previewTexture.height;
      SpriteEditorMenu.s_Setting.gridSpriteSize.x = (float) ((double) num1 - (double) SpriteEditorMenu.s_Setting.gridSpriteOffset.x - (double) SpriteEditorMenu.s_Setting.gridSpritePadding.x * (double) SpriteEditorMenu.s_Setting.gridCellCount.x) / SpriteEditorMenu.s_Setting.gridCellCount.x;
      SpriteEditorMenu.s_Setting.gridSpriteSize.y = (float) ((double) num2 - (double) SpriteEditorMenu.s_Setting.gridSpriteOffset.y - (double) SpriteEditorMenu.s_Setting.gridSpritePadding.y * (double) SpriteEditorMenu.s_Setting.gridCellCount.y) / SpriteEditorMenu.s_Setting.gridCellCount.y;
      SpriteEditorMenu.s_Setting.gridSpriteSize.x = Mathf.Clamp(SpriteEditorMenu.s_Setting.gridSpriteSize.x, 1f, (float) num1);
      SpriteEditorMenu.s_Setting.gridSpriteSize.y = Mathf.Clamp(SpriteEditorMenu.s_Setting.gridSpriteSize.y, 1f, (float) num2);
    }

    private class Styles
    {
      public GUIStyle background = (GUIStyle) "grey_border";
      public GUIStyle notice;

      public Styles()
      {
        this.notice = new GUIStyle(GUI.skin.label);
        this.notice.alignment = TextAnchor.MiddleCenter;
        this.notice.wordWrap = true;
      }
    }
  }
}
