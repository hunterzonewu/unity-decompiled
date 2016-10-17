// Decompiled with JetBrains decompiler
// Type: UnityEditor.TagManagerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TagManager))]
  internal class TagManagerInspector : Editor
  {
    protected SerializedProperty m_Tags;
    protected SerializedProperty m_SortingLayers;
    protected SerializedProperty m_Layers;
    private ReorderableList m_TagsList;
    private ReorderableList m_SortLayersList;
    private ReorderableList m_LayersList;
    protected bool m_IsEditable;

    public TagManager tagManager
    {
      get
      {
        return this.target as TagManager;
      }
    }

    internal override string targetTitle
    {
      get
      {
        return "Tags & Layers";
      }
    }

    public virtual void OnEnable()
    {
      this.m_Tags = this.serializedObject.FindProperty("tags");
      if (this.m_TagsList == null)
      {
        this.m_TagsList = new ReorderableList(this.serializedObject, this.m_Tags, false, false, true, true);
        this.m_TagsList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddToTagsList);
        this.m_TagsList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveFromTagsList);
        this.m_TagsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawTagListElement);
        this.m_TagsList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_TagsList.headerHeight = 3f;
      }
      this.m_SortingLayers = this.serializedObject.FindProperty("m_SortingLayers");
      if (this.m_SortLayersList == null)
      {
        this.m_SortLayersList = new ReorderableList(this.serializedObject, this.m_SortingLayers, true, false, true, true);
        this.m_SortLayersList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ReorderSortLayerList);
        this.m_SortLayersList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddToSortLayerList);
        this.m_SortLayersList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveFromSortLayerList);
        this.m_SortLayersList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveSortLayerEntry);
        this.m_SortLayersList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawSortLayerListElement);
        this.m_SortLayersList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_SortLayersList.headerHeight = 3f;
      }
      this.m_Layers = this.serializedObject.FindProperty("layers");
      if (this.m_LayersList == null)
      {
        this.m_LayersList = new ReorderableList(this.serializedObject, this.m_Layers, false, false, false, false);
        this.m_LayersList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawLayerListElement);
        this.m_LayersList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
        this.m_LayersList.headerHeight = 3f;
      }
      this.m_Tags.isExpanded = false;
      this.m_SortingLayers.isExpanded = false;
      this.m_Layers.isExpanded = false;
      string defaultExpandedFoldout = this.tagManager.m_DefaultExpandedFoldout;
      if (defaultExpandedFoldout != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (TagManagerInspector.\u003C\u003Ef__switch\u0024map1E == null)
        {
          // ISSUE: reference to a compiler-generated field
          TagManagerInspector.\u003C\u003Ef__switch\u0024map1E = new Dictionary<string, int>(3)
          {
            {
              "Tags",
              0
            },
            {
              "SortingLayers",
              1
            },
            {
              "Layers",
              2
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (TagManagerInspector.\u003C\u003Ef__switch\u0024map1E.TryGetValue(defaultExpandedFoldout, out num))
        {
          switch (num)
          {
            case 0:
              this.m_Tags.isExpanded = true;
              return;
            case 1:
              this.m_SortingLayers.isExpanded = true;
              return;
            case 2:
              this.m_Layers.isExpanded = true;
              return;
          }
        }
      }
      this.m_Layers.isExpanded = true;
    }

    private void AddToTagsList(ReorderableList list)
    {
      int arraySize = this.m_Tags.arraySize;
      this.m_Tags.InsertArrayElementAtIndex(arraySize);
      this.m_Tags.GetArrayElementAtIndex(arraySize).stringValue = "New Tag";
      list.index = list.serializedProperty.arraySize - 1;
    }

    private void RemoveFromTagsList(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
    }

    private void DrawTagListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable;
      string stringValue = this.m_Tags.GetArrayElementAtIndex(index).stringValue;
      string str = EditorGUI.TextField(rect, " Tag " + (object) index, stringValue);
      if (str != stringValue)
        this.m_Tags.GetArrayElementAtIndex(index).stringValue = str;
      GUI.enabled = enabled;
    }

    private void AddToSortLayerList(ReorderableList list)
    {
      this.serializedObject.ApplyModifiedProperties();
      InternalEditorUtility.AddSortingLayer();
      this.serializedObject.Update();
      list.index = list.serializedProperty.arraySize - 1;
    }

    public void ReorderSortLayerList(ReorderableList list)
    {
      InternalEditorUtility.UpdateSortingLayersOrder();
    }

    private void RemoveFromSortLayerList(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
      this.serializedObject.ApplyModifiedProperties();
      this.serializedObject.Update();
      InternalEditorUtility.UpdateSortingLayersOrder();
    }

    private bool CanEditSortLayerEntry(int index)
    {
      if (index < 0 || index >= InternalEditorUtility.GetSortingLayerCount())
        return false;
      return !InternalEditorUtility.IsSortingLayerDefault(index);
    }

    private bool CanRemoveSortLayerEntry(ReorderableList list)
    {
      return this.CanEditSortLayerEntry(list.index);
    }

    private void DrawSortLayerListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable && this.CanEditSortLayerEntry(index);
      string sortingLayerName = InternalEditorUtility.GetSortingLayerName(index);
      string name = EditorGUI.TextField(rect, " Layer ", sortingLayerName);
      if (name != sortingLayerName)
      {
        this.serializedObject.ApplyModifiedProperties();
        InternalEditorUtility.SetSortingLayerName(index, name);
        this.serializedObject.Update();
      }
      GUI.enabled = enabled;
    }

    private void DrawLayerListElement(Rect rect, int index, bool selected, bool focused)
    {
      rect.height -= 2f;
      rect.xMin -= 20f;
      bool flag = index >= 8;
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable && flag;
      string stringValue = this.m_Layers.GetArrayElementAtIndex(index).stringValue;
      string str = !flag ? EditorGUI.TextField(rect, " Builtin Layer " + (object) index, stringValue) : EditorGUI.TextField(rect, " User Layer " + (object) index, stringValue);
      if (str != stringValue)
        this.m_Layers.GetArrayElementAtIndex(index).stringValue = str;
      GUI.enabled = enabled;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.m_IsEditable = AssetDatabase.IsOpenForEdit("ProjectSettings/TagManager.asset");
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_IsEditable;
      this.m_Tags.isExpanded = EditorGUILayout.Foldout(this.m_Tags.isExpanded, "Tags");
      if (this.m_Tags.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_TagsList.DoLayoutList();
        --EditorGUI.indentLevel;
      }
      this.m_SortingLayers.isExpanded = EditorGUILayout.Foldout(this.m_SortingLayers.isExpanded, "Sorting Layers");
      if (this.m_SortingLayers.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_SortLayersList.DoLayoutList();
        --EditorGUI.indentLevel;
      }
      this.m_Layers.isExpanded = EditorGUILayout.Foldout(this.m_Layers.isExpanded, "Layers");
      if (this.m_Layers.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.m_LayersList.DoLayoutList();
        --EditorGUI.indentLevel;
      }
      GUI.enabled = enabled;
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
