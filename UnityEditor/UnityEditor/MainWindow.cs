// Decompiled with JetBrains decompiler
// Type: UnityEditor.MainWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class MainWindow : View, ICleanuppable
  {
    private const float kStatusbarHeight = 20f;
    private const float kMinWidth = 770f;

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      if (this.children.Length == 0)
        return;
      Toolbar child = (Toolbar) this.children[0];
      this.children[0].position = new Rect(0.0f, 0.0f, newPos.width, child.CalcHeight());
      if (this.children.Length <= 2)
        return;
      this.children[1].position = new Rect(0.0f, child.CalcHeight(), newPos.width, newPos.height - child.CalcHeight() - this.children[2].position.height);
      this.children[2].position = new Rect(0.0f, newPos.height - this.children[2].position.height, newPos.width, this.children[2].position.height);
    }

    protected override void ChildrenMinMaxChanged()
    {
      if (this.children.Length == 3)
        this.SetMinMaxSizes(new Vector2(770f, ((Toolbar) this.children[0]).CalcHeight() + 20f + this.children[1].minSize.y), new Vector2(10000f, 10000f));
      base.ChildrenMinMaxChanged();
    }

    public static void MakeMain()
    {
      ContainerWindow instance1 = ScriptableObject.CreateInstance<ContainerWindow>();
      MainWindow instance2 = ScriptableObject.CreateInstance<MainWindow>();
      instance2.SetMinMaxSizes(new Vector2(770f, 300f), new Vector2(10000f, 10000f));
      instance1.mainView = (View) instance2;
      Resolution desktopResolution = InternalEditorUtility.GetDesktopResolution();
      int num1 = Mathf.Clamp(desktopResolution.width * 3 / 4, 800, 1400);
      int num2 = Mathf.Clamp(desktopResolution.height * 3 / 4, 600, 950);
      instance1.position = new Rect(60f, 20f, (float) num1, (float) num2);
      instance1.Show(ShowMode.MainWindow, true, true);
      instance1.DisplayAllViews();
    }

    public void Cleanup()
    {
      if (this.children[1].children.Length != 0)
        return;
      this.window.position.height = ((Toolbar) this.children[0]).CalcHeight() + 20f;
    }
  }
}
