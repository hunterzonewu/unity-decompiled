// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmResult
  {
    public double version;
    public long messageID;
    public int status;

    public JspmResult()
    {
      this.version = 1.0;
      this.messageID = -1L;
      this.status = 0;
    }

    public JspmResult(long messageID, int status)
    {
      this.version = 1.0;
      this.messageID = messageID;
      this.status = status;
    }
  }
}
