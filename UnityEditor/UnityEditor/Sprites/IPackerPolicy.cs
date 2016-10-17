// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.IPackerPolicy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Sprites
{
  public interface IPackerPolicy
  {
    /// <summary>
    ///   <para>Implement custom atlas grouping here.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="job"></param>
    /// <param name="textureImporterInstanceIDs"></param>
    void OnGroupAtlases(BuildTarget target, PackerJob job, int[] textureImporterInstanceIDs);

    /// <summary>
    ///   <para>Return the version of your policy. Sprite Packer needs to know if atlas grouping logic changed.</para>
    /// </summary>
    int GetVersion();
  }
}
