// Decompiled with JetBrains decompiler
// Type: UnityEditor.ArrayUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Helpers for builtin arrays ...</para>
  /// </summary>
  public sealed class ArrayUtility
  {
    public static void Add<T>(ref T[] array, T item)
    {
      Array.Resize<T>(ref array, array.Length + 1);
      array[array.Length - 1] = item;
    }

    public static bool ArrayEquals<T>(T[] lhs, T[] rhs)
    {
      if (lhs.Length != rhs.Length)
        return false;
      for (int index = 0; index < lhs.Length; ++index)
      {
        if (!lhs[index].Equals((object) rhs[index]))
          return false;
      }
      return true;
    }

    public static void AddRange<T>(ref T[] array, T[] items)
    {
      int length = array.Length;
      Array.Resize<T>(ref array, array.Length + items.Length);
      for (int index = 0; index < items.Length; ++index)
        array[length + index] = items[index];
    }

    public static void Insert<T>(ref T[] array, int index, T item)
    {
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) array);
      arrayList.Insert(index, (object) item);
      array = arrayList.ToArray(typeof (T)) as T[];
    }

    public static void Remove<T>(ref T[] array, T item)
    {
      List<T> objList = new List<T>((IEnumerable<T>) array);
      objList.Remove(item);
      array = objList.ToArray();
    }

    public static List<T> FindAll<T>(T[] array, Predicate<T> match)
    {
      return new List<T>((IEnumerable<T>) array).FindAll(match);
    }

    public static T Find<T>(T[] array, Predicate<T> match)
    {
      return new List<T>((IEnumerable<T>) array).Find(match);
    }

    public static int FindIndex<T>(T[] array, Predicate<T> match)
    {
      return new List<T>((IEnumerable<T>) array).FindIndex(match);
    }

    public static int IndexOf<T>(T[] array, T value)
    {
      return new List<T>((IEnumerable<T>) array).IndexOf(value);
    }

    public static int LastIndexOf<T>(T[] array, T value)
    {
      return new List<T>((IEnumerable<T>) array).LastIndexOf(value);
    }

    public static void RemoveAt<T>(ref T[] array, int index)
    {
      List<T> objList = new List<T>((IEnumerable<T>) array);
      objList.RemoveAt(index);
      array = objList.ToArray();
    }

    public static bool Contains<T>(T[] array, T item)
    {
      return new List<T>((IEnumerable<T>) array).Contains(item);
    }

    public static void Clear<T>(ref T[] array)
    {
      Array.Clear((Array) array, 0, array.Length);
      Array.Resize<T>(ref array, 0);
    }
  }
}
