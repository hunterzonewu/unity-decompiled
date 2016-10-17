// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.InstrumentedAssemblyTypes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Flags]
  public enum InstrumentedAssemblyTypes
  {
    None = 0,
    System = 1,
    Unity = 2,
    Plugins = 4,
    Script = 8,
    All = 2147483647,
  }
}
