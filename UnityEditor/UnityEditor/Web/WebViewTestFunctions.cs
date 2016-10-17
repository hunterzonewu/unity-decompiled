// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebViewTestFunctions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Web
{
  internal class WebViewTestFunctions
  {
    public int ReturnInt()
    {
      return 5;
    }

    public string ReturnString()
    {
      return "Five";
    }

    public bool ReturnBool()
    {
      return true;
    }

    public int[] ReturnNumberArray()
    {
      return new int[3]{ 1, 2, 3 };
    }

    public string[] ReturnStringArray()
    {
      return new string[3]{ "One", "Two", "Three" };
    }

    public bool[] ReturnBoolArray()
    {
      return new bool[3]{ true, false, true };
    }

    public TestObject ReturnObject()
    {
      return new TestObject() { NumberProperty = 5, StringProperty = "Five", BoolProperty = true };
    }

    public void AcceptInt(int passedInt)
    {
      Debug.Log((object) ("A value was passed from JS: " + (object) passedInt));
    }

    public void AcceptString(string passedString)
    {
      Debug.Log((object) ("A value was passed from JS: " + passedString));
    }

    public void AcceptBool(bool passedBool)
    {
      Debug.Log((object) ("A value was passed from JS: " + (object) passedBool));
    }

    public void AcceptIntArray(int[] passedArray)
    {
      Debug.Log((object) "An array was passed from the JS. Array elements were:");
      for (int index = 0; index <= passedArray.Length; ++index)
        Debug.Log((object) ("Element at index " + (object) index + ": " + (object) passedArray[index]));
    }

    public void AcceptStringArray(string[] passedArray)
    {
      Debug.Log((object) "An array was passed from the JS. Array elements were:");
      for (int index = 0; index <= passedArray.Length; ++index)
        Debug.Log((object) ("Element at index " + (object) index + ": " + passedArray[index]));
    }

    public void AcceptBoolArray(bool[] passedArray)
    {
      Debug.Log((object) "An array was passed from the JS. Array elements were:");
      for (int index = 1; index <= passedArray.Length; ++index)
        Debug.Log((object) ("Element at index " + (object) index + ": " + (object) passedArray[index]));
    }

    public void AcceptTestObject(TestObject passedObject)
    {
      Debug.Log((object) "An object was passed from the JS. Properties were:");
      Debug.Log((object) ("StringProperty: " + passedObject.StringProperty));
      Debug.Log((object) ("NumberProperty: " + (object) passedObject.NumberProperty));
      Debug.Log((object) ("BoolProperty: " + (object) passedObject.BoolProperty));
    }

    public void VoidMethod(string logMessage)
    {
      Debug.Log((object) ("A method was called from the CEF: " + logMessage));
    }

    private string APrivateMethod(string input)
    {
      return "This method is private and not for CEF";
    }

    public string[] ArrayReverse(string[] input)
    {
      return (string[]) ((IEnumerable<string>) input).Reverse<string>();
    }

    public void LogMessage(string message)
    {
      Debug.Log((object) message);
    }

    public static void RunTestScript(string path)
    {
      WebViewEditorWindow.Create<WebViewTestFunctions>("Test Window", "file:///" + path, 0, 0, 0, 0).OnBatchMode();
    }
  }
}
