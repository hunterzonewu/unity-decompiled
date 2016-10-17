// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.APIUpdaterLogger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Scripting
{
  internal class APIUpdaterLogger
  {
    public static void WriteToFile(string msg, params object[] args)
    {
      Console.WriteLine("[Script API Updater] {0}", (object) string.Format(msg, args));
    }

    public static void WriteErrorToConsole(string msg, params object[] args)
    {
      Debug.LogErrorFormat(msg, args);
    }

    public static void WriteInfoToConsole(string line)
    {
      Debug.Log((object) line);
    }
  }
}
