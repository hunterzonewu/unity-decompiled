// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMaskInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AvatarMask))]
  internal class AvatarMaskInspector : Editor
  {
    private static AvatarMaskInspector.Styles styles = new AvatarMaskInspector.Styles();
    private bool m_ShowBodyMask = true;
    private bool m_CanImport = true;
    private bool m_BodyMaskFoldout;
    private SerializedProperty m_BodyMask;
    private SerializedProperty m_TransformMask;
    private SerializedProperty m_AnimationType;
    private AnimationClipInfoProperties m_ClipInfo;
    private string[] m_TransformPaths;
    private AvatarMaskInspector.NodeInfo[] m_NodeInfos;
    private Avatar m_RefAvatar;
    private ModelImporter m_RefImporter;
    private bool m_TransformMaskFoldout;
    private string[] m_HumanTransform;

    public bool canImport
    {
      get
      {
        return this.m_CanImport;
      }
      set
      {
        this.m_CanImport = value;
      }
    }

    public AnimationClipInfoProperties clipInfo
    {
      get
      {
        return this.m_ClipInfo;
      }
      set
      {
        this.m_ClipInfo = value;
        if (this.m_ClipInfo != null)
        {
          this.m_ClipInfo.MaskFromClip(this.target as AvatarMask);
          SerializedObject serializedObject = this.m_ClipInfo.maskTypeProperty.serializedObject;
          this.m_AnimationType = serializedObject.FindProperty("m_AnimationType");
          this.m_TransformPaths = (serializedObject.targetObject as ModelImporter).transformPaths;
        }
        else
        {
          this.m_TransformPaths = (string[]) null;
          this.m_AnimationType = (SerializedProperty) null;
        }
        this.InitializeSerializedProperties();
      }
    }

    private ModelImporterAnimationType animationType
    {
      get
      {
        if (this.m_AnimationType != null)
          return (ModelImporterAnimationType) this.m_AnimationType.intValue;
        return ModelImporterAnimationType.None;
      }
    }

    public bool showBody
    {
      get
      {
        return this.m_ShowBodyMask;
      }
      set
      {
        this.m_ShowBodyMask = value;
      }
    }

    public string[] humanTransforms
    {
      get
      {
        if (this.animationType == ModelImporterAnimationType.Human && this.clipInfo != null)
        {
          if (this.m_HumanTransform == null)
          {
            SerializedObject serializedObject = this.clipInfo.maskTypeProperty.serializedObject;
            ModelImporter targetObject = serializedObject.targetObject as ModelImporter;
            this.m_HumanTransform = AvatarMaskUtility.GetAvatarHumanTransform(serializedObject, targetObject.transformPaths);
          }
        }
        else
          this.m_HumanTransform = (string[]) null;
        return this.m_HumanTransform;
      }
    }

    private void InitializeSerializedProperties()
    {
      if (this.clipInfo != null)
      {
        this.m_BodyMask = this.clipInfo.bodyMaskProperty;
        this.m_TransformMask = this.clipInfo.transformMaskProperty;
      }
      else
      {
        this.m_BodyMask = this.serializedObject.FindProperty("m_Mask");
        this.m_TransformMask = this.serializedObject.FindProperty("m_Elements");
      }
    }

    private void OnEnable()
    {
      this.InitializeSerializedProperties();
    }

    public override void OnInspectorGUI()
    {
      Profiler.BeginSample("AvatarMaskInspector.OnInspectorGUI()");
      if (this.clipInfo == null)
        this.serializedObject.Update();
      bool flag = false;
      if (this.clipInfo != null)
      {
        EditorGUI.BeginChangeCheck();
        int maskType = (int) this.clipInfo.maskType;
        EditorGUI.showMixedValue = this.clipInfo.maskTypeProperty.hasMultipleDifferentValues;
        int num = EditorGUILayout.Popup(AvatarMaskInspector.styles.MaskDefinition, maskType, AvatarMaskInspector.styles.MaskDefinitionOpt, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          this.clipInfo.maskType = (ClipAnimationMaskType) num;
          this.UpdateMask(this.clipInfo.maskType);
        }
        flag = this.clipInfo.maskType == ClipAnimationMaskType.CopyFromOther;
      }
      if (flag)
        this.CopyFromOtherGUI();
      bool enabled = GUI.enabled;
      GUI.enabled = !flag;
      EditorGUI.BeginChangeCheck();
      this.OnBodyInspectorGUI();
      this.OnTransformInspectorGUI();
      if (this.clipInfo != null && EditorGUI.EndChangeCheck())
        this.clipInfo.MaskFromClip(this.target as AvatarMask);
      GUI.enabled = enabled;
      if (this.clipInfo == null)
        this.serializedObject.ApplyModifiedProperties();
      Profiler.EndSample();
    }

    protected void CopyFromOtherGUI()
    {
      if (this.clipInfo == null)
        return;
      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.clipInfo.maskSourceProperty, GUIContent.Temp("Source"), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck() && (UnityEngine.Object) (this.clipInfo.maskSourceProperty.objectReferenceValue as AvatarMask) != (UnityEngine.Object) null)
        this.UpdateMask(this.clipInfo.maskType);
      EditorGUILayout.EndHorizontal();
    }

    public bool IsMaskUpToDate()
    {
      if (this.clipInfo == null || this.m_TransformMask.arraySize != this.m_TransformPaths.Length)
        return false;
      for (int index = 0; index < this.m_TransformPaths.Length; ++index)
      {
        if (this.m_TransformMask.GetArrayElementAtIndex(index).FindPropertyRelative("m_Path").stringValue != this.m_TransformPaths[index])
          return false;
      }
      return true;
    }

    private void UpdateMask(ClipAnimationMaskType maskType)
    {
      if (this.clipInfo == null)
        return;
      if (maskType == ClipAnimationMaskType.CreateFromThisModel)
      {
        AvatarMaskUtility.UpdateTransformMask(this.m_TransformMask, (this.clipInfo.maskTypeProperty.serializedObject.targetObject as ModelImporter).transformPaths, this.humanTransforms);
        this.FillNodeInfos();
      }
      else if (maskType == ClipAnimationMaskType.CopyFromOther)
      {
        AvatarMask objectReferenceValue = this.clipInfo.maskSourceProperty.objectReferenceValue as AvatarMask;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
        {
          AvatarMask target = this.target as AvatarMask;
          target.Copy(objectReferenceValue);
          if (this.humanTransforms != null)
            AvatarMaskUtility.SetActiveHumanTransforms(target, this.humanTransforms);
          this.clipInfo.MaskToClip(target);
          this.FillNodeInfos();
        }
      }
      this.clipInfo.MaskFromClip(this.target as AvatarMask);
    }

    public void OnBodyInspectorGUI()
    {
      if (!this.m_ShowBodyMask)
        return;
      bool changed = GUI.changed;
      this.m_BodyMaskFoldout = EditorGUILayout.Foldout(this.m_BodyMaskFoldout, AvatarMaskInspector.styles.BodyMask);
      GUI.changed = changed;
      if (!this.m_BodyMaskFoldout)
        return;
      BodyMaskEditor.Show(this.m_BodyMask, 13);
    }

    public void OnTransformInspectorGUI()
    {
      float xmin = 0.0f;
      float ymin = 0.0f;
      float num = 0.0f;
      float ymax = 0.0f;
      bool changed = GUI.changed;
      this.m_TransformMaskFoldout = EditorGUILayout.Foldout(this.m_TransformMaskFoldout, AvatarMaskInspector.styles.TransformMask);
      GUI.changed = changed;
      if (this.m_TransformMaskFoldout)
      {
        if (this.canImport)
          this.ImportAvatarReference();
        if (this.m_NodeInfos == null || this.m_TransformMask.arraySize != this.m_NodeInfos.Length)
          this.FillNodeInfos();
        this.ComputeShownElements();
        GUILayout.Space(1f);
        int indentLevel = EditorGUI.indentLevel;
        int arraySize = this.m_TransformMask.arraySize;
        for (int index = 1; index < arraySize; ++index)
        {
          if (this.m_NodeInfos[index].m_Show)
          {
            GUILayout.BeginHorizontal();
            EditorGUI.indentLevel = this.m_NodeInfos[index].m_Depth + 1;
            EditorGUI.BeginChangeCheck();
            Rect rect = GUILayoutUtility.GetRect(15f, 15f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) });
            GUILayoutUtility.GetRect(10f, 15f, new GUILayoutOption[1]
            {
              GUILayout.ExpandWidth(false)
            });
            rect.x += 15f;
            bool enabled = GUI.enabled;
            GUI.enabled = this.m_NodeInfos[index].m_Enabled;
            bool flag1 = Event.current.button == 1;
            bool flag2 = (double) this.m_NodeInfos[index].m_Weight.floatValue > 0.0;
            bool flag3 = GUI.Toggle(rect, flag2, string.Empty);
            GUI.enabled = enabled;
            if (EditorGUI.EndChangeCheck())
            {
              this.m_NodeInfos[index].m_Weight.floatValue = !flag3 ? 0.0f : 1f;
              if (!flag1)
                this.CheckChildren(index, flag3);
            }
            if (this.m_NodeInfos[index].m_ChildIndices.Count > 0)
              this.m_NodeInfos[index].m_Expanded = EditorGUILayout.Foldout(this.m_NodeInfos[index].m_Expanded, this.m_NodeInfos[index].m_Name);
            else
              EditorGUILayout.LabelField(this.m_NodeInfos[index].m_Name);
            if (index == 1)
            {
              ymin = rect.yMin;
              xmin = rect.xMin;
            }
            else if (index == arraySize - 1)
              ymax = rect.yMax;
            num = Mathf.Max(num, GUILayoutUtility.GetLastRect().xMax);
            GUILayout.EndHorizontal();
          }
        }
        EditorGUI.indentLevel = indentLevel;
      }
      if (Event.current == null || Event.current.type != EventType.MouseUp || (Event.current.button != 1 || !Rect.MinMaxRect(xmin, ymin, num, ymax).Contains(Event.current.mousePosition)))
        return;
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Select all"), false, new GenericMenu.MenuFunction(this.SelectAll));
      genericMenu.AddItem(new GUIContent("Deselect all"), false, new GenericMenu.MenuFunction(this.DeselectAll));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void SetAllTransformActive(bool active)
    {
      for (int index = 0; index < this.m_NodeInfos.Length; ++index)
      {
        if (this.m_NodeInfos[index].m_Enabled)
          this.m_NodeInfos[index].m_Weight.floatValue = !active ? 0.0f : 1f;
      }
    }

    private void SelectAll()
    {
      this.SetAllTransformActive(true);
    }

    private void DeselectAll()
    {
      this.SetAllTransformActive(false);
    }

    private void ImportAvatarReference()
    {
      EditorGUI.BeginChangeCheck();
      this.m_RefAvatar = EditorGUILayout.ObjectField("Use skeleton from", (UnityEngine.Object) this.m_RefAvatar, typeof (Avatar), true, new GUILayoutOption[0]) as Avatar;
      if (EditorGUI.EndChangeCheck())
        this.m_RefImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) this.m_RefAvatar)) as ModelImporter;
      if (!((UnityEngine.Object) this.m_RefImporter != (UnityEngine.Object) null) || !GUILayout.Button("Import skeleton"))
        return;
      AvatarMaskUtility.UpdateTransformMask(this.m_TransformMask, this.m_RefImporter.transformPaths, (string[]) null);
    }

    private void FillNodeInfos()
    {
      this.m_NodeInfos = new AvatarMaskInspector.NodeInfo[this.m_TransformMask.arraySize];
      for (int index1 = 1; index1 < this.m_NodeInfos.Length; ++index1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvatarMaskInspector.\u003CFillNodeInfos\u003Ec__AnonStorey86 infosCAnonStorey86 = new AvatarMaskInspector.\u003CFillNodeInfos\u003Ec__AnonStorey86();
        this.m_NodeInfos[index1].m_Path = this.m_TransformMask.GetArrayElementAtIndex(index1).FindPropertyRelative("m_Path");
        this.m_NodeInfos[index1].m_Weight = this.m_TransformMask.GetArrayElementAtIndex(index1).FindPropertyRelative("m_Weight");
        // ISSUE: reference to a compiler-generated field
        infosCAnonStorey86.fullPath = this.m_NodeInfos[index1].m_Path.stringValue;
        // ISSUE: reference to a compiler-generated method
        this.m_NodeInfos[index1].m_Enabled = this.humanTransforms == null || ArrayUtility.FindIndex<string>(this.humanTransforms, new Predicate<string>(infosCAnonStorey86.\u003C\u003Em__146)) == -1;
        this.m_NodeInfos[index1].m_Expanded = true;
        this.m_NodeInfos[index1].m_ParentIndex = -1;
        this.m_NodeInfos[index1].m_ChildIndices = new List<int>();
        // ISSUE: reference to a compiler-generated field
        this.m_NodeInfos[index1].m_Depth = index1 != 0 ? infosCAnonStorey86.fullPath.Count<char>((Func<char, bool>) (f => (int) f == 47)) : 0;
        string str = string.Empty;
        // ISSUE: reference to a compiler-generated field
        int length = infosCAnonStorey86.fullPath.LastIndexOf('/');
        if (length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          str = infosCAnonStorey86.fullPath.Substring(0, length);
        }
        int startIndex = length != -1 ? length + 1 : 0;
        // ISSUE: reference to a compiler-generated field
        this.m_NodeInfos[index1].m_Name = infosCAnonStorey86.fullPath.Substring(startIndex);
        int arraySize = this.m_TransformMask.arraySize;
        for (int index2 = 0; index2 < arraySize; ++index2)
        {
          string stringValue = this.m_TransformMask.GetArrayElementAtIndex(index2).FindPropertyRelative("m_Path").stringValue;
          if (str != string.Empty && stringValue == str)
            this.m_NodeInfos[index1].m_ParentIndex = index2;
          // ISSUE: reference to a compiler-generated field
          if (stringValue.StartsWith(infosCAnonStorey86.fullPath) && stringValue.Count<char>((Func<char, bool>) (f => (int) f == 47)) == this.m_NodeInfos[index1].m_Depth + 1)
            this.m_NodeInfos[index1].m_ChildIndices.Add(index2);
        }
      }
    }

    private void ComputeShownElements()
    {
      for (int currentIndex = 0; currentIndex < this.m_NodeInfos.Length; ++currentIndex)
      {
        if (this.m_NodeInfos[currentIndex].m_ParentIndex == -1)
          this.ComputeShownElements(currentIndex, true);
      }
    }

    private void ComputeShownElements(int currentIndex, bool show)
    {
      this.m_NodeInfos[currentIndex].m_Show = show;
      bool show1 = show && this.m_NodeInfos[currentIndex].m_Expanded;
      using (List<int>.Enumerator enumerator = this.m_NodeInfos[currentIndex].m_ChildIndices.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.ComputeShownElements(enumerator.Current, show1);
      }
    }

    private void CheckChildren(int index, bool value)
    {
      using (List<int>.Enumerator enumerator = this.m_NodeInfos[index].m_ChildIndices.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          int current = enumerator.Current;
          if (this.m_NodeInfos[current].m_Enabled)
            this.m_NodeInfos[current].m_Weight.floatValue = !value ? 0.0f : 1f;
          this.CheckChildren(current, value);
        }
      }
    }

    private class Styles
    {
      public GUIContent MaskDefinition = EditorGUIUtility.TextContent("Definition|Choose between Create From This Model, Copy From Other Avatar. The first one create a Mask for this file and the second one use a Mask from another file to import animation.");
      public GUIContent[] MaskDefinitionOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Create From This Model|Create a Mask based on the model from this file. For Humanoid rig all the human transform are always imported and converted to muscle curve, thus they cannot be unchecked."), EditorGUIUtility.TextContent("Copy From Other Mask|Copy a Mask from another file to import animation clip.") };
      public GUIContent BodyMask = EditorGUIUtility.TextContent("Humanoid|Define which body part are active. Also define which animation curves will be imported for an Animation Clip.");
      public GUIContent TransformMask = EditorGUIUtility.TextContent("Transform|Define which transform are active. Also define which animation curves will be imported for an Animation Clip.");
    }

    private struct NodeInfo
    {
      public bool m_Expanded;
      public bool m_Show;
      public bool m_Enabled;
      public int m_ParentIndex;
      public List<int> m_ChildIndices;
      public int m_Depth;
      public SerializedProperty m_Path;
      public SerializedProperty m_Weight;
      public string m_Name;
    }
  }
}
