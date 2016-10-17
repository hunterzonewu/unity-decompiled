// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.RemoteAddress
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal struct RemoteAddress
  {
    public string ip;
    public int port;

    public RemoteAddress(string ip, int port)
    {
      this.ip = ip;
      this.port = port;
    }
  }
}
