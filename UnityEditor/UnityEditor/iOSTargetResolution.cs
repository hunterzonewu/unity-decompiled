// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSTargetResolution
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Obsolete("iOSTargetResolution is ignored, use Screen.SetResolution APIs")]
  public enum iOSTargetResolution
  {
    Native = 0,
    ResolutionAutoPerformance = 3,
    ResolutionAutoQuality = 4,
    Resolution320p = 5,
    Resolution640p = 6,
    Resolution768p = 7,
  }
}
