// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainTreeContextMenus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TerrainTreeContextMenus
  {
    [MenuItem("CONTEXT/TerrainEngineTrees/Add Tree")]
    internal static void AddTree(MenuCommand item)
    {
      TerrainWizard.DisplayTerrainWizard<TreeWizard>("Add Tree", "Add").InitializeDefaults((Terrain) item.context, -1);
    }

    [MenuItem("CONTEXT/TerrainEngineTrees/Edit Tree")]
    internal static void EditTree(MenuCommand item)
    {
      TerrainWizard.DisplayTerrainWizard<TreeWizard>("Edit Tree", "Apply").InitializeDefaults((Terrain) item.context, item.userData);
    }

    [MenuItem("CONTEXT/TerrainEngineTrees/Edit Tree", true)]
    internal static bool EditTreeCheck(MenuCommand item)
    {
      return TreePainter.selectedTree >= 0;
    }

    [MenuItem("CONTEXT/TerrainEngineTrees/Remove Tree")]
    internal static void RemoveTree(MenuCommand item)
    {
      TerrainEditorUtility.RemoveTree((Terrain) item.context, item.userData);
    }

    [MenuItem("CONTEXT/TerrainEngineTrees/Remove Tree", true)]
    internal static bool RemoveTreeCheck(MenuCommand item)
    {
      return TreePainter.selectedTree >= 0;
    }
  }
}
