// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextGenerator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class that can be used to generate text for rendering.</para>
  /// </summary>
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class TextGenerator : IDisposable
  {
    private static int s_NextId = 0;
    private static readonly Dictionary<int, WeakReference> s_Instances = new Dictionary<int, WeakReference>();
    internal IntPtr m_Ptr;
    private string m_LastString;
    private TextGenerationSettings m_LastSettings;
    private bool m_HasGenerated;
    private bool m_LastValid;
    private readonly List<UIVertex> m_Verts;
    private readonly List<UICharInfo> m_Characters;
    private readonly List<UILineInfo> m_Lines;
    private bool m_CachedVerts;
    private bool m_CachedCharacters;
    private bool m_CachedLines;
    private int m_Id;

    /// <summary>
    ///   <para>Array of generated vertices.</para>
    /// </summary>
    public IList<UIVertex> verts
    {
      get
      {
        if (!this.m_CachedVerts)
        {
          this.GetVertices(this.m_Verts);
          this.m_CachedVerts = true;
        }
        return (IList<UIVertex>) this.m_Verts;
      }
    }

    /// <summary>
    ///   <para>Array of generated characters.</para>
    /// </summary>
    public IList<UICharInfo> characters
    {
      get
      {
        if (!this.m_CachedCharacters)
        {
          this.GetCharacters(this.m_Characters);
          this.m_CachedCharacters = true;
        }
        return (IList<UICharInfo>) this.m_Characters;
      }
    }

    /// <summary>
    ///   <para>Information about each generated text line.</para>
    /// </summary>
    public IList<UILineInfo> lines
    {
      get
      {
        if (!this.m_CachedLines)
        {
          this.GetLines(this.m_Lines);
          this.m_CachedLines = true;
        }
        return (IList<UILineInfo>) this.m_Lines;
      }
    }

    /// <summary>
    ///   <para>Extents of the generated text in rect format.</para>
    /// </summary>
    public Rect rectExtents
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_rectExtents(out rect);
        return rect;
      }
    }

    /// <summary>
    ///   <para>Number of vertices generated.</para>
    /// </summary>
    public int vertexCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of characters that have been generated.</para>
    /// </summary>
    public int characterCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of characters that have been generated and are included in the visible lines.</para>
    /// </summary>
    public int characterCountVisible
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_LastString))
          return 0;
        return Mathf.Min(this.m_LastString.Length, Mathf.Max(0, (this.vertexCount - 4) / 4));
      }
    }

    /// <summary>
    ///   <para>Number of text lines generated.</para>
    /// </summary>
    public int lineCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The size of the font that was found if using best fit mode.</para>
    /// </summary>
    public int fontSizeUsedForBestFit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Create a TextGenerator.</para>
    /// </summary>
    /// <param name="initialCapacity"></param>
    public TextGenerator()
      : this(50)
    {
    }

    /// <summary>
    ///   <para>Create a TextGenerator.</para>
    /// </summary>
    /// <param name="initialCapacity"></param>
    public TextGenerator(int initialCapacity)
    {
      this.m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
      this.m_Characters = new List<UICharInfo>(initialCapacity + 1);
      this.m_Lines = new List<UILineInfo>(20);
      this.Init();
      lock (TextGenerator.s_Instances)
      {
        this.m_Id = TextGenerator.s_NextId++;
        TextGenerator.s_Instances.Add(this.m_Id, new WeakReference((object) this));
      }
    }

    ~TextGenerator()
    {
      ((IDisposable) this).Dispose();
    }

    void IDisposable.Dispose()
    {
      lock (TextGenerator.s_Instances)
        TextGenerator.s_Instances.Remove(this.m_Id);
      this.Dispose_cpp();
    }

    [RequiredByNativeCode]
    internal static void InvalidateAll()
    {
      lock (TextGenerator.s_Instances)
      {
        using (Dictionary<int, WeakReference>.Enumerator resource_0 = TextGenerator.s_Instances.GetEnumerator())
        {
          while (resource_0.MoveNext())
          {
            WeakReference local_3 = resource_0.Current.Value;
            if (local_3.IsAlive)
              (local_3.Target as TextGenerator).Invalidate();
          }
        }
      }
    }

    private TextGenerationSettings ValidatedSettings(TextGenerationSettings settings)
    {
      if ((Object) settings.font != (Object) null && settings.font.dynamic)
        return settings;
      if (settings.fontSize != 0 || settings.fontStyle != FontStyle.Normal)
      {
        Debug.LogWarning((object) "Font size and style overrides are only supported for dynamic fonts.");
        settings.fontSize = 0;
        settings.fontStyle = FontStyle.Normal;
      }
      if (settings.resizeTextForBestFit)
      {
        Debug.LogWarning((object) "BestFit is only supported for dynamic fonts.");
        settings.resizeTextForBestFit = false;
      }
      return settings;
    }

    /// <summary>
    ///   <para>Mark the text generator as invalid. This will force a full text generation the next time Populate is called.</para>
    /// </summary>
    public void Invalidate()
    {
      this.m_HasGenerated = false;
    }

    public void GetCharacters(List<UICharInfo> characters)
    {
      this.GetCharactersInternal((object) characters);
    }

    public void GetLines(List<UILineInfo> lines)
    {
      this.GetLinesInternal((object) lines);
    }

    public void GetVertices(List<UIVertex> vertices)
    {
      this.GetVerticesInternal((object) vertices);
    }

    /// <summary>
    ///   <para>Given a string and settings, returns the preferred width for a container that would hold this text.</para>
    /// </summary>
    /// <param name="str">Generation text.</param>
    /// <param name="settings">Settings for generation.</param>
    /// <returns>
    ///   <para>Preferred width.</para>
    /// </returns>
    public float GetPreferredWidth(string str, TextGenerationSettings settings)
    {
      settings.horizontalOverflow = HorizontalWrapMode.Overflow;
      settings.verticalOverflow = VerticalWrapMode.Overflow;
      settings.updateBounds = true;
      this.Populate(str, settings);
      return this.rectExtents.width;
    }

    /// <summary>
    ///   <para>Given a string and settings, returns the preferred height for a container that would hold this text.</para>
    /// </summary>
    /// <param name="str">Generation text.</param>
    /// <param name="settings">Settings for generation.</param>
    /// <returns>
    ///   <para>Preferred height.</para>
    /// </returns>
    public float GetPreferredHeight(string str, TextGenerationSettings settings)
    {
      settings.verticalOverflow = VerticalWrapMode.Overflow;
      settings.updateBounds = true;
      this.Populate(str, settings);
      return this.rectExtents.height;
    }

    /// <summary>
    ///   <para>Will generate the vertices and other data for the given string with the given settings.</para>
    /// </summary>
    /// <param name="str">String to generate.</param>
    /// <param name="settings">Settings.</param>
    public bool Populate(string str, TextGenerationSettings settings)
    {
      if (this.m_HasGenerated && str == this.m_LastString && settings.Equals(this.m_LastSettings))
        return this.m_LastValid;
      return this.PopulateAlways(str, settings);
    }

    private bool PopulateAlways(string str, TextGenerationSettings settings)
    {
      this.m_LastString = str;
      this.m_HasGenerated = true;
      this.m_CachedVerts = false;
      this.m_CachedCharacters = false;
      this.m_CachedLines = false;
      this.m_LastSettings = settings;
      TextGenerationSettings generationSettings = this.ValidatedSettings(settings);
      this.m_LastValid = this.Populate_Internal(str, generationSettings.font, generationSettings.color, generationSettings.fontSize, generationSettings.scaleFactor, generationSettings.lineSpacing, generationSettings.fontStyle, generationSettings.richText, generationSettings.resizeTextForBestFit, generationSettings.resizeTextMinSize, generationSettings.resizeTextMaxSize, generationSettings.verticalOverflow, generationSettings.horizontalOverflow, generationSettings.updateBounds, generationSettings.textAnchor, generationSettings.generationExtents, generationSettings.pivot, generationSettings.generateOutOfBounds, generationSettings.alignByGeometry);
      return this.m_LastValid;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Dispose_cpp();

    internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds, TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds, bool alignByGeometry)
    {
      return this.Populate_Internal_cpp(str, font, color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, (int) verticalOverFlow, (int) horizontalOverflow, updateBounds, anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds, alignByGeometry);
    }

    internal bool Populate_Internal_cpp(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry)
    {
      return TextGenerator.INTERNAL_CALL_Populate_Internal_cpp(this, str, font, ref color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, verticalOverFlow, horizontalOverflow, updateBounds, anchor, extentsX, extentsY, pivotX, pivotY, generateOutOfBounds, alignByGeometry);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Populate_Internal_cpp(TextGenerator self, string str, Font font, ref Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rectExtents(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetVerticesInternal(object vertices);

    /// <summary>
    ///   <para>Returns the current UILineInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Vertices.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public UIVertex[] GetVerticesArray();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetCharactersInternal(object characters);

    /// <summary>
    ///   <para>Returns the current UICharInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Character information.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public UICharInfo[] GetCharactersArray();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetLinesInternal(object lines);

    /// <summary>
    ///   <para>Returns the current UILineInfo.</para>
    /// </summary>
    /// <returns>
    ///   <para>Line information.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public UILineInfo[] GetLinesArray();
  }
}
