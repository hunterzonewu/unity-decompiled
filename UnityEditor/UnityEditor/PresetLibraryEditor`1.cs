// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryEditor`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PresetLibraryEditor<T> where T : PresetLibrary
  {
    private PresetLibraryEditor<T>.DragState m_DragState = new PresetLibraryEditor<T>.DragState();
    private readonly VerticalGrid m_Grid = new VerticalGrid();
    private Vector2 m_MinMaxPreviewHeight = new Vector2(14f, 64f);
    private float m_PreviewAspect = 8f;
    private bool m_ShowAddNewPresetItem = true;
    private bool m_IsOpenForEdit = true;
    private const float kGridLabelHeight = 16f;
    private const float kCheckoutButtonMaxWidth = 100f;
    private const float kCheckoutButtonMargin = 2f;
    private static PresetLibraryEditor<T>.Styles s_Styles;
    private readonly PresetLibraryEditorState m_State;
    private readonly ScriptableObjectSaveLoadHelper<T> m_SaveLoadHelper;
    private readonly System.Action<int, object> m_ItemClickedCallback;
    public System.Action<PresetLibrary> addDefaultPresets;
    public System.Action presetsWasReordered;
    private bool m_ShowedScrollBarLastFrame;
    private PresetFileLocation m_PresetLibraryFileLocation;

    public float contentHeight { get; private set; }

    private float topAreaHeight
    {
      get
      {
        return 20f;
      }
    }

    private float versionControlAreaHeight
    {
      get
      {
        return 20f;
      }
    }

    private float gridWidth { get; set; }

    public bool wantsToCreateLibrary { get; set; }

    public bool showHeader { get; set; }

    public float settingsMenuRightMargin { get; set; }

    public bool alwaysShowScrollAreaHorizontalLines { get; set; }

    public bool useOnePixelOverlappedGrid { get; set; }

    public RectOffset marginsForList { get; set; }

    public RectOffset marginsForGrid { get; set; }

    public string currentLibraryWithoutExtension
    {
      get
      {
        return this.m_State.m_CurrrentLibrary;
      }
      set
      {
        this.m_State.m_CurrrentLibrary = Path.ChangeExtension(value, (string) null);
        this.m_PresetLibraryFileLocation = PresetLibraryLocations.GetFileLocationFromPath(this.m_State.m_CurrrentLibrary);
        this.OnLayoutChanged();
        this.Repaint();
      }
    }

    public float previewAspect
    {
      get
      {
        return this.m_PreviewAspect;
      }
      set
      {
        this.m_PreviewAspect = value;
      }
    }

    public Vector2 minMaxPreviewHeight
    {
      get
      {
        return this.m_MinMaxPreviewHeight;
      }
      set
      {
        this.m_MinMaxPreviewHeight = value;
        this.previewHeight = this.previewHeight;
      }
    }

    public float previewHeight
    {
      get
      {
        return this.m_State.m_PreviewHeight;
      }
      set
      {
        this.m_State.m_PreviewHeight = Mathf.Clamp(value, this.minMaxPreviewHeight.x, this.minMaxPreviewHeight.y);
        this.Repaint();
      }
    }

    public PresetLibraryEditorState.ItemViewMode itemViewMode
    {
      get
      {
        return this.m_State.itemViewMode;
      }
      set
      {
        this.m_State.itemViewMode = value;
        this.OnLayoutChanged();
        this.Repaint();
      }
    }

    private bool drawLabels
    {
      get
      {
        return this.m_State.itemViewMode == PresetLibraryEditorState.ItemViewMode.List;
      }
    }

    private string pathWithExtension
    {
      get
      {
        return this.currentLibraryWithoutExtension + "." + this.m_SaveLoadHelper.fileExtensionWithoutDot;
      }
    }

    public PresetLibraryEditor(ScriptableObjectSaveLoadHelper<T> helper, PresetLibraryEditorState state, System.Action<int, object> itemClickedCallback)
    {
      this.m_SaveLoadHelper = helper;
      this.m_State = state;
      this.m_ItemClickedCallback = itemClickedCallback;
      this.settingsMenuRightMargin = 10f;
      this.useOnePixelOverlappedGrid = false;
      this.alwaysShowScrollAreaHorizontalLines = true;
      this.marginsForList = new RectOffset(10, 10, 5, 5);
      this.marginsForGrid = new RectOffset(5, 5, 5, 5);
      this.m_PresetLibraryFileLocation = PresetLibraryLocations.GetFileLocationFromPath(this.currentLibraryWithoutExtension);
    }

    public void InitializeGrid(float availableWidth)
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib != (UnityEngine.Object) null)
      {
        if ((double) availableWidth <= 0.0)
          return;
        this.SetupGrid(availableWidth, currentLib.Count());
      }
      else
        Debug.LogError((object) ("Could not load preset library " + this.currentLibraryWithoutExtension));
    }

    private void Repaint()
    {
      HandleUtility.Repaint();
    }

    private void ValidateNoExtension(string value)
    {
      if (!Path.HasExtension(value))
        return;
      Debug.LogError((object) ("currentLibraryWithoutExtension should not have an extension: " + value));
    }

    private string CreateNewLibraryCallback(string libraryName, PresetFileLocation fileLocation)
    {
      string presetLibraryPathWithoutExtension = Path.Combine(PresetLibraryLocations.GetDefaultFilePathForFileLocation(fileLocation), libraryName);
      if ((UnityEngine.Object) this.CreateNewLibrary(presetLibraryPathWithoutExtension) != (UnityEngine.Object) null)
        this.currentLibraryWithoutExtension = presetLibraryPathWithoutExtension;
      return ScriptableSingleton<PresetLibraryManager>.instance.GetLastError();
    }

    private static bool IsItemVisible(float scrollHeight, float itemYMin, float itemYMax, float scrollPos)
    {
      float num = itemYMin - scrollPos;
      return (double) (itemYMax - scrollPos) >= 0.0 && (double) num <= (double) scrollHeight;
    }

    private void OnLayoutChanged()
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null || (double) this.gridWidth <= 0.0)
        return;
      this.SetupGrid(this.gridWidth, currentLib.Count());
    }

    private void SetupGrid(float width, int itemCount)
    {
      if ((double) width < 1.0)
      {
        Debug.LogError((object) ("Invalid width " + (object) width + ", " + (object) Event.current.type));
      }
      else
      {
        if (this.m_ShowAddNewPresetItem)
          ++itemCount;
        this.m_Grid.useFixedHorizontalSpacing = this.useOnePixelOverlappedGrid;
        this.m_Grid.fixedHorizontalSpacing = !this.useOnePixelOverlappedGrid ? 0.0f : -1f;
        switch (this.m_State.itemViewMode)
        {
          case PresetLibraryEditorState.ItemViewMode.Grid:
            this.m_Grid.fixedWidth = width;
            this.m_Grid.topMargin = (float) this.marginsForGrid.top;
            this.m_Grid.bottomMargin = (float) this.marginsForGrid.bottom;
            this.m_Grid.leftMargin = (float) this.marginsForGrid.left;
            this.m_Grid.rightMargin = (float) this.marginsForGrid.right;
            this.m_Grid.verticalSpacing = !this.useOnePixelOverlappedGrid ? 2f : -1f;
            this.m_Grid.minHorizontalSpacing = 1f;
            this.m_Grid.itemSize = new Vector2(this.m_State.m_PreviewHeight * this.m_PreviewAspect, this.m_State.m_PreviewHeight);
            this.m_Grid.InitNumRowsAndColumns(itemCount, int.MaxValue);
            break;
          case PresetLibraryEditorState.ItemViewMode.List:
            this.m_Grid.fixedWidth = width;
            this.m_Grid.topMargin = (float) this.marginsForList.top;
            this.m_Grid.bottomMargin = (float) this.marginsForList.bottom;
            this.m_Grid.leftMargin = (float) this.marginsForList.left;
            this.m_Grid.rightMargin = (float) this.marginsForList.right;
            this.m_Grid.verticalSpacing = 2f;
            this.m_Grid.minHorizontalSpacing = 0.0f;
            this.m_Grid.itemSize = new Vector2(width - this.m_Grid.leftMargin, this.m_State.m_PreviewHeight);
            this.m_Grid.InitNumRowsAndColumns(itemCount, int.MaxValue);
            break;
        }
        this.contentHeight = (float) ((double) this.topAreaHeight + (double) (this.m_Grid.CalcRect(itemCount - 1, 0.0f).yMax + this.m_Grid.bottomMargin) + (!this.m_IsOpenForEdit ? (double) this.versionControlAreaHeight : 0.0));
      }
    }

    public void OnGUI(Rect rect, object presetObject)
    {
      if ((double) rect.width < 2.0)
        return;
      this.m_State.m_RenameOverlay.OnEvent();
      T currentLib = this.GetCurrentLib();
      if (PresetLibraryEditor<T>.s_Styles == null)
        PresetLibraryEditor<T>.s_Styles = new PresetLibraryEditor<T>.Styles();
      Rect rect1 = new Rect(rect.x, rect.y, rect.width, this.topAreaHeight);
      Rect rect2 = new Rect(rect.x, rect1.yMax, rect.width, rect.height - this.topAreaHeight);
      this.TopArea(rect1);
      this.ListArea(rect2, (PresetLibrary) currentLib, presetObject);
    }

    private void TopArea(Rect rect)
    {
      GUI.BeginGroup(rect);
      if (this.showHeader)
        GUI.Label(new Rect(10f, 2f, rect.width - 20f, rect.height), PresetLibraryEditor<T>.s_Styles.header);
      Rect rect1 = new Rect(rect.width - 16f - this.settingsMenuRightMargin, (float) (((double) rect.height - 6.0) * 0.5), 16f, rect.height);
      if (Event.current.type == EventType.Repaint)
        PresetLibraryEditor<T>.s_Styles.optionsButton.Draw(rect1, false, false, false, false);
      rect1.y = 0.0f;
      rect1.height = rect.height;
      rect1.width = 24f;
      if (GUI.Button(rect1, GUIContent.none, GUIStyle.none))
        PresetLibraryEditor<T>.SettingsMenu.Show(rect1, this);
      if (this.wantsToCreateLibrary)
      {
        this.wantsToCreateLibrary = false;
        PopupWindow.Show(rect1, (PopupWindowContent) new PopupWindowContentForNewLibrary(new Func<string, PresetFileLocation, string>(this.CreateNewLibraryCallback)));
        GUIUtility.ExitGUI();
      }
      GUI.EndGroup();
    }

    private Rect GetDragRect(Rect itemRect)
    {
      int num1 = Mathf.FloorToInt((float) ((double) this.m_Grid.horizontalSpacing * 0.5 + 0.5));
      int num2 = Mathf.FloorToInt((float) ((double) this.m_Grid.verticalSpacing * 0.5 + 0.5));
      return new RectOffset(num1, num1, num2, num2).Add(itemRect);
    }

    private void ClearDragState()
    {
      this.m_DragState.dragUponIndex = -1;
      this.m_DragState.draggingIndex = -1;
    }

    private void DrawHoverEffect(Rect itemRect, bool drawAsSelection)
    {
      Color color = GUI.color;
      GUI.color = new Color(0.0f, 0.0f, 0.4f, !drawAsSelection ? 0.3f : 0.8f);
      GUI.Label(new RectOffset(3, 3, 3, 3).Add(itemRect), GUIContent.none, EditorStyles.helpBox);
      GUI.color = color;
    }

    private void VersionControlArea(Rect rect)
    {
      if ((double) rect.width > 100.0)
        rect = new Rect((float) ((double) rect.xMax - 100.0 - 2.0), rect.y + 2f, 100f, rect.height - 4f);
      if (!GUI.Button(rect, "Check out", EditorStyles.miniButton))
        return;
      Provider.Checkout(new string[1]
      {
        this.pathWithExtension
      }, CheckoutMode.Asset);
    }

    private void ListArea(Rect rect, PresetLibrary lib, object newPresetObject)
    {
      if ((UnityEngine.Object) lib == (UnityEngine.Object) null)
        return;
      Event current = Event.current;
      if (this.m_PresetLibraryFileLocation == PresetFileLocation.ProjectFolder && current.type == EventType.Repaint)
        this.m_IsOpenForEdit = AssetDatabase.IsOpenForEdit(this.pathWithExtension);
      else if (this.m_PresetLibraryFileLocation == PresetFileLocation.PreferencesFolder)
        this.m_IsOpenForEdit = true;
      if (!this.m_IsOpenForEdit)
      {
        this.VersionControlArea(new Rect(rect.x, rect.yMax - this.versionControlAreaHeight, rect.width, this.versionControlAreaHeight));
        rect.height -= this.versionControlAreaHeight;
      }
      for (int index = 0; index < 2; ++index)
      {
        this.gridWidth = !this.m_ShowedScrollBarLastFrame ? rect.width : rect.width - 17f;
        this.SetupGrid(this.gridWidth, lib.Count());
        bool flag = (double) this.m_Grid.height > (double) rect.height;
        if (flag != this.m_ShowedScrollBarLastFrame)
          this.m_ShowedScrollBarLastFrame = flag;
        else
          break;
      }
      if ((this.m_ShowedScrollBarLastFrame || this.alwaysShowScrollAreaHorizontalLines) && Event.current.type == EventType.Repaint)
      {
        Rect rect1 = new RectOffset(1, 1, 1, 1).Add(rect);
        rect1.height = 1f;
        EditorGUI.DrawRect(rect1, new Color(0.0f, 0.0f, 0.0f, 0.3f));
        rect1.y += rect.height + 1f;
        EditorGUI.DrawRect(rect1, new Color(0.0f, 0.0f, 0.0f, 0.3f));
      }
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, this.m_Grid.height);
      this.m_State.m_ScrollPosition = GUI.BeginScrollView(rect, this.m_State.m_ScrollPosition, viewRect);
      float num = 0.0f;
      int maxIndex = !this.m_ShowAddNewPresetItem ? lib.Count() - 1 : lib.Count();
      int startIndex;
      int endIndex;
      bool flag1 = this.m_Grid.IsVisibleInScrollView(rect.height, this.m_State.m_ScrollPosition.y, num, maxIndex, out startIndex, out endIndex);
      bool flag2 = false;
      if (flag1)
      {
        if (this.GetRenameOverlay().IsRenaming() && !this.GetRenameOverlay().isWaitingForDelay)
        {
          if (!this.m_State.m_RenameOverlay.OnGUI())
          {
            this.EndRename();
            current.Use();
          }
          this.Repaint();
        }
        for (int index = startIndex; index <= endIndex; ++index)
        {
          int controlID = index + 1000000;
          Rect rect1 = this.m_Grid.CalcRect(index, num);
          Rect rect2 = rect1;
          Rect position = rect1;
          switch (this.m_State.itemViewMode)
          {
            case PresetLibraryEditorState.ItemViewMode.List:
              rect2.width = this.m_State.m_PreviewHeight * this.m_PreviewAspect;
              position.x += rect2.width + 8f;
              position.width -= rect2.width + 10f;
              position.height = 16f;
              position.y = rect1.yMin + (float) (((double) rect1.height - 16.0) * 0.5);
              break;
          }
          if (this.m_ShowAddNewPresetItem && index == lib.Count())
          {
            this.CreateNewPresetButton(rect2, newPresetObject, lib, this.m_IsOpenForEdit);
          }
          else
          {
            bool flag3 = this.IsRenaming(index);
            if (flag3)
            {
              Rect rect3 = position;
              --rect3.y;
              --rect3.x;
              this.m_State.m_RenameOverlay.editFieldRect = rect3;
            }
            switch (current.type)
            {
              case EventType.MouseDown:
                if (current.button == 0 && rect1.Contains(current.mousePosition))
                {
                  GUIUtility.hotControl = controlID;
                  if (current.clickCount == 1)
                  {
                    this.m_ItemClickedCallback(current.clickCount, lib.GetPreset(index));
                    current.Use();
                    continue;
                  }
                  continue;
                }
                continue;
              case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                  GUIUtility.hotControl = 0;
                  if (current.button == 0 && rect1.Contains(current.mousePosition) && (Event.current.alt && this.m_IsOpenForEdit))
                  {
                    this.DeletePreset(index);
                    current.Use();
                    continue;
                  }
                  continue;
                }
                continue;
              case EventType.MouseMove:
                if (rect1.Contains(current.mousePosition))
                {
                  if (this.m_State.m_HoverIndex != index)
                  {
                    this.m_State.m_HoverIndex = index;
                    this.Repaint();
                    continue;
                  }
                  continue;
                }
                if (this.m_State.m_HoverIndex == index)
                {
                  this.m_State.m_HoverIndex = -1;
                  this.Repaint();
                  continue;
                }
                continue;
              case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID && this.m_IsOpenForEdit)
                {
                  if (((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), controlID)).CanStartDrag())
                  {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.SetGenericData("DraggingPreset", (object) index);
                    DragAndDrop.objectReferences = new UnityEngine.Object[0];
                    DragAndDrop.StartDrag(string.Empty);
                    this.m_DragState.draggingIndex = index;
                    this.m_DragState.dragUponIndex = index;
                    GUIUtility.hotControl = 0;
                  }
                  current.Use();
                  continue;
                }
                continue;
              case EventType.Repaint:
                if (this.m_State.m_HoverIndex == index && !rect1.Contains(current.mousePosition))
                  this.m_State.m_HoverIndex = -1;
                if (this.m_DragState.draggingIndex == index || GUIUtility.hotControl == controlID)
                  this.DrawHoverEffect(rect1, false);
                lib.Draw(rect2, index);
                if (!flag3 && this.drawLabels)
                  GUI.Label(position, GUIContent.Temp(lib.GetName(index)));
                if (this.m_DragState.dragUponIndex == index && this.m_DragState.draggingIndex != this.m_DragState.dragUponIndex)
                  flag2 = true;
                if (GUIUtility.hotControl == 0 && Event.current.alt && this.m_IsOpenForEdit)
                {
                  EditorGUIUtility.AddCursorRect(rect1, MouseCursor.ArrowMinus);
                  continue;
                }
                continue;
              case EventType.DragUpdated:
              case EventType.DragPerform:
                Rect dragRect = this.GetDragRect(rect1);
                if (dragRect.Contains(current.mousePosition))
                {
                  this.m_DragState.dragUponIndex = index;
                  this.m_DragState.dragUponRect = rect1;
                  this.m_DragState.insertAfterIndex = this.m_State.itemViewMode != PresetLibraryEditorState.ItemViewMode.List ? ((double) current.mousePosition.x - (double) dragRect.x) / (double) dragRect.width > 0.5 : ((double) current.mousePosition.y - (double) dragRect.y) / (double) dragRect.height > 0.5;
                  if (current.type == EventType.DragPerform)
                  {
                    if (this.m_DragState.draggingIndex >= 0)
                    {
                      this.MovePreset(this.m_DragState.draggingIndex, this.m_DragState.dragUponIndex, this.m_DragState.insertAfterIndex);
                      DragAndDrop.AcceptDrag();
                    }
                    this.ClearDragState();
                  }
                  DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                  current.Use();
                  continue;
                }
                continue;
              case EventType.DragExited:
                if (this.m_DragState.IsDragging())
                {
                  this.ClearDragState();
                  current.Use();
                  continue;
                }
                continue;
              case EventType.ContextClick:
                if (rect1.Contains(current.mousePosition))
                {
                  PresetLibraryEditor<T>.PresetContextMenu.Show(this.m_IsOpenForEdit, index, newPresetObject, this);
                  current.Use();
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
        if (flag2)
          this.DrawDragInsertionMarker();
      }
      GUI.EndScrollView();
    }

    private void DrawDragInsertionMarker()
    {
      if (!this.m_DragState.IsDragging())
        return;
      Rect dragRect = this.GetDragRect(this.m_DragState.dragUponRect);
      EditorGUI.DrawRect(this.m_State.itemViewMode != PresetLibraryEditorState.ItemViewMode.List ? (!this.m_DragState.insertAfterIndex ? new Rect(dragRect.xMin - 1f, dragRect.yMin, 2f, dragRect.height) : new Rect(dragRect.xMax - 1f, dragRect.yMin, 2f, dragRect.height)) : (!this.m_DragState.insertAfterIndex ? new Rect(dragRect.xMin, dragRect.yMin - 1f, dragRect.width, 2f) : new Rect(dragRect.xMin, dragRect.yMax - 1f, dragRect.width, 2f)), new Color(0.3f, 0.3f, 1f));
    }

    private void CreateNewPresetButton(Rect buttonRect, object newPresetObject, PresetLibrary lib, bool isOpenForEdit)
    {
      EditorGUI.BeginDisabledGroup(!isOpenForEdit);
      if (GUI.Button(buttonRect, !isOpenForEdit ? PresetLibraryEditor<T>.s_Styles.plusButtonTextNotCheckedOut : PresetLibraryEditor<T>.s_Styles.plusButtonText))
      {
        int newPreset = this.CreateNewPreset(newPresetObject, string.Empty);
        if (this.drawLabels)
          this.BeginRenaming(string.Empty, newPreset, 0.0f);
        InspectorWindow.RepaintAllInspectors();
      }
      if (Event.current.type == EventType.Repaint)
      {
        Rect rect = new RectOffset(-3, -3, -3, -3).Add(buttonRect);
        lib.Draw(rect, newPresetObject);
        if ((double) buttonRect.width > 30.0)
          PresetLibraryEditor<T>.LabelWithOutline(buttonRect, PresetLibraryEditor<T>.s_Styles.newPreset, new Color(0.1f, 0.1f, 0.1f), PresetLibraryEditor<T>.s_Styles.newPresetStyle);
        else if (lib.Count() == 0 && isOpenForEdit)
        {
          buttonRect.x = buttonRect.xMax + 5f;
          buttonRect.width = 200f;
          buttonRect.height = 16f;
          EditorGUI.BeginDisabledGroup(true);
          GUI.Label(buttonRect, "Click to add new preset");
          EditorGUI.EndDisabledGroup();
        }
      }
      EditorGUI.EndDisabledGroup();
    }

    private static void LabelWithOutline(Rect rect, GUIContent content, Color outlineColor, GUIStyle style)
    {
      Color color = GUI.color;
      GUI.color = outlineColor;
      for (int index1 = -1; index1 <= 1; ++index1)
      {
        for (int index2 = -1; index2 <= 1; ++index2)
        {
          if (index1 != 0 || index2 != 0)
          {
            Rect position = rect;
            position.x += (float) index2;
            position.y += (float) index1;
            GUI.Label(position, content, style);
          }
        }
      }
      GUI.color = color;
      GUI.Label(rect, content, style);
    }

    private bool IsRenaming(int itemID)
    {
      if (this.GetRenameOverlay().IsRenaming() && this.GetRenameOverlay().userData == itemID)
        return !this.GetRenameOverlay().isWaitingForDelay;
      return false;
    }

    private RenameOverlay GetRenameOverlay()
    {
      return this.m_State.m_RenameOverlay;
    }

    private void BeginRenaming(string name, int itemIndex, float delay)
    {
      this.GetRenameOverlay().BeginRename(name, itemIndex, delay);
    }

    private void EndRename()
    {
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      T currentLib = this.GetCurrentLib();
      if (userData >= 0 && userData < currentLib.Count())
      {
        currentLib.SetName(userData, name);
        this.SaveCurrentLib();
      }
      this.GetRenameOverlay().EndRename(true);
    }

    public T GetCurrentLib()
    {
      T library = ScriptableSingleton<PresetLibraryManager>.instance.GetLibrary<T>(this.m_SaveLoadHelper, this.currentLibraryWithoutExtension);
      if ((UnityEngine.Object) library == (UnityEngine.Object) null)
      {
        library = ScriptableSingleton<PresetLibraryManager>.instance.GetLibrary<T>(this.m_SaveLoadHelper, PresetLibraryLocations.defaultPresetLibraryPath);
        if ((UnityEngine.Object) library == (UnityEngine.Object) null)
        {
          library = this.CreateNewLibrary(PresetLibraryLocations.defaultPresetLibraryPath);
          if ((UnityEngine.Object) library != (UnityEngine.Object) null)
          {
            if (this.addDefaultPresets != null)
            {
              this.addDefaultPresets((PresetLibrary) library);
              ScriptableSingleton<PresetLibraryManager>.instance.SaveLibrary<T>(this.m_SaveLoadHelper, library, PresetLibraryLocations.defaultPresetLibraryPath);
            }
          }
          else
            Debug.LogError((object) ("Could not create Default preset library " + ScriptableSingleton<PresetLibraryManager>.instance.GetLastError()));
        }
        this.currentLibraryWithoutExtension = PresetLibraryLocations.defaultPresetLibraryPath;
      }
      return library;
    }

    public void UnloadUsedLibraries()
    {
      ScriptableSingleton<PresetLibraryManager>.instance.UnloadAllLibrariesFor<T>(this.m_SaveLoadHelper);
    }

    public void DeletePreset(int presetIndex)
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null)
        return;
      if (presetIndex < 0 || presetIndex >= currentLib.Count())
      {
        Debug.LogError((object) "DeletePreset: Invalid index: out of bounds");
      }
      else
      {
        currentLib.Remove(presetIndex);
        this.SaveCurrentLib();
        if (this.presetsWasReordered != null)
          this.presetsWasReordered();
        this.OnLayoutChanged();
      }
    }

    public void ReplacePreset(int presetIndex, object presetObject)
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null)
        return;
      if (presetIndex < 0 || presetIndex >= currentLib.Count())
      {
        Debug.LogError((object) "ReplacePreset: Invalid index: out of bounds");
      }
      else
      {
        currentLib.Replace(presetIndex, presetObject);
        this.SaveCurrentLib();
        if (this.presetsWasReordered == null)
          return;
        this.presetsWasReordered();
      }
    }

    public void MovePreset(int presetIndex, int destPresetIndex, bool insertAfterDestIndex)
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null)
        return;
      if (presetIndex < 0 || presetIndex >= currentLib.Count())
      {
        Debug.LogError((object) "ReplacePreset: Invalid index: out of bounds");
      }
      else
      {
        currentLib.Move(presetIndex, destPresetIndex, insertAfterDestIndex);
        this.SaveCurrentLib();
        if (this.presetsWasReordered == null)
          return;
        this.presetsWasReordered();
      }
    }

    public int CreateNewPreset(object presetObject, string presetName)
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null)
      {
        Debug.Log((object) "No current library selected!");
        return -1;
      }
      currentLib.Add(presetObject, presetName);
      this.SaveCurrentLib();
      if (this.presetsWasReordered != null)
        this.presetsWasReordered();
      this.Repaint();
      this.OnLayoutChanged();
      return currentLib.Count() - 1;
    }

    public void SaveCurrentLib()
    {
      T currentLib = this.GetCurrentLib();
      if ((UnityEngine.Object) currentLib == (UnityEngine.Object) null)
      {
        Debug.Log((object) "No current library selected!");
      }
      else
      {
        ScriptableSingleton<PresetLibraryManager>.instance.SaveLibrary<T>(this.m_SaveLoadHelper, currentLib, this.currentLibraryWithoutExtension);
        InternalEditorUtility.RepaintAllViews();
      }
    }

    public T CreateNewLibrary(string presetLibraryPathWithoutExtension)
    {
      T library = ScriptableSingleton<PresetLibraryManager>.instance.CreateLibrary<T>(this.m_SaveLoadHelper, presetLibraryPathWithoutExtension);
      if ((UnityEngine.Object) library != (UnityEngine.Object) null)
      {
        ScriptableSingleton<PresetLibraryManager>.instance.SaveLibrary<T>(this.m_SaveLoadHelper, library, presetLibraryPathWithoutExtension);
        InternalEditorUtility.RepaintAllViews();
      }
      return library;
    }

    public void RevealCurrentLibrary()
    {
      if (this.m_PresetLibraryFileLocation == PresetFileLocation.PreferencesFolder)
        EditorUtility.RevealInFinder(Path.GetFullPath(this.pathWithExtension));
      else
        EditorGUIUtility.PingObject(AssetDatabase.GetMainAssetInstanceID(this.pathWithExtension));
    }

    private class Styles
    {
      public GUIStyle innerShadowBg = PresetLibraryEditor<T>.Styles.GetStyle("InnerShadowBg");
      public GUIStyle optionsButton = PresetLibraryEditor<T>.Styles.GetStyle("PaneOptions");
      public GUIStyle newPresetStyle = new GUIStyle(EditorStyles.boldLabel);
      public GUIContent plusButtonText = new GUIContent(string.Empty, "Add new preset");
      public GUIContent plusButtonTextNotCheckedOut = new GUIContent(string.Empty, "To add presets you need to press the 'Check out' button below");
      public GUIContent header = new GUIContent("Presets");
      public GUIContent newPreset = new GUIContent("New");

      public Styles()
      {
        this.newPresetStyle.alignment = TextAnchor.MiddleCenter;
        this.newPresetStyle.normal.textColor = Color.white;
      }

      private static GUIStyle GetStyle(string styleName)
      {
        return (GUIStyle) styleName;
      }
    }

    private class DragState
    {
      public int dragUponIndex { get; set; }

      public int draggingIndex { get; set; }

      public bool insertAfterIndex { get; set; }

      public Rect dragUponRect { get; set; }

      public DragState()
      {
        this.dragUponIndex = -1;
        this.draggingIndex = -1;
      }

      public bool IsDragging()
      {
        return this.draggingIndex != -1;
      }
    }

    internal class PresetContextMenu
    {
      private static PresetLibraryEditor<T> s_Caller;
      private static int s_PresetIndex;

      internal static void Show(bool isOpenForEdit, int presetIndex, object newPresetObject, PresetLibraryEditor<T> caller)
      {
        PresetLibraryEditor<T>.PresetContextMenu.s_Caller = caller;
        PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex = presetIndex;
        GUIContent content1 = new GUIContent("Replace");
        GUIContent content2 = new GUIContent("Delete");
        GUIContent content3 = new GUIContent("Rename");
        GUIContent content4 = new GUIContent("Move To First");
        GenericMenu genericMenu = new GenericMenu();
        if (isOpenForEdit)
        {
          genericMenu.AddItem(content1, false, new GenericMenu.MenuFunction2(new PresetLibraryEditor<T>.PresetContextMenu().Replace), newPresetObject);
          genericMenu.AddItem(content2, false, new GenericMenu.MenuFunction2(new PresetLibraryEditor<T>.PresetContextMenu().Delete), (object) 0);
          if (caller.drawLabels)
            genericMenu.AddItem(content3, false, new GenericMenu.MenuFunction2(new PresetLibraryEditor<T>.PresetContextMenu().Rename), (object) 0);
          genericMenu.AddItem(content4, false, new GenericMenu.MenuFunction2(new PresetLibraryEditor<T>.PresetContextMenu().MoveToTop), (object) 0);
        }
        else
        {
          genericMenu.AddDisabledItem(content1);
          genericMenu.AddDisabledItem(content2);
          if (caller.drawLabels)
            genericMenu.AddDisabledItem(content3);
          genericMenu.AddDisabledItem(content4);
        }
        genericMenu.ShowAsContext();
      }

      private void Delete(object userData)
      {
        PresetLibraryEditor<T>.PresetContextMenu.s_Caller.DeletePreset(PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex);
      }

      private void Replace(object userData)
      {
        object presetObject = userData;
        PresetLibraryEditor<T>.PresetContextMenu.s_Caller.ReplacePreset(PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex, presetObject);
      }

      private void Rename(object userData)
      {
        string name = PresetLibraryEditor<T>.PresetContextMenu.s_Caller.GetCurrentLib().GetName(PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex);
        PresetLibraryEditor<T>.PresetContextMenu.s_Caller.BeginRenaming(name, PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex, 0.0f);
      }

      private void MoveToTop(object userData)
      {
        PresetLibraryEditor<T>.PresetContextMenu.s_Caller.MovePreset(PresetLibraryEditor<T>.PresetContextMenu.s_PresetIndex, 0, false);
      }
    }

    private class SettingsMenu
    {
      private static PresetLibraryEditor<T> s_Owner;

      public static void Show(Rect activatorRect, PresetLibraryEditor<T> owner)
      {
        PresetLibraryEditor<T>.SettingsMenu.s_Owner = owner;
        GenericMenu genericMenu = new GenericMenu();
        int x = (int) PresetLibraryEditor<T>.SettingsMenu.s_Owner.minMaxPreviewHeight.x;
        int y = (int) PresetLibraryEditor<T>.SettingsMenu.s_Owner.minMaxPreviewHeight.y;
        List<PresetLibraryEditor<T>.SettingsMenu.ViewModeData> viewModeDataList;
        if (x == y)
          viewModeDataList = new List<PresetLibraryEditor<T>.SettingsMenu.ViewModeData>()
          {
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("Grid"),
              itemHeight = x,
              viewmode = PresetLibraryEditorState.ItemViewMode.Grid
            },
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("List"),
              itemHeight = x,
              viewmode = PresetLibraryEditorState.ItemViewMode.List
            }
          };
        else
          viewModeDataList = new List<PresetLibraryEditor<T>.SettingsMenu.ViewModeData>()
          {
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("Small Grid"),
              itemHeight = x,
              viewmode = PresetLibraryEditorState.ItemViewMode.Grid
            },
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("Large Grid"),
              itemHeight = y,
              viewmode = PresetLibraryEditorState.ItemViewMode.Grid
            },
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("Small List"),
              itemHeight = x,
              viewmode = PresetLibraryEditorState.ItemViewMode.List
            },
            new PresetLibraryEditor<T>.SettingsMenu.ViewModeData()
            {
              text = new GUIContent("Large List"),
              itemHeight = y,
              viewmode = PresetLibraryEditorState.ItemViewMode.List
            }
          };
        for (int index = 0; index < viewModeDataList.Count; ++index)
        {
          bool on = PresetLibraryEditor<T>.SettingsMenu.s_Owner.itemViewMode == viewModeDataList[index].viewmode && (int) PresetLibraryEditor<T>.SettingsMenu.s_Owner.previewHeight == viewModeDataList[index].itemHeight;
          genericMenu.AddItem(viewModeDataList[index].text, on, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.ViewModeChange), (object) viewModeDataList[index]);
        }
        genericMenu.AddSeparator(string.Empty);
        List<string> preferencesLibs;
        List<string> projectLibs;
        ScriptableSingleton<PresetLibraryManager>.instance.GetAvailableLibraries<T>(PresetLibraryEditor<T>.SettingsMenu.s_Owner.m_SaveLoadHelper, out preferencesLibs, out projectLibs);
        preferencesLibs.Sort();
        projectLibs.Sort();
        string str1 = PresetLibraryEditor<T>.SettingsMenu.s_Owner.currentLibraryWithoutExtension + "." + PresetLibraryEditor<T>.SettingsMenu.s_Owner.m_SaveLoadHelper.fileExtensionWithoutDot;
        string str2 = " (Project)";
        using (List<string>.Enumerator enumerator = preferencesLibs.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            string withoutExtension = Path.GetFileNameWithoutExtension(current);
            genericMenu.AddItem(new GUIContent(withoutExtension), str1 == current, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.LibraryModeChange), (object) current);
          }
        }
        using (List<string>.Enumerator enumerator = projectLibs.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            string withoutExtension = Path.GetFileNameWithoutExtension(current);
            genericMenu.AddItem(new GUIContent(withoutExtension + str2), str1 == current, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.LibraryModeChange), (object) current);
          }
        }
        genericMenu.AddSeparator(string.Empty);
        genericMenu.AddItem(new GUIContent("Create New Library..."), false, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.CreateLibrary), (object) 0);
        if (PresetLibraryEditor<T>.SettingsMenu.HasDefaultPresets())
        {
          genericMenu.AddSeparator(string.Empty);
          genericMenu.AddItem(new GUIContent("Add Factory Presets To Current Library"), false, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.AddDefaultPresetsToCurrentLibrary), (object) 0);
        }
        genericMenu.AddSeparator(string.Empty);
        genericMenu.AddItem(new GUIContent("Reveal Current Library Location"), false, new GenericMenu.MenuFunction2(PresetLibraryEditor<T>.SettingsMenu.RevealCurrentLibrary), (object) 0);
        genericMenu.DropDown(activatorRect);
      }

      private static void ViewModeChange(object userData)
      {
        PresetLibraryEditor<T>.SettingsMenu.ViewModeData viewModeData = (PresetLibraryEditor<T>.SettingsMenu.ViewModeData) userData;
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.itemViewMode = viewModeData.viewmode;
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.previewHeight = (float) viewModeData.itemHeight;
      }

      private static void LibraryModeChange(object userData)
      {
        string str = (string) userData;
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.currentLibraryWithoutExtension = str;
      }

      private static void CreateLibrary(object userData)
      {
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.wantsToCreateLibrary = true;
      }

      private static void RevealCurrentLibrary(object userData)
      {
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.RevealCurrentLibrary();
      }

      private static bool HasDefaultPresets()
      {
        return PresetLibraryEditor<T>.SettingsMenu.s_Owner.addDefaultPresets != null;
      }

      private static void AddDefaultPresetsToCurrentLibrary(object userData)
      {
        if (PresetLibraryEditor<T>.SettingsMenu.s_Owner.addDefaultPresets != null)
          PresetLibraryEditor<T>.SettingsMenu.s_Owner.addDefaultPresets((PresetLibrary) PresetLibraryEditor<T>.SettingsMenu.s_Owner.GetCurrentLib());
        PresetLibraryEditor<T>.SettingsMenu.s_Owner.SaveCurrentLib();
      }

      private class ViewModeData
      {
        public GUIContent text;
        public int itemHeight;
        public PresetLibraryEditorState.ItemViewMode viewmode;
      }
    }
  }
}
