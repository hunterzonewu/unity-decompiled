// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.PackerJob
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Sprites
{
  /// <summary>
  ///   <para>Current Sprite Packer job definition.</para>
  /// </summary>
  public sealed class PackerJob
  {
    internal PackerJob()
    {
    }

    /// <summary>
    ///   <para>Registers a new atlas.</para>
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="settings"></param>
    public void AddAtlas(string atlasName, AtlasSettings settings)
    {
      this.AddAtlas_Internal(atlasName, ref settings);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddAtlas_Internal(string atlasName, ref AtlasSettings settings);

    /// <summary>
    ///   <para>Assigns a Sprite to an already registered atlas.</para>
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="sprite"></param>
    /// <param name="packingMode"></param>
    /// <param name="packingRotation"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void AssignToAtlas(string atlasName, Sprite sprite, SpritePackingMode packingMode, SpritePackingRotation packingRotation);
  }
}
