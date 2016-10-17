// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExpressionEvaluator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class ExpressionEvaluator
  {
    private static readonly ExpressionEvaluator.Operator[] s_Operators = new ExpressionEvaluator.Operator[7]{ new ExpressionEvaluator.Operator('-', 2, 2, ExpressionEvaluator.Associativity.Left), new ExpressionEvaluator.Operator('+', 2, 2, ExpressionEvaluator.Associativity.Left), new ExpressionEvaluator.Operator('/', 3, 2, ExpressionEvaluator.Associativity.Left), new ExpressionEvaluator.Operator('*', 3, 2, ExpressionEvaluator.Associativity.Left), new ExpressionEvaluator.Operator('%', 3, 2, ExpressionEvaluator.Associativity.Left), new ExpressionEvaluator.Operator('^', 4, 2, ExpressionEvaluator.Associativity.Right), new ExpressionEvaluator.Operator('u', 4, 1, ExpressionEvaluator.Associativity.Left) };

    public static T Evaluate<T>(string expression)
    {
      T result = default (T);
      if (!ExpressionEvaluator.TryParse<T>(expression, out result))
      {
        expression = ExpressionEvaluator.PreFormatExpression(expression);
        result = ExpressionEvaluator.Evaluate<T>(ExpressionEvaluator.InfixToRPN(ExpressionEvaluator.FixUnaryOperators(ExpressionEvaluator.ExpressionToTokens(expression))));
      }
      return result;
    }

    private static T Evaluate<T>(string[] tokens)
    {
      Stack<string> source = new Stack<string>();
      foreach (string token in tokens)
      {
        if (ExpressionEvaluator.IsOperator(token))
        {
          ExpressionEvaluator.Operator @operator = ExpressionEvaluator.CharToOperator(token[0]);
          List<T> objList = new List<T>();
          bool flag = true;
          while (source.LongCount<string>() > 0L && !ExpressionEvaluator.IsCommand(source.Peek()) && objList.Count < @operator.inputs)
          {
            T result;
            flag &= ExpressionEvaluator.TryParse<T>(source.Pop(), out result);
            objList.Add(result);
          }
          objList.Reverse();
          if (!flag || objList.Count != @operator.inputs)
            return default (T);
          source.Push(ExpressionEvaluator.Evaluate<T>(objList.ToArray(), token[0]).ToString());
        }
        else
          source.Push(token);
      }
      T result1;
      if (source.LongCount<string>() == 1L && ExpressionEvaluator.TryParse<T>(source.Pop(), out result1))
        return result1;
      return default (T);
    }

    private static string[] InfixToRPN(string[] tokens)
    {
      Stack<char> charStack = new Stack<char>();
      Stack<string> source = new Stack<string>();
      foreach (string token in tokens)
      {
        if (ExpressionEvaluator.IsCommand(token))
        {
          char character = token[0];
          switch (character)
          {
            case '(':
              charStack.Push(character);
              continue;
            case ')':
              while (charStack.LongCount<char>() > 0L && (int) charStack.Peek() != 40)
                source.Push(charStack.Pop().ToString());
              if (charStack.LongCount<char>() > 0L)
              {
                int num = (int) charStack.Pop();
                continue;
              }
              continue;
            default:
              ExpressionEvaluator.Operator newOperator = ExpressionEvaluator.CharToOperator(character);
              while (ExpressionEvaluator.NeedToPop(charStack, newOperator))
                source.Push(charStack.Pop().ToString());
              charStack.Push(character);
              continue;
          }
        }
        else
          source.Push(token);
      }
      while (charStack.LongCount<char>() > 0L)
        source.Push(charStack.Pop().ToString());
      return source.Reverse<string>().ToArray<string>();
    }

    private static bool NeedToPop(Stack<char> operatorStack, ExpressionEvaluator.Operator newOperator)
    {
      if (operatorStack.LongCount<char>() > 0L)
      {
        ExpressionEvaluator.Operator @operator = ExpressionEvaluator.CharToOperator(operatorStack.Peek());
        if (ExpressionEvaluator.IsOperator(@operator.character) && (newOperator.associativity == ExpressionEvaluator.Associativity.Left && newOperator.presedence <= @operator.presedence || newOperator.associativity == ExpressionEvaluator.Associativity.Right && newOperator.presedence < @operator.presedence))
          return true;
      }
      return false;
    }

    private static string[] ExpressionToTokens(string expression)
    {
      List<string> stringList = new List<string>();
      string empty = string.Empty;
      for (int index = 0; index < expression.Length; ++index)
      {
        char character = expression[index];
        if (ExpressionEvaluator.IsCommand(character))
        {
          if (empty.Length > 0)
            stringList.Add(empty);
          stringList.Add(character.ToString());
          empty = string.Empty;
        }
        else if ((int) character != 32)
          empty += (string) (object) character;
      }
      if (empty.Length > 0)
        stringList.Add(empty);
      return stringList.ToArray();
    }

    private static bool IsCommand(string token)
    {
      if (token.Length != 1)
        return false;
      return ExpressionEvaluator.IsCommand(token[0]);
    }

    private static bool IsCommand(char character)
    {
      if ((int) character == 40 || (int) character == 41)
        return true;
      return ExpressionEvaluator.IsOperator(character);
    }

    private static bool IsOperator(string token)
    {
      if (token.Length != 1)
        return false;
      return ExpressionEvaluator.IsOperator(token[0]);
    }

    private static bool IsOperator(char character)
    {
      foreach (ExpressionEvaluator.Operator @operator in ExpressionEvaluator.s_Operators)
      {
        if ((int) @operator.character == (int) character)
          return true;
      }
      return false;
    }

    private static ExpressionEvaluator.Operator CharToOperator(char character)
    {
      foreach (ExpressionEvaluator.Operator @operator in ExpressionEvaluator.s_Operators)
      {
        if ((int) @operator.character == (int) character)
          return @operator;
      }
      return new ExpressionEvaluator.Operator();
    }

    private static string PreFormatExpression(string expression)
    {
      string str = expression.Trim();
      if (str.Length == 0)
        return str;
      char character = str[str.Length - 1];
      if (ExpressionEvaluator.IsOperator(character))
        str = str.TrimEnd(character);
      return str;
    }

    private static string[] FixUnaryOperators(string[] tokens)
    {
      if (tokens.Length == 0)
        return tokens;
      if (tokens[0] == "-")
        tokens[0] = "u";
      for (int index = 1; index < tokens.Length - 1; ++index)
      {
        string token1 = tokens[index];
        string token2 = tokens[index - 1];
        string token3 = tokens[index - 1];
        if (token1 == "-" && (ExpressionEvaluator.IsCommand(token2) || token3 == "(" || token3 == ")"))
          tokens[index] = "u";
      }
      return tokens;
    }

    private static T Evaluate<T>(T[] values, char oper)
    {
      if (typeof (T) == typeof (float))
      {
        if (values.Length == 1)
        {
          if ((int) oper == 117)
            return (T) (ValueType) (float) ((double) (float) (object) values[0] * -1.0);
        }
        else if (values.Length == 2)
        {
          char ch = oper;
          switch (ch)
          {
            case '*':
              return (T) (ValueType) (float) ((double) (float) (object) values[0] * (double) (float) (object) values[1]);
            case '+':
              return (T) (ValueType) (float) ((double) (float) (object) values[0] + (double) (float) (object) values[1]);
            case '-':
              return (T) (ValueType) (float) ((double) (float) (object) values[0] - (double) (float) (object) values[1]);
            case '/':
              return (T) (ValueType) (float) ((double) (float) (object) values[0] / (double) (float) (object) values[1]);
            default:
              if ((int) ch == 37)
                return (T) (ValueType) (float) ((double) (float) (object) values[0] % (double) (float) (object) values[1]);
              if ((int) ch == 94)
                return (T) (ValueType) Mathf.Pow((float) (object) values[0], (float) (object) values[1]);
              break;
          }
        }
      }
      else if (typeof (T) == typeof (int))
      {
        if (values.Length == 1)
        {
          if ((int) oper == 117)
            return (T) (ValueType) ((int) (object) values[0] * -1);
        }
        else if (values.Length == 2)
        {
          char ch = oper;
          switch (ch)
          {
            case '*':
              return (T) (ValueType) ((int) (object) values[0] * (int) (object) values[1]);
            case '+':
              return (T) (ValueType) ((int) (object) values[0] + (int) (object) values[1]);
            case '-':
              return (T) (ValueType) ((int) (object) values[0] - (int) (object) values[1]);
            case '/':
              return (T) (ValueType) ((int) (object) values[0] / (int) (object) values[1]);
            default:
              if ((int) ch == 37)
                return (T) (ValueType) ((int) (object) values[0] % (int) (object) values[1]);
              if ((int) ch == 94)
                return (T) (ValueType) (int) Math.Pow((double) (int) (object) values[0], (double) (int) (object) values[1]);
              break;
          }
        }
      }
      if (typeof (T) == typeof (double))
      {
        if (values.Length == 1)
        {
          if ((int) oper == 117)
            return (T) (ValueType) ((double) (object) values[0] * -1.0);
        }
        else if (values.Length == 2)
        {
          char ch = oper;
          switch (ch)
          {
            case '*':
              return (T) (ValueType) ((double) (object) values[0] * (double) (object) values[1]);
            case '+':
              return (T) (ValueType) ((double) (object) values[0] + (double) (object) values[1]);
            case '-':
              return (T) (ValueType) ((double) (object) values[0] - (double) (object) values[1]);
            case '/':
              return (T) (ValueType) ((double) (object) values[0] / (double) (object) values[1]);
            default:
              if ((int) ch == 37)
                return (T) (ValueType) ((double) (object) values[0] % (double) (object) values[1]);
              if ((int) ch == 94)
                return (T) (ValueType) Math.Pow((double) (object) values[0], (double) (object) values[1]);
              break;
          }
        }
      }
      else if (typeof (T) == typeof (long))
      {
        if (values.Length == 1)
        {
          if ((int) oper == 117)
            return (T) (ValueType) ((long) (object) values[0] * -1L);
        }
        else if (values.Length == 2)
        {
          char ch = oper;
          switch (ch)
          {
            case '*':
              return (T) (ValueType) ((long) (object) values[0] * (long) (object) values[1]);
            case '+':
              return (T) (ValueType) ((long) (object) values[0] + (long) (object) values[1]);
            case '-':
              return (T) (ValueType) ((long) (object) values[0] - (long) (object) values[1]);
            case '/':
              return (T) (ValueType) ((long) (object) values[0] / (long) (object) values[1]);
            default:
              if ((int) ch == 37)
                return (T) (ValueType) ((long) (object) values[0] % (long) (object) values[1]);
              if ((int) ch == 94)
                return (T) (ValueType) (long) Math.Pow((double) (long) (object) values[0], (double) (long) (object) values[1]);
              break;
          }
        }
      }
      return default (T);
    }

    private static bool TryParse<T>(string expression, out T result)
    {
      expression = expression.Replace(',', '.');
      bool flag = false;
      result = default (T);
      if (typeof (T) == typeof (float))
      {
        float result1 = 0.0f;
        flag = float.TryParse(expression, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1);
        result = (T) (ValueType) result1;
      }
      else if (typeof (T) == typeof (int))
      {
        int result1 = 0;
        flag = int.TryParse(expression, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1);
        result = (T) (ValueType) result1;
      }
      else if (typeof (T) == typeof (double))
      {
        double result1 = 0.0;
        flag = double.TryParse(expression, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1);
        result = (T) (ValueType) result1;
      }
      else if (typeof (T) == typeof (long))
      {
        long result1 = 0;
        flag = long.TryParse(expression, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1);
        result = (T) (ValueType) result1;
      }
      return flag;
    }

    private enum Associativity
    {
      Left,
      Right,
    }

    private struct Operator
    {
      public char character;
      public int presedence;
      public ExpressionEvaluator.Associativity associativity;
      public int inputs;

      public Operator(char character, int presedence, int inputs, ExpressionEvaluator.Associativity associativity)
      {
        this.character = character;
        this.presedence = presedence;
        this.inputs = inputs;
        this.associativity = associativity;
      }
    }
  }
}
