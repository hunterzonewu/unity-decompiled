// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ServerCallbackAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, to make them only run on servers, but not generate warnings.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class ServerCallbackAttribute : Attribute
  {
  }
}
