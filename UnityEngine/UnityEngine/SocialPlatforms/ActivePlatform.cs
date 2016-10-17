// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.ActivePlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.SocialPlatforms
{
  internal static class ActivePlatform
  {
    private static ISocialPlatform _active;

    internal static ISocialPlatform Instance
    {
      get
      {
        if (ActivePlatform._active == null)
          ActivePlatform._active = ActivePlatform.SelectSocialPlatform();
        return ActivePlatform._active;
      }
      set
      {
        ActivePlatform._active = value;
      }
    }

    private static ISocialPlatform SelectSocialPlatform()
    {
      return (ISocialPlatform) new Local();
    }
  }
}
