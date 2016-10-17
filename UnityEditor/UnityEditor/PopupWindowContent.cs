// Decompiled with JetBrains decompiler
// Type: UnityEditor.PopupWindowContent
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Class used to implement content for a popup window.</para>
  /// </summary>
  public abstract class PopupWindowContent
  {
    /// <summary>
    ///   <para>The EditorWindow that contains the popup content.</para>
    /// </summary>
    public EditorWindow editorWindow { get; internal set; }

    /// <summary>
    ///   <para>Callback for drawing GUI controls for the popup window.</para>
    /// </summary>
    /// <param name="rect">The rectangle to draw the GUI inside.</param>
    public abstract void OnGUI(Rect rect);

    /// <summary>
    ///   <para>The size of the popup window.</para>
    /// </summary>
    /// <returns>
    ///   <para>The size of the Popup window.</para>
    /// </returns>
    public virtual Vector2 GetWindowSize()
    {
      return new Vector2(200f, 200f);
    }

    /// <summary>
    ///   <para>Callback when the popup window is opened.</para>
    /// </summary>
    public virtual void OnOpen()
    {
    }

    /// <summary>
    ///   <para>Callback when the popup window is closed.</para>
    /// </summary>
    public virtual void OnClose()
    {
    }
  }
}
