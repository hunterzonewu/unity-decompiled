// Decompiled with JetBrains decompiler
// Type: UnityEditor.FontInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Font))]
  [CanEditMultipleObjects]
  internal class FontInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      foreach (Object target in this.targets)
      {
        if (target.hideFlags == HideFlags.NotEditable)
          return;
      }
      this.DrawDefaultInspector();
    }
  }
}
