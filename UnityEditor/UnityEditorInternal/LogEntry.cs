// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.LogEntry
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditorInternal
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class LogEntry
  {
    public string condition;
    public int errorNum;
    public string file;
    public int line;
    public int mode;
    public int instanceID;
    public int identifier;
    public int isWorldPlaying;
  }
}
