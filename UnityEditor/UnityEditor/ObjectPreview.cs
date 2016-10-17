// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base Class to derive from when creating Custom Previews.</para>
  /// </summary>
  public class ObjectPreview : IPreviewable
  {
    private const int kPreviewLabelHeight = 12;
    private const int kPreviewMinSize = 55;
    private const int kGridTargetCount = 25;
    private const int kGridSpacing = 10;
    private const int kPreviewLabelPadding = 5;
    private static ObjectPreview.Styles s_Styles;
    protected Object[] m_Targets;
    protected int m_ReferenceTargetIndex;

    /// <summary>
    ///   <para>The object currently being previewed.</para>
    /// </summary>
    public virtual Object target
    {
      get
      {
        return this.m_Targets[this.m_ReferenceTargetIndex];
      }
    }

    /// <summary>
    ///   <para>Called when the Preview gets created with the objects being previewed.</para>
    /// </summary>
    /// <param name="targets">The objects being previewed.</param>
    public virtual void Initialize(Object[] targets)
    {
      this.m_ReferenceTargetIndex = 0;
      this.m_Targets = targets;
    }

    /// <summary>
    ///   <para>Called to iterate through the targets, this will be used when previewing more than one target.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if there is another target available.</para>
    /// </returns>
    public virtual bool MoveNextTarget()
    {
      ++this.m_ReferenceTargetIndex;
      return this.m_ReferenceTargetIndex < this.m_Targets.Length - 1;
    }

    /// <summary>
    ///   <para>Called to Reset the target before iterating through them.</para>
    /// </summary>
    public virtual void ResetTarget()
    {
      this.m_ReferenceTargetIndex = 0;
    }

    /// <summary>
    ///   <para>Can this component be Previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public virtual bool HasPreviewGUI()
    {
      return false;
    }

    /// <summary>
    ///   <para>Override this method if you want to change the label of the Preview area.</para>
    /// </summary>
    public virtual GUIContent GetPreviewTitle()
    {
      GUIContent guiContent = new GUIContent();
      if (this.m_Targets.Length == 1)
      {
        guiContent.text = this.target.name;
      }
      else
      {
        guiContent.text = this.m_Targets.Length.ToString() + " ";
        if (this.target is MonoBehaviour)
          guiContent.text += MonoScript.FromMonoBehaviour(this.target as MonoBehaviour).GetClass().Name;
        else
          guiContent.text += ObjectNames.NicifyVariableName(ObjectNames.GetClassName(this.target));
        guiContent.text += "s";
      }
      return guiContent;
    }

    /// <summary>
    ///   <para>Implement to create your own custom preview for the preview area of the inspector, primary editor headers and the object selector.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public virtual void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      background.Draw(r, false, false, false, false);
    }

    /// <summary>
    ///   <para>Implement to create your own interactive custom preview. Interactive custom previews are used in the preview area of the inspector and the object selector.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public virtual void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      this.OnPreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Override this method if you want to show custom controls in the preview header.</para>
    /// </summary>
    public virtual void OnPreviewSettings()
    {
    }

    /// <summary>
    ///   <para>Implement this method to show object information on top of the object preview.</para>
    /// </summary>
    public virtual string GetInfoString()
    {
      return string.Empty;
    }

    /// <summary>
    ///   <para>This is the first entry point for Preview Drawing.</para>
    /// </summary>
    /// <param name="previewArea">The available area to draw the preview.</param>
    public void DrawPreview(Rect previewArea)
    {
      ObjectPreview.DrawPreview((IPreviewable) this, previewArea, this.m_Targets);
    }

    public virtual void ReloadPreviewInstances()
    {
    }

    internal static void DrawPreview(IPreviewable defaultPreview, Rect previewArea, Object[] targets)
    {
      if (ObjectPreview.s_Styles == null)
        ObjectPreview.s_Styles = new ObjectPreview.Styles();
      string str1 = string.Empty;
      Event current = Event.current;
      if (targets.Length > 1)
      {
        Rect rect = new RectOffset(16, 16, 20, 25).Remove(previewArea);
        int num1 = Mathf.Max(1, Mathf.FloorToInt((float) (((double) rect.height + 10.0) / 77.0)));
        int num2 = Mathf.Max(1, Mathf.FloorToInt((float) (((double) rect.width + 10.0) / 65.0)));
        int num3 = num1 * num2;
        int minimumNr = Mathf.Min(targets.Length, 25);
        bool flag1 = true;
        int[] numArray = new int[2]{ num2, num1 };
        if (minimumNr < num3)
        {
          numArray = ObjectPreview.GetGridDivision(rect, minimumNr, 12);
          flag1 = false;
        }
        int num4 = Mathf.Min(numArray[0] * numArray[1], targets.Length);
        rect.width += 10f;
        rect.height += 10f;
        Vector2 vector2 = new Vector2((float) Mathf.FloorToInt((float) ((double) rect.width / (double) numArray[0] - 10.0)), (float) Mathf.FloorToInt((float) ((double) rect.height / (double) numArray[1] - 10.0)));
        float num5 = Mathf.Min(vector2.x, vector2.y - 12f);
        if (flag1)
          num5 = Mathf.Min(num5, 55f);
        bool flag2 = current.type == EventType.MouseDown && current.button == 0 && current.clickCount == 2 && previewArea.Contains(current.mousePosition);
        defaultPreview.ResetTarget();
        for (int index = 0; index < num4; ++index)
        {
          Rect position1 = new Rect(rect.x + (float) (index % numArray[0]) * rect.width / (float) numArray[0], rect.y + (float) (index / numArray[0]) * rect.height / (float) numArray[1], vector2.x, vector2.y);
          if (flag2 && position1.Contains(Event.current.mousePosition))
            Selection.objects = new Object[1]
            {
              defaultPreview.target
            };
          position1.height -= 12f;
          Rect position2 = new Rect(position1.x + (float) (((double) position1.width - (double) num5) * 0.5), position1.y + (float) (((double) position1.height - (double) num5) * 0.5), num5, num5);
          GUI.BeginGroup(position2);
          Editor.m_AllowMultiObjectAccess = false;
          defaultPreview.OnInteractivePreviewGUI(new Rect(0.0f, 0.0f, num5, num5), ObjectPreview.s_Styles.preBackgroundSolid);
          Editor.m_AllowMultiObjectAccess = true;
          GUI.EndGroup();
          position1.y = position2.yMax;
          position1.height = 16f;
          GUI.Label(position1, targets[index].name, ObjectPreview.s_Styles.previewMiniLabel);
          defaultPreview.MoveNextTarget();
        }
        defaultPreview.ResetTarget();
        if (Event.current.type == EventType.Repaint)
          str1 = string.Format("Previewing {0} of {1} Objects", (object) num4, (object) targets.Length);
      }
      else
      {
        defaultPreview.OnInteractivePreviewGUI(previewArea, ObjectPreview.s_Styles.preBackground);
        if (Event.current.type == EventType.Repaint)
        {
          str1 = defaultPreview.GetInfoString();
          if (str1 != string.Empty)
          {
            string str2 = str1.Replace("\n", "   ");
            str1 = string.Format("{0}\n{1}", (object) defaultPreview.target.name, (object) str2);
          }
        }
      }
      if (Event.current.type != EventType.Repaint || !(str1 != string.Empty))
        return;
      float height = ObjectPreview.s_Styles.dropShadowLabelStyle.CalcHeight(GUIContent.Temp(str1), previewArea.width);
      EditorGUI.DropShadowLabel(new Rect(previewArea.x, (float) ((double) previewArea.yMax - (double) height - 5.0), previewArea.width, height), str1);
    }

    private static int[] GetGridDivision(Rect rect, int minimumNr, int labelHeight)
    {
      float num1 = Mathf.Sqrt(rect.width * rect.height / (float) minimumNr);
      int num2 = Mathf.FloorToInt(rect.width / num1);
      int num3 = Mathf.FloorToInt(rect.height / (num1 + (float) labelHeight));
      while (num2 * num3 < minimumNr)
      {
        if ((double) ObjectPreview.AbsRatioDiff((float) (num2 + 1) / rect.width, (float) num3 / (rect.height - (float) (num3 * labelHeight))) < (double) ObjectPreview.AbsRatioDiff((float) num2 / rect.width, (float) (num3 + 1) / (rect.height - (float) ((num3 + 1) * labelHeight))))
        {
          ++num2;
          if (num2 * num3 > minimumNr)
            num3 = Mathf.CeilToInt((float) minimumNr / (float) num2);
        }
        else
        {
          ++num3;
          if (num2 * num3 > minimumNr)
            num2 = Mathf.CeilToInt((float) minimumNr / (float) num3);
        }
      }
      return new int[2]{ num2, num3 };
    }

    private static float AbsRatioDiff(float x, float y)
    {
      return Mathf.Max(x / y, y / x);
    }

    private class Styles
    {
      public GUIStyle preBackground = (GUIStyle) "preBackground";
      public GUIStyle preBackgroundSolid = new GUIStyle((GUIStyle) "preBackground");
      public GUIStyle previewMiniLabel = new GUIStyle(EditorStyles.whiteMiniLabel);
      public GUIStyle dropShadowLabelStyle = new GUIStyle((GUIStyle) "PreOverlayLabel");

      public Styles()
      {
        this.preBackgroundSolid.overflow = this.preBackgroundSolid.border;
        this.previewMiniLabel.alignment = TextAnchor.UpperCenter;
      }
    }
  }
}
