// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IMeshModifier
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;

namespace UnityEngine.UI
{
  public interface IMeshModifier
  {
    /// <summary>
    ///   <para>Call used to modify mesh.</para>
    /// </summary>
    /// <param name="mesh"></param>
    [Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts) instead", false)]
    void ModifyMesh(Mesh mesh);

    void ModifyMesh(VertexHelper verts);
  }
}
