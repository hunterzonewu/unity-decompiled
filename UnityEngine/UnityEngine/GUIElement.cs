// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIElement
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for images &amp; text strings displayed in a GUI.</para>
  /// </summary>
  public class GUIElement : Behaviour
  {
    /// <summary>
    ///   <para>Is a point on screen inside the element?</para>
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <param name="camera"></param>
    public bool HitTest(Vector3 screenPosition, [DefaultValue("null")] Camera camera)
    {
      return GUIElement.INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
    }

    /// <summary>
    ///   <para>Is a point on screen inside the element?</para>
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <param name="camera"></param>
    [ExcludeFromDocs]
    public bool HitTest(Vector3 screenPosition)
    {
      Camera camera = (Camera) null;
      return GUIElement.INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_HitTest(GUIElement self, ref Vector3 screenPosition, Camera camera);

    /// <summary>
    ///   <para>Returns bounding rectangle of GUIElement in screen coordinates.</para>
    /// </summary>
    /// <param name="camera"></param>
    public Rect GetScreenRect([DefaultValue("null")] Camera camera)
    {
      Rect rect;
      GUIElement.INTERNAL_CALL_GetScreenRect(this, camera, out rect);
      return rect;
    }

    [ExcludeFromDocs]
    public Rect GetScreenRect()
    {
      Rect rect;
      GUIElement.INTERNAL_CALL_GetScreenRect(this, (Camera) null, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetScreenRect(GUIElement self, Camera camera, out Rect value);
  }
}
