// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizeGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class GameViewSizeGroup
  {
    [NonSerialized]
    private List<GameViewSize> m_Builtin = new List<GameViewSize>();
    [SerializeField]
    private List<GameViewSize> m_Custom = new List<GameViewSize>();

    public GameViewSize GetGameViewSize(int index)
    {
      if (index < this.m_Builtin.Count)
        return this.m_Builtin[index];
      index -= this.m_Builtin.Count;
      if (index >= 0 && index < this.m_Custom.Count)
        return this.m_Custom[index];
      Debug.LogError((object) ("Invalid index " + (object) (index + this.m_Builtin.Count) + " " + (object) this.m_Builtin.Count + " " + (object) this.m_Custom.Count));
      return new GameViewSize(GameViewSizeType.AspectRatio, 0, 0, string.Empty);
    }

    public string[] GetDisplayTexts()
    {
      List<string> stringList = new List<string>();
      using (List<GameViewSize>.Enumerator enumerator = this.m_Builtin.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameViewSize current = enumerator.Current;
          stringList.Add(current.displayText);
        }
      }
      using (List<GameViewSize>.Enumerator enumerator = this.m_Custom.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameViewSize current = enumerator.Current;
          stringList.Add(current.displayText);
        }
      }
      return stringList.ToArray();
    }

    public int GetTotalCount()
    {
      return this.m_Builtin.Count + this.m_Custom.Count;
    }

    public int GetBuiltinCount()
    {
      return this.m_Builtin.Count;
    }

    public int GetCustomCount()
    {
      return this.m_Custom.Count;
    }

    public void AddBuiltinSizes(params GameViewSize[] sizes)
    {
      for (int index = 0; index < sizes.Length; ++index)
        this.AddBuiltinSize(sizes[index]);
    }

    public void AddBuiltinSize(GameViewSize size)
    {
      this.m_Builtin.Add(size);
      ScriptableSingleton<GameViewSizes>.instance.Changed();
    }

    public void AddCustomSizes(params GameViewSize[] sizes)
    {
      for (int index = 0; index < sizes.Length; ++index)
        this.AddCustomSize(sizes[index]);
    }

    public void AddCustomSize(GameViewSize size)
    {
      this.m_Custom.Add(size);
      ScriptableSingleton<GameViewSizes>.instance.Changed();
    }

    public void RemoveCustomSize(int index)
    {
      int customIndex = this.TotalIndexToCustomIndex(index);
      if (customIndex >= 0 && customIndex < this.m_Custom.Count)
      {
        this.m_Custom.RemoveAt(customIndex);
        ScriptableSingleton<GameViewSizes>.instance.Changed();
      }
      else
        Debug.LogError((object) ("Invalid index " + (object) index + " " + (object) this.m_Builtin.Count + " " + (object) this.m_Custom.Count));
    }

    public bool IsCustomSize(int index)
    {
      return index >= this.m_Builtin.Count;
    }

    public int TotalIndexToCustomIndex(int index)
    {
      return index - this.m_Builtin.Count;
    }

    public int IndexOf(GameViewSize view)
    {
      int num = this.m_Builtin.IndexOf(view);
      if (num >= 0)
        return num;
      return this.m_Custom.IndexOf(view);
    }
  }
}
