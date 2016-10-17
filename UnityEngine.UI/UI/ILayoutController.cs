// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ILayoutController
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface ILayoutController
  {
    /// <summary>
    ///   <para>Callback invoked by the auto layout system which handles horizontal aspects of the layout.</para>
    /// </summary>
    void SetLayoutHorizontal();

    /// <summary>
    ///   <para>Callback invoked by the auto layout system which handles vertical aspects of the layout.</para>
    /// </summary>
    void SetLayoutVertical();
  }
}
