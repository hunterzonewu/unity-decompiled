// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryHelpers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class PresetLibraryHelpers
  {
    public static void MoveListItem<T>(List<T> list, int index, int destIndex, bool insertAfterDestIndex)
    {
      if (index < 0 || destIndex < 0)
      {
        Debug.LogError((object) "Invalid preset move");
      }
      else
      {
        if (index == destIndex)
          return;
        if (destIndex > index)
          --destIndex;
        if (insertAfterDestIndex && destIndex < list.Count - 1)
          ++destIndex;
        T obj = list[index];
        list.RemoveAt(index);
        list.Insert(destIndex, obj);
      }
    }
  }
}
