// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.Handler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal abstract class Handler
  {
    private void InvokeGet(Request request, string payload, WriteResponse writeResponse)
    {
      Handler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandleGet));
    }

    private void InvokePost(Request request, string payload, WriteResponse writeResponse)
    {
      Handler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandlePost));
    }

    private void InvokeDelete(Request request, string payload, WriteResponse writeResponse)
    {
      Handler.CallSafely(request, payload, writeResponse, new Func<Request, JSONValue, JSONValue>(this.HandleDelete));
    }

    private static void CallSafely(Request request, string payload, WriteResponse writeResponse, Func<Request, JSONValue, JSONValue> method)
    {
      try
      {
        JSONValue jsonValue = (JSONValue) ((string) null);
        if (payload.Trim().Length == 0)
        {
          jsonValue = new JSONValue();
        }
        else
        {
          try
          {
            jsonValue = new JSONParser(request.Payload).Parse();
          }
          catch (JSONParseException ex)
          {
            Handler.ThrowInvalidJSONException();
          }
        }
        writeResponse(HttpStatusCode.Ok, method(request, jsonValue).ToString());
      }
      catch (JSONTypeException ex)
      {
        Handler.ThrowInvalidJSONException();
      }
      catch (KeyNotFoundException ex)
      {
        Handler.RespondWithException(writeResponse, new RestRequestException()
        {
          HttpStatusCode = HttpStatusCode.BadRequest
        });
      }
      catch (RestRequestException ex)
      {
        Handler.RespondWithException(writeResponse, ex);
      }
      catch (Exception ex)
      {
        Handler.RespondWithException(writeResponse, new RestRequestException()
        {
          HttpStatusCode = HttpStatusCode.InternalServerError,
          RestErrorString = "InternalServerError",
          RestErrorDescription = "Caught exception while fulfilling request: " + (object) ex
        });
      }
    }

    private static void ThrowInvalidJSONException()
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Invalid JSON" };
    }

    private static void RespondWithException(WriteResponse writeResponse, RestRequestException rre)
    {
      StringBuilder stringBuilder = new StringBuilder("{");
      if (rre.RestErrorString != null)
        stringBuilder.AppendFormat("\"error\":\"{0}\",", (object) rre.RestErrorString);
      if (rre.RestErrorDescription != null)
        stringBuilder.AppendFormat("\"errordescription\":\"{0}\"", (object) rre.RestErrorDescription);
      stringBuilder.Append("}");
      writeResponse(rre.HttpStatusCode, stringBuilder.ToString());
    }

    protected virtual JSONValue HandleGet(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the GET verb." };
    }

    protected virtual JSONValue HandlePost(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the POST verb." };
    }

    protected virtual JSONValue HandleDelete(Request request, JSONValue payload)
    {
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.MethodNotAllowed, RestErrorString = "MethodNotAllowed", RestErrorDescription = "This endpoint does not support the DELETE verb." };
    }

    protected static JSONValue ToJSON(IEnumerable<string> strings)
    {
      return new JSONValue((object) strings.Select<string, JSONValue>((Func<string, JSONValue>) (s => new JSONValue((object) s))).ToList<JSONValue>());
    }
  }
}
