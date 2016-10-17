// Decompiled with JetBrains decompiler
// Type: UnityEditor.DropInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DropInfo
  {
    public DropInfo.Type type = DropInfo.Type.Window;
    public IDropArea dropArea;
    public object userData;
    public Rect rect;

    public DropInfo(IDropArea source)
    {
      this.dropArea = source;
    }

    internal enum Type
    {
      Tab,
      Pane,
      Window,
    }
  }
}
