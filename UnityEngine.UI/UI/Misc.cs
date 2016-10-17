// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Misc
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  internal static class Misc
  {
    public static void Destroy(Object obj)
    {
      if (!(obj != (Object) null))
        return;
      if (Application.isPlaying)
      {
        if (obj is GameObject)
          (obj as GameObject).transform.parent = (Transform) null;
        Object.Destroy(obj);
      }
      else
        Object.DestroyImmediate(obj);
    }

    public static void DestroyImmediate(Object obj)
    {
      if (!(obj != (Object) null))
        return;
      if (Application.isEditor)
        Object.DestroyImmediate(obj);
      else
        Object.Destroy(obj);
    }
  }
}
