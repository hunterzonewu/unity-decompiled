// Decompiled with JetBrains decompiler
// Type: UnityEngine.Internal_DrawWithTextSelectionArguments
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal struct Internal_DrawWithTextSelectionArguments
  {
    public IntPtr target;
    public Rect position;
    public int firstPos;
    public int lastPos;
    public Color cursorColor;
    public Color selectionColor;
    public int isHover;
    public int isActive;
    public int on;
    public int hasKeyboardFocus;
    public int drawSelectionAsComposition;
  }
}
