// Decompiled with JetBrains decompiler
// Type: UnityEditor.ChangeFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  [System.Flags]
  internal enum ChangeFlags
  {
    None = 0,
    Modified = 1,
    Renamed = 2,
    Moved = 4,
    Deleted = 8,
    Undeleted = 16,
    Created = 32,
  }
}
