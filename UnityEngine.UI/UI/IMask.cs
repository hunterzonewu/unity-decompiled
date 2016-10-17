// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IMask
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;

namespace UnityEngine.UI
{
  [Obsolete("Not supported anymore.", true)]
  public interface IMask
  {
    /// <summary>
    ///   <para>Return the RectTransform associated with this mask.</para>
    /// </summary>
    RectTransform rectTransform { get; }

    /// <summary>
    ///   <para>Is the mask enabled.</para>
    /// </summary>
    bool Enabled();
  }
}
