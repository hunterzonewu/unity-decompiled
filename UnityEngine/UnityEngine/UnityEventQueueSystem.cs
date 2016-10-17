// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityEventQueueSystem
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class UnityEventQueueSystem
  {
    public static string GenerateEventIdForPayload(string eventPayloadName)
    {
      byte[] byteArray = Guid.NewGuid().ToByteArray();
      return string.Format("REGISTER_EVENT_ID(0x{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}ULL,0x{8:X2}{9:X2}{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}ULL,{16})", (object) byteArray[0], (object) byteArray[1], (object) byteArray[2], (object) byteArray[3], (object) byteArray[4], (object) byteArray[5], (object) byteArray[6], (object) byteArray[7], (object) byteArray[8], (object) byteArray[9], (object) byteArray[10], (object) byteArray[11], (object) byteArray[12], (object) byteArray[13], (object) byteArray[14], (object) byteArray[15], (object) eventPayloadName);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern IntPtr GetGlobalEventQueue();
  }
}
