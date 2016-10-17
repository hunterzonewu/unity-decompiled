// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  internal struct TimeHelper
  {
    public float deltaTime;
    private long lastTime;

    public void Begin()
    {
      this.lastTime = DateTime.Now.Ticks;
    }

    public float Update()
    {
      this.deltaTime = (float) (DateTime.Now.Ticks - this.lastTime) / 1E+07f;
      this.lastTime = DateTime.Now.Ticks;
      return this.deltaTime;
    }
  }
}
