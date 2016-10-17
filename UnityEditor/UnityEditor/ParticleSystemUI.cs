// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ParticleSystemUI
  {
    public ParticleEffectUI m_ParticleEffectUI;
    public ModuleUI[] m_Modules;
    public ParticleSystem m_ParticleSystem;
    public SerializedObject m_ParticleSystemSerializedObject;
    public SerializedObject m_RendererSerializedObject;
    private static string[] s_ModuleNames;
    private static string m_SupportsCullingText;
    private static ParticleSystemUI.Texts s_Texts;

    public void Init(ParticleEffectUI owner, ParticleSystem ps)
    {
      if (ParticleSystemUI.s_ModuleNames == null)
        ParticleSystemUI.s_ModuleNames = ParticleSystemUI.GetUIModuleNames();
      this.m_ParticleEffectUI = owner;
      this.m_ParticleSystem = ps;
      this.m_ParticleSystemSerializedObject = new SerializedObject((Object) this.m_ParticleSystem);
      this.m_RendererSerializedObject = (SerializedObject) null;
      ParticleSystemUI.m_SupportsCullingText = (string) null;
      this.m_Modules = ParticleSystemUI.CreateUIModules(this, this.m_ParticleSystemSerializedObject);
      if ((Object) this.GetParticleSystemRenderer() != (Object) null)
        this.InitRendererUI();
      this.UpdateParticleSystemInfoString();
    }

    internal ParticleSystemRenderer GetParticleSystemRenderer()
    {
      if ((Object) this.m_ParticleSystem != (Object) null)
        return this.m_ParticleSystem.GetComponent<ParticleSystemRenderer>();
      return (ParticleSystemRenderer) null;
    }

    internal ModuleUI GetParticleSystemRendererModuleUI()
    {
      return this.m_Modules[this.m_Modules.Length - 1];
    }

    private void InitRendererUI()
    {
      if ((Object) this.GetParticleSystemRenderer() == (Object) null)
        this.m_ParticleSystem.gameObject.AddComponent<ParticleSystemRenderer>();
      ParticleSystemRenderer particleSystemRenderer = this.GetParticleSystemRenderer();
      if (!((Object) particleSystemRenderer != (Object) null))
        return;
      this.m_RendererSerializedObject = new SerializedObject((Object) particleSystemRenderer);
      this.m_Modules[this.m_Modules.Length - 1] = (ModuleUI) new RendererModuleUI(this, this.m_RendererSerializedObject, ParticleSystemUI.s_ModuleNames[ParticleSystemUI.s_ModuleNames.Length - 1]);
      EditorUtility.SetSelectedWireframeHidden((Renderer) particleSystemRenderer, !ParticleEffectUI.m_ShowWireframe);
    }

    private void ClearRenderer()
    {
      this.m_RendererSerializedObject = (SerializedObject) null;
      ParticleSystemRenderer particleSystemRenderer = this.GetParticleSystemRenderer();
      if ((Object) particleSystemRenderer != (Object) null)
        Undo.DestroyObjectImmediate((Object) particleSystemRenderer);
      this.m_Modules[this.m_Modules.Length - 1] = (ModuleUI) null;
    }

    public string GetName()
    {
      return this.m_ParticleSystem.gameObject.name;
    }

    public float GetEmitterDuration()
    {
      InitialModuleUI module = this.m_Modules[0] as InitialModuleUI;
      if (module != null)
        return module.m_LengthInSec.floatValue;
      return -1f;
    }

    private ParticleSystem GetSelectedParticleSystem()
    {
      return Selection.activeGameObject.GetComponent<ParticleSystem>();
    }

    public void OnGUI(ParticleSystem root, float width, bool fixedWidth)
    {
      if (ParticleSystemUI.s_Texts == null)
        ParticleSystemUI.s_Texts = new ParticleSystemUI.Texts();
      bool flag1 = Event.current.type == EventType.Repaint;
      string str = !(bool) ((Object) this.m_ParticleSystem) ? (string) null : this.m_ParticleSystem.gameObject.name;
      if (fixedWidth)
      {
        EditorGUIUtility.labelWidth = width * 0.55f;
        EditorGUILayout.BeginVertical(GUILayout.Width(width));
      }
      else
      {
        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUIUtility.labelWidth -= 4f;
        EditorGUILayout.BeginVertical();
      }
      for (int moduleIndex = 0; moduleIndex < this.m_Modules.Length; ++moduleIndex)
      {
        ModuleUI module1 = this.m_Modules[moduleIndex];
        if (module1 != null)
        {
          bool flag2 = module1 == this.m_Modules[0];
          if (module1.visibleUI || flag2)
          {
            GUIContent content = new GUIContent();
            Rect rect;
            GUIStyle style;
            if (flag2)
            {
              rect = GUILayoutUtility.GetRect(width, 25f);
              style = ParticleSystemStyles.Get().emitterHeaderStyle;
            }
            else
            {
              rect = GUILayoutUtility.GetRect(width, 15f);
              style = ParticleSystemStyles.Get().moduleHeaderStyle;
            }
            if (module1.foldout)
            {
              EditorGUI.BeginDisabledGroup(!module1.enabled);
              Rect position = EditorGUILayout.BeginVertical(ParticleSystemStyles.Get().modulePadding, new GUILayoutOption[0]);
              position.y -= 4f;
              position.height += 4f;
              GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().moduleBgStyle);
              module1.OnInspectorGUI(this.m_ParticleSystem);
              EditorGUILayout.EndVertical();
              EditorGUI.EndDisabledGroup();
            }
            if (flag2)
            {
              ParticleSystemRenderer particleSystemRenderer = this.GetParticleSystemRenderer();
              float num = 21f;
              Rect position = new Rect(rect.x + 4f, rect.y + 2f, num, num);
              if (flag1 && (Object) particleSystemRenderer != (Object) null)
              {
                bool flag3 = false;
                int instanceID = 0;
                RendererModuleUI module2 = this.m_Modules[this.m_Modules.Length - 1] as RendererModuleUI;
                if (module2 != null)
                {
                  if (module2.IsMeshEmitter())
                  {
                    if ((Object) particleSystemRenderer.mesh != (Object) null)
                      instanceID = particleSystemRenderer.mesh.GetInstanceID();
                  }
                  else if ((Object) particleSystemRenderer.sharedMaterial != (Object) null)
                    instanceID = particleSystemRenderer.sharedMaterial.GetInstanceID();
                  if (EditorUtility.IsDirty(instanceID))
                    AssetPreview.ClearTemporaryAssetPreviews();
                }
                if (instanceID != 0)
                {
                  Texture2D assetPreview = AssetPreview.GetAssetPreview(instanceID);
                  if ((Object) assetPreview != (Object) null)
                  {
                    GUI.DrawTexture(position, (Texture) assetPreview, ScaleMode.StretchToFill, true);
                    flag3 = true;
                  }
                }
                if (!flag3)
                  GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().moduleBgStyle);
              }
              if (EditorGUI.ButtonMouseDown(position, GUIContent.none, FocusType.Passive, GUIStyle.none))
              {
                if (EditorGUI.actionKey)
                {
                  List<int> intList = new List<int>();
                  int instanceId = this.m_ParticleSystem.gameObject.GetInstanceID();
                  intList.AddRange((IEnumerable<int>) Selection.instanceIDs);
                  if (!intList.Contains(instanceId) || intList.Count != 1)
                  {
                    if (intList.Contains(instanceId))
                      intList.Remove(instanceId);
                    else
                      intList.Add(instanceId);
                  }
                  Selection.instanceIDs = intList.ToArray();
                }
                else
                {
                  Selection.instanceIDs = new int[0];
                  Selection.activeInstanceID = this.m_ParticleSystem.gameObject.GetInstanceID();
                }
              }
            }
            Rect position1 = new Rect(rect.x + 2f, rect.y + 1f, 13f, 13f);
            if (!flag2 && GUI.Button(position1, GUIContent.none, GUIStyle.none))
              module1.enabled = !module1.enabled;
            Rect position2 = new Rect((float) ((double) rect.x + (double) rect.width - 10.0), (float) ((double) rect.y + (double) rect.height - 10.0), 10f, 10f);
            Rect position3 = new Rect(position2.x - 4f, position2.y - 4f, position2.width + 4f, position2.height + 4f);
            Rect position4 = new Rect(position2.x - 23f, position2.y - 3f, 16f, 16f);
            if (flag2 && EditorGUI.ButtonMouseDown(position3, ParticleSystemUI.s_Texts.addModules, FocusType.Passive, GUIStyle.none))
              this.ShowAddModuleMenu();
            content.text = string.IsNullOrEmpty(str) ? module1.displayName : (!flag2 ? module1.displayName : str);
            content.tooltip = module1.toolTip;
            if (GUI.Toggle(rect, module1.foldout, content, style) != module1.foldout)
            {
              switch (Event.current.button)
              {
                case 0:
                  bool flag4 = !module1.foldout;
                  if (Event.current.control)
                  {
                    foreach (ModuleUI module2 in this.m_Modules)
                    {
                      if (module2 != null && module2.visibleUI)
                        module2.foldout = flag4;
                    }
                    break;
                  }
                  module1.foldout = flag4;
                  break;
                case 1:
                  if (flag2)
                  {
                    this.ShowEmitterMenu();
                    break;
                  }
                  this.ShowModuleMenu(moduleIndex);
                  break;
              }
            }
            if (!flag2)
              GUI.Toggle(position1, module1.enabled, GUIContent.none, ParticleSystemStyles.Get().checkmark);
            if (flag1 && flag2)
              GUI.Label(position2, GUIContent.none, ParticleSystemStyles.Get().plus);
            ParticleSystemUI.s_Texts.supportsCullingText.tooltip = ParticleSystemUI.m_SupportsCullingText;
            if (flag2 && ParticleSystemUI.s_Texts.supportsCullingText.tooltip != null)
              GUI.Label(position4, ParticleSystemUI.s_Texts.supportsCullingText);
            GUILayout.Space(1f);
          }
        }
      }
      GUILayout.Space(-1f);
      EditorGUILayout.EndVertical();
      this.ApplyProperties();
    }

    public void OnSceneGUI()
    {
      if (this.m_Modules == null || (Object) this.m_ParticleSystem == (Object) null)
        return;
      if (this.m_ParticleSystem.particleCount > 0)
      {
        ParticleSystemRenderer particleSystemRenderer1 = this.GetParticleSystemRenderer();
        EditorUtility.SetSelectedWireframeHidden((Renderer) particleSystemRenderer1, !ParticleEffectUI.m_ShowWireframe);
        if (ParticleEffectUI.m_ShowWireframe)
        {
          ModuleUI rendererModuleUi = this.GetParticleSystemRendererModuleUI();
          ParticleSystemRenderer particleSystemRenderer2 = this.GetParticleSystemRenderer();
          if (rendererModuleUi != null && rendererModuleUi.enabled && particleSystemRenderer2.editorEnabled)
          {
            Vector3 extents = particleSystemRenderer1.bounds.extents;
            Transform transform = Camera.current.transform;
            Vector2 size = new Vector2(0.0f, 0.0f);
            Vector3[] vector3Array = new Vector3[8]{ new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, 1f), new Vector3(-1f, 1f, -1f), new Vector3(-1f, 1f, 1f), new Vector3(1f, -1f, -1f), new Vector3(1f, -1f, 1f), new Vector3(1f, 1f, -1f), new Vector3(1f, 1f, 1f) };
            for (int index = 0; index < 8; ++index)
            {
              size.x = Mathf.Max(size.x, Vector3.Dot(Vector3.Scale(vector3Array[index], extents), transform.right));
              size.y = Mathf.Max(size.y, Vector3.Dot(Vector3.Scale(vector3Array[index], extents), transform.up));
            }
            Handles.RectangleCap(0, particleSystemRenderer1.bounds.center, Camera.current.transform.rotation, size);
          }
        }
      }
      this.UpdateProperties();
      InitialModuleUI module1 = (InitialModuleUI) this.m_Modules[0];
      foreach (ModuleUI module2 in this.m_Modules)
      {
        if (module2 != null && module2.visibleUI && (module2.enabled && module2.foldout))
          module2.OnSceneGUI(this.m_ParticleSystem, module1);
      }
      this.ApplyProperties();
    }

    public void ApplyProperties()
    {
      bool modifiedProperties = this.m_ParticleSystemSerializedObject.hasModifiedProperties;
      this.m_ParticleSystemSerializedObject.ApplyModifiedProperties();
      if (modifiedProperties)
      {
        if (!ParticleEffectUI.IsStopped(ParticleSystemEditorUtils.GetRoot(this.m_ParticleSystem)) && ParticleSystemEditorUtils.editorResimulation)
          ParticleSystemEditorUtils.PerformCompleteResimulation();
        this.UpdateParticleSystemInfoString();
      }
      if (this.m_RendererSerializedObject == null)
        return;
      this.m_RendererSerializedObject.ApplyModifiedProperties();
    }

    private void UpdateParticleSystemInfoString()
    {
      string empty = string.Empty;
      foreach (ModuleUI module in this.m_Modules)
      {
        if (module != null && module.visibleUI && module.enabled)
          module.UpdateCullingSupportedString(ref empty);
      }
      if (empty != string.Empty)
        ParticleSystemUI.m_SupportsCullingText = "Automatic culling is disabled because: " + empty;
      else
        ParticleSystemUI.m_SupportsCullingText = (string) null;
    }

    public void UpdateProperties()
    {
      this.m_ParticleSystemSerializedObject.UpdateIfDirtyOrScript();
      if (this.m_RendererSerializedObject == null)
        return;
      this.m_RendererSerializedObject.UpdateIfDirtyOrScript();
    }

    private void ResetModules()
    {
      foreach (ModuleUI module in this.m_Modules)
      {
        if (module != null)
        {
          module.enabled = false;
          if (!ParticleEffectUI.GetAllModulesVisible())
            module.visibleUI = false;
        }
      }
      if (this.m_Modules[this.m_Modules.Length - 1] == null)
        this.InitRendererUI();
      int[] numArray = new int[3]{ 1, 2, this.m_Modules.Length - 1 };
      foreach (int index in numArray)
      {
        if (this.m_Modules[index] != null)
        {
          this.m_Modules[index].enabled = true;
          this.m_Modules[index].visibleUI = true;
        }
      }
    }

    private void ShowAddModuleMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < ParticleSystemUI.s_ModuleNames.Length; ++index)
      {
        if (this.m_Modules[index] == null || !this.m_Modules[index].visibleUI)
          genericMenu.AddItem(new GUIContent(ParticleSystemUI.s_ModuleNames[index]), false, new GenericMenu.MenuFunction2(this.AddModuleCallback), (object) index);
        else
          genericMenu.AddDisabledItem(new GUIContent(ParticleSystemUI.s_ModuleNames[index]));
      }
      genericMenu.AddSeparator(string.Empty);
      genericMenu.AddItem(new GUIContent("Show All Modules"), ParticleEffectUI.GetAllModulesVisible(), new GenericMenu.MenuFunction2(this.AddModuleCallback), (object) 10000);
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void AddModuleCallback(object obj)
    {
      int index = (int) obj;
      if (index >= 0 && index < this.m_Modules.Length)
      {
        if (index == this.m_Modules.Length - 1)
        {
          this.InitRendererUI();
        }
        else
        {
          this.m_Modules[index].enabled = true;
          this.m_Modules[index].foldout = true;
        }
      }
      else
        this.m_ParticleEffectUI.SetAllModulesVisible(!ParticleEffectUI.GetAllModulesVisible());
      this.ApplyProperties();
    }

    private void ModuleMenuCallback(object obj)
    {
      int index = (int) obj;
      if (index == this.m_Modules.Length - 1)
      {
        this.ClearRenderer();
      }
      else
      {
        if (!ParticleEffectUI.GetAllModulesVisible())
          this.m_Modules[index].visibleUI = false;
        this.m_Modules[index].enabled = false;
      }
    }

    private void ShowModuleMenu(int moduleIndex)
    {
      GenericMenu genericMenu = new GenericMenu();
      if (!ParticleEffectUI.GetAllModulesVisible())
        genericMenu.AddItem(new GUIContent("Remove"), false, new GenericMenu.MenuFunction2(this.ModuleMenuCallback), (object) moduleIndex);
      else
        genericMenu.AddDisabledItem(new GUIContent("Remove"));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void EmitterMenuCallback(object obj)
    {
      switch ((int) obj)
      {
        case 0:
          this.m_ParticleEffectUI.CreateParticleSystem(this.m_ParticleSystem, SubModuleUI.SubEmitterType.None);
          break;
        case 1:
          this.ResetModules();
          break;
        case 2:
          EditorGUIUtility.PingObject((Object) this.m_ParticleSystem);
          break;
      }
    }

    private void ShowEmitterMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Show Location"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 2);
      genericMenu.AddSeparator(string.Empty);
      if (this.m_ParticleSystem.gameObject.activeInHierarchy)
        genericMenu.AddItem(new GUIContent("Create Particle System"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 0);
      else
        genericMenu.AddDisabledItem(new GUIContent("Create new Particle System"));
      genericMenu.AddItem(new GUIContent("Reset"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 1);
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private static ModuleUI[] CreateUIModules(ParticleSystemUI e, SerializedObject so)
    {
      int num1 = 0;
      ModuleUI[] moduleUiArray = new ModuleUI[18];
      int index1 = 0;
      ParticleSystemUI owner1 = e;
      SerializedObject o1 = so;
      string[] moduleNames1 = ParticleSystemUI.s_ModuleNames;
      int index2 = num1;
      int num2 = 1;
      int num3 = index2 + num2;
      string displayName1 = moduleNames1[index2];
      InitialModuleUI initialModuleUi = new InitialModuleUI(owner1, o1, displayName1);
      moduleUiArray[index1] = (ModuleUI) initialModuleUi;
      int index3 = 1;
      ParticleSystemUI owner2 = e;
      SerializedObject o2 = so;
      string[] moduleNames2 = ParticleSystemUI.s_ModuleNames;
      int index4 = num3;
      int num4 = 1;
      int num5 = index4 + num4;
      string displayName2 = moduleNames2[index4];
      EmissionModuleUI emissionModuleUi = new EmissionModuleUI(owner2, o2, displayName2);
      moduleUiArray[index3] = (ModuleUI) emissionModuleUi;
      int index5 = 2;
      ParticleSystemUI owner3 = e;
      SerializedObject o3 = so;
      string[] moduleNames3 = ParticleSystemUI.s_ModuleNames;
      int index6 = num5;
      int num6 = 1;
      int num7 = index6 + num6;
      string displayName3 = moduleNames3[index6];
      ShapeModuleUI shapeModuleUi = new ShapeModuleUI(owner3, o3, displayName3);
      moduleUiArray[index5] = (ModuleUI) shapeModuleUi;
      int index7 = 3;
      ParticleSystemUI owner4 = e;
      SerializedObject o4 = so;
      string[] moduleNames4 = ParticleSystemUI.s_ModuleNames;
      int index8 = num7;
      int num8 = 1;
      int num9 = index8 + num8;
      string displayName4 = moduleNames4[index8];
      VelocityModuleUI velocityModuleUi1 = new VelocityModuleUI(owner4, o4, displayName4);
      moduleUiArray[index7] = (ModuleUI) velocityModuleUi1;
      int index9 = 4;
      ParticleSystemUI owner5 = e;
      SerializedObject o5 = so;
      string[] moduleNames5 = ParticleSystemUI.s_ModuleNames;
      int index10 = num9;
      int num10 = 1;
      int num11 = index10 + num10;
      string displayName5 = moduleNames5[index10];
      ClampVelocityModuleUI velocityModuleUi2 = new ClampVelocityModuleUI(owner5, o5, displayName5);
      moduleUiArray[index9] = (ModuleUI) velocityModuleUi2;
      int index11 = 5;
      ParticleSystemUI owner6 = e;
      SerializedObject o6 = so;
      string[] moduleNames6 = ParticleSystemUI.s_ModuleNames;
      int index12 = num11;
      int num12 = 1;
      int num13 = index12 + num12;
      string displayName6 = moduleNames6[index12];
      InheritVelocityModuleUI velocityModuleUi3 = new InheritVelocityModuleUI(owner6, o6, displayName6);
      moduleUiArray[index11] = (ModuleUI) velocityModuleUi3;
      int index13 = 6;
      ParticleSystemUI owner7 = e;
      SerializedObject o7 = so;
      string[] moduleNames7 = ParticleSystemUI.s_ModuleNames;
      int index14 = num13;
      int num14 = 1;
      int num15 = index14 + num14;
      string displayName7 = moduleNames7[index14];
      ForceModuleUI forceModuleUi = new ForceModuleUI(owner7, o7, displayName7);
      moduleUiArray[index13] = (ModuleUI) forceModuleUi;
      int index15 = 7;
      ParticleSystemUI owner8 = e;
      SerializedObject o8 = so;
      string[] moduleNames8 = ParticleSystemUI.s_ModuleNames;
      int index16 = num15;
      int num16 = 1;
      int num17 = index16 + num16;
      string displayName8 = moduleNames8[index16];
      ColorModuleUI colorModuleUi = new ColorModuleUI(owner8, o8, displayName8);
      moduleUiArray[index15] = (ModuleUI) colorModuleUi;
      int index17 = 8;
      ParticleSystemUI owner9 = e;
      SerializedObject o9 = so;
      string[] moduleNames9 = ParticleSystemUI.s_ModuleNames;
      int index18 = num17;
      int num18 = 1;
      int num19 = index18 + num18;
      string displayName9 = moduleNames9[index18];
      ColorByVelocityModuleUI velocityModuleUi4 = new ColorByVelocityModuleUI(owner9, o9, displayName9);
      moduleUiArray[index17] = (ModuleUI) velocityModuleUi4;
      int index19 = 9;
      ParticleSystemUI owner10 = e;
      SerializedObject o10 = so;
      string[] moduleNames10 = ParticleSystemUI.s_ModuleNames;
      int index20 = num19;
      int num20 = 1;
      int num21 = index20 + num20;
      string displayName10 = moduleNames10[index20];
      SizeModuleUI sizeModuleUi = new SizeModuleUI(owner10, o10, displayName10);
      moduleUiArray[index19] = (ModuleUI) sizeModuleUi;
      int index21 = 10;
      ParticleSystemUI owner11 = e;
      SerializedObject o11 = so;
      string[] moduleNames11 = ParticleSystemUI.s_ModuleNames;
      int index22 = num21;
      int num22 = 1;
      int num23 = index22 + num22;
      string displayName11 = moduleNames11[index22];
      SizeByVelocityModuleUI velocityModuleUi5 = new SizeByVelocityModuleUI(owner11, o11, displayName11);
      moduleUiArray[index21] = (ModuleUI) velocityModuleUi5;
      int index23 = 11;
      ParticleSystemUI owner12 = e;
      SerializedObject o12 = so;
      string[] moduleNames12 = ParticleSystemUI.s_ModuleNames;
      int index24 = num23;
      int num24 = 1;
      int num25 = index24 + num24;
      string displayName12 = moduleNames12[index24];
      RotationModuleUI rotationModuleUi = new RotationModuleUI(owner12, o12, displayName12);
      moduleUiArray[index23] = (ModuleUI) rotationModuleUi;
      int index25 = 12;
      ParticleSystemUI owner13 = e;
      SerializedObject o13 = so;
      string[] moduleNames13 = ParticleSystemUI.s_ModuleNames;
      int index26 = num25;
      int num26 = 1;
      int num27 = index26 + num26;
      string displayName13 = moduleNames13[index26];
      RotationByVelocityModuleUI velocityModuleUi6 = new RotationByVelocityModuleUI(owner13, o13, displayName13);
      moduleUiArray[index25] = (ModuleUI) velocityModuleUi6;
      int index27 = 13;
      ParticleSystemUI owner14 = e;
      SerializedObject o14 = so;
      string[] moduleNames14 = ParticleSystemUI.s_ModuleNames;
      int index28 = num27;
      int num28 = 1;
      int num29 = index28 + num28;
      string displayName14 = moduleNames14[index28];
      ExternalForcesModuleUI externalForcesModuleUi = new ExternalForcesModuleUI(owner14, o14, displayName14);
      moduleUiArray[index27] = (ModuleUI) externalForcesModuleUi;
      int index29 = 14;
      ParticleSystemUI owner15 = e;
      SerializedObject o15 = so;
      string[] moduleNames15 = ParticleSystemUI.s_ModuleNames;
      int index30 = num29;
      int num30 = 1;
      int num31 = index30 + num30;
      string displayName15 = moduleNames15[index30];
      CollisionModuleUI collisionModuleUi = new CollisionModuleUI(owner15, o15, displayName15);
      moduleUiArray[index29] = (ModuleUI) collisionModuleUi;
      int index31 = 15;
      ParticleSystemUI owner16 = e;
      SerializedObject o16 = so;
      string[] moduleNames16 = ParticleSystemUI.s_ModuleNames;
      int index32 = num31;
      int num32 = 1;
      int num33 = index32 + num32;
      string displayName16 = moduleNames16[index32];
      SubModuleUI subModuleUi = new SubModuleUI(owner16, o16, displayName16);
      moduleUiArray[index31] = (ModuleUI) subModuleUi;
      int index33 = 16;
      ParticleSystemUI owner17 = e;
      SerializedObject o17 = so;
      string[] moduleNames17 = ParticleSystemUI.s_ModuleNames;
      int index34 = num33;
      int num34 = 1;
      int num35 = index34 + num34;
      string displayName17 = moduleNames17[index34];
      UVModuleUI uvModuleUi = new UVModuleUI(owner17, o17, displayName17);
      moduleUiArray[index33] = (ModuleUI) uvModuleUi;
      return moduleUiArray;
    }

    public static string[] GetUIModuleNames()
    {
      return new string[18]{ string.Empty, "Emission", "Shape", "Velocity over Lifetime", "Limit Velocity over Lifetime", "Inherit Velocity", "Force over Lifetime", "Color over Lifetime", "Color by Speed", "Size over Lifetime", "Size by Speed", "Rotation over Lifetime", "Rotation by Speed", "External Forces", "Collision", "Sub Emitters", "Texture Sheet Animation", "Renderer" };
    }

    public enum DefaultTypes
    {
      Root,
      SubBirth,
      SubCollision,
      SubDeath,
    }

    protected class Texts
    {
      public GUIContent addModules = new GUIContent(string.Empty, "Show/Hide Modules");
      public GUIContent supportsCullingText = new GUIContent(string.Empty, (Texture) ParticleSystemStyles.Get().warningIcon);
    }
  }
}
