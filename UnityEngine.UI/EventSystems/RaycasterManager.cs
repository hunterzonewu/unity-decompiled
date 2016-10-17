// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.RaycasterManager
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  internal static class RaycasterManager
  {
    private static readonly List<BaseRaycaster> s_Raycasters = new List<BaseRaycaster>();

    public static void AddRaycaster(BaseRaycaster baseRaycaster)
    {
      if (RaycasterManager.s_Raycasters.Contains(baseRaycaster))
        return;
      RaycasterManager.s_Raycasters.Add(baseRaycaster);
    }

    public static List<BaseRaycaster> GetRaycasters()
    {
      return RaycasterManager.s_Raycasters;
    }

    public static void RemoveRaycasters(BaseRaycaster baseRaycaster)
    {
      if (!RaycasterManager.s_Raycasters.Contains(baseRaycaster))
        return;
      RaycasterManager.s_Raycasters.Remove(baseRaycaster);
    }
  }
}
