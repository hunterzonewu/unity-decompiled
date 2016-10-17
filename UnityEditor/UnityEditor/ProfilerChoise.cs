// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProfilerChoise
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal struct ProfilerChoise
  {
    public string Name;
    public bool Enabled;
    public Func<bool> IsSelected;
    public System.Action ConnectTo;
  }
}
