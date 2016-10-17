// Decompiled with JetBrains decompiler
// Type: UnityEngine.AccelerationEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Structure describing acceleration status of the device.</para>
  /// </summary>
  public struct AccelerationEvent
  {
    private float x;
    private float y;
    private float z;
    private float m_TimeDelta;

    /// <summary>
    ///   <para>Value of acceleration.</para>
    /// </summary>
    public Vector3 acceleration
    {
      get
      {
        return new Vector3(this.x, this.y, this.z);
      }
    }

    /// <summary>
    ///   <para>Amount of time passed since last accelerometer measurement.</para>
    /// </summary>
    public float deltaTime
    {
      get
      {
        return this.m_TimeDelta;
      }
    }
  }
}
