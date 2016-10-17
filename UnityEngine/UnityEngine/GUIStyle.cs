// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIStyle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Styling information for GUI elements.</para>
  /// </summary>
  [RequiredByNativeCode]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class GUIStyle
  {
    internal static bool showKeyboardFocus = true;
    [NonSerialized]
    internal IntPtr m_Ptr;
    [NonSerialized]
    private GUIStyleState m_Normal;
    [NonSerialized]
    private GUIStyleState m_Hover;
    [NonSerialized]
    private GUIStyleState m_Active;
    [NonSerialized]
    private GUIStyleState m_Focused;
    [NonSerialized]
    private GUIStyleState m_OnNormal;
    [NonSerialized]
    private GUIStyleState m_OnHover;
    [NonSerialized]
    private GUIStyleState m_OnActive;
    [NonSerialized]
    private GUIStyleState m_OnFocused;
    [NonSerialized]
    private RectOffset m_Border;
    [NonSerialized]
    private RectOffset m_Padding;
    [NonSerialized]
    private RectOffset m_Margin;
    [NonSerialized]
    private RectOffset m_Overflow;
    [NonSerialized]
    private Font m_FontInternal;
    private static GUIStyle s_None;

    /// <summary>
    ///   <para>Rendering settings for when the component is displayed normally.</para>
    /// </summary>
    public GUIStyleState normal
    {
      get
      {
        if (this.m_Normal == null)
          this.m_Normal = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(0));
        return this.m_Normal;
      }
      set
      {
        this.AssignStyleState(0, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the mouse is hovering over the control.</para>
    /// </summary>
    public GUIStyleState hover
    {
      get
      {
        if (this.m_Hover == null)
          this.m_Hover = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(1));
        return this.m_Hover;
      }
      set
      {
        this.AssignStyleState(1, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the control is pressed down.</para>
    /// </summary>
    public GUIStyleState active
    {
      get
      {
        if (this.m_Active == null)
          this.m_Active = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(2));
        return this.m_Active;
      }
      set
      {
        this.AssignStyleState(2, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the control is turned on.</para>
    /// </summary>
    public GUIStyleState onNormal
    {
      get
      {
        if (this.m_OnNormal == null)
          this.m_OnNormal = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(4));
        return this.m_OnNormal;
      }
      set
      {
        this.AssignStyleState(4, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the control is turned on and the mouse is hovering it.</para>
    /// </summary>
    public GUIStyleState onHover
    {
      get
      {
        if (this.m_OnHover == null)
          this.m_OnHover = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(5));
        return this.m_OnHover;
      }
      set
      {
        this.AssignStyleState(5, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the element is turned on and pressed down.</para>
    /// </summary>
    public GUIStyleState onActive
    {
      get
      {
        if (this.m_OnActive == null)
          this.m_OnActive = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(6));
        return this.m_OnActive;
      }
      set
      {
        this.AssignStyleState(6, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the element has keyboard focus.</para>
    /// </summary>
    public GUIStyleState focused
    {
      get
      {
        if (this.m_Focused == null)
          this.m_Focused = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(3));
        return this.m_Focused;
      }
      set
      {
        this.AssignStyleState(3, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Rendering settings for when the element has keyboard and is turned on.</para>
    /// </summary>
    public GUIStyleState onFocused
    {
      get
      {
        if (this.m_OnFocused == null)
          this.m_OnFocused = GUIStyleState.GetGUIStyleState(this, this.GetStyleStatePtr(7));
        return this.m_OnFocused;
      }
      set
      {
        this.AssignStyleState(7, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>The borders of all background images.</para>
    /// </summary>
    public RectOffset border
    {
      get
      {
        if (this.m_Border == null)
          this.m_Border = new RectOffset(this, this.GetRectOffsetPtr(0));
        return this.m_Border;
      }
      set
      {
        this.AssignRectOffset(0, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>The margins between elements rendered in this style and any other GUI elements.</para>
    /// </summary>
    public RectOffset margin
    {
      get
      {
        if (this.m_Margin == null)
          this.m_Margin = new RectOffset(this, this.GetRectOffsetPtr(1));
        return this.m_Margin;
      }
      set
      {
        this.AssignRectOffset(1, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Space from the edge of GUIStyle to the start of the contents.</para>
    /// </summary>
    public RectOffset padding
    {
      get
      {
        if (this.m_Padding == null)
          this.m_Padding = new RectOffset(this, this.GetRectOffsetPtr(2));
        return this.m_Padding;
      }
      set
      {
        this.AssignRectOffset(2, value.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Extra space to be added to the background image.</para>
    /// </summary>
    public RectOffset overflow
    {
      get
      {
        if (this.m_Overflow == null)
          this.m_Overflow = new RectOffset(this, this.GetRectOffsetPtr(3));
        return this.m_Overflow;
      }
      set
      {
        this.AssignRectOffset(3, value.m_Ptr);
      }
    }

    [Obsolete("warning Don't use clipOffset - put things inside BeginGroup instead. This functionality will be removed in a later version.")]
    public Vector2 clipOffset
    {
      get
      {
        return this.Internal_clipOffset;
      }
      set
      {
        this.Internal_clipOffset = value;
      }
    }

    /// <summary>
    ///   <para>The font to use for rendering. If null, the default font for the current GUISkin is used instead.</para>
    /// </summary>
    public Font font
    {
      get
      {
        return this.GetFontInternal();
      }
      set
      {
        this.SetFontInternal(value);
        this.m_FontInternal = value;
      }
    }

    /// <summary>
    ///   <para>The height of one line of text with this style, measured in pixels. (Read Only)</para>
    /// </summary>
    public float lineHeight
    {
      get
      {
        return Mathf.Round(GUIStyle.Internal_GetLineHeight(this.m_Ptr));
      }
    }

    /// <summary>
    ///   <para>Shortcut for an empty GUIStyle.</para>
    /// </summary>
    public static GUIStyle none
    {
      get
      {
        if (GUIStyle.s_None == null)
          GUIStyle.s_None = new GUIStyle();
        return GUIStyle.s_None;
      }
    }

    public bool isHeightDependantOnWidth
    {
      get
      {
        if ((double) this.fixedHeight == 0.0 && this.wordWrap)
          return this.imagePosition != ImagePosition.ImageOnly;
        return false;
      }
    }

    /// <summary>
    ///   <para>The name of this GUIStyle. Used for getting them based on name.</para>
    /// </summary>
    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How image and text of the GUIContent is combined.</para>
    /// </summary>
    public ImagePosition imagePosition { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Text alignment.</para>
    /// </summary>
    public TextAnchor alignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the text be wordwrapped?</para>
    /// </summary>
    public bool wordWrap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>What to do when the contents to be rendered is too large to fit within the area given.</para>
    /// </summary>
    public TextClipping clipping { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Pixel offset to apply to the content of this GUIstyle.</para>
    /// </summary>
    public Vector2 contentOffset
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_contentOffset(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_contentOffset(ref value);
      }
    }

    internal Vector2 Internal_clipOffset
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_Internal_clipOffset(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_Internal_clipOffset(ref value);
      }
    }

    /// <summary>
    ///   <para>If non-0, any GUI elements rendered with this style will have the width specified here.</para>
    /// </summary>
    public float fixedWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If non-0, any GUI elements rendered with this style will have the height specified here.</para>
    /// </summary>
    public float fixedHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Can GUI elements of this style be stretched horizontally for better layouting?</para>
    /// </summary>
    public bool stretchWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Can GUI elements of this style be stretched vertically for better layout?</para>
    /// </summary>
    public bool stretchHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font size to use (for dynamic fonts).</para>
    /// </summary>
    public int fontSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font style to use (for dynamic fonts).</para>
    /// </summary>
    public FontStyle fontStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable HTML-style tags for Text Formatting Markup.</para>
    /// </summary>
    public bool richText { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Constructor for empty GUIStyle.</para>
    /// </summary>
    public GUIStyle()
    {
      this.Init();
    }

    /// <summary>
    ///   <para>Constructs GUIStyle identical to given other GUIStyle.</para>
    /// </summary>
    /// <param name="other"></param>
    public GUIStyle(GUIStyle other)
    {
      this.InitCopy(other);
    }

    ~GUIStyle()
    {
      this.Cleanup();
    }

    public static implicit operator GUIStyle(string str)
    {
      if (!((Object) GUISkin.current == (Object) null))
        return GUISkin.current.GetStyle(str);
      Debug.LogError((object) "Unable to use a named GUIStyle without a current skin. Most likely you need to move your GUIStyle initialization code to OnGUI");
      return GUISkin.error;
    }

    internal void InternalOnAfterDeserialize()
    {
      this.m_FontInternal = this.GetFontInternalDuringLoadingThread();
      this.m_Normal = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(0));
      this.m_Hover = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(1));
      this.m_Active = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(2));
      this.m_Focused = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(3));
      this.m_OnNormal = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(4));
      this.m_OnHover = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(5));
      this.m_OnActive = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(6));
      this.m_OnFocused = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, this.GetStyleStatePtr(7));
    }

    private static void Internal_Draw(IntPtr target, Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
    {
      GUIStyle.Internal_Draw(content, ref new Internal_DrawArguments()
      {
        target = target,
        position = position,
        isHover = !isHover ? 0 : 1,
        isActive = !isActive ? 0 : 1,
        on = !on ? 0 : 1,
        hasKeyboardFocus = !hasKeyboardFocus ? 0 : 1
      });
    }

    /// <summary>
    ///   <para>Draw this GUIStyle on to the screen, internal version.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="on"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
    {
      if (Event.current.type != EventType.Repaint)
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event");
      else
        GUIStyle.Internal_Draw(this.m_Ptr, position, GUIContent.none, isHover, isActive, on, hasKeyboardFocus);
    }

    /// <summary>
    ///   <para>Draw the GUIStyle with a text string inside.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="text"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="on"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, string text, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
    {
      if (Event.current.type != EventType.Repaint)
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event");
      else
        GUIStyle.Internal_Draw(this.m_Ptr, position, GUIContent.Temp(text), isHover, isActive, on, hasKeyboardFocus);
    }

    /// <summary>
    ///   <para>Draw the GUIStyle with an image inside. If the image is too large to fit within the content area of the style it is scaled down.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="image"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="on"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, Texture image, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
    {
      if (Event.current.type != EventType.Repaint)
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event");
      else
        GUIStyle.Internal_Draw(this.m_Ptr, position, GUIContent.Temp(image), isHover, isActive, on, hasKeyboardFocus);
    }

    /// <summary>
    ///   <para>Draw the GUIStyle with text and an image inside. If the image is too large to fit within the content area of the style it is scaled down.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="controlID"></param>
    /// <param name="on"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
    {
      if (Event.current.type != EventType.Repaint)
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event");
      else
        GUIStyle.Internal_Draw(this.m_Ptr, position, content, isHover, isActive, on, hasKeyboardFocus);
    }

    /// <summary>
    ///   <para>Draw the GUIStyle with text and an image inside. If the image is too large to fit within the content area of the style it is scaled down.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="controlID"></param>
    /// <param name="on"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, GUIContent content, int controlID)
    {
      this.Draw(position, content, controlID, false);
    }

    /// <summary>
    ///   <para>Draw the GUIStyle with text and an image inside. If the image is too large to fit within the content area of the style it is scaled down.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="controlID"></param>
    /// <param name="on"></param>
    /// <param name="isHover"></param>
    /// <param name="isActive"></param>
    /// <param name="hasKeyboardFocus"></param>
    public void Draw(Rect position, GUIContent content, int controlID, bool on)
    {
      if (Event.current.type != EventType.Repaint)
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event.");
      else if (content != null)
        GUIStyle.Internal_Draw2(this.m_Ptr, position, content, controlID, on);
      else
        Debug.LogError((object) "Style.Draw may not be called with GUIContent that is null.");
    }

    internal void DrawPrefixLabel(Rect position, GUIContent content, int controlID)
    {
      if (content != null)
        GUIStyle.Internal_DrawPrefixLabel(this.m_Ptr, position, content, controlID, false);
      else
        Debug.LogError((object) "Style.DrawPrefixLabel may not be called with GUIContent that is null.");
    }

    /// <summary>
    ///   <para>Draw this GUIStyle with selected content.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="controlID"></param>
    /// <param name="Character"></param>
    public void DrawCursor(Rect position, GUIContent content, int controlID, int Character)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color cursorColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
      float num = (Time.realtimeSinceStartup - GUIStyle.Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
      if ((double) cursorFlashSpeed == 0.0 || (double) num < 0.5)
        cursorColor = GUI.skin.settings.cursorColor;
      GUIStyle.Internal_DrawCursor(this.m_Ptr, position, content, Character, cursorColor);
    }

    internal void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter, bool drawSelectionAsComposition)
    {
      if (Event.current.type != EventType.Repaint)
      {
        Debug.LogError((object) "Style.Draw may not be called if it is not a repaint event");
      }
      else
      {
        Event current = Event.current;
        Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
        float num = (Time.realtimeSinceStartup - GUIStyle.Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
        if ((double) cursorFlashSpeed == 0.0 || (double) num < 0.5)
          color = GUI.skin.settings.cursorColor;
        GUIStyle.Internal_DrawWithTextSelection(content, ref new Internal_DrawWithTextSelectionArguments()
        {
          target = this.m_Ptr,
          position = position,
          firstPos = firstSelectedCharacter,
          lastPos = lastSelectedCharacter,
          cursorColor = color,
          selectionColor = GUI.skin.settings.selectionColor,
          isHover = !position.Contains(current.mousePosition) ? 0 : 1,
          isActive = controlID != GUIUtility.hotControl ? 0 : 1,
          on = 0,
          hasKeyboardFocus = controlID != GUIUtility.keyboardControl || !GUIStyle.showKeyboardFocus ? 0 : 1,
          drawSelectionAsComposition = !drawSelectionAsComposition ? 0 : 1
        });
      }
    }

    /// <summary>
    ///   <para>Draw this GUIStyle with selected content.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="controlID"></param>
    /// <param name="firstSelectedCharacter"></param>
    /// <param name="lastSelectedCharacter"></param>
    public void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter)
    {
      this.DrawWithTextSelection(position, content, controlID, firstSelectedCharacter, lastSelectedCharacter, false);
    }

    /// <summary>
    ///   <para>Get the pixel position of a given string index.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="cursorStringIndex"></param>
    public Vector2 GetCursorPixelPosition(Rect position, GUIContent content, int cursorStringIndex)
    {
      Vector2 ret;
      GUIStyle.Internal_GetCursorPixelPosition(this.m_Ptr, position, content, cursorStringIndex, out ret);
      return ret;
    }

    /// <summary>
    ///   <para>Get the cursor position (indexing into contents.text) when the user clicked at cursorPixelPosition.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="cursorPixelPosition"></param>
    public int GetCursorStringIndex(Rect position, GUIContent content, Vector2 cursorPixelPosition)
    {
      return GUIStyle.Internal_GetCursorStringIndex(this.m_Ptr, position, content, cursorPixelPosition);
    }

    internal int GetNumCharactersThatFitWithinWidth(string text, float width)
    {
      return GUIStyle.Internal_GetNumCharactersThatFitWithinWidth(this.m_Ptr, text, width);
    }

    /// <summary>
    ///   <para>Calculate the size of a some content if it is rendered with this style.</para>
    /// </summary>
    /// <param name="content"></param>
    public Vector2 CalcSize(GUIContent content)
    {
      Vector2 ret;
      GUIStyle.Internal_CalcSize(this.m_Ptr, content, out ret);
      return ret;
    }

    internal Vector2 CalcSizeWithConstraints(GUIContent content, Vector2 constraints)
    {
      Vector2 ret;
      GUIStyle.Internal_CalcSizeWithConstraints(this.m_Ptr, content, constraints, out ret);
      return ret;
    }

    /// <summary>
    ///   <para>Calculate the size of an element formatted with this style, and a given space to content.</para>
    /// </summary>
    /// <param name="contentSize"></param>
    public Vector2 CalcScreenSize(Vector2 contentSize)
    {
      return new Vector2((double) this.fixedWidth == 0.0 ? Mathf.Ceil(contentSize.x + (float) this.padding.left + (float) this.padding.right) : this.fixedWidth, (double) this.fixedHeight == 0.0 ? Mathf.Ceil(contentSize.y + (float) this.padding.top + (float) this.padding.bottom) : this.fixedHeight);
    }

    /// <summary>
    ///   <para>How tall this element will be when rendered with content and a specific width.</para>
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    public float CalcHeight(GUIContent content, float width)
    {
      return GUIStyle.Internal_CalcHeight(this.m_Ptr, content, width);
    }

    public void CalcMinMaxWidth(GUIContent content, out float minWidth, out float maxWidth)
    {
      GUIStyle.Internal_CalcMinMaxWidth(this.m_Ptr, content, out minWidth, out maxWidth);
    }

    public override string ToString()
    {
      return UnityString.Format("GUIStyle '{0}'", (object) this.name);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InitCopy(GUIStyle other);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private IntPtr GetStyleStatePtr(int idx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AssignStyleState(int idx, IntPtr srcStyleState);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private IntPtr GetRectOffsetPtr(int idx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AssignRectOffset(int idx, IntPtr srcRectOffset);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_contentOffset(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_contentOffset(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_Internal_clipOffset(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_Internal_clipOffset(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float Internal_GetLineHeight(IntPtr target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetFontInternal(Font value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Font GetFontInternalDuringLoadingThread();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Font GetFontInternal();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Draw(GUIContent content, ref Internal_DrawArguments arguments);

    private static void Internal_Draw2(IntPtr style, Rect position, GUIContent content, int controlID, bool on)
    {
      GUIStyle.INTERNAL_CALL_Internal_Draw2(style, ref position, content, controlID, on);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_Draw2(IntPtr style, ref Rect position, GUIContent content, int controlID, bool on);

    private static void Internal_DrawPrefixLabel(IntPtr style, Rect position, GUIContent content, int controlID, bool on)
    {
      GUIStyle.INTERNAL_CALL_Internal_DrawPrefixLabel(style, ref position, content, controlID, on);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawPrefixLabel(IntPtr style, ref Rect position, GUIContent content, int controlID, bool on);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float Internal_GetCursorFlashOffset();

    private static void Internal_DrawCursor(IntPtr target, Rect position, GUIContent content, int pos, Color cursorColor)
    {
      GUIStyle.INTERNAL_CALL_Internal_DrawCursor(target, ref position, content, pos, ref cursorColor);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawCursor(IntPtr target, ref Rect position, GUIContent content, int pos, ref Color cursorColor);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawWithTextSelection(GUIContent content, ref Internal_DrawWithTextSelectionArguments arguments);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetDefaultFont(Font font);

    internal static void Internal_GetCursorPixelPosition(IntPtr target, Rect position, GUIContent content, int cursorStringIndex, out Vector2 ret)
    {
      GUIStyle.INTERNAL_CALL_Internal_GetCursorPixelPosition(target, ref position, content, cursorStringIndex, out ret);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_GetCursorPixelPosition(IntPtr target, ref Rect position, GUIContent content, int cursorStringIndex, out Vector2 ret);

    internal static int Internal_GetCursorStringIndex(IntPtr target, Rect position, GUIContent content, Vector2 cursorPixelPosition)
    {
      return GUIStyle.INTERNAL_CALL_Internal_GetCursorStringIndex(target, ref position, content, ref cursorPixelPosition);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_Internal_GetCursorStringIndex(IntPtr target, ref Rect position, GUIContent content, ref Vector2 cursorPixelPosition);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int Internal_GetNumCharactersThatFitWithinWidth(IntPtr target, string text, float width);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_CalcSize(IntPtr target, GUIContent content, out Vector2 ret);

    internal static void Internal_CalcSizeWithConstraints(IntPtr target, GUIContent content, Vector2 maxSize, out Vector2 ret)
    {
      GUIStyle.INTERNAL_CALL_Internal_CalcSizeWithConstraints(target, content, ref maxSize, out ret);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_CalcSizeWithConstraints(IntPtr target, GUIContent content, ref Vector2 maxSize, out Vector2 ret);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float Internal_CalcHeight(IntPtr target, GUIContent content, float width);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CalcMinMaxWidth(IntPtr target, GUIContent content, out float minWidth, out float maxWidth);
  }
}
