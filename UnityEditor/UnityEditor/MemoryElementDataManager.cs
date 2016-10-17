// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryElementDataManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class MemoryElementDataManager
  {
    private static int SortByMemoryClassName(ObjectInfo x, ObjectInfo y)
    {
      return y.className.CompareTo(x.className);
    }

    private static int SortByMemorySize(MemoryElement x, MemoryElement y)
    {
      return y.totalMemory.CompareTo(x.totalMemory);
    }

    private static MemoryElementDataManager.ObjectTypeFilter GetObjectTypeFilter(ObjectInfo info)
    {
      switch (info.reason)
      {
        case 1:
          return MemoryElementDataManager.ObjectTypeFilter.BuiltinResource;
        case 2:
          return MemoryElementDataManager.ObjectTypeFilter.DontSave;
        case 3:
        case 8:
        case 9:
          return MemoryElementDataManager.ObjectTypeFilter.Asset;
        case 10:
          return MemoryElementDataManager.ObjectTypeFilter.Other;
        default:
          return MemoryElementDataManager.ObjectTypeFilter.Scene;
      }
    }

    private static bool HasValidNames(List<MemoryElement> memory)
    {
      for (int index = 0; index < memory.Count; ++index)
      {
        if (!string.IsNullOrEmpty(memory[index].name))
          return true;
      }
      return false;
    }

    private static List<MemoryElement> GenerateObjectTypeGroups(ObjectInfo[] memory, MemoryElementDataManager.ObjectTypeFilter filter)
    {
      List<MemoryElement> memoryElementList = new List<MemoryElement>();
      MemoryElement memoryElement = (MemoryElement) null;
      foreach (ObjectInfo objectInfo in memory)
      {
        if (MemoryElementDataManager.GetObjectTypeFilter(objectInfo) == filter)
        {
          if (memoryElement == null || objectInfo.className != memoryElement.name)
          {
            memoryElement = new MemoryElement(objectInfo.className);
            memoryElementList.Add(memoryElement);
          }
          memoryElement.AddChild(new MemoryElement(objectInfo, true));
        }
      }
      memoryElementList.Sort(new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize));
      using (List<MemoryElement>.Enumerator enumerator = memoryElementList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MemoryElement current = enumerator.Current;
          current.children.Sort(new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize));
          if (filter == MemoryElementDataManager.ObjectTypeFilter.Other && !MemoryElementDataManager.HasValidNames(current.children))
            current.children.Clear();
        }
      }
      return memoryElementList;
    }

    public static MemoryElement GetTreeRoot(ObjectMemoryInfo[] memoryObjectList, int[] referencesIndices)
    {
      ObjectInfo[] objectInfoArray = new ObjectInfo[memoryObjectList.Length];
      for (int index = 0; index < memoryObjectList.Length; ++index)
        objectInfoArray[index] = new ObjectInfo()
        {
          instanceId = memoryObjectList[index].instanceId,
          memorySize = memoryObjectList[index].memorySize,
          reason = memoryObjectList[index].reason,
          name = memoryObjectList[index].name,
          className = memoryObjectList[index].className
        };
      int num = 0;
      for (int index1 = 0; index1 < memoryObjectList.Length; ++index1)
      {
        for (int index2 = 0; index2 < memoryObjectList[index1].count; ++index2)
        {
          int referencesIndex = referencesIndices[index2 + num];
          if (objectInfoArray[referencesIndex].referencedBy == null)
            objectInfoArray[referencesIndex].referencedBy = new List<ObjectInfo>();
          objectInfoArray[referencesIndex].referencedBy.Add(objectInfoArray[index1]);
        }
        num += memoryObjectList[index1].count;
      }
      MemoryElement memoryElement = new MemoryElement();
      Array.Sort<ObjectInfo>(objectInfoArray, new Comparison<ObjectInfo>(MemoryElementDataManager.SortByMemoryClassName));
      memoryElement.AddChild(new MemoryElement("Scene Memory", MemoryElementDataManager.GenerateObjectTypeGroups(objectInfoArray, MemoryElementDataManager.ObjectTypeFilter.Scene)));
      memoryElement.AddChild(new MemoryElement("Assets", MemoryElementDataManager.GenerateObjectTypeGroups(objectInfoArray, MemoryElementDataManager.ObjectTypeFilter.Asset)));
      memoryElement.AddChild(new MemoryElement("Builtin Resources", MemoryElementDataManager.GenerateObjectTypeGroups(objectInfoArray, MemoryElementDataManager.ObjectTypeFilter.BuiltinResource)));
      memoryElement.AddChild(new MemoryElement("Not Saved", MemoryElementDataManager.GenerateObjectTypeGroups(objectInfoArray, MemoryElementDataManager.ObjectTypeFilter.DontSave)));
      memoryElement.AddChild(new MemoryElement("Other", MemoryElementDataManager.GenerateObjectTypeGroups(objectInfoArray, MemoryElementDataManager.ObjectTypeFilter.Other)));
      memoryElement.children.Sort(new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize));
      return memoryElement;
    }

    private enum ObjectTypeFilter
    {
      Scene,
      Asset,
      BuiltinResource,
      DontSave,
      Other,
    }
  }
}
