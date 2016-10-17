// Decompiled with JetBrains decompiler
// Type: UnityEditor.UndoPropertyModification
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>See Also: Undo.postprocessModifications.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct UndoPropertyModification
  {
    public PropertyModification previousValue;
    public PropertyModification currentValue;
    private int m_KeepPrefabOverride;

    public bool keepPrefabOverride
    {
      get
      {
        return this.m_KeepPrefabOverride != 0;
      }
      set
      {
        this.m_KeepPrefabOverride = !value ? 0 : 1;
      }
    }
  }
}
