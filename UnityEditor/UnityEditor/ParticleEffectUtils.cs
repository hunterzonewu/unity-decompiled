// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleEffectUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ParticleEffectUtils
  {
    private static List<GameObject> s_Planes = new List<GameObject>();

    public static GameObject GetPlane(int index)
    {
      while (ParticleEffectUtils.s_Planes.Count <= index)
      {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.hideFlags = HideFlags.HideAndDontSave;
        ParticleEffectUtils.s_Planes.Add(primitive);
      }
      return ParticleEffectUtils.s_Planes[index];
    }

    public static void ClearPlanes()
    {
      if (ParticleEffectUtils.s_Planes.Count <= 0)
        return;
      using (List<GameObject>.Enumerator enumerator = ParticleEffectUtils.s_Planes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.DestroyImmediate((Object) enumerator.Current);
      }
      ParticleEffectUtils.s_Planes.Clear();
    }
  }
}
