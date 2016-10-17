// Decompiled with JetBrains decompiler
// Type: UnityEditor.TooltipView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TooltipView : GUIView
  {
    private GUIContent m_tooltip = new GUIContent();
    private const float MAX_WIDTH = 300f;
    private Vector2 m_optimalSize;
    private GUIStyle m_Style;
    private Rect m_hoverRect;
    private ContainerWindow m_tooltipContainer;
    private static TooltipView s_guiView;

    private void OnEnable()
    {
      TooltipView.s_guiView = this;
    }

    private void OnDisable()
    {
      TooltipView.s_guiView = (TooltipView) null;
    }

    private void OnGUI()
    {
      if (!((Object) this.m_tooltipContainer != (Object) null))
        return;
      GUI.Box(new Rect(0.0f, 0.0f, this.m_optimalSize.x, this.m_optimalSize.y), this.m_tooltip, this.m_Style);
    }

    private void Setup(string tooltip, Rect rect)
    {
      this.m_hoverRect = rect;
      this.m_tooltip.text = tooltip;
      this.m_Style = EditorStyles.tooltip;
      this.m_Style.wordWrap = false;
      this.m_optimalSize = this.m_Style.CalcSize(this.m_tooltip);
      if ((double) this.m_optimalSize.x > 300.0)
      {
        this.m_Style.wordWrap = true;
        this.m_optimalSize.x = 300f;
        this.m_optimalSize.y = this.m_Style.CalcHeight(this.m_tooltip, 300f);
      }
      this.m_tooltipContainer.position = new Rect(Mathf.Floor((float) ((double) this.m_hoverRect.x + (double) this.m_hoverRect.width / 2.0 - (double) this.m_optimalSize.x / 2.0)), Mathf.Floor((float) ((double) this.m_hoverRect.y + (double) this.m_hoverRect.height + 10.0)), this.m_optimalSize.x, this.m_optimalSize.y);
      this.position = new Rect(0.0f, 0.0f, this.m_optimalSize.x, this.m_optimalSize.y);
      this.m_tooltipContainer.ShowPopup();
      this.m_tooltipContainer.SetAlpha(1f);
      TooltipView.s_guiView.mouseRayInvisible = true;
      this.RepaintImmediately();
    }

    public static void Show(string tooltip, Rect rect)
    {
      if ((Object) TooltipView.s_guiView == (Object) null)
      {
        TooltipView.s_guiView = ScriptableObject.CreateInstance<TooltipView>();
        TooltipView.s_guiView.m_tooltipContainer = ScriptableObject.CreateInstance<ContainerWindow>();
        TooltipView.s_guiView.m_tooltipContainer.m_DontSaveToLayout = true;
        TooltipView.s_guiView.m_tooltipContainer.mainView = (View) TooltipView.s_guiView;
        TooltipView.s_guiView.m_tooltipContainer.SetMinMaxSizes(new Vector2(10f, 10f), new Vector2(2000f, 2000f));
      }
      if (TooltipView.s_guiView.m_tooltip.text == tooltip && rect == TooltipView.s_guiView.m_hoverRect)
        return;
      TooltipView.s_guiView.Setup(tooltip, rect);
    }

    public static void Close()
    {
      if (!((Object) TooltipView.s_guiView != (Object) null))
        return;
      TooltipView.s_guiView.m_tooltipContainer.Close();
    }

    public static void SetAlpha(float percent)
    {
      if (!((Object) TooltipView.s_guiView != (Object) null))
        return;
      TooltipView.s_guiView.m_tooltipContainer.SetAlpha(percent);
    }
  }
}
