// Decompiled with JetBrains decompiler
// Type: UnityEngine.NotificationServices
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("NotificationServices is deprecated. Please use iOS.NotificationServices instead (UnityUpgradable) -> UnityEngine.iOS.NotificationServices", true)]
  public sealed class NotificationServices
  {
    [Obsolete("RegisterForRemoteNotificationTypes is deprecated. Please use RegisterForNotifications instead (UnityUpgradable) -> UnityEngine.iOS.NotificationServices.RegisterForNotifications(*)", true)]
    public static void RegisterForRemoteNotificationTypes(RemoteNotificationType notificationTypes)
    {
    }
  }
}
