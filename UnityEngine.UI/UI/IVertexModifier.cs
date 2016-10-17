// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IVertexModifier
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  [Obsolete("Use IMeshModifier instead", true)]
  public interface IVertexModifier
  {
    [Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts)  instead", true)]
    void ModifyVertices(List<UIVertex> verts);
  }
}
