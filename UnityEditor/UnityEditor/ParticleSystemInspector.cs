// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ParticleSystem))]
  internal class ParticleSystemInspector : Editor, ParticleEffectUIOwner
  {
    private GUIContent m_PreviewTitle = new GUIContent("Particle System Curves");
    private GUIContent showWindowText = new GUIContent("Open Editor...");
    private GUIContent closeWindowText = new GUIContent("Close Editor");
    private GUIContent hideWindowText = new GUIContent("Hide Editor");
    private ParticleEffectUI m_ParticleEffectUI;
    private static GUIContent m_PlayBackTitle;

    public static GUIContent playBackTitle
    {
      get
      {
        if (ParticleSystemInspector.m_PlayBackTitle == null)
          ParticleSystemInspector.m_PlayBackTitle = new GUIContent("Particle Effect");
        return ParticleSystemInspector.m_PlayBackTitle;
      }
    }

    public void OnEnable()
    {
      EditorApplication.hierarchyWindowChanged += new EditorApplication.CallbackFunction(this.HierarchyOrProjectWindowWasChanged);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.HierarchyOrProjectWindowWasChanged);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnDisable()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.HierarchyOrProjectWindowWasChanged);
      EditorApplication.hierarchyWindowChanged -= new EditorApplication.CallbackFunction(this.HierarchyOrProjectWindowWasChanged);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      if (this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.Clear();
    }

    private void HierarchyOrProjectWindowWasChanged()
    {
      if (!this.ShouldShowInspector())
        return;
      this.Init(true);
    }

    private void UndoRedoPerformed()
    {
      if (this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.UndoRedoPerformed();
    }

    private void Init(bool forceInit)
    {
      ParticleSystem target = this.target as ParticleSystem;
      if ((Object) target == (Object) null)
        return;
      if (this.m_ParticleEffectUI == null)
      {
        this.m_ParticleEffectUI = new ParticleEffectUI((ParticleEffectUIOwner) this);
        this.m_ParticleEffectUI.InitializeIfNeeded(target);
      }
      else
      {
        if (!forceInit)
          return;
        this.m_ParticleEffectUI.InitializeIfNeeded(target);
      }
    }

    private void ShowEdiorButtonGUI()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      ParticleSystemWindow instance = ParticleSystemWindow.GetInstance();
      if (GUILayout.Button(!(bool) ((Object) instance) || !instance.IsVisible() ? this.showWindowText : (instance.GetNumTabs() <= 1 ? this.closeWindowText : this.hideWindowText), EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.Width(110f) }))
      {
        if ((bool) ((Object) instance))
        {
          if (instance.IsVisible())
          {
            if (!instance.ShowNextTabIfPossible())
              instance.Close();
          }
          else
            instance.Focus();
        }
        else
        {
          this.Clear();
          ParticleSystemWindow.CreateWindow();
          GUIUtility.ExitGUI();
        }
      }
      GUILayout.EndHorizontal();
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
      this.ShowEdiorButtonGUI();
      if (this.ShouldShowInspector())
      {
        if (this.m_ParticleEffectUI == null)
          this.Init(true);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
        this.m_ParticleEffectUI.OnGUI();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
      }
      else
        this.Clear();
      EditorGUILayout.EndVertical();
    }

    private void Clear()
    {
      if (this.m_ParticleEffectUI != null)
        this.m_ParticleEffectUI.Clear();
      this.m_ParticleEffectUI = (ParticleEffectUI) null;
    }

    private bool ShouldShowInspector()
    {
      ParticleSystemWindow instance = ParticleSystemWindow.GetInstance();
      if ((bool) ((Object) instance))
        return !instance.IsVisible();
      return true;
    }

    public void OnSceneGUI()
    {
      if (!this.ShouldShowInspector() || this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.OnSceneGUI();
    }

    public void OnSceneViewGUI(SceneView sceneView)
    {
      if (!this.ShouldShowInspector())
        return;
      this.Init(false);
      if (this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.OnSceneViewGUI();
    }

    public override bool HasPreviewGUI()
    {
      if (this.ShouldShowInspector())
        return Selection.objects.Length == 1;
      return false;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.GetParticleSystemCurveEditor().OnGUI(r);
    }

    public override GUIContent GetPreviewTitle()
    {
      return this.m_PreviewTitle;
    }

    public override void OnPreviewSettings()
    {
    }

    void ParticleEffectUIOwner.Repaint()
    {
      this.Repaint();
    }
  }
}
