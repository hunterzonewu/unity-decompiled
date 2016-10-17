// Decompiled with JetBrains decompiler
// Type: UnityEditor.CollisionModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class CollisionModuleUI : ModuleUI
  {
    private string[] m_PlaneVizTypeNames = new string[2]{ "Grid", "Solid" };
    private SerializedProperty[] m_Planes = new SerializedProperty[6];
    private CollisionModuleUI.PlaneVizType m_PlaneVisualizationType = CollisionModuleUI.PlaneVizType.Solid;
    private float m_ScaleGrid = 1f;
    private const int k_MaxNumPlanes = 6;
    private SerializedProperty m_Type;
    private SerializedMinMaxCurve m_Dampen;
    private SerializedMinMaxCurve m_Bounce;
    private SerializedMinMaxCurve m_LifetimeLossOnCollision;
    private SerializedProperty m_MinKillSpeed;
    private SerializedProperty m_RadiusScale;
    private SerializedProperty m_CollidesWith;
    private SerializedProperty m_CollidesWithDynamic;
    private SerializedProperty m_InteriorCollisions;
    private SerializedProperty m_MaxCollisionShapes;
    private SerializedProperty m_Quality;
    private SerializedProperty m_VoxelSize;
    private SerializedProperty m_CollisionMessages;
    private SerializedProperty m_CollisionMode;
    private SerializedProperty[] m_ShownPlanes;
    private bool m_VisualizeBounds;
    private static Transform m_SelectedTransform;
    private static CollisionModuleUI s_LastInteractedEditor;
    private static CollisionModuleUI.Texts s_Texts;

    public CollisionModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "CollisionModule", displayName)
    {
      this.m_ToolTip = "Allows you to specify multiple collision planes that the particle can collide with.";
    }

    protected override void Init()
    {
      if (this.m_Type != null)
        return;
      if (CollisionModuleUI.s_Texts == null)
        CollisionModuleUI.s_Texts = new CollisionModuleUI.Texts();
      this.m_Type = this.GetProperty("type");
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      for (int index = 0; index < this.m_Planes.Length; ++index)
      {
        this.m_Planes[index] = this.GetProperty("plane" + (object) index);
        if (index == 0 || this.m_Planes[index].objectReferenceValue != (Object) null)
          serializedPropertyList.Add(this.m_Planes[index]);
      }
      this.m_ShownPlanes = serializedPropertyList.ToArray();
      this.m_Dampen = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.dampen, "m_Dampen");
      this.m_Dampen.m_AllowCurves = false;
      this.m_Bounce = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.bounce, "m_Bounce");
      this.m_Bounce.m_AllowCurves = false;
      this.m_LifetimeLossOnCollision = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.lifetimeLoss, "m_EnergyLossOnCollision");
      this.m_LifetimeLossOnCollision.m_AllowCurves = false;
      this.m_MinKillSpeed = this.GetProperty("minKillSpeed");
      this.m_RadiusScale = this.GetProperty("radiusScale");
      this.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType) EditorPrefs.GetInt("PlaneColisionVizType", 1);
      this.m_ScaleGrid = EditorPrefs.GetFloat("ScalePlaneColision", 1f);
      this.m_VisualizeBounds = EditorPrefs.GetBool("VisualizeBounds", false);
      this.m_CollidesWith = this.GetProperty("collidesWith");
      this.m_CollidesWithDynamic = this.GetProperty("collidesWithDynamic");
      this.m_InteriorCollisions = this.GetProperty("interiorCollisions");
      this.m_MaxCollisionShapes = this.GetProperty("maxCollisionShapes");
      this.m_Quality = this.GetProperty("quality");
      this.m_VoxelSize = this.GetProperty("voxelSize");
      this.m_CollisionMessages = this.GetProperty("collisionMessages");
      this.m_CollisionMode = this.GetProperty("collisionMode");
      this.SyncVisualization();
    }

    protected override void SetVisibilityState(ModuleUI.VisibilityState newState)
    {
      base.SetVisibilityState(newState);
      if (newState != ModuleUI.VisibilityState.VisibleAndFoldedOut)
      {
        Tools.s_Hidden = false;
        CollisionModuleUI.m_SelectedTransform = (Transform) null;
        ParticleEffectUtils.ClearPlanes();
      }
      else
        this.SyncVisualization();
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (CollisionModuleUI.s_Texts == null)
        CollisionModuleUI.s_Texts = new CollisionModuleUI.Texts();
      string[] options = new string[2]{ "Planes", "World" };
      CollisionModuleUI.CollisionTypes collisionTypes = (CollisionModuleUI.CollisionTypes) ModuleUI.GUIPopup(string.Empty, this.m_Type, options);
      CollisionModuleUI.CollisionModes collisionModes = CollisionModuleUI.CollisionModes.Mode3D;
      if (collisionTypes == CollisionModuleUI.CollisionTypes.Plane)
      {
        this.DoListOfPlanesGUI();
        EditorGUI.BeginChangeCheck();
        this.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType) ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.visualization, (int) this.m_PlaneVisualizationType, this.m_PlaneVizTypeNames);
        if (EditorGUI.EndChangeCheck())
        {
          EditorPrefs.SetInt("PlaneColisionVizType", (int) this.m_PlaneVisualizationType);
          if (this.m_PlaneVisualizationType == CollisionModuleUI.PlaneVizType.Solid)
            this.SyncVisualization();
          else
            ParticleEffectUtils.ClearPlanes();
        }
        EditorGUI.BeginChangeCheck();
        this.m_ScaleGrid = ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.scalePlane, this.m_ScaleGrid, "f2");
        if (EditorGUI.EndChangeCheck())
        {
          this.m_ScaleGrid = Mathf.Max(0.0f, this.m_ScaleGrid);
          EditorPrefs.SetFloat("ScalePlaneColision", this.m_ScaleGrid);
          this.SyncVisualization();
        }
      }
      else
        collisionModes = (CollisionModuleUI.CollisionModes) ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.collisionMode, this.m_CollisionMode, new string[2]{ "3D", "2D" });
      EditorGUI.BeginChangeCheck();
      this.m_VisualizeBounds = ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.visualizeBounds, this.m_VisualizeBounds);
      if (EditorGUI.EndChangeCheck())
        EditorPrefs.SetBool("VisualizeBounds", this.m_VisualizeBounds);
      CollisionModuleUI.s_LastInteractedEditor = this;
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.dampen, this.m_Dampen);
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.bounce, this.m_Bounce);
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.lifetimeLoss, this.m_LifetimeLossOnCollision);
      double num1 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.minKillSpeed, this.m_MinKillSpeed);
      if (collisionTypes != CollisionModuleUI.CollisionTypes.World || collisionModes == CollisionModuleUI.CollisionModes.Mode2D)
      {
        double num2 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.radiusScale, this.m_RadiusScale);
      }
      if (collisionTypes == CollisionModuleUI.CollisionTypes.World)
      {
        ModuleUI.GUILayerMask(CollisionModuleUI.s_Texts.collidesWith, this.m_CollidesWith);
        ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collidesWithDynamic, this.m_CollidesWithDynamic);
        if (collisionModes == CollisionModuleUI.CollisionModes.Mode3D)
          ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.interiorCollisions, this.m_InteriorCollisions);
        ModuleUI.GUIInt(CollisionModuleUI.s_Texts.maxCollisionShapes, this.m_MaxCollisionShapes);
        ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.quality, this.m_Quality, CollisionModuleUI.s_Texts.qualitySettings);
        if (this.m_Quality.intValue > 0)
        {
          double num3 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.voxelSize, this.m_VoxelSize);
        }
      }
      ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collisionMessages, this.m_CollisionMessages);
    }

    protected override void OnModuleEnable()
    {
      base.OnModuleEnable();
      this.SyncVisualization();
    }

    protected override void OnModuleDisable()
    {
      base.OnModuleDisable();
      ParticleEffectUtils.ClearPlanes();
    }

    private void SyncVisualization()
    {
      if (!this.enabled || this.m_PlaneVisualizationType != CollisionModuleUI.PlaneVizType.Solid)
        return;
      for (int index = 0; index < this.m_ShownPlanes.Length; ++index)
      {
        Object objectReferenceValue = this.m_ShownPlanes[index].objectReferenceValue;
        if (!(objectReferenceValue == (Object) null))
        {
          Transform transform = objectReferenceValue as Transform;
          if (!((Object) transform == (Object) null))
          {
            GameObject plane = ParticleEffectUtils.GetPlane(index);
            plane.transform.position = transform.position;
            plane.transform.rotation = transform.rotation;
            plane.transform.localScale = new Vector3(this.m_ScaleGrid, this.m_ScaleGrid, this.m_ScaleGrid);
            plane.transform.position += transform.up.normalized * (1f / 500f);
          }
        }
      }
    }

    private static GameObject CreateEmptyGameObject(string name, ParticleSystem parentOfGameObject)
    {
      GameObject gameObject = new GameObject(name);
      if (!(bool) ((Object) gameObject))
        return (GameObject) null;
      if ((bool) ((Object) parentOfGameObject))
        gameObject.transform.parent = parentOfGameObject.transform;
      return gameObject;
    }

    private void DoListOfPlanesGUI()
    {
      int index = this.GUIListOfFloatObjectToggleFields(CollisionModuleUI.s_Texts.planes, this.m_ShownPlanes, (EditorGUI.ObjectFieldValidator) null, CollisionModuleUI.s_Texts.createPlane, true);
      if (index >= 0)
      {
        GameObject emptyGameObject = CollisionModuleUI.CreateEmptyGameObject("Plane Transform " + (object) (index + 1), this.m_ParticleSystemUI.m_ParticleSystem);
        emptyGameObject.transform.localPosition = new Vector3(0.0f, 0.0f, (float) (10 + index));
        emptyGameObject.transform.localEulerAngles = new Vector3(-90f, 0.0f, 0.0f);
        this.m_ShownPlanes[index].objectReferenceValue = (Object) emptyGameObject;
        this.SyncVisualization();
      }
      Rect rect = GUILayoutUtility.GetRect(0.0f, 16f);
      rect.x = (float) ((double) rect.xMax - 24.0 - 5.0);
      rect.width = 12f;
      if (this.m_ShownPlanes.Length > 1 && ModuleUI.MinusButton(rect))
      {
        this.m_ShownPlanes[this.m_ShownPlanes.Length - 1].objectReferenceValue = (Object) null;
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownPlanes);
        serializedPropertyList.RemoveAt(serializedPropertyList.Count - 1);
        this.m_ShownPlanes = serializedPropertyList.ToArray();
      }
      if (this.m_ShownPlanes.Length >= 6)
        return;
      rect.x += 17f;
      if (!ModuleUI.PlusButton(rect))
        return;
      List<SerializedProperty> serializedPropertyList1 = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownPlanes);
      serializedPropertyList1.Add(this.m_Planes[serializedPropertyList1.Count]);
      this.m_ShownPlanes = serializedPropertyList1.ToArray();
    }

    public override void OnSceneGUI(ParticleSystem s, InitialModuleUI initial)
    {
      Event current = Event.current;
      EventType eventType = current.type;
      if (current.type == EventType.Ignore && current.rawType == EventType.MouseUp)
        eventType = current.rawType;
      Color color1 = Handles.color;
      Handles.color = new Color(1f, 1f, 1f, 0.5f);
      if (this.m_Type.intValue == 0)
      {
        for (int index = 0; index < this.m_ShownPlanes.Length; ++index)
        {
          Object objectReferenceValue = this.m_ShownPlanes[index].objectReferenceValue;
          if (objectReferenceValue != (Object) null)
          {
            Transform transform = objectReferenceValue as Transform;
            if ((Object) transform != (Object) null)
            {
              Vector3 position = transform.position;
              Quaternion rotation = transform.rotation;
              Vector3 axis1 = rotation * Vector3.right;
              Vector3 normal = rotation * Vector3.up;
              Vector3 axis2 = rotation * Vector3.forward;
              if (object.ReferenceEquals((object) CollisionModuleUI.m_SelectedTransform, (object) transform))
              {
                Tools.s_Hidden = true;
                EditorGUI.BeginChangeCheck();
                if (Tools.current == Tool.Move)
                  transform.position = Handles.PositionHandle(position, rotation);
                else if (Tools.current == Tool.Rotate)
                  transform.rotation = Handles.RotationHandle(rotation, position);
                if (EditorGUI.EndChangeCheck())
                {
                  if (this.m_PlaneVisualizationType == CollisionModuleUI.PlaneVizType.Solid)
                  {
                    GameObject plane = ParticleEffectUtils.GetPlane(index);
                    plane.transform.position = position;
                    plane.transform.rotation = rotation;
                    plane.transform.localScale = new Vector3(this.m_ScaleGrid, this.m_ScaleGrid, this.m_ScaleGrid);
                  }
                  ParticleSystemEditorUtils.PerformCompleteResimulation();
                }
              }
              else
              {
                int keyboardControl = GUIUtility.keyboardControl;
                float size = HandleUtility.GetHandleSize(position) * 0.06f;
                Handles.FreeMoveHandle(position, Quaternion.identity, size, Vector3.zero, new Handles.DrawCapFunction(Handles.RectangleCap));
                if (eventType == EventType.MouseDown && current.type == EventType.Used && keyboardControl != GUIUtility.keyboardControl)
                {
                  CollisionModuleUI.m_SelectedTransform = transform;
                  eventType = EventType.Used;
                }
              }
              if (this.m_PlaneVisualizationType == CollisionModuleUI.PlaneVizType.Grid)
              {
                Color color2 = Handles.s_ColliderHandleColor * 0.9f;
                if (!this.enabled)
                  color2 = new Color(0.7f, 0.7f, 0.7f, 0.7f);
                this.DrawGrid(position, axis1, axis2, normal, color2, index);
              }
              else
                this.DrawSolidPlane(position, rotation, index);
            }
            else
              Debug.LogError((object) ("Not a transform: " + (object) objectReferenceValue.GetType()));
          }
        }
      }
      Handles.color = color1;
    }

    [DrawGizmo(GizmoType.Active)]
    private static void RenderCollisionBounds(ParticleSystem system, GizmoType gizmoType)
    {
      if (CollisionModuleUI.s_LastInteractedEditor == null || !CollisionModuleUI.s_LastInteractedEditor.m_VisualizeBounds || (Object) CollisionModuleUI.s_LastInteractedEditor.m_ParticleSystemUI.m_ParticleSystem != (Object) system)
        return;
      ParticleSystem.Particle[] particles1 = new ParticleSystem.Particle[system.particleCount];
      int particles2 = system.GetParticles(particles1);
      Color color = Gizmos.color;
      Gizmos.color = Color.green;
      Matrix4x4 matrix4x4 = Matrix4x4.identity;
      if (system.simulationSpace == ParticleSystemSimulationSpace.Local)
        matrix4x4 = system.transform.localToWorldMatrix;
      for (int index = 0; index < particles2; ++index)
      {
        ParticleSystem.Particle particle = particles1[index];
        Gizmos.DrawWireSphere(matrix4x4.MultiplyPoint(particle.position), particle.GetCurrentSize(system) * 0.5f * CollisionModuleUI.s_LastInteractedEditor.m_RadiusScale.floatValue);
      }
      Gizmos.color = color;
    }

    private void DrawSolidPlane(Vector3 pos, Quaternion rot, int planeIndex)
    {
    }

    private void DrawGrid(Vector3 pos, Vector3 axis1, Vector3 axis2, Vector3 normal, Color color, int planeIndex)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      if ((double) color.a <= 0.0)
        return;
      GL.Begin(1);
      float num1 = 10f * this.m_ScaleGrid;
      int num2 = Mathf.Clamp((int) num1, 10, 40);
      if (num2 % 2 == 0)
        ++num2;
      float num3 = num1 * 0.5f;
      float num4 = num1 / (float) (num2 - 1);
      Vector3 vector3_1 = axis1 * num1;
      Vector3 vector3_2 = axis2 * num1;
      Vector3 vector3_3 = axis1 * num4;
      Vector3 vector3_4 = axis2 * num4;
      Vector3 vector3_5 = pos - axis1 * num3 - axis2 * num3;
      for (int index = 0; index < num2; ++index)
      {
        if (index % 2 == 0)
          GL.Color(color * 0.7f);
        else
          GL.Color(color);
        GL.Vertex(vector3_5 + (float) index * vector3_3);
        GL.Vertex(vector3_5 + (float) index * vector3_3 + vector3_2);
        GL.Vertex(vector3_5 + (float) index * vector3_4);
        GL.Vertex(vector3_5 + (float) index * vector3_4 + vector3_1);
      }
      GL.Color(color);
      GL.Vertex(pos);
      GL.Vertex(pos + normal);
      GL.End();
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text = text + "\n\tCollision is enabled.";
    }

    private enum CollisionTypes
    {
      Plane,
      World,
    }

    private enum CollisionModes
    {
      Mode3D,
      Mode2D,
    }

    private enum PlaneVizType
    {
      Grid,
      Solid,
    }

    private class Texts
    {
      public GUIContent lifetimeLoss = new GUIContent("Lifetime Loss", "When particle collides, it will lose this fraction of its Start Lifetime");
      public GUIContent planes = new GUIContent("Planes", "Planes are defined by assigning a reference to a transform. This transform can be any transform in the scene and can be animated. Multiple planes can be used. Note: the Y-axis is used as the plane normal.");
      public GUIContent createPlane = new GUIContent(string.Empty, "Create an empty GameObject and assign it as a plane.");
      public GUIContent minKillSpeed = new GUIContent("Min Kill Speed", "When particles collide and their speed is lower than this value, they are killed.");
      public GUIContent dampen = new GUIContent("Dampen", "When particle collides, it will lose this fraction of its speed. Unless this is set to 0.0, particle will become slower after collision.");
      public GUIContent bounce = new GUIContent("Bounce", "When particle collides, the bounce is scaled with this value. The bounce is the upwards motion in the plane normal direction.");
      public GUIContent radiusScale = new GUIContent("Radius Scale", "Scale particle bounds by this amount to get more precise collisions.");
      public GUIContent visualization = new GUIContent("Visualization", "Only used for visualizing the planes: Wireframe or Solid.");
      public GUIContent scalePlane = new GUIContent("Scale Plane", "Resizes the visualization planes.");
      public GUIContent visualizeBounds = new GUIContent("Visualize Bounds", "Render the collision bounds of the particles.");
      public GUIContent collidesWith = new GUIContent("Collides With", "Collides the particles with colliders included in the layermask.");
      public GUIContent collidesWithDynamic = new GUIContent("Enable Dynamic Colliders", "Should particles collide with dynamic objects?");
      public GUIContent interiorCollisions = new GUIContent("Interior Collisions", "Should particles collide with the insides of objects?");
      public GUIContent maxCollisionShapes = new GUIContent("Max Collision Shapes", "How many collision shapes can be considered for particle collisions. Excess shapes will be ignored. Terrains take priority.");
      public GUIContent quality = new GUIContent("Collision Quality", "Quality of world collisions. Medium and low quality are approximate and may leak particles.");
      public string[] qualitySettings = new string[3]{ "High", "Medium", "Low" };
      public GUIContent voxelSize = new GUIContent("Voxel Size", "Size of voxels in the collision cache.");
      public GUIContent collisionMessages = new GUIContent("Send Collision Messages", "Send collision callback messages.");
      public GUIContent collisionMode = new GUIContent("Collision Mode", "Use 3D Physics or 2D Physics.");
    }
  }
}
