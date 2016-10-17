// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorCache : IDisposable
  {
    private Dictionary<UnityEngine.Object, EditorWrapper> m_EditorCache;
    private Dictionary<UnityEngine.Object, bool> m_UsedEditors;
    private EditorFeatures m_Requirements;

    public EditorWrapper this[UnityEngine.Object o]
    {
      get
      {
        this.m_UsedEditors[o] = true;
        if (this.m_EditorCache.ContainsKey(o))
          return this.m_EditorCache[o];
        EditorWrapper editorWrapper = EditorWrapper.Make(o, this.m_Requirements);
        this.m_EditorCache[o] = editorWrapper;
        return editorWrapper;
      }
    }

    public EditorCache()
      : this(EditorFeatures.None)
    {
    }

    public EditorCache(EditorFeatures requirements)
    {
      this.m_Requirements = requirements;
      this.m_EditorCache = new Dictionary<UnityEngine.Object, EditorWrapper>();
      this.m_UsedEditors = new Dictionary<UnityEngine.Object, bool>();
    }

    ~EditorCache()
    {
      Debug.LogError((object) "Failed to dispose EditorCache.");
    }

    public void CleanupUntouchedEditors()
    {
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      using (Dictionary<UnityEngine.Object, EditorWrapper>.KeyCollection.Enumerator enumerator = this.m_EditorCache.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          UnityEngine.Object current = enumerator.Current;
          if (!this.m_UsedEditors.ContainsKey(current))
            objectList.Add(current);
        }
      }
      if (this.m_EditorCache != null)
      {
        using (List<UnityEngine.Object>.Enumerator enumerator = objectList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UnityEngine.Object current = enumerator.Current;
            EditorWrapper editorWrapper = this.m_EditorCache[current];
            this.m_EditorCache.Remove(current);
            if (editorWrapper != null)
              editorWrapper.Dispose();
          }
        }
      }
      this.m_UsedEditors.Clear();
    }

    public void CleanupAllEditors()
    {
      this.m_UsedEditors.Clear();
      this.CleanupUntouchedEditors();
    }

    public void Dispose()
    {
      this.CleanupAllEditors();
      GC.SuppressFinalize((object) this);
    }
  }
}
