// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizesMenuItemProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class GameViewSizesMenuItemProvider : IFlexibleMenuItemProvider
  {
    private readonly GameViewSizeGroup m_GameViewSizeGroup;

    public GameViewSizesMenuItemProvider(GameViewSizeGroupType gameViewSizeGroupType)
    {
      this.m_GameViewSizeGroup = ScriptableSingleton<GameViewSizes>.instance.GetGroup(gameViewSizeGroupType);
    }

    public int Count()
    {
      return this.m_GameViewSizeGroup.GetTotalCount();
    }

    public object GetItem(int index)
    {
      return (object) this.m_GameViewSizeGroup.GetGameViewSize(index);
    }

    public int Add(object obj)
    {
      GameViewSize gameViewSize = GameViewSizesMenuItemProvider.CastToGameViewSize(obj);
      if (gameViewSize == null)
        return -1;
      this.m_GameViewSizeGroup.AddCustomSize(gameViewSize);
      ScriptableSingleton<GameViewSizes>.instance.SaveToHDD();
      return this.Count() - 1;
    }

    public void Replace(int index, object obj)
    {
      GameViewSize gameViewSize1 = GameViewSizesMenuItemProvider.CastToGameViewSize(obj);
      if (gameViewSize1 == null)
        return;
      if (index < this.m_GameViewSizeGroup.GetBuiltinCount())
      {
        Debug.LogError((object) "Only custom game view sizes can be changed");
      }
      else
      {
        GameViewSize gameViewSize2 = this.m_GameViewSizeGroup.GetGameViewSize(index);
        if (gameViewSize2 == null)
          return;
        gameViewSize2.Set(gameViewSize1);
        ScriptableSingleton<GameViewSizes>.instance.SaveToHDD();
      }
    }

    public void Remove(int index)
    {
      if (index < this.m_GameViewSizeGroup.GetBuiltinCount())
      {
        Debug.LogError((object) "Only custom game view sizes can be changed");
      }
      else
      {
        this.m_GameViewSizeGroup.RemoveCustomSize(index);
        ScriptableSingleton<GameViewSizes>.instance.SaveToHDD();
      }
    }

    public object Create()
    {
      return (object) new GameViewSize(GameViewSizeType.FixedResolution, 0, 0, string.Empty);
    }

    public void Move(int index, int destIndex, bool insertAfterDestIndex)
    {
      Debug.LogError((object) "Missing impl");
    }

    public string GetName(int index)
    {
      GameViewSize gameViewSize = this.m_GameViewSizeGroup.GetGameViewSize(index);
      if (gameViewSize != null)
        return gameViewSize.displayText;
      return string.Empty;
    }

    public bool IsModificationAllowed(int index)
    {
      return this.m_GameViewSizeGroup.IsCustomSize(index);
    }

    public int[] GetSeperatorIndices()
    {
      return new int[1]{ this.m_GameViewSizeGroup.GetBuiltinCount() - 1 };
    }

    private static GameViewSize CastToGameViewSize(object obj)
    {
      GameViewSize gameViewSize = obj as GameViewSize;
      if (obj != null)
        return gameViewSize;
      Debug.LogError((object) "Incorrect input");
      return (GameViewSize) null;
    }
  }
}
