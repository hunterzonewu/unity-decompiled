// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClothInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Cloth))]
  [CanEditMultipleObjects]
  internal class ClothInspector : Editor
  {
    private int m_MouseOver = -1;
    private float[] m_MaxVisualizedValue = new float[3];
    private float[] m_MinVisualizedValue = new float[3];
    private ClothInspector.RectSelectionMode m_RectSelectionMode = ClothInspector.RectSelectionMode.Add;
    private const float kDisabledValue = 3.402823E+38f;
    private bool[] m_Selection;
    private bool[] m_RectSelection;
    private int m_MeshVerticesPerSelectionVertex;
    private Mesh[] m_SelectionMesh;
    private Mesh[] m_SelectedMesh;
    private Mesh m_VertexMesh;
    private Mesh m_VertexMeshSelected;
    private Vector3[] m_LastVertices;
    private Vector2 m_SelectStartPoint;
    private Vector2 m_SelectMousePoint;
    private bool m_RectSelecting;
    private bool m_DidSelect;
    private static Color s_SelectionColor;
    private static Material s_SelectionMaterial;
    private static Material s_SelectionMaterialBackfaces;
    private static Material s_SelectedMaterial;
    private static Texture2D s_ColorTexture;
    private static int s_MaxVertices;
    private static GUIContent[] s_ToolIcons;
    private static GUIContent[] s_ModeStrings;
    private static GUIContent s_PaintIcon;

    private ClothInspectorState state
    {
      get
      {
        return ScriptableSingleton<ClothInspectorState>.instance;
      }
    }

    private ClothInspector.DrawMode drawMode
    {
      get
      {
        return this.state.DrawMode;
      }
      set
      {
        if (this.state.DrawMode == value)
          return;
        this.state.DrawMode = value;
        this.SetupSelectionMeshColors();
        this.Repaint();
      }
    }

    private Cloth cloth
    {
      get
      {
        return (Cloth) this.target;
      }
    }

    public bool editing
    {
      get
      {
        if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.Cloth)
          return UnityEditorInternal.EditMode.IsOwner((Editor) this);
        return false;
      }
    }

    private GUIContent GetModeString(ClothInspector.DrawMode mode)
    {
      return ClothInspector.s_ModeStrings[(int) mode];
    }

    private Texture2D GenerateColorTexture(int width)
    {
      Texture2D texture2D = new Texture2D(width, 1, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      texture2D.hideFlags = HideFlags.DontSave;
      Color[] colors = new Color[width];
      for (int index = 0; index < width; ++index)
        colors[index] = this.GetGradientColor((float) index / (float) (width - 1));
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    public override void OnInspectorGUI()
    {
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.Cloth, "Edit Constraints", EditorGUIUtility.IconContent("EditCollider"), this.GetClothBounds(), (Editor) this);
      base.OnInspectorGUI();
      if (!((UnityEngine.Object) this.cloth.GetComponent<MeshRenderer>() != (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) "MeshRenderer will not work with a cloth component! Use only SkinnedMeshRenderer. Any MeshRenderer's attached to a cloth component will be deleted at runtime.");
    }

    private Bounds GetClothBounds()
    {
      if (this.target is Cloth)
      {
        SkinnedMeshRenderer component = ((Component) this.target).GetComponent<SkinnedMeshRenderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return component.bounds;
      }
      return new Bounds();
    }

    private bool SelectionMeshDirty()
    {
      SkinnedMeshRenderer component = this.cloth.GetComponent<SkinnedMeshRenderer>();
      Vector3[] vertices = this.cloth.vertices;
      Transform actualRootBone = component.actualRootBone;
      if (this.m_LastVertices.Length != vertices.Length)
        return true;
      for (int index = 0; index < this.m_LastVertices.Length; ++index)
      {
        Vector3 vector3 = actualRootBone.rotation * vertices[index] + actualRootBone.position;
        if (!(this.m_LastVertices[index] == vector3))
          return true;
      }
      return false;
    }

    private void GenerateSelectionMesh()
    {
      SkinnedMeshRenderer component = this.cloth.GetComponent<SkinnedMeshRenderer>();
      Vector3[] vertices = this.cloth.vertices;
      int length1 = vertices.Length;
      this.m_Selection = new bool[vertices.Length];
      this.m_RectSelection = new bool[vertices.Length];
      if (this.m_SelectionMesh != null)
      {
        foreach (UnityEngine.Object @object in this.m_SelectionMesh)
          UnityEngine.Object.DestroyImmediate(@object);
        foreach (UnityEngine.Object @object in this.m_SelectedMesh)
          UnityEngine.Object.DestroyImmediate(@object);
      }
      int length2 = length1 / ClothInspector.s_MaxVertices + 1;
      this.m_SelectionMesh = new Mesh[length2];
      this.m_SelectedMesh = new Mesh[length2];
      this.m_LastVertices = new Vector3[length1];
      this.m_MeshVerticesPerSelectionVertex = this.m_VertexMesh.vertices.Length;
      Transform actualRootBone = component.actualRootBone;
      for (int index1 = 0; index1 < length2; ++index1)
      {
        this.m_SelectionMesh[index1] = new Mesh();
        this.m_SelectionMesh[index1].hideFlags |= HideFlags.DontSave;
        this.m_SelectedMesh[index1] = new Mesh();
        this.m_SelectedMesh[index1].hideFlags |= HideFlags.DontSave;
        int length3 = length1 - index1 * ClothInspector.s_MaxVertices;
        if (length3 > ClothInspector.s_MaxVertices)
          length3 = ClothInspector.s_MaxVertices;
        CombineInstance[] combine = new CombineInstance[length3];
        int num = index1 * ClothInspector.s_MaxVertices;
        for (int index2 = 0; index2 < length3; ++index2)
        {
          this.m_LastVertices[num + index2] = actualRootBone.rotation * vertices[num + index2] + actualRootBone.position;
          combine[index2].mesh = this.m_VertexMesh;
          combine[index2].transform = Matrix4x4.TRS(this.m_LastVertices[num + index2], Quaternion.identity, Vector3.one);
        }
        this.m_SelectionMesh[index1].CombineMeshes(combine);
        for (int index2 = 0; index2 < length3; ++index2)
          combine[index2].mesh = this.m_VertexMeshSelected;
        this.m_SelectedMesh[index1].CombineMeshes(combine);
      }
      this.SetupSelectionMeshColors();
    }

    private void OnEnable()
    {
      if ((UnityEngine.Object) ClothInspector.s_SelectionMaterial == (UnityEngine.Object) null)
      {
        ClothInspector.s_SelectionMaterial = EditorGUIUtility.LoadRequired("SceneView/VertexSelectionMaterial.mat") as Material;
        ClothInspector.s_SelectionMaterialBackfaces = EditorGUIUtility.LoadRequired("SceneView/VertexSelectionBackfacesMaterial.mat") as Material;
        ClothInspector.s_SelectedMaterial = EditorGUIUtility.LoadRequired("SceneView/VertexSelectedMaterial.mat") as Material;
      }
      if ((UnityEngine.Object) ClothInspector.s_ColorTexture == (UnityEngine.Object) null)
        ClothInspector.s_ColorTexture = this.GenerateColorTexture(100);
      if (ClothInspector.s_ToolIcons == null)
      {
        ClothInspector.s_ToolIcons = new GUIContent[2];
        ClothInspector.s_ToolIcons[0] = EditorGUIUtility.TextContent("Select|Select vertices and edit their cloth coefficients in the inspector.");
        ClothInspector.s_ToolIcons[1] = EditorGUIUtility.TextContent("Paint|Paint cloth coefficients on to vertices.");
      }
      if (ClothInspector.s_ModeStrings == null)
      {
        ClothInspector.s_ModeStrings = new GUIContent[3];
        ClothInspector.s_ModeStrings[0] = EditorGUIUtility.TextContent("Fixed");
        ClothInspector.s_ModeStrings[1] = EditorGUIUtility.TextContent("Max Distance");
        ClothInspector.s_ModeStrings[2] = EditorGUIUtility.TextContent("Surface Penetration");
      }
      ClothInspector.s_PaintIcon = EditorGUIUtility.IconContent("ClothInspector.PaintValue", "Change this vertex coefficient value by painting in the scene view.");
      this.m_VertexMesh = new Mesh();
      this.m_VertexMesh.hideFlags |= HideFlags.DontSave;
      Mesh builtinResource = (Mesh) Resources.GetBuiltinResource(typeof (Mesh), "Cube.fbx");
      this.m_VertexMesh.vertices = new Vector3[builtinResource.vertices.Length];
      this.m_VertexMesh.normals = builtinResource.normals;
      Vector4[] vector4Array = new Vector4[builtinResource.vertices.Length];
      Vector3[] vertices = builtinResource.vertices;
      for (int index = 0; index < builtinResource.vertices.Length; ++index)
        vector4Array[index] = (Vector4) (vertices[index] * -0.01f);
      this.m_VertexMesh.tangents = vector4Array;
      this.m_VertexMesh.triangles = builtinResource.triangles;
      this.m_VertexMeshSelected = new Mesh();
      this.m_VertexMeshSelected.hideFlags |= HideFlags.DontSave;
      this.m_VertexMeshSelected.vertices = this.m_VertexMesh.vertices;
      this.m_VertexMeshSelected.normals = this.m_VertexMesh.normals;
      for (int index = 0; index < builtinResource.vertices.Length; ++index)
        vector4Array[index] = (Vector4) (vertices[index] * -0.02f);
      this.m_VertexMeshSelected.tangents = vector4Array;
      this.m_VertexMeshSelected.triangles = this.m_VertexMesh.triangles;
      ClothInspector.s_MaxVertices = 65536 / this.m_VertexMesh.vertices.Length;
      this.GenerateSelectionMesh();
      this.SetupSelectedMeshColors();
    }

    private float GetCoefficient(ClothSkinningCoefficient coefficient)
    {
      switch (this.drawMode)
      {
        case ClothInspector.DrawMode.MaxDistance:
          return coefficient.maxDistance;
        case ClothInspector.DrawMode.CollisionSphereDistance:
          return coefficient.collisionSphereDistance;
        default:
          return 0.0f;
      }
    }

    private Color GetGradientColor(float val)
    {
      if ((double) val < 0.300000011920929)
        return Color.Lerp(Color.red, Color.magenta, val / 0.2f);
      if ((double) val < 0.699999988079071)
        return Color.Lerp(Color.magenta, Color.yellow, (float) (((double) val - 0.200000002980232) / 0.5));
      return Color.Lerp(Color.yellow, Color.green, (float) (((double) val - 0.699999988079071) / 0.300000011920929));
    }

    private void AssignColorsToMeshArray(Color[] colors, Mesh[] meshArray)
    {
      int num1 = colors.Length / this.m_MeshVerticesPerSelectionVertex;
      int num2 = num1 / ClothInspector.s_MaxVertices + 1;
      for (int index = 0; index < num2; ++index)
      {
        int num3 = num1 - index * ClothInspector.s_MaxVertices;
        if (num3 > ClothInspector.s_MaxVertices)
          num3 = ClothInspector.s_MaxVertices;
        Color[] colorArray = new Color[num3 * this.m_MeshVerticesPerSelectionVertex];
        Array.Copy((Array) colors, index * ClothInspector.s_MaxVertices * this.m_MeshVerticesPerSelectionVertex, (Array) colorArray, 0, num3 * this.m_MeshVerticesPerSelectionVertex);
        meshArray[index].colors = colorArray;
      }
    }

    private void SetupSelectionMeshColors()
    {
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      int length = coefficients.Length;
      Color[] colors = new Color[length * this.m_MeshVerticesPerSelectionVertex];
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        float coefficient = this.GetCoefficient(coefficients[index]);
        if ((double) coefficient < 3.40282346638529E+38)
        {
          if ((double) coefficient < (double) num1)
            num1 = coefficient;
          if ((double) coefficient > (double) num2)
            num2 = coefficient;
        }
      }
      for (int index1 = 0; index1 < length; ++index1)
      {
        float coefficient = this.GetCoefficient(coefficients[index1]);
        Color color = (double) coefficient < 3.40282346638529E+38 ? this.GetGradientColor((double) num2 - (double) num1 == 0.0 ? 0.0f : (float) (((double) coefficient - (double) num1) / ((double) num2 - (double) num1))) : Color.black;
        for (int index2 = 0; index2 < this.m_MeshVerticesPerSelectionVertex; ++index2)
          colors[index1 * this.m_MeshVerticesPerSelectionVertex + index2] = color;
      }
      this.m_MaxVisualizedValue[(int) this.drawMode] = num2;
      this.m_MinVisualizedValue[(int) this.drawMode] = num1;
      this.AssignColorsToMeshArray(colors, this.m_SelectionMesh);
    }

    private void SetupSelectedMeshColors()
    {
      int length = this.cloth.coefficients.Length;
      Color[] colors = new Color[length * this.m_MeshVerticesPerSelectionVertex];
      for (int index1 = 0; index1 < length; ++index1)
      {
        bool flag = this.m_Selection[index1];
        if (this.m_RectSelecting)
        {
          switch (this.m_RectSelectionMode)
          {
            case ClothInspector.RectSelectionMode.Replace:
              flag = this.m_RectSelection[index1];
              break;
            case ClothInspector.RectSelectionMode.Add:
              flag |= this.m_RectSelection[index1];
              break;
            case ClothInspector.RectSelectionMode.Substract:
              flag = flag && !this.m_RectSelection[index1];
              break;
          }
        }
        Color color = !flag ? Color.clear : ClothInspector.s_SelectionColor;
        for (int index2 = 0; index2 < this.m_MeshVerticesPerSelectionVertex; ++index2)
          colors[index1 * this.m_MeshVerticesPerSelectionVertex + index2] = color;
      }
      this.AssignColorsToMeshArray(colors, this.m_SelectedMesh);
    }

    private void OnDisable()
    {
      if (this.m_SelectionMesh != null)
      {
        foreach (UnityEngine.Object @object in this.m_SelectionMesh)
          UnityEngine.Object.DestroyImmediate(@object);
        foreach (UnityEngine.Object @object in this.m_SelectedMesh)
          UnityEngine.Object.DestroyImmediate(@object);
      }
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_VertexMesh);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_VertexMeshSelected);
    }

    private float CoefficientField(float value, float useValue, bool enabled, ClothInspector.DrawMode mode)
    {
      GUIContent modeString = this.GetModeString(mode);
      EditorGUI.BeginDisabledGroup(!enabled);
      GUILayout.BeginHorizontal();
      EditorGUI.showMixedValue = (double) useValue < 0.0;
      EditorGUI.BeginChangeCheck();
      useValue = !EditorGUILayout.Toggle(GUIContent.none, (double) useValue != 0.0, new GUILayoutOption[0]) ? 0.0f : 1f;
      if (EditorGUI.EndChangeCheck())
      {
        value = (double) useValue <= 0.0 ? float.MaxValue : 0.0f;
        this.drawMode = mode;
      }
      GUILayout.Space(-152f);
      EditorGUI.showMixedValue = false;
      EditorGUI.BeginDisabledGroup((double) useValue != 1.0);
      float num1 = value;
      EditorGUI.showMixedValue = (double) value < 0.0;
      EditorGUI.BeginChangeCheck();
      int keyboardControl = GUIUtility.keyboardControl;
      if ((double) useValue > 0.0)
      {
        num1 = EditorGUILayout.FloatField(modeString, value, new GUILayoutOption[0]);
      }
      else
      {
        double num2 = (double) EditorGUILayout.FloatField(modeString, 0.0f, new GUILayoutOption[0]);
      }
      bool flag = EditorGUI.EndChangeCheck();
      if (flag)
      {
        value = num1;
        if ((double) value < 0.0)
          value = 0.0f;
      }
      if (flag || keyboardControl != GUIUtility.keyboardControl)
        this.drawMode = mode;
      EditorGUI.EndDisabledGroup();
      EditorGUI.EndDisabledGroup();
      if ((double) useValue > 0.0)
      {
        float num3 = this.m_MinVisualizedValue[(int) mode];
        float num4 = this.m_MaxVisualizedValue[(int) mode];
        if ((double) num4 - (double) num3 > 0.0)
          this.DrawColorBox((Texture) null, this.GetGradientColor((float) (((double) value - (double) num3) / ((double) num4 - (double) num3))));
        else
          this.DrawColorBox((Texture) null, this.GetGradientColor((double) value > (double) num3 ? 1f : 0.0f));
      }
      else
        this.DrawColorBox((Texture) null, Color.black);
      EditorGUI.showMixedValue = false;
      GUILayout.EndHorizontal();
      return value;
    }

    private float PaintField(float value, ref bool enabled, ClothInspector.DrawMode mode)
    {
      GUIContent modeString = this.GetModeString(mode);
      GUILayout.BeginHorizontal();
      enabled = (GUILayout.Toggle((enabled ? 1 : 0) != 0, ClothInspector.s_PaintIcon, (GUIStyle) "MiniButton", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) ? 1 : 0) != 0;
      EditorGUI.BeginDisabledGroup(!enabled);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(GUIContent.none, (double) value < 3.40282346638529E+38, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        value = !flag ? float.MaxValue : 0.0f;
        this.drawMode = mode;
      }
      GUILayout.Space(-162f);
      EditorGUI.BeginDisabledGroup(!flag);
      float num1 = value;
      int keyboardControl = GUIUtility.keyboardControl;
      EditorGUI.BeginChangeCheck();
      if (flag)
      {
        num1 = EditorGUILayout.FloatField(modeString, value, new GUILayoutOption[0]);
      }
      else
      {
        double num2 = (double) EditorGUILayout.FloatField(modeString, 0.0f, new GUILayoutOption[0]);
      }
      if ((double) num1 < 0.0)
        num1 = 0.0f;
      if (EditorGUI.EndChangeCheck() || keyboardControl != GUIUtility.keyboardControl)
        this.drawMode = mode;
      EditorGUI.EndDisabledGroup();
      EditorGUI.EndDisabledGroup();
      if (flag)
      {
        float num3 = this.m_MinVisualizedValue[(int) mode];
        float num4 = this.m_MaxVisualizedValue[(int) mode];
        if ((double) num4 - (double) num3 > 0.0)
          this.DrawColorBox((Texture) null, this.GetGradientColor((float) (((double) value - (double) num3) / ((double) num4 - (double) num3))));
        else
          this.DrawColorBox((Texture) null, this.GetGradientColor((double) value > (double) num3 ? 1f : 0.0f));
      }
      else
        this.DrawColorBox((Texture) null, Color.black);
      GUILayout.EndHorizontal();
      return num1;
    }

    private void SelectionGUI()
    {
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      float num1 = 0.0f;
      float useValue1 = 0.0f;
      float num2 = 0.0f;
      float useValue2 = 0.0f;
      int num3 = 0;
      bool flag = true;
      for (int index = 0; index < this.m_Selection.Length; ++index)
      {
        if (this.m_Selection[index])
        {
          if (flag)
          {
            num1 = coefficients[index].maxDistance;
            useValue1 = (double) num1 >= 3.40282346638529E+38 ? 0.0f : 1f;
            num2 = coefficients[index].collisionSphereDistance;
            useValue2 = (double) num2 >= 3.40282346638529E+38 ? 0.0f : 1f;
            flag = false;
          }
          if ((double) coefficients[index].maxDistance != (double) num1)
            num1 = -1f;
          if ((double) coefficients[index].collisionSphereDistance != (double) num2)
            num2 = -1f;
          if ((double) useValue1 != ((double) coefficients[index].maxDistance >= 3.40282346638529E+38 ? 0.0 : 1.0))
            useValue1 = -1f;
          if ((double) useValue2 != ((double) coefficients[index].collisionSphereDistance >= 3.40282346638529E+38 ? 0.0 : 1.0))
            useValue2 = -1f;
          ++num3;
        }
      }
      float num4 = this.CoefficientField(num1, useValue1, num3 > 0, ClothInspector.DrawMode.MaxDistance);
      if ((double) num4 != (double) num1)
      {
        for (int index = 0; index < coefficients.Length; ++index)
        {
          if (this.m_Selection[index])
            coefficients[index].maxDistance = num4;
        }
        this.cloth.coefficients = coefficients;
        this.SetupSelectionMeshColors();
        Undo.RegisterCompleteObjectUndo(this.target, "Change Cloth Coefficients");
      }
      float num5 = this.CoefficientField(num2, useValue2, num3 > 0, ClothInspector.DrawMode.CollisionSphereDistance);
      if ((double) num5 != (double) num2)
      {
        for (int index = 0; index < coefficients.Length; ++index)
        {
          if (this.m_Selection[index])
            coefficients[index].collisionSphereDistance = num5;
        }
        this.cloth.coefficients = coefficients;
        this.SetupSelectionMeshColors();
        Undo.RegisterCompleteObjectUndo(this.target, "Change Cloth Coefficients");
      }
      EditorGUI.BeginDisabledGroup(true);
      GUILayout.BeginHorizontal();
      if (num3 > 0)
      {
        GUILayout.FlexibleSpace();
        GUILayout.Label(num3.ToString() + " selected");
      }
      else
      {
        GUILayout.Label("Select cloth vertices to edit their constraints.");
        GUILayout.FlexibleSpace();
      }
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Backspace)
        return;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        if (this.m_Selection[index])
        {
          switch (this.drawMode)
          {
            case ClothInspector.DrawMode.MaxDistance:
              coefficients[index].maxDistance = float.MaxValue;
              continue;
            case ClothInspector.DrawMode.CollisionSphereDistance:
              coefficients[index].collisionSphereDistance = float.MaxValue;
              continue;
            default:
              continue;
          }
        }
      }
      this.cloth.coefficients = coefficients;
      this.SetupSelectionMeshColors();
    }

    private void PaintGUI()
    {
      this.state.PaintMaxDistance = this.PaintField(this.state.PaintMaxDistance, ref this.state.PaintMaxDistanceEnabled, ClothInspector.DrawMode.MaxDistance);
      this.state.PaintCollisionSphereDistance = this.PaintField(this.state.PaintCollisionSphereDistance, ref this.state.PaintCollisionSphereDistanceEnabled, ClothInspector.DrawMode.CollisionSphereDistance);
      if (this.state.PaintMaxDistanceEnabled && !this.state.PaintCollisionSphereDistanceEnabled)
        this.drawMode = ClothInspector.DrawMode.MaxDistance;
      else if (!this.state.PaintMaxDistanceEnabled && this.state.PaintCollisionSphereDistanceEnabled)
        this.drawMode = ClothInspector.DrawMode.CollisionSphereDistance;
      EditorGUI.BeginDisabledGroup(true);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Set constraints to paint onto cloth vertices.");
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
    }

    private int GetMouseVertex(Event e)
    {
      if (Tools.current != Tool.None)
        return -1;
      SkinnedMeshRenderer component = this.cloth.GetComponent<SkinnedMeshRenderer>();
      Vector3[] normals = this.cloth.normals;
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
      float num1 = 1000f;
      int num2 = -1;
      Quaternion rotation = component.actualRootBone.rotation;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        float sqrMagnitude = Vector3.Cross(this.m_LastVertices[index] - worldRay.origin, worldRay.direction).sqrMagnitude;
        if (((double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0 || this.state.ManipulateBackfaces) && ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < 0.00250000017695129))
        {
          num1 = sqrMagnitude;
          num2 = index;
        }
      }
      return num2;
    }

    private void DrawVertices()
    {
      if (this.SelectionMeshDirty())
        this.GenerateSelectionMesh();
      if (this.state.ToolMode == ClothInspector.ToolMode.Select)
      {
        for (int pass = 0; pass < ClothInspector.s_SelectedMaterial.passCount; ++pass)
        {
          ClothInspector.s_SelectedMaterial.SetPass(pass);
          foreach (Mesh mesh in this.m_SelectedMesh)
            Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
        }
      }
      Material material = !this.state.ManipulateBackfaces ? ClothInspector.s_SelectionMaterial : ClothInspector.s_SelectionMaterialBackfaces;
      for (int pass = 0; pass < material.passCount; ++pass)
      {
        material.SetPass(pass);
        foreach (Mesh mesh in this.m_SelectionMesh)
          Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
      }
      if (this.m_MouseOver == -1)
        return;
      Matrix4x4 matrix = Matrix4x4.TRS(this.m_LastVertices[this.m_MouseOver], Quaternion.identity, Vector3.one * 1.2f);
      if (this.state.ToolMode == ClothInspector.ToolMode.Select)
      {
        material = ClothInspector.s_SelectedMaterial;
        material.color = new Color(ClothInspector.s_SelectionColor.r, ClothInspector.s_SelectionColor.g, ClothInspector.s_SelectionColor.b, 0.5f);
      }
      else
      {
        int index1 = this.m_MouseOver / ClothInspector.s_MaxVertices;
        int index2 = this.m_MouseOver - ClothInspector.s_MaxVertices * index1;
        material.color = this.m_SelectionMesh[index1].colors[index2];
      }
      for (int pass = 0; pass < material.passCount; ++pass)
      {
        material.SetPass(pass);
        Graphics.DrawMeshNow(this.m_VertexMeshSelected, matrix);
      }
      material.color = Color.white;
    }

    private bool UpdateRectSelection()
    {
      bool flag1 = false;
      SkinnedMeshRenderer component = this.cloth.GetComponent<SkinnedMeshRenderer>();
      Vector3[] normals = this.cloth.normals;
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      float x1 = Mathf.Min(this.m_SelectStartPoint.x, this.m_SelectMousePoint.x);
      float x2 = Mathf.Max(this.m_SelectStartPoint.x, this.m_SelectMousePoint.x);
      float y1 = Mathf.Min(this.m_SelectStartPoint.y, this.m_SelectMousePoint.y);
      float y2 = Mathf.Max(this.m_SelectStartPoint.y, this.m_SelectMousePoint.y);
      Ray worldRay1 = HandleUtility.GUIPointToWorldRay(new Vector2(x1, y1));
      Ray worldRay2 = HandleUtility.GUIPointToWorldRay(new Vector2(x2, y1));
      Ray worldRay3 = HandleUtility.GUIPointToWorldRay(new Vector2(x1, y2));
      Ray worldRay4 = HandleUtility.GUIPointToWorldRay(new Vector2(x2, y2));
      Plane plane1 = new Plane(worldRay2.origin + worldRay2.direction, worldRay1.origin + worldRay1.direction, worldRay1.origin);
      Plane plane2 = new Plane(worldRay3.origin + worldRay3.direction, worldRay4.origin + worldRay4.direction, worldRay4.origin);
      Plane plane3 = new Plane(worldRay1.origin + worldRay1.direction, worldRay3.origin + worldRay3.direction, worldRay3.origin);
      Plane plane4 = new Plane(worldRay4.origin + worldRay4.direction, worldRay2.origin + worldRay2.direction, worldRay2.origin);
      Quaternion rotation = component.actualRootBone.rotation;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        Vector3 lastVertex = this.m_LastVertices[index];
        bool flag2 = (double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0;
        bool flag3 = plane1.GetSide(lastVertex) && plane2.GetSide(lastVertex) && plane3.GetSide(lastVertex) && plane4.GetSide(lastVertex) && (this.state.ManipulateBackfaces || flag2);
        if (this.m_RectSelection[index] != flag3)
        {
          this.m_RectSelection[index] = flag3;
          flag1 = true;
        }
      }
      return flag1;
    }

    private void ApplyRectSelection()
    {
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        switch (this.m_RectSelectionMode)
        {
          case ClothInspector.RectSelectionMode.Replace:
            this.m_Selection[index] = this.m_RectSelection[index];
            break;
          case ClothInspector.RectSelectionMode.Add:
            this.m_Selection[index] |= this.m_RectSelection[index];
            break;
          case ClothInspector.RectSelectionMode.Substract:
            this.m_Selection[index] = this.m_Selection[index] && !this.m_RectSelection[index];
            break;
        }
      }
    }

    private bool RectSelectionModeFromEvent()
    {
      Event current = Event.current;
      ClothInspector.RectSelectionMode rectSelectionMode = ClothInspector.RectSelectionMode.Replace;
      if (current.shift)
        rectSelectionMode = ClothInspector.RectSelectionMode.Add;
      if (current.alt)
        rectSelectionMode = ClothInspector.RectSelectionMode.Substract;
      if (this.m_RectSelectionMode == rectSelectionMode)
        return false;
      this.m_RectSelectionMode = rectSelectionMode;
      return true;
    }

    internal void SendCommandsOnModifierKeys()
    {
      SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("ModifierKeysChanged"));
    }

    private void SelectionPreSceneGUI(int id)
    {
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.alt || current.control || (current.command || current.button != 0))
            break;
          GUIUtility.hotControl = id;
          int mouseVertex = this.GetMouseVertex(current);
          if (mouseVertex != -1)
          {
            if (current.shift)
            {
              this.m_Selection[mouseVertex] = !this.m_Selection[mouseVertex];
            }
            else
            {
              for (int index = 0; index < this.m_Selection.Length; ++index)
                this.m_Selection[index] = false;
              this.m_Selection[mouseVertex] = true;
            }
            this.m_DidSelect = true;
            this.SetupSelectedMeshColors();
            this.Repaint();
          }
          else
            this.m_DidSelect = false;
          this.m_SelectStartPoint = current.mousePosition;
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id || current.button != 0)
            break;
          GUIUtility.hotControl = 0;
          if (this.m_RectSelecting)
          {
            EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
            this.m_RectSelecting = false;
            this.RectSelectionModeFromEvent();
            this.ApplyRectSelection();
          }
          else if (!this.m_DidSelect && !current.alt && (!current.control && !current.command))
          {
            ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
            for (int index = 0; index < coefficients.Length; ++index)
              this.m_Selection[index] = false;
          }
          GUIUtility.keyboardControl = 0;
          this.SetupSelectedMeshColors();
          SceneView.RepaintAll();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != id)
            break;
          if (!this.m_RectSelecting && ((double) (current.mousePosition - this.m_SelectStartPoint).magnitude > 2.0 && !current.alt && (!current.control && !current.command)))
          {
            EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
            this.m_RectSelecting = true;
            this.RectSelectionModeFromEvent();
            this.SetupSelectedMeshColors();
          }
          if (!this.m_RectSelecting)
            break;
          this.m_SelectMousePoint = new Vector2(Mathf.Max(current.mousePosition.x, 0.0f), Mathf.Max(current.mousePosition.y, 0.0f));
          if (this.RectSelectionModeFromEvent() || this.UpdateRectSelection())
            this.SetupSelectedMeshColors();
          current.Use();
          break;
        default:
          if (typeForControl != EventType.ExecuteCommand || !this.m_RectSelecting || !(current.commandName == "ModifierKeysChanged") || !this.RectSelectionModeFromEvent() && !this.UpdateRectSelection())
            break;
          this.SetupSelectedMeshColors();
          break;
      }
    }

    private void PaintPreSceneGUI(int id)
    {
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
        case EventType.MouseDrag:
          ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
          if (GUIUtility.hotControl != id && (current.alt || current.control || (current.command || current.button != 0)))
            break;
          if (typeForControl == EventType.MouseDown)
            GUIUtility.hotControl = id;
          int mouseVertex = this.GetMouseVertex(current);
          if (mouseVertex != -1)
          {
            bool flag = false;
            if (this.state.PaintMaxDistanceEnabled && (double) coefficients[mouseVertex].maxDistance != (double) this.state.PaintMaxDistance)
            {
              coefficients[mouseVertex].maxDistance = this.state.PaintMaxDistance;
              flag = true;
            }
            if (this.state.PaintCollisionSphereDistanceEnabled && (double) coefficients[mouseVertex].collisionSphereDistance != (double) this.state.PaintCollisionSphereDistance)
            {
              coefficients[mouseVertex].collisionSphereDistance = this.state.PaintCollisionSphereDistance;
              flag = true;
            }
            if (flag)
            {
              Undo.RegisterCompleteObjectUndo(this.target, "Paint Cloth");
              this.cloth.coefficients = coefficients;
              this.SetupSelectionMeshColors();
              this.Repaint();
            }
          }
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id || current.button != 0)
            break;
          GUIUtility.hotControl = 0;
          current.Use();
          break;
      }
    }

    public void OnPreSceneGUI()
    {
      if (!this.editing)
        return;
      Tools.current = Tool.None;
      if (this.state.ToolMode == ~ClothInspector.ToolMode.Select)
        this.state.ToolMode = ClothInspector.ToolMode.Select;
      if (this.m_Selection.Length != this.cloth.coefficients.Length && this.m_Selection.Length != ClothInspector.s_MaxVertices)
        this.OnEnable();
      Handles.BeginGUI();
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseMove:
        case EventType.MouseDrag:
          int mouseOver = this.m_MouseOver;
          this.m_MouseOver = this.GetMouseVertex(current);
          if (this.m_MouseOver != mouseOver)
          {
            SceneView.RepaintAll();
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
      switch (this.state.ToolMode)
      {
        case ClothInspector.ToolMode.Select:
          this.SelectionPreSceneGUI(controlId);
          break;
        case ClothInspector.ToolMode.Paint:
          this.PaintPreSceneGUI(controlId);
          break;
      }
      Handles.EndGUI();
    }

    public void OnSceneGUI()
    {
      if (!this.editing || Selection.gameObjects.Length > 1)
        return;
      ClothInspector.s_SelectionColor = GUI.skin.settings.selectionColor;
      if (Event.current.type == EventType.Repaint)
        this.DrawVertices();
      Event current = Event.current;
      if (current.commandName == "SelectAll")
      {
        if (current.type == EventType.ValidateCommand)
          current.Use();
        if (current.type == EventType.ExecuteCommand)
        {
          int length = this.cloth.vertices.Length;
          for (int index = 0; index < length; ++index)
            this.m_Selection[index] = true;
          this.SetupSelectedMeshColors();
          SceneView.RepaintAll();
          this.state.ToolMode = ClothInspector.ToolMode.Select;
          current.Use();
        }
      }
      Handles.BeginGUI();
      if (this.m_RectSelecting && this.state.ToolMode == ClothInspector.ToolMode.Select && Event.current.type == EventType.Repaint)
        EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
      Handles.EndGUI();
      SceneViewOverlay.Window(new GUIContent("Cloth Constraints"), new SceneViewOverlay.WindowFunction(this.VertexEditing), 0, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
    }

    public void VisualizationMenuSetMaxDistanceMode()
    {
      this.drawMode = ClothInspector.DrawMode.MaxDistance;
      if (this.state.PaintMaxDistanceEnabled)
        return;
      this.state.PaintCollisionSphereDistanceEnabled = false;
      this.state.PaintMaxDistanceEnabled = true;
    }

    public void VisualizationMenuSetCollisionSphereMode()
    {
      this.drawMode = ClothInspector.DrawMode.CollisionSphereDistance;
      if (this.state.PaintCollisionSphereDistanceEnabled)
        return;
      this.state.PaintCollisionSphereDistanceEnabled = true;
      this.state.PaintMaxDistanceEnabled = false;
    }

    public void VisualizationMenuToggleManipulateBackfaces()
    {
      this.state.ManipulateBackfaces = !this.state.ManipulateBackfaces;
    }

    public void DrawColorBox(Texture gradientTex, Color col)
    {
      if (!GUI.enabled)
      {
        col = new Color(0.3f, 0.3f, 0.3f, 1f);
        EditorGUI.showMixedValue = false;
      }
      GUILayout.BeginVertical();
      GUILayout.Space(5f);
      Rect position = GUILayoutUtility.GetRect(new GUIContent(), GUIStyle.none, new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.Height(10f) });
      GUI.Box(position, GUIContent.none);
      position = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      if ((bool) ((UnityEngine.Object) gradientTex))
        GUI.DrawTexture(position, gradientTex);
      else
        EditorGUIUtility.DrawColorSwatch(position, col, false);
      GUILayout.EndVertical();
    }

    private bool IsConstrained()
    {
      foreach (ClothSkinningCoefficient coefficient in this.cloth.coefficients)
      {
        if ((double) coefficient.maxDistance < 3.40282346638529E+38 || (double) coefficient.collisionSphereDistance < 3.40282346638529E+38)
          return true;
      }
      return false;
    }

    private void VertexEditing(UnityEngine.Object unused, SceneView sceneView)
    {
      GUILayout.BeginVertical(GUILayout.Width(300f));
      GUILayout.BeginHorizontal();
      GUILayout.Label("Visualization: ", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.BeginVertical();
      if (EditorGUILayout.ButtonMouseDown(this.GetModeString(this.drawMode), FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        Rect last = GUILayoutUtility.topLevel.GetLast();
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(this.GetModeString(ClothInspector.DrawMode.MaxDistance), this.drawMode == ClothInspector.DrawMode.MaxDistance, new GenericMenu.MenuFunction(this.VisualizationMenuSetMaxDistanceMode));
        genericMenu.AddItem(this.GetModeString(ClothInspector.DrawMode.CollisionSphereDistance), this.drawMode == ClothInspector.DrawMode.CollisionSphereDistance, new GenericMenu.MenuFunction(this.VisualizationMenuSetCollisionSphereMode));
        genericMenu.AddSeparator(string.Empty);
        genericMenu.AddItem(new GUIContent("Manipulate Backfaces"), this.state.ManipulateBackfaces, new GenericMenu.MenuFunction(this.VisualizationMenuToggleManipulateBackfaces));
        genericMenu.DropDown(last);
      }
      GUILayout.BeginHorizontal();
      GUILayout.Label(this.m_MinVisualizedValue[(int) this.drawMode].ToString(), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      this.DrawColorBox((Texture) ClothInspector.s_ColorTexture, Color.clear);
      GUILayout.Label(this.m_MaxVisualizedValue[(int) this.drawMode].ToString(), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.Label("Unconstrained:");
      GUILayout.Space(-24f);
      GUILayout.BeginHorizontal(GUILayout.Width(20f));
      this.DrawColorBox((Texture) null, Color.black);
      GUILayout.EndHorizontal();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.BeginVertical((GUIStyle) "Box", new GUILayoutOption[0]);
      if (Tools.current != Tool.None)
        this.state.ToolMode = ~ClothInspector.ToolMode.Select;
      ClothInspector.ToolMode toolMode = this.state.ToolMode;
      this.state.ToolMode = (ClothInspector.ToolMode) GUILayout.Toolbar((int) this.state.ToolMode, ClothInspector.s_ToolIcons);
      if (this.state.ToolMode != toolMode)
      {
        GUIUtility.keyboardControl = 0;
        SceneView.RepaintAll();
        this.SetupSelectionMeshColors();
        this.SetupSelectedMeshColors();
      }
      switch (this.state.ToolMode)
      {
        case ClothInspector.ToolMode.Select:
          Tools.current = Tool.None;
          this.SelectionGUI();
          break;
        case ClothInspector.ToolMode.Paint:
          Tools.current = Tool.None;
          this.PaintGUI();
          break;
      }
      GUILayout.EndVertical();
      if (!this.IsConstrained())
        EditorGUILayout.HelpBox("No constraints have been set up, so the cloth will move freely. Set up vertex constraints here to restrict it.", MessageType.Info);
      GUILayout.EndVertical();
      GUILayout.Space(-4f);
    }

    public enum DrawMode
    {
      MaxDistance = 1,
      CollisionSphereDistance = 2,
    }

    public enum ToolMode
    {
      Select,
      Paint,
    }

    private enum RectSelectionMode
    {
      Replace,
      Add,
      Substract,
    }
  }
}
