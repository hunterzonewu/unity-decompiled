// Decompiled with JetBrains decompiler
// Type: UnityEditor.IFlexibleMenuItemProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal interface IFlexibleMenuItemProvider
  {
    int Count();

    object GetItem(int index);

    int Add(object obj);

    void Replace(int index, object newPresetObject);

    void Remove(int index);

    object Create();

    void Move(int index, int destIndex, bool insertAfterDestIndex);

    string GetName(int index);

    bool IsModificationAllowed(int index);

    int[] GetSeperatorIndices();
  }
}
