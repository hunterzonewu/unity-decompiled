// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.CheckoutMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>What to checkout when starting the Checkout task through the version control Provider.</para>
  /// </summary>
  [System.Flags]
  public enum CheckoutMode
  {
    Asset = 1,
    Meta = 2,
    Both = Meta | Asset,
    Exact = 4,
  }
}
