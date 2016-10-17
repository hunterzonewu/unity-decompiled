// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorWindowTitleAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class EditorWindowTitleAttribute : Attribute
  {
    public string title { get; set; }

    public string icon { get; set; }

    public bool useTypeNameAsIconName { get; set; }
  }
}
