// Decompiled with JetBrains decompiler
// Type: UnityEditor.Events.LayoutPropertiesPreview
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Events
{
  [CustomPreview(typeof (GameObject))]
  internal class LayoutPropertiesPreview : ObjectPreview
  {
    private LayoutPropertiesPreview.Styles m_Styles = new LayoutPropertiesPreview.Styles();
    private const float kLabelWidth = 110f;
    private const float kValueWidth = 100f;
    private GUIContent m_Title;

    public override void Initialize(UnityEngine.Object[] targets)
    {
      base.Initialize(targets);
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("Layout Properties");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      GameObject target = this.target as GameObject;
      if (!(bool) ((UnityEngine.Object) target))
        return false;
      return (UnityEngine.Object) target.GetComponent(typeof (ILayoutElement)) != (UnityEngine.Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (this.m_Styles == null)
        this.m_Styles = new LayoutPropertiesPreview.Styles();
      RectTransform transform = (this.target as GameObject).transform as RectTransform;
      if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
        return;
      r = new RectOffset(-5, -5, -5, -5).Add(r);
      r.height = EditorGUIUtility.singleLineHeight;
      Rect labelRect = r;
      Rect valueRect = r;
      Rect sourceRect = r;
      labelRect.width = 110f;
      valueRect.xMin += 110f;
      valueRect.width = 100f;
      sourceRect.xMin += 210f;
      GUI.Label(labelRect, "Property", this.m_Styles.headerStyle);
      GUI.Label(valueRect, "Value", this.m_Styles.headerStyle);
      GUI.Label(sourceRect, "Source", this.m_Styles.headerStyle);
      labelRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      valueRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      sourceRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      ILayoutElement source = (ILayoutElement) null;
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Min Width", LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.minWidth), 0.0f, out source).ToString(), source);
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Min Height", LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.minHeight), 0.0f, out source).ToString(), source);
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Preferred Width", LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.preferredWidth), 0.0f, out source).ToString(), source);
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Preferred Height", LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.preferredHeight), 0.0f, out source).ToString(), source);
      float layoutProperty1 = LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.flexibleWidth), 0.0f, out source);
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Flexible Width", (double) layoutProperty1 <= 0.0 ? "disabled" : "enabled (" + layoutProperty1.ToString() + ")", source);
      float layoutProperty2 = LayoutUtility.GetLayoutProperty(transform, (Func<ILayoutElement, float>) (e => e.flexibleHeight), 0.0f, out source);
      this.ShowProp(ref labelRect, ref valueRect, ref sourceRect, "Flexible Height", (double) layoutProperty2 <= 0.0 ? "disabled" : "enabled (" + layoutProperty2.ToString() + ")", source);
      if ((bool) ((UnityEngine.Object) transform.GetComponent<LayoutElement>()))
        return;
      GUI.Label(new Rect(labelRect.x, labelRect.y + 10f, r.width, EditorGUIUtility.singleLineHeight), "Add a LayoutElement to override values.", this.m_Styles.labelStyle);
    }

    private void ShowProp(ref Rect labelRect, ref Rect valueRect, ref Rect sourceRect, string label, string value, ILayoutElement source)
    {
      GUI.Label(labelRect, label, this.m_Styles.labelStyle);
      GUI.Label(valueRect, value, this.m_Styles.labelStyle);
      GUI.Label(sourceRect, source != null ? source.GetType().Name : "none", this.m_Styles.labelStyle);
      labelRect.y += EditorGUIUtility.singleLineHeight;
      valueRect.y += EditorGUIUtility.singleLineHeight;
      sourceRect.y += EditorGUIUtility.singleLineHeight;
    }

    private class Styles
    {
      public GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
      public GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);

      public Styles()
      {
        Color color = new Color(0.7f, 0.7f, 0.7f);
        this.labelStyle.padding.right += 4;
        this.labelStyle.normal.textColor = color;
        this.headerStyle.padding.right += 4;
        this.headerStyle.normal.textColor = color;
      }
    }
  }
}
