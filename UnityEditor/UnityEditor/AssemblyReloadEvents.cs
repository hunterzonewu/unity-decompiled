// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyReloadEvents
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssemblyReloadEvents
  {
    public static void OnBeforeAssemblyReload()
    {
      Security.ClearVerifiedAssemblies();
      InternalEditorUtility.AuxWindowManager_OnAssemblyReload();
    }

    public static void OnAfterAssemblyReload()
    {
      using (List<ProjectBrowser>.Enumerator enumerator = ProjectBrowser.GetAllProjectBrowsers().GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Repaint();
      }
    }
  }
}
