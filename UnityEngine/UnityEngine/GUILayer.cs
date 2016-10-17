// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Component added to a camera to make it render 2D GUI elements.</para>
  /// </summary>
  public sealed class GUILayer : Behaviour
  {
    /// <summary>
    ///   <para>Get the GUI element at a specific screen position.</para>
    /// </summary>
    /// <param name="screenPosition"></param>
    public GUIElement HitTest(Vector3 screenPosition)
    {
      return GUILayer.INTERNAL_CALL_HitTest(this, ref screenPosition);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GUIElement INTERNAL_CALL_HitTest(GUILayer self, ref Vector3 screenPosition);
  }
}
