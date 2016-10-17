// Decompiled with JetBrains decompiler
// Type: UnityEditor.HingeJointEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (HingeJoint))]
  [CanEditMultipleObjects]
  internal class HingeJointEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
      string message = string.Empty;
      JointLimits limits = ((HingeJoint) this.target).limits;
      if ((double) limits.min < -180.0 || (double) limits.min > 180.0)
        message += "Min Limit needs to be within [-180,180].";
      if ((double) limits.max < -180.0 || (double) limits.max > 180.0)
        message = message + (!string.IsNullOrEmpty(message) ? "\n" : string.Empty) + "Max Limit needs to be within [-180,180].";
      if ((double) limits.max < (double) limits.min)
        message = message + (!string.IsNullOrEmpty(message) ? "\n" : string.Empty) + "Max Limit needs to be larger or equal to the Min Limit.";
      if (string.IsNullOrEmpty(message))
        return;
      EditorGUILayout.HelpBox(message, MessageType.Warning);
    }
  }
}
