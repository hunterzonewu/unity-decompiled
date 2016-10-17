// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScreenShots
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ScreenShots
  {
    public static Color kToolbarBorderColor = new Color(0.54f, 0.54f, 0.54f, 1f);
    public static Color kWindowBorderColor = new Color(0.51f, 0.51f, 0.51f, 1f);
    public static bool s_TakeComponentScreenshot = false;

    [MenuItem("Window/Screenshot/Set Window Size %&l", false, 1000, true)]
    public static void SetMainWindowSize()
    {
      (Resources.FindObjectsOfTypeAll(typeof (MainWindow))[0] as MainWindow).window.position = new Rect(0.0f, 0.0f, 1024f, 768f);
    }

    [MenuItem("Window/Screenshot/Set Window Size Small", false, 1000, true)]
    public static void SetMainWindowSizeSmall()
    {
      (Resources.FindObjectsOfTypeAll(typeof (MainWindow))[0] as MainWindow).window.position = new Rect(0.0f, 0.0f, 762f, 600f);
    }

    [MenuItem("Window/Screenshot/Snap View %&j", false, 1000, true)]
    public static void Screenshot()
    {
      GUIView mouseOverView = ScreenShots.GetMouseOverView();
      if (!((Object) mouseOverView != (Object) null))
        return;
      string guiViewName = ScreenShots.GetGUIViewName(mouseOverView);
      Rect screenPosition = mouseOverView.screenPosition;
      --screenPosition.y;
      screenPosition.height += 2f;
      ScreenShots.SaveScreenShot(screenPosition, guiViewName);
    }

    [MenuItem("Window/Screenshot/Snap View Toolbar", false, 1000, true)]
    public static void ScreenshotToolbar()
    {
      GUIView mouseOverView = ScreenShots.GetMouseOverView();
      if (!((Object) mouseOverView != (Object) null))
        return;
      string name = ScreenShots.GetGUIViewName(mouseOverView) + "Toolbar";
      Rect screenPosition = mouseOverView.screenPosition;
      screenPosition.y += 19f;
      screenPosition.height = 16f;
      screenPosition.width -= 2f;
      ScreenShots.SaveScreenShotWithBorder(screenPosition, ScreenShots.kToolbarBorderColor, name);
    }

    [MenuItem("Window/Screenshot/Snap View Extended Right %&k", false, 1000, true)]
    public static void ScreenshotExtendedRight()
    {
      GUIView mouseOverView = ScreenShots.GetMouseOverView();
      if (!((Object) mouseOverView != (Object) null))
        return;
      string name = ScreenShots.GetGUIViewName(mouseOverView) + "Extended";
      MainWindow mainWindow = Resources.FindObjectsOfTypeAll(typeof (MainWindow))[0] as MainWindow;
      Rect screenPosition = mouseOverView.screenPosition;
      screenPosition.xMax = mainWindow.window.position.xMax;
      --screenPosition.y;
      screenPosition.height += 2f;
      ScreenShots.SaveScreenShot(screenPosition, name);
    }

    [MenuItem("Window/Screenshot/Snap Component", false, 1000, true)]
    public static void ScreenShotComponent()
    {
      ScreenShots.s_TakeComponentScreenshot = true;
    }

    public static void ScreenShotComponent(Rect contentRect, Object target)
    {
      ScreenShots.s_TakeComponentScreenshot = false;
      contentRect.yMax += 2f;
      ++contentRect.xMin;
      ScreenShots.SaveScreenShotWithBorder(contentRect, ScreenShots.kWindowBorderColor, target.GetType().Name + "Inspector");
    }

    [MenuItem("Window/Screenshot/Snap Game View Content", false, 1000, true)]
    public static void ScreenGameViewContent()
    {
      string uniquePathForName = ScreenShots.GetUniquePathForName("ContentExample");
      Application.CaptureScreenshot(uniquePathForName);
      Debug.Log((object) string.Format("Saved screenshot at {0}", (object) uniquePathForName));
    }

    [MenuItem("Window/Screenshot/Toggle DeveloperBuild", false, 1000, true)]
    public static void ToggleFakeNonDeveloperBuild()
    {
      Unsupported.fakeNonDeveloperBuild = !Unsupported.fakeNonDeveloperBuild;
      InternalEditorUtility.RequestScriptReload();
      InternalEditorUtility.RepaintAllViews();
    }

    private static GUIView GetMouseOverView()
    {
      GUIView mouseOverView = GUIView.mouseOverView;
      if ((Object) mouseOverView == (Object) null)
      {
        EditorApplication.Beep();
        Debug.LogWarning((object) "Could not take screenshot.");
      }
      return mouseOverView;
    }

    private static string GetGUIViewName(GUIView view)
    {
      HostView hostView = view as HostView;
      if ((Object) hostView != (Object) null)
        return hostView.actualView.GetType().Name;
      return "Window";
    }

    public static void SaveScreenShot(Rect r, string name)
    {
      ScreenShots.SaveScreenShot((int) r.width, (int) r.height, InternalEditorUtility.ReadScreenPixel(new Vector2(r.x, r.y), (int) r.width, (int) r.height), name);
    }

    public static string SaveScreenShotWithBorder(Rect r, Color borderColor, string name)
    {
      int width = (int) r.width;
      int height = (int) r.height;
      Color[] colorArray = InternalEditorUtility.ReadScreenPixel(new Vector2(r.x, r.y), width, height);
      Color[] pixels = new Color[(width + 2) * (height + 2)];
      for (int index1 = 0; index1 < width; ++index1)
      {
        for (int index2 = 0; index2 < height; ++index2)
          pixels[index1 + 1 + (width + 2) * (index2 + 1)] = colorArray[index1 + width * index2];
      }
      for (int index = 0; index < width + 2; ++index)
      {
        pixels[index] = borderColor;
        pixels[index + (width + 2) * (height + 1)] = borderColor;
      }
      for (int index = 0; index < height + 2; ++index)
      {
        pixels[index * (width + 2)] = borderColor;
        pixels[index * (width + 2) + (width + 1)] = borderColor;
      }
      return ScreenShots.SaveScreenShot((int) ((double) r.width + 2.0), (int) ((double) r.height + 2.0), pixels, name);
    }

    private static string SaveScreenShot(int width, int height, Color[] pixels, string name)
    {
      Texture2D texture2D = new Texture2D(width, height);
      texture2D.SetPixels(pixels, 0);
      texture2D.Apply(true);
      byte[] png = texture2D.EncodeToPNG();
      Object.DestroyImmediate((Object) texture2D, true);
      string uniquePathForName = ScreenShots.GetUniquePathForName(name);
      File.WriteAllBytes(uniquePathForName, png);
      Debug.Log((object) string.Format("Saved screenshot at {0}", (object) uniquePathForName));
      return uniquePathForName;
    }

    private static string GetUniquePathForName(string name)
    {
      string path = string.Format("{0}/../../{1}.png", (object) Application.dataPath, (object) name);
      int num = 0;
      while (File.Exists(path))
      {
        path = string.Format("{0}/../../{1}{2:000}.png", (object) Application.dataPath, (object) name, (object) num);
        ++num;
      }
      return path;
    }
  }
}
