// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIStyleState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Specialized values for the given states used by GUIStyle objects.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class GUIStyleState
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    private readonly GUIStyle m_SourceStyle;
    [NonSerialized]
    private Texture2D m_Background;

    /// <summary>
    ///   <para>The background image used by GUI elements in this given state.</para>
    /// </summary>
    public Texture2D background
    {
      get
      {
        return this.GetBackgroundInternal();
      }
      set
      {
        this.SetBackgroundInternal(value);
        this.m_Background = value;
      }
    }

    /// <summary>
    ///   <para>The text color used by GUI elements in this state.</para>
    /// </summary>
    public Color textColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_textColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_textColor(ref value);
      }
    }

    public GUIStyleState()
    {
      this.Init();
    }

    private GUIStyleState(GUIStyle sourceStyle, IntPtr source)
    {
      this.m_SourceStyle = sourceStyle;
      this.m_Ptr = source;
    }

    ~GUIStyleState()
    {
      if (this.m_SourceStyle != null)
        return;
      this.Cleanup();
    }

    internal static GUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
    {
      GUIStyleState guiStyleState = new GUIStyleState(sourceStyle, source);
      guiStyleState.m_Background = guiStyleState.GetBackgroundInternalFromDeserialization();
      return guiStyleState;
    }

    internal static GUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
    {
      GUIStyleState guiStyleState = new GUIStyleState(sourceStyle, source);
      guiStyleState.m_Background = guiStyleState.GetBackgroundInternal();
      return guiStyleState;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetBackgroundInternal(Texture2D value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Texture2D GetBackgroundInternalFromDeserialization();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Texture2D GetBackgroundInternal();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_textColor(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_textColor(ref Color value);
  }
}
