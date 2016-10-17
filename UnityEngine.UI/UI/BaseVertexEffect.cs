// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.BaseVertexEffect
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Base class for effects that modify the the generated Vertex Buffers.</para>
  /// </summary>
  [Obsolete("Use BaseMeshEffect instead", true)]
  public abstract class BaseVertexEffect
  {
    [Obsolete("Use BaseMeshEffect.ModifyMeshes instead", true)]
    public abstract void ModifyVertices(List<UIVertex> vertices);
  }
}
