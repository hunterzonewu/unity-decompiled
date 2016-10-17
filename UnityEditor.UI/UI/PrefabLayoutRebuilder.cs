// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.PrefabLayoutRebuilder
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  [InitializeOnLoad]
  internal class PrefabLayoutRebuilder
  {
    static PrefabLayoutRebuilder()
    {
      PrefabUtility.prefabInstanceUpdated += new PrefabUtility.PrefabInstanceUpdated(PrefabLayoutRebuilder.OnPrefabInstanceUpdates);
    }

    private static void OnPrefabInstanceUpdates(GameObject instance)
    {
      if (!(bool) ((Object) instance))
        return;
      RectTransform transform = instance.transform as RectTransform;
      if (!(bool) ((Object) transform))
        return;
      LayoutRebuilder.MarkLayoutForRebuild(transform);
    }
  }
}
