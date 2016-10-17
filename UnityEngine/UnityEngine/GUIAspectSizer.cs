// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIAspectSizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  internal sealed class GUIAspectSizer : GUILayoutEntry
  {
    private float aspect;

    public GUIAspectSizer(float aspect, GUILayoutOption[] options)
      : base(0.0f, 0.0f, 0.0f, 0.0f, GUIStyle.none)
    {
      this.aspect = aspect;
      this.ApplyOptions(options);
    }

    public override void CalcHeight()
    {
      this.minHeight = this.maxHeight = this.rect.width / this.aspect;
    }
  }
}
