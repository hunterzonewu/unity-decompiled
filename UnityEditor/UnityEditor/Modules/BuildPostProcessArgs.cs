// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.BuildPostProcessArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor.Modules
{
  internal struct BuildPostProcessArgs
  {
    public BuildTarget target;
    public string stagingArea;
    public string stagingAreaData;
    public string stagingAreaDataManaged;
    public string playerPackage;
    public string installPath;
    public string companyName;
    public string productName;
    public Guid productGUID;
    public BuildOptions options;
    internal RuntimeClassRegistry usedClassRegistry;
  }
}
