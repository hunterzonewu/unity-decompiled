// Decompiled with JetBrains decompiler
// Type: UnityEngine.WWWTranscoder
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.IO;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
  internal sealed class WWWTranscoder
  {
    private static byte[] ucHexChars = WWW.DefaultEncoding.GetBytes("0123456789ABCDEF");
    private static byte[] lcHexChars = WWW.DefaultEncoding.GetBytes("0123456789abcdef");
    private static byte urlEscapeChar = 37;
    private static byte urlSpace = 43;
    private static byte[] urlForbidden = WWW.DefaultEncoding.GetBytes("@&;:<>=?\"'/\\!#%+$,{}|^[]`");
    private static byte qpEscapeChar = 61;
    private static byte qpSpace = 95;
    private static byte[] qpForbidden = WWW.DefaultEncoding.GetBytes("&;=?\"'%+_");

    private static byte Hex2Byte(byte[] b, int offset)
    {
      byte num1 = 0;
      for (int index = offset; index < offset + 2; ++index)
      {
        byte num2 = (byte) ((uint) num1 * 16U);
        int num3 = (int) b[index];
        if (num3 >= 48 && num3 <= 57)
          num3 -= 48;
        else if (num3 >= 65 && num3 <= 75)
          num3 -= 55;
        else if (num3 >= 97 && num3 <= 102)
          num3 -= 87;
        if (num3 > 15)
          return 63;
        num1 = (byte) ((uint) num2 + (uint) (byte) num3);
      }
      return num1;
    }

    private static byte[] Byte2Hex(byte b, byte[] hexChars)
    {
      return new byte[2]{ hexChars[(int) b >> 4], hexChars[(int) b & 15] };
    }

    [ExcludeFromDocs]
    public static string URLEncode(string toEncode)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWWTranscoder.URLEncode(toEncode, utF8);
    }

    public static string URLEncode(string toEncode, [DefaultValue("Encoding.UTF8")] Encoding e)
    {
      byte[] bytes = WWWTranscoder.Encode(e.GetBytes(toEncode), WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace, WWWTranscoder.urlForbidden, false);
      return WWW.DefaultEncoding.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] URLEncode(byte[] toEncode)
    {
      return WWWTranscoder.Encode(toEncode, WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace, WWWTranscoder.urlForbidden, false);
    }

    [ExcludeFromDocs]
    public static string QPEncode(string toEncode)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWWTranscoder.QPEncode(toEncode, utF8);
    }

    public static string QPEncode(string toEncode, [DefaultValue("Encoding.UTF8")] Encoding e)
    {
      byte[] bytes = WWWTranscoder.Encode(e.GetBytes(toEncode), WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace, WWWTranscoder.qpForbidden, true);
      return WWW.DefaultEncoding.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] QPEncode(byte[] toEncode)
    {
      return WWWTranscoder.Encode(toEncode, WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace, WWWTranscoder.qpForbidden, true);
    }

    public static byte[] Encode(byte[] input, byte escapeChar, byte space, byte[] forbidden, bool uppercase)
    {
      using (MemoryStream memoryStream = new MemoryStream(input.Length * 2))
      {
        for (int index = 0; index < input.Length; ++index)
        {
          if ((int) input[index] == 32)
            memoryStream.WriteByte(space);
          else if ((int) input[index] < 32 || (int) input[index] > 126 || WWWTranscoder.ByteArrayContains(forbidden, input[index]))
          {
            memoryStream.WriteByte(escapeChar);
            memoryStream.Write(WWWTranscoder.Byte2Hex(input[index], !uppercase ? WWWTranscoder.lcHexChars : WWWTranscoder.ucHexChars), 0, 2);
          }
          else
            memoryStream.WriteByte(input[index]);
        }
        return memoryStream.ToArray();
      }
    }

    private static bool ByteArrayContains(byte[] array, byte b)
    {
      int length = array.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((int) array[index] == (int) b)
          return true;
      }
      return false;
    }

    [ExcludeFromDocs]
    public static string URLDecode(string toEncode)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWWTranscoder.URLDecode(toEncode, utF8);
    }

    public static string URLDecode(string toEncode, [DefaultValue("Encoding.UTF8")] Encoding e)
    {
      byte[] bytes = WWWTranscoder.Decode(WWW.DefaultEncoding.GetBytes(toEncode), WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace);
      return e.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] URLDecode(byte[] toEncode)
    {
      return WWWTranscoder.Decode(toEncode, WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace);
    }

    [ExcludeFromDocs]
    public static string QPDecode(string toEncode)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWWTranscoder.QPDecode(toEncode, utF8);
    }

    public static string QPDecode(string toEncode, [DefaultValue("Encoding.UTF8")] Encoding e)
    {
      byte[] bytes = WWWTranscoder.Decode(WWW.DefaultEncoding.GetBytes(toEncode), WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace);
      return e.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] QPDecode(byte[] toEncode)
    {
      return WWWTranscoder.Decode(toEncode, WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace);
    }

    public static byte[] Decode(byte[] input, byte escapeChar, byte space)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(input.Length))
      {
        for (int index = 0; index < input.Length; ++index)
        {
          if ((int) input[index] == (int) space)
            memoryStream1.WriteByte((byte) 32);
          else if ((int) input[index] == (int) escapeChar && index + 2 < input.Length)
          {
            int num1 = index + 1;
            MemoryStream memoryStream2 = memoryStream1;
            byte[] b = input;
            int offset = num1;
            int num2 = 1;
            index = offset + num2;
            int num3 = (int) WWWTranscoder.Hex2Byte(b, offset);
            memoryStream2.WriteByte((byte) num3);
          }
          else
            memoryStream1.WriteByte(input[index]);
        }
        return memoryStream1.ToArray();
      }
    }

    [ExcludeFromDocs]
    public static bool SevenBitClean(string s)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWWTranscoder.SevenBitClean(s, utF8);
    }

    public static bool SevenBitClean(string s, [DefaultValue("Encoding.UTF8")] Encoding e)
    {
      return WWWTranscoder.SevenBitClean(e.GetBytes(s));
    }

    public static bool SevenBitClean(byte[] input)
    {
      for (int index = 0; index < input.Length; ++index)
      {
        if ((int) input[index] < 32 || (int) input[index] > 126)
          return false;
      }
      return true;
    }
  }
}
