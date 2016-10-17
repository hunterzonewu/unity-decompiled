// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GradientContextMenu
  {
    private SerializedProperty m_Prop1;

    private GradientContextMenu(SerializedProperty prop1)
    {
      this.m_Prop1 = prop1;
    }

    internal static void Show(SerializedProperty prop)
    {
      GUIContent content1 = new GUIContent("Copy");
      GUIContent content2 = new GUIContent("Paste");
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(content1, false, new GenericMenu.MenuFunction(new GradientContextMenu(prop).Copy));
      if (ParticleSystemClipboard.HasSingleGradient())
        genericMenu.AddItem(content2, false, new GenericMenu.MenuFunction(new GradientContextMenu(prop).Paste));
      else
        genericMenu.AddDisabledItem(content2);
      genericMenu.ShowAsContext();
    }

    private void Copy()
    {
      ParticleSystemClipboard.CopyGradient(this.m_Prop1 == null ? (Gradient) null : this.m_Prop1.gradientValue, (Gradient) null);
    }

    private void Paste()
    {
      ParticleSystemClipboard.PasteGradient(this.m_Prop1, (SerializedProperty) null);
      GradientPreviewCache.ClearCache();
    }
  }
}
