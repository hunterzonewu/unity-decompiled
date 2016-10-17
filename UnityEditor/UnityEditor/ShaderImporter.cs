// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  public sealed class ShaderImporter : AssetImporter
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Shader GetShader();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetDefaultTextures(string[] name, Texture[] textures);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Texture GetDefaultTexture(string name);
  }
}
