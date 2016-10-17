// Decompiled with JetBrains decompiler
// Type: UnityEngine.PlayerPrefsException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>An exception thrown by the PlayerPrefs class in a  web player build.</para>
  /// </summary>
  public sealed class PlayerPrefsException : Exception
  {
    public PlayerPrefsException(string error)
      : base(error)
    {
    }
  }
}
