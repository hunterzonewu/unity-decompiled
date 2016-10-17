// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialHeaderDecorator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Globalization;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialHeaderDecorator : MaterialPropertyDrawer
  {
    private readonly string header;

    public MaterialHeaderDecorator(string header)
    {
      this.header = header;
    }

    public MaterialHeaderDecorator(float headerAsNumber)
    {
      this.header = headerAsNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      return 24f;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
      position.y += 8f;
      position = EditorGUI.IndentedRect(position);
      GUI.Label(position, this.header, EditorStyles.boldLabel);
    }
  }
}
