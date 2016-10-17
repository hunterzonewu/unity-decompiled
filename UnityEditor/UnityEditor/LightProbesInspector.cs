// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbesInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (LightProbes))]
  internal class LightProbesInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      LightProbes target = this.target as LightProbes;
      GUIStyle wrappedMiniLabel = EditorStyles.wordWrappedMiniLabel;
      GUILayout.Label("Light probe count: " + (object) target.count, wrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.Label("Cell count: " + (object) target.cellCount, wrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }
  }
}
