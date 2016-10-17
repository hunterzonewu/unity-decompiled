// Decompiled with JetBrains decompiler
// Type: UnityEngine.ShaderVariantCollection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
  /// <summary>
  ///   <para>ShaderVariantCollection records which shader variants are actually used in each shader.</para>
  /// </summary>
  public sealed class ShaderVariantCollection : Object
  {
    /// <summary>
    ///   <para>Number of shaders in this collection (Read Only).</para>
    /// </summary>
    public int shaderCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of total varians in this collection (Read Only).</para>
    /// </summary>
    public int variantCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is this ShaderVariantCollection already warmed up? (Read Only)</para>
    /// </summary>
    public bool isWarmedUp { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Create a new empty shader variant collection.</para>
    /// </summary>
    public ShaderVariantCollection()
    {
      ShaderVariantCollection.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] ShaderVariantCollection mono);

    public bool Add(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.AddInternal(variant.shader, variant.passType, variant.keywords);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool AddInternal(Shader shader, PassType passType, string[] keywords);

    public bool Remove(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.RemoveInternal(variant.shader, variant.passType, variant.keywords);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool RemoveInternal(Shader shader, PassType passType, string[] keywords);

    public bool Contains(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.ContainsInternal(variant.shader, variant.passType, variant.keywords);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool ContainsInternal(Shader shader, PassType passType, string[] keywords);

    /// <summary>
    ///   <para>Remove all shader variants from the collection.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Clear();

    /// <summary>
    ///   <para>Fully load shaders in ShaderVariantCollection.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void WarmUp();

    /// <summary>
    ///   <para>Identifies a specific variant of a shader.</para>
    /// </summary>
    public struct ShaderVariant
    {
      /// <summary>
      ///   <para>Shader to use in this variant.</para>
      /// </summary>
      public Shader shader;
      /// <summary>
      ///   <para>Pass type to use in this variant.</para>
      /// </summary>
      public PassType passType;
      /// <summary>
      ///   <para>Array of shader keywords to use in this variant.</para>
      /// </summary>
      public string[] keywords;

      /// <summary>
      ///   <para>Creates a ShaderVariant structure.</para>
      /// </summary>
      /// <param name="shader"></param>
      /// <param name="passType"></param>
      /// <param name="keywords"></param>
      public ShaderVariant(Shader shader, PassType passType, params string[] keywords)
      {
        this.shader = shader;
        this.passType = passType;
        this.keywords = keywords;
        ShaderVariantCollection.ShaderVariant.Internal_CheckVariant(shader, passType, keywords);
      }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void Internal_CheckVariant(Shader shader, PassType passType, string[] keywords);
    }
  }
}
