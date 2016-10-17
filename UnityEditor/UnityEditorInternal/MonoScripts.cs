// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MonoScripts
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  public static class MonoScripts
  {
    public static MonoScript CreateMonoScript(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript)
    {
      MonoScript monoScript = new MonoScript();
      monoScript.Init(scriptContents, className, nameSpace, assemblyName, isEditorScript);
      return monoScript;
    }
  }
}
