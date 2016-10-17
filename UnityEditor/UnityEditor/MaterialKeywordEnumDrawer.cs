// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialKeywordEnumDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaterialKeywordEnumDrawer : MaterialPropertyDrawer
  {
    private readonly string[] keywords;

    public MaterialKeywordEnumDrawer(string kw1)
      : this(new string[1]{ kw1 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2)
      : this(new string[2]{ kw1, kw2 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3)
      : this(new string[3]{ kw1, kw2, kw3 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4)
      : this(new string[4]{ kw1, kw2, kw3, kw4 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4, string kw5)
      : this(new string[5]{ kw1, kw2, kw3, kw4, kw5 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6)
      : this(new string[6]{ kw1, kw2, kw3, kw4, kw5, kw6 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7)
      : this(new string[7]{ kw1, kw2, kw3, kw4, kw5, kw6, kw7 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8)
      : this(new string[8]{ kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8 })
    {
    }

    public MaterialKeywordEnumDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8, string kw9)
      : this(new string[9]{ kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8, kw9 })
    {
    }

    public MaterialKeywordEnumDrawer(params string[] keywords)
    {
      this.keywords = keywords;
    }

    private static bool IsPropertyTypeSuitable(MaterialProperty prop)
    {
      if (prop.type != MaterialProperty.PropType.Float)
        return prop.type == MaterialProperty.PropType.Range;
      return true;
    }

    private void SetKeyword(MaterialProperty prop, int index)
    {
      for (int index1 = 0; index1 < this.keywords.Length; ++index1)
      {
        string keywordName = MaterialKeywordEnumDrawer.GetKeywordName(prop.name, this.keywords[index1]);
        foreach (Material target in prop.targets)
        {
          if (index == index1)
            target.EnableKeyword(keywordName);
          else
            target.DisableKeyword(keywordName);
        }
      }
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (!MaterialKeywordEnumDrawer.IsPropertyTypeSuitable(prop))
        return 40f;
      return base.GetPropertyHeight(prop, label, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (!MaterialKeywordEnumDrawer.IsPropertyTypeSuitable(prop))
      {
        GUIContent label1 = EditorGUIUtility.TempContent("KeywordEnum used on a non-float property: " + prop.name, (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        EditorGUI.LabelField(position, label1, EditorStyles.helpBox);
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        int floatValue = (int) prop.floatValue;
        int index = EditorGUI.Popup(position, label, floatValue, this.keywords);
        EditorGUI.showMixedValue = false;
        if (!EditorGUI.EndChangeCheck())
          return;
        prop.floatValue = (float) index;
        this.SetKeyword(prop, index);
      }
    }

    public override void Apply(MaterialProperty prop)
    {
      base.Apply(prop);
      if (!MaterialKeywordEnumDrawer.IsPropertyTypeSuitable(prop) || prop.hasMixedValue)
        return;
      this.SetKeyword(prop, (int) prop.floatValue);
    }

    private static string GetKeywordName(string propName, string name)
    {
      return (propName + "_" + name).Replace(' ', '_').ToUpperInvariant();
    }
  }
}
