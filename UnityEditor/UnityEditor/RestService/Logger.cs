// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.Logger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class Logger
  {
    public static void Log(Exception an_exception)
    {
      Debug.Log((object) an_exception.ToString());
    }

    public static void Log(string a_message)
    {
      Debug.Log((object) a_message);
    }
  }
}
