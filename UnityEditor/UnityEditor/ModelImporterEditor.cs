// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (ModelImporter))]
  internal class ModelImporterEditor : AssetImporterTabbedEditor
  {
    internal override bool showImportedObject
    {
      get
      {
        return this.activeEditor is ModelImporterModelEditor;
      }
    }

    protected override bool useAssetDrawPreview
    {
      get
      {
        return false;
      }
    }

    internal override void OnEnable()
    {
      if (this.m_SubEditorTypes == null)
      {
        this.m_SubEditorTypes = new System.Type[3]
        {
          typeof (ModelImporterModelEditor),
          typeof (ModelImporterRigEditor),
          typeof (ModelImporterClipEditor)
        };
        this.m_SubEditorNames = new string[3]
        {
          "Model",
          "Rig",
          "Animations"
        };
      }
      base.OnEnable();
    }

    public override bool HasPreviewGUI()
    {
      if (base.HasPreviewGUI())
        return this.targets.Length < 2;
      return false;
    }
  }
}
