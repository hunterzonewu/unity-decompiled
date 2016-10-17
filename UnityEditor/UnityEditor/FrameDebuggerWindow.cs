// Decompiled with JetBrains decompiler
// Type: UnityEditor.FrameDebuggerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class FrameDebuggerWindow : EditorWindow
  {
    public static readonly string[] s_FrameEventTypeNames = new string[21]{ "Clear (nothing)", "Clear (color)", "Clear (Z)", "Clear (color+Z)", "Clear (stencil)", "Clear (color+stencil)", "Clear (Z+stencil)", "Clear (color+Z+stencil)", "SetRenderTarget", "Resolve Color", "Resolve Depth", "Grab RenderTexture", "Static Batch", "Dynamic Batch", "Draw Mesh", "Draw Dynamic", "Draw GL", "GPU Skinning", "Draw Procedural", "Compute Shader", "Plugin Event" };
    private static List<FrameDebuggerWindow> s_FrameDebuggers = new List<FrameDebuggerWindow>();
    [SerializeField]
    private float m_ListWidth = 300f;
    private int m_RepaintFrames = 4;
    public Vector2 m_PreviewDir = new Vector2(120f, -20f);
    [NonSerialized]
    private float m_RTWhiteLevel = 1f;
    private Vector2 m_ScrollViewShaderProps = Vector2.zero;
    private GUIContent[] m_AdditionalInfoGuiContents = ((IEnumerable<string>) Enum.GetNames(typeof (ShowAdditionalInfo))).Select<string, GUIContent>((Func<string, GUIContent>) (m => new GUIContent(m))).ToArray<GUIContent>();
    private AttachProfilerUI m_AttachProfilerUI = new AttachProfilerUI();
    private const float kScrollbarWidth = 16f;
    private const float kResizerWidth = 5f;
    private const float kMinListWidth = 200f;
    private const float kMinDetailsWidth = 200f;
    private const float kMinWindowWidth = 240f;
    private const float kDetailsMargin = 4f;
    private const float kMinPreviewSize = 64f;
    private const string kFloatFormat = "g2";
    private const string kFloatDetailedFormat = "g7";
    private const float kPropertyFieldHeight = 16f;
    private const float kPropertyFieldIndent = 15f;
    private const float kPropertyNameWidth = 0.4f;
    private const float kPropertyFlagsWidth = 0.1f;
    private const float kPropertyValueWidth = 0.5f;
    private const int kNeedToRepaintFrames = 4;
    private PreviewRenderUtility m_PreviewUtility;
    private Material m_Material;
    private Material m_WireMaterial;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    [NonSerialized]
    private FrameDebuggerTreeView m_Tree;
    [NonSerialized]
    private int m_FrameEventsHash;
    [NonSerialized]
    private int m_RTIndex;
    [NonSerialized]
    private int m_RTChannel;
    [NonSerialized]
    private float m_RTBlackLevel;
    private int m_PrevEventsLimit;
    private int m_PrevEventsCount;
    private ShowAdditionalInfo m_AdditionalInfo;
    private static FrameDebuggerWindow.Styles ms_Styles;

    public static FrameDebuggerWindow.Styles styles
    {
      get
      {
        return FrameDebuggerWindow.ms_Styles ?? (FrameDebuggerWindow.ms_Styles = new FrameDebuggerWindow.Styles());
      }
    }

    public FrameDebuggerWindow()
    {
      this.position = new Rect(50f, 50f, 600f, 350f);
      this.minSize = new Vector2(400f, 200f);
    }

    [MenuItem("Window/Frame Debugger", false, 2100)]
    public static FrameDebuggerWindow ShowFrameDebuggerWindow()
    {
      FrameDebuggerWindow window = EditorWindow.GetWindow(typeof (FrameDebuggerWindow)) as FrameDebuggerWindow;
      if ((UnityEngine.Object) window != (UnityEngine.Object) null)
        window.titleContent = EditorGUIUtility.TextContent("Frame Debug");
      return window;
    }

    internal static void RepaintAll()
    {
      using (List<FrameDebuggerWindow>.Enumerator enumerator = FrameDebuggerWindow.s_FrameDebuggers.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Repaint();
      }
    }

    internal void ChangeFrameEventLimit(int newLimit)
    {
      if (newLimit <= 0 || newLimit > FrameDebuggerUtility.count)
        return;
      if (newLimit != FrameDebuggerUtility.limit && newLimit > 0)
      {
        GameObject frameEventGameObject = FrameDebuggerUtility.GetFrameEventGameObject(newLimit - 1);
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null)
          EditorGUIUtility.PingObject((UnityEngine.Object) frameEventGameObject);
      }
      FrameDebuggerUtility.limit = newLimit;
      if (this.m_Tree == null)
        return;
      this.m_Tree.SelectFrameEventIndex(newLimit);
    }

    private static void DisableFrameDebugger()
    {
      if (FrameDebuggerUtility.IsLocalEnabled())
        EditorApplication.SetSceneRepaintDirty();
      FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
    }

    internal void OnDidOpenScene()
    {
      FrameDebuggerWindow.DisableFrameDebugger();
    }

    private void OnPlayModeStateChanged()
    {
      this.RepaintOnLimitChange();
    }

    internal void OnEnable()
    {
      this.autoRepaintOnSceneChange = true;
      FrameDebuggerWindow.s_FrameDebuggers.Add(this);
      EditorApplication.playmodeStateChanged += new EditorApplication.CallbackFunction(this.OnPlayModeStateChanged);
      this.m_RepaintFrames = 4;
    }

    internal void OnDisable()
    {
      if ((UnityEngine.Object) this.m_WireMaterial != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_WireMaterial, true);
      if (this.m_PreviewUtility != null)
      {
        this.m_PreviewUtility.Cleanup();
        this.m_PreviewUtility = (PreviewRenderUtility) null;
      }
      FrameDebuggerWindow.s_FrameDebuggers.Remove(this);
      EditorApplication.playmodeStateChanged -= new EditorApplication.CallbackFunction(this.OnPlayModeStateChanged);
      FrameDebuggerWindow.DisableFrameDebugger();
    }

    public void EnableIfNeeded()
    {
      if (FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled())
        return;
      this.m_RTChannel = 0;
      this.m_RTIndex = 0;
      this.m_RTBlackLevel = 0.0f;
      this.m_RTWhiteLevel = 1f;
      this.ClickEnableFrameDebugger();
      this.RepaintOnLimitChange();
    }

    private void ClickEnableFrameDebugger()
    {
      bool flag1 = FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled();
      bool flag2 = !flag1 && this.m_AttachProfilerUI.IsEditor();
      if (flag2 && !FrameDebuggerUtility.locallySupported)
        return;
      if (flag2 && EditorApplication.isPlaying && !EditorApplication.isPaused)
        EditorApplication.isPaused = true;
      if (!flag1)
        FrameDebuggerUtility.SetEnabled(true, ProfilerDriver.connectedProfiler);
      else
        FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
      if (FrameDebuggerUtility.IsLocalEnabled())
      {
        GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
        if ((bool) ((UnityEngine.Object) editorWindowOfType))
          editorWindowOfType.ShowTab();
      }
      this.m_PrevEventsLimit = FrameDebuggerUtility.limit;
      this.m_PrevEventsCount = FrameDebuggerUtility.count;
    }

    private bool DrawToolbar(FrameDebuggerEvent[] descs)
    {
      bool flag1 = false;
      bool flag2 = !this.m_AttachProfilerUI.IsEditor() || FrameDebuggerUtility.locallySupported;
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginDisabledGroup(!flag2);
      GUILayout.Toggle((FrameDebuggerUtility.IsLocalEnabled() ? 1 : (FrameDebuggerUtility.IsRemoteEnabled() ? 1 : 0)) != 0, FrameDebuggerWindow.styles.recordButton, EditorStyles.toolbarButton, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(80f)
      });
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck())
      {
        this.ClickEnableFrameDebugger();
        flag1 = true;
      }
      this.m_AttachProfilerUI.OnGUILayout((EditorWindow) this);
      bool flag3 = FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled();
      if (flag3 && ProfilerDriver.connectedProfiler != FrameDebuggerUtility.GetRemotePlayerGUID())
      {
        FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
        FrameDebuggerUtility.SetEnabled(true, ProfilerDriver.connectedProfiler);
      }
      GUI.enabled = flag3;
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginDisabledGroup(FrameDebuggerUtility.count <= 1);
      int newLimit = EditorGUILayout.IntSlider(FrameDebuggerUtility.limit, 1, FrameDebuggerUtility.count);
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck())
        this.ChangeFrameEventLimit(newLimit);
      GUILayout.Label(" of " + (object) FrameDebuggerUtility.count, EditorStyles.miniLabel, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(newLimit <= 1);
      if (GUILayout.Button(FrameDebuggerWindow.styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.ChangeFrameEventLimit(newLimit - 1);
      EditorGUI.EndDisabledGroup();
      EditorGUI.BeginDisabledGroup(newLimit >= FrameDebuggerUtility.count);
      if (GUILayout.Button(FrameDebuggerWindow.styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.ChangeFrameEventLimit(newLimit + 1);
      if (this.m_PrevEventsLimit == this.m_PrevEventsCount && FrameDebuggerUtility.count != this.m_PrevEventsCount && FrameDebuggerUtility.limit == this.m_PrevEventsLimit)
        this.ChangeFrameEventLimit(FrameDebuggerUtility.count);
      this.m_PrevEventsLimit = FrameDebuggerUtility.limit;
      this.m_PrevEventsCount = FrameDebuggerUtility.count;
      EditorGUI.EndDisabledGroup();
      GUILayout.EndHorizontal();
      return flag1;
    }

    private void DrawMeshPreview(FrameDebuggerEventData curEventData, Rect previewRect, Rect meshInfoRect, Mesh mesh, int meshSubset)
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility();
        this.m_PreviewUtility.m_CameraFieldOfView = 30f;
      }
      if ((UnityEngine.Object) this.m_Material == (UnityEngine.Object) null)
        this.m_Material = EditorGUIUtility.GetBuiltinExtraResource(typeof (Material), "Default-Material.mat") as Material;
      if ((UnityEngine.Object) this.m_WireMaterial == (UnityEngine.Object) null)
        this.m_WireMaterial = ModelInspector.CreateWireframeMaterial();
      this.m_PreviewUtility.BeginPreview(previewRect, (GUIStyle) "preBackground");
      ModelInspector.RenderMeshPreview(mesh, this.m_PreviewUtility, this.m_Material, this.m_WireMaterial, this.m_PreviewDir, meshSubset);
      this.m_PreviewUtility.EndAndDrawPreview(previewRect);
      string str = mesh.name;
      if (string.IsNullOrEmpty(str))
        str = "<no name>";
      string text = str + " subset " + (object) meshSubset + "\n" + (object) curEventData.vertexCount + " verts, " + (object) curEventData.indexCount + " indices";
      EditorGUI.DropShadowLabel(meshInfoRect, text);
    }

    private bool DrawEventMesh(FrameDebuggerEventData curEventData)
    {
      Mesh mesh = curEventData.mesh;
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
        return false;
      Rect rect = GUILayoutUtility.GetRect(10f, 10f, new GUILayoutOption[1]{ GUILayout.ExpandHeight(true) });
      if ((double) rect.width < 64.0 || (double) rect.height < 64.0)
        return true;
      GameObject frameEventGameObject = FrameDebuggerUtility.GetFrameEventGameObject(curEventData.frameEventIndex);
      Rect meshInfoRect = rect;
      meshInfoRect.yMin = meshInfoRect.yMax - EditorGUIUtility.singleLineHeight * 2f;
      Rect position = meshInfoRect;
      meshInfoRect.xMin = meshInfoRect.center.x;
      position.xMax = position.center.x;
      if (Event.current.type == EventType.MouseDown)
      {
        if (meshInfoRect.Contains(Event.current.mousePosition))
        {
          EditorGUIUtility.PingObject((UnityEngine.Object) mesh);
          Event.current.Use();
        }
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null && position.Contains(Event.current.mousePosition))
        {
          EditorGUIUtility.PingObject(frameEventGameObject.GetInstanceID());
          Event.current.Use();
        }
      }
      this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, rect);
      if (Event.current.type == EventType.Repaint)
      {
        int meshSubset = curEventData.meshSubset;
        this.DrawMeshPreview(curEventData, rect, meshInfoRect, mesh, meshSubset);
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null)
          EditorGUI.DropShadowLabel(position, frameEventGameObject.name);
      }
      return true;
    }

    private void DrawRenderTargetControls(FrameDebuggerEventData cur)
    {
      if (cur.rtWidth <= 0 || cur.rtHeight <= 0)
        return;
      bool disabled = cur.rtFormat == 1 || cur.rtFormat == 3;
      bool flag1 = (int) cur.rtHasDepthTexture != 0;
      short rtCount = cur.rtCount;
      if (flag1)
        ++rtCount;
      GUILayout.Label("RenderTarget: " + cur.rtName, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginDisabledGroup((int) rtCount <= 1);
      GUIContent[] displayedOptions = new GUIContent[(int) rtCount];
      for (int index = 0; index < (int) cur.rtCount; ++index)
        displayedOptions[index] = FrameDebuggerWindow.Styles.mrtLabels[index];
      if (flag1)
        displayedOptions[(int) cur.rtCount] = FrameDebuggerWindow.Styles.depthLabel;
      int num = Mathf.Clamp(this.m_RTIndex, 0, (int) rtCount - 1);
      bool flag2 = num != this.m_RTIndex;
      this.m_RTIndex = num;
      this.m_RTIndex = EditorGUILayout.Popup(this.m_RTIndex, displayedOptions, EditorStyles.toolbarPopup, new GUILayoutOption[1]
      {
        GUILayout.Width(70f)
      });
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(10f);
      EditorGUI.BeginDisabledGroup(disabled);
      GUILayout.Label(FrameDebuggerWindow.Styles.channelHeader, EditorStyles.miniLabel, new GUILayoutOption[0]);
      this.m_RTChannel = GUILayout.Toolbar(this.m_RTChannel, FrameDebuggerWindow.Styles.channelLabels, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(10f);
      GUILayout.Label(FrameDebuggerWindow.Styles.levelsHeader, EditorStyles.miniLabel, new GUILayoutOption[0]);
      EditorGUILayout.MinMaxSlider(ref this.m_RTBlackLevel, ref this.m_RTWhiteLevel, 0.0f, 1f, GUILayout.MaxWidth(200f));
      if (EditorGUI.EndChangeCheck() || flag2)
      {
        Vector4 channels = Vector4.zero;
        if (this.m_RTChannel == 1)
          channels.x = 1f;
        else if (this.m_RTChannel == 2)
          channels.y = 1f;
        else if (this.m_RTChannel == 3)
          channels.z = 1f;
        else if (this.m_RTChannel == 4)
          channels.w = 1f;
        else
          channels = Vector4.one;
        int rtIndex = this.m_RTIndex;
        if (rtIndex >= (int) cur.rtCount)
          rtIndex = -1;
        FrameDebuggerUtility.SetRenderTargetDisplayOptions(rtIndex, channels, this.m_RTBlackLevel, this.m_RTWhiteLevel);
        this.RepaintAllNeededThings();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.Label(string.Format("{0}x{1} {2}", (object) cur.rtWidth, (object) cur.rtHeight, (object) (RenderTextureFormat) cur.rtFormat));
      if (cur.rtDim == 4)
        GUILayout.Label("Rendering into cubemap");
      if (cur.rtFormat != 3 || !SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 9"))
        return;
      EditorGUILayout.HelpBox("Rendering into shadowmap on DX9, can't visualize it in the game view properly", MessageType.Info, true);
    }

    private void DrawCurrentEvent(Rect rect, FrameDebuggerEvent[] descs)
    {
      int index = FrameDebuggerUtility.limit - 1;
      if (index < 0 || index >= descs.Length)
        return;
      GUILayout.BeginArea(rect);
      FrameDebuggerEvent desc = descs[index];
      FrameDebuggerEventData frameDebuggerEventData;
      bool frameEventData = FrameDebuggerUtility.GetFrameEventData(index, out frameDebuggerEventData);
      if (frameEventData)
        this.DrawRenderTargetControls(frameDebuggerEventData);
      GUILayout.Label(string.Format("Event #{0}: {1}", (object) (index + 1), (object) FrameDebuggerWindow.s_FrameEventTypeNames[(int) desc.type]), EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (FrameDebuggerUtility.IsRemoteEnabled() && FrameDebuggerUtility.receivingRemoteFrameEventData)
        GUILayout.Label("Receiving frame event data...");
      else if (frameEventData && (frameDebuggerEventData.vertexCount > 0 || frameDebuggerEventData.indexCount > 0))
      {
        Shader shader = frameDebuggerEventData.shader;
        int shaderPassIndex = frameDebuggerEventData.shaderPassIndex;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Shader: " + frameDebuggerEventData.shaderName + " pass #" + (object) shaderPassIndex, GUI.skin.label, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        {
          EditorGUIUtility.PingObject((UnityEngine.Object) shader);
          Event.current.Use();
        }
        GUILayout.Label(frameDebuggerEventData.shaderKeywords, EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        this.DrawStates(frameDebuggerEventData);
        GUILayout.Space(15f);
        this.m_AdditionalInfo = (ShowAdditionalInfo) GUILayout.Toolbar((int) this.m_AdditionalInfo, this.m_AdditionalInfoGuiContents);
        switch (this.m_AdditionalInfo)
        {
          case ShowAdditionalInfo.Preview:
            if (frameEventData && !this.DrawEventMesh(frameDebuggerEventData))
            {
              GUILayout.Label("Vertices: " + (object) frameDebuggerEventData.vertexCount);
              GUILayout.Label("Indices: " + (object) frameDebuggerEventData.indexCount);
              break;
            }
            break;
          case ShowAdditionalInfo.ShaderProperties:
            if (frameEventData)
            {
              this.DrawShaderProperties(frameDebuggerEventData.shaderProperties);
              break;
            }
            break;
        }
      }
      GUILayout.EndArea();
    }

    private void DrawShaderPropertyFlags(Rect flagsRect, int flags)
    {
      string empty = string.Empty;
      if ((flags & 2) != 0)
        empty += (string) (object) 'v';
      if ((flags & 4) != 0)
        empty += (string) (object) 'f';
      if ((flags & 8) != 0)
        empty += (string) (object) 'g';
      if ((flags & 16) != 0)
        empty += (string) (object) 'h';
      if ((flags & 32) != 0)
        empty += (string) (object) 'd';
      GUI.Label(flagsRect, empty, EditorStyles.miniLabel);
    }

    private void ShaderPropertyCopyValueMenu(Rect valueRect, object value)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FrameDebuggerWindow.\u003CShaderPropertyCopyValueMenu\u003Ec__AnonStoreyA6 menuCAnonStoreyA6 = new FrameDebuggerWindow.\u003CShaderPropertyCopyValueMenu\u003Ec__AnonStoreyA6();
      // ISSUE: reference to a compiler-generated field
      menuCAnonStoreyA6.value = value;
      Event current = Event.current;
      if (current.type != EventType.ContextClick || !valueRect.Contains(current.mousePosition))
        return;
      current.Use();
      GenericMenu genericMenu = new GenericMenu();
      // ISSUE: reference to a compiler-generated method
      genericMenu.AddItem(new GUIContent("Copy value"), false, new GenericMenu.MenuFunction(menuCAnonStoreyA6.\u003C\u003Em__1E7));
      genericMenu.ShowAsContext();
    }

    private void OnGUIShaderPropFloat(Rect nameRect, Rect flagsRect, Rect valueRect, ShaderFloatInfo t)
    {
      GUI.Label(nameRect, t.name, EditorStyles.miniLabel);
      this.DrawShaderPropertyFlags(flagsRect, t.flags);
      GUI.Label(valueRect, t.value.ToString("g2"), EditorStyles.miniLabel);
      this.ShaderPropertyCopyValueMenu(valueRect, (object) t.value);
    }

    private void OnGUIShaderPropVector4(Rect nameRect, Rect flagsRect, Rect valueRect, ShaderVectorInfo t)
    {
      GUI.Label(nameRect, t.name, EditorStyles.miniLabel);
      this.DrawShaderPropertyFlags(flagsRect, t.flags);
      GUI.Label(valueRect, t.value.ToString("g2"), EditorStyles.miniLabel);
      this.ShaderPropertyCopyValueMenu(valueRect, (object) t.value);
    }

    private void OnGUIShaderPropMatrix(Rect nameRect, Rect flagsRect, Rect valueRect, ShaderMatrixInfo t)
    {
      GUI.Label(nameRect, t.name, EditorStyles.miniLabel);
      this.DrawShaderPropertyFlags(flagsRect, t.flags);
      string text = t.value.ToString("g2");
      GUI.Label(valueRect, text, EditorStyles.miniLabel);
      this.ShaderPropertyCopyValueMenu(valueRect, (object) t.value);
    }

    private void OnGUIShaderPropTexture(Rect nameRect, Rect flagsRect, Rect valueRect, ShaderTextureInfo t)
    {
      GUI.Label(nameRect, t.name, EditorStyles.miniLabel);
      this.DrawShaderPropertyFlags(flagsRect, t.flags);
      if (Event.current.type == EventType.Repaint)
      {
        Rect position1 = valueRect;
        position1.width = position1.height;
        Rect position2 = valueRect;
        position2.xMin += position1.width;
        if ((UnityEngine.Object) t.value != (UnityEngine.Object) null)
          EditorGUI.DrawPreviewTexture(position1, t.value);
        GUI.Label(position2, !((UnityEngine.Object) t.value != (UnityEngine.Object) null) ? t.textureName : t.value.name);
      }
      else
      {
        if (Event.current.type != EventType.MouseDown || !valueRect.Contains(Event.current.mousePosition))
          return;
        EditorGUIUtility.PingObject((UnityEngine.Object) t.value);
        Event.current.Use();
      }
    }

    private void GetPropertyFieldRects(int count, float height, out Rect nameRect, out Rect flagsRect, out Rect valueRect)
    {
      Rect rect = GUILayoutUtility.GetRect(1f, height * (float) count);
      rect.height /= (float) count;
      rect.xMin += 15f;
      nameRect = rect;
      nameRect.width *= 0.4f;
      flagsRect = rect;
      flagsRect.width *= 0.1f;
      flagsRect.x += nameRect.width;
      valueRect = rect;
      valueRect.width *= 0.5f;
      valueRect.x += nameRect.width + flagsRect.width;
    }

    private void DrawShaderProperties(ShaderProperties props)
    {
      this.m_ScrollViewShaderProps = GUILayout.BeginScrollView(this.m_ScrollViewShaderProps);
      Rect nameRect;
      Rect flagsRect;
      Rect valueRect;
      if (((IEnumerable<ShaderTextureInfo>) props.textures).Count<ShaderTextureInfo>() > 0)
      {
        GUILayout.Label("Textures", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.GetPropertyFieldRects(((IEnumerable<ShaderTextureInfo>) props.textures).Count<ShaderTextureInfo>(), 16f, out nameRect, out flagsRect, out valueRect);
        foreach (ShaderTextureInfo texture in props.textures)
        {
          this.OnGUIShaderPropTexture(nameRect, flagsRect, valueRect, texture);
          nameRect.y += nameRect.height;
          flagsRect.y += flagsRect.height;
          valueRect.y += valueRect.height;
        }
      }
      if (((IEnumerable<ShaderFloatInfo>) props.floats).Count<ShaderFloatInfo>() > 0)
      {
        GUILayout.Label("Floats", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.GetPropertyFieldRects(((IEnumerable<ShaderFloatInfo>) props.floats).Count<ShaderFloatInfo>(), 16f, out nameRect, out flagsRect, out valueRect);
        foreach (ShaderFloatInfo t in props.floats)
        {
          this.OnGUIShaderPropFloat(nameRect, flagsRect, valueRect, t);
          nameRect.y += nameRect.height;
          flagsRect.y += flagsRect.height;
          valueRect.y += valueRect.height;
        }
      }
      if (((IEnumerable<ShaderVectorInfo>) props.vectors).Count<ShaderVectorInfo>() > 0)
      {
        GUILayout.Label("Vectors", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.GetPropertyFieldRects(((IEnumerable<ShaderVectorInfo>) props.vectors).Count<ShaderVectorInfo>(), 16f, out nameRect, out flagsRect, out valueRect);
        foreach (ShaderVectorInfo vector in props.vectors)
        {
          this.OnGUIShaderPropVector4(nameRect, flagsRect, valueRect, vector);
          nameRect.y += nameRect.height;
          flagsRect.y += flagsRect.height;
          valueRect.y += valueRect.height;
        }
      }
      if (((IEnumerable<ShaderMatrixInfo>) props.matrices).Count<ShaderMatrixInfo>() > 0)
      {
        GUILayout.Label("Matrices", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.GetPropertyFieldRects(((IEnumerable<ShaderMatrixInfo>) props.matrices).Count<ShaderMatrixInfo>(), 48f, out nameRect, out flagsRect, out valueRect);
        foreach (ShaderMatrixInfo matrix in props.matrices)
        {
          this.OnGUIShaderPropMatrix(nameRect, flagsRect, valueRect, matrix);
          nameRect.y += nameRect.height;
          flagsRect.y += flagsRect.height;
          valueRect.y += valueRect.height;
        }
      }
      GUILayout.EndScrollView();
    }

    private void DrawStates(FrameDebuggerEventData curEventData)
    {
      FrameDebuggerBlendState blendState = curEventData.blendState;
      FrameDebuggerRasterState rasterState = curEventData.rasterState;
      FrameDebuggerDepthState depthState = curEventData.depthState;
      string str = string.Empty;
      if ((int) blendState.renderTargetWriteMask == 0)
      {
        str = "0";
      }
      else
      {
        if (((int) blendState.renderTargetWriteMask & 2) != 0)
          str += "R";
        if (((int) blendState.renderTargetWriteMask & 4) != 0)
          str += "G";
        if (((int) blendState.renderTargetWriteMask & 8) != 0)
          str += "B";
        if (((int) blendState.renderTargetWriteMask & 1) != 0)
          str += "A";
      }
      GUILayout.Label(string.Format("Blend {0} {1}, {2} {3} ColorMask {4}", (object) blendState.srcBlend, (object) blendState.dstBlend, (object) blendState.srcBlendAlpha, (object) blendState.dstBlendAlpha, (object) str), EditorStyles.miniLabel, new GUILayoutOption[0]);
      GUILayout.Label(string.Format("ZTest {0} ZWrite {1} Cull {2} Offset {3}, {4}", (object) depthState.depthFunc, (object) (depthState.depthWrite != 0 ? "On" : "Off"), (object) rasterState.cullMode, (object) rasterState.slopeScaledDepthBias, (object) rasterState.depthBias), EditorStyles.miniLabel, new GUILayoutOption[0]);
    }

    internal void OnGUI()
    {
      FrameDebuggerEvent[] frameEvents = FrameDebuggerUtility.GetFrameEvents();
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      if (this.m_Tree == null)
      {
        this.m_Tree = new FrameDebuggerTreeView(frameEvents, this.m_TreeViewState, this, new Rect());
        this.m_FrameEventsHash = FrameDebuggerUtility.eventsHash;
        this.m_Tree.m_DataSource.SetExpandedWithChildren(this.m_Tree.m_DataSource.root, true);
      }
      if (FrameDebuggerUtility.eventsHash != this.m_FrameEventsHash)
      {
        this.m_Tree.m_DataSource.SetEvents(frameEvents);
        this.m_FrameEventsHash = FrameDebuggerUtility.eventsHash;
      }
      int limit = FrameDebuggerUtility.limit;
      bool flag = this.DrawToolbar(frameEvents);
      if (!FrameDebuggerUtility.IsLocalEnabled() && !FrameDebuggerUtility.IsRemoteEnabled() && this.m_AttachProfilerUI.IsEditor())
      {
        GUI.enabled = true;
        if (!FrameDebuggerUtility.locallySupported)
          EditorGUILayout.HelpBox("Frame Debugger requires multi-threaded renderer. Usually Unity uses that; if it does not, try starting with -force-gfx-mt command line argument.", MessageType.Warning, true);
        EditorGUILayout.HelpBox("Frame Debugger lets you step through draw calls and see how exactly frame is rendered. Click Enable!", MessageType.Info, true);
      }
      else
      {
        float fixedHeight = EditorStyles.toolbar.fixedHeight;
        Rect dragRect = EditorGUIUtility.HandleHorizontalSplitter(new Rect(this.m_ListWidth, fixedHeight, 5f, this.position.height - fixedHeight), this.position.width, 200f, 200f);
        this.m_ListWidth = dragRect.x;
        Rect rect1 = new Rect(0.0f, fixedHeight, this.m_ListWidth, this.position.height - fixedHeight);
        Rect rect2 = new Rect(this.m_ListWidth + 4f, fixedHeight + 4f, (float) ((double) this.position.width - (double) this.m_ListWidth - 8.0), (float) ((double) this.position.height - (double) fixedHeight - 8.0));
        this.DrawEventsTree(rect1);
        EditorGUIUtility.DrawHorizontalSplitter(dragRect);
        this.DrawCurrentEvent(rect2, frameEvents);
      }
      if (flag || limit != FrameDebuggerUtility.limit)
        this.RepaintOnLimitChange();
      if (this.m_RepaintFrames <= 0)
        return;
      this.m_Tree.SelectFrameEventIndex(FrameDebuggerUtility.limit);
      this.RepaintAllNeededThings();
      --this.m_RepaintFrames;
    }

    private void RepaintOnLimitChange()
    {
      this.m_RepaintFrames = 4;
      this.RepaintAllNeededThings();
    }

    private void RepaintAllNeededThings()
    {
      EditorApplication.SetSceneRepaintDirty();
      this.Repaint();
    }

    private void DrawEventsTree(Rect rect)
    {
      this.m_Tree.OnGUI(rect);
    }

    internal class Styles
    {
      public static readonly string[] s_ColumnNames = new string[4]{ "#", "Type", "Vertices", "Indices" };
      public static readonly GUIContent[] mrtLabels = new GUIContent[8]{ EditorGUIUtility.TextContent("RT 0|Show render target #0"), EditorGUIUtility.TextContent("RT 1|Show render target #1"), EditorGUIUtility.TextContent("RT 2|Show render target #2"), EditorGUIUtility.TextContent("RT 3|Show render target #3"), EditorGUIUtility.TextContent("RT 4|Show render target #4"), EditorGUIUtility.TextContent("RT 5|Show render target #5"), EditorGUIUtility.TextContent("RT 6|Show render target #6"), EditorGUIUtility.TextContent("RT 7|Show render target #7") };
      public static readonly GUIContent depthLabel = EditorGUIUtility.TextContent("Depth|Show depth buffer");
      public static readonly GUIContent[] channelLabels = new GUIContent[5]{ EditorGUIUtility.TextContent("All|Show all (RGB) color channels"), EditorGUIUtility.TextContent("R|Show red channel only"), EditorGUIUtility.TextContent("G|Show green channel only"), EditorGUIUtility.TextContent("B|Show blue channel only"), EditorGUIUtility.TextContent("A|Show alpha channel only") };
      public static readonly GUIContent channelHeader = EditorGUIUtility.TextContent("Channels|Which render target color channels to show");
      public static readonly GUIContent levelsHeader = EditorGUIUtility.TextContent("Levels|Render target display black/white intensity levels");
      public GUIStyle header = (GUIStyle) "OL title";
      public GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public GUIStyle rowText = (GUIStyle) "OL Label";
      public GUIStyle rowTextRight = new GUIStyle((GUIStyle) "OL Label");
      public GUIContent recordButton = new GUIContent(EditorGUIUtility.TextContent("Record|Record profiling information"));
      public GUIContent prevFrame = new GUIContent(EditorGUIUtility.IconContent("Profiler.PrevFrame", "|Go back one frame"));
      public GUIContent nextFrame = new GUIContent(EditorGUIUtility.IconContent("Profiler.NextFrame", "|Go one frame forwards"));
      public GUIContent[] headerContent;

      public Styles()
      {
        this.rowTextRight.alignment = TextAnchor.MiddleRight;
        this.recordButton.text = "Enable";
        this.recordButton.tooltip = "Enable Frame Debugging";
        this.prevFrame.tooltip = "Previous event";
        this.nextFrame.tooltip = "Next event";
        this.headerContent = new GUIContent[FrameDebuggerWindow.Styles.s_ColumnNames.Length];
        for (int index = 0; index < this.headerContent.Length; ++index)
          this.headerContent[index] = EditorGUIUtility.TextContent(FrameDebuggerWindow.Styles.s_ColumnNames[index]);
      }
    }
  }
}
