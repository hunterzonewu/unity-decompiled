// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoxEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class BoxEditor
  {
    private bool m_AllowNegativeSize = true;
    private const float kViewAngleThreshold = 0.05235988f;
    private int m_ControlIdHint;
    private int m_HandleControlID;
    private bool m_UseLossyScale;
    private bool m_AlwaysDisplayHandles;
    private bool m_DisableZaxis;
    public Handles.DrawCapFunction drawMethodForHandles;
    public Func<Vector3, float> getHandleSizeMethod;

    public bool allowNegativeSize
    {
      get
      {
        return this.m_AllowNegativeSize;
      }
      set
      {
        this.m_AllowNegativeSize = value;
      }
    }

    public float backfaceAlphaMultiplier { get; set; }

    public BoxEditor(bool useLossyScale, int controlIdHint)
    {
      this.m_UseLossyScale = useLossyScale;
      this.m_ControlIdHint = controlIdHint;
      this.backfaceAlphaMultiplier = Handles.backfaceAlphaMultiplier;
    }

    public BoxEditor(bool useLossyScale, int controlIdHint, bool disableZaxis)
    {
      this.m_UseLossyScale = useLossyScale;
      this.m_ControlIdHint = controlIdHint;
      this.m_DisableZaxis = disableZaxis;
      this.backfaceAlphaMultiplier = Handles.backfaceAlphaMultiplier;
    }

    public void OnEnable()
    {
      this.m_HandleControlID = -1;
    }

    public void OnDisable()
    {
    }

    public void SetAlwaysDisplayHandles(bool enable)
    {
      this.m_AlwaysDisplayHandles = enable;
    }

    public bool OnSceneGUI(Transform transform, Color color, ref Vector3 center, ref Vector3 size)
    {
      return this.OnSceneGUI(transform, color, true, ref center, ref size);
    }

    public bool OnSceneGUI(Transform transform, Color color, bool handlesOnly, ref Vector3 center, ref Vector3 size)
    {
      if (!this.m_UseLossyScale)
        return this.OnSceneGUI(transform.localToWorldMatrix, color, handlesOnly, ref center, ref size);
      Matrix4x4 transform1 = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
      size.Scale(transform.lossyScale);
      center = transform.TransformPoint(center);
      center = transform1.inverse.MultiplyPoint(center);
      bool flag = this.OnSceneGUI(transform1, color, handlesOnly, ref center, ref size);
      center = transform1.MultiplyPoint(center);
      center = transform.InverseTransformPoint(center);
      size.Scale(new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z));
      return flag;
    }

    public bool OnSceneGUI(Matrix4x4 transform, Color color, bool handlesOnly, ref Vector3 center, ref Vector3 size)
    {
      return this.OnSceneGUI(transform, color, color, handlesOnly, ref center, ref size);
    }

    public bool OnSceneGUI(Matrix4x4 transform, Color boxColor, Color midPointHandleColor, bool handlesOnly, ref Vector3 center, ref Vector3 size)
    {
      if (!this.m_AlwaysDisplayHandles && GUIUtility.hotControl != this.m_HandleControlID)
      {
        for (int index = 0; index < 6; ++index)
          GUIUtility.GetControlID(this.m_ControlIdHint, FocusType.Keyboard);
        return false;
      }
      if (Tools.viewToolActive)
        return false;
      Color color = Handles.color;
      Handles.color = boxColor;
      Vector3 minPos = center - size * 0.5f;
      Vector3 maxPos = center + size * 0.5f;
      Matrix4x4 matrix = Handles.matrix;
      Handles.matrix = transform;
      int hotControl = GUIUtility.hotControl;
      if (!handlesOnly)
        this.DrawWireframeBox(center, size);
      Vector3 point = transform.inverse.MultiplyPoint(Camera.current.transform.position);
      bool isCameraInsideBox = new Bounds(center, size).Contains(point);
      Handles.color = midPointHandleColor;
      this.MidpointHandles(ref minPos, ref maxPos, Handles.matrix, isCameraInsideBox);
      if (hotControl != GUIUtility.hotControl && GUIUtility.hotControl != 0)
        this.m_HandleControlID = GUIUtility.hotControl;
      bool changed = GUI.changed;
      if (changed)
      {
        center = (maxPos + minPos) * 0.5f;
        size = maxPos - minPos;
      }
      Handles.color = color;
      Handles.matrix = matrix;
      return changed;
    }

    public void DrawWireframeBox(Vector3 center, Vector3 siz)
    {
      Vector3 vector3 = siz * 0.5f;
      Vector3[] vector3Array = new Vector3[10]{ center + new Vector3(-vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, vector3.y, -vector3.z), center + new Vector3(vector3.x, vector3.y, -vector3.z), center + new Vector3(vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, -vector3.y, -vector3.z), center + new Vector3(-vector3.x, -vector3.y, vector3.z), center + new Vector3(-vector3.x, vector3.y, vector3.z), center + new Vector3(vector3.x, vector3.y, vector3.z), center + new Vector3(vector3.x, -vector3.y, vector3.z), center + new Vector3(-vector3.x, -vector3.y, vector3.z) };
      Handles.DrawPolyLine(vector3Array);
      Handles.DrawLine(vector3Array[1], vector3Array[6]);
      Handles.DrawLine(vector3Array[2], vector3Array[7]);
      Handles.DrawLine(vector3Array[3], vector3Array[8]);
    }

    private void MidpointHandles(ref Vector3 minPos, ref Vector3 maxPos, Matrix4x4 transform, bool isCameraInsideBox)
    {
      Vector3 vector3_1 = new Vector3(1f, 0.0f, 0.0f);
      Vector3 localTangent = new Vector3(0.0f, 1f, 0.0f);
      Vector3 localBinormal = new Vector3(0.0f, 0.0f, 1f);
      Vector3 vector3_2 = (maxPos + minPos) * 0.5f;
      Vector3 localPos = new Vector3(maxPos.x, vector3_2.y, vector3_2.z);
      Vector3 vector3_3 = this.MidpointHandle(localPos, localTangent, localBinormal, transform, isCameraInsideBox);
      maxPos.x = !this.m_AllowNegativeSize ? Math.Max(vector3_3.x, minPos.x) : vector3_3.x;
      localPos = new Vector3(minPos.x, vector3_2.y, vector3_2.z);
      vector3_3 = this.MidpointHandle(localPos, localTangent, -localBinormal, transform, isCameraInsideBox);
      minPos.x = !this.m_AllowNegativeSize ? Math.Min(vector3_3.x, maxPos.x) : vector3_3.x;
      localPos = new Vector3(vector3_2.x, maxPos.y, vector3_2.z);
      vector3_3 = this.MidpointHandle(localPos, vector3_1, -localBinormal, transform, isCameraInsideBox);
      maxPos.y = !this.m_AllowNegativeSize ? Math.Max(vector3_3.y, minPos.y) : vector3_3.y;
      localPos = new Vector3(vector3_2.x, minPos.y, vector3_2.z);
      vector3_3 = this.MidpointHandle(localPos, vector3_1, localBinormal, transform, isCameraInsideBox);
      minPos.y = !this.m_AllowNegativeSize ? Math.Min(vector3_3.y, maxPos.y) : vector3_3.y;
      if (this.m_DisableZaxis)
        return;
      localPos = new Vector3(vector3_2.x, vector3_2.y, maxPos.z);
      vector3_3 = this.MidpointHandle(localPos, localTangent, -vector3_1, transform, isCameraInsideBox);
      maxPos.z = !this.m_AllowNegativeSize ? Math.Max(vector3_3.z, minPos.z) : vector3_3.z;
      localPos = new Vector3(vector3_2.x, vector3_2.y, minPos.z);
      vector3_3 = this.MidpointHandle(localPos, localTangent, vector3_1, transform, isCameraInsideBox);
      minPos.z = !this.m_AllowNegativeSize ? Math.Min(vector3_3.z, maxPos.z) : vector3_3.z;
    }

    private static void DefaultMidPointDrawFunc(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      Handles.DotCap(controlID, position, rotation, size);
    }

    private static float DefaultMidpointGetSizeFunc(Vector3 localPos)
    {
      return HandleUtility.GetHandleSize(localPos) * 0.03f;
    }

    private Vector3 MidpointHandle(Vector3 localPos, Vector3 localTangent, Vector3 localBinormal, Matrix4x4 transform, bool isCameraInsideBox)
    {
      Color color = Handles.color;
      float alphaFactor = 1f;
      this.AdjustMidpointHandleColor(localPos, localTangent, localBinormal, transform, alphaFactor, isCameraInsideBox);
      int controlId = GUIUtility.GetControlID(this.m_ControlIdHint, FocusType.Keyboard);
      if ((double) alphaFactor > 0.0)
      {
        Vector3 normalized = Vector3.Cross(localTangent, localBinormal).normalized;
        Handles.DrawCapFunction drawFunc = this.drawMethodForHandles ?? new Handles.DrawCapFunction(BoxEditor.DefaultMidPointDrawFunc);
        Func<Vector3, float> func = this.getHandleSizeMethod ?? new Func<Vector3, float>(BoxEditor.DefaultMidpointGetSizeFunc);
        localPos = Slider1D.Do(controlId, localPos, normalized, func(localPos), drawFunc, SnapSettings.scale);
      }
      Handles.color = color;
      return localPos;
    }

    private void AdjustMidpointHandleColor(Vector3 localPos, Vector3 localTangent, Vector3 localBinormal, Matrix4x4 transform, float alphaFactor, bool isCameraInsideBox)
    {
      if (!isCameraInsideBox)
      {
        Vector3 vector3 = transform.MultiplyPoint(localPos);
        Vector3 normalized = Vector3.Cross(transform.MultiplyVector(localTangent), transform.MultiplyVector(localBinormal)).normalized;
        if ((!Camera.current.orthographic ? (double) Vector3.Dot((Camera.current.transform.position - vector3).normalized, normalized) : (double) Vector3.Dot(-Camera.current.transform.forward, normalized)) < -9.99999974737875E-05)
          alphaFactor *= this.backfaceAlphaMultiplier;
      }
      if ((double) alphaFactor >= 1.0)
        return;
      Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, Handles.color.a * alphaFactor);
    }

    private void EdgeHandles(ref Vector3 minPos, ref Vector3 maxPos, Matrix4x4 transform)
    {
      Vector3 vector3_1 = new Vector3(1f, 0.0f, 0.0f);
      Vector3 vector3_2 = new Vector3(0.0f, 1f, 0.0f);
      Vector3 slideDir2 = new Vector3(0.0f, 0.0f, 1f);
      float z = (float) (((double) minPos.z + (double) maxPos.z) * 0.5);
      Vector3 handlePos1 = new Vector3(minPos.x, minPos.y, z);
      Vector3 vector3_3 = this.EdgeHandle(handlePos1, vector3_1, -vector3_1, -vector3_2, transform);
      minPos.x = vector3_3.x;
      minPos.y = vector3_3.y;
      handlePos1 = new Vector3(minPos.x, maxPos.y, z);
      Vector3 vector3_4 = this.EdgeHandle(handlePos1, vector3_1, -vector3_1, vector3_2, transform);
      minPos.x = vector3_4.x;
      maxPos.y = vector3_4.y;
      handlePos1 = new Vector3(maxPos.x, maxPos.y, z);
      Vector3 vector3_5 = this.EdgeHandle(handlePos1, vector3_1, vector3_1, vector3_2, transform);
      maxPos.x = vector3_5.x;
      maxPos.y = vector3_5.y;
      handlePos1 = new Vector3(maxPos.x, minPos.y, z);
      Vector3 vector3_6 = this.EdgeHandle(handlePos1, vector3_1, vector3_1, -vector3_2, transform);
      maxPos.x = vector3_6.x;
      minPos.y = vector3_6.y;
      float y = (float) (((double) minPos.y + (double) maxPos.y) * 0.5);
      Vector3 handlePos2 = new Vector3(minPos.x, y, minPos.z);
      Vector3 vector3_7 = this.EdgeHandle(handlePos2, vector3_2, -vector3_1, -slideDir2, transform);
      minPos.x = vector3_7.x;
      minPos.z = vector3_7.z;
      handlePos2 = new Vector3(minPos.x, y, maxPos.z);
      Vector3 vector3_8 = this.EdgeHandle(handlePos2, vector3_2, -vector3_1, slideDir2, transform);
      minPos.x = vector3_8.x;
      maxPos.z = vector3_8.z;
      handlePos2 = new Vector3(maxPos.x, y, maxPos.z);
      Vector3 vector3_9 = this.EdgeHandle(handlePos2, vector3_2, vector3_1, slideDir2, transform);
      maxPos.x = vector3_9.x;
      maxPos.z = vector3_9.z;
      handlePos2 = new Vector3(maxPos.x, y, minPos.z);
      Vector3 vector3_10 = this.EdgeHandle(handlePos2, vector3_2, vector3_1, -slideDir2, transform);
      maxPos.x = vector3_10.x;
      minPos.z = vector3_10.z;
      float x = (float) (((double) minPos.x + (double) maxPos.x) * 0.5);
      Vector3 handlePos3 = new Vector3(x, minPos.y, minPos.z);
      Vector3 vector3_11 = this.EdgeHandle(handlePos3, vector3_2, -vector3_2, -slideDir2, transform);
      minPos.y = vector3_11.y;
      minPos.z = vector3_11.z;
      handlePos3 = new Vector3(x, minPos.y, maxPos.z);
      Vector3 vector3_12 = this.EdgeHandle(handlePos3, vector3_2, -vector3_2, slideDir2, transform);
      minPos.y = vector3_12.y;
      maxPos.z = vector3_12.z;
      handlePos3 = new Vector3(x, maxPos.y, maxPos.z);
      Vector3 vector3_13 = this.EdgeHandle(handlePos3, vector3_2, vector3_2, slideDir2, transform);
      maxPos.y = vector3_13.y;
      maxPos.z = vector3_13.z;
      handlePos3 = new Vector3(x, maxPos.y, minPos.z);
      Vector3 vector3_14 = this.EdgeHandle(handlePos3, vector3_2, vector3_2, -slideDir2, transform);
      maxPos.y = vector3_14.y;
      minPos.z = vector3_14.z;
    }

    private Vector3 EdgeHandle(Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, Matrix4x4 transform)
    {
      Color color = Handles.color;
      bool flag = true;
      if ((bool) ((UnityEngine.Object) Camera.current))
      {
        Vector3 vector3 = Handles.matrix.inverse.MultiplyPoint(Camera.current.transform.position);
        Vector3 normalized = (handlePos - vector3).normalized;
        if ((double) Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.Cross(slideDir1, slideDir2), normalized))) > 1.51843643188477)
          flag = false;
      }
      float alphaFactor = !flag ? 0.0f : 1f;
      this.AdjustEdgeHandleColor(handlePos, slideDir1, slideDir2, transform, alphaFactor);
      int controlId = GUIUtility.GetControlID(this.m_ControlIdHint, FocusType.Keyboard);
      if ((double) alphaFactor > 0.0)
        handlePos = Slider2D.Do(controlId, handlePos, handleDir, slideDir1, slideDir2, HandleUtility.GetHandleSize(handlePos) * 0.03f, new Handles.DrawCapFunction(Handles.DotCap), SnapSettings.scale, true);
      Handles.color = color;
      return handlePos;
    }

    private void AdjustEdgeHandleColor(Vector3 handlePos, Vector3 slideDir1, Vector3 slideDir2, Matrix4x4 transform, float alphaFactor)
    {
      Vector3 inPoint = transform.MultiplyPoint(handlePos);
      Vector3 normalized1 = transform.MultiplyVector(slideDir1).normalized;
      Vector3 normalized2 = transform.MultiplyVector(slideDir2).normalized;
      if (!Camera.current.orthographic ? !new Plane(normalized1, inPoint).GetSide(Camera.current.transform.position) && !new Plane(normalized2, inPoint).GetSide(Camera.current.transform.position) : (double) Vector3.Dot(-Camera.current.transform.forward, normalized1) < 0.0 && (double) Vector3.Dot(-Camera.current.transform.forward, normalized2) < 0.0)
        alphaFactor *= this.backfaceAlphaMultiplier;
      if ((double) alphaFactor >= 1.0)
        return;
      Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, Handles.color.a * alphaFactor);
    }
  }
}
