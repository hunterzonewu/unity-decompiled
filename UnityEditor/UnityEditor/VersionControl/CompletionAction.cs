// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.CompletionAction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Different actions a version control task can do upon completion.</para>
  /// </summary>
  public enum CompletionAction
  {
    UpdatePendingWindow = 1,
    OnChangeContentsPendingWindow = 2,
    OnIncomingPendingWindow = 3,
    OnChangeSetsPendingWindow = 4,
    OnGotLatestPendingWindow = 5,
    OnSubmittedChangeWindow = 6,
    OnAddedChangeWindow = 7,
    OnCheckoutCompleted = 8,
  }
}
