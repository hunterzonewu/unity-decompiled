// Decompiled with JetBrains decompiler
// Type: UnityEditor.DelayedCallback
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class DelayedCallback
  {
    private System.Action m_Callback;
    private double m_CallbackTime;

    public DelayedCallback(System.Action function, double timeFromNow)
    {
      this.m_Callback = function;
      this.m_CallbackTime = EditorApplication.timeSinceStartup + timeFromNow;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
    }

    public void Clear()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      this.m_CallbackTime = 0.0;
      this.m_Callback = (System.Action) null;
    }

    private void Update()
    {
      if (EditorApplication.timeSinceStartup <= this.m_CallbackTime)
        return;
      System.Action callback = this.m_Callback;
      this.Clear();
      callback();
    }
  }
}
