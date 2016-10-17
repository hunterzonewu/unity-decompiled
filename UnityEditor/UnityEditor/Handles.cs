// Decompiled with JetBrains decompiler
// Type: UnityEditor.Handles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Custom 3D GUI controls and drawing in the scene view.</para>
  /// </summary>
  public sealed class Handles
  {
    private static Color lineTransparency = new Color(1f, 1f, 1f, 0.75f);
    private static Vector3[] verts = new Vector3[4]
    {
      Vector3.zero,
      Vector3.zero,
      Vector3.zero,
      Vector3.zero
    };
    private static bool s_FreeMoveMode = false;
    private static Vector3 s_PlanarHandlesOctant = Vector3.one;
    internal static PrefColor s_XAxisColor = new PrefColor("Scene/X Axis", 0.8588235f, 0.2431373f, 0.1137255f, 0.93f);
    internal static PrefColor s_YAxisColor = new PrefColor("Scene/Y Axis", 0.6039216f, 0.9529412f, 0.282353f, 0.93f);
    internal static PrefColor s_ZAxisColor = new PrefColor("Scene/Z Axis", 0.227451f, 0.4784314f, 0.972549f, 0.93f);
    internal static PrefColor s_CenterColor = new PrefColor("Scene/Center Axis", 0.8f, 0.8f, 0.8f, 0.93f);
    internal static PrefColor s_SelectedColor = new PrefColor("Scene/Selected Axis", 0.9647059f, 0.9490196f, 0.1960784f, 0.89f);
    internal static PrefColor s_SecondaryColor = new PrefColor("Scene/Guide Line", 0.5f, 0.5f, 0.5f, 0.2f);
    internal static Color staticColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);
    internal static float staticBlend = 0.6f;
    internal static float backfaceAlphaMultiplier = 0.2f;
    internal static Color s_ColliderHandleColor = new Color(145f, 244f, 139f, 210f) / (float) byte.MaxValue;
    internal static Color s_ColliderHandleColorDisabled = new Color(84f, 200f, 77f, 140f) / (float) byte.MaxValue;
    internal static Color s_BoundingBoxHandleColor = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 150f) / (float) byte.MaxValue;
    internal static int s_SliderHash = "SliderHash".GetHashCode();
    internal static int s_Slider2DHash = "Slider2DHash".GetHashCode();
    internal static int s_FreeRotateHandleHash = "FreeRotateHandleHash".GetHashCode();
    internal static int s_RadiusHandleHash = "RadiusHandleHash".GetHashCode();
    internal static int s_xAxisMoveHandleHash = "xAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_yAxisMoveHandleHash = "yAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_zAxisMoveHandleHash = "xAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_FreeMoveHandleHash = "FreeMoveHandleHash".GetHashCode();
    internal static int s_xzAxisMoveHandleHash = "xzAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_xyAxisMoveHandleHash = "xyAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_yzAxisMoveHandleHash = "yzAxisFreeMoveHandleHash".GetHashCode();
    internal static int s_ScaleSliderHash = "ScaleSliderHash".GetHashCode();
    internal static int s_ScaleValueHandleHash = "ScaleValueHandleHash".GetHashCode();
    internal static int s_DiscHash = "DiscHash".GetHashCode();
    internal static int s_ButtonHash = "ButtonHash".GetHashCode();
    private static bool s_Lighting = true;
    internal static Matrix4x4 s_Matrix = Matrix4x4.identity;
    internal static Matrix4x4 s_InverseMatrix = Matrix4x4.identity;
    private static Vector3[] s_RectangleCapPointsCache = new Vector3[5];
    private const int kMaxDottedLineVertices = 1000;
    private const float k_BoneThickness = 0.08f;
    private static Color s_Color;
    internal static Mesh s_CubeMesh;
    internal static Mesh s_SphereMesh;
    internal static Mesh s_ConeMesh;
    internal static Mesh s_CylinderMesh;
    internal static Mesh s_QuadMesh;

    /// <summary>
    ///   <para>Color to use for handles that manipulates the X coordinate of something.</para>
    /// </summary>
    public static Color xAxisColor
    {
      get
      {
        return (Color) Handles.s_XAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that manipulates the Y coordinate of something.</para>
    /// </summary>
    public static Color yAxisColor
    {
      get
      {
        return (Color) Handles.s_YAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that manipulates the Z coordinate of something.</para>
    /// </summary>
    public static Color zAxisColor
    {
      get
      {
        return (Color) Handles.s_ZAxisColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for handles that represent the center of something.</para>
    /// </summary>
    public static Color centerColor
    {
      get
      {
        return (Color) Handles.s_CenterColor;
      }
    }

    /// <summary>
    ///   <para>Color to use for the currently active handle.</para>
    /// </summary>
    public static Color selectedColor
    {
      get
      {
        return (Color) Handles.s_SelectedColor;
      }
    }

    /// <summary>
    ///   <para>Soft color to use for for general things.</para>
    /// </summary>
    public static Color secondaryColor
    {
      get
      {
        return (Color) Handles.s_SecondaryColor;
      }
    }

    /// <summary>
    ///   <para>Are handles lit?</para>
    /// </summary>
    public static bool lighting
    {
      get
      {
        return Handles.s_Lighting;
      }
      set
      {
        Handles.s_Lighting = value;
      }
    }

    /// <summary>
    ///   <para>Colors of the handles.</para>
    /// </summary>
    public static Color color
    {
      get
      {
        return Handles.s_Color;
      }
      set
      {
        Handles.s_Color = value;
      }
    }

    /// <summary>
    ///   <para>Matrix for all handle operations.</para>
    /// </summary>
    public static Matrix4x4 matrix
    {
      get
      {
        return Handles.s_Matrix;
      }
      set
      {
        Handles.s_Matrix = value;
        Handles.s_InverseMatrix = value.inverse;
      }
    }

    /// <summary>
    ///   <para>The inverse of the matrix for all handle operations.</para>
    /// </summary>
    public static Matrix4x4 inverseMatrix
    {
      get
      {
        return Handles.s_InverseMatrix;
      }
    }

    /// <summary>
    ///   <para>Setup viewport and stuff for a current camera.</para>
    /// </summary>
    public Camera currentCamera
    {
      get
      {
        return Camera.current;
      }
      set
      {
        Handles.Internal_SetCurrentCamera(value);
      }
    }

    internal static Color realHandleColor
    {
      get
      {
        return Handles.s_Color * new Color(1f, 1f, 1f, 0.5f) + (!Handles.s_Lighting ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(0.0f, 0.0f, 0.0f, 0.5f));
      }
    }

    private static bool currentlyDragging
    {
      get
      {
        return GUIUtility.hotControl != 0;
      }
    }

    /// <summary>
    ///   <para>Make a 3D Scene view position handle.</para>
    /// </summary>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="rotation">Orientation of the handle in 3D space.</param>
    /// <returns>
    ///         <para>The new position. If the user has not performed any operation, it will return the same value as you passed it in postion.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static Vector3 PositionHandle(Vector3 position, Quaternion rotation)
    {
      return Handles.DoPositionHandle(position, rotation);
    }

    /// <summary>
    ///   <para>Make a Scene view rotation handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <returns>
    ///         <para>The modified rotation
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static Quaternion RotationHandle(Quaternion rotation, Vector3 position)
    {
      return Handles.DoRotationHandle(rotation, position);
    }

    /// <summary>
    ///   <para>Make a Scene view scale handle.</para>
    /// </summary>
    /// <param name="scale">Scale to modify.</param>
    /// <param name="position">The position of the handle.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size"></param>
    /// <returns>
    ///         <para>The new scale vector.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static Vector3 ScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, float size)
    {
      return Handles.DoScaleHandle(scale, position, rotation, size);
    }

    /// <summary>
    ///   <para>Make a Scene view radius handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="radius">Radius to modify.</param>
    /// <param name="handlesOnly"></param>
    /// <returns>
    ///         <para>The modified radius
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static float RadiusHandle(Quaternion rotation, Vector3 position, float radius, bool handlesOnly)
    {
      return Handles.DoRadiusHandle(rotation, position, radius, handlesOnly);
    }

    /// <summary>
    ///   <para>Make a Scene view radius handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="radius">Radius to modify.</param>
    /// <param name="handlesOnly"></param>
    /// <returns>
    ///         <para>The modified radius
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</para>
    ///       </returns>
    public static float RadiusHandle(Quaternion rotation, Vector3 position, float radius)
    {
      return Handles.DoRadiusHandle(rotation, position, radius, false);
    }

    internal static Vector2 ConeHandle(Quaternion rotation, Vector3 position, Vector2 angleAndRange, float angleScale, float rangeScale, bool handlesOnly)
    {
      return Handles.DoConeHandle(rotation, position, angleAndRange, angleScale, rangeScale, handlesOnly);
    }

    internal static Vector3 ConeFrustrumHandle(Quaternion rotation, Vector3 position, Vector3 radiusAngleRange)
    {
      return Handles.DoConeFrustrumHandle(rotation, position, radiusAngleRange);
    }

    /// <summary>
    ///   <para>Make a 3D slider.</para>
    /// </summary>
    /// <param name="position">The position of the current point.</param>
    /// <param name="direction">The direction of the sliding.</param>
    /// <param name="size">3D size the size of the handle.</param>
    /// <param name="drawFunc">The function to call for doing the actual drawing - by default, it's Handles.ArrowCap, but any function that has the same signature can be used.</param>
    /// <param name="snap">The snap value (see Handles.SnapValue).
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static Vector3 Slider(Vector3 position, Vector3 direction)
    {
      return Handles.Slider(position, direction, HandleUtility.GetHandleSize(position), new Handles.DrawCapFunction(Handles.ArrowCap), -1f);
    }

    public static Vector3 Slider(Vector3 position, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      return Slider1D.Do(GUIUtility.GetControlID(Handles.s_SliderHash, FocusType.Keyboard), position, direction, size, drawFunc, snap);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Keyboard), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(id, handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    public static Vector3 Slider2D(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, [DefaultValue("false")] bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    [ExcludeFromDocs]
    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap)
    {
      bool drawHelper = false;
      return Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
    }

    public static Vector3 Slider2D(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, [DefaultValue("false")] bool drawHelper)
    {
      return Handles.Slider2D(GUIUtility.GetControlID(Handles.s_Slider2DHash, FocusType.Keyboard), handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    /// <summary>
    ///   <para>Make an unconstrained rotation handle.</para>
    /// </summary>
    /// <param name="rotation">Orientation of the handle.</param>
    /// <param name="position">Center of the handle in 3D space.</param>
    /// <param name="size">The size of the handle.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static Quaternion FreeRotateHandle(Quaternion rotation, Vector3 position, float size)
    {
      return FreeRotate.Do(GUIUtility.GetControlID(Handles.s_FreeRotateHandleHash, FocusType.Keyboard), rotation, position, size);
    }

    public static Vector3 FreeMoveHandle(Vector3 position, Quaternion rotation, float size, Vector3 snap, Handles.DrawCapFunction capFunc)
    {
      return FreeMove.Do(GUIUtility.GetControlID(Handles.s_FreeMoveHandleHash, FocusType.Keyboard), position, rotation, size, snap, capFunc);
    }

    /// <summary>
    ///   <para>Make a directional scale slider.</para>
    /// </summary>
    /// <param name="scale">The value the user can modify.</param>
    /// <param name="position">The position of the handle.</param>
    /// <param name="direction">The direction of the handle.</param>
    /// <param name="rotation">The rotation of whole object.</param>
    /// <param name="size">The size of the handle.</param>
    /// <param name="snap">The new value after the user has modified it.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static float ScaleSlider(float scale, Vector3 position, Vector3 direction, Quaternion rotation, float size, float snap)
    {
      return SliderScale.DoAxis(GUIUtility.GetControlID(Handles.s_ScaleSliderHash, FocusType.Keyboard), scale, position, direction, rotation, size, snap);
    }

    public static float ScaleValueHandle(float value, Vector3 position, Quaternion rotation, float size, Handles.DrawCapFunction capFunc, float snap)
    {
      return SliderScale.DoCenter(GUIUtility.GetControlID(Handles.s_ScaleValueHandleHash, FocusType.Keyboard), value, position, rotation, size, capFunc, snap);
    }

    /// <summary>
    ///   <para>Make a 3D disc that can be dragged with the mouse.</para>
    /// </summary>
    /// <param name="rotation">The rotation of the disc.</param>
    /// <param name="position">The center of the disc.</param>
    /// <param name="axis">The axis to rotate around.</param>
    /// <param name="size">The size of the disc in world space See Also:HandleUtility.GetHandleSize.</param>
    /// <param name="cutoffPlane">If true, only the front-facing half of the circle is draw / draggable. This is useful when you have many overlapping rotation axes (like in the default rotate tool) to avoid clutter.</param>
    /// <param name="snap">The new value after the user has modified it.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static Quaternion Disc(Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
    {
      return Disc.Do(GUIUtility.GetControlID(Handles.s_DiscHash, FocusType.Keyboard), rotation, position, axis, size, cutoffPlane, snap);
    }

    public static bool Button(Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      return Button.Do(GUIUtility.GetControlID(Handles.s_ButtonHash, FocusType.Passive), position, direction, size, pickSize, capFunc);
    }

    internal static bool Button(int controlID, Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      return Button.Do(controlID, position, direction, size, pickSize, capFunc);
    }

    internal static void SetupIgnoreRaySnapObjects()
    {
      HandleUtility.ignoreRaySnapObjects = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.Editable);
    }

    /// <summary>
    ///   <para>Rounds the value val to the closest multiple of snap (snap can only be posiive).</para>
    /// </summary>
    /// <param name="val"></param>
    /// <param name="snap"></param>
    /// <returns>
    ///   <para>The rounded value, if snap is positive, and val otherwise.</para>
    /// </returns>
    public static float SnapValue(float val, float snap)
    {
      if (EditorGUI.actionKey && (double) snap > 0.0)
        return Mathf.Round(val / snap) * snap;
      return val;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawCameraWithGrid(Camera cam, int renderMode, ref DrawGridParameters gridParam);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawCamera(Camera cam, int renderMode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_FinishDrawingCamera(Camera cam);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ClearCamera(Camera cam);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_SetCurrentCamera(Camera cam);

    internal static void SetSceneViewColors(Color wire, Color wireOverlay, Color active, Color selected)
    {
      Handles.INTERNAL_CALL_SetSceneViewColors(ref wire, ref wireOverlay, ref active, ref selected);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetSceneViewColors(ref Color wire, ref Color wireOverlay, ref Color active, ref Color selected);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraFx(Camera cam, bool fx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraFlares(Camera cam, bool flares);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EnableCameraSkybox(Camera cam, bool skybox);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCameraOnlyDrawMesh(Camera cam);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetupCamera(Camera cam);

    internal static void DrawTwoShadedWireDisc(Vector3 position, Vector3 axis, float radius)
    {
      Color color1 = Handles.color;
      Color color2 = color1;
      color1.a *= Handles.backfaceAlphaMultiplier;
      Handles.color = color1;
      Handles.DrawWireDisc(position, axis, radius);
      Handles.color = color2;
    }

    internal static void DrawTwoShadedWireDisc(Vector3 position, Vector3 axis, Vector3 from, float degrees, float radius)
    {
      Handles.DrawWireArc(position, axis, from, degrees, radius);
      Color color1 = Handles.color;
      Color color2 = color1;
      color1.a *= Handles.backfaceAlphaMultiplier;
      Handles.color = color1;
      Handles.DrawWireArc(position, axis, from, degrees - 360f, radius);
      Handles.color = color2;
    }

    internal static Matrix4x4 StartCapDraw(Vector3 position, Quaternion rotation, float size)
    {
      Shader.SetGlobalColor("_HandleColor", Handles.realHandleColor);
      Shader.SetGlobalFloat("_HandleSize", size);
      Matrix4x4 mat = Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one);
      Shader.SetGlobalMatrix("_ObjectToWorld", mat);
      HandleUtility.handleMaterial.SetPass(0);
      return mat;
    }

    /// <summary>
    ///   <para>Draw a cube. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position of the handle's start point.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void CubeCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.s_CubeMesh, Handles.StartCapDraw(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw a Sphere. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    public static void SphereCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.s_SphereMesh, Handles.StartCapDraw(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw a Cone. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position of the handle's start point.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void ConeCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.s_ConeMesh, Handles.StartCapDraw(position, rotation, size));
    }

    /// <summary>
    ///   <para>Draw a Cylinder. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position of the handle's start point.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void CylinderCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Graphics.DrawMeshNow(Handles.s_CylinderMesh, Handles.StartCapDraw(position, rotation, size));
    }

    public static void RectangleCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.RectangleCap(controlID, position, rotation, new Vector2(size, size));
    }

    internal static void RectangleCap(int controlID, Vector3 position, Quaternion rotation, Vector2 size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3 vector3_1 = rotation * new Vector3(size.x, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size.y, 0.0f);
      Handles.s_RectangleCapPointsCache[0] = position + vector3_1 + vector3_2;
      Handles.s_RectangleCapPointsCache[1] = position + vector3_1 - vector3_2;
      Handles.s_RectangleCapPointsCache[2] = position - vector3_1 - vector3_2;
      Handles.s_RectangleCapPointsCache[3] = position - vector3_1 + vector3_2;
      Handles.s_RectangleCapPointsCache[4] = position + vector3_1 + vector3_2;
      Handles.DrawPolyLine(Handles.s_RectangleCapPointsCache);
    }

    /// <summary>
    ///   <para>Draw a camera facing selection frame.</para>
    /// </summary>
    /// <param name="controlID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    public static void SelectionFrame(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.StartCapDraw(position, rotation, size);
      Vector3 vector3_1 = rotation * new Vector3(size, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size, 0.0f);
      Vector3 vector3_3 = position - vector3_1 + vector3_2;
      Vector3 vector3_4 = position + vector3_1 + vector3_2;
      Vector3 vector3_5 = position + vector3_1 - vector3_2;
      Vector3 vector3_6 = position - vector3_1 - vector3_2;
      Handles.DrawLine(vector3_3, vector3_4);
      Handles.DrawLine(vector3_4, vector3_5);
      Handles.DrawLine(vector3_5, vector3_6);
      Handles.DrawLine(vector3_6, vector3_3);
    }

    /// <summary>
    ///   <para>Draw a camera-facing dot. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position of the handle's start point.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void DotCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      position = Handles.matrix.MultiplyPoint(position);
      Vector3 vector3_1 = Camera.current.transform.right * size;
      Vector3 vector3_2 = Camera.current.transform.up * size;
      Color c = Handles.s_Color * new Color(1f, 1f, 1f, 0.99f);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(c);
      GL.Vertex(position + vector3_1 + vector3_2);
      GL.Vertex(position + vector3_1 - vector3_2);
      GL.Vertex(position - vector3_1 - vector3_2);
      GL.Vertex(position - vector3_1 + vector3_2);
      GL.End();
    }

    /// <summary>
    ///   <para>Draw a camera-facing Circle. Pass this into handle functions.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position for the start of the handle.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void CircleCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.StartCapDraw(position, rotation, size);
      Vector3 normal = rotation * new Vector3(0.0f, 0.0f, 1f);
      Handles.DrawWireDisc(position, normal, size);
    }

    /// <summary>
    ///   <para>Draw an arrow like those used by the move tool.</para>
    /// </summary>
    /// <param name="controlID">The control ID for the handle.</param>
    /// <param name="position">The world-space position of the handle's start point.</param>
    /// <param name="rotation">The rotation of the handle.</param>
    /// <param name="size">The size of the handle in world-space units.</param>
    public static void ArrowCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3 forward = rotation * Vector3.forward;
      Handles.ConeCap(controlID, position + forward * size, Quaternion.LookRotation(forward), size * 0.2f);
      Handles.DrawLine(position, position + forward * size * 0.9f);
    }

    [Obsolete("DrawCylinder has been renamed to CylinderCap.")]
    public static void DrawCylinder(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.CylinderCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawSphere has been renamed to SphereCap.")]
    public static void DrawSphere(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.SphereCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawRectangle has been renamed to RectangleCap.")]
    public static void DrawRectangle(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.RectangleCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawCube has been renamed to CubeCap.")]
    public static void DrawCube(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.CubeCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawArrow has been renamed to ArrowCap.")]
    public static void DrawArrow(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.ArrowCap(controlID, position, rotation, size);
    }

    [Obsolete("DrawCone has been renamed to ConeCap.")]
    public static void DrawCone(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.ConeCap(controlID, position, rotation, size);
    }

    internal static void DrawAAPolyLine(Color[] colors, Vector3[] points)
    {
      Handles.DoDrawAAPolyLine(colors, points, -1, (Texture2D) null, 2f, 0.75f);
    }

    internal static void DrawAAPolyLine(float width, Color[] colors, Vector3[] points)
    {
      Handles.DoDrawAAPolyLine(colors, points, -1, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, (Texture2D) null, 2f, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(float width, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(Texture2D lineTex, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, lineTex, (float) (lineTex.height / 2), 0.99f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(float width, int actualNumberOfPoints, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, actualNumberOfPoints, (Texture2D) null, width, 0.75f);
    }

    /// <summary>
    ///   <para>Draw anti-aliased line specified with point array and width.</para>
    /// </summary>
    /// <param name="lineTex">The AA texture used for rendering. To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.</param>
    /// <param name="width">The width of the line. Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    /// <param name="points">List of points to build the line from.</param>
    /// <param name="actualNumberOfPoints"></param>
    public static void DrawAAPolyLine(Texture2D lineTex, float width, params Vector3[] points)
    {
      Handles.DoDrawAAPolyLine((Color[]) null, points, -1, lineTex, width, 0.99f);
    }

    private static void DoDrawAAPolyLine(Color[] colors, Vector3[] points, int actualNumberOfPoints, Texture2D lineTex, float width, float alpha)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      Color defaultColor = new Color(1f, 1f, 1f, alpha);
      if (colors != null)
      {
        for (int index = 0; index < colors.Length; ++index)
          colors[index] *= defaultColor;
      }
      else
        defaultColor *= Handles.s_Color;
      Handles.Internal_DrawAAPolyLine(colors, points, defaultColor, actualNumberOfPoints, lineTex, width, Handles.matrix);
    }

    private static void Internal_DrawAAPolyLine(Color[] colors, Vector3[] points, Color defaultColor, int actualNumberOfPoints, Texture2D texture, float width, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawAAPolyLine(colors, points, ref defaultColor, actualNumberOfPoints, texture, width, ref toWorld);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawAAPolyLine(Color[] colors, Vector3[] points, ref Color defaultColor, int actualNumberOfPoints, Texture2D texture, float width, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw anti-aliased convex polygon specified with point array.</para>
    /// </summary>
    /// <param name="points">List of points describing the convex polygon.</param>
    public static void DrawAAConvexPolygon(params Vector3[] points)
    {
      Handles.DoDrawAAConvexPolygon(points, -1, 1f);
    }

    private static void DoDrawAAConvexPolygon(Vector3[] points, int actualNumberOfPoints, float alpha)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      Color defaultColor = new Color(1f, 1f, 1f, alpha) * Handles.s_Color;
      Handles.Internal_DrawAAConvexPolygon(points, defaultColor, actualNumberOfPoints, Handles.matrix);
    }

    private static void Internal_DrawAAConvexPolygon(Vector3[] points, Color defaultColor, int actualNumberOfPoints, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawAAConvexPolygon(points, ref defaultColor, actualNumberOfPoints, ref toWorld);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawAAConvexPolygon(Vector3[] points, ref Color defaultColor, int actualNumberOfPoints, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw textured bezier line through start and end points with the given tangents.  To get an anti-aliased effect use a texture that is 1x2 pixels with one transparent white pixel and one opaque white pixel.  The bezier curve will be swept using this texture.</para>
    /// </summary>
    /// <param name="startPosition">The start point of the bezier line.</param>
    /// <param name="endPosition">The end point of the bezier line.</param>
    /// <param name="startTangent">The start tangent of the bezier line.</param>
    /// <param name="endTangent">The end tangent of the bezier line.</param>
    /// <param name="color">The color to use for the bezier line.</param>
    /// <param name="texture">The texture to use for drawing the bezier line.</param>
    /// <param name="width">The width of the bezier line.</param>
    public static void DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      Handles.Internal_DrawBezier(startPosition, endPosition, startTangent, endTangent, color, texture, width, Handles.matrix);
    }

    private static void Internal_DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width, Matrix4x4 toWorld)
    {
      Handles.INTERNAL_CALL_Internal_DrawBezier(ref startPosition, ref endPosition, ref startTangent, ref endTangent, ref color, texture, width, ref toWorld);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawBezier(ref Vector3 startPosition, ref Vector3 endPosition, ref Vector3 startTangent, ref Vector3 endTangent, ref Color color, Texture2D texture, float width, ref Matrix4x4 toWorld);

    /// <summary>
    ///   <para>Draw the outline of a flat disc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the dics.</param>
    /// <param name="normal">The normal of the disc.</param>
    /// <param name="radius">The radius of the dics
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawWireDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      Handles.DrawWireArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Draw a circular arc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="normal">The normal of the circle.</param>
    /// <param name="from">The direction of the point on the circle circumference, relative to the center, where the arc begins.</param>
    /// <param name="angle">The angle of the arc, in degrees.</param>
    /// <param name="radius">The radius of the circle
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      Vector3[] dest = new Vector3[60];
      Handles.SetDiscSectionPoints(dest, 60, center, normal, from, angle, radius);
      Handles.DrawPolyLine(dest);
    }

    public static void DrawSolidRectangleWithOutline(Rect rectangle, Color faceColor, Color outlineColor)
    {
      Handles.DrawSolidRectangleWithOutline(new Vector3[4]
      {
        new Vector3(rectangle.xMin, rectangle.yMin, 0.0f),
        new Vector3(rectangle.xMax, rectangle.yMin, 0.0f),
        new Vector3(rectangle.xMax, rectangle.yMax, 0.0f),
        new Vector3(rectangle.xMin, rectangle.yMax, 0.0f)
      }, faceColor, outlineColor);
    }

    /// <summary>
    ///   <para>Draw a solid outlined rectangle in 3D space.</para>
    /// </summary>
    /// <param name="verts">The 4 vertices of the rectangle in world coordinates.</param>
    /// <param name="faceColor">The color of the rectangle's face.</param>
    /// <param name="outlineColor">The outline color of the rectangle.</param>
    public static void DrawSolidRectangleWithOutline(Vector3[] verts, Color faceColor, Color outlineColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      if ((double) faceColor.a > 0.0)
      {
        Color c = faceColor * Handles.color;
        GL.Begin(4);
        for (int index = 0; index < 2; ++index)
        {
          GL.Color(c);
          GL.Vertex(verts[index * 2]);
          GL.Vertex(verts[index * 2 + 1]);
          GL.Vertex(verts[(index * 2 + 2) % 4]);
          GL.Vertex(verts[index * 2]);
          GL.Vertex(verts[(index * 2 + 2) % 4]);
          GL.Vertex(verts[index * 2 + 1]);
        }
        GL.End();
      }
      if ((double) outlineColor.a > 0.0)
      {
        Color c = outlineColor * Handles.color;
        GL.Begin(1);
        GL.Color(c);
        for (int index = 0; index < 4; ++index)
        {
          GL.Vertex(verts[index]);
          GL.Vertex(verts[(index + 1) % 4]);
        }
        GL.End();
      }
      GL.PopMatrix();
    }

    /// <summary>
    ///   <para>Draw a solid flat disc in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the dics.</param>
    /// <param name="normal">The normal of the disc.</param>
    /// <param name="radius">The radius of the dics
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawSolidDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      Handles.DrawSolidArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Draw a circular sector (pie piece) in 3D space.</para>
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="normal">The normal of the circle.</param>
    /// <param name="from">The direction of the point on the circumference, relative to the center, where the sector begins.</param>
    /// <param name="angle">The angle of the sector, in degrees.</param>
    /// <param name="radius">The radius of the circle
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void DrawSolidArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Vector3[] dest = new Vector3[60];
      Handles.SetDiscSectionPoints(dest, 60, center, normal, from, angle, radius);
      Shader.SetGlobalColor("_HandleColor", Handles.color * new Color(1f, 1f, 1f, 0.5f));
      Shader.SetGlobalFloat("_HandleSize", 1f);
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(4);
      for (int index = 1; index < dest.Length; ++index)
      {
        GL.Color(Handles.color);
        GL.Vertex(center);
        GL.Vertex(dest[index - 1]);
        GL.Vertex(dest[index]);
        GL.Vertex(center);
        GL.Vertex(dest[index]);
        GL.Vertex(dest[index - 1]);
      }
      GL.End();
      GL.PopMatrix();
    }

    internal static void SetDiscSectionPoints(Vector3[] dest, int count, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      from.Normalize();
      Quaternion quaternion = Quaternion.AngleAxis(angle / (float) (count - 1), normal);
      Vector3 vector3 = from * radius;
      for (int index = 0; index < count; ++index)
      {
        dest[index] = center + vector3;
        vector3 = quaternion * vector3;
      }
    }

    internal static void Init()
    {
      if ((bool) ((UnityEngine.Object) Handles.s_CubeMesh))
        return;
      GameObject gameObject = (GameObject) EditorGUIUtility.Load("SceneView/HandlesGO.fbx");
      if (!(bool) ((UnityEngine.Object) gameObject))
        Debug.Log((object) "ARGH - We couldn't find SceneView/HandlesGO.fbx");
      gameObject.SetActive(false);
      foreach (Transform transform in gameObject.transform)
      {
        MeshFilter component = transform.GetComponent<MeshFilter>();
        string name = transform.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (Handles.\u003C\u003Ef__switch\u0024map6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Handles.\u003C\u003Ef__switch\u0024map6 = new Dictionary<string, int>(5)
            {
              {
                "Cube",
                0
              },
              {
                "Sphere",
                1
              },
              {
                "Cone",
                2
              },
              {
                "Cylinder",
                3
              },
              {
                "Quad",
                4
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (Handles.\u003C\u003Ef__switch\u0024map6.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                Handles.s_CubeMesh = component.sharedMesh;
                continue;
              case 1:
                Handles.s_SphereMesh = component.sharedMesh;
                continue;
              case 2:
                Handles.s_ConeMesh = component.sharedMesh;
                continue;
              case 3:
                Handles.s_CylinderMesh = component.sharedMesh;
                continue;
              case 4:
                Handles.s_QuadMesh = component.sharedMesh;
                continue;
              default:
                continue;
            }
          }
        }
      }
      if (Application.platform != RuntimePlatform.WindowsEditor)
        return;
      Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande.ttf"));
      Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Bold.ttf"));
      Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Small.ttf"));
      Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Small Bold.ttf"));
      Handles.ReplaceFontForWindows((Font) EditorGUIUtility.LoadRequired(EditorResourcesUtility.fontsPath + "Lucida Grande Big.ttf"));
    }

    private static void ReplaceFontForWindows(Font font)
    {
      if (font.name.Contains("Bold"))
        font.fontNames = new string[2]
        {
          "Verdana Bold",
          "Tahoma Bold"
        };
      else
        font.fontNames = new string[2]
        {
          "Verdana",
          "Tahoma"
        };
      font.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, string text)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(text), GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, Texture image)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(image), GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, GUIContent content)
    {
      Handles.Label(position, content, GUI.skin.label);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, string text, GUIStyle style)
    {
      Handles.Label(position, EditorGUIUtility.TempContent(text), style);
    }

    /// <summary>
    ///   <para>Make a text label positioned in 3D space.</para>
    /// </summary>
    /// <param name="position">Position in 3D space as seen from the current handle camera.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="image">Texture to display on the label.</param>
    /// <param name="content">Text, image and tooltip for this label.</param>
    /// <param name="style">The style to use. If left out, the label style from the current GUISkin is used.
    /// 
    /// Note: Use HandleUtility.GetHandleSize where you might want to have constant screen-sized handles.</param>
    public static void Label(Vector3 position, GUIContent content, GUIStyle style)
    {
      Handles.BeginGUI();
      GUI.Label(HandleUtility.WorldPointToSizedRect(position, content, style), content, style);
      Handles.EndGUI();
    }

    internal static Rect GetCameraRect(Rect position)
    {
      Rect rect = GUIClip.Unclip(position);
      return new Rect(rect.xMin, (float) Screen.height - rect.yMax, rect.width, rect.height);
    }

    /// <summary>
    ///   <para>Get the width and height of the main game view.</para>
    /// </summary>
    public static Vector2 GetMainGameViewSize()
    {
      return GameView.GetSizeOfMainGameView();
    }

    /// <summary>
    ///   <para>Clears the camera.</para>
    /// </summary>
    /// <param name="position">Where in the Scene to clear.</param>
    /// <param name="camera">The camera to clear.</param>
    public static void ClearCamera(Rect position, Camera camera)
    {
      Event current = Event.current;
      if ((UnityEngine.Object) camera.targetTexture == (UnityEngine.Object) null)
      {
        Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
        Rect rect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
        camera.pixelRect = rect;
      }
      else
        camera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
      if (current.type == EventType.Repaint)
        Handles.Internal_ClearCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    internal static void DrawCameraImpl(Rect position, Camera camera, DrawCameraMode drawMode, bool drawGrid, DrawGridParameters gridParam, bool finish)
    {
      if (Event.current.type == EventType.Repaint)
      {
        if ((UnityEngine.Object) camera.targetTexture == (UnityEngine.Object) null)
        {
          Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
          camera.pixelRect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
        }
        else
          camera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
        if (drawMode == DrawCameraMode.Normal)
        {
          RenderTexture targetTexture = camera.targetTexture;
          camera.targetTexture = RenderTexture.active;
          camera.Render();
          camera.targetTexture = targetTexture;
        }
        else
        {
          if (drawGrid)
            Handles.Internal_DrawCameraWithGrid(camera, (int) drawMode, ref gridParam);
          else
            Handles.Internal_DrawCamera(camera, (int) drawMode);
          if (!finish)
            return;
          Handles.Internal_FinishDrawingCamera(camera);
        }
      }
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    internal static void DrawCamera(Rect position, Camera camera, DrawCameraMode drawMode, DrawGridParameters gridParam)
    {
      Handles.DrawCameraImpl(position, camera, drawMode, true, gridParam, true);
    }

    internal static void DrawCameraStep1(Rect position, Camera camera, DrawCameraMode drawMode, DrawGridParameters gridParam)
    {
      Handles.DrawCameraImpl(position, camera, drawMode, true, gridParam, false);
    }

    internal static void DrawCameraStep2(Camera camera, DrawCameraMode drawMode)
    {
      if (Event.current.type != EventType.Repaint || drawMode == DrawCameraMode.Normal)
        return;
      Handles.Internal_FinishDrawingCamera(camera);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool DrawCameraTonemap(Camera camera, RenderTexture srcRT, RenderTexture dstRT);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void EmitGUIGeometryForCamera(Camera source, Camera dest);

    /// <summary>
    ///   <para>Draws a camera inside a rectangle.</para>
    /// </summary>
    /// <param name="position">The area to draw the camera within in GUI coordinates.</param>
    /// <param name="camera">The camera to draw.</param>
    /// <param name="drawMode">How the camera is drawn (textured, wireframe, etc.).</param>
    [ExcludeFromDocs]
    public static void DrawCamera(Rect position, Camera camera)
    {
      DrawCameraMode drawMode = DrawCameraMode.Normal;
      Handles.DrawCamera(position, camera, drawMode);
    }

    /// <summary>
    ///   <para>Draws a camera inside a rectangle.</para>
    /// </summary>
    /// <param name="position">The area to draw the camera within in GUI coordinates.</param>
    /// <param name="camera">The camera to draw.</param>
    /// <param name="drawMode">How the camera is drawn (textured, wireframe, etc.).</param>
    public static void DrawCamera(Rect position, Camera camera, [DefaultValue("DrawCameraMode.Normal")] DrawCameraMode drawMode)
    {
      DrawGridParameters gridParam = new DrawGridParameters();
      Handles.DrawCameraImpl(position, camera, drawMode, false, gridParam, true);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCameraFilterMode(Camera camera, Handles.FilterMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Handles.FilterMode GetCameraFilterMode(Camera camera);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DrawCameraFade(Camera camera, float fade);

    /// <summary>
    ///   <para>Set the current camera so all Handles and Gizmos are draw with its settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="position"></param>
    public static void SetCamera(Camera camera)
    {
      if (Event.current.type == EventType.Repaint)
        Handles.Internal_SetupCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    /// <summary>
    ///   <para>Set the current camera so all Handles and Gizmos are draw with its settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="position"></param>
    public static void SetCamera(Rect position, Camera camera)
    {
      Rect pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
      Rect rect = new Rect(pixels.xMin, (float) Screen.height - pixels.yMax, pixels.width, pixels.height);
      camera.pixelRect = rect;
      if (Event.current.type == EventType.Repaint)
        Handles.Internal_SetupCamera(camera);
      else
        Handles.Internal_SetCurrentCamera(camera);
    }

    /// <summary>
    ///   <para>Begin a 2D GUI block inside the 3D handle GUI.</para>
    /// </summary>
    /// <param name="position">The position and size of the 2D GUI area.</param>
    public static void BeginGUI()
    {
      if (!(bool) ((UnityEngine.Object) Camera.current) || Event.current.type != EventType.Repaint)
        return;
      GUIClip.Reapply();
    }

    /// <summary>
    ///   <para>Begin a 2D GUI block inside the 3D handle GUI.</para>
    /// </summary>
    /// <param name="position">The position and size of the 2D GUI area.</param>
    [Obsolete("Please use BeginGUI() with GUILayout.BeginArea(position) / GUILayout.EndArea()")]
    public static void BeginGUI(Rect position)
    {
      GUILayout.BeginArea(position);
    }

    /// <summary>
    ///   <para>End a 2D GUI block and get back to the 3D handle GUI.</para>
    /// </summary>
    public static void EndGUI()
    {
      Camera current = Camera.current;
      if (!(bool) ((UnityEngine.Object) current) || Event.current.type != EventType.Repaint)
        return;
      Handles.Internal_SetupCamera(current);
    }

    internal static void ShowStaticLabelIfNeeded(Vector3 pos)
    {
      if (Tools.s_Hidden || !EditorApplication.isPlaying || !GameObjectUtility.ContainsStatic(Selection.gameObjects))
        return;
      Handles.color = Color.white;
      GUIStyle style = (GUIStyle) "SC ViewAxisLabel";
      style.alignment = TextAnchor.MiddleLeft;
      style.fixedWidth = 0.0f;
      Handles.BeginGUI();
      Rect sizedRect = HandleUtility.WorldPointToSizedRect(pos, EditorGUIUtility.TempContent("Static"), style);
      sizedRect.x += 10f;
      sizedRect.y += 10f;
      GUI.Label(sizedRect, EditorGUIUtility.TempContent("Static"), style);
      Handles.EndGUI();
    }

    private static Vector3[] Internal_MakeBezierPoints(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int division)
    {
      return Handles.INTERNAL_CALL_Internal_MakeBezierPoints(ref startPosition, ref endPosition, ref startTangent, ref endTangent, division);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Vector3[] INTERNAL_CALL_Internal_MakeBezierPoints(ref Vector3 startPosition, ref Vector3 endPosition, ref Vector3 startTangent, ref Vector3 endTangent, int division);

    /// <summary>
    ///   <para>Retuns an array of points to representing the bezier curve. See Handles.DrawBezier.</para>
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <param name="startTangent"></param>
    /// <param name="endTangent"></param>
    /// <param name="division"></param>
    public static Vector3[] MakeBezierPoints(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int division)
    {
      return Handles.Internal_MakeBezierPoints(startPosition, endPosition, startTangent, endTangent, division);
    }

    private static bool BeginLineDrawing(Matrix4x4 matrix, bool dottedLines)
    {
      if (Event.current.type != EventType.Repaint)
        return false;
      Color c = Handles.s_Color * Handles.lineTransparency;
      if (dottedLines)
        HandleUtility.ApplyDottedWireMaterial();
      else
        HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(matrix);
      GL.Begin(1);
      GL.Color(c);
      return true;
    }

    private static void EndLineDrawing()
    {
      GL.End();
      GL.PopMatrix();
    }

    /// <summary>
    ///   <para>Draw a line going through the list of all points.</para>
    /// </summary>
    /// <param name="points"></param>
    public static void DrawPolyLine(params Vector3[] points)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false))
        return;
      for (int index = 1; index < points.Length; ++index)
      {
        GL.Vertex(points[index]);
        GL.Vertex(points[index - 1]);
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false))
        return;
      GL.Vertex(p1);
      GL.Vertex(p2);
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    public static void DrawLines(Vector3[] lineSegments)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false))
        return;
      int index = 0;
      while (index < lineSegments.Length)
      {
        Vector3 lineSegment1 = lineSegments[index];
        Vector3 lineSegment2 = lineSegments[index + 1];
        GL.Vertex(lineSegment1);
        GL.Vertex(lineSegment2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of indexed line segments.</para>
    /// </summary>
    /// <param name="points">A list of points.</param>
    /// <param name="segmentIndices">A list of pairs of indices to the start and end points of the line segments.</param>
    public static void DrawLines(Vector3[] points, int[] segmentIndices)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, false))
        return;
      int index = 0;
      while (index < segmentIndices.Length)
      {
        Vector3 point1 = points[segmentIndices[index]];
        Vector3 point2 = points[segmentIndices[index + 1]];
        GL.Vertex(point1);
        GL.Vertex(point2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a dotted line from p1 to p2.</para>
    /// </summary>
    /// <param name="p1">The start point.</param>
    /// <param name="p2">The end point.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLine(Vector3 p1, Vector3 p2, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      GL.MultiTexCoord(1, p1);
      GL.MultiTexCoord2(2, x, 0.0f);
      GL.Vertex(p1);
      GL.MultiTexCoord(1, p1);
      GL.MultiTexCoord2(2, x, 0.0f);
      GL.Vertex(p2);
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of dotted line segments.</para>
    /// </summary>
    /// <param name="lineSegments">A list of pairs of points that represent the start and end of line segments.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLines(Vector3[] lineSegments, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      int index = 0;
      while (index < lineSegments.Length)
      {
        Vector3 lineSegment1 = lineSegments[index];
        Vector3 lineSegment2 = lineSegments[index + 1];
        GL.MultiTexCoord(1, lineSegment1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(lineSegment1);
        GL.MultiTexCoord(1, lineSegment1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(lineSegment2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    /// <summary>
    ///   <para>Draw a list of indexed dotted line segments.</para>
    /// </summary>
    /// <param name="points">A list of points.</param>
    /// <param name="segmentIndices">A list of pairs of indices to the start and end points of the line segments.</param>
    /// <param name="screenSpaceSize">The size in pixels for the lengths of the line segments and the gaps between them.</param>
    public static void DrawDottedLines(Vector3[] points, int[] segmentIndices, float screenSpaceSize)
    {
      if (!Handles.BeginLineDrawing(Handles.matrix, true))
        return;
      float x = screenSpaceSize * EditorGUIUtility.pixelsPerPoint;
      int index = 0;
      while (index < segmentIndices.Length)
      {
        Vector3 point1 = points[segmentIndices[index]];
        Vector3 point2 = points[segmentIndices[index + 1]];
        GL.MultiTexCoord(1, point1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(point1);
        GL.MultiTexCoord(1, point1);
        GL.MultiTexCoord2(2, x, 0.0f);
        GL.Vertex(point2);
        index += 2;
      }
      Handles.EndLineDrawing();
    }

    internal static float DistanceToPolygone(Vector3[] vertices)
    {
      return HandleUtility.DistanceToPolyLine(vertices);
    }

    internal static void DoBoneHandle(Transform target)
    {
      Handles.DoBoneHandle(target, (Dictionary<Transform, bool>) null);
    }

    internal static void DoBoneHandle(Transform target, Dictionary<Transform, bool> validBones)
    {
      int hashCode = target.name.GetHashCode();
      Event current = Event.current;
      bool flag = false;
      if (validBones != null)
      {
        foreach (Transform key in target)
        {
          if (validBones.ContainsKey(key))
          {
            flag = true;
            break;
          }
        }
      }
      Vector3 position = target.position;
      List<Vector3> vector3List = new List<Vector3>();
      if (!flag && (UnityEngine.Object) target.parent != (UnityEngine.Object) null)
      {
        vector3List.Add(target.position + (target.position - target.parent.position) * 0.4f);
      }
      else
      {
        foreach (Transform key in target)
        {
          if (validBones == null || validBones.ContainsKey(key))
            vector3List.Add(key.position);
        }
      }
      for (int index = 0; index < vector3List.Count; ++index)
      {
        Vector3 endPoint = vector3List[index];
        switch (current.GetTypeForControl(hashCode))
        {
          case EventType.MouseDown:
            if (!current.alt && (HandleUtility.nearestControl == hashCode && current.button == 0 || GUIUtility.keyboardControl == hashCode && current.button == 2))
            {
              int num = hashCode;
              GUIUtility.keyboardControl = num;
              GUIUtility.hotControl = num;
              if (current.shift)
              {
                UnityEngine.Object[] objects = Selection.objects;
                if (!ArrayUtility.Contains<UnityEngine.Object>(objects, (UnityEngine.Object) target))
                {
                  ArrayUtility.Add<UnityEngine.Object>(ref objects, (UnityEngine.Object) target);
                  Selection.objects = objects;
                }
              }
              else
                Selection.activeObject = (UnityEngine.Object) target;
              EditorGUIUtility.PingObject((UnityEngine.Object) target);
              current.Use();
              break;
            }
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl == hashCode && (current.button == 0 || current.button == 2))
            {
              GUIUtility.hotControl = 0;
              current.Use();
              break;
            }
            break;
          case EventType.MouseDrag:
            if (!current.alt && GUIUtility.hotControl == hashCode)
            {
              DragAndDrop.PrepareStartDrag();
              DragAndDrop.objectReferences = new UnityEngine.Object[1]
              {
                (UnityEngine.Object) target
              };
              DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle((UnityEngine.Object) target));
              current.Use();
              break;
            }
            break;
          case EventType.Repaint:
            float num1 = Vector3.Magnitude(endPoint - position);
            if ((double) num1 > 0.0)
            {
              float size = num1 * 0.08f;
              if (flag)
              {
                Handles.DrawBone(endPoint, position, size);
                break;
              }
              Handles.SphereCap(hashCode, position, target.rotation, size * 5f);
              break;
            }
            break;
          case EventType.Layout:
            float radius = Vector3.Magnitude(endPoint - position) * 0.08f;
            Vector3[] boneVertices = Handles.GetBoneVertices(endPoint, position, radius);
            HandleUtility.AddControl(hashCode, Handles.DistanceToPolygone(boneVertices));
            break;
        }
      }
    }

    internal static void DrawBone(Vector3 endPoint, Vector3 basePoint, float size)
    {
      Vector3[] boneVertices = Handles.GetBoneVertices(endPoint, basePoint, size);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(4);
      GL.Color(Handles.s_Color);
      for (int index = 0; index < 3; ++index)
      {
        GL.Vertex(boneVertices[index * 6]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 2]);
        GL.Vertex(boneVertices[index * 6 + 3]);
        GL.Vertex(boneVertices[index * 6 + 4]);
        GL.Vertex(boneVertices[index * 6 + 5]);
      }
      GL.End();
      GL.Begin(1);
      GL.Color(Handles.s_Color * new Color(1f, 1f, 1f, 0.0f) + new Color(0.0f, 0.0f, 0.0f, 1f));
      for (int index = 0; index < 3; ++index)
      {
        GL.Vertex(boneVertices[index * 6]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 1]);
        GL.Vertex(boneVertices[index * 6 + 2]);
      }
      GL.End();
    }

    internal static Vector3[] GetBoneVertices(Vector3 endPoint, Vector3 basePoint, float radius)
    {
      Vector3 lhs = Vector3.Normalize(endPoint - basePoint);
      Vector3 vector3_1 = Vector3.Cross(lhs, Vector3.up);
      if ((double) Vector3.SqrMagnitude(vector3_1) < 0.100000001490116)
        vector3_1 = Vector3.Cross(lhs, Vector3.right);
      vector3_1.Normalize();
      Vector3 vector3_2 = Vector3.Cross(lhs, vector3_1);
      Vector3[] vector3Array = new Vector3[18];
      float f = 0.0f;
      for (int index = 0; index < 3; ++index)
      {
        float num1 = Mathf.Cos(f);
        float num2 = Mathf.Sin(f);
        float num3 = Mathf.Cos(f + 2.094395f);
        float num4 = Mathf.Sin(f + 2.094395f);
        Vector3 vector3_3 = basePoint + vector3_1 * (num1 * radius) + vector3_2 * (num2 * radius);
        Vector3 vector3_4 = basePoint + vector3_1 * (num3 * radius) + vector3_2 * (num4 * radius);
        vector3Array[index * 6] = endPoint;
        vector3Array[index * 6 + 1] = vector3_3;
        vector3Array[index * 6 + 2] = vector3_4;
        vector3Array[index * 6 + 3] = basePoint;
        vector3Array[index * 6 + 4] = vector3_4;
        vector3Array[index * 6 + 5] = vector3_3;
        f += 2.094395f;
      }
      return vector3Array;
    }

    internal static Vector3 DoConeFrustrumHandle(Quaternion rotation, Vector3 position, Vector3 radiusAngleRange)
    {
      Vector3 vector3 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      float x = radiusAngleRange.x;
      float y1 = radiusAngleRange.y;
      float z = radiusAngleRange.z;
      float y2 = Mathf.Max(0.0f, y1);
      bool changed1 = GUI.changed;
      float num1 = Handles.SizeSlider(position, vector3, z);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      float r1 = Handles.SizeSlider(position, d1, x);
      float r2 = Handles.SizeSlider(position, -d1, r1);
      float r3 = Handles.SizeSlider(position, d2, r2);
      float num2 = Handles.SizeSlider(position, -d2, r3);
      if (GUI.changed)
        num2 = Mathf.Max(0.0f, num2);
      GUI.changed |= changed2;
      bool changed3 = GUI.changed;
      GUI.changed = false;
      float r4 = Mathf.Min(1000f, Mathf.Abs(num1 * Mathf.Tan((float) Math.PI / 180f * y2)) + num2);
      float r5 = Handles.SizeSlider(position + vector3 * num1, d1, r4);
      float r6 = Handles.SizeSlider(position + vector3 * num1, -d1, r5);
      float r7 = Handles.SizeSlider(position + vector3 * num1, d2, r6);
      float radius = Handles.SizeSlider(position + vector3 * num1, -d2, r7);
      if (GUI.changed)
        y2 = Mathf.Clamp(57.29578f * Mathf.Atan((radius - num2) / Mathf.Abs(num1)), 0.0f, 90f);
      GUI.changed |= changed3;
      if ((double) num2 > 0.0)
        Handles.DrawWireDisc(position, vector3, num2);
      if ((double) radius > 0.0)
        Handles.DrawWireDisc(position + num1 * vector3, vector3, radius);
      Handles.DrawLine(position + d1 * num2, position + vector3 * num1 + d1 * radius);
      Handles.DrawLine(position - d1 * num2, position + vector3 * num1 - d1 * radius);
      Handles.DrawLine(position + d2 * num2, position + vector3 * num1 + d2 * radius);
      Handles.DrawLine(position - d2 * num2, position + vector3 * num1 - d2 * radius);
      return new Vector3(num2, y2, num1);
    }

    internal static Vector2 DoConeHandle(Quaternion rotation, Vector3 position, Vector2 angleAndRange, float angleScale, float rangeScale, bool handlesOnly)
    {
      float x = angleAndRange.x;
      float y = angleAndRange.y;
      float r1 = y * rangeScale;
      Vector3 vector3 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      bool changed1 = GUI.changed;
      GUI.changed = false;
      float num = Handles.SizeSlider(position, vector3, r1);
      if (GUI.changed)
        y = Mathf.Max(0.0f, num / rangeScale);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      float r2 = num * Mathf.Tan((float) (Math.PI / 180.0 * (double) x / 2.0)) * angleScale;
      float r3 = Handles.SizeSlider(position + vector3 * num, d1, r2);
      float r4 = Handles.SizeSlider(position + vector3 * num, -d1, r3);
      float r5 = Handles.SizeSlider(position + vector3 * num, d2, r4);
      float radius = Handles.SizeSlider(position + vector3 * num, -d2, r5);
      if (GUI.changed)
        x = Mathf.Clamp((float) (57.2957801818848 * (double) Mathf.Atan(radius / (num * angleScale)) * 2.0), 0.0f, 179f);
      GUI.changed |= changed2;
      if (!handlesOnly)
      {
        Handles.DrawLine(position, position + vector3 * num + d1 * radius);
        Handles.DrawLine(position, position + vector3 * num - d1 * radius);
        Handles.DrawLine(position, position + vector3 * num + d2 * radius);
        Handles.DrawLine(position, position + vector3 * num - d2 * radius);
        Handles.DrawWireDisc(position + num * vector3, vector3, radius);
      }
      return new Vector2(x, y);
    }

    private static float SizeSlider(Vector3 p, Vector3 d, float r)
    {
      Vector3 position = p + d * r;
      float handleSize = HandleUtility.GetHandleSize(position);
      bool changed = GUI.changed;
      GUI.changed = false;
      Vector3 vector3 = Handles.Slider(position, d, handleSize * 0.03f, new Handles.DrawCapFunction(Handles.DotCap), 0.0f);
      if (GUI.changed)
        r = Vector3.Dot(vector3 - p, d);
      GUI.changed |= changed;
      return r;
    }

    public static Vector3 DoPositionHandle(Vector3 position, Quaternion rotation)
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.KeyDown:
          if (current.keyCode == KeyCode.V && !Handles.currentlyDragging)
          {
            Handles.s_FreeMoveMode = true;
            break;
          }
          break;
        case EventType.KeyUp:
          position = Handles.DoPositionHandle_Internal(position, rotation);
          if (current.keyCode == KeyCode.V && !current.shift && !Handles.currentlyDragging)
            Handles.s_FreeMoveMode = false;
          return position;
        case EventType.Layout:
          if (!Handles.currentlyDragging && !Tools.vertexDragging)
          {
            Handles.s_FreeMoveMode = current.shift;
            break;
          }
          break;
      }
      return Handles.DoPositionHandle_Internal(position, rotation);
    }

    private static Vector3 DoPositionHandle_Internal(Vector3 position, Quaternion rotation)
    {
      float handleSize = HandleUtility.GetHandleSize(position);
      Color color = Handles.color;
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      Handles.color = !flag ? Handles.xAxisColor : Color.Lerp(Handles.xAxisColor, Handles.staticColor, Handles.staticBlend);
      GUI.SetNextControlName("xAxis");
      position = Handles.Slider(position, rotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.x);
      Handles.color = !flag ? Handles.yAxisColor : Color.Lerp(Handles.yAxisColor, Handles.staticColor, Handles.staticBlend);
      GUI.SetNextControlName("yAxis");
      position = Handles.Slider(position, rotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.y);
      Handles.color = !flag ? Handles.zAxisColor : Color.Lerp(Handles.zAxisColor, Handles.staticColor, Handles.staticBlend);
      GUI.SetNextControlName("zAxis");
      position = Handles.Slider(position, rotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.z);
      if (Handles.s_FreeMoveMode)
      {
        Handles.color = Handles.centerColor;
        GUI.SetNextControlName("FreeMoveAxis");
        position = Handles.FreeMoveHandle(position, rotation, handleSize * 0.15f, SnapSettings.move, new Handles.DrawCapFunction(Handles.RectangleCap));
      }
      else
      {
        position = Handles.DoPlanarHandle(Handles.PlaneHandle.xzPlane, position, rotation, handleSize * 0.25f);
        position = Handles.DoPlanarHandle(Handles.PlaneHandle.xyPlane, position, rotation, handleSize * 0.25f);
        position = Handles.DoPlanarHandle(Handles.PlaneHandle.yzPlane, position, rotation, handleSize * 0.25f);
      }
      Handles.color = color;
      return position;
    }

    private static Vector3 DoPlanarHandle(Handles.PlaneHandle planeID, Vector3 position, Quaternion rotation, float handleSize)
    {
      int index1 = 0;
      int index2 = 0;
      int hint = 0;
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      switch (planeID)
      {
        case Handles.PlaneHandle.xzPlane:
          index1 = 0;
          index2 = 2;
          Handles.color = !flag ? Handles.yAxisColor : Handles.staticColor;
          hint = Handles.s_xzAxisMoveHandleHash;
          break;
        case Handles.PlaneHandle.xyPlane:
          index1 = 0;
          index2 = 1;
          Handles.color = !flag ? Handles.zAxisColor : Handles.staticColor;
          hint = Handles.s_xyAxisMoveHandleHash;
          break;
        case Handles.PlaneHandle.yzPlane:
          index1 = 1;
          index2 = 2;
          Handles.color = !flag ? Handles.xAxisColor : Handles.staticColor;
          hint = Handles.s_yzAxisMoveHandleHash;
          break;
      }
      int index3 = 3 - index2 - index1;
      Color color = Handles.color;
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(position, rotation, Vector3.one);
      Vector3 vector3 = !Camera.current.orthographic ? matrix4x4.inverse.MultiplyPoint(SceneView.currentDrawingSceneView.camera.transform.position).normalized : matrix4x4.inverse.MultiplyVector(SceneView.currentDrawingSceneView.cameraTargetRotation * -Vector3.forward).normalized;
      int controlId = GUIUtility.GetControlID(hint, FocusType.Keyboard);
      if ((double) Mathf.Abs(vector3[index3]) < 0.0500000007450581 && GUIUtility.hotControl != controlId)
      {
        Handles.color = color;
        return position;
      }
      if (!Handles.currentlyDragging)
      {
        Handles.s_PlanarHandlesOctant[index1] = (double) vector3[index1] >= -0.00999999977648258 ? 1f : -1f;
        Handles.s_PlanarHandlesOctant[index2] = (double) vector3[index2] >= -0.00999999977648258 ? 1f : -1f;
      }
      Vector3 planarHandlesOctant = Handles.s_PlanarHandlesOctant;
      planarHandlesOctant[index3] = 0.0f;
      Vector3 offset = rotation * planarHandlesOctant * handleSize * 0.5f;
      Vector3 zero1 = Vector3.zero;
      Vector3 zero2 = Vector3.zero;
      Vector3 zero3 = Vector3.zero;
      zero1[index1] = 1f;
      zero2[index2] = 1f;
      zero3[index3] = 1f;
      Vector3 slideDir1 = rotation * zero1;
      Vector3 slideDir2 = rotation * zero2;
      Vector3 handleDir = rotation * zero3;
      Handles.verts[0] = position + offset + (slideDir1 + slideDir2) * handleSize * 0.5f;
      Handles.verts[1] = position + offset + (-slideDir1 + slideDir2) * handleSize * 0.5f;
      Handles.verts[2] = position + offset + (-slideDir1 - slideDir2) * handleSize * 0.5f;
      Handles.verts[3] = position + offset + (slideDir1 - slideDir2) * handleSize * 0.5f;
      Handles.DrawSolidRectangleWithOutline(Handles.verts, new Color(Handles.color.r, Handles.color.g, Handles.color.b, 0.1f), new Color(0.0f, 0.0f, 0.0f, 0.0f));
      position = Handles.Slider2D(controlId, position, offset, handleDir, slideDir1, slideDir2, handleSize * 0.5f, new Handles.DrawCapFunction(Handles.RectangleCap), new Vector2(SnapSettings.move[index1], SnapSettings.move[index2]));
      Handles.color = color;
      return position;
    }

    internal static float DoRadiusHandle(Quaternion rotation, Vector3 position, float radius, bool handlesOnly)
    {
      float num1 = 90f;
      Vector3[] vector3Array = new Vector3[6]
      {
        rotation * Vector3.right,
        rotation * Vector3.up,
        rotation * Vector3.forward,
        rotation * -Vector3.right,
        rotation * -Vector3.up,
        rotation * -Vector3.forward
      };
      Vector3 vector3;
      if (Camera.current.orthographic)
      {
        vector3 = Camera.current.transform.forward;
        if (!handlesOnly)
        {
          Handles.DrawWireDisc(position, vector3, radius);
          for (int index = 0; index < 3; ++index)
          {
            Vector3 normalized = Vector3.Cross(vector3Array[index], vector3).normalized;
            Handles.DrawTwoShadedWireDisc(position, vector3Array[index], normalized, 180f, radius);
          }
        }
      }
      else
      {
        vector3 = position - Camera.current.transform.position;
        float sqrMagnitude = vector3.sqrMagnitude;
        float num2 = radius * radius;
        float f1 = num2 * num2 / sqrMagnitude;
        float num3 = f1 / num2;
        if ((double) num3 < 1.0)
        {
          float num4 = Mathf.Sqrt(num2 - f1);
          num1 = Mathf.Atan2(num4, Mathf.Sqrt(f1)) * 57.29578f;
          if (!handlesOnly)
            Handles.DrawWireDisc(position - num2 * vector3 / sqrMagnitude, vector3, num4);
        }
        else
          num1 = -1000f;
        if (!handlesOnly)
        {
          for (int index = 0; index < 3; ++index)
          {
            if ((double) num3 < 1.0)
            {
              float a = Vector3.Angle(vector3, vector3Array[index]);
              float num4 = Mathf.Tan((90f - Mathf.Min(a, 180f - a)) * ((float) Math.PI / 180f));
              float f2 = Mathf.Sqrt(f1 + num4 * num4 * f1) / radius;
              if ((double) f2 < 1.0)
              {
                float angle = Mathf.Asin(f2) * 57.29578f;
                Vector3 normalized = Vector3.Cross(vector3Array[index], vector3).normalized;
                Vector3 from = Quaternion.AngleAxis(angle, vector3Array[index]) * normalized;
                Handles.DrawTwoShadedWireDisc(position, vector3Array[index], from, (float) ((90.0 - (double) angle) * 2.0), radius);
              }
              else
                Handles.DrawTwoShadedWireDisc(position, vector3Array[index], radius);
            }
            else
              Handles.DrawTwoShadedWireDisc(position, vector3Array[index], radius);
          }
        }
      }
      Color color1 = Handles.color;
      for (int index = 0; index < 6; ++index)
      {
        int controlId = GUIUtility.GetControlID(Handles.s_RadiusHandleHash, FocusType.Keyboard);
        float num2 = Vector3.Angle(vector3Array[index], -vector3);
        if ((double) num2 > 5.0 && (double) num2 < 175.0 || GUIUtility.hotControl == controlId)
        {
          Color color2 = color1;
          color2.a = (double) num2 <= (double) num1 + 5.0 ? Mathf.Clamp01(color1.a * 2f) : Mathf.Clamp01((float) ((double) Handles.backfaceAlphaMultiplier * (double) color1.a * 2.0));
          Handles.color = color2;
          Vector3 position1 = position + radius * vector3Array[index];
          bool changed = GUI.changed;
          GUI.changed = false;
          Vector3 a = Slider1D.Do(controlId, position1, vector3Array[index], HandleUtility.GetHandleSize(position1) * 0.03f, new Handles.DrawCapFunction(Handles.DotCap), 0.0f);
          if (GUI.changed)
            radius = Vector3.Distance(a, position);
          GUI.changed |= changed;
        }
      }
      Handles.color = color1;
      return radius;
    }

    internal static Vector2 DoRectHandles(Quaternion rotation, Vector3 position, Vector2 size)
    {
      Vector3 vector3_1 = rotation * Vector3.forward;
      Vector3 d1 = rotation * Vector3.up;
      Vector3 d2 = rotation * Vector3.right;
      float r1 = 0.5f * size.x;
      float r2 = 0.5f * size.y;
      Vector3 vector3_2 = position + d1 * r2 + d2 * r1;
      Vector3 vector3_3 = position - d1 * r2 + d2 * r1;
      Vector3 vector3_4 = position - d1 * r2 - d2 * r1;
      Vector3 vector3_5 = position + d1 * r2 - d2 * r1;
      Handles.DrawLine(vector3_2, vector3_3);
      Handles.DrawLine(vector3_3, vector3_4);
      Handles.DrawLine(vector3_4, vector3_5);
      Handles.DrawLine(vector3_5, vector3_2);
      Color color = Handles.color;
      color.a = Mathf.Clamp01(color.a * 2f);
      Handles.color = color;
      float r3 = Handles.SizeSlider(position, d1, r2);
      float num1 = Handles.SizeSlider(position, -d1, r3);
      float r4 = Handles.SizeSlider(position, d2, r1);
      float num2 = Handles.SizeSlider(position, -d2, r4);
      if (Tools.current != Tool.Move && Tools.current != Tool.Scale || Tools.pivotRotation != PivotRotation.Local)
        Handles.DrawLine(position, position + vector3_1);
      size.x = 2f * num2;
      size.y = 2f * num1;
      return size;
    }

    public static Quaternion DoRotationHandle(Quaternion rotation, Vector3 position)
    {
      float handleSize = HandleUtility.GetHandleSize(position);
      Color color = Handles.color;
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      Handles.color = !flag ? Handles.xAxisColor : Color.Lerp(Handles.xAxisColor, Handles.staticColor, Handles.staticBlend);
      rotation = Handles.Disc(rotation, position, rotation * Vector3.right, handleSize, true, SnapSettings.rotation);
      Handles.color = !flag ? Handles.yAxisColor : Color.Lerp(Handles.yAxisColor, Handles.staticColor, Handles.staticBlend);
      rotation = Handles.Disc(rotation, position, rotation * Vector3.up, handleSize, true, SnapSettings.rotation);
      Handles.color = !flag ? Handles.zAxisColor : Color.Lerp(Handles.zAxisColor, Handles.staticColor, Handles.staticBlend);
      rotation = Handles.Disc(rotation, position, rotation * Vector3.forward, handleSize, true, SnapSettings.rotation);
      if (!flag)
      {
        Handles.color = Handles.centerColor;
        rotation = Handles.Disc(rotation, position, Camera.current.transform.forward, handleSize * 1.1f, false, 0.0f);
        rotation = Handles.FreeRotateHandle(rotation, position, handleSize);
      }
      Handles.color = color;
      return rotation;
    }

    public static Vector3 DoScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, float size)
    {
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      Handles.color = !flag ? Handles.xAxisColor : Color.Lerp(Handles.xAxisColor, Handles.staticColor, Handles.staticBlend);
      scale.x = Handles.ScaleSlider(scale.x, position, rotation * Vector3.right, rotation, size, SnapSettings.scale);
      Handles.color = !flag ? Handles.yAxisColor : Color.Lerp(Handles.yAxisColor, Handles.staticColor, Handles.staticBlend);
      scale.y = Handles.ScaleSlider(scale.y, position, rotation * Vector3.up, rotation, size, SnapSettings.scale);
      Handles.color = !flag ? Handles.zAxisColor : Color.Lerp(Handles.zAxisColor, Handles.staticColor, Handles.staticBlend);
      scale.z = Handles.ScaleSlider(scale.z, position, rotation * Vector3.forward, rotation, size, SnapSettings.scale);
      Handles.color = Handles.centerColor;
      EditorGUI.BeginChangeCheck();
      float num1 = Handles.ScaleValueHandle(scale.x, position, rotation, size, new Handles.DrawCapFunction(Handles.CubeCap), SnapSettings.scale);
      if (EditorGUI.EndChangeCheck())
      {
        float num2 = num1 / scale.x;
        scale.x = num1;
        scale.y *= num2;
        scale.z *= num2;
      }
      return scale;
    }

    internal static float DoSimpleEdgeHandle(Quaternion rotation, Vector3 position, float radius)
    {
      Vector3 d = rotation * Vector3.right;
      EditorGUI.BeginChangeCheck();
      radius = Handles.SizeSlider(position, d, radius);
      radius = Handles.SizeSlider(position, -d, radius);
      if (EditorGUI.EndChangeCheck())
        radius = Mathf.Max(0.0f, radius);
      if ((double) radius > 0.0)
        Handles.DrawLine(position - d * radius, position + d * radius);
      return radius;
    }

    internal static void DoSimpleRadiusArcHandleXY(Quaternion rotation, Vector3 position, ref float radius, ref float arc)
    {
      Vector3 vector3_1 = rotation * Vector3.forward;
      Vector3 d = rotation * Vector3.up;
      Vector3 vector3_2 = rotation * Vector3.right;
      Vector3 vector3_3 = Quaternion.Euler(0.0f, 0.0f, arc) * vector3_2;
      EditorGUI.BeginChangeCheck();
      if ((double) arc < 315.0)
        radius = Handles.SizeSlider(position, vector3_2, radius);
      if ((double) arc > 135.0)
        radius = Handles.SizeSlider(position, d, radius);
      if ((double) arc > 225.0)
        radius = Handles.SizeSlider(position, -vector3_2, radius);
      if ((double) arc > 315.0)
        radius = Handles.SizeSlider(position, -d, radius);
      if (EditorGUI.EndChangeCheck())
        radius = Mathf.Max(0.0f, radius);
      if ((double) radius <= 0.0)
        return;
      Handles.DrawWireArc(position, vector3_1, vector3_2, arc, radius);
      if ((double) arc < 360.0)
      {
        Handles.DrawLine(position, vector3_2 * radius);
        Handles.DrawLine(position, vector3_3 * radius);
      }
      else
        Handles.DrawDottedLine(position, vector3_2 * radius, 5f);
      Vector3 vector3_4 = vector3_3 * radius;
      float handleSize = HandleUtility.GetHandleSize(vector3_4);
      EditorGUI.BeginChangeCheck();
      Vector3 rhs = Handles.FreeMoveHandle(vector3_4, Quaternion.identity, handleSize * 0.03f, SnapSettings.move, new Handles.DrawCapFunction(Handles.CircleCap));
      if (!EditorGUI.EndChangeCheck())
        return;
      arc = arc + Mathf.Atan2(Vector3.Dot(vector3_1, Vector3.Cross(vector3_4, rhs)), Vector3.Dot(vector3_4, rhs)) * 57.29578f;
    }

    internal static float DoSimpleRadiusHandle(Quaternion rotation, Vector3 position, float radius, bool hemisphere)
    {
      Vector3 vector3_1 = rotation * Vector3.forward;
      Vector3 vector3_2 = rotation * Vector3.up;
      Vector3 vector3_3 = rotation * Vector3.right;
      bool changed1 = GUI.changed;
      GUI.changed = false;
      radius = Handles.SizeSlider(position, vector3_1, radius);
      if (!hemisphere)
        radius = Handles.SizeSlider(position, -vector3_1, radius);
      if (GUI.changed)
        radius = Mathf.Max(0.0f, radius);
      GUI.changed |= changed1;
      bool changed2 = GUI.changed;
      GUI.changed = false;
      radius = Handles.SizeSlider(position, vector3_2, radius);
      radius = Handles.SizeSlider(position, -vector3_2, radius);
      radius = Handles.SizeSlider(position, vector3_3, radius);
      radius = Handles.SizeSlider(position, -vector3_3, radius);
      if (GUI.changed)
        radius = Mathf.Max(0.0f, radius);
      GUI.changed |= changed2;
      if ((double) radius > 0.0)
      {
        Handles.DrawWireDisc(position, vector3_1, radius);
        Handles.DrawWireArc(position, vector3_2, -vector3_3, !hemisphere ? 360f : 180f, radius);
        Handles.DrawWireArc(position, vector3_3, vector3_2, !hemisphere ? 360f : 180f, radius);
      }
      return radius;
    }

    internal enum FilterMode
    {
      Off,
      ShowFiltered,
      ShowRest,
    }

    private enum PlaneHandle
    {
      xzPlane,
      xyPlane,
      yzPlane,
    }

    /// <summary>
    ///   <para>The function to use for drawing the handle e.g. Handles.RectangleCap.</para>
    /// </summary>
    /// <param name="controlID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    public delegate void DrawCapFunction(int controlID, Vector3 position, Quaternion rotation, float size);
  }
}
