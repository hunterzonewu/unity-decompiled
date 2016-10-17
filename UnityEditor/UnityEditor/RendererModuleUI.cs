// Decompiled with JetBrains decompiler
// Type: UnityEditor.RendererModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class RendererModuleUI : ModuleUI
  {
    private SerializedProperty[] m_Meshes = new SerializedProperty[4];
    private const int k_MaxNumMeshes = 4;
    private SerializedProperty m_CastShadows;
    private SerializedProperty m_ReceiveShadows;
    private SerializedProperty m_Material;
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_SortingLayerID;
    private SerializedProperty m_RenderMode;
    private SerializedProperty[] m_ShownMeshes;
    private SerializedProperty m_MinParticleSize;
    private SerializedProperty m_MaxParticleSize;
    private SerializedProperty m_CameraVelocityScale;
    private SerializedProperty m_VelocityScale;
    private SerializedProperty m_LengthScale;
    private SerializedProperty m_SortMode;
    private SerializedProperty m_SortingFudge;
    private SerializedProperty m_NormalDirection;
    private RendererEditorBase.Probes m_Probes;
    private SerializedProperty m_RenderAlignment;
    private SerializedProperty m_Pivot;
    private static RendererModuleUI.Texts s_Texts;

    public RendererModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ParticleSystemRenderer", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
    {
      this.m_ToolTip = "Specifies how the particles are rendered.";
    }

    protected override void Init()
    {
      if (this.m_CastShadows != null)
        return;
      this.m_CastShadows = this.GetProperty0("m_CastShadows");
      this.m_ReceiveShadows = this.GetProperty0("m_ReceiveShadows");
      this.m_Material = this.GetProperty0("m_Materials.Array.data[0]");
      this.m_SortingOrder = this.GetProperty0("m_SortingOrder");
      this.m_SortingLayerID = this.GetProperty0("m_SortingLayerID");
      this.m_RenderMode = this.GetProperty0("m_RenderMode");
      this.m_MinParticleSize = this.GetProperty0("m_MinParticleSize");
      this.m_MaxParticleSize = this.GetProperty0("m_MaxParticleSize");
      this.m_CameraVelocityScale = this.GetProperty0("m_CameraVelocityScale");
      this.m_VelocityScale = this.GetProperty0("m_VelocityScale");
      this.m_LengthScale = this.GetProperty0("m_LengthScale");
      this.m_SortingFudge = this.GetProperty0("m_SortingFudge");
      this.m_SortMode = this.GetProperty0("m_SortMode");
      this.m_NormalDirection = this.GetProperty0("m_NormalDirection");
      this.m_Probes = new RendererEditorBase.Probes();
      this.m_Probes.Initialize(this.serializedObject, false);
      this.m_RenderAlignment = this.GetProperty0("m_RenderAlignment");
      this.m_Pivot = this.GetProperty0("m_Pivot");
      this.m_Meshes[0] = this.GetProperty0("m_Mesh");
      this.m_Meshes[1] = this.GetProperty0("m_Mesh1");
      this.m_Meshes[2] = this.GetProperty0("m_Mesh2");
      this.m_Meshes[3] = this.GetProperty0("m_Mesh3");
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      for (int index = 0; index < this.m_Meshes.Length; ++index)
      {
        if (index == 0 || this.m_Meshes[index].objectReferenceValue != (Object) null)
          serializedPropertyList.Add(this.m_Meshes[index]);
      }
      this.m_ShownMeshes = serializedPropertyList.ToArray();
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (RendererModuleUI.s_Texts == null)
        RendererModuleUI.s_Texts = new RendererModuleUI.Texts();
      RendererModuleUI.RenderMode intValue = (RendererModuleUI.RenderMode) this.m_RenderMode.intValue;
      RendererModuleUI.RenderMode renderMode = (RendererModuleUI.RenderMode) ModuleUI.GUIPopup(RendererModuleUI.s_Texts.renderMode, this.m_RenderMode, RendererModuleUI.s_Texts.particleTypes);
      if (renderMode == RendererModuleUI.RenderMode.Mesh)
      {
        ++EditorGUI.indentLevel;
        this.DoListOfMeshesGUI();
        --EditorGUI.indentLevel;
        if (intValue != RendererModuleUI.RenderMode.Mesh && this.m_Meshes[0].objectReferenceInstanceIDValue == 0)
          this.m_Meshes[0].objectReferenceValue = Resources.GetBuiltinResource(typeof (Mesh), "Cube.fbx");
      }
      else if (renderMode == RendererModuleUI.RenderMode.Stretch3D)
      {
        ++EditorGUI.indentLevel;
        double num1 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.cameraSpeedScale, this.m_CameraVelocityScale);
        double num2 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.speedScale, this.m_VelocityScale);
        double num3 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.lengthScale, this.m_LengthScale);
        --EditorGUI.indentLevel;
      }
      if (renderMode != RendererModuleUI.RenderMode.Mesh)
      {
        double num4 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.normalDirection, this.m_NormalDirection);
      }
      if (this.m_Material != null)
        ModuleUI.GUIObject(RendererModuleUI.s_Texts.material, this.m_Material);
      ModuleUI.GUIPopup(RendererModuleUI.s_Texts.sortMode, this.m_SortMode, RendererModuleUI.s_Texts.sortTypes);
      double num5 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.sortingFudge, this.m_SortingFudge);
      ModuleUI.GUIPopup(RendererModuleUI.s_Texts.castShadows, this.m_CastShadows, this.m_CastShadows.enumDisplayNames);
      EditorGUI.BeginDisabledGroup(SceneView.IsUsingDeferredRenderingPath());
      ModuleUI.GUIToggle(RendererModuleUI.s_Texts.receiveShadows, this.m_ReceiveShadows);
      EditorGUI.EndDisabledGroup();
      double num6 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.minParticleSize, this.m_MinParticleSize);
      double num7 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.maxParticleSize, this.m_MaxParticleSize);
      EditorGUILayout.Space();
      EditorGUILayout.SortingLayerField(RendererModuleUI.s_Texts.sortingLayer, this.m_SortingLayerID, ParticleSystemStyles.Get().popup, ParticleSystemStyles.Get().label);
      ModuleUI.GUIInt(RendererModuleUI.s_Texts.sortingOrder, this.m_SortingOrder);
      if (renderMode == RendererModuleUI.RenderMode.Billboard)
        ModuleUI.GUIPopup(RendererModuleUI.s_Texts.space, this.m_RenderAlignment, RendererModuleUI.s_Texts.spaces);
      ModuleUI.GUIVector3Field(RendererModuleUI.s_Texts.pivot, this.m_Pivot);
      this.m_Probes.OnGUI((Object[]) null, s.GetComponent<Renderer>(), true);
    }

    private void DoListOfMeshesGUI()
    {
      this.GUIListOfFloatObjectToggleFields(RendererModuleUI.s_Texts.mesh, this.m_ShownMeshes, (EditorGUI.ObjectFieldValidator) null, (GUIContent) null, false);
      Rect rect = GUILayoutUtility.GetRect(0.0f, 13f);
      rect.x = (float) ((double) rect.xMax - 24.0 - 5.0);
      rect.width = 12f;
      if (this.m_ShownMeshes.Length > 1 && ModuleUI.MinusButton(rect))
      {
        this.m_ShownMeshes[this.m_ShownMeshes.Length - 1].objectReferenceValue = (Object) null;
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownMeshes);
        serializedPropertyList.RemoveAt(serializedPropertyList.Count - 1);
        this.m_ShownMeshes = serializedPropertyList.ToArray();
      }
      if (this.m_ShownMeshes.Length >= 4)
        return;
      rect.x += 17f;
      if (!ModuleUI.PlusButton(rect))
        return;
      List<SerializedProperty> serializedPropertyList1 = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownMeshes);
      serializedPropertyList1.Add(this.m_Meshes[serializedPropertyList1.Count]);
      this.m_ShownMeshes = serializedPropertyList1.ToArray();
    }

    public bool IsMeshEmitter()
    {
      if (this.m_RenderMode != null)
        return this.m_RenderMode.intValue == 4;
      return false;
    }

    private enum RenderMode
    {
      Billboard,
      Stretch3D,
      BillboardFixedHorizontal,
      BillboardFixedVertical,
      Mesh,
    }

    private class Texts
    {
      public GUIContent renderMode = new GUIContent("Render Mode", "Defines the render mode of the particle renderer.");
      public GUIContent material = new GUIContent("Material", "Defines the material used to render particles.");
      public GUIContent mesh = new GUIContent("Mesh", "Defines the mesh that will be rendered as particle.");
      public GUIContent minParticleSize = new GUIContent("Min Particle Size", "How small is a particle allowed to be on screen at least? 1 is entire viewport. 0.5 is half viewport.");
      public GUIContent maxParticleSize = new GUIContent("Max Particle Size", "How large is a particle allowed to be on screen at most? 1 is entire viewport. 0.5 is half viewport.");
      public GUIContent cameraSpeedScale = new GUIContent("Camera Scale", "How much the camera speed is factored in when determining particle stretching.");
      public GUIContent speedScale = new GUIContent("Speed Scale", "Defines the length of the particle compared to its speed.");
      public GUIContent lengthScale = new GUIContent("Length Scale", "Defines the length of the particle compared to its width.");
      public GUIContent sortingFudge = new GUIContent("Sorting Fudge", "Lower the number and most likely these particles will appear in front of other transparent objects, including other particles.");
      public GUIContent sortMode = new GUIContent("Sort Mode", "The draw order of particles can be sorted by distance, oldest in front, or youngest in front.");
      public GUIContent rotation = new GUIContent("Rotation", "Set whether the rotation of the particles is defined in Screen or World space.");
      public GUIContent castShadows = new GUIContent("Cast Shadows", "Only opaque materials cast shadows");
      public GUIContent receiveShadows = new GUIContent("Receive Shadows", "Only opaque materials receive shadows");
      public GUIContent normalDirection = new GUIContent("Normal Direction", "Value between 0.0 and 1.0. If 1.0 is used, normals will point towards camera. If 0.0 is used, normals will point out in the corner direction of the particle.");
      public GUIContent sortingLayer = EditorGUIUtility.TextContent("Sorting Layer");
      public GUIContent sortingOrder = EditorGUIUtility.TextContent("Order in Layer");
      public GUIContent space = new GUIContent("Billboard Alignment", "Specifies if the particles will face the camera, align to world axes, or stay local to the system's transform.");
      public GUIContent pivot = new GUIContent("Pivot", "Applies an offset to the pivot of particles, as a multiplier of its size.");
      public string[] particleTypes = new string[5]{ "Billboard", "Stretched Billboard", "Horizontal Billboard", "Vertical Billboard", "Mesh" };
      public string[] sortTypes = new string[4]{ "None", "By Distance", "Oldest in Front", "Youngest in Front" };
      public string[] spaces = new string[3]{ "View", "World", "Local" };
    }
  }
}
