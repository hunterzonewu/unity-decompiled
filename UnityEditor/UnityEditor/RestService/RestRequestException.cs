// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.RestRequestException
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor.RestService
{
  internal class RestRequestException : Exception
  {
    public string RestErrorString { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public string RestErrorDescription { get; set; }

    public RestRequestException()
    {
    }

    public RestRequestException(HttpStatusCode httpStatusCode, string restErrorString)
      : this(httpStatusCode, restErrorString, (string) null)
    {
    }

    public RestRequestException(HttpStatusCode httpStatusCode, string restErrorString, string restErrorDescription)
    {
      this.HttpStatusCode = httpStatusCode;
      this.RestErrorString = restErrorString;
      this.RestErrorDescription = restErrorDescription;
    }
  }
}
