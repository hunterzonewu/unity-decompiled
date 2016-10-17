// Decompiled with JetBrains decompiler
// Type: UnityEngine.ICanvasRaycastFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  public interface ICanvasRaycastFilter
  {
    /// <summary>
    ///   <para>Given a point and a camera is the raycast valid.</para>
    /// </summary>
    /// <param name="sp">Screen position.</param>
    /// <param name="eventCamera">Raycast camera.</param>
    /// <returns>
    ///   <para>Valid.</para>
    /// </returns>
    bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera);
  }
}
