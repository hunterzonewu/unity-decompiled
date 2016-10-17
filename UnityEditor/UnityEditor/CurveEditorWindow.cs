// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditorWindow : EditorWindow
  {
    private static GUIContent s_WrapModeMenuIcon = EditorGUIUtility.IconContent("AnimationWrapModeMenu");
    private GUIContent m_GUIContent = new GUIContent();
    private const int kPresetsHeight = 46;
    private static CurveEditorWindow s_SharedCurveEditor;
    private CurveEditor m_CurveEditor;
    private AnimationCurve m_Curve;
    private Color m_Color;
    private CurvePresetsContentsForPopupWindow m_CurvePresets;
    [SerializeField]
    private GUIView delegateView;
    internal static CurveEditorWindow.Styles ms_Styles;

    public static CurveEditorWindow instance
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor))
          CurveEditorWindow.s_SharedCurveEditor = ScriptableObject.CreateInstance<CurveEditorWindow>();
        return CurveEditorWindow.s_SharedCurveEditor;
      }
    }

    public string currentPresetLibrary
    {
      get
      {
        this.InitCurvePresets();
        return this.m_CurvePresets.currentPresetLibrary;
      }
      set
      {
        this.InitCurvePresets();
        this.m_CurvePresets.currentPresetLibrary = value;
      }
    }

    public static AnimationCurve curve
    {
      get
      {
        return CurveEditorWindow.instance.m_Curve;
      }
      set
      {
        if (value == null)
        {
          CurveEditorWindow.instance.m_Curve = (AnimationCurve) null;
        }
        else
        {
          CurveEditorWindow.instance.m_Curve = value;
          CurveEditorWindow.instance.RefreshShownCurves();
        }
      }
    }

    public static Color color
    {
      get
      {
        return CurveEditorWindow.instance.m_Color;
      }
      set
      {
        CurveEditorWindow.instance.m_Color = value;
        CurveEditorWindow.instance.RefreshShownCurves();
      }
    }

    public static bool visible
    {
      get
      {
        return (UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor != (UnityEngine.Object) null;
      }
    }

    private CurveLibraryType curveLibraryType
    {
      get
      {
        return this.m_CurveEditor.settings.hasUnboundedRanges ? CurveLibraryType.Unbounded : CurveLibraryType.NormalizedZeroToOne;
      }
    }

    private void OnEnable()
    {
      CurveEditorWindow.s_SharedCurveEditor = this;
      this.Init((CurveEditorSettings) null);
    }

    private void Init(CurveEditorSettings settings)
    {
      this.m_CurveEditor = new CurveEditor(this.GetCurveEditorRect(), this.GetCurveWrapperArray(), true);
      this.m_CurveEditor.curvesUpdated = new CurveEditor.CallbackFunction(this.UpdateCurve);
      this.m_CurveEditor.scaleWithWindow = true;
      this.m_CurveEditor.margin = 40f;
      if (settings != null)
        this.m_CurveEditor.settings = settings;
      this.m_CurveEditor.settings.hTickLabelOffset = 10f;
      bool horizontally = true;
      bool vertically = true;
      if ((double) this.m_CurveEditor.settings.hRangeMin != double.NegativeInfinity && (double) this.m_CurveEditor.settings.hRangeMax != double.PositiveInfinity)
      {
        this.m_CurveEditor.SetShownHRangeInsideMargins(this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.hRangeMax);
        horizontally = false;
      }
      if ((double) this.m_CurveEditor.settings.vRangeMin != double.NegativeInfinity && (double) this.m_CurveEditor.settings.vRangeMax != double.PositiveInfinity)
      {
        this.m_CurveEditor.SetShownVRangeInsideMargins(this.m_CurveEditor.settings.vRangeMin, this.m_CurveEditor.settings.vRangeMax);
        vertically = false;
      }
      this.m_CurveEditor.FrameSelected(horizontally, vertically);
    }

    private bool GetNormalizationRect(out Rect normalizationRect)
    {
      normalizationRect = new Rect();
      if (this.m_CurveEditor.settings.hasUnboundedRanges)
        return false;
      normalizationRect = new Rect(this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.vRangeMin, this.m_CurveEditor.settings.hRangeMax - this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.vRangeMax - this.m_CurveEditor.settings.vRangeMin);
      return true;
    }

    private static Keyframe[] CopyAndScaleCurveKeys(Keyframe[] orgKeys, Rect rect, bool realToNormalized)
    {
      if ((double) rect.width == 0.0 || (double) rect.height == 0.0 || (float.IsInfinity(rect.width) || float.IsInfinity(rect.height)))
      {
        Debug.LogError((object) ("CopyAndScaleCurve: Invalid scale: " + (object) rect));
        return orgKeys;
      }
      Keyframe[] keyframeArray = new Keyframe[orgKeys.Length];
      if (realToNormalized)
      {
        for (int index = 0; index < keyframeArray.Length; ++index)
        {
          keyframeArray[index].time = (orgKeys[index].time - rect.xMin) / rect.width;
          keyframeArray[index].value = (orgKeys[index].value - rect.yMin) / rect.height;
        }
      }
      else
      {
        for (int index = 0; index < keyframeArray.Length; ++index)
        {
          keyframeArray[index].time = orgKeys[index].time * rect.width + rect.xMin;
          keyframeArray[index].value = orgKeys[index].value * rect.height + rect.yMin;
        }
      }
      return keyframeArray;
    }

    private void InitCurvePresets()
    {
      if (this.m_CurvePresets != null)
        return;
      this.m_CurvePresets = new CurvePresetsContentsForPopupWindow((AnimationCurve) null, this.curveLibraryType, (System.Action<AnimationCurve>) (presetCurve =>
      {
        this.ValidateCurveLibraryTypeAndScale();
        Rect normalizationRect;
        if (this.GetNormalizationRect(out normalizationRect))
        {
          bool realToNormalized = false;
          this.m_Curve.keys = CurveEditorWindow.CopyAndScaleCurveKeys(presetCurve.keys, normalizationRect, realToNormalized);
        }
        else
          this.m_Curve.keys = presetCurve.keys;
        this.m_Curve.postWrapMode = presetCurve.postWrapMode;
        this.m_Curve.preWrapMode = presetCurve.preWrapMode;
        this.m_CurveEditor.SelectNone();
        this.RefreshShownCurves();
        this.SendEvent("CurveChanged", true);
      }));
      this.m_CurvePresets.InitIfNeeded();
    }

    private void OnDestroy()
    {
      this.m_CurvePresets.GetPresetLibraryEditor().UnloadUsedLibraries();
    }

    private void OnDisable()
    {
      this.m_CurveEditor.OnDisable();
      if ((UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor == (UnityEngine.Object) this)
        CurveEditorWindow.s_SharedCurveEditor = (CurveEditorWindow) null;
      else if (!this.Equals((object) CurveEditorWindow.s_SharedCurveEditor))
        throw new ApplicationException("s_SharedCurveEditor does not equal this");
    }

    private void RefreshShownCurves()
    {
      this.m_CurveEditor.animationCurves = this.GetCurveWrapperArray();
    }

    public void Show(GUIView viewToUpdate, CurveEditorSettings settings)
    {
      this.delegateView = viewToUpdate;
      this.Init(settings);
      this.ShowAuxWindow();
      this.titleContent = new GUIContent("Curve");
      this.minSize = new Vector2(240f, 286f);
      this.maxSize = new Vector2(10000f, 10000f);
    }

    private CurveWrapper[] GetCurveWrapperArray()
    {
      if (this.m_Curve == null)
        return new CurveWrapper[0];
      CurveWrapper curveWrapper = new CurveWrapper();
      curveWrapper.id = "Curve".GetHashCode();
      curveWrapper.groupId = -1;
      curveWrapper.color = this.m_Color;
      curveWrapper.hidden = false;
      curveWrapper.readOnly = false;
      curveWrapper.renderer = (CurveRenderer) new NormalCurveRenderer(this.m_Curve);
      curveWrapper.renderer.SetWrap(this.m_Curve.preWrapMode, this.m_Curve.postWrapMode);
      return new CurveWrapper[1]
      {
        curveWrapper
      };
    }

    private Rect GetCurveEditorRect()
    {
      return new Rect(0.0f, 0.0f, this.position.width, this.position.height - 46f);
    }

    internal static Keyframe[] GetLinearKeys()
    {
      Keyframe[] keyframeArray = new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f, 1f, 1f),
        new Keyframe(1f, 1f, 1f, 1f)
      };
      for (int index = 0; index < 2; ++index)
      {
        CurveUtility.SetKeyBroken(ref keyframeArray[index], false);
        CurveUtility.SetKeyTangentMode(ref keyframeArray[index], 0, TangentMode.Smooth);
        CurveUtility.SetKeyTangentMode(ref keyframeArray[index], 1, TangentMode.Smooth);
      }
      return keyframeArray;
    }

    internal static Keyframe[] GetLinearMirrorKeys()
    {
      Keyframe[] keyframeArray = new Keyframe[2]
      {
        new Keyframe(0.0f, 1f, -1f, -1f),
        new Keyframe(1f, 0.0f, -1f, -1f)
      };
      for (int index = 0; index < 2; ++index)
      {
        CurveUtility.SetKeyBroken(ref keyframeArray[index], false);
        CurveUtility.SetKeyTangentMode(ref keyframeArray[index], 0, TangentMode.Smooth);
        CurveUtility.SetKeyTangentMode(ref keyframeArray[index], 1, TangentMode.Smooth);
      }
      return keyframeArray;
    }

    internal static Keyframe[] GetEaseInKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f, 0.0f, 0.0f),
        new Keyframe(1f, 1f, 2f, 2f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 1f, -2f, -2f),
        new Keyframe(1f, 0.0f, 0.0f, 0.0f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseOutKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f, 2f, 2f),
        new Keyframe(1f, 1f, 0.0f, 0.0f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseOutMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 1f, 0.0f, 0.0f),
        new Keyframe(1f, 0.0f, -2f, -2f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInOutKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f, 0.0f, 0.0f),
        new Keyframe(1f, 1f, 0.0f, 0.0f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInOutMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, 1f, 0.0f, 0.0f),
        new Keyframe(1f, 0.0f, 0.0f, 0.0f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetConstantKeys(float value)
    {
      Keyframe[] keys = new Keyframe[2]
      {
        new Keyframe(0.0f, value, 0.0f, 0.0f),
        new Keyframe(1f, value, 0.0f, 0.0f)
      };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static void SetSmoothEditable(ref Keyframe[] keys)
    {
      for (int index = 0; index < keys.Length; ++index)
      {
        CurveUtility.SetKeyBroken(ref keys[index], false);
        CurveUtility.SetKeyTangentMode(ref keys[index], 0, TangentMode.Editable);
        CurveUtility.SetKeyTangentMode(ref keys[index], 1, TangentMode.Editable);
      }
    }

    private void OnGUI()
    {
      bool flag = Event.current.type == EventType.MouseUp;
      if ((UnityEngine.Object) this.delegateView == (UnityEngine.Object) null)
        this.m_Curve = (AnimationCurve) null;
      if (CurveEditorWindow.ms_Styles == null)
        CurveEditorWindow.ms_Styles = new CurveEditorWindow.Styles();
      this.m_CurveEditor.rect = this.GetCurveEditorRect();
      this.m_CurveEditor.hRangeLocked = Event.current.shift;
      this.m_CurveEditor.vRangeLocked = EditorGUI.actionKey;
      GUI.changed = false;
      GUI.Label(this.m_CurveEditor.drawRect, GUIContent.none, CurveEditorWindow.ms_Styles.curveEditorBackground);
      this.m_CurveEditor.BeginViewGUI();
      this.m_CurveEditor.GridGUI();
      this.m_CurveEditor.CurveGUI();
      this.DoWrapperPopups();
      this.m_CurveEditor.EndViewGUI();
      GUI.Box(new Rect(0.0f, this.position.height - 46f, this.position.width, 46f), string.Empty, CurveEditorWindow.ms_Styles.curveSwatchArea);
      this.m_Color.a *= 0.6f;
      float y = (float) ((double) this.position.height - 46.0 + 10.5);
      this.InitCurvePresets();
      CurvePresetLibrary currentLib = this.m_CurvePresets.GetPresetLibraryEditor().GetCurrentLib();
      if ((UnityEngine.Object) currentLib != (UnityEngine.Object) null)
      {
        for (int index = 0; index < currentLib.Count(); ++index)
        {
          Rect rect = new Rect((float) (45.0 + 45.0 * (double) index), y, 40f, 25f);
          this.m_GUIContent.tooltip = currentLib.GetName(index);
          if (GUI.Button(rect, this.m_GUIContent, CurveEditorWindow.ms_Styles.curveSwatch))
          {
            AnimationCurve preset = currentLib.GetPreset(index) as AnimationCurve;
            this.m_Curve.keys = preset.keys;
            this.m_Curve.postWrapMode = preset.postWrapMode;
            this.m_Curve.preWrapMode = preset.preWrapMode;
            this.m_CurveEditor.SelectNone();
            this.SendEvent("CurveChanged", true);
          }
          if (Event.current.type == EventType.Repaint)
            currentLib.Draw(rect, index);
          if ((double) rect.xMax > (double) this.position.width - 90.0)
            break;
        }
      }
      this.PresetDropDown(new Rect(25f, y + 5f, 20f, 20f));
      if (Event.current.type == EventType.Used && flag)
      {
        this.DoUpdateCurve(false);
        this.SendEvent("CurveChangeCompleted", true);
      }
      else
      {
        if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
          return;
        this.DoUpdateCurve(true);
      }
    }

    private void PresetDropDown(Rect rect)
    {
      if (!EditorGUI.ButtonMouseDown(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Native, EditorStyles.inspectorTitlebarText) || this.m_Curve == null)
        return;
      if (this.m_CurvePresets == null)
      {
        Debug.LogError((object) "Curve presets error");
      }
      else
      {
        this.ValidateCurveLibraryTypeAndScale();
        AnimationCurve animationCurve = new AnimationCurve();
        Rect normalizationRect;
        if (this.GetNormalizationRect(out normalizationRect))
        {
          bool realToNormalized = true;
          animationCurve.keys = CurveEditorWindow.CopyAndScaleCurveKeys(this.m_Curve.keys, normalizationRect, realToNormalized);
        }
        else
          animationCurve = new AnimationCurve(this.m_Curve.keys);
        animationCurve.postWrapMode = this.m_Curve.postWrapMode;
        animationCurve.preWrapMode = this.m_Curve.preWrapMode;
        this.m_CurvePresets.curveToSaveAsPreset = animationCurve;
        PopupWindow.Show(rect, (PopupWindowContent) this.m_CurvePresets);
      }
    }

    private void ValidateCurveLibraryTypeAndScale()
    {
      Rect normalizationRect;
      if (this.GetNormalizationRect(out normalizationRect))
      {
        if (this.curveLibraryType == CurveLibraryType.NormalizedZeroToOne)
          return;
        Debug.LogError((object) ("When having a normalize rect we should be using curve library type: NormalizedZeroToOne (normalizationRect: " + (object) normalizationRect + ")"));
      }
      else
      {
        if (this.curveLibraryType == CurveLibraryType.Unbounded)
          return;
        Debug.LogError((object) "When NOT having a normalize rect we should be using library type: Unbounded");
      }
    }

    public void UpdateCurve()
    {
      this.DoUpdateCurve(false);
    }

    private void DoUpdateCurve(bool exitGUI)
    {
      if (this.m_CurveEditor.animationCurves.Length <= 0 || this.m_CurveEditor.animationCurves[0] == null || !this.m_CurveEditor.animationCurves[0].changed)
        return;
      this.m_CurveEditor.animationCurves[0].changed = false;
      this.SendEvent("CurveChanged", exitGUI);
    }

    private void DoWrapperPopups()
    {
      if (this.m_Curve == null || this.m_Curve.length < 2 || this.m_Curve.preWrapMode == WrapMode.Default)
        return;
      Color contentColor = GUI.contentColor;
      GUI.contentColor = Color.white;
      WrapMode wrapMode1 = this.WrapModeIconPopup(this.m_Curve.keys[0], this.m_Curve == null ? WrapMode.Default : this.m_Curve.preWrapMode, -1.5f);
      if (this.m_Curve != null && wrapMode1 != this.m_Curve.preWrapMode)
      {
        this.m_Curve.preWrapMode = wrapMode1;
        this.RefreshShownCurves();
        this.SendEvent("CurveChanged", true);
      }
      WrapMode wrapMode2 = this.WrapModeIconPopup(this.m_Curve.keys[this.m_Curve.length - 1], this.m_Curve == null ? WrapMode.Default : this.m_Curve.postWrapMode, 0.5f);
      if (this.m_Curve != null && wrapMode2 != this.m_Curve.postWrapMode)
      {
        this.m_Curve.postWrapMode = wrapMode2;
        this.RefreshShownCurves();
        this.SendEvent("CurveChanged", true);
      }
      GUI.contentColor = contentColor;
    }

    private WrapMode WrapModeIconPopup(Keyframe key, WrapMode oldWrap, float hOffset)
    {
      float width = (float) CurveEditorWindow.s_WrapModeMenuIcon.image.width;
      Vector3 viewTransformPoint = this.m_CurveEditor.DrawingToViewTransformPoint(new Vector3(key.time, key.value));
      Rect position = new Rect(viewTransformPoint.x + width * hOffset, viewTransformPoint.y, width, width);
      WrapModeFixedCurve wrapModeFixedCurve = (WrapModeFixedCurve) oldWrap;
      Enum[] array1 = Enum.GetValues(typeof (WrapModeFixedCurve)).Cast<Enum>().ToArray<Enum>();
      string[] array2 = ((IEnumerable<string>) Enum.GetNames(typeof (WrapModeFixedCurve))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      int selected = Array.IndexOf<Enum>(array1, (Enum) wrapModeFixedCurve);
      int controlId = GUIUtility.GetControlID("WrapModeIconPopup".GetHashCode(), EditorGUIUtility.native, position);
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, selected);
      GUIContent[] options = EditorGUIUtility.TempContent(array2);
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(controlId))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selectedValueForControl * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlId);
            EditorUtility.DisplayCustomMenu(position, options, selectedValueForControl, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          GUIStyle.none.Draw(position, CurveEditorWindow.s_WrapModeMenuIcon, controlId, false);
          break;
        default:
          if (type == EventType.MouseDown && current.button == 0 && position.Contains(current.mousePosition))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selectedValueForControl * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlId);
            EditorUtility.DisplayCustomMenu(position, options, selectedValueForControl, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            GUIUtility.keyboardControl = controlId;
            current.Use();
            break;
          }
          break;
      }
      return (WrapMode) array1[selectedValueForControl];
    }

    private void SendEvent(string eventName, bool exitGUI)
    {
      if ((bool) ((UnityEngine.Object) this.delegateView))
      {
        Event e = EditorGUIUtility.CommandEvent(eventName);
        this.Repaint();
        this.delegateView.SendEvent(e);
        if (exitGUI)
          GUIUtility.ExitGUI();
      }
      GUI.changed = true;
    }

    internal class Styles
    {
      public GUIStyle curveEditorBackground = (GUIStyle) "PopupCurveEditorBackground";
      public GUIStyle miniToolbarPopup = (GUIStyle) "MiniToolbarPopup";
      public GUIStyle miniToolbarButton = (GUIStyle) "MiniToolbarButtonLeft";
      public GUIStyle curveSwatch = (GUIStyle) "PopupCurveEditorSwatch";
      public GUIStyle curveSwatchArea = (GUIStyle) "PopupCurveSwatchBackground";
      public GUIStyle curveWrapPopup = (GUIStyle) "PopupCurveDropdown";
    }
  }
}
