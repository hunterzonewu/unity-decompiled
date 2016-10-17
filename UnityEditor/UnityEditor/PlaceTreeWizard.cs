// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlaceTreeWizard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PlaceTreeWizard : TerrainWizard
  {
    public int numberOfTrees = 10000;
    public bool keepExistingTrees = true;
    private const int kMaxNumberOfTrees = 1000000;

    public void OnEnable()
    {
      this.minSize = new Vector2(250f, 150f);
    }

    private void OnWizardCreate()
    {
      if (this.numberOfTrees > 1000000)
      {
        this.isValid = false;
        this.errorString = string.Format("Mass placing more than {0} trees is not supported", (object) 1000000);
        Debug.LogError((object) this.errorString);
      }
      else
      {
        TreePainter.MassPlaceTrees(this.m_Terrain.terrainData, this.numberOfTrees, true, this.keepExistingTrees);
        this.m_Terrain.Flush();
      }
    }
  }
}
