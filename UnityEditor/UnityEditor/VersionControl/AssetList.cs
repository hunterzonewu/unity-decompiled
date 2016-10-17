// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.AssetList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>A list of version control information about assets.</para>
  /// </summary>
  public class AssetList : List<Asset>
  {
    public AssetList()
    {
    }

    public AssetList(AssetList src)
    {
    }

    public AssetList Filter(bool includeFolder, params Asset.States[] states)
    {
      AssetList assetList = new AssetList();
      if (!includeFolder && (states == null || states.Length == 0))
        return assetList;
      using (List<Asset>.Enumerator enumerator = this.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Asset current = enumerator.Current;
          if (current.isFolder)
          {
            if (includeFolder)
              assetList.Add(current);
          }
          else if (current.IsOneOfStates(states))
            assetList.Add(current);
        }
      }
      return assetList;
    }

    public int FilterCount(bool includeFolder, params Asset.States[] states)
    {
      int num = 0;
      if (!includeFolder && states == null)
        return this.Count;
      using (List<Asset>.Enumerator enumerator = this.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Asset current = enumerator.Current;
          if (current.isFolder)
            ++num;
          else if (current.IsOneOfStates(states))
            ++num;
        }
      }
      return num;
    }

    /// <summary>
    ///   <para>Create an optimised list of assets by removing children of folders in the same list.</para>
    /// </summary>
    public AssetList FilterChildren()
    {
      AssetList assetList = new AssetList();
      assetList.AddRange((IEnumerable<Asset>) this);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetList.\u003CFilterChildren\u003Ec__AnonStoreyC0 childrenCAnonStoreyC0 = new AssetList.\u003CFilterChildren\u003Ec__AnonStoreyC0();
      using (List<Asset>.Enumerator enumerator = this.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          // ISSUE: reference to a compiler-generated field
          childrenCAnonStoreyC0.asset = enumerator.Current;
          // ISSUE: reference to a compiler-generated method
          assetList.RemoveAll(new Predicate<Asset>(childrenCAnonStoreyC0.\u003C\u003Em__22F));
        }
      }
      return assetList;
    }
  }
}
