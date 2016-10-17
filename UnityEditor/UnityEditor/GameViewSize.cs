// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSize
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class GameViewSize
  {
    private const int kMaxBaseTextLength = 40;
    private const int kMinResolution = 10;
    private const int kMinAspect = 0;
    private const int kMaxResolutionOrAspect = 99999;
    [SerializeField]
    private string m_BaseText;
    [SerializeField]
    private GameViewSizeType m_SizeType;
    [SerializeField]
    private int m_Width;
    [SerializeField]
    private int m_Height;
    [NonSerialized]
    private string m_CachedDisplayText;

    public string baseText
    {
      get
      {
        return this.m_BaseText;
      }
      set
      {
        this.m_BaseText = value;
        if (this.m_BaseText.Length > 40)
          this.m_BaseText = this.m_BaseText.Substring(0, 40);
        this.Changed();
      }
    }

    public GameViewSizeType sizeType
    {
      get
      {
        return this.m_SizeType;
      }
      set
      {
        this.m_SizeType = value;
        this.Clamp();
        this.Changed();
      }
    }

    public int width
    {
      get
      {
        return this.m_Width;
      }
      set
      {
        this.m_Width = value;
        this.Clamp();
        this.Changed();
      }
    }

    public int height
    {
      get
      {
        return this.m_Height;
      }
      set
      {
        this.m_Height = value;
        this.Clamp();
        this.Changed();
      }
    }

    public bool isFreeAspectRatio
    {
      get
      {
        return this.width == 0;
      }
    }

    public float aspectRatio
    {
      get
      {
        if (this.height == 0)
          return 0.0f;
        return (float) this.width / (float) this.height;
      }
    }

    public string displayText
    {
      get
      {
        return this.m_CachedDisplayText ?? (this.m_CachedDisplayText = this.ComposeDisplayString());
      }
    }

    private string sizeText
    {
      get
      {
        if (this.sizeType == GameViewSizeType.AspectRatio)
          return string.Format("{0}:{1}", (object) this.width, (object) this.height);
        if (this.sizeType == GameViewSizeType.FixedResolution)
          return string.Format("{0}x{1}", (object) this.width, (object) this.height);
        Debug.LogError((object) "Unhandled game view size type");
        return string.Empty;
      }
    }

    public GameViewSize(GameViewSizeType type, int width, int height, string baseText)
    {
      this.sizeType = type;
      this.width = width;
      this.height = height;
      this.baseText = baseText;
    }

    public GameViewSize(GameViewSize other)
    {
      this.Set(other);
    }

    private void Clamp()
    {
      int width = this.m_Width;
      int height = this.m_Height;
      int min = 0;
      switch (this.sizeType)
      {
        case GameViewSizeType.AspectRatio:
          min = 0;
          break;
        case GameViewSizeType.FixedResolution:
          min = 10;
          break;
        default:
          Debug.LogError((object) "Unhandled enum!");
          break;
      }
      this.m_Width = Mathf.Clamp(this.m_Width, min, 99999);
      this.m_Height = Mathf.Clamp(this.m_Height, min, 99999);
      if (this.m_Width == width && this.m_Height == height)
        return;
      this.Changed();
    }

    public void Set(GameViewSize other)
    {
      this.sizeType = other.sizeType;
      this.width = other.width;
      this.height = other.height;
      this.baseText = other.baseText;
      this.Changed();
    }

    private string ComposeDisplayString()
    {
      if (this.width == 0 && this.height == 0)
        return this.baseText;
      if (string.IsNullOrEmpty(this.baseText))
        return this.sizeText;
      return this.baseText + " (" + this.sizeText + ")";
    }

    private void Changed()
    {
      this.m_CachedDisplayText = (string) null;
      ScriptableSingleton<GameViewSizes>.instance.Changed();
    }
  }
}
