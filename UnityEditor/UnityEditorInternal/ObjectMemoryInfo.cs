// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ObjectMemoryInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditorInternal
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class ObjectMemoryInfo
  {
    public int instanceId;
    public int memorySize;
    public int count;
    public int reason;
    public string name;
    public string className;
  }
}
