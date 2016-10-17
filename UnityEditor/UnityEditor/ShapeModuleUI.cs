// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ShapeModuleUI : ModuleUI
  {
    private static int s_BoxHash = "BoxColliderEditor".GetHashCode();
    private static Color s_ShapeGizmoColor = new Color(0.5803922f, 0.8980392f, 1f, 0.9f);
    private static ShapeModuleUI.Texts s_Texts = new ShapeModuleUI.Texts();
    private BoxEditor m_BoxEditor = new BoxEditor(true, ShapeModuleUI.s_BoxHash);
    private string[] m_GuiNames = new string[9]{ "Sphere", "Hemisphere", "Cone", "Box", "Mesh", "Mesh Renderer", "Skinned Mesh Renderer", "Circle", "Edge" };
    private ShapeModuleUI.ShapeTypes[] m_GuiTypes = new ShapeModuleUI.ShapeTypes[9]{ ShapeModuleUI.ShapeTypes.Sphere, ShapeModuleUI.ShapeTypes.Hemisphere, ShapeModuleUI.ShapeTypes.Cone, ShapeModuleUI.ShapeTypes.Box, ShapeModuleUI.ShapeTypes.Mesh, ShapeModuleUI.ShapeTypes.MeshRenderer, ShapeModuleUI.ShapeTypes.SkinnedMeshRenderer, ShapeModuleUI.ShapeTypes.Circle, ShapeModuleUI.ShapeTypes.SingleSidedEdge };
    private int[] m_TypeToGuiTypeIndex = new int[15]{ 0, 0, 1, 1, 2, 3, 4, 2, 2, 2, 7, 7, 8, 5, 6 };
    private SerializedProperty m_Type;
    private SerializedProperty m_RandomDirection;
    private SerializedProperty m_Radius;
    private SerializedProperty m_Angle;
    private SerializedProperty m_Length;
    private SerializedProperty m_BoxX;
    private SerializedProperty m_BoxY;
    private SerializedProperty m_BoxZ;
    private SerializedProperty m_Arc;
    private SerializedProperty m_PlacementMode;
    private SerializedProperty m_Mesh;
    private SerializedProperty m_MeshRenderer;
    private SerializedProperty m_SkinnedMeshRenderer;
    private SerializedProperty m_MeshMaterialIndex;
    private SerializedProperty m_UseMeshMaterialIndex;
    private SerializedProperty m_UseMeshColors;
    private SerializedProperty m_MeshNormalOffset;
    private Material m_Material;

    public ShapeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ShapeModule", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
    {
      this.m_ToolTip = "Shape of the emitter volume, which controls where particles are emitted and their initial direction.";
    }

    protected override void Init()
    {
      if (this.m_Type != null)
        return;
      if (ShapeModuleUI.s_Texts == null)
        ShapeModuleUI.s_Texts = new ShapeModuleUI.Texts();
      this.m_Type = this.GetProperty("type");
      this.m_Radius = this.GetProperty("radius");
      this.m_Angle = this.GetProperty("angle");
      this.m_Length = this.GetProperty("length");
      this.m_BoxX = this.GetProperty("boxX");
      this.m_BoxY = this.GetProperty("boxY");
      this.m_BoxZ = this.GetProperty("boxZ");
      this.m_Arc = this.GetProperty("arc");
      this.m_PlacementMode = this.GetProperty("placementMode");
      this.m_Mesh = this.GetProperty("m_Mesh");
      this.m_MeshRenderer = this.GetProperty("m_MeshRenderer");
      this.m_SkinnedMeshRenderer = this.GetProperty("m_SkinnedMeshRenderer");
      this.m_MeshMaterialIndex = this.GetProperty("m_MeshMaterialIndex");
      this.m_UseMeshMaterialIndex = this.GetProperty("m_UseMeshMaterialIndex");
      this.m_UseMeshColors = this.GetProperty("m_UseMeshColors");
      this.m_MeshNormalOffset = this.GetProperty("m_MeshNormalOffset");
      this.m_RandomDirection = this.GetProperty("randomDirection");
      this.m_Material = EditorGUIUtility.GetBuiltinExtraResource(typeof (Material), "Default-Material.mat") as Material;
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
    }

    public override float GetXAxisScalar()
    {
      return this.m_ParticleSystemUI.GetEmitterDuration();
    }

    private ShapeModuleUI.ShapeTypes ConvertConeEmitFromToConeType(int emitFrom)
    {
      if (emitFrom == 0)
        return ShapeModuleUI.ShapeTypes.Cone;
      if (emitFrom == 1)
        return ShapeModuleUI.ShapeTypes.ConeShell;
      return emitFrom == 2 ? ShapeModuleUI.ShapeTypes.ConeVolume : ShapeModuleUI.ShapeTypes.ConeVolumeShell;
    }

    private int ConvertConeTypeToConeEmitFrom(ShapeModuleUI.ShapeTypes shapeType)
    {
      if (shapeType == ShapeModuleUI.ShapeTypes.Cone)
        return 0;
      if (shapeType == ShapeModuleUI.ShapeTypes.ConeShell)
        return 1;
      if (shapeType == ShapeModuleUI.ShapeTypes.ConeVolume)
        return 2;
      return shapeType == ShapeModuleUI.ShapeTypes.ConeVolumeShell ? 3 : 0;
    }

    private bool GetUsesShell(ShapeModuleUI.ShapeTypes shapeType)
    {
      return shapeType == ShapeModuleUI.ShapeTypes.HemisphereShell || shapeType == ShapeModuleUI.ShapeTypes.SphereShell || (shapeType == ShapeModuleUI.ShapeTypes.ConeShell || shapeType == ShapeModuleUI.ShapeTypes.ConeVolumeShell) || shapeType == ShapeModuleUI.ShapeTypes.CircleEdge;
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (ShapeModuleUI.s_Texts == null)
        ShapeModuleUI.s_Texts = new ShapeModuleUI.Texts();
      int index1 = this.m_Type.intValue;
      int intValue = this.m_TypeToGuiTypeIndex[index1];
      bool usesShell = this.GetUsesShell((ShapeModuleUI.ShapeTypes) index1);
      int index2 = ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.shape, intValue, this.m_GuiNames);
      ShapeModuleUI.ShapeTypes guiType = this.m_GuiTypes[index2];
      if (index2 != intValue)
        index1 = (int) guiType;
      switch (guiType)
      {
        case ShapeModuleUI.ShapeTypes.Sphere:
          double num1 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius);
          index1 = !ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.emitFromShell, usesShell) ? 0 : 1;
          break;
        case ShapeModuleUI.ShapeTypes.Hemisphere:
          double num2 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius);
          index1 = !ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.emitFromShell, usesShell) ? 2 : 3;
          break;
        case ShapeModuleUI.ShapeTypes.Cone:
          double num3 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneAngle, this.m_Angle);
          double num4 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius);
          EditorGUI.BeginDisabledGroup((index1 == 8 ? 1 : (index1 == 9 ? 1 : 0)) == 0);
          double num5 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneLength, this.m_Length);
          EditorGUI.EndDisabledGroup();
          string[] options1 = new string[4]{ "Base", "Base Shell", "Volume", "Volume Shell" };
          int coneEmitFrom = this.ConvertConeTypeToConeEmitFrom((ShapeModuleUI.ShapeTypes) index1);
          index1 = (int) this.ConvertConeEmitFromToConeType(ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.emitFrom, coneEmitFrom, options1));
          break;
        case ShapeModuleUI.ShapeTypes.Box:
          double num6 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.boxX, this.m_BoxX);
          double num7 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.boxY, this.m_BoxY);
          double num8 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.boxZ, this.m_BoxZ);
          break;
        case ShapeModuleUI.ShapeTypes.Mesh:
        case ShapeModuleUI.ShapeTypes.MeshRenderer:
        case ShapeModuleUI.ShapeTypes.SkinnedMeshRenderer:
          string[] options2 = new string[3]{ "Vertex", "Edge", "Triangle" };
          ModuleUI.GUIPopup(string.Empty, this.m_PlacementMode, options2);
          if (guiType == ShapeModuleUI.ShapeTypes.Mesh)
            ModuleUI.GUIObject(ShapeModuleUI.s_Texts.mesh, this.m_Mesh);
          else if (guiType == ShapeModuleUI.ShapeTypes.MeshRenderer)
            ModuleUI.GUIObject(ShapeModuleUI.s_Texts.meshRenderer, this.m_MeshRenderer);
          else
            ModuleUI.GUIObject(ShapeModuleUI.s_Texts.skinnedMeshRenderer, this.m_SkinnedMeshRenderer);
          ModuleUI.GUIToggleWithIntField(ShapeModuleUI.s_Texts.meshMaterialIndex, this.m_UseMeshMaterialIndex, this.m_MeshMaterialIndex, false);
          ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.useMeshColors, this.m_UseMeshColors);
          double num9 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.meshNormalOffset, this.m_MeshNormalOffset);
          break;
        case ShapeModuleUI.ShapeTypes.Circle:
          double num10 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius);
          double num11 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.arc, this.m_Arc);
          index1 = !ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.emitFromEdge, usesShell) ? 10 : 11;
          break;
        case ShapeModuleUI.ShapeTypes.SingleSidedEdge:
          double num12 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius);
          break;
      }
      this.m_Type.intValue = index1;
      ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.randomDirection, this.m_RandomDirection);
    }

    public override void OnSceneGUI(ParticleSystem s, InitialModuleUI initial)
    {
      Color color = Handles.color;
      Handles.color = ShapeModuleUI.s_ShapeGizmoColor;
      Matrix4x4 matrix = Handles.matrix;
      Matrix4x4 transform = new Matrix4x4();
      transform.SetTRS(s.transform.position, s.transform.rotation, s.transform.lossyScale);
      Handles.matrix = transform;
      EditorGUI.BeginChangeCheck();
      int intValue = this.m_Type.intValue;
      if (intValue == 0 || intValue == 1)
        this.m_Radius.floatValue = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, this.m_Radius.floatValue, false);
      if (intValue == 10 || intValue == 11)
      {
        float floatValue1 = this.m_Radius.floatValue;
        float floatValue2 = this.m_Arc.floatValue;
        Handles.DoSimpleRadiusArcHandleXY(Quaternion.identity, Vector3.zero, ref floatValue1, ref floatValue2);
        this.m_Radius.floatValue = floatValue1;
        this.m_Arc.floatValue = floatValue2;
      }
      else if (intValue == 2 || intValue == 3)
        this.m_Radius.floatValue = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, this.m_Radius.floatValue, true);
      else if (intValue == 4 || intValue == 7)
      {
        Vector3 vector3 = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, new Vector3(this.m_Radius.floatValue, this.m_Angle.floatValue, initial.m_Speed.scalar.floatValue));
        this.m_Radius.floatValue = vector3.x;
        this.m_Angle.floatValue = vector3.y;
        initial.m_Speed.scalar.floatValue = vector3.z;
      }
      else if (intValue == 8 || intValue == 9)
      {
        Vector3 vector3 = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, new Vector3(this.m_Radius.floatValue, this.m_Angle.floatValue, this.m_Length.floatValue));
        this.m_Radius.floatValue = vector3.x;
        this.m_Angle.floatValue = vector3.y;
        this.m_Length.floatValue = vector3.z;
      }
      else if (intValue == 5)
      {
        Vector3 zero = Vector3.zero;
        Vector3 size = new Vector3(this.m_BoxX.floatValue, this.m_BoxY.floatValue, this.m_BoxZ.floatValue);
        if (this.m_BoxEditor.OnSceneGUI(transform, ShapeModuleUI.s_ShapeGizmoColor, false, ref zero, ref size))
        {
          this.m_BoxX.floatValue = size.x;
          this.m_BoxY.floatValue = size.y;
          this.m_BoxZ.floatValue = size.z;
        }
      }
      else if (intValue == 12)
        this.m_Radius.floatValue = Handles.DoSimpleEdgeHandle(Quaternion.identity, Vector3.zero, this.m_Radius.floatValue);
      else if (intValue == 6)
      {
        Mesh objectReferenceValue = (Mesh) this.m_Mesh.objectReferenceValue;
        if ((bool) ((Object) objectReferenceValue))
        {
          bool wireframe = GL.wireframe;
          GL.wireframe = true;
          this.m_Material.SetPass(0);
          Graphics.DrawMeshNow(objectReferenceValue, s.transform.localToWorldMatrix);
          GL.wireframe = wireframe;
        }
      }
      if (EditorGUI.EndChangeCheck())
        this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
      Handles.color = color;
      Handles.matrix = matrix;
    }

    private enum ShapeTypes
    {
      Sphere,
      SphereShell,
      Hemisphere,
      HemisphereShell,
      Cone,
      Box,
      Mesh,
      ConeShell,
      ConeVolume,
      ConeVolumeShell,
      Circle,
      CircleEdge,
      SingleSidedEdge,
      MeshRenderer,
      SkinnedMeshRenderer,
    }

    private class Texts
    {
      public GUIContent shape = new GUIContent("Shape", "Defines the shape of the volume from which particles can be emitted, and the direction of the start velocity.");
      public GUIContent radius = new GUIContent("Radius", "Radius of the shape.");
      public GUIContent coneAngle = new GUIContent("Angle", "Angle of the cone.");
      public GUIContent coneLength = new GUIContent("Length", "Length of the cone.");
      public GUIContent boxX = new GUIContent("Box X", "Scale of the box in X Axis.");
      public GUIContent boxY = new GUIContent("Box Y", "Scale of the box in Y Axis.");
      public GUIContent boxZ = new GUIContent("Box Z", "Scale of the box in Z Axis.");
      public GUIContent mesh = new GUIContent("Mesh", "Mesh that the particle system will emit from.");
      public GUIContent meshRenderer = new GUIContent("Mesh", "MeshRenderer that the particle system will emit from.");
      public GUIContent skinnedMeshRenderer = new GUIContent("Mesh", "SkinnedMeshRenderer that the particle system will emit from.");
      public GUIContent meshMaterialIndex = new GUIContent("Single Material", "Only emit from a specific material of the mesh.");
      public GUIContent useMeshColors = EditorGUIUtility.TextContent("Use Mesh Colors|Modulate particle color with mesh vertex colors, or if they don't exist, use the shader color property \"_Color\" or \"_TintColor\" from the material.");
      public GUIContent meshNormalOffset = new GUIContent("Normal Offset", "Offset particle spawn positions along the mesh normal.");
      public GUIContent randomDirection = new GUIContent("Random Direction", "Randomizes the starting direction of particles.");
      public GUIContent emitFromShell = new GUIContent("Emit from Shell", "Emit from shell of the sphere. If disabled particles will be emitted from the volume of the shape.");
      public GUIContent emitFromEdge = new GUIContent("Emit from Edge", "Emit from edge of the shape. If disabled particles will be emitted from the volume of the shape.");
      public GUIContent emitFrom = new GUIContent("Emit from:", "Specifies from where particles are emitted.");
      public GUIContent arc = new GUIContent("Arc", "Circle arc angle.");
    }
  }
}
