// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.JspmStubInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Web
{
  internal class JspmStubInfo
  {
    public JspmPropertyInfo[] properties;
    public JspmMethodInfo[] methods;
    public string[] events;

    public JspmStubInfo(JspmPropertyInfo[] properties, JspmMethodInfo[] methods, string[] events)
    {
      this.methods = methods;
      this.properties = properties;
      this.events = events;
    }
  }
}
