// Decompiled with JetBrains decompiler
// Type: UnityEditor.VerticalLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class VerticalLayout : IDisposable
  {
    private static readonly VerticalLayout instance = new VerticalLayout();

    private VerticalLayout()
    {
    }

    void IDisposable.Dispose()
    {
      GUILayout.EndVertical();
    }

    public static IDisposable DoLayout()
    {
      GUILayout.BeginVertical();
      return (IDisposable) VerticalLayout.instance;
    }
  }
}
