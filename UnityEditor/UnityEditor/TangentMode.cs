// Decompiled with JetBrains decompiler
// Type: UnityEditor.TangentMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  [System.Flags]
  internal enum TangentMode
  {
    Editable = 0,
    Smooth = 1,
    Linear = 2,
    Stepped = Linear | Smooth,
  }
}
