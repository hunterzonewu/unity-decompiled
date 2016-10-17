// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialPowerSliderDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaterialPowerSliderDrawer : MaterialPropertyDrawer
  {
    private readonly float power;

    public MaterialPowerSliderDrawer(float power)
    {
      this.power = power;
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (prop.type != MaterialProperty.PropType.Range)
        return 40f;
      return base.GetPropertyHeight(prop, label, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (prop.type != MaterialProperty.PropType.Range)
      {
        GUIContent label1 = EditorGUIUtility.TempContent("PowerSlider used on a non-range property: " + prop.name, (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        EditorGUI.LabelField(position, label1, EditorStyles.helpBox);
      }
      else
      {
        double num = (double) MaterialEditor.DoPowerRangeProperty(position, prop, label, this.power);
      }
    }
  }
}
