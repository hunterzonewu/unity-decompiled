// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.OnOpenAssetAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Callback attribute for opening an asset in Unity (e.g the callback is fired when double clicking an asset in the Project Browser).</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class OnOpenAssetAttribute : CallbackOrderAttribute
  {
    public OnOpenAssetAttribute()
    {
      this.m_CallbackOrder = 1;
    }

    public OnOpenAssetAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
