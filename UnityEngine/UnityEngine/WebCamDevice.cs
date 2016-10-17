// Decompiled with JetBrains decompiler
// Type: UnityEngine.WebCamDevice
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A structure describing the webcam device.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct WebCamDevice
  {
    internal string m_Name;
    internal int m_Flags;

    /// <summary>
    ///   <para>A human-readable name of the device. Varies across different systems.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
    }

    /// <summary>
    ///   <para>True if camera faces the same direction a screen does, false otherwise.</para>
    /// </summary>
    public bool isFrontFacing
    {
      get
      {
        return (this.m_Flags & 1) == 1;
      }
    }
  }
}
