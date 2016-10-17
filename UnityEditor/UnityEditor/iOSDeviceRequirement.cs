// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSDeviceRequirement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  /// <summary>
  ///   <para>A device requirement description used for configuration of App Slicing.</para>
  /// </summary>
  public sealed class iOSDeviceRequirement
  {
    private SortedDictionary<string, string> m_Values = new SortedDictionary<string, string>();

    /// <summary>
    ///   <para>The values of the device requirement description.</para>
    /// </summary>
    public IDictionary<string, string> values
    {
      get
      {
        return (IDictionary<string, string>) this.m_Values;
      }
    }
  }
}
