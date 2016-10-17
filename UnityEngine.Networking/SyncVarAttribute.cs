// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncVarAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///         <para>[SyncVar] is an attribute that can be put on member variables of UNeBehaviour classes. These variables will have their values sychronized from the server to clients in the game that are in the ready state.
  /// </para>
  ///       </summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class SyncVarAttribute : Attribute
  {
    /// <summary>
    ///   <para>The hook attribute can be used to specify a function to be called when the sync var changes value on the client.</para>
    /// </summary>
    public string hook;
  }
}
