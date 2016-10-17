// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmStubInfoSuccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmStubInfoSuccess : JspmSuccess
  {
    public string reference;

    public JspmStubInfoSuccess(long messageID, string reference, JspmPropertyInfo[] properties, JspmMethodInfo[] methods, string[] events)
      : base(messageID, (object) new JspmStubInfo(properties, methods, events), "GETSTUBINFO")
    {
      this.reference = reference;
    }
  }
}
