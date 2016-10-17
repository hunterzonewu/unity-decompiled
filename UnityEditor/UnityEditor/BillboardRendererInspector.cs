// Decompiled with JetBrains decompiler
// Type: UnityEditor.BillboardRendererInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (BillboardRenderer))]
  internal class BillboardRendererInspector : RendererEditorBase
  {
    private string[] m_ExcludedProperties;

    public override void OnEnable()
    {
      base.OnEnable();
      this.InitializeProbeFields();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) new string[2]
      {
        "m_Materials",
        "m_LightmapParameters"
      });
      stringList.AddRange((IEnumerable<string>) RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_ExcludedProperties = stringList.ToArray();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Editor.DrawPropertiesExcluding(this.serializedObject, this.m_ExcludedProperties);
      this.RenderProbeFields();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
