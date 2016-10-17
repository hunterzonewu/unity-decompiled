// Decompiled with JetBrains decompiler
// Type: UnityEditor.ComputeShaderInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ComputeShader))]
  internal class ComputeShaderInspector : Editor
  {
    private Vector2 m_ScrollPosition = Vector2.zero;
    private const float kSpace = 5f;

    public virtual void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
      ComputeShader target = this.target as ComputeShader;
      if ((Object) target == (Object) null)
        return;
      GUI.enabled = true;
      EditorGUI.indentLevel = 0;
      this.ShowDebuggingData(target);
      this.ShowShaderErrors(target);
    }

    private void ShowDebuggingData(ComputeShader cs)
    {
      GUILayout.Space(5f);
      GUILayout.Label("Compiled code:", EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel("All variants", EditorStyles.miniButton);
      if (GUILayout.Button(ComputeShaderInspector.Styles.showAll, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        ShaderUtil.OpenCompiledComputeShader(cs, true);
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.EndHorizontal();
    }

    private void ShowShaderErrors(ComputeShader s)
    {
      if (ShaderUtil.GetComputeShaderErrorCount(s) < 1)
        return;
      ShaderInspector.ShaderErrorListUI((Object) s, ShaderUtil.GetComputeShaderErrors(s), ref this.m_ScrollPosition);
    }

    internal class Styles
    {
      public static GUIContent showAll = EditorGUIUtility.TextContent("Show code");
    }
  }
}
