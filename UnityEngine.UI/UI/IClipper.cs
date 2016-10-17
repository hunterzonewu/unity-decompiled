// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IClipper
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface IClipper
  {
    /// <summary>
    ///   <para>Called after layout and before Graphic update of the Canvas update loop.</para>
    /// </summary>
    void PerformClipping();
  }
}
