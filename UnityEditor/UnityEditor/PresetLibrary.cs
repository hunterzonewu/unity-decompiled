// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class PresetLibrary : ScriptableObject
  {
    public abstract int Count();

    public abstract object GetPreset(int index);

    public abstract void Add(object presetObject, string presetName);

    public abstract void Replace(int index, object newPresetObject);

    public abstract void Remove(int index);

    public abstract void Move(int index, int destIndex, bool insertAfterDestIndex);

    public abstract void Draw(Rect rect, int index);

    public abstract void Draw(Rect rect, object presetObject);

    public abstract string GetName(int index);

    public abstract void SetName(int index, string name);
  }
}
