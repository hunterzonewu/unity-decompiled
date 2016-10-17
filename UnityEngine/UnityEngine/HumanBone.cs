// Decompiled with JetBrains decompiler
// Type: UnityEngine.HumanBone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The mapping between a bone in the model and the conceptual bone in the Mecanim human anatomy.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct HumanBone
  {
    private string m_BoneName;
    private string m_HumanName;
    /// <summary>
    ///   <para>The rotation limits that define the muscle for this bone.</para>
    /// </summary>
    public HumanLimit limit;

    /// <summary>
    ///   <para>The name of the bone to which the Mecanim human bone is mapped.</para>
    /// </summary>
    public string boneName
    {
      get
      {
        return this.m_BoneName;
      }
      set
      {
        this.m_BoneName = value;
      }
    }

    /// <summary>
    ///   <para>The name of the Mecanim human bone to which the bone from the model is mapped.</para>
    /// </summary>
    public string humanName
    {
      get
      {
        return this.m_HumanName;
      }
      set
      {
        this.m_HumanName = value;
      }
    }
  }
}
