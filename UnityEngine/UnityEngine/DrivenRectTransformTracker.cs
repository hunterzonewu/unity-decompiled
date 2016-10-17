// Decompiled with JetBrains decompiler
// Type: UnityEngine.DrivenRectTransformTracker
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A component can be designed drive a RectTransform. The DrivenRectTransformTracker struct is used to specify which RectTransforms it is driving.</para>
  /// </summary>
  public struct DrivenRectTransformTracker
  {
    private List<RectTransform> m_Tracked;

    /// <summary>
    ///   <para>Add a RectTransform to be driven.</para>
    /// </summary>
    /// <param name="driver">The object to drive properties.</param>
    /// <param name="rectTransform">The RectTransform to be driven.</param>
    /// <param name="drivenProperties">The properties to be driven.</param>
    public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
    {
      if (this.m_Tracked == null)
        this.m_Tracked = new List<RectTransform>();
      rectTransform.drivenByObject = driver;
      rectTransform.drivenProperties = rectTransform.drivenProperties | drivenProperties;
      if (!Application.isPlaying)
        RuntimeUndo.RecordObject((Object) rectTransform, "Driving RectTransform");
      this.m_Tracked.Add(rectTransform);
    }

    /// <summary>
    ///   <para>Clear the list of RectTransforms being driven.</para>
    /// </summary>
    public void Clear()
    {
      if (this.m_Tracked == null)
        return;
      for (int index = 0; index < this.m_Tracked.Count; ++index)
      {
        if ((Object) this.m_Tracked[index] != (Object) null)
        {
          if (!Application.isPlaying)
            RuntimeUndo.RecordObject((Object) this.m_Tracked[index], "Driving RectTransform");
          this.m_Tracked[index].drivenByObject = (Object) null;
        }
      }
      this.m_Tracked.Clear();
    }
  }
}
