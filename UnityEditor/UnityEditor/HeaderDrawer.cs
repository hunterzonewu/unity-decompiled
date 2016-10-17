// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeaderDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (HeaderAttribute))]
  internal sealed class HeaderDrawer : DecoratorDrawer
  {
    public override void OnGUI(Rect position)
    {
      position.y += 8f;
      position = EditorGUI.IndentedRect(position);
      GUI.Label(position, (this.attribute as HeaderAttribute).header, EditorStyles.boldLabel);
    }

    public override float GetHeight()
    {
      return 24f;
    }
  }
}
