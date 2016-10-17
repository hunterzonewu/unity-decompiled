// Decompiled with JetBrains decompiler
// Type: UnityEditor.EventSystems.EventSystemEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEditor.EventSystems
{
  /// <summary>
  ///   <para>Custom Editor for the EventSystem Component.</para>
  /// </summary>
  [CustomEditor(typeof (EventSystem), true)]
  public class EventSystemEditor : Editor
  {
    private GUIStyle m_PreviewLabelStyle;

    protected GUIStyle previewLabelStyle
    {
      get
      {
        if (this.m_PreviewLabelStyle == null)
          this.m_PreviewLabelStyle = new GUIStyle((GUIStyle) "PreOverlayLabel")
          {
            richText = true,
            alignment = TextAnchor.UpperLeft,
            fontStyle = FontStyle.Normal
          };
        return this.m_PreviewLabelStyle;
      }
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
      EventSystem target = this.target as EventSystem;
      if ((Object) target == (Object) null || (Object) target.GetComponent<BaseInputModule>() != (Object) null || !GUILayout.Button("Add Default Input Modules"))
        return;
      Undo.AddComponent<StandaloneInputModule>(target.gameObject);
    }

    /// <summary>
    ///   <para>Can this component be previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public override bool HasPreviewGUI()
    {
      return Application.isPlaying;
    }

    /// <summary>
    ///   <para>Does this edit require to be repainted constantly in its current state?</para>
    /// </summary>
    public override bool RequiresConstantRepaint()
    {
      return Application.isPlaying;
    }

    /// <summary>
    ///   <para>Custom preview for Image component.</para>
    /// </summary>
    /// <param name="rect">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
      EventSystem target = this.target as EventSystem;
      if ((Object) target == (Object) null)
        return;
      GUI.Label(rect, target.ToString(), this.previewLabelStyle);
    }
  }
}
