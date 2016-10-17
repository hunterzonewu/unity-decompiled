// Decompiled with JetBrains decompiler
// Type: UnityEngine.ResourceRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Asynchronous load request from the Resources bundle.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class ResourceRequest : AsyncOperation
  {
    internal string m_Path;
    internal System.Type m_Type;

    /// <summary>
    ///   <para>Asset object being loaded (Read Only).</para>
    /// </summary>
    public Object asset
    {
      get
      {
        return Resources.Load(this.m_Path, this.m_Type);
      }
    }
  }
}
