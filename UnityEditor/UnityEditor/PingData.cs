// Decompiled with JetBrains decompiler
// Type: UnityEditor.PingData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PingData
  {
    public float m_TimeStart = -1f;
    public float m_ZoomTime = 0.2f;
    public float m_WaitTime = 2.5f;
    public float m_FadeOutTime = 1.5f;
    public float m_PeakScale = 1.75f;
    public float m_AvailableWidth = 100f;
    public System.Action<Rect> m_ContentDraw;
    public Rect m_ContentRect;
    public GUIStyle m_PingStyle;

    public bool isPinging
    {
      get
      {
        return (double) this.m_TimeStart > -1.0;
      }
    }

    public void HandlePing()
    {
      if (!this.isPinging)
        return;
      float num1 = this.m_ZoomTime + this.m_WaitTime + this.m_FadeOutTime;
      float num2 = Time.realtimeSinceStartup - this.m_TimeStart;
      if ((double) num2 > 0.0 && (double) num2 < (double) num1)
      {
        Color color = GUI.color;
        Matrix4x4 matrix1 = GUI.matrix;
        if ((double) num2 < (double) this.m_ZoomTime)
        {
          float num3 = this.m_ZoomTime / 2f;
          float num4 = (float) (((double) this.m_PeakScale - 1.0) * (((double) this.m_ZoomTime - (double) Mathf.Abs(num3 - num2)) / (double) num3 - 1.0) + 1.0);
          Matrix4x4 matrix2 = GUI.matrix;
          Vector2 vector2 = GUIClip.Unclip((double) this.m_ContentRect.xMax >= (double) this.m_AvailableWidth ? new Vector2(this.m_AvailableWidth, this.m_ContentRect.center.y) : this.m_ContentRect.center);
          GUI.matrix = Matrix4x4.TRS((Vector3) vector2, Quaternion.identity, new Vector3(num4, num4, 1f)) * Matrix4x4.TRS((Vector3) (-vector2), Quaternion.identity, Vector3.one) * matrix2;
        }
        else if ((double) num2 > (double) this.m_ZoomTime + (double) this.m_WaitTime)
        {
          float num3 = (num1 - num2) / this.m_FadeOutTime;
          GUI.color = new Color(color.r, color.g, color.b, color.a * num3);
        }
        if (this.m_ContentDraw != null && Event.current.type == EventType.Repaint)
        {
          Rect contentRect = this.m_ContentRect;
          contentRect.x -= (float) this.m_PingStyle.padding.left;
          contentRect.y -= (float) this.m_PingStyle.padding.top;
          this.m_PingStyle.Draw(contentRect, GUIContent.none, false, false, false, false);
          this.m_ContentDraw(this.m_ContentRect);
        }
        GUI.matrix = matrix1;
        GUI.color = color;
      }
      else
        this.m_TimeStart = -1f;
    }
  }
}
