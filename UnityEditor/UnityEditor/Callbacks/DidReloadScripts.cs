// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.DidReloadScripts
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to a method to get a notification after scripts have been reloaded.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class DidReloadScripts : CallbackOrderAttribute
  {
    /// <summary>
    ///   <para>DidReloadScripts attribute.</para>
    /// </summary>
    /// <param name="callbackOrder">Order in which separate attributes will be processed.</param>
    public DidReloadScripts()
    {
      this.m_CallbackOrder = 1;
    }

    /// <summary>
    ///   <para>DidReloadScripts attribute.</para>
    /// </summary>
    /// <param name="callbackOrder">Order in which separate attributes will be processed.</param>
    public DidReloadScripts(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
