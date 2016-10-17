// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetServerConfig
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetServerConfig
  {
    private static Regex sKeyTag = new Regex("<key>([^<]+)</key>");
    private static Regex sValueTag = new Regex("<string>([^<]+)</string>");
    private Dictionary<string, string> fileContents;
    private string fileName;

    public string connectionSettings
    {
      get
      {
        return this.fileContents["Maint Connection Settings"];
      }
      set
      {
        this.fileContents["Maint Connection Settings"] = value;
      }
    }

    public string server
    {
      get
      {
        return this.fileContents["Maint Server"];
      }
      set
      {
        this.fileContents["Maint Server"] = value;
      }
    }

    public int portNumber
    {
      get
      {
        return int.Parse(this.fileContents["Maint port number"]);
      }
      set
      {
        this.fileContents["Maint port number"] = value.ToString();
      }
    }

    public float timeout
    {
      get
      {
        return float.Parse(this.fileContents["Maint Timeout"]);
      }
      set
      {
        this.fileContents["Maint Timeout"] = value.ToString();
      }
    }

    public string userName
    {
      get
      {
        return this.fileContents["Maint UserName"];
      }
      set
      {
        this.fileContents["Maint UserName"] = value;
      }
    }

    public string dbName
    {
      get
      {
        return this.fileContents["Maint database name"];
      }
      set
      {
        this.fileContents["Maint database name"] = value;
      }
    }

    public string projectName
    {
      get
      {
        return this.fileContents["Maint project name"];
      }
      set
      {
        this.fileContents["Maint project name"] = value;
      }
    }

    public string settingsType
    {
      get
      {
        return this.fileContents["Maint settings type"];
      }
      set
      {
        this.fileContents["Maint settings type"] = value;
      }
    }

    public AssetServerConfig()
    {
      this.fileContents = new Dictionary<string, string>();
      this.fileName = Application.dataPath + "/../Library/ServerPreferences.plist";
      try
      {
        using (StreamReader streamReader = new StreamReader(this.fileName))
        {
          string index = ".unkown";
          string input;
          while ((input = streamReader.ReadLine()) != null)
          {
            Match match1 = AssetServerConfig.sKeyTag.Match(input);
            if (match1.Success)
              index = match1.Groups[1].Value;
            Match match2 = AssetServerConfig.sValueTag.Match(input);
            if (match2.Success)
              this.fileContents[index] = match2.Groups[1].Value;
          }
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Could not read asset server configuration: " + ex.Message));
      }
    }
  }
}
