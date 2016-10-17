// Decompiled with JetBrains decompiler
// Type: UnityEditor.PaneDragTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  internal class PaneDragTab : GUIView
  {
    [SerializeField]
    private Vector2 m_ThumbnailSize = new Vector2(80f, 60f);
    private AnimBool m_PaneVisible = new AnimBool();
    private AnimBool m_TabVisible = new AnimBool();
    private float m_StartAlpha = 1f;
    private float m_TargetAlpha = 1f;
    private DropInfo.Type m_Type = ~DropInfo.Type.Tab;
    private const float kTopThumbnailOffset = 10f;
    [SerializeField]
    private bool m_Shadow;
    private static PaneDragTab s_Get;
    private Rect m_StartRect;
    [SerializeField]
    private Rect m_TargetRect;
    [SerializeField]
    private static GUIStyle s_PaneStyle;
    [SerializeField]
    private static GUIStyle s_TabStyle;
    private bool m_DidResizeOnLastLayout;
    private float m_StartTime;
    public GUIContent content;
    private Texture2D m_Thumbnail;
    [SerializeField]
    internal ContainerWindow m_Window;
    [SerializeField]
    private ContainerWindow m_InFrontOfWindow;

    public static PaneDragTab get
    {
      get
      {
        if (!(bool) ((Object) PaneDragTab.s_Get))
        {
          Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (PaneDragTab));
          if (objectsOfTypeAll.Length != 0)
            PaneDragTab.s_Get = (PaneDragTab) objectsOfTypeAll[0];
          if ((bool) ((Object) PaneDragTab.s_Get))
            return PaneDragTab.s_Get;
          PaneDragTab.s_Get = ScriptableObject.CreateInstance<PaneDragTab>();
        }
        return PaneDragTab.s_Get;
      }
    }

    public void OnEnable()
    {
      this.m_PaneVisible.valueChanged.AddListener(new UnityAction(((GUIView) this).Repaint));
      this.m_TabVisible.valueChanged.AddListener(new UnityAction(((GUIView) this).Repaint));
    }

    public void OnDisable()
    {
      this.m_PaneVisible.valueChanged.RemoveListener(new UnityAction(((GUIView) this).Repaint));
      this.m_TabVisible.valueChanged.RemoveListener(new UnityAction(((GUIView) this).Repaint));
    }

    public void GrabThumbnail()
    {
      if ((Object) this.m_Thumbnail != (Object) null)
        Object.DestroyImmediate((Object) this.m_Thumbnail);
      this.m_Thumbnail = new Texture2D(Screen.width, Screen.height);
      this.m_Thumbnail.ReadPixels(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), 0, 0);
      this.m_Thumbnail.Apply();
      this.m_ThumbnailSize = new Vector2((float) this.m_Thumbnail.width, (float) this.m_Thumbnail.height) * Mathf.Sqrt(Mathf.Clamp01(50000f / (float) (this.m_Thumbnail.width * this.m_Thumbnail.height)));
    }

    public void SetDropInfo(DropInfo di, Vector2 mouseScreenPos, ContainerWindow inFrontOf)
    {
      if (this.m_Type != di.type || di.type == DropInfo.Type.Pane && di.rect != this.m_TargetRect)
      {
        this.m_Type = di.type;
        this.m_StartRect = this.GetInterpolatedRect(this.CalcFade());
        this.m_StartTime = Time.realtimeSinceStartup;
        switch (di.type)
        {
          case DropInfo.Type.Tab:
          case DropInfo.Type.Pane:
            this.m_TargetAlpha = 1f;
            break;
          case DropInfo.Type.Window:
            this.m_TargetAlpha = 0.6f;
            break;
        }
      }
      switch (di.type)
      {
        case DropInfo.Type.Tab:
        case DropInfo.Type.Pane:
          this.m_TargetRect = di.rect;
          break;
        case DropInfo.Type.Window:
          this.m_TargetRect = new Rect(mouseScreenPos.x - this.m_ThumbnailSize.x / 2f, mouseScreenPos.y - this.m_ThumbnailSize.y / 2f, this.m_ThumbnailSize.x, this.m_ThumbnailSize.y);
          break;
      }
      this.m_PaneVisible.target = di.type == DropInfo.Type.Pane;
      this.m_TabVisible.target = di.type == DropInfo.Type.Tab;
      this.m_TargetRect.x = Mathf.Round(this.m_TargetRect.x);
      this.m_TargetRect.y = Mathf.Round(this.m_TargetRect.y);
      this.m_TargetRect.width = Mathf.Round(this.m_TargetRect.width);
      this.m_TargetRect.height = Mathf.Round(this.m_TargetRect.height);
      this.m_InFrontOfWindow = inFrontOf;
      this.m_Window.MoveInFrontOf(this.m_InFrontOfWindow);
      this.SetWindowPos(this.GetInterpolatedRect(this.CalcFade()));
      this.Repaint();
    }

    public void Close()
    {
      if ((Object) this.m_Thumbnail != (Object) null)
        Object.DestroyImmediate((Object) this.m_Thumbnail);
      if ((bool) ((Object) this.m_Window))
        this.m_Window.Close();
      Object.DestroyImmediate((Object) this, true);
      PaneDragTab.s_Get = (PaneDragTab) null;
    }

    public void Show(Rect pixelPos, Vector2 mouseScreenPosition)
    {
      if (!(bool) ((Object) this.m_Window))
      {
        this.m_Window = ScriptableObject.CreateInstance<ContainerWindow>();
        this.m_Window.m_DontSaveToLayout = true;
        this.SetMinMaxSizes(Vector2.zero, new Vector2(10000f, 10000f));
        this.SetWindowPos(pixelPos);
        this.m_Window.mainView = (View) this;
      }
      else
        this.SetWindowPos(pixelPos);
      this.m_Window.Show(ShowMode.NoShadow, true, false);
      this.m_TargetRect = pixelPos;
    }

    private void SetWindowPos(Rect screenPosition)
    {
      this.m_Window.position = screenPosition;
    }

    private float CalcFade()
    {
      if (Application.platform == RuntimePlatform.WindowsEditor)
        return 1f;
      return Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01((float) (5.0 * ((double) Time.realtimeSinceStartup - (double) this.m_StartTime))));
    }

    private Rect GetInterpolatedRect(float fade)
    {
      return new Rect(Mathf.Lerp(this.m_StartRect.x, this.m_TargetRect.x, fade), Mathf.Lerp(this.m_StartRect.y, this.m_TargetRect.y, fade), Mathf.Lerp(this.m_StartRect.width, this.m_TargetRect.width, fade), Mathf.Lerp(this.m_StartRect.height, this.m_TargetRect.height, fade));
    }

    private void OnGUI()
    {
      float num = this.CalcFade();
      if (PaneDragTab.s_PaneStyle == null)
      {
        PaneDragTab.s_PaneStyle = (GUIStyle) "dragtabdropwindow";
        PaneDragTab.s_TabStyle = (GUIStyle) "dragtab";
      }
      if (Event.current.type == EventType.Layout)
      {
        this.m_DidResizeOnLastLayout = !this.m_DidResizeOnLastLayout;
        if (!this.m_DidResizeOnLastLayout)
        {
          this.SetWindowPos(this.GetInterpolatedRect(num));
          if (Application.platform != RuntimePlatform.OSXEditor)
            return;
          this.m_Window.SetAlpha(Mathf.Lerp(this.m_StartAlpha, this.m_TargetAlpha, num));
          return;
        }
      }
      if (Event.current.type == EventType.Repaint)
      {
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 1f);
        if ((Object) this.m_Thumbnail != (Object) null)
          GUI.DrawTexture(new Rect(0.0f, 0.0f, this.position.width, this.position.height), (Texture) this.m_Thumbnail, ScaleMode.StretchToFill, false);
        if ((double) this.m_TabVisible.faded != 0.0)
        {
          GUI.color = new Color(1f, 1f, 1f, this.m_TabVisible.faded);
          PaneDragTab.s_TabStyle.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), this.content, false, false, true, true);
        }
        if ((double) this.m_PaneVisible.faded != 0.0)
        {
          GUI.color = new Color(1f, 1f, 1f, this.m_PaneVisible.faded);
          PaneDragTab.s_PaneStyle.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), this.content, false, false, true, true);
        }
        GUI.color = color;
      }
      if (Application.platform == RuntimePlatform.WindowsEditor)
        return;
      this.Repaint();
    }
  }
}
