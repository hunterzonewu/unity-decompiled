// Decompiled with JetBrains decompiler
// Type: SerializedStringTable
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using UnityEngine;

[Serializable]
internal class SerializedStringTable
{
  [SerializeField]
  private string[] keys;
  [SerializeField]
  private int[] values;
  private Hashtable table;

  public Hashtable hashtable
  {
    get
    {
      this.SanityCheck();
      return this.table;
    }
  }

  public int Length
  {
    get
    {
      this.SanityCheck();
      return this.keys.Length;
    }
  }

  private void SanityCheck()
  {
    if (this.keys == null)
    {
      this.keys = new string[0];
      this.values = new int[0];
    }
    if (this.table != null)
      return;
    this.table = new Hashtable();
    for (int index = 0; index < this.keys.Length; ++index)
      this.table[(object) this.keys[index]] = (object) this.values[index];
  }

  private void SynchArrays()
  {
    this.keys = new string[this.table.Count];
    this.values = new int[this.table.Count];
    this.table.Keys.CopyTo((Array) this.keys, 0);
    this.table.Values.CopyTo((Array) this.values, 0);
  }

  public void Set(string key, int value)
  {
    this.SanityCheck();
    this.table[(object) key] = (object) value;
    this.SynchArrays();
  }

  public void Set(string key)
  {
    this.Set(key, 0);
  }

  public bool Contains(string key)
  {
    this.SanityCheck();
    return this.table.Contains((object) key);
  }

  public int Get(string key)
  {
    this.SanityCheck();
    if (!this.table.Contains((object) key))
      return -1;
    return (int) this.table[(object) key];
  }

  public void Remove(string key)
  {
    this.SanityCheck();
    if (this.table.Contains((object) key))
      this.table.Remove((object) key);
    this.SynchArrays();
  }
}
