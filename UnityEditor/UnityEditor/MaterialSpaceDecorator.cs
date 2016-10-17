// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialSpaceDecorator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaterialSpaceDecorator : MaterialPropertyDrawer
  {
    private readonly float height;

    public MaterialSpaceDecorator()
    {
      this.height = 6f;
    }

    public MaterialSpaceDecorator(float height)
    {
      this.height = height;
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      return this.height;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
    }
  }
}
