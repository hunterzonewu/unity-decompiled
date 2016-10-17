// Decompiled with JetBrains decompiler
// Type: SimpleJson.SimpleJson
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using SimpleJson.Reflection;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  internal static class SimpleJson
  {
    private const int TOKEN_NONE = 0;
    private const int TOKEN_CURLY_OPEN = 1;
    private const int TOKEN_CURLY_CLOSE = 2;
    private const int TOKEN_SQUARED_OPEN = 3;
    private const int TOKEN_SQUARED_CLOSE = 4;
    private const int TOKEN_COLON = 5;
    private const int TOKEN_COMMA = 6;
    private const int TOKEN_STRING = 7;
    private const int TOKEN_NUMBER = 8;
    private const int TOKEN_TRUE = 9;
    private const int TOKEN_FALSE = 10;
    private const int TOKEN_NULL = 11;
    private const int BUILDER_CAPACITY = 2000;
    private static IJsonSerializerStrategy _currentJsonSerializerStrategy;
    private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;

    public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
    {
      get
      {
        return SimpleJson.SimpleJson._currentJsonSerializerStrategy ?? (SimpleJson.SimpleJson._currentJsonSerializerStrategy = (IJsonSerializerStrategy) SimpleJson.SimpleJson.PocoJsonSerializerStrategy);
      }
      set
      {
        SimpleJson.SimpleJson._currentJsonSerializerStrategy = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy
    {
      get
      {
        return SimpleJson.SimpleJson._pocoJsonSerializerStrategy ?? (SimpleJson.SimpleJson._pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());
      }
    }

    public static object DeserializeObject(string json)
    {
      object obj;
      if (SimpleJson.SimpleJson.TryDeserializeObject(json, out obj))
        return obj;
      throw new SerializationException("Invalid JSON string");
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    public static bool TryDeserializeObject(string json, out object obj)
    {
      bool success = true;
      if (json != null)
      {
        char[] charArray = json.ToCharArray();
        int index = 0;
        obj = SimpleJson.SimpleJson.ParseValue(charArray, ref index, ref success);
      }
      else
        obj = (object) null;
      return success;
    }

    public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
    {
      object obj = SimpleJson.SimpleJson.DeserializeObject(json);
      if (type == null || obj != null && ReflectionUtils.IsAssignableFrom(obj.GetType(), type))
        return obj;
      return (jsonSerializerStrategy ?? SimpleJson.SimpleJson.CurrentJsonSerializerStrategy).DeserializeObject(obj, type);
    }

    public static object DeserializeObject(string json, Type type)
    {
      return SimpleJson.SimpleJson.DeserializeObject(json, type, (IJsonSerializerStrategy) null);
    }

    public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
    {
      return (T) SimpleJson.SimpleJson.DeserializeObject(json, typeof (T), jsonSerializerStrategy);
    }

    public static T DeserializeObject<T>(string json)
    {
      return (T) SimpleJson.SimpleJson.DeserializeObject(json, typeof (T), (IJsonSerializerStrategy) null);
    }

    public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy)
    {
      StringBuilder builder = new StringBuilder(2000);
      if (SimpleJson.SimpleJson.SerializeValue(jsonSerializerStrategy, json, builder))
        return builder.ToString();
      return (string) null;
    }

    public static string SerializeObject(object json)
    {
      return SimpleJson.SimpleJson.SerializeObject(json, SimpleJson.SimpleJson.CurrentJsonSerializerStrategy);
    }

    public static string EscapeToJavascriptString(string jsonString)
    {
      if (string.IsNullOrEmpty(jsonString))
        return jsonString;
      StringBuilder stringBuilder = new StringBuilder();
      int index = 0;
      while (index < jsonString.Length)
      {
        char ch = jsonString[index++];
        if ((int) ch == 92)
        {
          if (jsonString.Length - index >= 2)
          {
            switch (jsonString[index])
            {
              case '\\':
                stringBuilder.Append('\\');
                ++index;
                continue;
              case '"':
                stringBuilder.Append("\"");
                ++index;
                continue;
              case 't':
                stringBuilder.Append('\t');
                ++index;
                continue;
              case 'b':
                stringBuilder.Append('\b');
                ++index;
                continue;
              case 'n':
                stringBuilder.Append('\n');
                ++index;
                continue;
              case 'r':
                stringBuilder.Append('\r');
                ++index;
                continue;
              default:
                continue;
            }
          }
        }
        else
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    private static IDictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new JsonObject();
      SimpleJson.SimpleJson.NextToken(json, ref index);
      bool flag = false;
      while (!flag)
      {
        switch (SimpleJson.SimpleJson.LookAhead(json, index))
        {
          case 0:
            success = false;
            return (IDictionary<string, object>) null;
          case 6:
            SimpleJson.SimpleJson.NextToken(json, ref index);
            continue;
          case 2:
            SimpleJson.SimpleJson.NextToken(json, ref index);
            return dictionary;
          default:
            string index1 = SimpleJson.SimpleJson.ParseString(json, ref index, ref success);
            if (!success)
            {
              success = false;
              return (IDictionary<string, object>) null;
            }
            if (SimpleJson.SimpleJson.NextToken(json, ref index) != 5)
            {
              success = false;
              return (IDictionary<string, object>) null;
            }
            object obj = SimpleJson.SimpleJson.ParseValue(json, ref index, ref success);
            if (!success)
            {
              success = false;
              return (IDictionary<string, object>) null;
            }
            dictionary[index1] = obj;
            continue;
        }
      }
      return dictionary;
    }

    private static JsonArray ParseArray(char[] json, ref int index, ref bool success)
    {
      JsonArray jsonArray = new JsonArray();
      SimpleJson.SimpleJson.NextToken(json, ref index);
      bool flag = false;
      while (!flag)
      {
        switch (SimpleJson.SimpleJson.LookAhead(json, index))
        {
          case 0:
            success = false;
            return (JsonArray) null;
          case 6:
            SimpleJson.SimpleJson.NextToken(json, ref index);
            continue;
          case 4:
            SimpleJson.SimpleJson.NextToken(json, ref index);
            goto label_9;
          default:
            object obj = SimpleJson.SimpleJson.ParseValue(json, ref index, ref success);
            if (!success)
              return (JsonArray) null;
            jsonArray.Add(obj);
            continue;
        }
      }
label_9:
      return jsonArray;
    }

    private static object ParseValue(char[] json, ref int index, ref bool success)
    {
      switch (SimpleJson.SimpleJson.LookAhead(json, index))
      {
        case 1:
          return (object) SimpleJson.SimpleJson.ParseObject(json, ref index, ref success);
        case 3:
          return (object) SimpleJson.SimpleJson.ParseArray(json, ref index, ref success);
        case 7:
          return (object) SimpleJson.SimpleJson.ParseString(json, ref index, ref success);
        case 8:
          return SimpleJson.SimpleJson.ParseNumber(json, ref index, ref success);
        case 9:
          SimpleJson.SimpleJson.NextToken(json, ref index);
          return (object) true;
        case 10:
          SimpleJson.SimpleJson.NextToken(json, ref index);
          return (object) false;
        case 11:
          SimpleJson.SimpleJson.NextToken(json, ref index);
          return (object) null;
        default:
          success = false;
          return (object) null;
      }
    }

    private static string ParseString(char[] json, ref int index, ref bool success)
    {
      StringBuilder stringBuilder = new StringBuilder(2000);
      SimpleJson.SimpleJson.EatWhitespace(json, ref index);
      char[] chArray1 = json;
      int num1;
      index = (num1 = index) + 1;
      int index1 = num1;
      char ch1 = chArray1[index1];
      bool flag = false;
      while (!flag && index != json.Length)
      {
        char[] chArray2 = json;
        int num2;
        index = (num2 = index) + 1;
        int index2 = num2;
        char ch2 = chArray2[index2];
        switch (ch2)
        {
          case '"':
            flag = true;
            goto label_23;
          case '\\':
            if (index != json.Length)
            {
              char[] chArray3 = json;
              int num3;
              index = (num3 = index) + 1;
              int index3 = num3;
              switch (chArray3[index3])
              {
                case '"':
                  stringBuilder.Append('"');
                  continue;
                case '\\':
                  stringBuilder.Append('\\');
                  continue;
                case '/':
                  stringBuilder.Append('/');
                  continue;
                case 'b':
                  stringBuilder.Append('\b');
                  continue;
                case 'f':
                  stringBuilder.Append('\f');
                  continue;
                case 'n':
                  stringBuilder.Append('\n');
                  continue;
                case 'r':
                  stringBuilder.Append('\r');
                  continue;
                case 't':
                  stringBuilder.Append('\t');
                  continue;
                case 'u':
                  if (json.Length - index >= 4)
                  {
                    uint result1;
                    // ISSUE: explicit reference operation
                    // ISSUE: cast to a reference type
                    // ISSUE: explicit reference operation
                    if ((int) (^(sbyte&) @success = (sbyte) uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result1)) == 0)
                      return string.Empty;
                    if (55296U <= result1 && result1 <= 56319U)
                    {
                      index = index + 4;
                      uint result2;
                      if (json.Length - index >= 6 && new string(json, index, 2) == "\\u" && (uint.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result2) && 56320U <= result2) && result2 <= 57343U)
                      {
                        stringBuilder.Append((char) result1);
                        stringBuilder.Append((char) result2);
                        index = index + 6;
                        continue;
                      }
                      success = false;
                      return string.Empty;
                    }
                    stringBuilder.Append(SimpleJson.SimpleJson.ConvertFromUtf32((int) result1));
                    index = index + 4;
                    continue;
                  }
                  goto label_23;
                default:
                  continue;
              }
            }
            else
              goto label_23;
          default:
            stringBuilder.Append(ch2);
            continue;
        }
      }
label_23:
      if (flag)
        return stringBuilder.ToString();
      success = false;
      return (string) null;
    }

    private static string ConvertFromUtf32(int utf32)
    {
      if (utf32 < 0 || utf32 > 1114111)
        throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
      if (55296 <= utf32 && utf32 <= 57343)
        throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
      if (utf32 < 65536)
        return new string((char) utf32, 1);
      utf32 -= 65536;
      return new string(new char[2]{ (char) ((utf32 >> 10) + 55296), (char) (utf32 % 1024 + 56320) });
    }

    private static object ParseNumber(char[] json, ref int index, ref bool success)
    {
      SimpleJson.SimpleJson.EatWhitespace(json, ref index);
      int lastIndexOfNumber = SimpleJson.SimpleJson.GetLastIndexOfNumber(json, index);
      int length = lastIndexOfNumber - index + 1;
      string str = new string(json, index, length);
      object obj;
      if (str.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || str.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
      {
        double result;
        success = double.TryParse(new string(json, index, length), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result);
        obj = (object) result;
      }
      else
      {
        long result;
        success = long.TryParse(new string(json, index, length), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result);
        obj = (object) result;
      }
      index = lastIndexOfNumber + 1;
      return obj;
    }

    private static int GetLastIndexOfNumber(char[] json, int index)
    {
      int index1 = index;
      while (index1 < json.Length && "0123456789+-.eE".IndexOf(json[index1]) != -1)
        ++index1;
      return index1 - 1;
    }

    private static void EatWhitespace(char[] json, ref int index)
    {
      while (index < json.Length && " \t\n\r\b\f".IndexOf(json[index]) != -1)
        index = index + 1;
    }

    private static int LookAhead(char[] json, int index)
    {
      int index1 = index;
      return SimpleJson.SimpleJson.NextToken(json, ref index1);
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    private static int NextToken(char[] json, ref int index)
    {
      SimpleJson.SimpleJson.EatWhitespace(json, ref index);
      if (index == json.Length)
        return 0;
      char ch1 = json[index];
      index = index + 1;
      char ch2 = ch1;
      switch (ch2)
      {
        case '"':
          return 7;
        case ',':
          return 6;
        case '-':
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
          return 8;
        case ':':
          return 5;
        default:
          switch (ch2)
          {
            case '[':
              return 3;
            case ']':
              return 4;
            default:
              switch (ch2)
              {
                case '{':
                  return 1;
                case '}':
                  return 2;
                default:
                  index = index - 1;
                  int num = json.Length - index;
                  if (num >= 5 && (int) json[index] == 102 && ((int) json[index + 1] == 97 && (int) json[index + 2] == 108) && ((int) json[index + 3] == 115 && (int) json[index + 4] == 101))
                  {
                    index = index + 5;
                    return 10;
                  }
                  if (num >= 4 && (int) json[index] == 116 && ((int) json[index + 1] == 114 && (int) json[index + 2] == 117) && (int) json[index + 3] == 101)
                  {
                    index = index + 4;
                    return 9;
                  }
                  if (num < 4 || (int) json[index] != 110 || ((int) json[index + 1] != 117 || (int) json[index + 2] != 108) || (int) json[index + 3] != 108)
                    return 0;
                  index = index + 4;
                  return 11;
              }
          }
      }
    }

    private static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
    {
      bool flag = true;
      string aString = value as string;
      if (aString != null)
      {
        flag = SimpleJson.SimpleJson.SerializeString(aString, builder);
      }
      else
      {
        IDictionary<string, object> dictionary1 = value as IDictionary<string, object>;
        if (dictionary1 != null)
        {
          flag = SimpleJson.SimpleJson.SerializeObject(jsonSerializerStrategy, (IEnumerable) dictionary1.Keys, (IEnumerable) dictionary1.Values, builder);
        }
        else
        {
          IDictionary<string, string> dictionary2 = value as IDictionary<string, string>;
          if (dictionary2 != null)
          {
            flag = SimpleJson.SimpleJson.SerializeObject(jsonSerializerStrategy, (IEnumerable) dictionary2.Keys, (IEnumerable) dictionary2.Values, builder);
          }
          else
          {
            IEnumerable anArray = value as IEnumerable;
            if (anArray != null)
              flag = SimpleJson.SimpleJson.SerializeArray(jsonSerializerStrategy, anArray, builder);
            else if (SimpleJson.SimpleJson.IsNumeric(value))
              flag = SimpleJson.SimpleJson.SerializeNumber(value, builder);
            else if (value is bool)
              builder.Append(!(bool) value ? "false" : "true");
            else if (value == null)
            {
              builder.Append("null");
            }
            else
            {
              object output;
              flag = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out output);
              if (flag)
                SimpleJson.SimpleJson.SerializeValue(jsonSerializerStrategy, output, builder);
            }
          }
        }
      }
      return flag;
    }

    private static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
    {
      builder.Append("{");
      IEnumerator enumerator1 = keys.GetEnumerator();
      IEnumerator enumerator2 = values.GetEnumerator();
      bool flag = true;
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
      {
        object current1 = enumerator1.Current;
        object current2 = enumerator2.Current;
        if (!flag)
          builder.Append(",");
        string aString = current1 as string;
        if (aString != null)
          SimpleJson.SimpleJson.SerializeString(aString, builder);
        else if (!SimpleJson.SimpleJson.SerializeValue(jsonSerializerStrategy, current2, builder))
          return false;
        builder.Append(":");
        if (!SimpleJson.SimpleJson.SerializeValue(jsonSerializerStrategy, current2, builder))
          return false;
        flag = false;
      }
      builder.Append("}");
      return true;
    }

    private static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
    {
      builder.Append("[");
      bool flag = true;
      foreach (object an in anArray)
      {
        if (!flag)
          builder.Append(",");
        if (!SimpleJson.SimpleJson.SerializeValue(jsonSerializerStrategy, an, builder))
          return false;
        flag = false;
      }
      builder.Append("]");
      return true;
    }

    private static bool SerializeString(string aString, StringBuilder builder)
    {
      builder.Append("\"");
      foreach (char ch in aString.ToCharArray())
      {
        switch (ch)
        {
          case '"':
            builder.Append("\\\"");
            break;
          case '\\':
            builder.Append("\\\\");
            break;
          case '\b':
            builder.Append("\\b");
            break;
          case '\f':
            builder.Append("\\f");
            break;
          case '\n':
            builder.Append("\\n");
            break;
          case '\r':
            builder.Append("\\r");
            break;
          case '\t':
            builder.Append("\\t");
            break;
          default:
            builder.Append(ch);
            break;
        }
      }
      builder.Append("\"");
      return true;
    }

    private static bool SerializeNumber(object number, StringBuilder builder)
    {
      if (number is long)
        builder.Append(((long) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else if (number is ulong)
        builder.Append(((ulong) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else if (number is int)
        builder.Append(((int) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else if (number is uint)
        builder.Append(((uint) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else if (number is Decimal)
        builder.Append(((Decimal) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else if (number is float)
        builder.Append(((float) number).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      else
        builder.Append(Convert.ToDouble(number, (IFormatProvider) CultureInfo.InvariantCulture).ToString("r", (IFormatProvider) CultureInfo.InvariantCulture));
      return true;
    }

    private static bool IsNumeric(object value)
    {
      return value is sbyte || value is byte || (value is short || value is ushort) || (value is int || value is uint || (value is long || value is ulong)) || (value is float || value is double || value is Decimal);
    }
  }
}
