// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickTimerHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class TickTimerHelper
  {
    private double m_NextTick;
    private double m_Interval;

    public TickTimerHelper(double intervalBetweenTicksInSeconds)
    {
      this.m_Interval = intervalBetweenTicksInSeconds;
    }

    public bool DoTick()
    {
      if (EditorApplication.timeSinceStartup <= this.m_NextTick)
        return false;
      this.m_NextTick = EditorApplication.timeSinceStartup + this.m_Interval;
      return true;
    }

    public void Reset()
    {
      this.m_NextTick = 0.0;
    }
  }
}
