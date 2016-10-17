// Decompiled with JetBrains decompiler
// Type: UnityEditor.SubModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SubModuleUI : ModuleUI
  {
    private int m_CheckObjectTypeIndex = -1;
    private int m_CheckObjectIndex = -1;
    private const int k_MaxSubPerType = 2;
    private SerializedProperty[,] m_SubEmitters;
    private static SubModuleUI.Texts s_Texts;

    public SubModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SubModule", displayName)
    {
      this.m_ToolTip = "Sub emission of particles. This allows each particle to emit particles in another system.";
      this.Init();
    }

    protected override void Init()
    {
      if (this.m_SubEmitters != null)
        return;
      this.m_SubEmitters = new SerializedProperty[3, 2];
      this.m_SubEmitters[0, 0] = this.GetProperty("subEmitterBirth");
      this.m_SubEmitters[0, 1] = this.GetProperty("subEmitterBirth1");
      this.m_SubEmitters[1, 0] = this.GetProperty("subEmitterCollision");
      this.m_SubEmitters[1, 1] = this.GetProperty("subEmitterCollision1");
      this.m_SubEmitters[2, 0] = this.GetProperty("subEmitterDeath");
      this.m_SubEmitters[2, 1] = this.GetProperty("subEmitterDeath1");
    }

    public override void Validate()
    {
    }

    private void CreateAndAssignSubEmitter(SerializedProperty objectRefProp, SubModuleUI.SubEmitterType type)
    {
      GameObject particleSystem = this.m_ParticleSystemUI.m_ParticleEffectUI.CreateParticleSystem(this.m_ParticleSystemUI.m_ParticleSystem, type);
      particleSystem.name = "SubEmitter";
      objectRefProp.objectReferenceValue = (Object) particleSystem.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
      if (this.m_CheckObjectIndex < 0 || this.m_CheckObjectTypeIndex < 0 || ObjectSelector.isVisible)
        return;
      Object objectReferenceValue = this.m_SubEmitters[this.m_CheckObjectTypeIndex, this.m_CheckObjectIndex].objectReferenceValue;
      ParticleSystem subEmitter = objectReferenceValue as ParticleSystem;
      if ((Object) subEmitter != (Object) null)
      {
        bool flag = true;
        if (this.ValidateSubemitter(subEmitter))
        {
          string str = ParticleSystemEditorUtils.CheckCircularReferences(subEmitter);
          if (str.Length == 0)
          {
            this.CheckIfChild(objectReferenceValue);
          }
          else
          {
            EditorUtility.DisplayDialog("Circular References Detected", string.Format("'{0}' could not be assigned as subemitter on '{1}' due to circular referencing!\nBacktrace: {2} \n\nReference will be removed.", (object) subEmitter.gameObject.name, (object) this.m_ParticleSystemUI.m_ParticleSystem.gameObject.name, (object) str), "Ok");
            flag = false;
          }
        }
        else
          flag = false;
        if (!flag)
        {
          this.m_SubEmitters[this.m_CheckObjectTypeIndex, this.m_CheckObjectIndex].objectReferenceValue = (Object) null;
          this.m_ParticleSystemUI.ApplyProperties();
          this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
        }
      }
      this.m_CheckObjectIndex = -1;
      this.m_CheckObjectTypeIndex = -1;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
    }

    internal static bool IsChild(ParticleSystem subEmitter, ParticleSystem root)
    {
      if ((Object) subEmitter == (Object) null || (Object) root == (Object) null)
        return false;
      return (Object) ParticleSystemEditorUtils.GetRoot(subEmitter) == (Object) root;
    }

    private bool ValidateSubemitter(ParticleSystem subEmitter)
    {
      if ((Object) subEmitter == (Object) null)
        return false;
      ParticleSystem root = this.m_ParticleSystemUI.m_ParticleEffectUI.GetRoot();
      if (root.gameObject.activeInHierarchy && !subEmitter.gameObject.activeInHierarchy)
      {
        EditorUtility.DisplayDialog("Invalid Sub Emitter", string.Format("The assigned sub emitter is part of a prefab and can therefore not be assigned."), "Ok");
        return false;
      }
      if (root.gameObject.activeInHierarchy || !subEmitter.gameObject.activeInHierarchy)
        return true;
      EditorUtility.DisplayDialog("Invalid Sub Emitter", string.Format("The assigned sub emitter is part of a scene object and can therefore not be assigned to a prefab."), "Ok");
      return false;
    }

    private void CheckIfChild(Object subEmitter)
    {
      if (!(subEmitter != (Object) null))
        return;
      ParticleSystem root = this.m_ParticleSystemUI.m_ParticleEffectUI.GetRoot();
      if (SubModuleUI.IsChild(subEmitter as ParticleSystem, root) || !EditorUtility.DisplayDialog("Reparent GameObjects", string.Format("The assigned sub emitter is not a child of the current root particle system GameObject: '{0}' and is therefore NOT considered a part of the current effect. Do you want to reparent it?", (object) root.gameObject.name), "Yes, Reparent", "No"))
        return;
      if (EditorUtility.IsPersistent(subEmitter))
      {
        GameObject gameObject = Object.Instantiate(subEmitter) as GameObject;
        if (!((Object) gameObject != (Object) null))
          return;
        gameObject.transform.parent = this.m_ParticleSystemUI.m_ParticleSystem.transform;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
      }
      else
      {
        ParticleSystem particleSystem = subEmitter as ParticleSystem;
        if (!(bool) ((Object) particleSystem))
          return;
        Undo.SetTransformParent(this.m_ParticleSystemUI.m_ParticleSystem.transform, particleSystem.gameObject.transform.transform, "Reparent sub emitter");
      }
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (SubModuleUI.s_Texts == null)
        SubModuleUI.s_Texts = new SubModuleUI.Texts();
      Object[,] objectArray1 = new Object[3, 2]{ { this.m_SubEmitters[0, 0].objectReferenceValue, this.m_SubEmitters[0, 1].objectReferenceValue }, { this.m_SubEmitters[1, 0].objectReferenceValue, this.m_SubEmitters[1, 1].objectReferenceValue }, { this.m_SubEmitters[2, 0].objectReferenceValue, this.m_SubEmitters[2, 1].objectReferenceValue } };
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = this.GUIListOfFloatObjectToggleFields(SubModuleUI.s_Texts.subEmitterTypeTexts[index1], new SerializedProperty[2]{ this.m_SubEmitters[index1, 0], this.m_SubEmitters[index1, 1] }, (EditorGUI.ObjectFieldValidator) null, SubModuleUI.s_Texts.create, 1 != 0);
        if (index2 != -1)
          this.CreateAndAssignSubEmitter(this.m_SubEmitters[index1, index2], (SubModuleUI.SubEmitterType) index1);
      }
      Object[,] objectArray2 = new Object[3, 2]{ { this.m_SubEmitters[0, 0].objectReferenceValue, this.m_SubEmitters[0, 1].objectReferenceValue }, { this.m_SubEmitters[1, 0].objectReferenceValue, this.m_SubEmitters[1, 1].objectReferenceValue }, { this.m_SubEmitters[2, 0].objectReferenceValue, this.m_SubEmitters[2, 1].objectReferenceValue } };
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          if (objectArray1[index1, index2] != objectArray2[index1, index2])
          {
            if (this.m_CheckObjectIndex == -1 && this.m_CheckObjectTypeIndex == -1)
              EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
            this.m_CheckObjectTypeIndex = index1;
            this.m_CheckObjectIndex = index2;
          }
        }
      }
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text = text + "\n\tSub Emitters are enabled.";
    }

    public enum SubEmitterType
    {
      None = -1,
      Birth = 0,
      Collision = 1,
      Death = 2,
      TypesMax = 3,
    }

    private class Texts
    {
      public GUIContent create = new GUIContent(string.Empty, "Create and assign a Particle System as sub emitter");
      public GUIContent[] subEmitterTypeTexts;

      public Texts()
      {
        this.subEmitterTypeTexts = new GUIContent[3];
        this.subEmitterTypeTexts[0] = new GUIContent("Birth", "Start spawning on birth of particle.");
        this.subEmitterTypeTexts[1] = new GUIContent("Collision", "Spawn on collision of particle. Sub emitter can only emit as burst.");
        this.subEmitterTypeTexts[2] = new GUIContent("Death", "Spawn on death of particle. Sub emitter can only emit as burst.");
      }
    }
  }
}
