// Decompiled with JetBrains decompiler
// Type: UnityEngine.Color32
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Representation of RGBA colors in 32 bit format.</para>
  /// </summary>
  [IL2CPPStructAlignment(Align = 4)]
  [UsedByNativeCode]
  public struct Color32
  {
    /// <summary>
    ///   <para>Red component of the color.</para>
    /// </summary>
    public byte r;
    /// <summary>
    ///   <para>Green component of the color.</para>
    /// </summary>
    public byte g;
    /// <summary>
    ///   <para>Blue component of the color.</para>
    /// </summary>
    public byte b;
    /// <summary>
    ///   <para>Alpha component of the color.</para>
    /// </summary>
    public byte a;

    /// <summary>
    ///   <para>Constructs a new Color with given r, g, b, a components.</para>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    public Color32(byte r, byte g, byte b, byte a)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public static implicit operator Color32(Color c)
    {
      return new Color32((byte) ((double) Mathf.Clamp01(c.r) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.g) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.b) * (double) byte.MaxValue), (byte) ((double) Mathf.Clamp01(c.a) * (double) byte.MaxValue));
    }

    public static implicit operator Color(Color32 c)
    {
      return new Color((float) c.r / (float) byte.MaxValue, (float) c.g / (float) byte.MaxValue, (float) c.b / (float) byte.MaxValue, (float) c.a / (float) byte.MaxValue);
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string of this color.</para>
    /// </summary>
    /// <param name="format"></param>
    public override string ToString()
    {
      return UnityString.Format("RGBA({0}, {1}, {2}, {3})", (object) this.r, (object) this.g, (object) this.b, (object) this.a);
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string of this color.</para>
    /// </summary>
    /// <param name="format"></param>
    public string ToString(string format)
    {
      return UnityString.Format("RGBA({0}, {1}, {2}, {3})", (object) this.r.ToString(format), (object) this.g.ToString(format), (object) this.b.ToString(format), (object) this.a.ToString(format));
    }

    /// <summary>
    ///   <para>Linearly interpolates between colors a and b by t.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    public static Color32 Lerp(Color32 a, Color32 b, float t)
    {
      t = Mathf.Clamp01(t);
      return new Color32((byte) ((double) a.r + (double) ((int) b.r - (int) a.r) * (double) t), (byte) ((double) a.g + (double) ((int) b.g - (int) a.g) * (double) t), (byte) ((double) a.b + (double) ((int) b.b - (int) a.b) * (double) t), (byte) ((double) a.a + (double) ((int) b.a - (int) a.a) * (double) t));
    }

    /// <summary>
    ///   <para>Linearly interpolates between colors a and b by t.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
    {
      return new Color32((byte) ((double) a.r + (double) ((int) b.r - (int) a.r) * (double) t), (byte) ((double) a.g + (double) ((int) b.g - (int) a.g) * (double) t), (byte) ((double) a.b + (double) ((int) b.b - (int) a.b) * (double) t), (byte) ((double) a.a + (double) ((int) b.a - (int) a.a) * (double) t));
    }
  }
}
