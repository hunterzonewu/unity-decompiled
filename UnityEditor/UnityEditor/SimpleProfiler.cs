// Decompiled with JetBrains decompiler
// Type: UnityEditor.SimpleProfiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor
{
  internal class SimpleProfiler
  {
    private static Stack<string> m_Names = new Stack<string>();
    private static Stack<float> m_StartTime = new Stack<float>();
    private static Dictionary<string, float> m_Timers = new Dictionary<string, float>();
    private static Dictionary<string, int> m_Calls = new Dictionary<string, int>();

    [Conditional("SIMPLE_PROFILER")]
    public static void Begin(string label)
    {
      SimpleProfiler.m_Names.Push(label);
      SimpleProfiler.m_StartTime.Push(Time.realtimeSinceStartup);
    }

    [Conditional("SIMPLE_PROFILER")]
    public static void End()
    {
      string key = SimpleProfiler.m_Names.Pop();
      float num = Time.realtimeSinceStartup - SimpleProfiler.m_StartTime.Pop();
      if (SimpleProfiler.m_Timers.ContainsKey(key))
      {
        Dictionary<string, float> timers;
        string index;
        (timers = SimpleProfiler.m_Timers)[index = key] = timers[index] + num;
      }
      else
        SimpleProfiler.m_Timers[key] = num;
      if (SimpleProfiler.m_Calls.ContainsKey(key))
      {
        Dictionary<string, int> calls;
        string index;
        (calls = SimpleProfiler.m_Calls)[index = key] = calls[index] + 1;
      }
      else
        SimpleProfiler.m_Calls[key] = 1;
    }

    [Conditional("SIMPLE_PROFILER")]
    public static void PrintTimes()
    {
      string str = "Measured execution times:\n----------------------------\n";
      using (Dictionary<string, float>.Enumerator enumerator = SimpleProfiler.m_Timers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, float> current = enumerator.Current;
          str += string.Format("{0,6:0.0} ms: {1} in {2} calls\n", (object) (float) ((double) current.Value * 1000.0), (object) current.Key, (object) SimpleProfiler.m_Calls[current.Key]);
        }
      }
      UnityEngine.Debug.Log((object) str);
      SimpleProfiler.m_Names.Clear();
      SimpleProfiler.m_StartTime.Clear();
      SimpleProfiler.m_Timers.Clear();
      SimpleProfiler.m_Calls.Clear();
    }
  }
}
