// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.StencilMaterial
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
  public static class StencilMaterial
  {
    private static List<StencilMaterial.MatEntry> m_List = new List<StencilMaterial.MatEntry>();

    [Obsolete("Use Material.Add instead.", true)]
    public static Material Add(Material baseMat, int stencilID)
    {
      return (Material) null;
    }

    public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask)
    {
      return StencilMaterial.Add(baseMat, stencilID, operation, compareFunction, colorWriteMask, (int) byte.MaxValue, (int) byte.MaxValue);
    }

    public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask, int readMask, int writeMask)
    {
      if (stencilID <= 0 && colorWriteMask == ColorWriteMask.All || (UnityEngine.Object) baseMat == (UnityEngine.Object) null)
        return baseMat;
      if (!baseMat.HasProperty("_Stencil"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _Stencil property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      if (!baseMat.HasProperty("_StencilOp"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _StencilOp property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      if (!baseMat.HasProperty("_StencilComp"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _StencilComp property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      if (!baseMat.HasProperty("_StencilReadMask"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _StencilReadMask property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      if (!baseMat.HasProperty("_StencilReadMask"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _StencilWriteMask property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      if (!baseMat.HasProperty("_ColorMask"))
      {
        Debug.LogWarning((object) ("Material " + baseMat.name + " doesn't have _ColorMask property"), (UnityEngine.Object) baseMat);
        return baseMat;
      }
      for (int index = 0; index < StencilMaterial.m_List.Count; ++index)
      {
        StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[index];
        if ((UnityEngine.Object) matEntry.baseMat == (UnityEngine.Object) baseMat && matEntry.stencilId == stencilID && (matEntry.operation == operation && matEntry.compareFunction == compareFunction) && (matEntry.readMask == readMask && matEntry.writeMask == writeMask && matEntry.colorMask == colorWriteMask))
        {
          ++matEntry.count;
          return matEntry.customMat;
        }
      }
      StencilMaterial.MatEntry matEntry1 = new StencilMaterial.MatEntry();
      matEntry1.count = 1;
      matEntry1.baseMat = baseMat;
      matEntry1.customMat = new Material(baseMat);
      matEntry1.customMat.hideFlags = HideFlags.HideAndDontSave;
      matEntry1.stencilId = stencilID;
      matEntry1.operation = operation;
      matEntry1.compareFunction = compareFunction;
      matEntry1.readMask = readMask;
      matEntry1.writeMask = writeMask;
      matEntry1.colorMask = colorWriteMask;
      matEntry1.useAlphaClip = operation != StencilOp.Keep && writeMask > 0;
      matEntry1.customMat.name = string.Format("Stencil Id:{0}, Op:{1}, Comp:{2}, WriteMask:{3}, ReadMask:{4}, ColorMask:{5} AlphaClip:{6} ({7})", (object) stencilID, (object) operation, (object) compareFunction, (object) writeMask, (object) readMask, (object) colorWriteMask, (object) matEntry1.useAlphaClip, (object) baseMat.name);
      matEntry1.customMat.SetInt("_Stencil", stencilID);
      matEntry1.customMat.SetInt("_StencilOp", (int) operation);
      matEntry1.customMat.SetInt("_StencilComp", (int) compareFunction);
      matEntry1.customMat.SetInt("_StencilReadMask", readMask);
      matEntry1.customMat.SetInt("_StencilWriteMask", writeMask);
      matEntry1.customMat.SetInt("_ColorMask", (int) colorWriteMask);
      if (matEntry1.customMat.HasProperty("_UseAlphaClip"))
        matEntry1.customMat.SetInt("_UseAlphaClip", !matEntry1.useAlphaClip ? 0 : 1);
      if (matEntry1.useAlphaClip)
        matEntry1.customMat.EnableKeyword("UNITY_UI_ALPHACLIP");
      else
        matEntry1.customMat.DisableKeyword("UNITY_UI_ALPHACLIP");
      StencilMaterial.m_List.Add(matEntry1);
      return matEntry1.customMat;
    }

    public static void Remove(Material customMat)
    {
      if ((UnityEngine.Object) customMat == (UnityEngine.Object) null)
        return;
      for (int index = 0; index < StencilMaterial.m_List.Count; ++index)
      {
        StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[index];
        if (!((UnityEngine.Object) matEntry.customMat != (UnityEngine.Object) customMat))
        {
          if (--matEntry.count != 0)
            break;
          Misc.DestroyImmediate((UnityEngine.Object) matEntry.customMat);
          matEntry.baseMat = (Material) null;
          StencilMaterial.m_List.RemoveAt(index);
          break;
        }
      }
    }

    public static void ClearAll()
    {
      for (int index = 0; index < StencilMaterial.m_List.Count; ++index)
      {
        StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[index];
        Misc.DestroyImmediate((UnityEngine.Object) matEntry.customMat);
        matEntry.baseMat = (Material) null;
      }
      StencilMaterial.m_List.Clear();
    }

    private class MatEntry
    {
      public CompareFunction compareFunction = CompareFunction.Always;
      public Material baseMat;
      public Material customMat;
      public int count;
      public int stencilId;
      public StencilOp operation;
      public int readMask;
      public int writeMask;
      public bool useAlphaClip;
      public ColorWriteMask colorMask;
    }
  }
}
