// Decompiled with JetBrains decompiler
// Type: UnityEngine.ISerializationCallbackReceiver
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequiredByNativeCode]
  public interface ISerializationCallbackReceiver
  {
    /// <summary>
    ///   <para>Implement this method to receive a callback after unity serialized your object.</para>
    /// </summary>
    void OnBeforeSerialize();

    /// <summary>
    ///   <para>See ISerializationCallbackReceiver.OnBeforeSerialize for documentation on how to use this method.</para>
    /// </summary>
    void OnAfterDeserialize();
  }
}
