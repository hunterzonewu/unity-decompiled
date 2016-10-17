// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.RestRequest
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Net;
using System.Text;

namespace UnityEditor.RestService
{
  internal class RestRequest
  {
    public static bool Send(string endpoint, string payload, int timeout)
    {
      if (ScriptEditorSettings.ServerURL == null)
        return false;
      byte[] bytes = Encoding.UTF8.GetBytes(payload);
      WebRequest webRequest = WebRequest.Create(ScriptEditorSettings.ServerURL + endpoint);
      webRequest.Timeout = timeout;
      webRequest.Method = "POST";
      webRequest.ContentType = "application/json";
      webRequest.ContentLength = (long) bytes.Length;
      try
      {
        Stream requestStream = webRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        return false;
      }
      try
      {
        webRequest.BeginGetResponse(new AsyncCallback(RestRequest.GetResponseCallback), (object) webRequest);
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        return false;
      }
      return true;
    }

    private static void GetResponseCallback(IAsyncResult asynchronousResult)
    {
      WebResponse response = ((WebRequest) asynchronousResult.AsyncState).EndGetResponse(asynchronousResult);
      try
      {
        Stream responseStream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream);
        streamReader.ReadToEnd();
        streamReader.Close();
        responseStream.Close();
      }
      finally
      {
        response.Close();
      }
    }
  }
}
