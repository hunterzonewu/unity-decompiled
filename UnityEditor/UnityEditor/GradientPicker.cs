// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientPicker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class GradientPicker : EditorWindow
  {
    private const int k_DefaultNumSteps = 0;
    private static GradientPicker s_GradientPicker;
    private GradientEditor m_GradientEditor;
    private PresetLibraryEditor<GradientPresetLibrary> m_GradientLibraryEditor;
    [SerializeField]
    private PresetLibraryEditorState m_GradientLibraryEditorState;
    private Gradient m_Gradient;
    private GUIView m_DelegateView;

    public static string presetsEditorPrefID
    {
      get
      {
        return "Gradient";
      }
    }

    private bool gradientChanged { get; set; }

    public static GradientPicker instance
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) GradientPicker.s_GradientPicker))
          Debug.LogError((object) "Gradient Picker not initalized, did you call Show first?");
        return GradientPicker.s_GradientPicker;
      }
    }

    public string currentPresetLibrary
    {
      get
      {
        this.InitIfNeeded();
        return this.m_GradientLibraryEditor.currentLibraryWithoutExtension;
      }
      set
      {
        this.InitIfNeeded();
        this.m_GradientLibraryEditor.currentLibraryWithoutExtension = value;
      }
    }

    public static bool visible
    {
      get
      {
        return (UnityEngine.Object) GradientPicker.s_GradientPicker != (UnityEngine.Object) null;
      }
    }

    public static Gradient gradient
    {
      get
      {
        if ((UnityEngine.Object) GradientPicker.s_GradientPicker != (UnityEngine.Object) null)
          return GradientPicker.s_GradientPicker.m_Gradient;
        return (Gradient) null;
      }
    }

    private GradientPicker()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    public static void Show(Gradient newGradient)
    {
      GUIView current = GUIView.current;
      if ((UnityEngine.Object) GradientPicker.s_GradientPicker == (UnityEngine.Object) null)
      {
        GradientPicker.s_GradientPicker = (GradientPicker) EditorWindow.GetWindow(typeof (GradientPicker), true, "Gradient Editor", false);
        Vector2 vector2_1 = new Vector2(360f, 224f);
        Vector2 vector2_2 = new Vector2(1900f, 3000f);
        GradientPicker.s_GradientPicker.minSize = vector2_1;
        GradientPicker.s_GradientPicker.maxSize = vector2_2;
        GradientPicker.s_GradientPicker.wantsMouseMove = true;
        GradientPicker.s_GradientPicker.ShowAuxWindow();
      }
      else
        GradientPicker.s_GradientPicker.Repaint();
      GradientPicker.s_GradientPicker.m_DelegateView = current;
      GradientPicker.s_GradientPicker.Init(newGradient);
    }

    private void Init(Gradient newGradient)
    {
      this.m_Gradient = newGradient;
      if (this.m_GradientEditor != null)
        this.m_GradientEditor.Init(newGradient, 0);
      this.Repaint();
    }

    private void SetGradientData(Gradient gradient)
    {
      this.m_Gradient.colorKeys = gradient.colorKeys;
      this.m_Gradient.alphaKeys = gradient.alphaKeys;
      this.Init(this.m_Gradient);
    }

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
      if (this.m_GradientLibraryEditorState != null)
        this.m_GradientLibraryEditorState.TransferEditorPrefsState(false);
      if (this.m_GradientEditor != null)
        this.m_GradientEditor.Clear();
      GradientPicker.s_GradientPicker = (GradientPicker) null;
    }

    public void OnDestroy()
    {
      this.m_GradientLibraryEditor.UnloadUsedLibraries();
    }

    private void OnPlayModeStateChanged()
    {
      this.Close();
    }

    private void InitIfNeeded()
    {
      if (this.m_GradientEditor == null)
      {
        this.m_GradientEditor = new GradientEditor();
        this.m_GradientEditor.Init(this.m_Gradient, 0);
      }
      if (this.m_GradientLibraryEditorState == null)
      {
        this.m_GradientLibraryEditorState = new PresetLibraryEditorState(GradientPicker.presetsEditorPrefID);
        this.m_GradientLibraryEditorState.TransferEditorPrefsState(true);
      }
      if (this.m_GradientLibraryEditor != null)
        return;
      this.m_GradientLibraryEditor = new PresetLibraryEditor<GradientPresetLibrary>(new ScriptableObjectSaveLoadHelper<GradientPresetLibrary>("gradients", SaveType.Text), this.m_GradientLibraryEditorState, new System.Action<int, object>(this.PresetClickedCallback));
      this.m_GradientLibraryEditor.showHeader = true;
      this.m_GradientLibraryEditor.minMaxPreviewHeight = new Vector2(14f, 14f);
    }

    private void PresetClickedCallback(int clickCount, object presetObject)
    {
      Gradient gradient = presetObject as Gradient;
      if (gradient == null)
        Debug.LogError((object) ("Incorrect object passed " + presetObject));
      GradientPicker.SetCurrentGradient(gradient);
      this.gradientChanged = true;
    }

    public void OnGUI()
    {
      if (this.m_Gradient == null)
        return;
      this.InitIfNeeded();
      float num1 = Mathf.Min(this.position.height, 120f);
      float num2 = 10f;
      float height = this.position.height - num1 - num2;
      Rect position = new Rect(10f, 10f, this.position.width - 20f, num1 - 20f);
      Rect rect = new Rect(0.0f, num1 + num2, this.position.width, height);
      EditorGUI.DrawRect(new Rect(rect.x, rect.y - 1f, rect.width, 1f), new Color(0.0f, 0.0f, 0.0f, 0.3f));
      EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), new Color(1f, 1f, 1f, 0.1f));
      EditorGUI.BeginChangeCheck();
      this.m_GradientEditor.OnGUI(position);
      if (EditorGUI.EndChangeCheck())
        this.gradientChanged = true;
      this.m_GradientLibraryEditor.OnGUI(rect, (object) this.m_Gradient);
      if (!this.gradientChanged)
        return;
      this.gradientChanged = false;
      this.SendEvent(true);
    }

    private void SendEvent(bool exitGUI)
    {
      if (!(bool) ((UnityEngine.Object) this.m_DelegateView))
        return;
      Event e = EditorGUIUtility.CommandEvent("GradientPickerChanged");
      this.Repaint();
      this.m_DelegateView.SendEvent(e);
      if (!exitGUI)
        return;
      GUIUtility.ExitGUI();
    }

    public static void SetCurrentGradient(Gradient gradient)
    {
      if ((UnityEngine.Object) GradientPicker.s_GradientPicker == (UnityEngine.Object) null)
        return;
      GradientPicker.s_GradientPicker.SetGradientData(gradient);
      GUI.changed = true;
    }

    public static void CloseWindow()
    {
      if ((UnityEngine.Object) GradientPicker.s_GradientPicker == (UnityEngine.Object) null)
        return;
      GradientPicker.s_GradientPicker.Close();
      GUIUtility.ExitGUI();
    }

    public static void RepaintWindow()
    {
      if ((UnityEngine.Object) GradientPicker.s_GradientPicker == (UnityEngine.Object) null)
        return;
      GradientPicker.s_GradientPicker.Repaint();
    }
  }
}
