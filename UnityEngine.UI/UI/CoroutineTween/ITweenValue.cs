// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.CoroutineTween.ITweenValue
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI.CoroutineTween
{
  internal interface ITweenValue
  {
    bool ignoreTimeScale { get; }

    float duration { get; }

    void TweenValue(float floatPercentage);

    bool ValidTarget();
  }
}
