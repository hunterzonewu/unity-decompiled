// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmError
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmError : JspmResult
  {
    public string errorClass;
    public string message;

    public JspmError(long messageID, int status, string errorClass, string message)
      : base(messageID, status)
    {
      this.errorClass = errorClass;
      this.message = message;
    }
  }
}
