// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASHistoryFileView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ASHistoryFileView
  {
    private static float m_RowHeight = 16f;
    private static int ms_FileViewHash = "FileView".GetHashCode();
    private static bool OSX = Application.platform == RuntimePlatform.OSXEditor;
    private static ASHistoryFileView.Styles ms_Styles = (ASHistoryFileView.Styles) null;
    private int[] m_ExpandedArray = new int[0];
    private float m_FoldoutSize = 14f;
    private float m_Indent = 16f;
    private float m_BaseIndent = 16f;
    private float m_SpaceAtTheTop = ASHistoryFileView.m_RowHeight + 6f;
    private int m_FileViewControlID = -1;
    private ParentViewState m_DelPVstate = new ParentViewState();
    private GUIContent[] dropDownMenuItems = new GUIContent[1]
    {
      new GUIContent("Recover")
    };
    public Vector2 m_ScrollPosition;
    private ASHistoryFileView.SelectionType m_SelType;
    private bool m_DeletedItemsToggle;
    private DeletedAsset[] m_DeletedItems;
    private bool m_DeletedItemsInitialized;
    private Rect m_ScreenRect;

    public ASHistoryFileView.SelectionType SelType
    {
      get
      {
        return this.m_SelType;
      }
      set
      {
        if (this.m_SelType == ASHistoryFileView.SelectionType.DeletedItems && value != ASHistoryFileView.SelectionType.DeletedItems)
        {
          for (int index = 0; index < this.m_DelPVstate.selectedItems.Length; ++index)
            this.m_DelPVstate.selectedItems[index] = false;
        }
        this.m_SelType = value;
      }
    }

    private bool DeletedItemsToggle
    {
      get
      {
        return this.m_DeletedItemsToggle;
      }
      set
      {
        this.m_DeletedItemsToggle = value;
        if (!this.m_DeletedItemsToggle || this.m_DeletedItemsInitialized)
          return;
        this.SetupDeletedItems();
      }
    }

    public ASHistoryFileView()
    {
      this.m_DelPVstate.lv = new ListViewState(0);
      this.m_DelPVstate.lv.totalRows = 0;
    }

    private void SetupDeletedItems()
    {
      this.m_DeletedItems = AssetServer.GetServerDeletedItems();
      this.m_DelPVstate.Clear();
      this.m_DelPVstate.lv = new ListViewState(0);
      this.m_DelPVstate.AddAssetItems(this.m_DeletedItems);
      this.m_DelPVstate.AddAssetItems(AssetServer.GetLocalDeletedItems());
      this.m_DelPVstate.SetLineCount();
      this.m_DelPVstate.selectedItems = new bool[this.m_DelPVstate.lv.totalRows];
      this.m_DeletedItemsInitialized = true;
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0 || selected != 0)
        return;
      this.DoRecover();
    }

    public void SelectDeletedItem(string guid)
    {
      this.SelType = ASHistoryFileView.SelectionType.DeletedItems;
      this.DeletedItemsToggle = true;
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_DelPVstate.folders.Length; ++index2)
      {
        ParentViewFolder folder = this.m_DelPVstate.folders[index2];
        this.m_DelPVstate.selectedItems[index1] = false;
        if (folder.guid == guid)
        {
          this.m_DelPVstate.selectedItems[index1] = true;
          this.ScrollToDeletedItem(index1);
          break;
        }
        for (int index3 = 0; index3 < folder.files.Length; ++index3)
        {
          ++index1;
          this.m_DelPVstate.selectedItems[index1] = false;
          if (folder.files[index3].guid == guid)
          {
            this.m_DelPVstate.selectedItems[index1] = true;
            this.ScrollToDeletedItem(index1);
            return;
          }
        }
        ++index1;
      }
    }

    public void DoRecover()
    {
      string[] deletedItemGuiDs = this.GetSelectedDeletedItemGUIDs();
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      int num = 0;
      for (int index1 = 0; index1 < deletedItemGuiDs.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.m_DeletedItems.Length; ++index2)
        {
          if (this.m_DeletedItems[index2].guid == deletedItemGuiDs[index1])
          {
            dictionary[this.m_DeletedItems[index2].guid] = index2;
            break;
          }
        }
      }
      DeletedAsset[] assets = new DeletedAsset[dictionary.Count];
      while (dictionary.Count != 0)
      {
        DeletedAsset deletedAsset = (DeletedAsset) null;
        using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            deletedAsset = this.m_DeletedItems[enumerator.Current.Value];
            if (!dictionary.ContainsKey(deletedAsset.parent))
            {
              assets[num++] = deletedAsset;
              break;
            }
          }
        }
        dictionary.Remove(deletedAsset.guid);
      }
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBReinitASMainWindow");
      AssetServer.DoRecoverOnNextTick(assets);
    }

    public string[] GetSelectedDeletedItemGUIDs()
    {
      List<string> stringList = new List<string>();
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_DelPVstate.folders.Length; ++index2)
      {
        ParentViewFolder folder = this.m_DelPVstate.folders[index2];
        if (this.m_DelPVstate.selectedItems[index1])
          stringList.Add(folder.guid);
        for (int index3 = 0; index3 < folder.files.Length; ++index3)
        {
          ++index1;
          if (this.m_DelPVstate.selectedItems[index1])
            stringList.Add(folder.files[index3].guid);
        }
        ++index1;
      }
      return stringList.ToArray();
    }

    public string[] GetAllDeletedItemGUIDs()
    {
      if (!this.m_DeletedItemsInitialized)
        this.SetupDeletedItems();
      string[] strArray = new string[this.m_DeletedItems.Length];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = this.m_DeletedItems[index].guid;
      return strArray;
    }

    public void FilterItems(string filterText)
    {
    }

    private int ControlIDForProperty(HierarchyProperty property)
    {
      if (property != null)
        return property.row + 10000000;
      return -1;
    }

    private void SetExpanded(int instanceID, bool expand)
    {
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < this.m_ExpandedArray.Length; ++index)
        hashtable.Add((object) this.m_ExpandedArray[index], (object) null);
      if (expand != hashtable.Contains((object) instanceID))
      {
        if (expand)
          hashtable.Add((object) instanceID, (object) null);
        else
          hashtable.Remove((object) instanceID);
        this.m_ExpandedArray = new int[hashtable.Count];
        int index = 0;
        foreach (int key in (IEnumerable) hashtable.Keys)
        {
          this.m_ExpandedArray[index] = key;
          ++index;
        }
      }
      InternalEditorUtility.expandedProjectWindowItems = this.m_ExpandedArray;
    }

    private void SetExpandedRecurse(int instanceID, bool expand)
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      if (!hierarchyProperty.Find(instanceID, this.m_ExpandedArray))
        return;
      this.SetExpanded(instanceID, expand);
      int depth = hierarchyProperty.depth;
      while (hierarchyProperty.Next((int[]) null) && hierarchyProperty.depth > depth)
        this.SetExpanded(hierarchyProperty.instanceID, expand);
    }

    private void SelectionClick(HierarchyProperty property)
    {
      if (EditorGUI.actionKey)
      {
        ArrayList arrayList = new ArrayList();
        arrayList.AddRange((ICollection) Selection.objects);
        if (arrayList.Contains((object) property.pptrValue))
          arrayList.Remove((object) property.pptrValue);
        else
          arrayList.Add((object) property.pptrValue);
        Selection.objects = arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[];
      }
      else if (Event.current.shift)
      {
        HierarchyProperty firstSelected = this.GetFirstSelected();
        HierarchyProperty lastSelected = this.GetLastSelected();
        if (!firstSelected.isValid)
        {
          Selection.activeObject = property.pptrValue;
          return;
        }
        HierarchyProperty hierarchyProperty1;
        HierarchyProperty hierarchyProperty2;
        if (property.row > lastSelected.row)
        {
          hierarchyProperty1 = firstSelected;
          hierarchyProperty2 = property;
        }
        else
        {
          hierarchyProperty1 = property;
          hierarchyProperty2 = lastSelected;
        }
        ArrayList arrayList = new ArrayList();
        arrayList.Add((object) hierarchyProperty1.pptrValue);
        while (hierarchyProperty1.Next(this.m_ExpandedArray))
        {
          arrayList.Add((object) hierarchyProperty1.pptrValue);
          if (hierarchyProperty1.instanceID == hierarchyProperty2.instanceID)
            break;
        }
        Selection.objects = arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[];
      }
      else
        Selection.activeObject = property.pptrValue;
      this.SelType = ASHistoryFileView.SelectionType.Items;
      this.FrameObject(Selection.activeObject);
    }

    private HierarchyProperty GetActiveSelected()
    {
      return this.GetFirstSelected();
    }

    private HierarchyProperty GetFirstSelected()
    {
      int num = 1000000000;
      HierarchyProperty hierarchyProperty1 = (HierarchyProperty) null;
      foreach (UnityEngine.Object @object in Selection.objects)
      {
        HierarchyProperty hierarchyProperty2 = new HierarchyProperty(HierarchyType.Assets);
        if (hierarchyProperty2.Find(@object.GetInstanceID(), this.m_ExpandedArray) && hierarchyProperty2.row < num)
        {
          num = hierarchyProperty2.row;
          hierarchyProperty1 = hierarchyProperty2;
        }
      }
      return hierarchyProperty1;
    }

    private HierarchyProperty GetLast()
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      int count = hierarchyProperty.CountRemaining(this.m_ExpandedArray);
      hierarchyProperty.Reset();
      if (hierarchyProperty.Skip(count, this.m_ExpandedArray))
        return hierarchyProperty;
      return (HierarchyProperty) null;
    }

    private HierarchyProperty GetFirst()
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      if (hierarchyProperty.Next(this.m_ExpandedArray))
        return hierarchyProperty;
      return (HierarchyProperty) null;
    }

    private void OpenAssetSelection()
    {
      foreach (int instanceId in Selection.instanceIDs)
      {
        if (AssetDatabase.Contains(instanceId))
          AssetDatabase.OpenAsset(instanceId);
      }
    }

    private HierarchyProperty GetLastSelected()
    {
      int num = -1;
      HierarchyProperty hierarchyProperty1 = (HierarchyProperty) null;
      foreach (UnityEngine.Object @object in Selection.objects)
      {
        HierarchyProperty hierarchyProperty2 = new HierarchyProperty(HierarchyType.Assets);
        if (hierarchyProperty2.Find(@object.GetInstanceID(), this.m_ExpandedArray) && hierarchyProperty2.row > num)
        {
          num = hierarchyProperty2.row;
          hierarchyProperty1 = hierarchyProperty2;
        }
      }
      return hierarchyProperty1;
    }

    private void AllProjectKeyboard()
    {
      if (Event.current.keyCode != KeyCode.DownArrow || this.GetFirst() == null)
        return;
      Selection.activeObject = this.GetFirst().pptrValue;
      this.FrameObject(Selection.activeObject);
      this.SelType = ASHistoryFileView.SelectionType.Items;
      Event.current.Use();
    }

    private void AssetViewKeyboard()
    {
      KeyCode keyCode = Event.current.keyCode;
      switch (keyCode)
      {
        case KeyCode.KeypadEnter:
          if (Application.platform == RuntimePlatform.WindowsEditor)
          {
            this.OpenAssetSelection();
            GUIUtility.ExitGUI();
            break;
          }
          break;
        case KeyCode.UpArrow:
          Event.current.Use();
          HierarchyProperty firstSelected = this.GetFirstSelected();
          if (firstSelected != null)
          {
            if (firstSelected.instanceID == this.GetFirst().instanceID)
            {
              this.SelType = ASHistoryFileView.SelectionType.All;
              Selection.objects = new UnityEngine.Object[0];
              this.ScrollTo(0.0f);
              break;
            }
            if (firstSelected.Previous(this.m_ExpandedArray))
            {
              UnityEngine.Object pptrValue = firstSelected.pptrValue;
              this.SelectionClick(firstSelected);
              this.FrameObject(pptrValue);
              break;
            }
            break;
          }
          break;
        case KeyCode.DownArrow:
          Event.current.Use();
          HierarchyProperty lastSelected = this.GetLastSelected();
          if (Application.platform == RuntimePlatform.OSXEditor && Event.current.command)
          {
            this.OpenAssetSelection();
            GUIUtility.ExitGUI();
            break;
          }
          if (lastSelected != null)
          {
            if (lastSelected.instanceID == this.GetLast().instanceID)
            {
              this.SelType = ASHistoryFileView.SelectionType.DeletedItemsRoot;
              Selection.objects = new UnityEngine.Object[0];
              this.ScrollToDeletedItem(-1);
              break;
            }
            if (lastSelected.Next(this.m_ExpandedArray))
            {
              UnityEngine.Object pptrValue = lastSelected.pptrValue;
              this.SelectionClick(lastSelected);
              this.FrameObject(pptrValue);
              break;
            }
            break;
          }
          break;
        case KeyCode.RightArrow:
          HierarchyProperty activeSelected1 = this.GetActiveSelected();
          if (activeSelected1 != null)
          {
            this.SetExpanded(activeSelected1.instanceID, true);
            break;
          }
          break;
        case KeyCode.LeftArrow:
          HierarchyProperty activeSelected2 = this.GetActiveSelected();
          if (activeSelected2 != null)
          {
            this.SetExpanded(activeSelected2.instanceID, false);
            break;
          }
          break;
        case KeyCode.Home:
          if (this.GetFirst() != null)
          {
            Selection.activeObject = this.GetFirst().pptrValue;
            this.FrameObject(Selection.activeObject);
            break;
          }
          break;
        case KeyCode.End:
          if (this.GetLast() != null)
          {
            Selection.activeObject = this.GetLast().pptrValue;
            this.FrameObject(Selection.activeObject);
            break;
          }
          break;
        case KeyCode.PageUp:
          Event.current.Use();
          if (ASHistoryFileView.OSX)
          {
            this.m_ScrollPosition.y -= this.m_ScreenRect.height;
            if ((double) this.m_ScrollPosition.y < 0.0)
            {
              this.m_ScrollPosition.y = 0.0f;
              break;
            }
            break;
          }
          HierarchyProperty property1 = this.GetFirstSelected();
          if (property1 != null)
          {
            for (int index = 0; (double) index < (double) this.m_ScreenRect.height / (double) ASHistoryFileView.m_RowHeight; ++index)
            {
              if (!property1.Previous(this.m_ExpandedArray))
              {
                property1 = this.GetFirst();
                break;
              }
            }
            UnityEngine.Object pptrValue = property1.pptrValue;
            this.SelectionClick(property1);
            this.FrameObject(pptrValue);
            break;
          }
          if (this.GetFirst() != null)
          {
            Selection.activeObject = this.GetFirst().pptrValue;
            this.FrameObject(Selection.activeObject);
            break;
          }
          break;
        case KeyCode.PageDown:
          Event.current.Use();
          if (ASHistoryFileView.OSX)
          {
            this.m_ScrollPosition.y += this.m_ScreenRect.height;
            break;
          }
          HierarchyProperty property2 = this.GetLastSelected();
          if (property2 != null)
          {
            for (int index = 0; (double) index < (double) this.m_ScreenRect.height / (double) ASHistoryFileView.m_RowHeight; ++index)
            {
              if (!property2.Next(this.m_ExpandedArray))
              {
                property2 = this.GetLast();
                break;
              }
            }
            UnityEngine.Object pptrValue = property2.pptrValue;
            this.SelectionClick(property2);
            this.FrameObject(pptrValue);
            break;
          }
          if (this.GetLast() != null)
          {
            Selection.activeObject = this.GetLast().pptrValue;
            this.FrameObject(Selection.activeObject);
            break;
          }
          break;
        default:
          if (keyCode != KeyCode.Return)
            return;
          goto case KeyCode.KeypadEnter;
      }
      Event.current.Use();
    }

    private void DeletedItemsRootKeyboard(ASHistoryWindow parentWin)
    {
      switch (Event.current.keyCode)
      {
        case KeyCode.UpArrow:
          this.SelType = ASHistoryFileView.SelectionType.Items;
          if (this.GetLast() != null)
          {
            Selection.activeObject = this.GetLast().pptrValue;
            this.FrameObject(Selection.activeObject);
            break;
          }
          break;
        case KeyCode.DownArrow:
          if (this.m_DelPVstate.selectedItems.Length > 0 && this.DeletedItemsToggle)
          {
            this.SelType = ASHistoryFileView.SelectionType.DeletedItems;
            this.m_DelPVstate.selectedItems[0] = true;
            this.m_DelPVstate.lv.row = 0;
            this.ScrollToDeletedItem(0);
            break;
          }
          break;
        case KeyCode.RightArrow:
          this.DeletedItemsToggle = true;
          break;
        case KeyCode.LeftArrow:
          this.DeletedItemsToggle = false;
          break;
        default:
          return;
      }
      if (this.SelType != ASHistoryFileView.SelectionType.Items)
        parentWin.DoLocalSelectionChange();
      Event.current.Use();
    }

    private void DeletedItemsKeyboard(ASHistoryWindow parentWin)
    {
      int row = this.m_DelPVstate.lv.row;
      int num = row;
      if (!this.DeletedItemsToggle)
        return;
      switch (Event.current.keyCode)
      {
        case KeyCode.UpArrow:
          if (num > 0)
          {
            --num;
            break;
          }
          this.SelType = ASHistoryFileView.SelectionType.DeletedItemsRoot;
          this.ScrollToDeletedItem(-1);
          parentWin.DoLocalSelectionChange();
          break;
        case KeyCode.DownArrow:
          if (num < this.m_DelPVstate.lv.totalRows - 1)
          {
            ++num;
            break;
          }
          break;
        case KeyCode.RightArrow:
          return;
        case KeyCode.LeftArrow:
          return;
        case KeyCode.Insert:
          return;
        case KeyCode.Home:
          num = 0;
          break;
        case KeyCode.End:
          num = this.m_DelPVstate.lv.totalRows - 1;
          break;
        case KeyCode.PageUp:
          if (ASHistoryFileView.OSX)
          {
            this.m_ScrollPosition.y -= this.m_ScreenRect.height;
            if ((double) this.m_ScrollPosition.y < 0.0)
            {
              this.m_ScrollPosition.y = 0.0f;
              break;
            }
            break;
          }
          num -= (int) ((double) this.m_ScreenRect.height / (double) ASHistoryFileView.m_RowHeight);
          if (num < 0)
          {
            num = 0;
            break;
          }
          break;
        case KeyCode.PageDown:
          if (ASHistoryFileView.OSX)
          {
            this.m_ScrollPosition.y += this.m_ScreenRect.height;
            break;
          }
          num += (int) ((double) this.m_ScreenRect.height / (double) ASHistoryFileView.m_RowHeight);
          if (num > this.m_DelPVstate.lv.totalRows - 1)
          {
            num = this.m_DelPVstate.lv.totalRows - 1;
            break;
          }
          break;
        default:
          return;
      }
      Event.current.Use();
      if (num == row)
        return;
      this.m_DelPVstate.lv.row = num;
      ListViewShared.MultiSelection((ListViewShared.InternalListViewState) null, row, num, ref this.m_DelPVstate.initialSelectedItem, ref this.m_DelPVstate.selectedItems);
      this.ScrollToDeletedItem(num);
      parentWin.DoLocalSelectionChange();
    }

    private void ScrollToDeletedItem(int index)
    {
      float scrollTop = (float) ((double) this.m_SpaceAtTheTop + (double) new HierarchyProperty(HierarchyType.Assets).CountRemaining(this.m_ExpandedArray) * (double) ASHistoryFileView.m_RowHeight + 6.0);
      if (index == -1)
        this.ScrollTo(scrollTop);
      else
        this.ScrollTo(scrollTop + (float) (index + 1) * ASHistoryFileView.m_RowHeight);
    }

    private void KeyboardGUI(ASHistoryWindow parentWin)
    {
      if (Event.current.GetTypeForControl(this.m_FileViewControlID) != EventType.KeyDown || this.m_FileViewControlID != GUIUtility.keyboardControl)
        return;
      switch (this.SelType)
      {
        case ASHistoryFileView.SelectionType.All:
          this.AllProjectKeyboard();
          break;
        case ASHistoryFileView.SelectionType.Items:
          this.AssetViewKeyboard();
          break;
        case ASHistoryFileView.SelectionType.DeletedItemsRoot:
          this.DeletedItemsRootKeyboard(parentWin);
          break;
        case ASHistoryFileView.SelectionType.DeletedItems:
          this.DeletedItemsKeyboard(parentWin);
          break;
      }
    }

    private bool FrameObject(UnityEngine.Object target)
    {
      if (target == (UnityEngine.Object) null)
        return false;
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      if (hierarchyProperty.Find(target.GetInstanceID(), (int[]) null))
      {
        while (hierarchyProperty.Parent())
          this.SetExpanded(hierarchyProperty.instanceID, true);
      }
      hierarchyProperty.Reset();
      if (!hierarchyProperty.Find(target.GetInstanceID(), this.m_ExpandedArray))
        return false;
      this.ScrollTo(ASHistoryFileView.m_RowHeight * (float) hierarchyProperty.row + this.m_SpaceAtTheTop);
      return true;
    }

    private void ScrollTo(float scrollTop)
    {
      this.m_ScrollPosition.y = Mathf.Clamp(this.m_ScrollPosition.y, scrollTop - this.m_ScreenRect.height + ASHistoryFileView.m_RowHeight, scrollTop);
    }

    public void DoDeletedItemsGUI(ASHistoryWindow parentWin, Rect theRect, GUIStyle s, float offset, float endOffset, bool focused)
    {
      Event current = Event.current;
      Texture2D texture = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      offset += 3f;
      Rect position1 = new Rect(this.m_Indent, offset, theRect.width - this.m_Indent, ASHistoryFileView.m_RowHeight);
      if (current.type == EventType.MouseDown && position1.Contains(current.mousePosition))
      {
        GUIUtility.keyboardControl = this.m_FileViewControlID;
        this.SelType = ASHistoryFileView.SelectionType.DeletedItemsRoot;
        this.ScrollToDeletedItem(-1);
        parentWin.DoLocalSelectionChange();
      }
      position1.width -= position1.x;
      position1.x = 0.0f;
      GUIContent content = new GUIContent("Deleted Assets");
      content.image = (Texture) texture;
      int baseIndent = (int) this.m_BaseIndent;
      s.padding.left = baseIndent;
      if (current.type == EventType.Repaint)
        s.Draw(position1, content, false, false, this.SelType == ASHistoryFileView.SelectionType.DeletedItemsRoot, focused);
      Rect position2 = new Rect(this.m_BaseIndent - this.m_FoldoutSize, offset, this.m_FoldoutSize, ASHistoryFileView.m_RowHeight);
      if (!this.m_DeletedItemsInitialized || this.m_DelPVstate.lv.totalRows != 0)
        this.DeletedItemsToggle = GUI.Toggle(position2, this.DeletedItemsToggle, GUIContent.none, ASHistoryFileView.ms_Styles.foldout);
      offset += ASHistoryFileView.m_RowHeight;
      if (!this.DeletedItemsToggle)
        return;
      int row = this.m_DelPVstate.lv.row;
      int index1 = 0;
      int folder1 = -1;
      int file = -1;
      int index2 = 0;
      while ((double) offset <= (double) endOffset && index2 < this.m_DelPVstate.lv.totalRows)
      {
        if ((double) offset + (double) ASHistoryFileView.m_RowHeight >= 0.0)
        {
          if (folder1 == -1)
            this.m_DelPVstate.IndexToFolderAndFile(index2, ref folder1, ref file);
          position1 = new Rect(0.0f, offset, (float) Screen.width, ASHistoryFileView.m_RowHeight);
          ParentViewFolder folder2 = this.m_DelPVstate.folders[folder1];
          if (current.type == EventType.MouseDown && position1.Contains(current.mousePosition))
          {
            if (current.button != 1 || this.SelType != ASHistoryFileView.SelectionType.DeletedItems || !this.m_DelPVstate.selectedItems[index1])
            {
              GUIUtility.keyboardControl = this.m_FileViewControlID;
              this.SelType = ASHistoryFileView.SelectionType.DeletedItems;
              this.m_DelPVstate.lv.row = index1;
              ListViewShared.MultiSelection((ListViewShared.InternalListViewState) null, row, this.m_DelPVstate.lv.row, ref this.m_DelPVstate.initialSelectedItem, ref this.m_DelPVstate.selectedItems);
              this.ScrollToDeletedItem(index1);
              parentWin.DoLocalSelectionChange();
            }
            if (current.button == 1 && this.SelType == ASHistoryFileView.SelectionType.DeletedItems)
            {
              GUIUtility.hotControl = 0;
              EditorUtility.DisplayCustomMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 1f, 1f), this.dropDownMenuItems, -1, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
            }
            Event.current.Use();
          }
          int num;
          if (file != -1)
          {
            content.text = folder2.files[file].name;
            content.image = (Texture) InternalEditorUtility.GetIconForFile(folder2.files[file].name);
            num = (int) ((double) this.m_BaseIndent + (double) this.m_Indent * 2.0);
          }
          else
          {
            content.text = folder2.name;
            content.image = (Texture) texture;
            num = (int) ((double) this.m_BaseIndent + (double) this.m_Indent);
          }
          s.padding.left = num;
          if (Event.current.type == EventType.Repaint)
            s.Draw(position1, content, false, false, this.m_DelPVstate.selectedItems[index1], focused);
          this.m_DelPVstate.NextFileFolder(ref folder1, ref file);
          ++index1;
        }
        ++index2;
        offset += ASHistoryFileView.m_RowHeight;
      }
    }

    public void DoGUI(ASHistoryWindow parentWin, Rect theRect, bool focused)
    {
      if (ASHistoryFileView.ms_Styles == null)
        ASHistoryFileView.ms_Styles = new ASHistoryFileView.Styles();
      this.m_ScreenRect = theRect;
      Hashtable hashtable = new Hashtable();
      foreach (UnityEngine.Object @object in Selection.objects)
        hashtable.Add((object) @object.GetInstanceID(), (object) null);
      this.m_FileViewControlID = GUIUtility.GetControlID(ASHistoryFileView.ms_FileViewHash, FocusType.Native);
      this.KeyboardGUI(parentWin);
      focused &= GUIUtility.keyboardControl == this.m_FileViewControlID;
      HierarchyProperty property = new HierarchyProperty(HierarchyType.Assets);
      int num1 = property.CountRemaining(this.m_ExpandedArray);
      int num2 = !this.DeletedItemsToggle ? 0 : this.m_DelPVstate.lv.totalRows;
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, (float) ((double) (num1 + 2 + num2) * (double) ASHistoryFileView.m_RowHeight + 16.0));
      this.m_ScrollPosition = GUI.BeginScrollView(this.m_ScreenRect, this.m_ScrollPosition, viewRect);
      theRect.width = (double) viewRect.height <= (double) this.m_ScreenRect.height ? theRect.width : theRect.width - 18f;
      int count = Mathf.RoundToInt(this.m_ScrollPosition.y - 6f - ASHistoryFileView.m_RowHeight) / Mathf.RoundToInt(ASHistoryFileView.m_RowHeight);
      if (count > num1)
        count = num1;
      else if (count < 0)
      {
        count = 0;
        this.m_ScrollPosition.y = 0.0f;
      }
      GUIContent guiContent = new GUIContent();
      Event current = Event.current;
      GUIStyle s = new GUIStyle(ASHistoryFileView.ms_Styles.label);
      Texture2D texture = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      float y1 = (float) ((double) count * (double) ASHistoryFileView.m_RowHeight + 3.0);
      float endOffset = this.m_ScreenRect.height + this.m_ScrollPosition.y;
      Rect position = new Rect(0.0f, y1, theRect.width, ASHistoryFileView.m_RowHeight);
      if (current.type == EventType.MouseDown && position.Contains(current.mousePosition))
      {
        this.SelType = ASHistoryFileView.SelectionType.All;
        GUIUtility.keyboardControl = this.m_FileViewControlID;
        this.ScrollTo(0.0f);
        parentWin.DoLocalSelectionChange();
        current.Use();
      }
      GUIContent content = new GUIContent("Entire Project");
      content.image = (Texture) texture;
      int baseIndent = (int) this.m_BaseIndent;
      s.padding.left = 3;
      if (Event.current.type == EventType.Repaint)
        s.Draw(position, content, false, false, this.SelType == ASHistoryFileView.SelectionType.All, focused);
      float y2 = y1 + (ASHistoryFileView.m_RowHeight + 3f);
      property.Reset();
      property.Skip(count, this.m_ExpandedArray);
      while (property.Next(this.m_ExpandedArray) && (double) y2 <= (double) endOffset)
      {
        int instanceId = property.instanceID;
        position = new Rect(0.0f, y2, theRect.width, ASHistoryFileView.m_RowHeight);
        if (Event.current.type == EventType.Repaint)
        {
          content.text = property.name;
          content.image = (Texture) property.icon;
          int num3 = (int) ((double) this.m_BaseIndent + (double) this.m_Indent * (double) property.depth);
          s.padding.left = num3;
          bool on = hashtable.Contains((object) instanceId);
          s.Draw(position, content, false, false, on, focused);
        }
        if (property.hasChildren)
        {
          bool flag = property.IsExpanded(this.m_ExpandedArray);
          GUI.changed = false;
          bool expand = GUI.Toggle(new Rect(this.m_BaseIndent + this.m_Indent * (float) property.depth - this.m_FoldoutSize, y2, this.m_FoldoutSize, ASHistoryFileView.m_RowHeight), flag, GUIContent.none, ASHistoryFileView.ms_Styles.foldout);
          if (GUI.changed)
          {
            if (Event.current.alt)
              this.SetExpandedRecurse(instanceId, expand);
            else
              this.SetExpanded(instanceId, expand);
          }
        }
        if (current.type == EventType.MouseDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition))
        {
          GUIUtility.keyboardControl = this.m_FileViewControlID;
          if (Event.current.clickCount == 2)
          {
            AssetDatabase.OpenAsset(instanceId);
            GUIUtility.ExitGUI();
          }
          else if (position.Contains(current.mousePosition))
            this.SelectionClick(property);
          current.Use();
        }
        y2 += ASHistoryFileView.m_RowHeight;
      }
      float offset = y2 + 3f;
      this.DoDeletedItemsGUI(parentWin, theRect, s, offset, endOffset, focused);
      GUI.EndScrollView();
      switch (current.type)
      {
        case EventType.MouseDown:
          if (current.button == 0 && this.m_ScreenRect.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = this.m_FileViewControlID;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == this.m_FileViewControlID)
          {
            if (this.m_ScreenRect.Contains(current.mousePosition))
              Selection.activeObject = (UnityEngine.Object) null;
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
      }
      this.HandleFraming();
    }

    private void HandleFraming()
    {
      if (Event.current.type != EventType.ExecuteCommand && Event.current.type != EventType.ValidateCommand || !(Event.current.commandName == "FrameSelected"))
        return;
      if (Event.current.type == EventType.ExecuteCommand)
        this.DoFramingMindSelectionType();
      HandleUtility.Repaint();
      Event.current.Use();
    }

    private void DoFramingMindSelectionType()
    {
      switch (this.m_SelType)
      {
        case ASHistoryFileView.SelectionType.All:
          this.ScrollTo(0.0f);
          break;
        case ASHistoryFileView.SelectionType.Items:
          this.FrameObject(Selection.activeObject);
          break;
        case ASHistoryFileView.SelectionType.DeletedItemsRoot:
          this.ScrollToDeletedItem(-1);
          break;
        case ASHistoryFileView.SelectionType.DeletedItems:
          this.ScrollToDeletedItem(this.m_DelPVstate.lv.row);
          break;
      }
    }

    private List<int> GetOneFolderImplicitSelection(HierarchyProperty property, Hashtable selection, bool rootSelected, ref bool retHasSelectionInside, out bool eof)
    {
      int depth = property.depth;
      bool retHasSelectionInside1 = false;
      bool flag = false;
      eof = false;
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      do
      {
        if (property.depth > depth)
          intList3.AddRange((IEnumerable<int>) this.GetOneFolderImplicitSelection(property, selection, rootSelected || flag, ref retHasSelectionInside1, out eof));
        if (depth == property.depth && !eof)
        {
          if (rootSelected && !retHasSelectionInside1)
            intList1.Add(property.instanceID);
          if (selection.Contains((object) property.instanceID))
          {
            intList2.Add(property.instanceID);
            retHasSelectionInside1 = true;
            flag = true;
          }
          else
            flag = false;
          eof = !property.Next((int[]) null);
        }
        else
          break;
      }
      while (!eof);
      retHasSelectionInside = retHasSelectionInside | retHasSelectionInside1;
      List<int> intList4 = !rootSelected || retHasSelectionInside1 ? intList2 : intList1;
      intList4.AddRange((IEnumerable<int>) intList3);
      return intList4;
    }

    public string[] GetImplicitProjectViewSelection()
    {
      HierarchyProperty property = new HierarchyProperty(HierarchyType.Assets);
      bool retHasSelectionInside = false;
      if (!property.Next((int[]) null))
        return new string[0];
      Hashtable selection = new Hashtable();
      foreach (UnityEngine.Object @object in Selection.objects)
        selection.Add((object) @object.GetInstanceID(), (object) null);
      bool eof;
      List<int> implicitSelection = this.GetOneFolderImplicitSelection(property, selection, false, ref retHasSelectionInside, out eof);
      string[] strArray = new string[implicitSelection.Count];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(implicitSelection[index]));
      return strArray;
    }

    public enum SelectionType
    {
      None,
      All,
      Items,
      DeletedItemsRoot,
      DeletedItems,
    }

    private class Styles
    {
      public GUIStyle foldout = (GUIStyle) "IN Foldout";
      public GUIStyle insertion = (GUIStyle) "PR Insertion";
      public GUIStyle label = (GUIStyle) "PR Label";
      public GUIStyle ping = new GUIStyle((GUIStyle) "PR Ping");
      public GUIStyle toolbarButton = (GUIStyle) "ToolbarButton";

      public Styles()
      {
        this.ping.overflow.left = -2;
        this.ping.overflow.right = -21;
        this.ping.padding.left = 48;
        this.ping.padding.right = 0;
      }
    }
  }
}
