// Decompiled with JetBrains decompiler
// Type: UnityEditor.CompareInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal struct CompareInfo
  {
    public int left;
    public int right;
    public int convert_binary;
    public int autodetect_binary;

    public CompareInfo(int ver1, int ver2, int binary, int abinary)
    {
      this.left = ver1;
      this.right = ver2;
      this.convert_binary = binary;
      this.autodetect_binary = abinary;
    }
  }
}
