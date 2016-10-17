// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.GraphicEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Extend this class to write your own graphic editor.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (MaskableGraphic), false)]
  public class GraphicEditor : Editor
  {
    protected SerializedProperty m_Script;
    protected SerializedProperty m_Color;
    protected SerializedProperty m_Material;
    protected SerializedProperty m_RaycastTarget;
    private GUIContent m_CorrectButtonContent;
    protected AnimBool m_ShowNativeSize;

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected virtual void OnDisable()
    {
      Tools.hidden = false;
      this.m_ShowNativeSize.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    protected virtual void OnEnable()
    {
      this.m_CorrectButtonContent = new GUIContent("Set Native Size", "Sets the size to match the content.");
      this.m_Script = this.serializedObject.FindProperty("m_Script");
      this.m_Color = this.serializedObject.FindProperty("m_Color");
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_RaycastTarget = this.serializedObject.FindProperty("m_RaycastTarget");
      this.m_ShowNativeSize = new AnimBool(false);
      this.m_ShowNativeSize.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    /// <summary>
    ///   <para>Implement specific GraphicEditor inspector GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Script);
      this.AppearanceControlsGUI();
      this.RaycastControlsGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    ///   <para>Set if the 'Set Native Size' button should be visible for this editor.</para>
    /// </summary>
    /// <param name="show"></param>
    /// <param name="instant"></param>
    protected void SetShowNativeSize(bool show, bool instant)
    {
      if (instant)
        this.m_ShowNativeSize.value = show;
      else
        this.m_ShowNativeSize.target = show;
    }

    /// <summary>
    ///   <para>GUI for showing a button that sets the size of the RectTransform to the native size for this Graphic.</para>
    /// </summary>
    protected void NativeSizeButtonGUI()
    {
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowNativeSize.faded))
      {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUIUtility.labelWidth);
        if (GUILayout.Button(this.m_CorrectButtonContent, EditorStyles.miniButton, new GUILayoutOption[0]))
        {
          foreach (Graphic graphic in ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, Graphic>((Func<UnityEngine.Object, Graphic>) (obj => obj as Graphic)))
          {
            Undo.RecordObject((UnityEngine.Object) graphic.rectTransform, "Set Native Size");
            graphic.SetNativeSize();
            EditorUtility.SetDirty((UnityEngine.Object) graphic);
          }
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndFadeGroup();
    }

    /// <summary>
    ///   <para>GUI related to the appearance of the graphic. Color and Material properties appear here.</para>
    /// </summary>
    protected void AppearanceControlsGUI()
    {
      EditorGUILayout.PropertyField(this.m_Color);
      EditorGUILayout.PropertyField(this.m_Material);
    }

    /// <summary>
    ///   <para>GUI related to the Raycasting settings for the graphic.</para>
    /// </summary>
    protected void RaycastControlsGUI()
    {
      EditorGUILayout.PropertyField(this.m_RaycastTarget);
    }
  }
}
