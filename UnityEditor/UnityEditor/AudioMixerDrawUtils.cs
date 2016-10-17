// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerDrawUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerDrawUtils
  {
    private static float vertexOffset = -1f;
    private static readonly Color kAttenuationColor = AudioCurveRendering.kAudioOrange;
    private static readonly Color kEffectColor = new Color(0.4039216f, 0.627451f, 0.0f);
    private static readonly Color kSendColor = new Color(0.0f, 0.5764706f, 0.6666667f);
    private static readonly Color kReceiveColor = AudioMixerDrawUtils.kSendColor;
    private static readonly Color kDuckVolumeColor = AudioMixerDrawUtils.kSendColor;
    public static Color kBackgroundHi = new Color(0.5f, 0.5f, 0.5f);
    public static Color kBackgroundLo = new Color(0.3f, 0.3f, 0.3f);
    public static Color kBackgroundHiHighlight = new Color(0.6f, 0.6f, 0.6f);
    public static Color kBackgroundLoHighlight = new Color(0.4f, 0.4f, 0.4f);
    internal const float kSectionHeaderHeight = 22f;
    internal const float kSpaceBetweenSections = 10f;
    private static AudioMixerDrawUtils.Styles s_Styles;

    public static AudioMixerDrawUtils.Styles styles
    {
      get
      {
        return AudioMixerDrawUtils.s_Styles;
      }
    }

    private static void DetectVertexOffset()
    {
      AudioMixerDrawUtils.vertexOffset = 0.0f;
    }

    public static Color GetEffectColor(AudioMixerEffectController effect)
    {
      if (effect.IsSend())
      {
        if ((Object) effect.sendTarget != (Object) null)
          return AudioMixerDrawUtils.kSendColor;
        return Color.gray;
      }
      if (effect.IsReceive())
        return AudioMixerDrawUtils.kReceiveColor;
      if (effect.IsDuckVolume())
        return AudioMixerDrawUtils.kDuckVolumeColor;
      if (effect.IsAttenuation())
        return AudioMixerDrawUtils.kAttenuationColor;
      return AudioMixerDrawUtils.kEffectColor;
    }

    public static void InitStyles()
    {
      if (AudioMixerDrawUtils.s_Styles != null)
        return;
      AudioMixerDrawUtils.s_Styles = new AudioMixerDrawUtils.Styles();
      AudioMixerDrawUtils.DetectVertexOffset();
    }

    public static float GetAlpha()
    {
      return GUI.enabled ? 1f : 0.7f;
    }

    public static void DrawSplitter()
    {
      Rect rect = GUILayoutUtility.GetRect(1f, 1f);
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 1.333f) : new Color(0.12f, 0.12f, 0.12f, 1.333f);
      EditorGUI.DrawRect(rect, color);
    }

    public static void Vertex(float x, float y)
    {
      GL.Vertex3(x + AudioMixerDrawUtils.vertexOffset, y + AudioMixerDrawUtils.vertexOffset, 0.0f);
    }

    public static void DrawLine(float x1, float y1, float x2, float y2, Color c)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      GL.Color(new Color(c.r, c.g, c.b, c.a * AudioMixerDrawUtils.GetAlpha()));
      AudioMixerDrawUtils.Vertex(x1, y1);
      AudioMixerDrawUtils.Vertex(x2, y2);
      GL.End();
    }

    public static void DrawGradientRect(Rect r, Color c1, Color c2)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(new Color(c1.r, c1.g, c1.b, c1.a * AudioMixerDrawUtils.GetAlpha()));
      AudioMixerDrawUtils.Vertex(r.x, r.y);
      AudioMixerDrawUtils.Vertex(r.x + r.width, r.y);
      GL.Color(new Color(c2.r, c2.g, c2.b, c2.a * AudioMixerDrawUtils.GetAlpha()));
      AudioMixerDrawUtils.Vertex(r.x + r.width, r.y + r.height);
      AudioMixerDrawUtils.Vertex(r.x, r.y + r.height);
      GL.End();
    }

    public static void DrawGradientRectHorizontal(Rect r, Color c1, Color c2)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(new Color(c1.r, c1.g, c1.b, c1.a * AudioMixerDrawUtils.GetAlpha()));
      AudioMixerDrawUtils.Vertex(r.x + r.width, r.y);
      AudioMixerDrawUtils.Vertex(r.x + r.width, r.y + r.height);
      GL.Color(new Color(c2.r, c2.g, c2.b, c2.a * AudioMixerDrawUtils.GetAlpha()));
      AudioMixerDrawUtils.Vertex(r.x, r.y + r.height);
      AudioMixerDrawUtils.Vertex(r.x, r.y);
      GL.End();
    }

    public static void DrawRegionBg(Rect rect, out Rect headerRect, out Rect contentRect)
    {
      headerRect = new Rect(rect.x + 2f, rect.y, rect.width - 2f, 22f);
      contentRect = new Rect(rect.x, headerRect.yMax, rect.width, rect.height - 22f);
      GUI.Label(new RectOffset(1, 1, 1, 1).Add(contentRect), GUIContent.none, EditorStyles.helpBox);
    }

    public static void HeaderLabel(Rect r, GUIContent text, Texture2D icon)
    {
      if ((Object) icon != (Object) null)
      {
        EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
        GUI.Label(r, (Texture) icon, AudioMixerDrawUtils.styles.headerStyle);
        EditorGUIUtility.SetIconSize(Vector2.zero);
        r.xMin += 20f;
      }
      GUI.Label(r, text, AudioMixerDrawUtils.styles.headerStyle);
    }

    public static GUIStyle BuildGUIStyleForLabel(Color color, int fontSize, bool wrapText, FontStyle fontstyle, TextAnchor anchor)
    {
      GUIStyle guiStyle = new GUIStyle();
      guiStyle.focused.background = guiStyle.onNormal.background;
      guiStyle.focused.textColor = color;
      guiStyle.alignment = anchor;
      guiStyle.fontSize = fontSize;
      guiStyle.fontStyle = fontstyle;
      guiStyle.wordWrap = wrapText;
      guiStyle.clipping = TextClipping.Clip;
      guiStyle.normal.textColor = color;
      guiStyle.padding = new RectOffset(4, 4, 4, 4);
      return guiStyle;
    }

    public static void ReadOnlyLabel(Rect r, GUIContent content, GUIStyle style)
    {
      GUI.Label(r, content, style);
    }

    public static void ReadOnlyLabel(Rect r, string text, GUIStyle style)
    {
      GUI.Label(r, GUIContent.Temp(text), style);
    }

    public static void ReadOnlyLabel(Rect r, string text, GUIStyle style, string tooltipText)
    {
      GUI.Label(r, GUIContent.Temp(text, tooltipText), style);
    }

    public static void AddTooltipOverlay(Rect r, string tooltip)
    {
      GUI.Label(r, GUIContent.Temp(string.Empty, tooltip), AudioMixerDrawUtils.s_Styles.headerStyle);
    }

    public static void DrawConnection(Color col, float mixLevel, float srcX, float srcY, float dstX, float dstY, float width)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      float num1 = dstX - srcX;
      float num2 = dstY - srcY;
      float num3 = width / Mathf.Sqrt((float) ((double) num1 * (double) num1 + (double) num2 * (double) num2));
      float num4 = num1 * num3;
      float num5 = num2 * num3;
      float num6 = 2f;
      float num7 = 1.2f;
      Vector3[] points = new Vector3[4];
      points[0] = new Vector3(dstX, dstY);
      points[1] = new Vector3((float) ((double) dstX - (double) num6 * (double) num4 + (double) num7 * (double) num5), (float) ((double) dstY - (double) num6 * (double) num5 - (double) num7 * (double) num4));
      points[2] = new Vector3((float) ((double) dstX - (double) num6 * (double) num4 - (double) num7 * (double) num5), (float) ((double) dstY - (double) num6 * (double) num5 + (double) num7 * (double) num4));
      points[3] = points[0];
      Color color = new Color(col.r, col.g, col.b, (float) ((double) mixLevel * 0.300000011920929 + 0.300000011920929));
      Shader.SetGlobalColor("_HandleColor", color);
      GL.Begin(4);
      GL.Color(color);
      GL.Vertex(points[0]);
      GL.Vertex(points[1]);
      GL.Vertex(points[2]);
      GL.End();
      Handles.DrawAAPolyLine(width, new Color[4]
      {
        color,
        color,
        color,
        color
      }, points);
      Handles.DrawAAPolyLine(width, new Color[2]
      {
        new Color(col.r, col.g, col.b, mixLevel),
        new Color(col.r, col.g, col.b, mixLevel)
      }, new Vector3[2]
      {
        new Vector3(srcX, srcY, 0.0f),
        new Vector3(dstX, dstY, 0.0f)
      });
    }

    public static void DrawVerticalShow(Rect rect, bool fadeToTheRight)
    {
      Rect texCoords = !fadeToTheRight ? new Rect(1f, 1f, -1f, -1f) : new Rect(0.0f, 0.0f, 1f, 1f);
      GUI.DrawTextureWithTexCoords(rect, (Texture) AudioMixerDrawUtils.styles.leftToRightShadowTexture, texCoords);
    }

    public static void DrawScrollDropShadow(Rect scrollViewRect, float scrollY, float contentHeight)
    {
      if (Event.current.type != EventType.Repaint || (double) contentHeight <= (double) scrollViewRect.height)
        return;
      Color color = GUI.color;
      float a1 = (double) scrollY <= 10.0 ? scrollY / 10f : 1f;
      if ((double) a1 < 1.0)
        GUI.color = new Color(1f, 1f, 1f, a1);
      if ((double) a1 > 0.0)
        GUI.DrawTexture(new Rect(scrollViewRect.x, scrollViewRect.y, scrollViewRect.width, 20f), (Texture) AudioMixerDrawUtils.styles.scrollShadowTexture);
      if ((double) a1 < 1.0)
        GUI.color = color;
      float num = Mathf.Max(contentHeight - scrollViewRect.height, 0.0f);
      float a2 = (double) num - (double) scrollY <= 10.0 ? (float) (((double) num - (double) scrollY) / 10.0) : 1f;
      if ((double) a2 < 1.0)
        GUI.color = new Color(1f, 1f, 1f, a2);
      if ((double) a2 > 0.0)
        GUI.DrawTextureWithTexCoords(new Rect(scrollViewRect.x, scrollViewRect.yMax - 10f, scrollViewRect.width, 10f), (Texture) AudioMixerDrawUtils.styles.scrollShadowTexture, new Rect(1f, 1f, -1f, -1f));
      if ((double) a2 >= 1.0)
        return;
      GUI.color = color;
    }

    public static void DrawRect(Rect rect, Color color)
    {
      Color color1 = GUI.color;
      GUI.color = color;
      GUI.Label(rect, GUIContent.Temp(string.Empty), EditorGUIUtility.whiteTextureStyle);
      GUI.color = color1;
    }

    public class Styles
    {
      public GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
      public GUIStyle reorderableListLabel = new GUIStyle((GUIStyle) "PR Label");
      public GUIStyle regionBg = AudioMixerDrawUtils.Styles.GetStyle("RegionBg");
      public GUIStyle channelStripVUMeterBg = AudioMixerDrawUtils.Styles.GetStyle("ChannelStripVUMeterBg");
      public GUIStyle channelStripAreaBackground = (GUIStyle) "AnimationCurveEditorBackground";
      public GUIStyle channelStripBg = AudioMixerDrawUtils.Styles.GetStyle("ChannelStripBg");
      public GUIStyle duckingMarker = AudioMixerDrawUtils.Styles.GetStyle("ChannelStripDuckingMarker");
      public GUIStyle channelStripAttenuationMarkerSquare = AudioMixerDrawUtils.Styles.GetStyle("ChannelStripAttenuationMarkerSquare");
      public GUIStyle soloToggle = AudioMixerDrawUtils.Styles.GetStyle("SoloToggle");
      public GUIStyle muteToggle = AudioMixerDrawUtils.Styles.GetStyle("MuteToggle");
      public GUIStyle bypassToggle = AudioMixerDrawUtils.Styles.GetStyle("BypassToggle");
      public GUIStyle circularToggle = AudioMixerDrawUtils.Styles.GetStyle("CircularToggle");
      public GUIStyle totalVULevel = new GUIStyle(EditorStyles.label);
      public GUIStyle attenuationBar = (GUIStyle) "ChannelStripAttenuationBar";
      public GUIStyle effectBar = (GUIStyle) "ChannelStripEffectBar";
      public GUIStyle sendReturnBar = (GUIStyle) "ChannelStripSendReturnBar";
      public GUIStyle effectName = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle vuValue = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle mixerHeader = new GUIStyle(EditorStyles.largeLabel);
      public GUIStyle warningOverlay = AudioMixerDrawUtils.Styles.GetStyle("WarningOverlay");
      public Texture2D scrollShadowTexture = EditorGUIUtility.FindTexture("ScrollShadow");
      public Texture2D leftToRightShadowTexture = EditorGUIUtility.FindTexture("LeftToRightShadow");
      public GUIContent soloGUIContent = new GUIContent(string.Empty, "Adds this group to set of soloed groups");
      public GUIContent muteGUIContent = new GUIContent(string.Empty, "Mutes this group");
      public GUIContent bypassGUIContent = new GUIContent(string.Empty, "Bypasses the effects on this group");
      public GUIContent effectSlotGUIContent = new GUIContent(string.Empty, "Drag horizontally to change wet mix levels or vertically to change order of effects. Note: Enable wet mixing in the context menu.");
      public GUIContent attenuationSlotGUIContent = new GUIContent(string.Empty, "Place the attenuation slot in the effect stack where attenuation should take effect");
      public GUIContent emptySendSlotGUIContent = new GUIContent(string.Empty, "Connect to a Receive in the context menu or in the inspector");
      public GUIContent returnSlotGUIContent = new GUIContent(string.Empty, "Connect a Send to this Receive");
      public GUIContent duckVolumeSlotGUIContent = new GUIContent(string.Empty, "Connect a Send to this Duck Volume");
      public GUIContent duckingFaderGUIContent = new GUIContent(string.Empty, "Ducking Fader");
      public GUIContent attenuationFader = new GUIContent(string.Empty, "Attenuation fader");
      public GUIContent vuMeterGUIContent = new GUIContent(string.Empty, "The VU meter shows the current level of the mix of all sounds and subgroups.");
      public GUIContent referencedGroups = new GUIContent("Referenced groups", "Mixer groups that are hidden but are referenced by the visible mixer groups are shown here for convenience");
      public GUIContent sendString = new GUIContent("s");
      public GUIStyle channelStripHeaderStyle;

      public Styles()
      {
        this.headerStyle.alignment = TextAnchor.MiddleLeft;
        Texture2D background = this.reorderableListLabel.hover.background;
        this.reorderableListLabel.normal.background = background;
        this.reorderableListLabel.active.background = background;
        this.reorderableListLabel.focused.background = background;
        this.reorderableListLabel.onNormal.background = background;
        this.reorderableListLabel.onHover.background = background;
        this.reorderableListLabel.onActive.background = background;
        this.reorderableListLabel.onFocused.background = background;
        RectOffset padding = this.reorderableListLabel.padding;
        int num1 = 0;
        this.reorderableListLabel.padding.right = num1;
        int num2 = num1;
        padding.left = num2;
        this.reorderableListLabel.alignment = TextAnchor.MiddleLeft;
        this.scrollShadowTexture = EditorGUIUtility.FindTexture("ScrollShadow");
        this.channelStripHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
        this.channelStripHeaderStyle.alignment = TextAnchor.MiddleLeft;
        this.channelStripHeaderStyle.fontSize = 11;
        this.channelStripHeaderStyle.fontStyle = FontStyle.Bold;
        this.channelStripHeaderStyle.wordWrap = false;
        this.channelStripHeaderStyle.clipping = TextClipping.Clip;
        this.channelStripHeaderStyle.padding = new RectOffset(4, 4, 4, 4);
        this.totalVULevel.alignment = TextAnchor.MiddleRight;
        this.totalVULevel.padding.right = 20;
        this.effectName.padding.left = 4;
        this.effectName.padding.top = 2;
        this.vuValue.padding.left = 4;
        this.vuValue.padding.right = 4;
        this.vuValue.padding.top = 0;
        this.vuValue.alignment = TextAnchor.MiddleRight;
        this.vuValue.clipping = TextClipping.Overflow;
        this.warningOverlay.alignment = TextAnchor.MiddleCenter;
        this.mixerHeader.fontStyle = FontStyle.Bold;
        this.mixerHeader.fontSize = 17;
        this.mixerHeader.margin = new RectOffset();
        this.mixerHeader.padding = new RectOffset();
        this.mixerHeader.alignment = TextAnchor.UpperLeft;
        if (EditorGUIUtility.isProSkin)
          this.mixerHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        else
          this.mixerHeader.normal.textColor = new Color(0.2f, 0.2f, 0.2f, 1f);
      }

      private static GUIStyle GetStyle(string styleName)
      {
        return (GUIStyle) styleName;
      }
    }
  }
}
