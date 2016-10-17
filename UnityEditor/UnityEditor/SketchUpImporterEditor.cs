// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImporterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SketchUpImporter))]
  internal class SketchUpImporterEditor : ModelImporterEditor
  {
    internal override bool showImportedObject
    {
      get
      {
        return this.activeEditor is SketchUpImporterModelEditor;
      }
    }

    internal override void OnEnable()
    {
      if (this.m_SubEditorTypes == null)
      {
        this.m_SubEditorTypes = new System.Type[3]
        {
          typeof (SketchUpImporterModelEditor),
          typeof (ModelImporterRigEditor),
          typeof (ModelImporterClipEditor)
        };
        this.m_SubEditorNames = new string[3]
        {
          "Sketch Up",
          "Rig",
          "Animations"
        };
      }
      base.OnEnable();
    }
  }
}
