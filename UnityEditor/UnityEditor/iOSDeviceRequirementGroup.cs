// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSDeviceRequirementGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class iOSDeviceRequirementGroup
  {
    private string m_VariantName;

    public int count
    {
      get
      {
        return iOSDeviceRequirementGroup.GetCountForVariantImpl(this.m_VariantName);
      }
    }

    public iOSDeviceRequirement this[int index]
    {
      get
      {
        string[] keys;
        string[] values;
        iOSDeviceRequirementGroup.GetDeviceRequirementForVariantNameImpl(this.m_VariantName, index, out keys, out values);
        iOSDeviceRequirement deviceRequirement = new iOSDeviceRequirement();
        for (int index1 = 0; index1 < keys.Length; ++index1)
          deviceRequirement.values.Add(keys[index1], values[index1]);
        return deviceRequirement;
      }
      set
      {
        iOSDeviceRequirementGroup.SetOrAddDeviceRequirementForVariantNameImpl(this.m_VariantName, index, value.values.Keys.ToArray<string>(), value.values.Values.ToArray<string>());
      }
    }

    internal iOSDeviceRequirementGroup(string variantName)
    {
      this.m_VariantName = variantName;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetDeviceRequirementForVariantNameImpl(string name, int index, out string[] keys, out string[] values);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetOrAddDeviceRequirementForVariantNameImpl(string name, int index, string[] keys, string[] values);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetCountForVariantImpl(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void RemoveAtImpl(string name, int index);

    public void RemoveAt(int index)
    {
      iOSDeviceRequirementGroup.RemoveAtImpl(this.m_VariantName, index);
    }

    public void Add(iOSDeviceRequirement requirement)
    {
      iOSDeviceRequirementGroup.SetOrAddDeviceRequirementForVariantNameImpl(this.m_VariantName, -1, requirement.values.Keys.ToArray<string>(), requirement.values.Values.ToArray<string>());
    }
  }
}
