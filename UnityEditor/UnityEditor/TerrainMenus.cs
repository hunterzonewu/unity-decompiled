// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainMenus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TerrainMenus
  {
    [MenuItem("GameObject/3D Object/Terrain", false, 3000)]
    private static void CreateTerrain(MenuCommand menuCommand)
    {
      TerrainData assignTerrain = new TerrainData();
      assignTerrain.heightmapResolution = 1025;
      assignTerrain.size = new Vector3(1000f, 600f, 1000f);
      assignTerrain.heightmapResolution = 512;
      assignTerrain.baseMapResolution = 1024;
      assignTerrain.SetDetailResolution(1024, assignTerrain.detailResolutionPerPatch);
      AssetDatabase.CreateAsset((Object) assignTerrain, AssetDatabase.GenerateUniqueAssetPath("Assets/New Terrain.asset"));
      GameObject terrainGameObject = Terrain.CreateTerrainGameObject(assignTerrain);
      GameObjectUtility.SetParentAndAlign(terrainGameObject, menuCommand.context as GameObject);
      Selection.activeObject = (Object) terrainGameObject;
      Undo.RegisterCreatedObjectUndo((Object) terrainGameObject, "Create terrain");
    }

    internal static void ImportRaw()
    {
      string path = EditorUtility.OpenFilePanel("Import Raw Heightmap", string.Empty, "raw");
      if (!(path != string.Empty))
        return;
      TerrainWizard.DisplayTerrainWizard<ImportRawHeightmap>("Import Heightmap", "Import").InitializeImportRaw(TerrainMenus.GetActiveTerrain(), path);
    }

    internal static void ExportHeightmapRaw()
    {
      ((TerrainWizard) TerrainWizard.DisplayTerrainWizard<ExportRawHeightmap>("Export Heightmap", "Export")).InitializeDefaults(TerrainMenus.GetActiveTerrain());
    }

    internal static void MassPlaceTrees()
    {
      TerrainWizard.DisplayTerrainWizard<PlaceTreeWizard>("Place Trees", "Place").InitializeDefaults(TerrainMenus.GetActiveTerrain());
    }

    internal static void Flatten()
    {
      TerrainWizard.DisplayTerrainWizard<FlattenHeightmap>("Flatten Heightmap", "Flatten").InitializeDefaults(TerrainMenus.GetActiveTerrain());
    }

    internal static void RefreshPrototypes()
    {
      TerrainMenus.GetActiveTerrainData().RefreshPrototypes();
      TerrainMenus.GetActiveTerrain().Flush();
      EditorApplication.SetSceneRepaintDirty();
    }

    private static void FlushHeightmapModification()
    {
      TerrainMenus.GetActiveTerrain().Flush();
    }

    private static Terrain GetActiveTerrain()
    {
      Object[] filtered = Selection.GetFiltered(typeof (Terrain), SelectionMode.Editable);
      if (filtered.Length != 0)
        return filtered[0] as Terrain;
      return Terrain.activeTerrain;
    }

    private static TerrainData GetActiveTerrainData()
    {
      if ((bool) ((Object) TerrainMenus.GetActiveTerrain()))
        return TerrainMenus.GetActiveTerrain().terrainData;
      return (TerrainData) null;
    }
  }
}
