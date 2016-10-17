// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterClipEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ModelImporterClipEditor : AssetImporterInspector
  {
    public int m_SelectedClipIndexDoNotUseDirectly = -1;
    private const int kFrameColumnWidth = 45;
    private AnimationClipEditor m_AnimationClipEditor;
    private SerializedObject m_DefaultClipsSerializedObject;
    private SerializedProperty m_AnimationType;
    private SerializedProperty m_ImportAnimation;
    private SerializedProperty m_ClipAnimations;
    private SerializedProperty m_BakeSimulation;
    private SerializedProperty m_ResampleCurves;
    private SerializedProperty m_AnimationCompression;
    private SerializedProperty m_AnimationRotationError;
    private SerializedProperty m_AnimationPositionError;
    private SerializedProperty m_AnimationScaleError;
    private SerializedProperty m_AnimationWrapMode;
    private SerializedProperty m_LegacyGenerateAnimations;
    private SerializedProperty m_MotionNodeName;
    private SerializedProperty m_PivotNodeName;
    private SerializedProperty m_AnimationImportErrors;
    private SerializedProperty m_AnimationImportWarnings;
    private SerializedProperty m_AnimationRetargetingWarnings;
    private SerializedProperty m_AnimationDoRetargetingWarnings;
    private GUIContent[] m_MotionNodeList;
    private static bool motionNodeFoldout;
    private static bool importMessageFoldout;
    private ReorderableList m_ClipList;
    private static ModelImporterClipEditor.Styles styles;
    private AvatarMask m_Mask;
    private AvatarMaskInspector m_MaskInspector;
    private static bool m_MaskFoldout;

    private ModelImporter singleImporter
    {
      get
      {
        return this.targets[0] as ModelImporter;
      }
    }

    public int selectedClipIndex
    {
      get
      {
        return this.m_SelectedClipIndexDoNotUseDirectly;
      }
      set
      {
        this.m_SelectedClipIndexDoNotUseDirectly = value;
        if (this.m_ClipList == null)
          return;
        this.m_ClipList.index = value;
      }
    }

    public int motionNodeIndex { get; set; }

    public int pivotNodeIndex { get; set; }

    private string[] referenceTransformPaths
    {
      get
      {
        return this.singleImporter.transformPaths;
      }
    }

    private ModelImporterAnimationType animationType
    {
      get
      {
        return (ModelImporterAnimationType) this.m_AnimationType.intValue;
      }
      set
      {
        this.m_AnimationType.intValue = (int) value;
      }
    }

    private ModelImporterGenerateAnimations legacyGenerateAnimations
    {
      get
      {
        return (ModelImporterGenerateAnimations) this.m_LegacyGenerateAnimations.intValue;
      }
      set
      {
        this.m_LegacyGenerateAnimations.intValue = (int) value;
      }
    }

    public void OnEnable()
    {
      this.m_ClipAnimations = this.serializedObject.FindProperty("m_ClipAnimations");
      this.m_AnimationType = this.serializedObject.FindProperty("m_AnimationType");
      this.m_LegacyGenerateAnimations = this.serializedObject.FindProperty("m_LegacyGenerateAnimations");
      this.m_ImportAnimation = this.serializedObject.FindProperty("m_ImportAnimation");
      this.m_BakeSimulation = this.serializedObject.FindProperty("m_BakeSimulation");
      this.m_ResampleCurves = this.serializedObject.FindProperty("m_ResampleRotations");
      this.m_AnimationCompression = this.serializedObject.FindProperty("m_AnimationCompression");
      this.m_AnimationRotationError = this.serializedObject.FindProperty("m_AnimationRotationError");
      this.m_AnimationPositionError = this.serializedObject.FindProperty("m_AnimationPositionError");
      this.m_AnimationScaleError = this.serializedObject.FindProperty("m_AnimationScaleError");
      this.m_AnimationWrapMode = this.serializedObject.FindProperty("m_AnimationWrapMode");
      this.m_AnimationImportErrors = this.serializedObject.FindProperty("m_AnimationImportErrors");
      this.m_AnimationImportWarnings = this.serializedObject.FindProperty("m_AnimationImportWarnings");
      this.m_AnimationRetargetingWarnings = this.serializedObject.FindProperty("m_AnimationRetargetingWarnings");
      this.m_AnimationDoRetargetingWarnings = this.serializedObject.FindProperty("m_AnimationDoRetargetingWarnings");
      if (this.serializedObject.isEditingMultipleObjects)
        return;
      if (this.m_ClipAnimations.arraySize == 0)
        this.SetupDefaultClips();
      this.selectedClipIndex = EditorPrefs.GetInt("ModelImporterClipEditor.ActiveClipIndex", 0);
      this.ValidateClipSelectionIndex();
      EditorPrefs.SetInt("ModelImporterClipEditor.ActiveClipIndex", this.selectedClipIndex);
      if ((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null && this.selectedClipIndex >= 0)
        this.SyncClipEditor();
      if (this.m_ClipAnimations.arraySize != 0)
        this.SelectClip(this.selectedClipIndex);
      string[] transformPaths = this.singleImporter.transformPaths;
      this.m_MotionNodeList = new GUIContent[transformPaths.Length + 1];
      this.m_MotionNodeList[0] = new GUIContent("<None>");
      for (int index = 0; index < transformPaths.Length; ++index)
      {
        if (index == 0)
          this.m_MotionNodeList[1] = new GUIContent("<Root Transform>");
        else
          this.m_MotionNodeList[index + 1] = new GUIContent(transformPaths[index]);
      }
      this.m_MotionNodeName = this.serializedObject.FindProperty("m_MotionNodeName");
      this.motionNodeIndex = ArrayUtility.FindIndex<GUIContent>(this.m_MotionNodeList, (Predicate<GUIContent>) (content => content.text == this.m_MotionNodeName.stringValue));
      this.motionNodeIndex = this.motionNodeIndex >= 1 ? this.motionNodeIndex : 0;
    }

    private void SyncClipEditor()
    {
      if ((UnityEngine.Object) this.m_AnimationClipEditor == (UnityEngine.Object) null || (UnityEngine.Object) this.m_MaskInspector == (UnityEngine.Object) null)
        return;
      AnimationClipInfoProperties animationClipInfoAtIndex = this.GetAnimationClipInfoAtIndex(this.selectedClipIndex);
      this.m_MaskInspector.clipInfo = animationClipInfoAtIndex;
      this.m_AnimationClipEditor.ShowRange(animationClipInfoAtIndex);
      this.m_AnimationClipEditor.mask = this.m_Mask;
    }

    private void SetupDefaultClips()
    {
      this.m_DefaultClipsSerializedObject = new SerializedObject(this.target);
      this.m_ClipAnimations = this.m_DefaultClipsSerializedObject.FindProperty("m_ClipAnimations");
      this.m_AnimationType = this.m_DefaultClipsSerializedObject.FindProperty("m_AnimationType");
      this.m_ClipAnimations.arraySize = 0;
      foreach (TakeInfo importedTakeInfo in this.singleImporter.importedTakeInfos)
        this.AddClip(importedTakeInfo);
    }

    private void PatchDefaultClipTakeNamesToSplitClipNames()
    {
      foreach (TakeInfo importedTakeInfo in this.singleImporter.importedTakeInfos)
        PatchImportSettingRecycleID.Patch(this.serializedObject, 74, importedTakeInfo.name, importedTakeInfo.defaultClipName);
    }

    private void TransferDefaultClipsToCustomClips()
    {
      if (this.m_DefaultClipsSerializedObject == null)
        return;
      if (this.serializedObject.FindProperty("m_ClipAnimations").arraySize != 0)
        Debug.LogError((object) "Transferring default clips failed, target already has clips");
      this.serializedObject.CopyFromSerializedProperty(this.m_ClipAnimations);
      this.m_ClipAnimations = this.serializedObject.FindProperty("m_ClipAnimations");
      this.m_DefaultClipsSerializedObject = (SerializedObject) null;
      this.PatchDefaultClipTakeNamesToSplitClipNames();
      this.SyncClipEditor();
    }

    private void ValidateClipSelectionIndex()
    {
      if (this.selectedClipIndex <= this.m_ClipAnimations.arraySize - 1)
        return;
      this.selectedClipIndex = 0;
    }

    public void OnDestroy()
    {
      this.DestroyEditorsAndData();
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.m_ClipAnimations = this.serializedObject.FindProperty("m_ClipAnimations");
      this.m_AnimationType = this.serializedObject.FindProperty("m_AnimationType");
      this.m_DefaultClipsSerializedObject = (SerializedObject) null;
      if (this.m_ClipAnimations.arraySize == 0)
        this.SetupDefaultClips();
      this.ValidateClipSelectionIndex();
      this.UpdateList();
      this.SelectClip(this.selectedClipIndex);
    }

    private void AnimationClipGUI()
    {
      string stringValue1 = this.m_AnimationImportErrors.stringValue;
      string stringValue2 = this.m_AnimationImportWarnings.stringValue;
      string stringValue3 = this.m_AnimationRetargetingWarnings.stringValue;
      if (stringValue1.Length > 0)
        EditorGUILayout.HelpBox("Error(s) found while importing animation this file. Open \"Import Messages\" foldout below for more details", MessageType.Error);
      else if (stringValue2.Length > 0)
        EditorGUILayout.HelpBox("Warning(s) found while importing this animation file. Open \"Import Messages\" foldout below for more details", MessageType.Warning);
      this.AnimationSettings();
      if (this.serializedObject.isEditingMultipleObjects)
        return;
      Profiler.BeginSample("Clip inspector");
      EditorGUILayout.Space();
      if (this.targets.Length == 1)
        this.AnimationSplitTable();
      else
        GUILayout.Label(ModelImporterClipEditor.styles.clipMultiEditInfo, EditorStyles.helpBox, new GUILayoutOption[0]);
      Profiler.EndSample();
      this.RootMotionNodeSettings();
      ModelImporterClipEditor.importMessageFoldout = EditorGUILayout.Foldout(ModelImporterClipEditor.importMessageFoldout, ModelImporterClipEditor.styles.ImportMessages);
      if (!ModelImporterClipEditor.importMessageFoldout)
        return;
      if (stringValue1.Length > 0)
        EditorGUILayout.HelpBox(stringValue1, MessageType.Error);
      if (stringValue2.Length > 0)
        EditorGUILayout.HelpBox(stringValue2, MessageType.Warning);
      if (this.animationType != ModelImporterAnimationType.Human)
        return;
      EditorGUILayout.PropertyField(this.m_AnimationDoRetargetingWarnings, ModelImporterClipEditor.styles.GenerateRetargetingWarnings, new GUILayoutOption[0]);
      if (this.m_AnimationDoRetargetingWarnings.boolValue)
      {
        if (stringValue3.Length <= 0)
          return;
        EditorGUILayout.HelpBox(stringValue3, MessageType.Info);
      }
      else
        EditorGUILayout.HelpBox("Retargeting Quality compares retargeted with original animation. It reports average and maximum position/orientation difference for body parts. It may slow down import time of this file.", MessageType.Info);
    }

    public override void OnInspectorGUI()
    {
      if (ModelImporterClipEditor.styles == null)
        ModelImporterClipEditor.styles = new ModelImporterClipEditor.Styles();
      EditorGUILayout.PropertyField(this.m_ImportAnimation, ModelImporterClipEditor.styles.ImportAnimations, new GUILayoutOption[0]);
      if (this.m_ImportAnimation.boolValue && !this.m_ImportAnimation.hasMultipleDifferentValues)
      {
        bool flag = this.targets.Length == 1 && this.singleImporter.importedTakeInfos.Length == 0;
        if (this.IsDeprecatedMultiAnimationRootImport())
          EditorGUILayout.HelpBox("Animation data was imported using a deprecated Generation option in the Rig tab. Please switch to a non-deprecated import mode in the Rig tab to be able to edit the animation import settings.", MessageType.Info);
        else if (flag)
        {
          if (this.serializedObject.hasModifiedProperties)
            EditorGUILayout.HelpBox("The animations settings can be edited after clicking Apply.", MessageType.Info);
          else
            EditorGUILayout.HelpBox("No animation data available in this model.", MessageType.Info);
        }
        else if (this.m_AnimationType.hasMultipleDifferentValues)
          EditorGUILayout.HelpBox("The rigs of the selected models have different animation types.", MessageType.Info);
        else if (this.animationType == ModelImporterAnimationType.None)
          EditorGUILayout.HelpBox("The rigs is not setup to handle animation. Edit the settings in the Rig tab.", MessageType.Info);
        else if (this.m_ImportAnimation.boolValue && !this.m_ImportAnimation.hasMultipleDifferentValues)
          this.AnimationClipGUI();
      }
      this.ApplyRevertGUI();
    }

    private void AnimationSettings()
    {
      EditorGUILayout.Space();
      bool flag = true;
      foreach (ModelImporter target in this.targets)
      {
        if (!target.isBakeIKSupported)
          flag = false;
      }
      EditorGUI.BeginDisabledGroup(!flag);
      EditorGUILayout.PropertyField(this.m_BakeSimulation, ModelImporterClipEditor.styles.BakeIK, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      if (this.animationType == ModelImporterAnimationType.Generic)
        EditorGUILayout.PropertyField(this.m_ResampleCurves, ModelImporterClipEditor.styles.ResampleCurves, new GUILayoutOption[0]);
      else
        this.m_ResampleCurves.boolValue = true;
      if (this.animationType == ModelImporterAnimationType.Legacy)
      {
        EditorGUI.showMixedValue = this.m_AnimationWrapMode.hasMultipleDifferentValues;
        EditorGUILayout.Popup(this.m_AnimationWrapMode, ModelImporterClipEditor.styles.AnimWrapModeOpt, ModelImporterClipEditor.styles.AnimWrapModeLabel, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        int[] optionValues = new int[3]{ 0, 1, 2 };
        EditorGUILayout.IntPopup(this.m_AnimationCompression, ModelImporterClipEditor.styles.AnimCompressionOptLegacy, optionValues, ModelImporterClipEditor.styles.AnimCompressionLabel, new GUILayoutOption[0]);
      }
      else
      {
        int[] optionValues = new int[3]{ 0, 1, 3 };
        EditorGUILayout.IntPopup(this.m_AnimationCompression, ModelImporterClipEditor.styles.AnimCompressionOpt, optionValues, ModelImporterClipEditor.styles.AnimCompressionLabel, new GUILayoutOption[0]);
      }
      if (this.m_AnimationCompression.intValue <= 0)
        return;
      EditorGUILayout.PropertyField(this.m_AnimationRotationError, ModelImporterClipEditor.styles.AnimRotationErrorLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_AnimationPositionError, ModelImporterClipEditor.styles.AnimPositionErrorLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_AnimationScaleError, ModelImporterClipEditor.styles.AnimScaleErrorLabel, new GUILayoutOption[0]);
      GUILayout.Label(ModelImporterClipEditor.styles.AnimationCompressionHelp, EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private void RootMotionNodeSettings()
    {
      if (this.animationType != ModelImporterAnimationType.Human && this.animationType != ModelImporterAnimationType.Generic)
        return;
      ModelImporterClipEditor.motionNodeFoldout = EditorGUILayout.Foldout(ModelImporterClipEditor.motionNodeFoldout, ModelImporterClipEditor.styles.MotionSetting);
      if (!ModelImporterClipEditor.motionNodeFoldout)
        return;
      EditorGUI.BeginChangeCheck();
      this.motionNodeIndex = EditorGUILayout.Popup(ModelImporterClipEditor.styles.MotionNode, this.motionNodeIndex, this.m_MotionNodeList, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (this.motionNodeIndex > 0 && this.motionNodeIndex < this.m_MotionNodeList.Length)
        this.m_MotionNodeName.stringValue = this.m_MotionNodeList[this.motionNodeIndex].text;
      else
        this.m_MotionNodeName.stringValue = string.Empty;
    }

    private void DestroyEditorsAndData()
    {
      if ((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_AnimationClipEditor);
        this.m_AnimationClipEditor = (AnimationClipEditor) null;
      }
      if ((bool) ((UnityEngine.Object) this.m_MaskInspector))
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_MaskInspector);
        this.m_MaskInspector = (AvatarMaskInspector) null;
      }
      if (!(bool) ((UnityEngine.Object) this.m_Mask))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Mask);
      this.m_Mask = (AvatarMask) null;
    }

    private void SelectClip(int selected)
    {
      if (EditorGUI.s_DelayedTextEditor != null && Event.current != null)
        EditorGUI.s_DelayedTextEditor.EndGUI(Event.current.type);
      this.DestroyEditorsAndData();
      this.selectedClipIndex = selected;
      if (this.selectedClipIndex < 0 || this.selectedClipIndex >= this.m_ClipAnimations.arraySize)
      {
        this.selectedClipIndex = -1;
      }
      else
      {
        AnimationClipInfoProperties animationClipInfoAtIndex = this.GetAnimationClipInfoAtIndex(selected);
        AnimationClip animationClipForTake = this.singleImporter.GetPreviewAnimationClipForTake(animationClipInfoAtIndex.takeName);
        if (!((UnityEngine.Object) animationClipForTake != (UnityEngine.Object) null))
          return;
        this.m_AnimationClipEditor = (AnimationClipEditor) Editor.CreateEditor((UnityEngine.Object) animationClipForTake);
        this.InitMask(animationClipInfoAtIndex);
        this.SyncClipEditor();
      }
    }

    private void UpdateList()
    {
      if (this.m_ClipList == null)
        return;
      List<AnimationClipInfoProperties> clipInfoPropertiesList = new List<AnimationClipInfoProperties>();
      for (int index = 0; index < this.m_ClipAnimations.arraySize; ++index)
        clipInfoPropertiesList.Add(this.GetAnimationClipInfoAtIndex(index));
      this.m_ClipList.list = (IList) clipInfoPropertiesList;
    }

    private void AddClipInList(ReorderableList list)
    {
      if (this.m_DefaultClipsSerializedObject != null)
        this.TransferDefaultClipsToCustomClips();
      int index1 = 0;
      if (0 < this.selectedClipIndex && this.selectedClipIndex < this.m_ClipAnimations.arraySize)
      {
        AnimationClipInfoProperties animationClipInfoAtIndex = this.GetAnimationClipInfoAtIndex(this.selectedClipIndex);
        for (int index2 = 0; index2 < this.singleImporter.importedTakeInfos.Length; ++index2)
        {
          if (this.singleImporter.importedTakeInfos[index2].name == animationClipInfoAtIndex.takeName)
          {
            index1 = index2;
            break;
          }
        }
      }
      this.AddClip(this.singleImporter.importedTakeInfos[index1]);
      this.UpdateList();
      this.SelectClip(list.list.Count - 1);
    }

    private void RemoveClipInList(ReorderableList list)
    {
      this.TransferDefaultClipsToCustomClips();
      this.RemoveClip(list.index);
      this.UpdateList();
      this.SelectClip(Mathf.Min(list.index, list.count - 1));
    }

    private void SelectClipInList(ReorderableList list)
    {
      this.SelectClip(list.index);
    }

    private void DrawClipElement(Rect rect, int index, bool selected, bool focused)
    {
      AnimationClipInfoProperties clipInfoProperties = this.m_ClipList.list[index] as AnimationClipInfoProperties;
      rect.xMax -= 90f;
      GUI.Label(rect, clipInfoProperties.name, EditorStyles.label);
      rect.x = rect.xMax;
      rect.width = 45f;
      GUI.Label(rect, clipInfoProperties.firstFrame.ToString("0.0"), ModelImporterClipEditor.styles.numberStyle);
      rect.x = rect.xMax;
      GUI.Label(rect, clipInfoProperties.lastFrame.ToString("0.0"), ModelImporterClipEditor.styles.numberStyle);
    }

    private void DrawClipHeader(Rect rect)
    {
      rect.xMax -= 90f;
      GUI.Label(rect, "Clips", EditorStyles.label);
      rect.x = rect.xMax;
      rect.width = 45f;
      GUI.Label(rect, "Start", ModelImporterClipEditor.styles.numberStyle);
      rect.x = rect.xMax;
      GUI.Label(rect, "End", ModelImporterClipEditor.styles.numberStyle);
    }

    private void AnimationSplitTable()
    {
      if (this.m_ClipList == null)
      {
        this.m_ClipList = new ReorderableList((IList) new List<AnimationClipInfoProperties>(), typeof (string), false, true, true, true);
        this.m_ClipList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddClipInList);
        this.m_ClipList.onSelectCallback = new ReorderableList.SelectCallbackDelegate(this.SelectClipInList);
        this.m_ClipList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveClipInList);
        this.m_ClipList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawClipElement);
        this.m_ClipList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawClipHeader);
        this.m_ClipList.elementHeight = 16f;
        this.UpdateList();
        this.m_ClipList.index = this.selectedClipIndex;
      }
      this.m_ClipList.DoLayoutList();
      EditorGUI.BeginChangeCheck();
      AnimationClipInfoProperties selectedClipInfo = this.GetSelectedClipInfo();
      if (selectedClipInfo == null)
        return;
      if ((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null && this.selectedClipIndex != -1)
      {
        GUILayout.Space(5f);
        AnimationClip target = this.m_AnimationClipEditor.target as AnimationClip;
        if (!target.legacy)
          this.GetSelectedClipInfo().AssignToPreviewClip(target);
        TakeInfo[] importedTakeInfos = this.singleImporter.importedTakeInfos;
        string[] array = new string[importedTakeInfos.Length];
        for (int index = 0; index < importedTakeInfos.Length; ++index)
          array[index] = importedTakeInfos[index].name;
        EditorGUI.BeginChangeCheck();
        string name = selectedClipInfo.name;
        int num = ArrayUtility.IndexOf<string>(array, selectedClipInfo.takeName);
        this.m_AnimationClipEditor.takeNames = array;
        this.m_AnimationClipEditor.takeIndex = ArrayUtility.IndexOf<string>(array, selectedClipInfo.takeName);
        this.m_AnimationClipEditor.DrawHeader();
        if (EditorGUI.EndChangeCheck())
        {
          if (selectedClipInfo.name != name)
          {
            this.TransferDefaultClipsToCustomClips();
            PatchImportSettingRecycleID.Patch(this.serializedObject, 74, name, selectedClipInfo.name);
          }
          int takeIndex = this.m_AnimationClipEditor.takeIndex;
          if (takeIndex != -1 && takeIndex != num)
          {
            selectedClipInfo.name = this.MakeUniqueClipName(array[takeIndex], -1);
            this.SetupTakeNameAndFrames(selectedClipInfo, importedTakeInfos[takeIndex]);
            GUIUtility.keyboardControl = 0;
            this.SelectClip(this.selectedClipIndex);
            target = this.m_AnimationClipEditor.target as AnimationClip;
          }
        }
        this.m_AnimationClipEditor.OnInspectorGUI();
        this.AvatarMaskSettings(this.GetSelectedClipInfo());
        if (!target.legacy)
          this.GetSelectedClipInfo().ExtractFromPreviewClip(target);
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.TransferDefaultClipsToCustomClips();
    }

    public override bool HasPreviewGUI()
    {
      if ((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null)
        return this.m_AnimationClipEditor.HasPreviewGUI();
      return false;
    }

    public override void OnPreviewSettings()
    {
      if (!((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null))
        return;
      this.m_AnimationClipEditor.OnPreviewSettings();
    }

    private bool IsDeprecatedMultiAnimationRootImport()
    {
      if (this.animationType != ModelImporterAnimationType.Legacy)
        return false;
      if (this.legacyGenerateAnimations != ModelImporterGenerateAnimations.InOriginalRoots)
        return this.legacyGenerateAnimations == ModelImporterGenerateAnimations.InNodes;
      return true;
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (!(bool) ((UnityEngine.Object) this.m_AnimationClipEditor))
        return;
      this.m_AnimationClipEditor.OnInteractivePreviewGUI(r, background);
    }

    private AnimationClipInfoProperties GetAnimationClipInfoAtIndex(int index)
    {
      return new AnimationClipInfoProperties(this.m_ClipAnimations.GetArrayElementAtIndex(index));
    }

    private AnimationClipInfoProperties GetSelectedClipInfo()
    {
      if (this.selectedClipIndex >= 0 && this.selectedClipIndex < this.m_ClipAnimations.arraySize)
        return this.GetAnimationClipInfoAtIndex(this.selectedClipIndex);
      return (AnimationClipInfoProperties) null;
    }

    private string MakeUniqueClipName(string name, int row)
    {
      string str = name;
      int num = 0;
      int index;
      do
      {
        for (index = 0; index < this.m_ClipAnimations.arraySize; ++index)
        {
          AnimationClipInfoProperties animationClipInfoAtIndex = this.GetAnimationClipInfoAtIndex(index);
          if (str == animationClipInfoAtIndex.name && row != index)
          {
            str = name + num.ToString();
            ++num;
            break;
          }
        }
      }
      while (index != this.m_ClipAnimations.arraySize);
      return str;
    }

    private void RemoveClip(int index)
    {
      this.m_ClipAnimations.DeleteArrayElementAtIndex(index);
      if (this.m_ClipAnimations.arraySize != 0)
        return;
      this.SetupDefaultClips();
      this.m_ImportAnimation.boolValue = false;
    }

    private void SetupTakeNameAndFrames(AnimationClipInfoProperties info, TakeInfo takeInfo)
    {
      info.takeName = takeInfo.name;
      info.firstFrame = (float) (int) Mathf.Round(takeInfo.bakeStartTime * takeInfo.sampleRate);
      info.lastFrame = (float) (int) Mathf.Round(takeInfo.bakeStopTime * takeInfo.sampleRate);
    }

    private void AddClip(TakeInfo takeInfo)
    {
      this.m_ClipAnimations.InsertArrayElementAtIndex(this.m_ClipAnimations.arraySize);
      AnimationClipInfoProperties animationClipInfoAtIndex = this.GetAnimationClipInfoAtIndex(this.m_ClipAnimations.arraySize - 1);
      animationClipInfoAtIndex.name = this.MakeUniqueClipName(takeInfo.defaultClipName, -1);
      this.SetupTakeNameAndFrames(animationClipInfoAtIndex, takeInfo);
      animationClipInfoAtIndex.wrapMode = 0;
      animationClipInfoAtIndex.loop = false;
      animationClipInfoAtIndex.orientationOffsetY = 0.0f;
      animationClipInfoAtIndex.level = 0.0f;
      animationClipInfoAtIndex.cycleOffset = 0.0f;
      animationClipInfoAtIndex.loopTime = false;
      animationClipInfoAtIndex.loopBlend = false;
      animationClipInfoAtIndex.loopBlendOrientation = false;
      animationClipInfoAtIndex.loopBlendPositionY = false;
      animationClipInfoAtIndex.loopBlendPositionXZ = false;
      animationClipInfoAtIndex.keepOriginalOrientation = false;
      animationClipInfoAtIndex.keepOriginalPositionY = true;
      animationClipInfoAtIndex.keepOriginalPositionXZ = false;
      animationClipInfoAtIndex.heightFromFeet = false;
      animationClipInfoAtIndex.mirror = false;
      animationClipInfoAtIndex.maskType = ClipAnimationMaskType.CreateFromThisModel;
      this.SetBodyMaskDefaultValues(animationClipInfoAtIndex);
      this.SetTransformMaskFromReference(animationClipInfoAtIndex);
      animationClipInfoAtIndex.ClearEvents();
      animationClipInfoAtIndex.ClearCurves();
    }

    private void AvatarMaskSettings(AnimationClipInfoProperties clipInfo)
    {
      if (clipInfo == null || !((UnityEngine.Object) this.m_AnimationClipEditor != (UnityEngine.Object) null))
        return;
      this.InitMask(clipInfo);
      int indentLevel = EditorGUI.indentLevel;
      bool changed = GUI.changed;
      ModelImporterClipEditor.m_MaskFoldout = EditorGUILayout.Foldout(ModelImporterClipEditor.m_MaskFoldout, ModelImporterClipEditor.styles.Mask);
      GUI.changed = changed;
      if (clipInfo.maskType == ClipAnimationMaskType.CreateFromThisModel && !this.m_MaskInspector.IsMaskUpToDate())
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("Mask does not match hierarchy. Animation might not import correctly", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button("Fix Mask"))
          this.SetTransformMaskFromReference(clipInfo);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
      }
      else if (clipInfo.maskType == ClipAnimationMaskType.CopyFromOther && clipInfo.MaskNeedsUpdating())
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("Source Mask has changed since last import. It must be Updated", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button("Update Mask"))
          clipInfo.MaskToClip(clipInfo.maskSource);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
      }
      if (ModelImporterClipEditor.m_MaskFoldout)
      {
        ++EditorGUI.indentLevel;
        this.m_MaskInspector.OnInspectorGUI();
      }
      EditorGUI.indentLevel = indentLevel;
    }

    private void InitMask(AnimationClipInfoProperties clipInfo)
    {
      if (!((UnityEngine.Object) this.m_Mask == (UnityEngine.Object) null))
        return;
      AnimationClip target = this.m_AnimationClipEditor.target as AnimationClip;
      this.m_Mask = new AvatarMask();
      this.m_MaskInspector = (AvatarMaskInspector) Editor.CreateEditor((UnityEngine.Object) this.m_Mask);
      this.m_MaskInspector.canImport = false;
      this.m_MaskInspector.showBody = target.isHumanMotion;
      this.m_MaskInspector.clipInfo = clipInfo;
    }

    private void SetTransformMaskFromReference(AnimationClipInfoProperties clipInfo)
    {
      string[] referenceTransformPaths = this.referenceTransformPaths;
      string[] humanTransforms = this.animationType != ModelImporterAnimationType.Human ? (string[]) null : AvatarMaskUtility.GetAvatarHumanTransform(this.serializedObject, referenceTransformPaths);
      AvatarMaskUtility.UpdateTransformMask(clipInfo.transformMaskProperty, referenceTransformPaths, humanTransforms);
    }

    private void SetBodyMaskDefaultValues(AnimationClipInfoProperties clipInfo)
    {
      SerializedProperty bodyMaskProperty = clipInfo.bodyMaskProperty;
      bodyMaskProperty.ClearArray();
      for (int index = 0; index < 13; ++index)
      {
        bodyMaskProperty.InsertArrayElementAtIndex(index);
        bodyMaskProperty.GetArrayElementAtIndex(index).intValue = 1;
      }
    }

    private class Styles
    {
      public GUIContent ImportAnimations = EditorGUIUtility.TextContent("Import Animation|Controls if animations are imported.");
      public GUIStyle numberStyle = new GUIStyle(EditorStyles.label);
      public GUIContent AnimWrapModeLabel = EditorGUIUtility.TextContent("Wrap Mode|The default Wrap Mode for the animation in the mesh being imported.");
      public GUIContent[] AnimWrapModeOpt = new GUIContent[5]{ EditorGUIUtility.TextContent("Default|The animation plays as specified in the animation splitting options below."), EditorGUIUtility.TextContent("Once|The animation plays through to the end once and then stops."), EditorGUIUtility.TextContent("Loop|The animation plays through and then restarts when the end is reached."), EditorGUIUtility.TextContent("PingPong|The animation plays through and then plays in reverse from the end to the start, and so on."), EditorGUIUtility.TextContent("ClampForever|The animation plays through but the last frame is repeated indefinitely. This is not the same as Once mode because playback does not technically stop at the last frame (which is useful when blending animations).") };
      public GUIContent BakeIK = EditorGUIUtility.TextContent("Bake Animations|Enable this when using IK or simulation in your animation package. Unity will convert to forward kinematics on import. This option is available only for Maya, 3dsMax and Cinema4D files.");
      public GUIContent ResampleCurves = EditorGUIUtility.TextContent("Resample Curves | Curves will be resampled on every frame. Use this if you're having issues with the interpolation between keys in your original animation. Disable this to keep curves as close as possible to how they were originally authored.");
      public GUIContent AnimCompressionLabel = EditorGUIUtility.TextContent("Anim. Compression|The type of compression that will be applied to this mesh's animation(s).");
      public GUIContent[] AnimCompressionOptLegacy = new GUIContent[3]{ EditorGUIUtility.TextContent("Off|Disables animation compression. This means that Unity doesn't reduce keyframe count on import, which leads to the highest precision animations, but slower performance and bigger file and runtime memory size. It is generally not advisable to use this option - if you need higher precision animation, you should enable keyframe reduction and lower allowed Animation Compression Error values instead."), EditorGUIUtility.TextContent("Keyframe Reduction|Reduces keyframes on import. If selected, the Animation Compression Errors options are displayed."), EditorGUIUtility.TextContent("Keyframe Reduction and Compression|Reduces keyframes on import and compresses keyframes when storing animations in files. This affects only file size - the runtime memory size is the same as Keyframe Reduction. If selected, the Animation Compression Errors options are displayed.") };
      public GUIContent[] AnimCompressionOpt = new GUIContent[3]{ EditorGUIUtility.TextContent("Off|Disables animation compression. This means that Unity doesn't reduce keyframe count on import, which leads to the highest precision animations, but slower performance and bigger file and runtime memory size. It is generally not advisable to use this option - if you need higher precision animation, you should enable keyframe reduction and lower allowed Animation Compression Error values instead."), EditorGUIUtility.TextContent("Keyframe Reduction|Reduces keyframes on import. If selected, the Animation Compression Errors options are displayed."), EditorGUIUtility.TextContent("Optimal|Reduces keyframes on import and choose between different curve representations to reduce memory usage at runtime. This affects the runtime memory size and how curves are evaluated.") };
      public GUIContent AnimRotationErrorLabel = EditorGUIUtility.TextContent("Rotation Error|Defines how much rotation curves should be reduced. The smaller value you use - the higher precision you get.");
      public GUIContent AnimPositionErrorLabel = EditorGUIUtility.TextContent("Position Error|Defines how much position curves should be reduced. The smaller value you use - the higher precision you get.");
      public GUIContent AnimScaleErrorLabel = EditorGUIUtility.TextContent("Scale Error|Defines how much scale curves should be reduced. The smaller value you use - the higher precision you get.");
      public GUIContent AnimationCompressionHelp = EditorGUIUtility.TextContent("Rotation error is defined as maximum angle deviation allowed in degrees, for others it is defined as maximum distance/delta deviation allowed in percents");
      public GUIContent clipMultiEditInfo = new GUIContent("Multi-object editing of clips not supported.");
      public GUIContent updateMuscleDefinitionFromSource = EditorGUIUtility.TextContent("Update|Update the copy of the muscle definition from the source.");
      public GUIContent MotionSetting = EditorGUIUtility.TextContent("Motion|Advanced setting for root motion and blending pivot");
      public GUIContent MotionNode = EditorGUIUtility.TextContent("Root Motion Node|Define a transform node that will be used to create root motion curves");
      public GUIContent ImportMessages = EditorGUIUtility.TextContent("Import Messages");
      public GUIContent GenerateRetargetingWarnings = EditorGUIUtility.TextContent("Generate Retargeting Quality Report");
      public GUIContent Mask = EditorGUIUtility.TextContent("Mask|Configure the mask for this clip to remove unnecessary curves.");

      public Styles()
      {
        this.numberStyle.alignment = TextAnchor.UpperRight;
      }
    }
  }
}
