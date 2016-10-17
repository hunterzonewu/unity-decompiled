// Decompiled with JetBrains decompiler
// Type: UnityEditor.RendererEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class RendererEditorBase : Editor
  {
    private GUIContent m_SortingLayerStyle = EditorGUIUtility.TextContent("Sorting Layer");
    private GUIContent m_SortingOrderStyle = EditorGUIUtility.TextContent("Order in Layer");
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_SortingLayerID;
    protected RendererEditorBase.Probes m_Probes;

    public virtual void OnEnable()
    {
      this.m_SortingOrder = this.serializedObject.FindProperty("m_SortingOrder");
      this.m_SortingLayerID = this.serializedObject.FindProperty("m_SortingLayerID");
    }

    protected void RenderSortingLayerFields()
    {
      EditorGUILayout.Space();
      EditorGUILayout.SortingLayerField(this.m_SortingLayerStyle, this.m_SortingLayerID, EditorStyles.popup, EditorStyles.label);
      EditorGUILayout.PropertyField(this.m_SortingOrder, this.m_SortingOrderStyle, new GUILayoutOption[0]);
    }

    protected void InitializeProbeFields()
    {
      this.m_Probes = new RendererEditorBase.Probes();
      this.m_Probes.Initialize(this.serializedObject);
    }

    protected void RenderProbeFields()
    {
      this.m_Probes.OnGUI(this.targets, (Renderer) this.target, false);
    }

    internal class Probes
    {
      private GUIContent m_UseLightProbesStyle = EditorGUIUtility.TextContent("Use Light Probes");
      private GUIContent m_ReflectionProbeUsageStyle = EditorGUIUtility.TextContent("Reflection Probes");
      private GUIContent m_ProbeAnchorStyle = EditorGUIUtility.TextContent("Anchor Override|If set, the Renderer will use this Transform's position to sample light probes and find the matching reflection probe.");
      private GUIContent m_DeferredNote = EditorGUIUtility.TextContent("In Deferred Shading, all objects receive shadows and get per-pixel reflection probes.");
      private string[] m_ReflectionProbeUsageNames = ((IEnumerable<string>) Enum.GetNames(typeof (ReflectionProbeUsage))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      private GUIContent[] m_ReflectionProbeUsageOptions = ((IEnumerable<string>) ((IEnumerable<string>) Enum.GetNames(typeof (ReflectionProbeUsage))).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      private List<ReflectionProbeBlendInfo> m_BlendInfo = new List<ReflectionProbeBlendInfo>();
      private SerializedProperty m_UseLightProbes;
      private SerializedProperty m_ReflectionProbeUsage;
      private SerializedProperty m_ProbeAnchor;
      private SerializedProperty m_ReceiveShadows;

      internal void Initialize(SerializedObject serializedObject, bool initializeLightProbes)
      {
        if (initializeLightProbes)
          this.m_UseLightProbes = serializedObject.FindProperty("m_UseLightProbes");
        this.m_ReflectionProbeUsage = serializedObject.FindProperty("m_ReflectionProbeUsage");
        this.m_ProbeAnchor = serializedObject.FindProperty("m_ProbeAnchor");
        this.m_ReceiveShadows = serializedObject.FindProperty("m_ReceiveShadows");
      }

      internal void Initialize(SerializedObject serializedObject)
      {
        this.Initialize(serializedObject, true);
      }

      internal void OnGUI(UnityEngine.Object[] targets, Renderer renderer, bool useMiniStyle)
      {
        bool disabled1 = SceneView.IsUsingDeferredRenderingPath();
        bool flag1 = disabled1 && UnityEngine.Rendering.GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) != UnityEngine.Rendering.BuiltinShaderMode.Disabled;
        bool disabled2 = false;
        if (targets != null)
        {
          foreach (Renderer target in targets)
          {
            if (LightmapEditorSettings.IsLightmappedOrDynamicLightmappedForRendering(target))
            {
              disabled2 = true;
              break;
            }
          }
        }
        if (this.m_UseLightProbes != null)
        {
          EditorGUI.BeginDisabledGroup(disabled2);
          if (!useMiniStyle)
          {
            if (disabled2)
              EditorGUILayout.Toggle(this.m_UseLightProbesStyle, false, new GUILayoutOption[0]);
            else
              EditorGUILayout.PropertyField(this.m_UseLightProbes, this.m_UseLightProbesStyle, new GUILayoutOption[0]);
          }
          else if (disabled2)
            ModuleUI.GUIToggle(this.m_UseLightProbesStyle, false);
          else
            ModuleUI.GUIToggle(this.m_UseLightProbesStyle, this.m_UseLightProbes);
          EditorGUI.EndDisabledGroup();
        }
        EditorGUI.BeginDisabledGroup(disabled1);
        if (!useMiniStyle)
        {
          if (flag1)
            EditorGUILayout.EnumPopup(this.m_ReflectionProbeUsageStyle, (Enum) (ReflectionProbeUsage) (this.m_ReflectionProbeUsage.intValue == 0 ? 0 : 3), new GUILayoutOption[0]);
          else
            EditorGUILayout.Popup(this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageOptions, this.m_ReflectionProbeUsageStyle, new GUILayoutOption[0]);
        }
        else if (flag1)
          ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, 3, this.m_ReflectionProbeUsageNames);
        else
          ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageNames);
        EditorGUI.EndDisabledGroup();
        bool flag2 = !this.m_ReflectionProbeUsage.hasMultipleDifferentValues && this.m_ReflectionProbeUsage.intValue != 0 || this.m_UseLightProbes != null && !this.m_UseLightProbes.hasMultipleDifferentValues && this.m_UseLightProbes.boolValue;
        if (flag2)
        {
          if (!useMiniStyle)
            EditorGUILayout.PropertyField(this.m_ProbeAnchor, this.m_ProbeAnchorStyle, new GUILayoutOption[0]);
          else
            ModuleUI.GUIObject(this.m_ProbeAnchorStyle, this.m_ProbeAnchor);
          if (!flag1)
          {
            renderer.GetClosestReflectionProbes(this.m_BlendInfo);
            RendererEditorBase.Probes.ShowClosestReflectionProbes(this.m_BlendInfo);
          }
        }
        bool flag3 = !this.m_ReceiveShadows.hasMultipleDifferentValues && this.m_ReceiveShadows.boolValue;
        if ((!disabled1 || !flag3) && (!flag1 || !flag2))
          return;
        EditorGUILayout.HelpBox(this.m_DeferredNote.text, MessageType.Info);
      }

      internal static void ShowClosestReflectionProbes(List<ReflectionProbeBlendInfo> blendInfos)
      {
        float num1 = 20f;
        float num2 = 60f;
        EditorGUI.BeginDisabledGroup(true);
        for (int index = 0; index < blendInfos.Count; ++index)
        {
          Rect source = GUILayoutUtility.GetRect(0.0f, 16f);
          source = EditorGUI.IndentedRect(source);
          float num3 = source.width - num1 - num2;
          Rect position = source;
          position.width = num1;
          GUI.Label(position, "#" + (object) index, EditorStyles.miniLabel);
          position.x += position.width;
          position.width = num3;
          EditorGUI.ObjectField(position, (UnityEngine.Object) blendInfos[index].probe, typeof (ReflectionProbe), true);
          position.x += position.width;
          position.width = num2;
          GUI.Label(position, "Weight " + blendInfos[index].weight.ToString("f2"), EditorStyles.miniLabel);
        }
        EditorGUI.EndDisabledGroup();
      }

      internal static string[] GetFieldsStringArray()
      {
        return new string[3]{ "m_UseLightProbes", "m_ReflectionProbeUsage", "m_ProbeAnchor" };
      }
    }
  }
}
