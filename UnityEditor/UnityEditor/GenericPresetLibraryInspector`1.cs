// Decompiled with JetBrains decompiler
// Type: UnityEditor.GenericPresetLibraryInspector`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class GenericPresetLibraryInspector<T> where T : ScriptableObject
  {
    private float m_LastRepaintedWidth = -1f;
    private readonly ScriptableObjectSaveLoadHelper<T> m_SaveLoadHelper;
    private readonly UnityEngine.Object m_Target;
    private readonly string m_Header;
    private readonly VerticalGrid m_Grid;
    private readonly System.Action<string> m_EditButtonClickedCallback;
    private static GUIStyle s_EditButtonStyle;

    public int maxShowNumPresets { get; set; }

    public Vector2 presetSize { get; set; }

    public float lineSpacing { get; set; }

    public string extension
    {
      get
      {
        return this.m_SaveLoadHelper.fileExtensionWithoutDot;
      }
    }

    public bool useOnePixelOverlappedGrid { get; set; }

    public RectOffset marginsForList { get; set; }

    public RectOffset marginsForGrid { get; set; }

    public PresetLibraryEditorState.ItemViewMode itemViewMode { get; set; }

    public GenericPresetLibraryInspector(UnityEngine.Object target, string header, System.Action<string> editButtonClicked)
    {
      this.m_Target = target;
      this.m_Header = header;
      this.m_EditButtonClickedCallback = editButtonClicked;
      string fileExtensionWithoutDot = Path.GetExtension(AssetDatabase.GetAssetPath(this.m_Target.GetInstanceID()));
      if (!string.IsNullOrEmpty(fileExtensionWithoutDot))
        fileExtensionWithoutDot = fileExtensionWithoutDot.TrimStart('.');
      this.m_SaveLoadHelper = new ScriptableObjectSaveLoadHelper<T>(fileExtensionWithoutDot, SaveType.Text);
      this.m_Grid = new VerticalGrid();
      this.maxShowNumPresets = 49;
      this.presetSize = new Vector2(14f, 14f);
      this.lineSpacing = 1f;
      this.useOnePixelOverlappedGrid = false;
      this.marginsForList = new RectOffset(10, 10, 5, 5);
      this.marginsForGrid = new RectOffset(10, 10, 5, 5);
      this.itemViewMode = PresetLibraryEditorState.ItemViewMode.List;
    }

    public void OnDestroy()
    {
      ScriptableSingleton<PresetLibraryManager>.instance.UnloadAllLibrariesFor<T>(this.m_SaveLoadHelper);
    }

    public void OnInspectorGUI()
    {
      if (GenericPresetLibraryInspector<T>.s_EditButtonStyle == null)
      {
        GenericPresetLibraryInspector<T>.s_EditButtonStyle = new GUIStyle(EditorStyles.miniButton);
        GenericPresetLibraryInspector<T>.s_EditButtonStyle.margin.top = 7;
      }
      string libraryPath = Path.ChangeExtension(AssetDatabase.GetAssetPath(this.m_Target.GetInstanceID()), (string) null);
      bool flag = libraryPath.Contains("/Editor/");
      GUILayout.BeginHorizontal();
      GUILayout.Label(this.m_Header, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      if (flag && this.m_EditButtonClickedCallback != null && (GUILayout.Button("Edit...", GenericPresetLibraryInspector<T>.s_EditButtonStyle, new GUILayoutOption[0]) && this.m_EditButtonClickedCallback != null))
        this.m_EditButtonClickedCallback(libraryPath);
      GUILayout.EndHorizontal();
      GUILayout.Space(6f);
      if (!flag)
        GUILayout.Label(new GUIContent("Preset libraries should be placed in an 'Editor' folder.", (Texture) EditorGUIUtility.warningIcon), EditorStyles.helpBox, new GUILayoutOption[0]);
      this.DrawPresets(libraryPath);
    }

    private void DrawPresets(string libraryPath)
    {
      if ((double) GUIClip.visibleRect.width > 0.0)
        this.m_LastRepaintedWidth = GUIClip.visibleRect.width;
      if ((double) this.m_LastRepaintedWidth < 0.0)
      {
        GUILayoutUtility.GetRect(1f, 1f);
        HandleUtility.Repaint();
      }
      else
      {
        PresetLibrary library = (object) ScriptableSingleton<PresetLibraryManager>.instance.GetLibrary<T>(this.m_SaveLoadHelper, libraryPath) as PresetLibrary;
        if ((UnityEngine.Object) library == (UnityEngine.Object) null)
        {
          Debug.Log((object) ("Could not load preset library '" + libraryPath + "'"));
        }
        else
        {
          this.SetupGrid(this.m_LastRepaintedWidth, library.Count(), this.itemViewMode);
          int num1 = Mathf.Min(library.Count(), this.maxShowNumPresets);
          int num2 = library.Count() - num1;
          float height = this.m_Grid.CalcRect(num1 - 1, 0.0f).yMax + (num2 <= 0 ? 0.0f : 20f);
          Rect rect1 = GUILayoutUtility.GetRect(1f, height);
          float num3 = this.presetSize.x + 6f;
          for (int index = 0; index < num1; ++index)
          {
            Rect rect2 = this.m_Grid.CalcRect(index, rect1.y);
            Rect rect3 = new Rect(rect2.x, rect2.y, this.presetSize.x, this.presetSize.y);
            library.Draw(rect3, index);
            if (this.itemViewMode == PresetLibraryEditorState.ItemViewMode.List)
              GUI.Label(new Rect(rect2.x + num3, rect2.y, rect2.width - num3, rect2.height), library.GetName(index));
          }
          if (num2 <= 0)
            return;
          GUI.Label(new Rect(this.m_Grid.CalcRect(0, 0.0f).x, (float) ((double) rect1.y + (double) height - 20.0), rect1.width, 20f), string.Format("+ {0} more...", (object) num2));
        }
      }
    }

    private void SetupGrid(float availableWidth, int itemCount, PresetLibraryEditorState.ItemViewMode presetsViewMode)
    {
      this.m_Grid.useFixedHorizontalSpacing = this.useOnePixelOverlappedGrid;
      this.m_Grid.fixedHorizontalSpacing = !this.useOnePixelOverlappedGrid ? 0.0f : -1f;
      switch (presetsViewMode)
      {
        case PresetLibraryEditorState.ItemViewMode.Grid:
          this.m_Grid.fixedWidth = availableWidth;
          this.m_Grid.topMargin = (float) this.marginsForGrid.top;
          this.m_Grid.bottomMargin = (float) this.marginsForGrid.bottom;
          this.m_Grid.leftMargin = (float) this.marginsForGrid.left;
          this.m_Grid.rightMargin = (float) this.marginsForGrid.right;
          this.m_Grid.verticalSpacing = !this.useOnePixelOverlappedGrid ? this.lineSpacing : -1f;
          this.m_Grid.minHorizontalSpacing = 8f;
          this.m_Grid.itemSize = this.presetSize;
          this.m_Grid.InitNumRowsAndColumns(itemCount, int.MaxValue);
          break;
        case PresetLibraryEditorState.ItemViewMode.List:
          this.m_Grid.fixedWidth = availableWidth;
          this.m_Grid.topMargin = (float) this.marginsForList.top;
          this.m_Grid.bottomMargin = (float) this.marginsForList.bottom;
          this.m_Grid.leftMargin = (float) this.marginsForList.left;
          this.m_Grid.rightMargin = (float) this.marginsForList.right;
          this.m_Grid.verticalSpacing = this.lineSpacing;
          this.m_Grid.minHorizontalSpacing = 0.0f;
          this.m_Grid.itemSize = new Vector2(availableWidth - this.m_Grid.leftMargin, this.presetSize.y);
          this.m_Grid.InitNumRowsAndColumns(itemCount, int.MaxValue);
          break;
      }
    }
  }
}
