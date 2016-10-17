// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationKeyTime
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal struct AnimationKeyTime
  {
    private float m_FrameRate;
    private int m_Frame;
    private float m_Time;

    public float time
    {
      get
      {
        return this.m_Time;
      }
    }

    public int frame
    {
      get
      {
        return this.m_Frame;
      }
    }

    public float frameRate
    {
      get
      {
        return this.m_FrameRate;
      }
    }

    public float frameFloor
    {
      get
      {
        return ((float) this.frame - 0.5f) / this.frameRate;
      }
    }

    public float frameCeiling
    {
      get
      {
        return ((float) this.frame + 0.5f) / this.frameRate;
      }
    }

    public static AnimationKeyTime Time(float time, float frameRate)
    {
      return new AnimationKeyTime() { m_Time = time, m_FrameRate = frameRate, m_Frame = Mathf.RoundToInt(time * frameRate) };
    }

    public static AnimationKeyTime Frame(int frame, float frameRate)
    {
      return new AnimationKeyTime() { m_Time = (float) frame / frameRate, m_FrameRate = frameRate, m_Frame = frame };
    }

    public bool ContainsTime(float time)
    {
      if ((double) time >= (double) this.frameFloor)
        return (double) time < (double) this.frameCeiling;
      return false;
    }
  }
}
