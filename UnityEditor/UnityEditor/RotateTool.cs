// Decompiled with JetBrains decompiler
// Type: UnityEditor.RotateTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RotateTool : ManipulationTool
  {
    private static RotateTool s_Instance;

    public static void OnGUI(SceneView view)
    {
      if (RotateTool.s_Instance == null)
        RotateTool.s_Instance = new RotateTool();
      RotateTool.s_Instance.OnToolGUI(view);
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      Quaternion handleRotation = Tools.handleRotation;
      EditorGUI.BeginChangeCheck();
      Quaternion quaternion = Handles.RotationHandle(handleRotation, handlePosition);
      if (!EditorGUI.EndChangeCheck() || isStatic)
        return;
      float angle;
      Vector3 axis1;
      (Quaternion.Inverse(handleRotation) * quaternion).ToAngleAxis(out angle, out axis1);
      Vector3 vector3 = handleRotation * axis1;
      if (TransformManipulator.individualSpace)
        vector3 = Quaternion.Inverse(Tools.handleRotation) * vector3;
      Undo.RecordObjects((Object[]) Selection.transforms, "Rotate");
      foreach (Transform transform in Selection.transforms)
      {
        Vector3 axis2 = vector3;
        if (TransformManipulator.individualSpace)
          axis2 = transform.rotation * vector3;
        if (Tools.pivotMode == PivotMode.Center)
          transform.RotateAround(handlePosition, axis2, angle);
        else
          transform.RotateAround(transform.position, axis2, angle);
        if ((Object) transform.parent != (Object) null)
          transform.SendTransformChangedScale();
      }
      Tools.handleRotation = quaternion;
    }
  }
}
