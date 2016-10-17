// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMuscleEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AvatarMuscleEditor : AvatarSubEditor
  {
    protected int[][] m_Muscles = new int[8][]{ new int[6]{ 0, 1, 2, 3, 4, 5 }, new int[12]{ 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }, new int[9]{ 34, 35, 36, 37, 38, 39, 40, 41, 42 }, new int[20]{ 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71 }, new int[9]{ 43, 44, 45, 46, 47, 48, 49, 50, 51 }, new int[20]{ 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91 }, new int[8]{ 18, 19, 20, 21, 22, 23, 24, 25 }, new int[8]{ 26, 27, 28, 29, 30, 31, 32, 33 } };
    protected int[][] m_MasterMuscle = new int[7][]{ new int[18]{ 0, 3, 6, 9, 18, 21, 23, 26, 29, 31, 34, 36, 39, 41, 43, 45, 48, 50 }, new int[4]{ 1, 4, 7, 10 }, new int[4]{ 2, 5, 8, 11 }, new int[10]{ 19, 24, 27, 32, 35, 37, 42, 44, 46, 51 }, new int[8]{ 20, 22, 28, 30, 38, 40, 47, 49 }, new int[30]{ 52, 54, 55, 56, 58, 59, 60, 62, 63, 64, 66, 67, 68, 70, 71, 72, 74, 75, 76, 78, 79, 80, 82, 83, 84, 86, 87, 88, 90, 91 }, new int[10]{ 53, 57, 61, 65, 69, 73, 77, 81, 85, 89 } };
    private const string sMinX = "m_Limit.m_Min.x";
    private const string sMinY = "m_Limit.m_Min.y";
    private const string sMinZ = "m_Limit.m_Min.z";
    private const string sMaxX = "m_Limit.m_Max.x";
    private const string sMaxY = "m_Limit.m_Max.y";
    private const string sMaxZ = "m_Limit.m_Max.z";
    private const string sModified = "m_Limit.m_Modified";
    private const string sArmTwist = "m_HumanDescription.m_ArmTwist";
    private const string sForeArmTwist = "m_HumanDescription.m_ForeArmTwist";
    private const string sUpperLegTwist = "m_HumanDescription.m_UpperLegTwist";
    private const string sLegTwist = "m_HumanDescription.m_LegTwist";
    private const string sArmStretch = "m_HumanDescription.m_ArmStretch";
    private const string sLegStretch = "m_HumanDescription.m_LegStretch";
    private const string sFeetSpacing = "m_HumanDescription.m_FeetSpacing";
    private const string sHasTranslationDoF = "m_HumanDescription.m_HasTranslationDoF";
    private const float sMuscleMin = -180f;
    private const float sMuscleMax = 180f;
    private const float kPreviewWidth = 80f;
    private const float kNumberWidth = 38f;
    private const float kLineHeight = 16f;
    private static AvatarMuscleEditor.Styles s_Styles;
    private bool[] m_MuscleBodyGroupToggle;
    private bool[] m_MuscleToggle;
    private int m_FocusedMuscle;
    [SerializeField]
    private float[] m_MuscleValue;
    [SerializeField]
    private float[] m_MuscleMasterValue;
    [SerializeField]
    protected float m_ArmTwistFactor;
    [SerializeField]
    protected float m_ForeArmTwistFactor;
    [SerializeField]
    protected float m_UpperLegTwistFactor;
    [SerializeField]
    protected float m_LegTwistFactor;
    [SerializeField]
    protected float m_ArmStretchFactor;
    [SerializeField]
    protected float m_LegStretchFactor;
    [SerializeField]
    protected float m_FeetSpacingFactor;
    [SerializeField]
    protected bool m_HasTranslationDoF;
    private string[] m_MuscleName;
    private int m_MuscleCount;
    private SerializedProperty[] m_MuscleMin;
    private SerializedProperty[] m_MuscleMax;
    [SerializeField]
    private float[] m_MuscleMinEdit;
    [SerializeField]
    private float[] m_MuscleMaxEdit;
    private SerializedProperty[] m_Modified;
    private SerializedProperty m_ArmTwistProperty;
    private SerializedProperty m_ForeArmTwistProperty;
    private SerializedProperty m_UpperLegTwistProperty;
    private SerializedProperty m_LegTwistProperty;
    private SerializedProperty m_ArmStretchProperty;
    private SerializedProperty m_LegStretchProperty;
    private SerializedProperty m_FeetSpacingProperty;
    private SerializedProperty m_HasTranslationDoFProperty;
    protected AvatarSetupTool.BoneWrapper[] m_Bones;

    private static AvatarMuscleEditor.Styles styles
    {
      get
      {
        if (AvatarMuscleEditor.s_Styles == null)
          AvatarMuscleEditor.s_Styles = new AvatarMuscleEditor.Styles();
        return AvatarMuscleEditor.s_Styles;
      }
    }

    private static Rect GetSettingsRect(Rect wholeWidthRect)
    {
      wholeWidthRect.xMin += 83f;
      wholeWidthRect.width -= 4f;
      return wholeWidthRect;
    }

    private static Rect GetSettingsRect()
    {
      return AvatarMuscleEditor.GetSettingsRect(GUILayoutUtility.GetRect(10f, 16f));
    }

    private static Rect GetPreviewRect(Rect wholeWidthRect)
    {
      wholeWidthRect.width = 71f;
      wholeWidthRect.x += 5f;
      wholeWidthRect.height = 16f;
      return wholeWidthRect;
    }

    private void HeaderGUI(string h1, string h2)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(h1, AvatarMuscleEditor.styles.title, new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUILayout.Label(h2, AvatarMuscleEditor.styles.title, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      GUILayout.EndHorizontal();
    }

    private static float PreviewSlider(Rect position, float val)
    {
      val = GUI.HorizontalSlider(AvatarMuscleEditor.GetPreviewRect(position), val, -1f, 1f);
      if ((double) val > -0.100000001490116 && (double) val < 0.100000001490116)
        val = 0.0f;
      return val;
    }

    internal void ResetValuesFromProperties()
    {
      this.m_ArmTwistFactor = this.m_ArmTwistProperty.floatValue;
      this.m_ForeArmTwistFactor = this.m_ForeArmTwistProperty.floatValue;
      this.m_UpperLegTwistFactor = this.m_UpperLegTwistProperty.floatValue;
      this.m_LegTwistFactor = this.m_LegTwistProperty.floatValue;
      this.m_ArmStretchFactor = this.m_ArmStretchProperty.floatValue;
      this.m_LegStretchFactor = this.m_LegStretchProperty.floatValue;
      this.m_FeetSpacingFactor = this.m_FeetSpacingProperty.floatValue;
      this.m_HasTranslationDoF = this.m_HasTranslationDoFProperty.boolValue;
      for (int i1 = 0; i1 < this.m_Bones.Length; ++i1)
      {
        if (this.m_Modified[i1] != null)
        {
          bool boolValue = this.m_Modified[i1].boolValue;
          int i2 = HumanTrait.MuscleFromBone(i1, 0);
          int i3 = HumanTrait.MuscleFromBone(i1, 1);
          int i4 = HumanTrait.MuscleFromBone(i1, 2);
          if (i2 != -1)
          {
            this.m_MuscleMinEdit[i2] = !boolValue ? HumanTrait.GetMuscleDefaultMin(i2) : this.m_MuscleMin[i2].floatValue;
            this.m_MuscleMaxEdit[i2] = !boolValue ? HumanTrait.GetMuscleDefaultMax(i2) : this.m_MuscleMax[i2].floatValue;
          }
          if (i3 != -1)
          {
            this.m_MuscleMinEdit[i3] = !boolValue ? HumanTrait.GetMuscleDefaultMin(i3) : this.m_MuscleMin[i3].floatValue;
            this.m_MuscleMaxEdit[i3] = !boolValue ? HumanTrait.GetMuscleDefaultMax(i3) : this.m_MuscleMax[i3].floatValue;
          }
          if (i4 != -1)
          {
            this.m_MuscleMinEdit[i4] = !boolValue ? HumanTrait.GetMuscleDefaultMin(i4) : this.m_MuscleMin[i4].floatValue;
            this.m_MuscleMaxEdit[i4] = !boolValue ? HumanTrait.GetMuscleDefaultMax(i4) : this.m_MuscleMax[i4].floatValue;
          }
        }
      }
    }

    internal void InitializeProperties()
    {
      this.m_ArmTwistProperty = this.serializedObject.FindProperty("m_HumanDescription.m_ArmTwist");
      this.m_ForeArmTwistProperty = this.serializedObject.FindProperty("m_HumanDescription.m_ForeArmTwist");
      this.m_UpperLegTwistProperty = this.serializedObject.FindProperty("m_HumanDescription.m_UpperLegTwist");
      this.m_LegTwistProperty = this.serializedObject.FindProperty("m_HumanDescription.m_LegTwist");
      this.m_ArmStretchProperty = this.serializedObject.FindProperty("m_HumanDescription.m_ArmStretch");
      this.m_LegStretchProperty = this.serializedObject.FindProperty("m_HumanDescription.m_LegStretch");
      this.m_FeetSpacingProperty = this.serializedObject.FindProperty("m_HumanDescription.m_FeetSpacing");
      this.m_HasTranslationDoFProperty = this.serializedObject.FindProperty("m_HumanDescription.m_HasTranslationDoF");
      for (int i = 0; i < this.m_Bones.Length; ++i)
      {
        SerializedProperty serializedProperty = this.m_Bones[i].GetSerializedProperty(this.serializedObject, false);
        if (serializedProperty != null)
        {
          this.m_Modified[i] = serializedProperty.FindPropertyRelative("m_Limit.m_Modified");
          int index1 = HumanTrait.MuscleFromBone(i, 0);
          int index2 = HumanTrait.MuscleFromBone(i, 1);
          int index3 = HumanTrait.MuscleFromBone(i, 2);
          if (index1 != -1)
          {
            this.m_MuscleMin[index1] = serializedProperty.FindPropertyRelative("m_Limit.m_Min.x");
            this.m_MuscleMax[index1] = serializedProperty.FindPropertyRelative("m_Limit.m_Max.x");
          }
          if (index2 != -1)
          {
            this.m_MuscleMin[index2] = serializedProperty.FindPropertyRelative("m_Limit.m_Min.y");
            this.m_MuscleMax[index2] = serializedProperty.FindPropertyRelative("m_Limit.m_Max.y");
          }
          if (index3 != -1)
          {
            this.m_MuscleMin[index3] = serializedProperty.FindPropertyRelative("m_Limit.m_Min.z");
            this.m_MuscleMax[index3] = serializedProperty.FindPropertyRelative("m_Limit.m_Max.z");
          }
        }
      }
    }

    internal void Initialize()
    {
      if (this.m_Bones == null)
        this.m_Bones = AvatarSetupTool.GetHumanBones(this.serializedObject, this.modelBones);
      this.m_FocusedMuscle = -1;
      this.m_MuscleBodyGroupToggle = new bool[this.m_Muscles.Length];
      for (int index = 0; index < this.m_Muscles.Length; ++index)
        this.m_MuscleBodyGroupToggle[index] = false;
      this.m_MuscleName = HumanTrait.MuscleName;
      for (int index = 0; index < this.m_MuscleName.Length; ++index)
      {
        if (this.m_MuscleName[index].StartsWith("Right"))
          this.m_MuscleName[index] = this.m_MuscleName[index].Substring(5).Trim();
        if (this.m_MuscleName[index].StartsWith("Left"))
          this.m_MuscleName[index] = this.m_MuscleName[index].Substring(4).Trim();
      }
      this.m_MuscleCount = HumanTrait.MuscleCount;
      this.m_MuscleToggle = new bool[this.m_MuscleCount];
      this.m_MuscleValue = new float[this.m_MuscleCount];
      this.m_MuscleMin = new SerializedProperty[this.m_MuscleCount];
      this.m_MuscleMax = new SerializedProperty[this.m_MuscleCount];
      this.m_MuscleMinEdit = new float[this.m_MuscleCount];
      this.m_MuscleMaxEdit = new float[this.m_MuscleCount];
      for (int index = 0; index < this.m_MuscleCount; ++index)
      {
        this.m_MuscleToggle[index] = false;
        this.m_MuscleValue[index] = 0.0f;
        this.m_MuscleMin[index] = (SerializedProperty) null;
        this.m_MuscleMin[index] = (SerializedProperty) null;
      }
      this.m_Modified = new SerializedProperty[this.m_Bones.Length];
      for (int index = 0; index < this.m_Bones.Length; ++index)
        this.m_Modified[index] = (SerializedProperty) null;
      this.InitializeProperties();
      this.ResetValuesFromProperties();
      this.m_MuscleMasterValue = new float[this.m_MasterMuscle.Length];
      for (int index = 0; index < this.m_MasterMuscle.Length; ++index)
        this.m_MuscleMasterValue[index] = 0.0f;
    }

    public override void Enable(AvatarEditor inspector)
    {
      base.Enable(inspector);
      this.Initialize();
      this.WritePose();
    }

    public override void OnInspectorGUI()
    {
      if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")
        this.WritePose();
      EditorGUI.BeginDisabledGroup(!this.avatarAsset.isHuman);
      EditorGUIUtility.labelWidth = 110f;
      EditorGUIUtility.fieldWidth = 40f;
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      this.MuscleGroupGUI();
      EditorGUILayout.Space();
      this.MuscleGUI();
      EditorGUILayout.Space();
      this.PropertiesGUI();
      GUILayout.EndVertical();
      GUILayout.Space(1f);
      GUILayout.EndHorizontal();
      this.DisplayMuscleButtons();
      this.ApplyRevertGUI();
      EditorGUI.EndDisabledGroup();
    }

    protected void DisplayMuscleButtons()
    {
      GUILayout.BeginHorizontal(string.Empty, AvatarMuscleEditor.styles.toolbar, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      Rect rect = GUILayoutUtility.GetRect(AvatarMuscleEditor.styles.muscle, AvatarMuscleEditor.styles.toolbarDropDown);
      if (GUI.Button(rect, AvatarMuscleEditor.styles.muscle, AvatarMuscleEditor.styles.toolbarDropDown))
      {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(AvatarMuscleEditor.styles.resetMuscle, false, new GenericMenu.MenuFunction(this.ResetMuscleToDefault));
        genericMenu.DropDown(rect);
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
    }

    protected override void ResetValues()
    {
      this.serializedObject.Update();
      this.ResetValuesFromProperties();
    }

    protected void ResetMuscleToDefault()
    {
      Avatar avatar = (Avatar) null;
      if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
        avatar = (this.gameObject.GetComponent(typeof (Animator)) as Animator).avatar;
      for (int index1 = 0; index1 < this.m_MuscleCount; ++index1)
      {
        float muscleDefaultMin = HumanTrait.GetMuscleDefaultMin(index1);
        float muscleDefaultMax = HumanTrait.GetMuscleDefaultMax(index1);
        if (this.m_MuscleMin[index1] != null && this.m_MuscleMax[index1] != null)
        {
          this.m_MuscleMin[index1].floatValue = this.m_MuscleMinEdit[index1] = muscleDefaultMin;
          this.m_MuscleMax[index1].floatValue = this.m_MuscleMaxEdit[index1] = muscleDefaultMax;
        }
        int index2 = HumanTrait.BoneFromMuscle(index1);
        if (this.m_Modified[index2] != null && index2 != -1)
          this.m_Modified[index2].boolValue = false;
        if ((UnityEngine.Object) avatar != (UnityEngine.Object) null)
          avatar.SetMuscleMinMax(index1, muscleDefaultMin, muscleDefaultMax);
      }
      this.WritePose();
    }

    protected void UpdateAvatarParameter(HumanParameter parameterId, float value)
    {
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      (this.gameObject.GetComponent(typeof (Animator)) as Animator).avatar.SetParameter((int) parameterId, value);
    }

    protected bool UpdateMuscle(int muscleId, float min, float max)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Updated muscle range");
      this.m_MuscleMin[muscleId].floatValue = min;
      this.m_MuscleMax[muscleId].floatValue = max;
      int index = HumanTrait.BoneFromMuscle(muscleId);
      if (index != -1)
        this.m_Modified[index].boolValue = true;
      this.m_FocusedMuscle = muscleId;
      if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
        (this.gameObject.GetComponent(typeof (Animator)) as Animator).avatar.SetMuscleMinMax(muscleId, min, max);
      SceneView.RepaintAll();
      return (UnityEngine.Object) this.gameObject != (UnityEngine.Object) null;
    }

    protected void MuscleGroupGUI()
    {
      bool flag = false;
      this.HeaderGUI("Preview", "Muscle Group Preview");
      GUILayout.BeginVertical(AvatarMuscleEditor.styles.box, new GUILayoutOption[0]);
      Rect rect1 = GUILayoutUtility.GetRect(10f, 16f);
      Rect settingsRect1 = AvatarMuscleEditor.GetSettingsRect(rect1);
      if (GUI.Button(AvatarMuscleEditor.GetPreviewRect(rect1), "Reset All", EditorStyles.miniButton))
      {
        for (int index = 0; index < this.m_MuscleMasterValue.Length; ++index)
          this.m_MuscleMasterValue[index] = 0.0f;
        for (int index = 0; index < this.m_MuscleValue.Length; ++index)
          this.m_MuscleValue[index] = 0.0f;
        flag = true;
      }
      GUI.Label(settingsRect1, "Reset All Preview Values", EditorStyles.label);
      for (int index1 = 0; index1 < this.m_MasterMuscle.Length; ++index1)
      {
        Rect rect2 = GUILayoutUtility.GetRect(10f, 16f);
        Rect settingsRect2 = AvatarMuscleEditor.GetSettingsRect(rect2);
        float num = this.m_MuscleMasterValue[index1];
        this.m_MuscleMasterValue[index1] = AvatarMuscleEditor.PreviewSlider(rect2, this.m_MuscleMasterValue[index1]);
        if ((double) this.m_MuscleMasterValue[index1] != (double) num)
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Muscle preview");
          for (int index2 = 0; index2 < this.m_MasterMuscle[index1].Length; ++index2)
          {
            if (this.m_MasterMuscle[index1][index2] != -1)
              this.m_MuscleValue[this.m_MasterMuscle[index1][index2]] = this.m_MuscleMasterValue[index1];
          }
        }
        flag = ((flag ? 1 : 0) | ((double) this.m_MuscleMasterValue[index1] == (double) num ? 0 : ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null ? 1 : 0))) != 0;
        GUI.Label(settingsRect2, AvatarMuscleEditor.styles.muscleTypeGroup[index1], EditorStyles.label);
      }
      GUILayout.EndVertical();
      if (!flag)
        return;
      this.WritePose();
    }

    protected void MuscleGUI()
    {
      bool flag1 = false;
      this.HeaderGUI("Preview", "Per-Muscle Settings");
      GUILayout.BeginVertical(AvatarMuscleEditor.styles.box, new GUILayoutOption[0]);
      for (int index1 = 0; index1 < this.m_MuscleBodyGroupToggle.Length; ++index1)
      {
        Rect settingsRect = AvatarMuscleEditor.GetSettingsRect(GUILayoutUtility.GetRect(10f, 16f));
        this.m_MuscleBodyGroupToggle[index1] = GUI.Toggle(settingsRect, this.m_MuscleBodyGroupToggle[index1], AvatarMuscleEditor.styles.muscleBodyGroup[index1], EditorStyles.foldout);
        if (this.m_MuscleBodyGroupToggle[index1])
        {
          for (int index2 = 0; index2 < this.m_Muscles[index1].Length; ++index2)
          {
            int muscleId = this.m_Muscles[index1][index2];
            if (muscleId != -1 && this.m_MuscleMin[muscleId] != null && this.m_MuscleMax[muscleId] != null)
            {
              bool flag2 = this.m_MuscleToggle[muscleId];
              Rect rect = GUILayoutUtility.GetRect(10f, !flag2 ? 16f : 32f);
              settingsRect = AvatarMuscleEditor.GetSettingsRect(rect);
              settingsRect.xMin += 15f;
              settingsRect.height = 16f;
              this.m_MuscleToggle[muscleId] = GUI.Toggle(settingsRect, this.m_MuscleToggle[muscleId], this.m_MuscleName[muscleId], EditorStyles.foldout);
              float num = AvatarMuscleEditor.PreviewSlider(rect, this.m_MuscleValue[muscleId]);
              if ((double) this.m_MuscleValue[muscleId] != (double) num)
              {
                Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Muscle preview");
                this.m_FocusedMuscle = muscleId;
                this.m_MuscleValue[muscleId] = num;
                flag1 |= (UnityEngine.Object) this.gameObject != (UnityEngine.Object) null;
              }
              if (flag2)
              {
                bool flag3 = false;
                settingsRect.xMin += 15f;
                settingsRect.y += 16f;
                Rect position1 = settingsRect;
                if ((double) settingsRect.width > 160.0)
                {
                  Rect position2 = settingsRect;
                  position2.width = 38f;
                  EditorGUI.BeginChangeCheck();
                  this.m_MuscleMinEdit[muscleId] = EditorGUI.FloatField(position2, this.m_MuscleMinEdit[muscleId]);
                  bool flag4 = flag3 | EditorGUI.EndChangeCheck();
                  position2.x = settingsRect.xMax - 38f;
                  EditorGUI.BeginChangeCheck();
                  this.m_MuscleMaxEdit[muscleId] = EditorGUI.FloatField(position2, this.m_MuscleMaxEdit[muscleId]);
                  flag3 = flag4 | EditorGUI.EndChangeCheck();
                  position1.xMin += 43f;
                  position1.xMax -= 43f;
                }
                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(position1, ref this.m_MuscleMinEdit[muscleId], ref this.m_MuscleMaxEdit[muscleId], -180f, 180f);
                if (flag3 | EditorGUI.EndChangeCheck())
                {
                  this.m_MuscleMinEdit[muscleId] = Mathf.Clamp(this.m_MuscleMinEdit[muscleId], -180f, 0.0f);
                  this.m_MuscleMaxEdit[muscleId] = Mathf.Clamp(this.m_MuscleMaxEdit[muscleId], 0.0f, 180f);
                  flag1 |= this.UpdateMuscle(muscleId, this.m_MuscleMinEdit[muscleId], this.m_MuscleMaxEdit[muscleId]);
                }
              }
            }
          }
        }
      }
      GUILayout.EndVertical();
      if (!flag1)
        return;
      this.WritePose();
    }

    protected void PropertiesGUI()
    {
      bool flag = false;
      this.HeaderGUI(string.Empty, "Additional Settings");
      GUILayout.BeginVertical(AvatarMuscleEditor.styles.box, new GUILayoutOption[0]);
      this.m_ArmTwistFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.armTwist, this.m_ArmTwistFactor, 0.0f, 1f);
      if ((double) this.m_ArmTwistProperty.floatValue != (double) this.m_ArmTwistFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Upper arm twist");
        this.m_ArmTwistProperty.floatValue = this.m_ArmTwistFactor;
        this.UpdateAvatarParameter(HumanParameter.UpperArmTwist, this.m_ArmTwistFactor);
        flag = true;
      }
      this.m_ForeArmTwistFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.foreArmTwist, this.m_ForeArmTwistFactor, 0.0f, 1f);
      if ((double) this.m_ForeArmTwistProperty.floatValue != (double) this.m_ForeArmTwistFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Lower arm twist");
        this.m_ForeArmTwistProperty.floatValue = this.m_ForeArmTwistFactor;
        this.UpdateAvatarParameter(HumanParameter.LowerArmTwist, this.m_ForeArmTwistFactor);
        flag = true;
      }
      this.m_UpperLegTwistFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.upperLegTwist, this.m_UpperLegTwistFactor, 0.0f, 1f);
      if ((double) this.m_UpperLegTwistProperty.floatValue != (double) this.m_UpperLegTwistFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Upper leg twist");
        this.m_UpperLegTwistProperty.floatValue = this.m_UpperLegTwistFactor;
        this.UpdateAvatarParameter(HumanParameter.UpperLegTwist, this.m_UpperLegTwistFactor);
        flag = true;
      }
      this.m_LegTwistFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.legTwist, this.m_LegTwistFactor, 0.0f, 1f);
      if ((double) this.m_LegTwistProperty.floatValue != (double) this.m_LegTwistFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Lower leg twist");
        this.m_LegTwistProperty.floatValue = this.m_LegTwistFactor;
        this.UpdateAvatarParameter(HumanParameter.LowerLegTwist, this.m_LegTwistFactor);
        flag = true;
      }
      this.m_ArmStretchFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.armStretch, this.m_ArmStretchFactor, 0.0f, 1f);
      if ((double) this.m_ArmStretchProperty.floatValue != (double) this.m_ArmStretchFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Arm stretch");
        this.m_ArmStretchProperty.floatValue = this.m_ArmStretchFactor;
        this.UpdateAvatarParameter(HumanParameter.ArmStretch, this.m_ArmStretchFactor);
        flag = true;
      }
      this.m_LegStretchFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.legStretch, this.m_LegStretchFactor, 0.0f, 1f);
      if ((double) this.m_LegStretchProperty.floatValue != (double) this.m_LegStretchFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Leg stretch");
        this.m_LegStretchProperty.floatValue = this.m_LegStretchFactor;
        this.UpdateAvatarParameter(HumanParameter.LegStretch, this.m_LegStretchFactor);
        flag = true;
      }
      this.m_FeetSpacingFactor = EditorGUI.Slider(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.feetSpacing, this.m_FeetSpacingFactor, 0.0f, 1f);
      if ((double) this.m_FeetSpacingProperty.floatValue != (double) this.m_FeetSpacingFactor)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Feet spacing");
        this.m_FeetSpacingProperty.floatValue = this.m_FeetSpacingFactor;
        this.UpdateAvatarParameter(HumanParameter.FeetSpacing, this.m_FeetSpacingFactor);
        flag = true;
      }
      this.m_HasTranslationDoF = EditorGUI.Toggle(AvatarMuscleEditor.GetSettingsRect(), AvatarMuscleEditor.styles.hasTranslationDoF, this.m_HasTranslationDoF);
      if (this.m_HasTranslationDoFProperty.boolValue != this.m_HasTranslationDoF)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, "Translation DoF");
        this.m_HasTranslationDoFProperty.boolValue = this.m_HasTranslationDoF;
      }
      GUILayout.EndVertical();
      if (!flag)
        return;
      this.WritePose();
    }

    protected void WritePose()
    {
      if (!(bool) ((UnityEngine.Object) this.gameObject))
        return;
      Animator component = this.gameObject.GetComponent(typeof (Animator)) as Animator;
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      Avatar avatar = component.avatar;
      if (!((UnityEngine.Object) avatar != (UnityEngine.Object) null) || !avatar.isValid || !avatar.isHuman)
        return;
      AvatarUtility.SetHumanPose(component, this.m_MuscleValue);
      SceneView.RepaintAll();
    }

    public void DrawMuscleHandle(Transform t, int humanId)
    {
      Avatar avatar = (this.gameObject.GetComponent(typeof (Animator)) as Animator).avatar;
      int index1 = HumanTrait.MuscleFromBone(humanId, 0);
      int index2 = HumanTrait.MuscleFromBone(humanId, 1);
      int index3 = HumanTrait.MuscleFromBone(humanId, 2);
      float axisLength = avatar.GetAxisLength(humanId);
      Quaternion preRotation = avatar.GetPreRotation(humanId);
      Quaternion postRotation = avatar.GetPostRotation(humanId);
      Quaternion quaternion1 = t.parent.rotation * preRotation;
      Quaternion quaternion2 = t.rotation * postRotation;
      Color color = new Color(1f, 1f, 1f, 0.5f);
      Quaternion zyRoll = avatar.GetZYRoll(humanId, Vector3.zero);
      Vector3 limitSign = avatar.GetLimitSign(humanId);
      Vector3 vector3_1 = quaternion2 * Vector3.right;
      Vector3 p2 = t.position + vector3_1 * axisLength;
      Handles.color = Color.white;
      Handles.DrawLine(t.position, p2);
      if (index1 != -1)
      {
        Quaternion zyPostQ = avatar.GetZYPostQ(humanId, t.parent.rotation, t.rotation);
        float angle = this.m_MuscleMinEdit[index1];
        float num = this.m_MuscleMaxEdit[index1];
        Vector3 vector3_2 = quaternion2 * Vector3.right;
        Vector3 vector3_3 = zyPostQ * Vector3.forward;
        Handles.color = Color.black;
        Vector3 vector3_4 = t.position + vector3_2 * axisLength * 0.75f;
        Vector3 vector3_5 = quaternion2 * Vector3.right * limitSign.x;
        Vector3 from = Quaternion.AngleAxis(angle, vector3_5) * vector3_3;
        Handles.color = Color.yellow;
        Handles.color = Handles.xAxisColor * color;
        Handles.DrawSolidArc(vector3_4, vector3_5, from, num - angle, axisLength * 0.25f);
        Vector3 vector3_6 = quaternion2 * Vector3.forward;
        Handles.color = Handles.centerColor;
        Handles.DrawLine(vector3_4, vector3_4 + vector3_6 * axisLength * 0.25f);
      }
      if (index2 != -1)
      {
        float angle = this.m_MuscleMinEdit[index2];
        float num = this.m_MuscleMaxEdit[index2];
        Vector3 vector3_2 = quaternion1 * Vector3.up * limitSign.y;
        Vector3 vector3_3 = quaternion1 * zyRoll * Vector3.right;
        Handles.color = Color.black;
        Vector3 from = Quaternion.AngleAxis(angle, vector3_2) * vector3_3;
        Handles.color = Color.yellow;
        Handles.color = Handles.yAxisColor * color;
        Handles.DrawSolidArc(t.position, vector3_2, from, num - angle, axisLength * 0.25f);
      }
      if (index3 == -1)
        return;
      float angle1 = this.m_MuscleMinEdit[index3];
      float num1 = this.m_MuscleMaxEdit[index3];
      Vector3 vector3_7 = quaternion1 * Vector3.forward * limitSign.z;
      Vector3 vector3_8 = quaternion1 * zyRoll * Vector3.right;
      Handles.color = Color.black;
      Vector3 from1 = Quaternion.AngleAxis(angle1, vector3_7) * vector3_8;
      Handles.color = Color.yellow;
      Handles.color = Handles.zAxisColor * color;
      Handles.DrawSolidArc(t.position, vector3_7, from1, num1 - angle1, axisLength * 0.25f);
    }

    public override void OnSceneGUI()
    {
      AvatarSkeletonDrawer.DrawSkeleton(this.root, this.modelBones);
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || (this.m_FocusedMuscle == -1 || (UnityEngine.Object) (this.gameObject.GetComponent(typeof (Animator)) as Animator) == (UnityEngine.Object) null))
        return;
      int humanId = HumanTrait.BoneFromMuscle(this.m_FocusedMuscle);
      if (humanId == -1)
        return;
      this.DrawMuscleHandle(this.m_Bones[humanId].bone, humanId);
    }

    private class Styles
    {
      public GUIContent[] muscleBodyGroup = new GUIContent[8]{ EditorGUIUtility.TextContent("Body"), EditorGUIUtility.TextContent("Head"), EditorGUIUtility.TextContent("Left Arm"), EditorGUIUtility.TextContent("Left Fingers"), EditorGUIUtility.TextContent("Right Arm"), EditorGUIUtility.TextContent("Right Fingers"), EditorGUIUtility.TextContent("Left Leg"), EditorGUIUtility.TextContent("Right Leg") };
      public GUIContent[] muscleTypeGroup = new GUIContent[7]{ EditorGUIUtility.TextContent("Open Close"), EditorGUIUtility.TextContent("Left Right"), EditorGUIUtility.TextContent("Roll Left Right"), EditorGUIUtility.TextContent("In Out"), EditorGUIUtility.TextContent("Roll In Out"), EditorGUIUtility.TextContent("Finger Open Close"), EditorGUIUtility.TextContent("Finger In Out") };
      public GUIContent armTwist = EditorGUIUtility.TextContent("Upper Arm Twist");
      public GUIContent foreArmTwist = EditorGUIUtility.TextContent("Lower Arm Twist");
      public GUIContent upperLegTwist = EditorGUIUtility.TextContent("Upper Leg Twist");
      public GUIContent legTwist = EditorGUIUtility.TextContent("Lower Leg Twist");
      public GUIContent armStretch = EditorGUIUtility.TextContent("Arm Stretch");
      public GUIContent legStretch = EditorGUIUtility.TextContent("Leg Stretch");
      public GUIContent feetSpacing = EditorGUIUtility.TextContent("Feet Spacing");
      public GUIContent hasTranslationDoF = EditorGUIUtility.TextContent("Translation DoF");
      public GUIStyle box = new GUIStyle((GUIStyle) "OL box noexpand");
      public GUIStyle title = new GUIStyle((GUIStyle) "OL TITLE");
      public GUIStyle toolbar = (GUIStyle) "TE Toolbar";
      public GUIStyle toolbarDropDown = (GUIStyle) "TE ToolbarDropDown";
      public GUIContent muscle = EditorGUIUtility.TextContent("Muscles");
      public GUIContent resetMuscle = EditorGUIUtility.TextContent("Reset");

      public Styles()
      {
        this.box.padding = new RectOffset(0, 0, 4, 4);
      }
    }
  }
}
