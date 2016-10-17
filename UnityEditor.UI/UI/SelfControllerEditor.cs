// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.SelfControllerEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Base class for custom editors that are for components that implement the SelfControllerEditor interface.</para>
  /// </summary>
  public class SelfControllerEditor : Editor
  {
    private static string s_Warning = "Parent has a type of layout group component. A child of a layout group should not have a {0} component, since it should be driven by the layout group.";

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      bool flag = false;
      for (int index = 0; index < this.targets.Length; ++index)
      {
        Component target = this.targets[index] as Component;
        ILayoutIgnorer component1 = target.GetComponent(typeof (ILayoutIgnorer)) as ILayoutIgnorer;
        if (component1 == null || !component1.ignoreLayout)
        {
          RectTransform parent = target.transform.parent as RectTransform;
          if ((Object) parent != (Object) null)
          {
            Behaviour component2 = parent.GetComponent(typeof (ILayoutGroup)) as Behaviour;
            if ((Object) component2 != (Object) null && component2.enabled)
            {
              flag = true;
              break;
            }
          }
        }
      }
      if (!flag)
        return;
      EditorGUILayout.HelpBox(string.Format(SelfControllerEditor.s_Warning, (object) ObjectNames.NicifyVariableName(this.target.GetType().Name)), MessageType.Warning);
    }
  }
}
