// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlattenHeightmap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FlattenHeightmap : TerrainWizard
  {
    public float height;

    internal override void OnWizardUpdate()
    {
      if (!(bool) ((Object) this.terrainData))
        return;
      this.helpString = ((double) this.height).ToString() + " meters (" + (object) (float) ((double) this.height / (double) this.terrainData.size.y * 100.0) + "%)";
    }

    private void OnWizardCreate()
    {
      Undo.RegisterCompleteObjectUndo((Object) this.terrainData, "Flatten Heightmap");
      HeightmapFilters.Flatten(this.terrainData, this.height / this.terrainData.size.y);
    }
  }
}
