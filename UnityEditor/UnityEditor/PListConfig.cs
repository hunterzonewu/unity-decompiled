// Decompiled with JetBrains decompiler
// Type: UnityEditor.PListConfig
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using System.Text.RegularExpressions;

namespace UnityEditor
{
  internal class PListConfig
  {
    private string fileName;
    private string xml;

    public string this[string paramName]
    {
      get
      {
        Match match = PListConfig.GetRegex(paramName).Match(this.xml);
        if (match.Success)
          return match.Groups["Value"].Value;
        return string.Empty;
      }
      set
      {
        if (PListConfig.GetRegex(paramName).Match(this.xml).Success)
          this.xml = PListConfig.GetRegex(paramName).Replace(this.xml, "${Part1}" + value + "</string>");
        else
          this.WriteNewValue(paramName, value);
      }
    }

    public PListConfig(string fileName)
    {
      if (File.Exists(fileName))
      {
        StreamReader streamReader = new StreamReader(fileName);
        this.xml = streamReader.ReadToEnd();
        streamReader.Close();
      }
      else
        this.Clear();
      this.fileName = fileName;
    }

    private static Regex GetRegex(string paramName)
    {
      return new Regex("(?<Part1><key>" + paramName + "</key>\\s*<string>)(?<Value>.*)</string>");
    }

    public void Save()
    {
      StreamWriter streamWriter = new StreamWriter(this.fileName);
      streamWriter.Write(this.xml);
      streamWriter.Close();
    }

    private void WriteNewValue(string key, string val)
    {
      this.xml = new Regex("</dict>").Replace(this.xml, "\t<key>" + key + "</key>\n\t<string>" + val + "</string>\n</dict>");
    }

    public void Clear()
    {
      this.xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n<plist version=\"1.0\">\n<dict>\n</dict>\n</plist>\n";
    }
  }
}
