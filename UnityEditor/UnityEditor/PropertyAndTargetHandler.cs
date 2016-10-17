// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyAndTargetHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PropertyAndTargetHandler
  {
    public SerializedProperty property;
    public Object target;
    public TargetChoiceHandler.TargetChoiceMenuFunction function;

    public PropertyAndTargetHandler(SerializedProperty property, Object target, TargetChoiceHandler.TargetChoiceMenuFunction function)
    {
      this.property = property;
      this.target = target;
      this.function = function;
    }
  }
}
