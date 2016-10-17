// Decompiled with JetBrains decompiler
// Type: UnityEditor.IHierarchyProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface IHierarchyProperty
  {
    int instanceID { get; }

    Object pptrValue { get; }

    string name { get; }

    bool hasChildren { get; }

    int depth { get; }

    int row { get; }

    int colorCode { get; }

    string guid { get; }

    Texture2D icon { get; }

    bool isValid { get; }

    bool isMainRepresentation { get; }

    bool hasFullPreviewImage { get; }

    IconDrawStyle iconDrawStyle { get; }

    bool isFolder { get; }

    int[] ancestors { get; }

    void Reset();

    bool IsExpanded(int[] expanded);

    bool Next(int[] expanded);

    bool NextWithDepthCheck(int[] expanded, int minDepth);

    bool Previous(int[] expanded);

    bool Parent();

    bool Find(int instanceID, int[] expanded);

    int[] FindAllAncestors(int[] instanceIDs);

    bool Skip(int count, int[] expanded);

    int CountRemaining(int[] expanded);
  }
}
