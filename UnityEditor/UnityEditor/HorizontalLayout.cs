// Decompiled with JetBrains decompiler
// Type: UnityEditor.HorizontalLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class HorizontalLayout : IDisposable
  {
    private static readonly HorizontalLayout instance = new HorizontalLayout();

    private HorizontalLayout()
    {
    }

    void IDisposable.Dispose()
    {
      GUILayout.EndHorizontal();
    }

    public static IDisposable DoLayout()
    {
      GUILayout.BeginHorizontal();
      return (IDisposable) HorizontalLayout.instance;
    }
  }
}
