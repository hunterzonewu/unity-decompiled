// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Select this to set basic parameters depending on the purpose of your texture.</para>
  /// </summary>
  public enum TextureImporterType
  {
    Image = 0,
    Bump = 1,
    GUI = 2,
    Cubemap = 3,
    [Obsolete("Use Cubemap")] Reflection = 3,
    Cookie = 4,
    Advanced = 5,
    Lightmap = 6,
    Cursor = 7,
    Sprite = 8,
  }
}
