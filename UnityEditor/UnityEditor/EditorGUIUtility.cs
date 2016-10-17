// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUIUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Miscellaneous helper stuff for EditorGUI.</para>
  /// </summary>
  public sealed class EditorGUIUtility : GUIUtility
  {
    internal static int s_FontIsBold = -1;
    internal static SliderLabels sliderLabels = new SliderLabels();
    internal static Color kDarkViewBackground = new Color(0.22f, 0.22f, 0.22f, 0.0f);
    private static GUIContent s_ObjectContent = new GUIContent();
    private static GUIContent s_Text = new GUIContent();
    private static GUIContent s_Image = new GUIContent();
    private static GUIContent s_TextImage = new GUIContent();
    private static GUIContent s_BlankContent = new GUIContent(" ");
    private static Hashtable s_TextGUIContents = new Hashtable();
    private static Hashtable s_IconGUIContents = new Hashtable();
    internal static int s_LastControlID = 0;
    private static bool s_HierarchyMode = false;
    internal static bool s_WideMode = false;
    private static float s_ContextWidth = 0.0f;
    private static float s_LabelWidth = 0.0f;
    private static float s_FieldWidth = 0.0f;
    public static FocusType native = FocusType.Keyboard;
    private static Texture2D s_InfoIcon;
    private static Texture2D s_WarningIcon;
    private static Texture2D s_ErrorIcon;
    private static GUIStyle s_WhiteTextureStyle;
    private static GUIStyle s_BasicTextureStyle;

    /// <summary>
    ///   <para>Get the height used for a single Editor control such as a one-line EditorGUI.TextField or EditorGUI.Popup.</para>
    /// </summary>
    public static float singleLineHeight
    {
      get
      {
        return 16f;
      }
    }

    /// <summary>
    ///   <para>Get the height used by default for vertical spacing between controls.</para>
    /// </summary>
    public static float standardVerticalSpacing
    {
      get
      {
        return 2f;
      }
    }

    /// <summary>
    ///   <para>Is the user currently using the pro skin? (Read Only)</para>
    /// </summary>
    public static bool isProSkin
    {
      get
      {
        return EditorGUIUtility.skinIndex == 1;
      }
    }

    internal static extern int skinIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get a white texture.</para>
    /// </summary>
    public static extern Texture2D whiteTexture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static Texture2D infoIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_InfoIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_InfoIcon = EditorGUIUtility.LoadIcon("console.infoicon");
        return EditorGUIUtility.s_InfoIcon;
      }
    }

    internal static Texture2D warningIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_WarningIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
        return EditorGUIUtility.s_WarningIcon;
      }
    }

    internal static Texture2D errorIcon
    {
      get
      {
        if ((UnityEngine.Object) EditorGUIUtility.s_ErrorIcon == (UnityEngine.Object) null)
          EditorGUIUtility.s_ErrorIcon = EditorGUIUtility.LoadIcon("console.erroricon");
        return EditorGUIUtility.s_ErrorIcon;
      }
    }

    internal static GUIContent blankContent
    {
      get
      {
        return EditorGUIUtility.s_BlankContent;
      }
    }

    internal static GUIStyle whiteTextureStyle
    {
      get
      {
        if (EditorGUIUtility.s_WhiteTextureStyle == null)
        {
          EditorGUIUtility.s_WhiteTextureStyle = new GUIStyle();
          EditorGUIUtility.s_WhiteTextureStyle.normal.background = EditorGUIUtility.whiteTexture;
        }
        return EditorGUIUtility.s_WhiteTextureStyle;
      }
    }

    /// <summary>
    ///   <para>Is a text field currently editing text?</para>
    /// </summary>
    public static bool editingTextField
    {
      get
      {
        return EditorGUI.RecycledTextEditor.s_ActuallyEditing;
      }
      set
      {
        EditorGUI.RecycledTextEditor.s_ActuallyEditing = value;
      }
    }

    /// <summary>
    ///   <para>Is the Editor GUI is hierarchy mode?</para>
    /// </summary>
    public static bool hierarchyMode
    {
      get
      {
        return EditorGUIUtility.s_HierarchyMode;
      }
      set
      {
        EditorGUIUtility.s_HierarchyMode = value;
      }
    }

    /// <summary>
    ///   <para>Is the Editor GUI currently in wide mode?</para>
    /// </summary>
    public static bool wideMode
    {
      get
      {
        return EditorGUIUtility.s_WideMode;
      }
      set
      {
        EditorGUIUtility.s_WideMode = value;
      }
    }

    internal static float contextWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_ContextWidth > 0.0)
          return EditorGUIUtility.s_ContextWidth;
        return EditorGUIUtility.CalcContextWidth();
      }
    }

    /// <summary>
    ///   <para>The width of the GUI area for the current EditorWindow or other view.</para>
    /// </summary>
    public static float currentViewWidth
    {
      get
      {
        return GUIView.current.position.width;
      }
    }

    /// <summary>
    ///   <para>The width in pixels reserved for labels of Editor GUI controls.</para>
    /// </summary>
    public static float labelWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_LabelWidth > 0.0)
          return EditorGUIUtility.s_LabelWidth;
        if (EditorGUIUtility.s_HierarchyMode)
          return Mathf.Max((float) ((double) EditorGUIUtility.contextWidth * 0.449999988079071 - 40.0), 120f);
        return 150f;
      }
      set
      {
        EditorGUIUtility.s_LabelWidth = value;
      }
    }

    /// <summary>
    ///   <para>The minimum width in pixels reserved for the fields of Editor GUI controls.</para>
    /// </summary>
    public static float fieldWidth
    {
      get
      {
        if ((double) EditorGUIUtility.s_FieldWidth > 0.0)
          return EditorGUIUtility.s_FieldWidth;
        return 50f;
      }
      set
      {
        EditorGUIUtility.s_FieldWidth = value;
      }
    }

    /// <summary>
    ///   <para>The system copy buffer.</para>
    /// </summary>
    public new static extern string systemCopyBuffer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static EventType magnifyGestureEventType
    {
      get
      {
        return (EventType) 1000;
      }
    }

    internal static EventType swipeGestureEventType
    {
      get
      {
        return (EventType) 1001;
      }
    }

    internal static EventType rotateGestureEventType
    {
      get
      {
        return (EventType) 1002;
      }
    }

    internal new static float pixelsPerPoint
    {
      get
      {
        return GUIUtility.pixelsPerPoint;
      }
    }

    static EditorGUIUtility()
    {
      GUISkin.m_SkinChanged += new GUISkin.SkinChangedDelegate(EditorGUIUtility.SkinChanged);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string SerializeMainMenuToString();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetMenuLocalizationTestMode(bool onoff);

    internal static GUIContent TextContent(string textAndTooltip)
    {
      if (textAndTooltip == null)
        textAndTooltip = string.Empty;
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_TextGUIContents[(object) textAndTooltip];
      if (guiContent == null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(textAndTooltip);
        guiContent = new GUIContent(andTooltipString[1]);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
        EditorGUIUtility.s_TextGUIContents[(object) textAndTooltip] = (object) guiContent;
      }
      return guiContent;
    }

    internal static GUIContent TextContentWithIcon(string textAndTooltip, string icon)
    {
      if (textAndTooltip == null)
        textAndTooltip = string.Empty;
      GUIContent guiContent = (GUIContent) EditorGUIUtility.s_TextGUIContents[(object) textAndTooltip];
      if (guiContent == null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(textAndTooltip);
        guiContent = new GUIContent(andTooltipString[1]);
        guiContent.image = (Texture) EditorGUIUtility.LoadIconRequired(icon);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
        EditorGUIUtility.s_TextGUIContents[(object) textAndTooltip] = (object) guiContent;
      }
      return guiContent;
    }

    internal static string[] GetNameAndTooltipString(string nameAndTooltip)
    {
      nameAndTooltip = LocalizationDatabase.GetLocalizedString(nameAndTooltip);
      string[] strArray1 = new string[3];
      string[] strArray2 = nameAndTooltip.Split('|');
      switch (strArray2.Length)
      {
        case 0:
          strArray1[0] = string.Empty;
          strArray1[1] = string.Empty;
          break;
        case 1:
          strArray1[0] = strArray2[0].Trim();
          strArray1[1] = strArray1[0];
          break;
        case 2:
          strArray1[0] = strArray2[0].Trim();
          strArray1[1] = strArray1[0];
          strArray1[2] = strArray2[1].Trim();
          break;
        default:
          Debug.LogError((object) ("Error in Tooltips: Too many strings in line beginning with '" + strArray2[0] + "'"));
          break;
      }
      return strArray1;
    }

    internal static Texture2D LoadIconRequired(string name)
    {
      Texture2D texture2D = EditorGUIUtility.LoadIcon(name);
      if (!(bool) ((UnityEngine.Object) texture2D))
        Debug.LogErrorFormat("Unable to load the icon: '{0}'.\nNote that either full project path should be used (with extension) or just the icon name if the icon is located in the following location: '{1}' (without extension, since png is assumed)", new object[2]
        {
          (object) name,
          (object) ("Assets/Editor Default Resources/" + EditorResourcesUtility.iconsPath)
        });
      return texture2D;
    }

    internal static Texture2D LoadIcon(string name)
    {
      return EditorGUIUtility.LoadIconForSkin(name, EditorGUIUtility.skinIndex);
    }

    private static Texture2D LoadGeneratedIconOrNormalIcon(string name)
    {
      Texture2D texture2D = EditorGUIUtility.Load(EditorResourcesUtility.generatedIconsPath + name + ".asset") as Texture2D;
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.Load(EditorResourcesUtility.iconsPath + name + ".png") as Texture2D;
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.Load(name) as Texture2D;
      return texture2D;
    }

    internal static Texture2D LoadIconForSkin(string name, int skinIndex)
    {
      if (string.IsNullOrEmpty(name))
        return (Texture2D) null;
      if (skinIndex == 0)
        return EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name);
      string name1 = "d_" + Path.GetFileName(name);
      string directoryName = Path.GetDirectoryName(name);
      if (!string.IsNullOrEmpty(directoryName))
        name1 = string.Format("{0}/{1}", (object) directoryName, (object) name1);
      Texture2D texture2D = EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name1);
      if (!(bool) ((UnityEngine.Object) texture2D))
        texture2D = EditorGUIUtility.LoadGeneratedIconOrNormalIcon(name);
      return texture2D;
    }

    /// <summary>
    ///   <para>Fetch the GUIContent from the Unity builtin resources with the given name.</para>
    /// </summary>
    /// <param name="name">Content name.</param>
    /// <param name="tooltip">Tooltip.</param>
    [ExcludeFromDocs]
    public static GUIContent IconContent(string name)
    {
      string tooltip = (string) null;
      return EditorGUIUtility.IconContent(name, tooltip);
    }

    /// <summary>
    ///   <para>Fetch the GUIContent from the Unity builtin resources with the given name.</para>
    /// </summary>
    /// <param name="name">Content name.</param>
    /// <param name="tooltip">Tooltip.</param>
    public static GUIContent IconContent(string name, [DefaultValue("null")] string tooltip)
    {
      GUIContent iconGuiContent = (GUIContent) EditorGUIUtility.s_IconGUIContents[(object) name];
      if (iconGuiContent != null)
        return iconGuiContent;
      GUIContent guiContent = new GUIContent();
      if (tooltip != null)
      {
        string[] andTooltipString = EditorGUIUtility.GetNameAndTooltipString(tooltip);
        if (andTooltipString[2] != null)
          guiContent.tooltip = andTooltipString[2];
      }
      guiContent.image = (Texture) EditorGUIUtility.LoadIconRequired(name);
      EditorGUIUtility.s_IconGUIContents[(object) name] = (object) guiContent;
      return guiContent;
    }

    internal static void Internal_SwitchSkin()
    {
      EditorGUIUtility.skinIndex = 1 - EditorGUIUtility.skinIndex;
    }

    /// <summary>
    ///   <para>Return a GUIContent object with the name and icon of an Object.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    public static GUIContent ObjectContent(UnityEngine.Object obj, System.Type type)
    {
      if ((bool) obj)
      {
        EditorGUIUtility.s_ObjectContent.text = !(obj is AudioMixerGroup) ? (!(obj is AudioMixerSnapshot) ? obj.name : obj.name + " (" + ((AudioMixerSnapshot) obj).audioMixer.name + ")") : obj.name + " (" + ((AudioMixerGroup) obj).audioMixer.name + ")";
        EditorGUIUtility.s_ObjectContent.image = (Texture) AssetPreview.GetMiniThumbnail(obj);
      }
      else
      {
        string str = type != null ? (type.Namespace == null ? type.ToString() : type.ToString().Substring(type.Namespace.ToString().Length + 1)) : "<no type>";
        EditorGUIUtility.s_ObjectContent.text = string.Format("None ({0})", (object) str);
        EditorGUIUtility.s_ObjectContent.image = (Texture) AssetPreview.GetMiniTypeThumbnail(type);
      }
      return EditorGUIUtility.s_ObjectContent;
    }

    internal static GUIContent TempContent(string t)
    {
      EditorGUIUtility.s_Text.text = t;
      return EditorGUIUtility.s_Text;
    }

    internal static GUIContent TempContent(Texture i)
    {
      EditorGUIUtility.s_Image.image = i;
      return EditorGUIUtility.s_Image;
    }

    internal static GUIContent TempContent(string t, Texture i)
    {
      EditorGUIUtility.s_TextImage.image = i;
      EditorGUIUtility.s_TextImage.text = t;
      return EditorGUIUtility.s_TextImage;
    }

    internal static GUIContent[] TempContent(string[] texts)
    {
      GUIContent[] guiContentArray = new GUIContent[texts.Length];
      for (int index = 0; index < texts.Length; ++index)
        guiContentArray[index] = new GUIContent(texts[index]);
      return guiContentArray;
    }

    internal static bool HasHolddownKeyModifiers(Event evt)
    {
      return evt.shift | evt.control | evt.alt | evt.command;
    }

    /// <summary>
    ///   <para>Does a given class have per-object thumbnails?</para>
    /// </summary>
    /// <param name="objType"></param>
    public static bool HasObjectThumbnail(System.Type objType)
    {
      if (objType == null)
        return false;
      if (!objType.IsSubclassOf(typeof (Texture)) && objType != typeof (Texture))
        return objType == typeof (Sprite);
      return true;
    }

    /// <summary>
    ///   <para>Set icons rendered as part of GUIContent to be rendered at a specific size.</para>
    /// </summary>
    /// <param name="size"></param>
    public static void SetIconSize(Vector2 size)
    {
      EditorGUIUtility.INTERNAL_CALL_SetIconSize(ref size);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetIconSize(ref Vector2 size);

    /// <summary>
    ///   <para>Get the size that has been set using SetIconSize.</para>
    /// </summary>
    public static Vector2 GetIconSize()
    {
      Vector2 size;
      EditorGUIUtility.Internal_GetIconSize(out size);
      return size;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_GetIconSize(out Vector2 size);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object GetScript(string scriptClass);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetIconForObject(UnityEngine.Object obj, Texture2D icon);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D GetIconForObject(UnityEngine.Object obj);

    internal static Texture2D GetHelpIcon(MessageType type)
    {
      switch (type)
      {
        case MessageType.Info:
          return EditorGUIUtility.infoIcon;
        case MessageType.Warning:
          return EditorGUIUtility.warningIcon;
        case MessageType.Error:
          return EditorGUIUtility.errorIcon;
        default:
          return (Texture2D) null;
      }
    }

    internal static GUIStyle GetBasicTextureStyle(Texture2D tex)
    {
      if (EditorGUIUtility.s_BasicTextureStyle == null)
        EditorGUIUtility.s_BasicTextureStyle = new GUIStyle();
      EditorGUIUtility.s_BasicTextureStyle.normal.background = tex;
      return EditorGUIUtility.s_BasicTextureStyle;
    }

    internal static void NotifyLanguageChanged(SystemLanguage newLanguage)
    {
      EditorGUIUtility.s_TextGUIContents = new Hashtable();
      EditorUtility.Internal_UpdateMenuTitleForLanguage(newLanguage);
      LocalizationDatabase.SetCurrentEditorLanguage(newLanguage);
      EditorApplication.RequestRepaintAllViews();
    }

    /// <summary>
    ///   <para>Get a texture from its source filename.</para>
    /// </summary>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D FindTexture(string name);

    /// <summary>
    ///   <para>Get one of the built-in GUI skins, which can be the game view, inspector or scene view skin as chosen by the parameter.</para>
    /// </summary>
    /// <param name="skin"></param>
    public static GUISkin GetBuiltinSkin(EditorSkin skin)
    {
      return GUIUtility.GetBuiltinSkin((int) skin);
    }

    /// <summary>
    ///   <para>Load a built-in resource that has to be there.</para>
    /// </summary>
    /// <param name="path"></param>
    public static UnityEngine.Object LoadRequired(string path)
    {
      UnityEngine.Object @object = EditorGUIUtility.Load(path, typeof (UnityEngine.Object));
      if (!(bool) @object)
        Debug.LogError((object) ("Unable to find required resource at 'Editor Default Resources/" + path + "'"));
      return @object;
    }

    /// <summary>
    ///   <para>Load a built-in resource.</para>
    /// </summary>
    /// <param name="path"></param>
    public static UnityEngine.Object Load(string path)
    {
      return EditorGUIUtility.Load(path, typeof (UnityEngine.Object));
    }

    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    private static UnityEngine.Object Load(string filename, System.Type type)
    {
      UnityEngine.Object object1 = AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/" + filename, type);
      if (object1 != (UnityEngine.Object) null)
        return object1;
      UnityEngine.Object object2 = EditorGUIUtility.GetEditorAssetBundle().LoadAsset(filename, type);
      if (object2 != (UnityEngine.Object) null)
        return object2;
      return AssetDatabase.LoadAssetAtPath(filename, type);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object GetBuiltinExtraResource(System.Type type, string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern BuiltinResource[] GetBuiltinResourceList(int classID);

    /// <summary>
    ///   <para>Ping an object in a window like clicking it in an inspector.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetInstanceID"></param>
    public static void PingObject(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      EditorGUIUtility.PingObject(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Ping an object in a window like clicking it in an inspector.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetInstanceID"></param>
    public static void PingObject(int targetInstanceID)
    {
      using (List<SceneHierarchyWindow>.Enumerator enumerator = SceneHierarchyWindow.GetAllSceneHierarchyWindows().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SceneHierarchyWindow current = enumerator.Current;
          bool ping = true;
          current.FrameObject(targetInstanceID, ping);
        }
      }
      using (List<ProjectBrowser>.Enumerator enumerator = ProjectBrowser.GetAllProjectBrowsers().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ProjectBrowser current = enumerator.Current;
          bool ping = true;
          current.FrameObject(targetInstanceID, ping);
        }
      }
    }

    internal static void MoveFocusAndScroll(bool forward)
    {
      int keyboardControl = GUIUtility.keyboardControl;
      EditorGUIUtility.Internal_MoveKeyboardFocus(forward);
      if (keyboardControl == GUIUtility.keyboardControl)
        return;
      EditorGUIUtility.RefreshScrollPosition();
    }

    internal static void RefreshScrollPosition()
    {
      GUI.ScrollViewState topScrollView = GUI.GetTopScrollView();
      Rect rect;
      if (topScrollView == null || !EditorGUIUtility.Internal_GetKeyboardRect(GUIUtility.keyboardControl, out rect))
        return;
      topScrollView.ScrollTo(rect);
    }

    internal static void ScrollForTabbing(bool forward)
    {
      GUI.ScrollViewState topScrollView = GUI.GetTopScrollView();
      Rect rect;
      if (topScrollView == null || !EditorGUIUtility.Internal_GetKeyboardRect(EditorGUIUtility.Internal_GetNextKeyboardControlID(forward), out rect))
        return;
      topScrollView.ScrollTo(rect);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_GetKeyboardRect(int id, out Rect rect);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_MoveKeyboardFocus(bool forward);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetNextKeyboardControlID(bool forward);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AssetBundle GetEditorAssetBundle();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetRenderTextureNoViewport(RenderTexture rt);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetVisibleLayers(int layers);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetLockedLayers(int layers);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsGizmosAllowedForObject(UnityEngine.Object obj);

    internal static void ResetGUIState()
    {
      GUI.skin = (GUISkin) null;
      Color white = Color.white;
      GUI.contentColor = white;
      GUI.backgroundColor = white;
      GUI.color = !EditorApplication.isPlayingOrWillChangePlaymode ? Color.white : (Color) HostView.kPlayModeDarken;
      GUI.enabled = true;
      GUI.changed = false;
      EditorGUI.indentLevel = 0;
      EditorGUI.ClearStacks();
      EditorGUIUtility.fieldWidth = 0.0f;
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUIUtility.SetBoldDefaultFont(false);
      EditorGUIUtility.UnlockContextWidth();
      EditorGUIUtility.hierarchyMode = false;
      EditorGUIUtility.wideMode = false;
      ScriptAttributeUtility.propertyHandlerCache = (PropertyHandlerCache) null;
    }

    internal static void RenderGameViewCamerasInternal(Rect cameraRect, int targetDisplay, bool gizmos, bool gui)
    {
      EditorGUIUtility.INTERNAL_CALL_RenderGameViewCamerasInternal(ref cameraRect, targetDisplay, gizmos, gui);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RenderGameViewCamerasInternal(ref Rect cameraRect, int targetDisplay, bool gizmos, bool gui);

    [Obsolete("RenderGameViewCameras is no longer supported, and will be removed in a future version of Unity. Consider rendering cameras manually.")]
    public static void RenderGameViewCameras(Rect cameraRect, int targetDisplay, bool gizmos, bool gui)
    {
      EditorGUIUtility.INTERNAL_CALL_RenderGameViewCameras(ref cameraRect, targetDisplay, gizmos, gui);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RenderGameViewCameras(ref Rect cameraRect, int targetDisplay, bool gizmos, bool gui);

    /// <summary>
    ///   <para>Render all ingame cameras.</para>
    /// </summary>
    /// <param name="cameraRect">The device coordinates to render all game cameras into.</param>
    /// <param name="gizmos">Show gizmos as well.</param>
    /// <param name="gui"></param>
    /// <param name="statsRect"></param>
    [Obsolete("RenderGameViewCameras is no longer supported, and will be removed in a future version of Unity. Consider rendering cameras manually.")]
    public static void RenderGameViewCameras(Rect cameraRect, bool gizmos, bool gui)
    {
      EditorGUIUtility.RenderGameViewCameras(cameraRect, 0, gizmos, gui);
    }

    /// <summary>
    ///   <para>Check if any enabled camera can render to a particular display.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <returns>
    ///   <para>True if a camera will render to the display.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsDisplayReferencedByCameras(int displayIndex);

    /// <summary>
    ///   <para>Render all ingame cameras.</para>
    /// </summary>
    /// <param name="cameraRect">The device coordinates to render all game cameras into.</param>
    /// <param name="gizmos">Show gizmos as well.</param>
    /// <param name="gui"></param>
    /// <param name="statsRect"></param>
    [Obsolete("RenderGameViewCameras is no longer supported, and will be removed in a future version of Unity. Consider rendering cameras manually.")]
    public static void RenderGameViewCameras(Rect cameraRect, Rect statsRect, bool gizmos, bool gui)
    {
      EditorGUIUtility.RenderGameViewCameras(cameraRect, 0, gizmos, gui);
    }

    /// <summary>
    ///   <para>Send an input event into the game.</para>
    /// </summary>
    /// <param name="evt"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void QueueGameViewInputEvent(Event evt);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetDefaultFont(Font font);

    private static GUIStyle GetStyle(string styleName)
    {
      GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
      if (guiStyle == null)
      {
        Debug.Log((object) ("Missing built-in guistyle " + styleName));
        guiStyle = GUISkin.error;
      }
      return guiStyle;
    }

    [RequiredByNativeCode]
    internal static void HandleControlID(int id)
    {
      EditorGUIUtility.s_LastControlID = id;
      if (EditorGUI.s_PrefixLabel.text == null)
        return;
      EditorGUI.HandlePrefixLabel(EditorGUI.s_PrefixTotalRect, EditorGUI.s_PrefixRect, EditorGUI.s_PrefixLabel, EditorGUIUtility.s_LastControlID, EditorGUI.s_PrefixStyle);
    }

    private static float CalcContextWidth()
    {
      float num = GUIClip.GetTopRect().width;
      if ((double) num < 1.0 || (double) num >= 40000.0)
        num = EditorGUIUtility.currentViewWidth;
      return num;
    }

    internal static void LockContextWidth()
    {
      EditorGUIUtility.s_ContextWidth = EditorGUIUtility.CalcContextWidth();
    }

    internal static void UnlockContextWidth()
    {
      EditorGUIUtility.s_ContextWidth = 0.0f;
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like regular controls.</para>
    /// </summary>
    /// <param name="labelWidth">Width to use for prefixed labels.</param>
    /// <param name="fieldWidth">Width of text entries.</param>
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    [ExcludeFromDocs]
    public static void LookLikeControls(float labelWidth)
    {
      float fieldWidth = 0.0f;
      EditorGUIUtility.LookLikeControls(labelWidth, fieldWidth);
    }

    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    [ExcludeFromDocs]
    public static void LookLikeControls()
    {
      EditorGUIUtility.LookLikeControls(0.0f, 0.0f);
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like regular controls.</para>
    /// </summary>
    /// <param name="labelWidth">Width to use for prefixed labels.</param>
    /// <param name="fieldWidth">Width of text entries.</param>
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated. Use EditorGUIUtility.labelWidth and EditorGUIUtility.fieldWidth to control label and field widths.")]
    public static void LookLikeControls([DefaultValue("0")] float labelWidth, [DefaultValue("0")] float fieldWidth)
    {
      EditorGUIUtility.fieldWidth = fieldWidth;
      EditorGUIUtility.labelWidth = labelWidth;
    }

    /// <summary>
    ///   <para>Make all EditorGUI look like simplified outline view controls.</para>
    /// </summary>
    [Obsolete("LookLikeControls and LookLikeInspector modes are deprecated.")]
    public static void LookLikeInspector()
    {
      EditorGUIUtility.fieldWidth = 0.0f;
      EditorGUIUtility.labelWidth = 0.0f;
    }

    internal static void SkinChanged()
    {
      EditorStyles.UpdateSkinCache();
    }

    internal static Rect DragZoneRect(Rect position)
    {
      return new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
    }

    internal static void SetBoldDefaultFont(bool isBold)
    {
      int num = !isBold ? 0 : 1;
      if (num == EditorGUIUtility.s_FontIsBold)
        return;
      EditorGUIUtility.SetDefaultFont(!isBold ? EditorStyles.standardFont : EditorStyles.boldFont);
      EditorGUIUtility.s_FontIsBold = num;
    }

    internal static bool GetBoldDefaultFont()
    {
      return EditorGUIUtility.s_FontIsBold == 1;
    }

    /// <summary>
    ///   <para>Creates an event.</para>
    /// </summary>
    /// <param name="commandName"></param>
    public static Event CommandEvent(string commandName)
    {
      Event @event = new Event();
      EditorGUIUtility.Internal_SetupEventValues((object) @event);
      @event.type = EventType.ExecuteCommand;
      @event.commandName = commandName;
      return @event;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetupEventValues(object evt);

    /// <summary>
    ///   <para>Draw a color swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="color">The color to draw.</param>
    public static void DrawColorSwatch(Rect position, Color color)
    {
      EditorGUIUtility.DrawColorSwatch(position, color, true);
    }

    internal static void DrawColorSwatch(Rect position, Color color, bool showAlpha)
    {
      EditorGUIUtility.DrawColorSwatch(position, color, showAlpha, false);
    }

    internal static void DrawColorSwatch(Rect position, Color color, bool showAlpha, bool hdr)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color1 = GUI.color;
      float a = !GUI.enabled ? 2f : 1f;
      GUI.color = !EditorGUI.showMixedValue ? new Color(color.r, color.g, color.b, a) : new Color(0.82f, 0.82f, 0.82f, a) * color1;
      GUIStyle whiteTextureStyle = EditorGUIUtility.whiteTextureStyle;
      whiteTextureStyle.Draw(position, false, false, false, false);
      float maxColorComponent = GUI.color.maxColorComponent;
      if (hdr && (double) maxColorComponent > 1.0)
      {
        float width = position.width / 3f;
        Rect position1 = new Rect(position.x, position.y, width, position.height);
        Rect position2 = new Rect(position.xMax - width, position.y, width, position.height);
        Color color2 = GUI.color.RGBMultiplied(1f / maxColorComponent);
        Color color3 = GUI.color;
        GUI.color = color2;
        GUIStyle basicTextureStyle = EditorGUIUtility.GetBasicTextureStyle(EditorGUIUtility.whiteTexture);
        basicTextureStyle.Draw(position1, false, false, false, false);
        basicTextureStyle.Draw(position2, false, false, false, false);
        GUI.color = color3;
        EditorGUIUtility.GetBasicTextureStyle(ColorPicker.GetGradientTextureWithAlpha0To1()).Draw(position1, false, false, false, false);
        EditorGUIUtility.GetBasicTextureStyle(ColorPicker.GetGradientTextureWithAlpha1To0()).Draw(position2, false, false, false, false);
      }
      if (!EditorGUI.showMixedValue)
      {
        if (showAlpha)
        {
          GUI.color = new Color(0.0f, 0.0f, 0.0f, a);
          float height = Mathf.Clamp(position.height * 0.2f, 2f, 20f);
          Rect position1 = new Rect(position.x, position.yMax - height, position.width, height);
          whiteTextureStyle.Draw(position1, false, false, false, false);
          GUI.color = new Color(1f, 1f, 1f, a);
          position1.width *= Mathf.Clamp01(color.a);
          whiteTextureStyle.Draw(position1, false, false, false, false);
        }
      }
      else
      {
        EditorGUI.BeginHandleMixedValueContentColor();
        whiteTextureStyle.Draw(position, EditorGUI.mixedValueContent, false, false, false, false);
        EditorGUI.EndHandleMixedValueContentColor();
      }
      GUI.color = color1;
      if (!hdr || (double) maxColorComponent <= 1.0)
        return;
      GUI.Label(new Rect(position.x, position.y, position.width - 3f, position.height), "HDR", EditorStyles.centeredGreyMiniLabel);
    }

    /// <summary>
    ///   <para>Draw a curve swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="curve">The curve to draw.</param>
    /// <param name="property">The curve to draw as a SerializedProperty.</param>
    /// <param name="color">The color to draw the curve with.</param>
    /// <param name="bgColor">The color to draw the background with.</param>
    /// <param name="curveRanges">Optional parameter to specify the range of the curve which should be included in swatch.</param>
    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, false, new Rect());
    }

    /// <summary>
    ///   <para>Draw a curve swatch.</para>
    /// </summary>
    /// <param name="position">The rectangle to draw the color swatch within.</param>
    /// <param name="curve">The curve to draw.</param>
    /// <param name="property">The curve to draw as a SerializedProperty.</param>
    /// <param name="color">The color to draw the curve with.</param>
    /// <param name="bgColor">The color to draw the background with.</param>
    /// <param name="curveRanges">Optional parameter to specify the range of the curve which should be included in swatch.</param>
    public static void DrawCurveSwatch(Rect position, AnimationCurve curve, SerializedProperty property, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, (AnimationCurve) null, property, (SerializedProperty) null, color, bgColor, true, curveRanges);
    }

    /// <summary>
    ///   <para>Draw swatch with a filled region between two SerializedProperty curves.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="property2"></param>
    /// <param name="color"></param>
    /// <param name="bgColor"></param>
    /// <param name="curveRanges"></param>
    public static void DrawRegionSwatch(Rect position, SerializedProperty property, SerializedProperty property2, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, (AnimationCurve) null, (AnimationCurve) null, property, property2, color, bgColor, true, curveRanges);
    }

    /// <summary>
    ///   <para>Draw swatch with a filled region between two curves.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="curve"></param>
    /// <param name="curve2"></param>
    /// <param name="color"></param>
    /// <param name="bgColor"></param>
    /// <param name="curveRanges"></param>
    public static void DrawRegionSwatch(Rect position, AnimationCurve curve, AnimationCurve curve2, Color color, Color bgColor, Rect curveRanges)
    {
      EditorGUIUtility.DrawCurveSwatchInternal(position, curve, curve2, (SerializedProperty) null, (SerializedProperty) null, color, bgColor, true, curveRanges);
    }

    private static void DrawCurveSwatchInternal(Rect position, AnimationCurve curve, AnimationCurve curve2, SerializedProperty property, SerializedProperty property2, Color color, Color bgColor, bool useCurveRanges, Rect curveRanges)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      int width = (int) position.width;
      int height = (int) position.height;
      Color color1 = GUI.color;
      GUI.color = bgColor;
      EditorGUIUtility.whiteTextureStyle.Draw(position, false, false, false, false);
      GUI.color = color1;
      if (property != null && property.hasMultipleDifferentValues)
      {
        EditorGUI.BeginHandleMixedValueContentColor();
        GUI.Label(position, EditorGUI.mixedValueContent, (GUIStyle) "PreOverlayLabel");
        EditorGUI.EndHandleMixedValueContentColor();
      }
      else
      {
        Texture2D tex = (Texture2D) null;
        if (property != null)
          tex = property2 != null ? (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(width, height, property, property2, color) : AnimationCurvePreviewCache.GetPreview(width, height, property, property2, color, curveRanges)) : (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(width, height, property, color) : AnimationCurvePreviewCache.GetPreview(width, height, property, color, curveRanges));
        else if (curve != null)
          tex = curve2 != null ? (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(width, height, curve, curve2, color) : AnimationCurvePreviewCache.GetPreview(width, height, curve, curve2, color, curveRanges)) : (!useCurveRanges ? AnimationCurvePreviewCache.GetPreview(width, height, curve, color) : AnimationCurvePreviewCache.GetPreview(width, height, curve, color, curveRanges));
        GUIStyle basicTextureStyle = EditorGUIUtility.GetBasicTextureStyle(tex);
        position.width = (float) tex.width;
        position.height = (float) tex.height;
        basicTextureStyle.Draw(position, false, false, false, false);
      }
    }

    [Obsolete("EditorGUIUtility.RGBToHSV is obsolete. Use Color.RGBToHSV instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.RGBToHSV(*)", true)]
    public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
    {
      Color.RGBToHSV(rgbColor, out H, out S, out V);
    }

    [Obsolete("EditorGUIUtility.HSVToRGB is obsolete. Use Color.HSVToRGB instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.HSVToRGB(*)", true)]
    public static Color HSVToRGB(float H, float S, float V)
    {
      return Color.HSVToRGB(H, S, V);
    }

    [Obsolete("EditorGUIUtility.HSVToRGB is obsolete. Use Color.HSVToRGB instead (UnityUpgradable) -> [UnityEngine] UnityEngine.Color.HSVToRGB(*)", true)]
    public static Color HSVToRGB(float H, float S, float V, bool hdr)
    {
      return Color.HSVToRGB(H, S, V, hdr);
    }

    internal static void SetPasteboardColor(Color color)
    {
      EditorGUIUtility.INTERNAL_CALL_SetPasteboardColor(ref color);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetPasteboardColor(ref Color color);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasPasteboardColor();

    internal static Color GetPasteboardColor()
    {
      Color color;
      EditorGUIUtility.INTERNAL_CALL_GetPasteboardColor(out color);
      return color;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPasteboardColor(out Color value);

    /// <summary>
    ///   <para>Add a custom mouse pointer to a control.</para>
    /// </summary>
    /// <param name="position">The rectangle the control should be shown within.</param>
    /// <param name="mouse">The mouse cursor to use.</param>
    /// <param name="controlID">ID of a target control.</param>
    public static void AddCursorRect(Rect position, MouseCursor mouse)
    {
      EditorGUIUtility.AddCursorRect(position, mouse, 0);
    }

    /// <summary>
    ///   <para>Add a custom mouse pointer to a control.</para>
    /// </summary>
    /// <param name="position">The rectangle the control should be shown within.</param>
    /// <param name="mouse">The mouse cursor to use.</param>
    /// <param name="controlID">ID of a target control.</param>
    public static void AddCursorRect(Rect position, MouseCursor mouse, int controlID)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Rect rect = GUIClip.Unclip(position);
      Rect topmostRect = GUIClip.topmostRect;
      Rect r = Rect.MinMaxRect(Mathf.Max(rect.x, topmostRect.x), Mathf.Max(rect.y, topmostRect.y), Mathf.Min(rect.xMax, topmostRect.xMax), Mathf.Min(rect.yMax, topmostRect.yMax));
      if ((double) r.width <= 0.0 || (double) r.height <= 0.0)
        return;
      EditorGUIUtility.Internal_AddCursorRect(r, mouse, controlID);
    }

    private static void Internal_AddCursorRect(Rect r, MouseCursor m, int controlID)
    {
      EditorGUIUtility.INTERNAL_CALL_Internal_AddCursorRect(ref r, m, controlID);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_AddCursorRect(ref Rect r, MouseCursor m, int controlID);

    internal static Rect HandleHorizontalSplitter(Rect dragRect, float width, float minLeftSide, float minRightSide)
    {
      if (Event.current.type == EventType.Repaint)
        EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.SplitResizeLeftRight);
      float num = 0.0f;
      float x = EditorGUI.MouseDeltaReader(dragRect, true).x;
      if ((double) x != 0.0)
      {
        dragRect.x += x;
        num = Mathf.Clamp(dragRect.x, minLeftSide, width - minRightSide);
      }
      if ((double) dragRect.x > (double) width - (double) minRightSide)
        num = width - minRightSide;
      if ((double) num > 0.0)
        dragRect.x = num;
      return dragRect;
    }

    internal static void DrawHorizontalSplitter(Rect dragRect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color *= !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 1.333f) : new Color(0.12f, 0.12f, 0.12f, 1.333f);
      GUI.DrawTexture(new Rect(dragRect.x - 1f, dragRect.y, 1f, dragRect.height), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CleanCache(string text);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSearchIndexOfControlIDList(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSearchIndexOfControlIDList();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanHaveKeyboardFocus(int id);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetWantsMouseJumping(int wantz);

    public static void ShowObjectPicker<T>(UnityEngine.Object obj, bool allowSceneObjects, string searchFilter, int controlID) where T : UnityEngine.Object
    {
      System.Type requiredType = typeof (T);
      ObjectSelector.get.Show(obj, requiredType, (SerializedProperty) null, allowSceneObjects);
      ObjectSelector.get.objectSelectorID = controlID;
      ObjectSelector.get.searchFilter = searchFilter;
    }

    /// <summary>
    ///   <para>The object currently selected in the object picker.</para>
    /// </summary>
    public static UnityEngine.Object GetObjectPickerObject()
    {
      return ObjectSelector.GetCurrentObject();
    }

    /// <summary>
    ///   <para>The controlID of the currently showing object picker.</para>
    /// </summary>
    public static int GetObjectPickerControlID()
    {
      return ObjectSelector.get.objectSelectorID;
    }

    internal static Rect PointsToPixels(Rect rect)
    {
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      rect.x *= pixelsPerPoint;
      rect.y *= pixelsPerPoint;
      rect.width *= pixelsPerPoint;
      rect.height *= pixelsPerPoint;
      return rect;
    }

    internal static Rect PixelsToPoints(Rect rect)
    {
      float num = 1f / EditorGUIUtility.pixelsPerPoint;
      rect.x *= num;
      rect.y *= num;
      rect.width *= num;
      rect.height *= num;
      return rect;
    }

    internal static Vector2 PointsToPixels(Vector2 position)
    {
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      position.x *= pixelsPerPoint;
      position.y *= pixelsPerPoint;
      return position;
    }

    internal static Vector2 PixelsToPoints(Vector2 position)
    {
      float num = 1f / EditorGUIUtility.pixelsPerPoint;
      position.x *= num;
      position.y *= num;
      return position;
    }

    public static List<Rect> GetFlowLayoutedRects(Rect rect, GUIStyle style, float horizontalSpacing, float verticalSpacing, List<string> items)
    {
      List<Rect> rectList = new List<Rect>(items.Count);
      Vector2 position = rect.position;
      for (int index = 0; index < items.Count; ++index)
      {
        GUIContent content = EditorGUIUtility.TempContent(items[index]);
        Vector2 size = style.CalcSize(content);
        Rect rect1 = new Rect(position, size);
        if ((double) position.x + (double) size.x + (double) horizontalSpacing >= (double) rect.xMax)
        {
          position.x = rect.x;
          position.y += size.y + verticalSpacing;
          rect1.position = position;
        }
        rectList.Add(rect1);
        position.x += size.x + horizontalSpacing;
      }
      return rectList;
    }
  }
}
