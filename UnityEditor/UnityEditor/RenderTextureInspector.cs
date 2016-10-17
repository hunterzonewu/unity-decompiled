// Decompiled with JetBrains decompiler
// Type: UnityEditor.RenderTextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (RenderTexture))]
  internal class RenderTextureInspector : TextureInspector
  {
    private static readonly GUIContent[] kRenderTextureAntiAliasing = new GUIContent[4]{ new GUIContent("None"), new GUIContent("2 samples"), new GUIContent("4 samples"), new GUIContent("8 samples") };
    private static readonly int[] kRenderTextureAntiAliasingValues = new int[4]{ 1, 2, 4, 8 };
    private SerializedProperty m_Width;
    private SerializedProperty m_Height;
    private SerializedProperty m_ColorFormat;
    private SerializedProperty m_DepthFormat;
    private SerializedProperty m_AntiAliasing;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Width = this.serializedObject.FindProperty("m_Width");
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_AntiAliasing = this.serializedObject.FindProperty("m_AntiAliasing");
      this.m_ColorFormat = this.serializedObject.FindProperty("m_ColorFormat");
      this.m_DepthFormat = this.serializedObject.FindProperty("m_DepthFormat");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      GUI.changed = false;
      GUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel("Size", EditorStyles.popup);
      EditorGUILayout.PropertyField(this.m_Width, GUIContent.none, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      GUILayout.Label("x");
      EditorGUILayout.PropertyField(this.m_Height, GUIContent.none, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      EditorGUILayout.IntPopup(this.m_AntiAliasing, RenderTextureInspector.kRenderTextureAntiAliasing, RenderTextureInspector.kRenderTextureAntiAliasingValues, EditorGUIUtility.TempContent("Anti-Aliasing"), new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ColorFormat, EditorGUIUtility.TempContent("Color Format"), new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DepthFormat, EditorGUIUtility.TempContent("Depth Buffer"), new GUILayoutOption[0]);
      RenderTexture target = this.target as RenderTexture;
      if (GUI.changed && (Object) target != (Object) null)
        target.Release();
      this.isInspectorDirty = true;
      EditorGUILayout.Space();
      this.DoWrapModePopup();
      this.DoFilterModePopup();
      EditorGUI.BeginDisabledGroup(this.RenderTextureHasDepth());
      this.DoAnisoLevelSlider();
      EditorGUI.EndDisabledGroup();
      if (this.RenderTextureHasDepth())
      {
        this.m_Aniso.intValue = 0;
        EditorGUILayout.HelpBox("RenderTextures with depth must have an Aniso Level of 0.", MessageType.Info);
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private bool RenderTextureHasDepth()
    {
      if (TextureUtil.IsDepthRTFormat((RenderTextureFormat) this.m_ColorFormat.enumValueIndex))
        return true;
      return this.m_DepthFormat.enumValueIndex != 0;
    }

    public override string GetInfoString()
    {
      RenderTexture target = this.target as RenderTexture;
      string str = target.width.ToString() + "x" + (object) target.height;
      if (!target.isPowerOfTwo)
        str += "(NPOT)";
      return str + "  " + (object) target.format + "  " + EditorUtility.FormatBytes(TextureUtil.GetRuntimeMemorySize((Texture) target));
    }
  }
}
