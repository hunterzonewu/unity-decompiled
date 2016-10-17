// Decompiled with JetBrains decompiler
// Type: UnityEngine.SystemClock
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal class SystemClock
  {
    private static readonly DateTime s_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime now
    {
      get
      {
        return DateTime.Now;
      }
    }

    public static long ToUnixTimeMilliseconds(DateTime date)
    {
      return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalMilliseconds);
    }

    public static long ToUnixTimeSeconds(DateTime date)
    {
      return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalSeconds);
    }
  }
}
