// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkHash128
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A 128 bit number used to represent assets in a networking context.</para>
  /// </summary>
  [Serializable]
  public struct NetworkHash128
  {
    public byte i0;
    public byte i1;
    public byte i2;
    public byte i3;
    public byte i4;
    public byte i5;
    public byte i6;
    public byte i7;
    public byte i8;
    public byte i9;
    public byte i10;
    public byte i11;
    public byte i12;
    public byte i13;
    public byte i14;
    public byte i15;

    /// <summary>
    ///   <para>Resets the value of a NetworkHash to zero (invalid).</para>
    /// </summary>
    public void Reset()
    {
      this.i0 = (byte) 0;
      this.i1 = (byte) 0;
      this.i2 = (byte) 0;
      this.i3 = (byte) 0;
      this.i4 = (byte) 0;
      this.i5 = (byte) 0;
      this.i6 = (byte) 0;
      this.i7 = (byte) 0;
      this.i8 = (byte) 0;
      this.i9 = (byte) 0;
      this.i10 = (byte) 0;
      this.i11 = (byte) 0;
      this.i12 = (byte) 0;
      this.i13 = (byte) 0;
      this.i14 = (byte) 0;
      this.i15 = (byte) 0;
    }

    /// <summary>
    ///   <para>A valid NetworkHash has a non-zero value.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if the value is non-zero.</para>
    /// </returns>
    public bool IsValid()
    {
      return ((int) this.i0 | (int) this.i1 | (int) this.i2 | (int) this.i3 | (int) this.i4 | (int) this.i5 | (int) this.i6 | (int) this.i7 | (int) this.i8 | (int) this.i9 | (int) this.i10 | (int) this.i11 | (int) this.i12 | (int) this.i13 | (int) this.i14 | (int) this.i15) != 0;
    }

    private static int HexToNumber(char c)
    {
      if ((int) c >= 48 && (int) c <= 57)
        return (int) c - 48;
      if ((int) c >= 97 && (int) c <= 102)
        return (int) c - 97 + 10;
      if ((int) c >= 65 && (int) c <= 70)
        return (int) c - 65 + 10;
      return 0;
    }

    /// <summary>
    ///   <para>This parses the string representation of a NetworkHash into a binary object.</para>
    /// </summary>
    /// <param name="text">A hex string to parse.</param>
    /// <returns>
    ///   <para>A 128 bit network hash object.</para>
    /// </returns>
    public static NetworkHash128 Parse(string text)
    {
      int length = text.Length;
      if (length < 32)
      {
        string empty = string.Empty;
        for (int index = 0; index < 32 - length; ++index)
          empty += "0";
        text = empty + text;
      }
      NetworkHash128 networkHash128;
      networkHash128.i0 = (byte) (NetworkHash128.HexToNumber(text[0]) * 16 + NetworkHash128.HexToNumber(text[1]));
      networkHash128.i1 = (byte) (NetworkHash128.HexToNumber(text[2]) * 16 + NetworkHash128.HexToNumber(text[3]));
      networkHash128.i2 = (byte) (NetworkHash128.HexToNumber(text[4]) * 16 + NetworkHash128.HexToNumber(text[5]));
      networkHash128.i3 = (byte) (NetworkHash128.HexToNumber(text[6]) * 16 + NetworkHash128.HexToNumber(text[7]));
      networkHash128.i4 = (byte) (NetworkHash128.HexToNumber(text[8]) * 16 + NetworkHash128.HexToNumber(text[9]));
      networkHash128.i5 = (byte) (NetworkHash128.HexToNumber(text[10]) * 16 + NetworkHash128.HexToNumber(text[11]));
      networkHash128.i6 = (byte) (NetworkHash128.HexToNumber(text[12]) * 16 + NetworkHash128.HexToNumber(text[13]));
      networkHash128.i7 = (byte) (NetworkHash128.HexToNumber(text[14]) * 16 + NetworkHash128.HexToNumber(text[15]));
      networkHash128.i8 = (byte) (NetworkHash128.HexToNumber(text[16]) * 16 + NetworkHash128.HexToNumber(text[17]));
      networkHash128.i9 = (byte) (NetworkHash128.HexToNumber(text[18]) * 16 + NetworkHash128.HexToNumber(text[19]));
      networkHash128.i10 = (byte) (NetworkHash128.HexToNumber(text[20]) * 16 + NetworkHash128.HexToNumber(text[21]));
      networkHash128.i11 = (byte) (NetworkHash128.HexToNumber(text[22]) * 16 + NetworkHash128.HexToNumber(text[23]));
      networkHash128.i12 = (byte) (NetworkHash128.HexToNumber(text[24]) * 16 + NetworkHash128.HexToNumber(text[25]));
      networkHash128.i13 = (byte) (NetworkHash128.HexToNumber(text[26]) * 16 + NetworkHash128.HexToNumber(text[27]));
      networkHash128.i14 = (byte) (NetworkHash128.HexToNumber(text[28]) * 16 + NetworkHash128.HexToNumber(text[29]));
      networkHash128.i15 = (byte) (NetworkHash128.HexToNumber(text[30]) * 16 + NetworkHash128.HexToNumber(text[31]));
      return networkHash128;
    }

    /// <summary>
    ///   <para>Returns a string representation of a NetworkHash object.</para>
    /// </summary>
    /// <returns>
    ///   <para>A hex asset string.</para>
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}{8:x2}{9:x2}{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}", (object) this.i0, (object) this.i1, (object) this.i2, (object) this.i3, (object) this.i4, (object) this.i5, (object) this.i6, (object) this.i7, (object) this.i8, (object) this.i9, (object) this.i10, (object) this.i11, (object) this.i12, (object) this.i13, (object) this.i14, (object) this.i15);
    }
  }
}
