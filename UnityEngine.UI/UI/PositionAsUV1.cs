// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.PositionAsUV1
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>An IVertexModifier which sets the raw vertex position into UV1 of the generated verts.</para>
  /// </summary>
  [AddComponentMenu("UI/Effects/Position As UV1", 16)]
  public class PositionAsUV1 : BaseMeshEffect
  {
    protected PositionAsUV1()
    {
    }

    public override void ModifyMesh(VertexHelper vh)
    {
      UIVertex vertex = new UIVertex();
      for (int i = 0; i < vh.currentVertCount; ++i)
      {
        vh.PopulateUIVertex(ref vertex, i);
        vertex.uv1 = new Vector2(vertex.position.x, vertex.position.y);
        vh.SetUIVertex(vertex, i);
      }
    }
  }
}
