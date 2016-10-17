// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmSuccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmSuccess : JspmResult
  {
    public object result;
    public string type;

    public JspmSuccess(long messageID, object result, string type)
      : base(messageID, 0)
    {
      this.result = result;
      this.type = type;
    }
  }
}
