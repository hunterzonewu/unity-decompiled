// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleEffectUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class ParticleEffectUI
  {
    public static bool m_ShowWireframe = false;
    private static readonly Vector2 k_MinEmitterAreaSize = new Vector2(125f, 100f);
    private static readonly Vector2 k_MinCurveAreaSize = new Vector2(100f, 100f);
    private static readonly Color k_DarkSkinDisabledColor = new Color(0.66f, 0.66f, 0.66f, 0.95f);
    private static readonly Color k_LightSkinDisabledColor = new Color(0.84f, 0.84f, 0.84f, 0.95f);
    private static PrefKey kPlay = new PrefKey("ParticleSystem/Play", ",");
    private static PrefKey kStop = new PrefKey("ParticleSystem/Stop", ".");
    private static PrefKey kForward = new PrefKey("ParticleSystem/Forward", "m");
    private static PrefKey kReverse = new PrefKey("ParticleSystem/Reverse", "n");
    private TimeHelper m_TimeHelper = new TimeHelper();
    private float m_EmitterAreaWidth = 230f;
    private float m_CurveEditorAreaHeight = 330f;
    private Vector2 m_EmitterAreaScrollPos = Vector2.zero;
    private int m_IsDraggingTimeHotControlID = -1;
    private const string k_SimulationStateId = "SimulationState";
    private const string k_ShowSelectedId = "ShowSelected";
    public ParticleEffectUIOwner m_Owner;
    public ParticleSystemUI[] m_Emitters;
    private ParticleSystemCurveEditor m_ParticleSystemCurveEditor;
    private ParticleSystem m_SelectedParticleSystem;
    private bool m_ShowOnlySelectedMode;
    public static bool m_VerticalLayout;
    private static ParticleEffectUI.Texts s_Texts;

    internal static ParticleEffectUI.Texts texts
    {
      get
      {
        if (ParticleEffectUI.s_Texts == null)
          ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
        return ParticleEffectUI.s_Texts;
      }
    }

    public ParticleEffectUI(ParticleEffectUIOwner owner)
    {
      this.m_Owner = owner;
    }

    private bool ShouldManagePlaybackState(ParticleSystem root)
    {
      bool flag = false;
      if ((UnityEngine.Object) root != (UnityEngine.Object) null)
        flag = root.gameObject.activeInHierarchy;
      if (!EditorApplication.isPlaying && !ParticleSystemEditorUtils.editorUpdateAll)
        return flag;
      return false;
    }

    private static Color GetDisabledColor()
    {
      if (!EditorGUIUtility.isProSkin)
        return ParticleEffectUI.k_LightSkinDisabledColor;
      return ParticleEffectUI.k_DarkSkinDisabledColor;
    }

    internal static ParticleSystem[] GetParticleSystems(ParticleSystem root)
    {
      List<ParticleSystem> particleSystems = new List<ParticleSystem>();
      particleSystems.Add(root);
      ParticleEffectUI.GetDirectParticleSystemChildrenRecursive(root.transform, particleSystems);
      return particleSystems.ToArray();
    }

    private static void GetDirectParticleSystemChildrenRecursive(Transform transform, List<ParticleSystem> particleSystems)
    {
      foreach (Transform transform1 in transform)
      {
        ParticleSystem component = transform1.gameObject.GetComponent<ParticleSystem>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          particleSystems.Add(component);
          ParticleEffectUI.GetDirectParticleSystemChildrenRecursive(transform1, particleSystems);
        }
      }
    }

    public bool InitializeIfNeeded(ParticleSystem shuriken)
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(shuriken);
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        return false;
      ParticleSystem[] particleSystems = ParticleEffectUI.GetParticleSystems(root);
      if ((UnityEngine.Object) root == (UnityEngine.Object) this.GetRoot() && this.m_ParticleSystemCurveEditor != null && (this.m_Emitters != null && particleSystems.Length == this.m_Emitters.Length))
      {
        this.m_SelectedParticleSystem = shuriken;
        if (this.IsShowOnlySelectedMode())
          this.RefreshShowOnlySelected();
        return false;
      }
      if (this.m_ParticleSystemCurveEditor != null)
        this.Clear();
      this.m_SelectedParticleSystem = shuriken;
      ParticleSystemEditorUtils.PerformCompleteResimulation();
      this.m_ParticleSystemCurveEditor = new ParticleSystemCurveEditor();
      this.m_ParticleSystemCurveEditor.Init();
      this.m_EmitterAreaWidth = EditorPrefs.GetFloat("ParticleSystemEmitterAreaWidth", ParticleEffectUI.k_MinEmitterAreaSize.x);
      this.m_CurveEditorAreaHeight = EditorPrefs.GetFloat("ParticleSystemCurveEditorAreaHeight", ParticleEffectUI.k_MinCurveAreaSize.y);
      this.InitAllEmitters(particleSystems);
      this.m_ShowOnlySelectedMode = this.m_Owner is ParticleSystemWindow && SessionState.GetBool("ShowSelected" + (object) root.GetInstanceID(), false);
      if (this.IsShowOnlySelectedMode())
        this.RefreshShowOnlySelected();
      this.m_EmitterAreaScrollPos.x = SessionState.GetFloat("CurrentEmitterAreaScroll", 0.0f);
      if (this.ShouldManagePlaybackState(root))
      {
        Vector3 vector3 = SessionState.GetVector3("SimulationState" + (object) root.GetInstanceID(), Vector3.zero);
        if (root.GetInstanceID() == (int) vector3.x)
        {
          float z = vector3.z;
          if ((double) z > 0.0)
            ParticleSystemEditorUtils.editorPlaybackTime = z;
        }
        this.Play();
      }
      return true;
    }

    internal void UndoRedoPerformed()
    {
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        foreach (ModuleUI module in emitter.m_Modules)
        {
          if (module != null)
            module.CheckVisibilityState();
        }
      }
      this.m_Owner.Repaint();
    }

    public void Clear()
    {
      ParticleSystem root = this.GetRoot();
      if (this.ShouldManagePlaybackState(root) && (UnityEngine.Object) root != (UnityEngine.Object) null)
      {
        ParticleEffectUI.PlayState playState = !this.IsPlaying() ? (!this.IsPaused() ? ParticleEffectUI.PlayState.Stopped : ParticleEffectUI.PlayState.Paused) : ParticleEffectUI.PlayState.Playing;
        int instanceId = root.GetInstanceID();
        SessionState.SetVector3("SimulationState" + (object) instanceId, new Vector3((float) instanceId, (float) playState, ParticleSystemEditorUtils.editorPlaybackTime));
      }
      this.m_ParticleSystemCurveEditor.OnDisable();
      ParticleEffectUtils.ClearPlanes();
      Tools.s_Hidden = false;
      if ((UnityEngine.Object) root != (UnityEngine.Object) null)
        SessionState.SetBool("ShowSelected" + (object) root.GetInstanceID(), this.m_ShowOnlySelectedMode);
      this.SetShowOnlySelectedMode(false);
      GameView.RepaintAll();
      SceneView.RepaintAll();
    }

    public static Vector2 GetMinSize()
    {
      return ParticleEffectUI.k_MinEmitterAreaSize + ParticleEffectUI.k_MinCurveAreaSize;
    }

    public void Refresh()
    {
      this.UpdateProperties();
      this.m_ParticleSystemCurveEditor.Refresh();
    }

    public string GetNextParticleSystemName()
    {
      string empty = string.Empty;
      for (int index = 2; index < 50; ++index)
      {
        string str = "Particle System " + (object) index;
        bool flag = false;
        foreach (ParticleSystemUI emitter in this.m_Emitters)
        {
          if (emitter.m_ParticleSystem.name == str)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return str;
      }
      return "Particle System";
    }

    public bool IsParticleSystemUIVisible(ParticleSystemUI psUI)
    {
      switch (!(this.m_Owner is ParticleSystemInspector) ? ParticleEffectUI.OwnerType.ParticleSystemWindow : ParticleEffectUI.OwnerType.Inspector)
      {
        case ParticleEffectUI.OwnerType.ParticleSystemWindow:
          return true;
        case ParticleEffectUI.OwnerType.Inspector:
          if (!((UnityEngine.Object) psUI.m_ParticleSystem == (UnityEngine.Object) this.m_SelectedParticleSystem))
            break;
          goto case ParticleEffectUI.OwnerType.ParticleSystemWindow;
      }
      return false;
    }

    private void InitAllEmitters(ParticleSystem[] shurikens)
    {
      int length = shurikens.Length;
      if (length == 0)
        return;
      this.m_Emitters = new ParticleSystemUI[length];
      for (int index = 0; index < length; ++index)
      {
        this.m_Emitters[index] = new ParticleSystemUI();
        this.m_Emitters[index].Init(this, shurikens[index]);
      }
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        foreach (ModuleUI module in emitter.m_Modules)
        {
          if (module != null)
            module.Validate();
        }
      }
      if (!ParticleEffectUI.GetAllModulesVisible())
        return;
      this.SetAllModulesVisible(true);
    }

    public ParticleSystemUI GetParticleSystemUIForParticleSystem(ParticleSystem shuriken)
    {
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        if ((UnityEngine.Object) emitter.m_ParticleSystem == (UnityEngine.Object) shuriken)
          return emitter;
      }
      return (ParticleSystemUI) null;
    }

    public void PlayOnAwakeChanged(bool newPlayOnAwake)
    {
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        (emitter.m_Modules[0] as InitialModuleUI).m_PlayOnAwake.boolValue = newPlayOnAwake;
        emitter.ApplyProperties();
      }
    }

    public bool ValidateParticleSystemProperty(SerializedProperty shurikenProperty)
    {
      if (shurikenProperty != null)
      {
        ParticleSystem objectReferenceValue = shurikenProperty.objectReferenceValue as ParticleSystem;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null && this.GetParticleSystemUIForParticleSystem(objectReferenceValue) == null)
        {
          EditorUtility.DisplayDialog("ParticleSystem Warning", "The SubEmitter module cannot reference a ParticleSystem that is not a child of the root ParticleSystem.\n\nThe ParticleSystem '" + objectReferenceValue.name + "' must be a child of the ParticleSystem '" + ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem).name + "'.", "Ok");
          shurikenProperty.objectReferenceValue = (UnityEngine.Object) null;
          return false;
        }
      }
      return true;
    }

    public GameObject CreateParticleSystem(ParticleSystem parentOfNewParticleSystem, SubModuleUI.SubEmitterType defaultType)
    {
      GameObject gameObject = new GameObject(this.GetNextParticleSystemName(), new System.Type[1]{ typeof (ParticleSystem) });
      if (!(bool) ((UnityEngine.Object) gameObject))
        return (GameObject) null;
      if ((bool) ((UnityEngine.Object) parentOfNewParticleSystem))
        gameObject.transform.parent = parentOfNewParticleSystem.transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = Quaternion.identity;
      ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
      if (defaultType != SubModuleUI.SubEmitterType.None)
        component.SetupDefaultType((int) defaultType);
      SessionState.SetFloat("CurrentEmitterAreaScroll", this.m_EmitterAreaScrollPos.x);
      return gameObject;
    }

    public List<ParticleSystemUI> GetParticleSystemUIList(List<ParticleSystem> shurikens)
    {
      List<ParticleSystemUI> particleSystemUiList = new List<ParticleSystemUI>();
      using (List<ParticleSystem>.Enumerator enumerator = shurikens.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ParticleSystemUI forParticleSystem = this.GetParticleSystemUIForParticleSystem(enumerator.Current);
          if (forParticleSystem != null)
            particleSystemUiList.Add(forParticleSystem);
        }
      }
      return particleSystemUiList;
    }

    public ParticleSystemCurveEditor GetParticleSystemCurveEditor()
    {
      return this.m_ParticleSystemCurveEditor;
    }

    private void SceneViewGUICallback(UnityEngine.Object target, SceneView sceneView)
    {
      this.PlayStopGUI();
    }

    public void OnSceneViewGUI()
    {
      ParticleSystem root = this.GetRoot();
      if (!(bool) ((UnityEngine.Object) root) || !root.gameObject.activeInHierarchy)
        return;
      SceneViewOverlay.Window(ParticleSystemInspector.playBackTitle, new SceneViewOverlay.WindowFunction(this.SceneViewGUICallback), 400, SceneViewOverlay.WindowDisplayOption.OneWindowPerTitle);
    }

    public void OnSceneGUI()
    {
      foreach (ParticleSystemUI emitter in this.m_Emitters)
        emitter.OnSceneGUI();
    }

    internal void PlayBackTimeGUI(ParticleSystem root)
    {
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      EventType type = Event.current.type;
      int hotControl = GUIUtility.hotControl;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.BeginChangeCheck();
      EditorGUI.kFloatFieldFormatString = ParticleEffectUI.s_Texts.secondsFloatFieldFormatString;
      float a = EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.previewTime, ParticleSystemEditorUtils.editorPlaybackTime, new GUILayoutOption[0]);
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      if (EditorGUI.EndChangeCheck())
      {
        if (type == EventType.MouseDrag)
        {
          ParticleSystemEditorUtils.editorIsScrubbing = true;
          float editorSimulationSpeed = ParticleSystemEditorUtils.editorSimulationSpeed;
          float editorPlaybackTime = ParticleSystemEditorUtils.editorPlaybackTime;
          float num = a - editorPlaybackTime;
          a = editorPlaybackTime + num * (0.05f * editorSimulationSpeed);
        }
        ParticleSystemEditorUtils.editorPlaybackTime = Mathf.Max(a, 0.0f);
        if (root.isStopped)
        {
          root.Play();
          root.Pause();
        }
        ParticleSystemEditorUtils.PerformCompleteResimulation();
      }
      if (type == EventType.MouseDown && GUIUtility.hotControl != hotControl)
      {
        this.m_IsDraggingTimeHotControlID = GUIUtility.hotControl;
        ParticleSystemEditorUtils.editorIsScrubbing = true;
      }
      if (this.m_IsDraggingTimeHotControlID != -1 && GUIUtility.hotControl != this.m_IsDraggingTimeHotControlID)
      {
        this.m_IsDraggingTimeHotControlID = -1;
        ParticleSystemEditorUtils.editorIsScrubbing = false;
      }
      double num1 = (double) EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.particleCount, (float) root.particleCount, new GUILayoutOption[0]);
    }

    private void HandleKeyboardShortcuts(ParticleSystem root)
    {
      Event current = Event.current;
      if (current.type == EventType.KeyDown)
      {
        int num = 0;
        if (current.keyCode == (Event) ParticleEffectUI.kPlay.keyCode)
        {
          if (EditorApplication.isPlaying)
          {
            this.Stop();
            this.Play();
          }
          else if (!ParticleSystemEditorUtils.editorIsPlaying)
            this.Play();
          else
            this.Pause();
          current.Use();
        }
        else if (current.keyCode == (Event) ParticleEffectUI.kStop.keyCode)
        {
          this.Stop();
          current.Use();
        }
        else if (current.keyCode == (Event) ParticleEffectUI.kReverse.keyCode)
          num = -1;
        else if (current.keyCode == (Event) ParticleEffectUI.kForward.keyCode)
          num = 1;
        if (num != 0)
        {
          ParticleSystemEditorUtils.editorIsScrubbing = true;
          float editorSimulationSpeed = ParticleSystemEditorUtils.editorSimulationSpeed;
          ParticleSystemEditorUtils.editorPlaybackTime = Mathf.Max(0.0f, ParticleSystemEditorUtils.editorPlaybackTime + (float) ((!current.shift ? 1.0 : 3.0) * (double) this.m_TimeHelper.deltaTime * (num <= 0 ? -3.0 : 3.0)) * editorSimulationSpeed);
          if (root.isStopped)
          {
            root.Play();
            root.Pause();
          }
          ParticleSystemEditorUtils.PerformCompleteResimulation();
          current.Use();
        }
      }
      if (current.type != EventType.KeyUp || current.keyCode != (Event) ParticleEffectUI.kReverse.keyCode && current.keyCode != (Event) ParticleEffectUI.kForward.keyCode)
        return;
      ParticleSystemEditorUtils.editorIsScrubbing = false;
    }

    internal ParticleSystem GetRoot()
    {
      return ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
    }

    internal static bool IsStopped(ParticleSystem root)
    {
      if (!ParticleSystemEditorUtils.editorIsPlaying && !ParticleSystemEditorUtils.editorIsPaused)
        return !ParticleSystemEditorUtils.editorIsScrubbing;
      return false;
    }

    internal bool IsPaused()
    {
      if (!this.IsPlaying())
        return !ParticleEffectUI.IsStopped(this.GetRoot());
      return false;
    }

    internal bool IsPlaying()
    {
      return ParticleSystemEditorUtils.editorIsPlaying;
    }

    internal void Play()
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      if (!(bool) ((UnityEngine.Object) root))
        return;
      root.Play();
      ParticleSystemEditorUtils.editorIsScrubbing = false;
      this.m_Owner.Repaint();
    }

    internal void Pause()
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      if (!(bool) ((UnityEngine.Object) root))
        return;
      root.Pause();
      ParticleSystemEditorUtils.editorIsScrubbing = true;
      this.m_Owner.Repaint();
    }

    internal void Stop()
    {
      ParticleSystemEditorUtils.editorIsScrubbing = false;
      ParticleSystemEditorUtils.editorPlaybackTime = 0.0f;
      ParticleSystemEditorUtils.StopEffect();
      this.m_Owner.Repaint();
    }

    internal void PlayStopGUI()
    {
      if (ParticleEffectUI.s_Texts == null)
        ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      if (Event.current.type == EventType.Layout)
      {
        double num = (double) this.m_TimeHelper.Update();
      }
      if (!EditorApplication.isPlaying)
      {
        GUILayout.BeginHorizontal();
        bool flag = ParticleSystemEditorUtils.editorIsPlaying && !ParticleSystemEditorUtils.editorIsPaused;
        if (GUILayout.Button(!flag ? ParticleEffectUI.s_Texts.play : ParticleEffectUI.s_Texts.pause, (GUIStyle) "ButtonLeft", new GUILayoutOption[0]))
        {
          if (flag)
            this.Pause();
          else
            this.Play();
        }
        if (GUILayout.Button(ParticleEffectUI.s_Texts.stop, (GUIStyle) "ButtonRight", new GUILayoutOption[0]))
          this.Stop();
        GUILayout.EndHorizontal();
        string fieldFormatString = EditorGUI.kFloatFieldFormatString;
        EditorGUI.kFloatFieldFormatString = ParticleEffectUI.s_Texts.secondsFloatFieldFormatString;
        ParticleSystemEditorUtils.editorSimulationSpeed = Mathf.Clamp(EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.previewSpeed, ParticleSystemEditorUtils.editorSimulationSpeed, new GUILayoutOption[0]), 0.0f, 10f);
        EditorGUI.kFloatFieldFormatString = fieldFormatString;
        this.PlayBackTimeGUI(root);
      }
      else
      {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ParticleEffectUI.s_Texts.play))
        {
          this.Stop();
          this.Play();
        }
        if (GUILayout.Button(ParticleEffectUI.s_Texts.stop))
          this.Stop();
        GUILayout.EndHorizontal();
      }
      this.HandleKeyboardShortcuts(root);
    }

    private void SingleParticleSystemGUI()
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      GUILayout.BeginVertical(ParticleSystemStyles.Get().effectBgStyle, new GUILayoutOption[0]);
      ParticleSystemUI forParticleSystem = this.GetParticleSystemUIForParticleSystem(this.m_SelectedParticleSystem);
      if (forParticleSystem != null)
      {
        float width = GUIClip.visibleRect.width - 18f;
        forParticleSystem.OnGUI(root, width, false);
      }
      GUILayout.EndVertical();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      ParticleSystemEditorUtils.editorResimulation = GUILayout.Toggle(ParticleSystemEditorUtils.editorResimulation, ParticleEffectUI.s_Texts.resimulation, EditorStyles.toggle, new GUILayoutOption[0]);
      ParticleEffectUI.m_ShowWireframe = GUILayout.Toggle(ParticleEffectUI.m_ShowWireframe, "Wireframe", EditorStyles.toggle, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      this.HandleKeyboardShortcuts(root);
    }

    private void DrawSelectionMarker(Rect rect)
    {
      ++rect.x;
      ++rect.y;
      rect.width -= 2f;
      rect.height -= 2f;
      ParticleSystemStyles.Get().selectionMarker.Draw(rect, GUIContent.none, false, true, true, false);
    }

    private List<ParticleSystemUI> GetSelectedParticleSystemUIs()
    {
      List<ParticleSystemUI> particleSystemUiList = new List<ParticleSystemUI>();
      int[] instanceIds = Selection.instanceIDs;
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        if (((IEnumerable<int>) instanceIds).Contains<int>(emitter.m_ParticleSystem.gameObject.GetInstanceID()))
          particleSystemUiList.Add(emitter);
      }
      return particleSystemUiList;
    }

    private void MultiParticleSystemGUI(bool verticalLayout)
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystem);
      GUILayout.BeginVertical(ParticleSystemStyles.Get().effectBgStyle, new GUILayoutOption[0]);
      this.m_EmitterAreaScrollPos = EditorGUILayout.BeginScrollView(this.m_EmitterAreaScrollPos);
      Rect position = EditorGUILayout.BeginVertical();
      this.m_EmitterAreaScrollPos -= EditorGUI.MouseDeltaReader(position, Event.current.alt);
      GUILayout.Space(3f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(3f);
      Color color = GUI.color;
      bool flag1 = Event.current.type == EventType.Repaint;
      bool flag2 = this.IsShowOnlySelectedMode();
      List<ParticleSystemUI> particleSystemUis = this.GetSelectedParticleSystemUIs();
      for (int index = 0; index < this.m_Emitters.Length; ++index)
      {
        if (index != 0)
          GUILayout.Space(ModuleUI.k_SpaceBetweenModules);
        bool flag3 = particleSystemUis.Contains(this.m_Emitters[index]);
        ModuleUI rendererModuleUi = this.m_Emitters[index].GetParticleSystemRendererModuleUI();
        if (flag1 && rendererModuleUi != null && !rendererModuleUi.enabled)
          GUI.color = ParticleEffectUI.GetDisabledColor();
        if (flag1 && flag2 && !flag3)
          GUI.color = ParticleEffectUI.GetDisabledColor();
        Rect rect = EditorGUILayout.BeginVertical();
        if (flag1 && flag3 && this.m_Emitters.Length > 1)
          this.DrawSelectionMarker(rect);
        this.m_Emitters[index].OnGUI(root, ModuleUI.k_CompactFixedModuleWidth, true);
        EditorGUILayout.EndVertical();
        GUI.color = color;
      }
      GUILayout.Space(5f);
      if (GUILayout.Button(ParticleEffectUI.s_Texts.addParticleSystem, (GUIStyle) "OL Plus", new GUILayoutOption[1]{ GUILayout.Width(20f) }))
        this.CreateParticleSystem(root, SubModuleUI.SubEmitterType.None);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
      this.m_EmitterAreaScrollPos -= EditorGUI.MouseDeltaReader(position, true);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
      GUILayout.EndVertical();
      this.HandleKeyboardShortcuts(root);
    }

    private void WindowCurveEditorGUI(bool verticalLayout)
    {
      Rect rect;
      if (verticalLayout)
      {
        rect = GUILayoutUtility.GetRect(13f, this.m_CurveEditorAreaHeight, new GUILayoutOption[1]
        {
          GUILayout.MinHeight(this.m_CurveEditorAreaHeight)
        });
      }
      else
      {
        EditorWindow owner = (EditorWindow) this.m_Owner;
        rect = GUILayoutUtility.GetRect(owner.position.width - this.m_EmitterAreaWidth, owner.position.height - 17f);
      }
      this.ResizeHandling(verticalLayout);
      this.m_ParticleSystemCurveEditor.OnGUI(rect);
    }

    private Rect ResizeHandling(bool verticalLayout)
    {
      Rect position;
      if (verticalLayout)
      {
        position = GUILayoutUtility.GetLastRect();
        position.y += -5f;
        position.height = 5f;
        float y = EditorGUI.MouseDeltaReader(position, true).y;
        if ((double) y != 0.0)
        {
          this.m_CurveEditorAreaHeight -= y;
          this.ClampWindowContentSizes();
          EditorPrefs.SetFloat("ParticleSystemCurveEditorAreaHeight", this.m_CurveEditorAreaHeight);
        }
        if (Event.current.type == EventType.Repaint)
          EditorGUIUtility.AddCursorRect(position, MouseCursor.SplitResizeUpDown);
      }
      else
      {
        position = new Rect(this.m_EmitterAreaWidth - 5f, 0.0f, 5f, GUIClip.visibleRect.height);
        float x = EditorGUI.MouseDeltaReader(position, true).x;
        if ((double) x != 0.0)
        {
          this.m_EmitterAreaWidth += x;
          this.ClampWindowContentSizes();
          EditorPrefs.SetFloat("ParticleSystemEmitterAreaWidth", this.m_EmitterAreaWidth);
        }
        if (Event.current.type == EventType.Repaint)
          EditorGUIUtility.AddCursorRect(position, MouseCursor.SplitResizeLeftRight);
      }
      return position;
    }

    private void ClampWindowContentSizes()
    {
      if (Event.current.type == EventType.Layout)
        return;
      float width = GUIClip.visibleRect.width;
      float height = GUIClip.visibleRect.height;
      if (ParticleEffectUI.m_VerticalLayout)
        this.m_CurveEditorAreaHeight = Mathf.Clamp(this.m_CurveEditorAreaHeight, ParticleEffectUI.k_MinCurveAreaSize.y, height - ParticleEffectUI.k_MinEmitterAreaSize.y);
      else
        this.m_EmitterAreaWidth = Mathf.Clamp(this.m_EmitterAreaWidth, ParticleEffectUI.k_MinEmitterAreaSize.x, width - ParticleEffectUI.k_MinCurveAreaSize.x);
    }

    public void OnGUI()
    {
      if (ParticleEffectUI.s_Texts == null)
        ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
      if (this.m_Emitters == null)
        return;
      this.UpdateProperties();
      switch (!(this.m_Owner is ParticleSystemInspector) ? ParticleEffectUI.OwnerType.ParticleSystemWindow : ParticleEffectUI.OwnerType.Inspector)
      {
        case ParticleEffectUI.OwnerType.Inspector:
          this.SingleParticleSystemGUI();
          break;
        case ParticleEffectUI.OwnerType.ParticleSystemWindow:
          this.ClampWindowContentSizes();
          bool verticalLayout = ParticleEffectUI.m_VerticalLayout;
          if (verticalLayout)
          {
            this.MultiParticleSystemGUI(verticalLayout);
            this.WindowCurveEditorGUI(verticalLayout);
            break;
          }
          GUILayout.BeginHorizontal();
          this.MultiParticleSystemGUI(verticalLayout);
          this.WindowCurveEditorGUI(verticalLayout);
          GUILayout.EndHorizontal();
          break;
        default:
          Debug.LogError((object) "Unhandled enum");
          break;
      }
      this.ApplyModifiedProperties();
    }

    private void ApplyModifiedProperties()
    {
      for (int index = 0; index < this.m_Emitters.Length; ++index)
        this.m_Emitters[index].ApplyProperties();
    }

    internal void UpdateProperties()
    {
      for (int index = 0; index < this.m_Emitters.Length; ++index)
        this.m_Emitters[index].UpdateProperties();
    }

    internal bool IsPlayOnAwake()
    {
      if (this.m_Emitters.Length > 0)
        return (this.m_Emitters[0].m_Modules[0] as InitialModuleUI).m_PlayOnAwake.boolValue;
      return false;
    }

    internal GameObject[] GetParticleSystemGameObjects()
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < this.m_Emitters.Length; ++index)
        gameObjectList.Add(this.m_Emitters[index].m_ParticleSystem.gameObject);
      return gameObjectList.ToArray();
    }

    internal static bool GetAllModulesVisible()
    {
      return EditorPrefs.GetBool("ParticleSystemShowAllModules", true);
    }

    internal void SetAllModulesVisible(bool showAll)
    {
      EditorPrefs.SetBool("ParticleSystemShowAllModules", showAll);
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        for (int index = 0; index < emitter.m_Modules.Length; ++index)
        {
          ModuleUI module = emitter.m_Modules[index];
          if (module != null)
          {
            if (showAll)
            {
              if (!module.visibleUI)
                module.visibleUI = true;
            }
            else
            {
              bool flag = true;
              if (module is RendererModuleUI && (UnityEngine.Object) emitter.GetParticleSystemRenderer() != (UnityEngine.Object) null)
                flag = false;
              if (flag && !module.enabled)
                module.visibleUI = false;
            }
          }
        }
      }
    }

    internal int GetNumEnabledRenderers()
    {
      int num = 0;
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        ModuleUI rendererModuleUi = emitter.GetParticleSystemRendererModuleUI();
        if (rendererModuleUi != null && rendererModuleUi.enabled)
          ++num;
      }
      return num;
    }

    internal bool IsShowOnlySelectedMode()
    {
      return this.m_ShowOnlySelectedMode;
    }

    internal void SetShowOnlySelectedMode(bool enable)
    {
      this.m_ShowOnlySelectedMode = enable;
      this.RefreshShowOnlySelected();
    }

    internal void RefreshShowOnlySelected()
    {
      if (this.IsShowOnlySelectedMode())
      {
        int[] instanceIds = Selection.instanceIDs;
        foreach (ParticleSystemUI emitter in this.m_Emitters)
        {
          ParticleSystemRenderer particleSystemRenderer = emitter.GetParticleSystemRenderer();
          if ((UnityEngine.Object) particleSystemRenderer != (UnityEngine.Object) null)
            particleSystemRenderer.editorEnabled = ((IEnumerable<int>) instanceIds).Contains<int>(emitter.m_ParticleSystem.gameObject.GetInstanceID());
        }
      }
      else
      {
        foreach (ParticleSystemUI emitter in this.m_Emitters)
        {
          ParticleSystemRenderer particleSystemRenderer = emitter.GetParticleSystemRenderer();
          if ((UnityEngine.Object) particleSystemRenderer != (UnityEngine.Object) null)
            particleSystemRenderer.editorEnabled = true;
        }
      }
    }

    private enum PlayState
    {
      Stopped,
      Playing,
      Paused,
    }

    private enum OwnerType
    {
      Inspector,
      ParticleSystemWindow,
    }

    internal class Texts
    {
      public GUIContent previewSpeed = new GUIContent("Playback Speed");
      public GUIContent previewTime = new GUIContent("Playback Time");
      public GUIContent particleCount = new GUIContent("Particle Count");
      public GUIContent play = new GUIContent("Simulate");
      public GUIContent stop = new GUIContent("Stop");
      public GUIContent pause = new GUIContent("Pause");
      public GUIContent addParticleSystem = new GUIContent(string.Empty, "Create Particle System");
      public GUIContent wireframe = new GUIContent("Wireframe", "Show particles with wireframe and particle system bounds");
      public GUIContent resimulation = new GUIContent("Resimulate", "If resimulate is enabled the particle system will show changes made to the system immediately (including changes made to the particle system transform)");
      public string secondsFloatFieldFormatString = "f2";
    }
  }
}
