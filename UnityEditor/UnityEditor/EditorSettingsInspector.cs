// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorSettingsInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Hardware;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (EditorSettings))]
  internal class EditorSettingsInspector : Editor
  {
    private EditorSettingsInspector.PopupElement[] vcDefaultPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement(ExternalVersionControl.Disabled), new EditorSettingsInspector.PopupElement(ExternalVersionControl.Generic), new EditorSettingsInspector.PopupElement(ExternalVersionControl.AssetServer, true) };
    private EditorSettingsInspector.PopupElement[] serializationPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("Mixed"), new EditorSettingsInspector.PopupElement("Force Binary"), new EditorSettingsInspector.PopupElement("Force Text") };
    private EditorSettingsInspector.PopupElement[] behaviorPopupList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("3D"), new EditorSettingsInspector.PopupElement("2D") };
    private EditorSettingsInspector.PopupElement[] spritePackerPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("Disabled"), new EditorSettingsInspector.PopupElement("Enabled For Builds"), new EditorSettingsInspector.PopupElement("Always Enabled") };
    private EditorSettingsInspector.PopupElement[] spritePackerPaddingPowerPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("1"), new EditorSettingsInspector.PopupElement("2"), new EditorSettingsInspector.PopupElement("3") };
    private EditorSettingsInspector.PopupElement[] remoteCompressionList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("JPEG"), new EditorSettingsInspector.PopupElement("PNG") };
    private EditorSettingsInspector.PopupElement[] remoteResolutionList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("Normal"), new EditorSettingsInspector.PopupElement("Downsize") };
    private string[] logLevelPopupList = new string[4]{ "Verbose", "Info", "Notice", "Fatal" };
    private string[] semanticMergePopupList = new string[3]{ "Off", "Premerge", "Ask" };
    public readonly GUIContent spritePaddingPower = EditorGUIUtility.TextContent("Padding Power|Set Padding Power if Atlas is enabled.");
    private EditorSettingsInspector.PopupElement[] vcPopupList;
    private EditorSettingsInspector.PopupElement[] remoteDevicePopupList;
    private DevDevice[] remoteDeviceList;

    public void OnEnable()
    {
      Plugin[] availablePlugins = Plugin.availablePlugins;
      this.vcPopupList = new EditorSettingsInspector.PopupElement[availablePlugins.Length + this.vcDefaultPopupList.Length];
      Array.Copy((Array) this.vcDefaultPopupList, (Array) this.vcPopupList, this.vcDefaultPopupList.Length);
      int index = 0;
      int length = this.vcDefaultPopupList.Length;
      while (length < this.vcPopupList.Length)
      {
        this.vcPopupList[length] = new EditorSettingsInspector.PopupElement(availablePlugins[index].name, true);
        ++length;
        ++index;
      }
      DevDeviceList.Changed += new DevDeviceList.OnChangedHandler(this.OnDeviceListChanged);
      this.BuildRemoteDeviceList();
    }

    public void OnDisable()
    {
      DevDeviceList.Changed -= new DevDeviceList.OnChangedHandler(this.OnDeviceListChanged);
    }

    private void OnDeviceListChanged()
    {
      this.BuildRemoteDeviceList();
    }

    private void BuildRemoteDeviceList()
    {
      List<DevDevice> devDeviceList = new List<DevDevice>();
      List<EditorSettingsInspector.PopupElement> popupElementList = new List<EditorSettingsInspector.PopupElement>();
      devDeviceList.Add(DevDevice.none);
      popupElementList.Add(new EditorSettingsInspector.PopupElement("None"));
      devDeviceList.Add(new DevDevice("Any Android Device", "Any Android Device", "Android", "Android", DevDeviceState.Connected, DevDeviceFeatures.RemoteConnection));
      popupElementList.Add(new EditorSettingsInspector.PopupElement("Any Android Device"));
      foreach (DevDevice device in DevDeviceList.GetDevices())
      {
        bool flag = (device.features & DevDeviceFeatures.RemoteConnection) != DevDeviceFeatures.None;
        if (device.isConnected && flag)
        {
          devDeviceList.Add(device);
          popupElementList.Add(new EditorSettingsInspector.PopupElement(device.name));
        }
      }
      this.remoteDeviceList = devDeviceList.ToArray();
      this.remoteDevicePopupList = popupElementList.ToArray();
    }

    public override void OnInspectorGUI()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey8F guICAnonStorey8F = new EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey8F();
      bool enabled = GUI.enabled;
      this.ShowUnityRemoteGUI(enabled);
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Version Control", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      // ISSUE: reference to a compiler-generated field
      guICAnonStorey8F.selvc = (ExternalVersionControl) EditorSettings.externalVersionControl;
      // ISSUE: reference to a compiler-generated method
      int selectedIndex1 = Array.FindIndex<EditorSettingsInspector.PopupElement>(this.vcPopupList, new Predicate<EditorSettingsInspector.PopupElement>(guICAnonStorey8F.\u003C\u003Em__15E));
      if (selectedIndex1 < 0)
        selectedIndex1 = 0;
      GUIContent content1 = new GUIContent(this.vcPopupList[selectedIndex1].content);
      Rect rect1 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content1, EditorStyles.popup), 0, new GUIContent("Mode"));
      if (EditorGUI.ButtonMouseDown(rect1, content1, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect1, this.vcPopupList, selectedIndex1, new GenericMenu.MenuFunction2(this.SetVersionControlSystem));
      if (this.VersionControlSystemHasGUI())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey90 guICAnonStorey90 = new EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey90();
        GUI.enabled = true;
        bool flag1 = false;
        if (EditorSettings.externalVersionControl == ExternalVersionControl.AssetServer)
        {
          EditorUserSettings.SetConfigValue("vcUsername", EditorGUILayout.TextField("User", EditorUserSettings.GetConfigValue("vcUsername"), new GUILayoutOption[0]));
          EditorUserSettings.SetConfigValue("vcPassword", EditorGUILayout.PasswordField("Password", EditorUserSettings.GetConfigValue("vcPassword"), new GUILayoutOption[0]));
        }
        else if (!(EditorSettings.externalVersionControl == ExternalVersionControl.Generic) && !(EditorSettings.externalVersionControl == ExternalVersionControl.Disabled))
        {
          ConfigField[] activeConfigFields = Provider.GetActiveConfigFields();
          flag1 = true;
          foreach (ConfigField configField in activeConfigFields)
          {
            string configValue = EditorUserSettings.GetConfigValue(configField.name);
            string str = !configField.isPassword ? EditorGUILayout.TextField(configField.label, configValue, new GUILayoutOption[0]) : EditorGUILayout.PasswordField(configField.label, configValue, new GUILayoutOption[0]);
            if (str != configValue)
              EditorUserSettings.SetConfigValue(configField.name, str);
            if (configField.isRequired && string.IsNullOrEmpty(str))
              flag1 = false;
          }
        }
        // ISSUE: reference to a compiler-generated field
        guICAnonStorey90.logLevel = EditorUserSettings.GetConfigValue("vcSharedLogLevel");
        // ISSUE: reference to a compiler-generated method
        int index1 = Array.FindIndex<string>(this.logLevelPopupList, new Predicate<string>(guICAnonStorey90.\u003C\u003Em__15F));
        if (index1 == -1)
        {
          // ISSUE: reference to a compiler-generated field
          guICAnonStorey90.logLevel = "info";
        }
        int index2 = EditorGUILayout.Popup("Log Level", Math.Abs(index1), this.logLevelPopupList, new GUILayoutOption[0]);
        if (index2 != index1)
          EditorUserSettings.SetConfigValue("vcSharedLogLevel", this.logLevelPopupList[index2].ToLower());
        GUI.enabled = enabled;
        string label2 = "Connected";
        if (Provider.onlineState == OnlineState.Updating)
          label2 = "Connecting...";
        else if (Provider.onlineState == OnlineState.Offline)
          label2 = "Disconnected";
        EditorGUILayout.LabelField("Status", label2, new GUILayoutOption[0]);
        if (Provider.onlineState != OnlineState.Online && !string.IsNullOrEmpty(Provider.offlineReason))
        {
          GUI.enabled = false;
          GUILayout.TextArea(Provider.offlineReason);
          GUI.enabled = enabled;
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.enabled = flag1;
        if (GUILayout.Button("Connect", EditorStyles.miniButton, new GUILayoutOption[0]))
          Provider.UpdateSettings();
        GUILayout.EndHorizontal();
        EditorUserSettings.AutomaticAdd = EditorGUILayout.Toggle("Automatic add", EditorUserSettings.AutomaticAdd, new GUILayoutOption[0]);
        if (Provider.requiresNetwork)
        {
          bool flag2 = EditorGUILayout.Toggle("Work Offline", EditorUserSettings.WorkOffline, new GUILayoutOption[0]);
          if (flag2 != EditorUserSettings.WorkOffline)
          {
            if (flag2 && !EditorUtility.DisplayDialog("Confirm working offline", "Working offline and making changes to your assets means that you will have to manually integrate changes back into version control using your standard version control client before you stop working offline in Unity. Make sure you know what you are doing.", "Work offline", "Cancel"))
              flag2 = false;
            EditorUserSettings.WorkOffline = flag2;
            EditorApplication.RequestRepaintAllViews();
          }
        }
        if (Provider.hasCheckoutSupport)
          EditorUserSettings.showFailedCheckout = EditorGUILayout.Toggle("Show failed checkouts", EditorUserSettings.showFailedCheckout, new GUILayoutOption[0]);
        GUI.enabled = enabled;
        EditorUserSettings.semanticMergeMode = (SemanticMergeMode) EditorGUILayout.Popup("Smart merge", (int) EditorUserSettings.semanticMergeMode, this.semanticMergePopupList, new GUILayoutOption[0]);
        this.DrawOverlayDescriptions();
      }
      GUILayout.Space(10f);
      GUILayout.Label("WWW Security Emulation", EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorSettings.webSecurityEmulationEnabled = EditorGUILayout.Toggle("Enable Webplayer Security Emulation", EditorSettings.webSecurityEmulationEnabled, new GUILayoutOption[0]);
      string str1 = EditorGUILayout.TextField("Host URL", EditorSettings.webSecurityEmulationHostUrl, new GUILayoutOption[0]);
      if (str1 != EditorSettings.webSecurityEmulationHostUrl)
        EditorSettings.webSecurityEmulationHostUrl = str1;
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Asset Serialization", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      GUIContent content2 = new GUIContent(this.serializationPopupList[(int) EditorSettings.serializationMode].content);
      Rect rect2 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content2, EditorStyles.popup), 0, new GUIContent("Mode"));
      if (EditorGUI.ButtonMouseDown(rect2, content2, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect2, this.serializationPopupList, (int) EditorSettings.serializationMode, new GenericMenu.MenuFunction2(this.SetAssetSerializationMode));
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Default Behavior Mode", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      int selectedIndex2 = Mathf.Clamp((int) EditorSettings.defaultBehaviorMode, 0, this.behaviorPopupList.Length - 1);
      GUIContent content3 = new GUIContent(this.behaviorPopupList[selectedIndex2].content);
      Rect rect3 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content3, EditorStyles.popup), 0, new GUIContent("Mode"));
      if (EditorGUI.ButtonMouseDown(rect3, content3, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect3, this.behaviorPopupList, selectedIndex2, new GenericMenu.MenuFunction2(this.SetDefaultBehaviorMode));
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Sprite Packer", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      int selectedIndex3 = Mathf.Clamp((int) EditorSettings.spritePackerMode, 0, this.spritePackerPopupList.Length - 1);
      GUIContent content4 = new GUIContent(this.spritePackerPopupList[selectedIndex3].content);
      Rect rect4 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content4, EditorStyles.popup), 0, new GUIContent("Mode"));
      if (EditorGUI.ButtonMouseDown(rect4, content4, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect4, this.spritePackerPopupList, selectedIndex3, new GenericMenu.MenuFunction2(this.SetSpritePackerMode));
      int selectedIndex4 = Mathf.Clamp(EditorSettings.spritePackerPaddingPower - 1, 0, 2);
      GUIContent content5 = new GUIContent(this.spritePackerPaddingPowerPopupList[selectedIndex4].content);
      Rect rect5 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content5, EditorStyles.popup), 0, new GUIContent("Padding Power"));
      if (EditorGUI.ButtonMouseDown(rect5, content5, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect5, this.spritePackerPaddingPowerPopupList, selectedIndex4, new GenericMenu.MenuFunction2(this.SetSpritePackerPaddingPower));
      this.DoProjectGenerationSettings();
    }

    private void DoProjectGenerationSettings()
    {
      GUILayout.Space(10f);
      GUILayout.Label("C# Project Generation", EditorStyles.boldLabel, new GUILayoutOption[0]);
      string generationUserExtensions = EditorSettings.Internal_ProjectGenerationUserExtensions;
      string str1 = EditorGUILayout.TextField("Additional extensions to include", generationUserExtensions, new GUILayoutOption[0]);
      if (str1 != generationUserExtensions)
        EditorSettings.Internal_ProjectGenerationUserExtensions = str1;
      string generationRootNamespace = EditorSettings.projectGenerationRootNamespace;
      string str2 = EditorGUILayout.TextField("Root namespace", generationRootNamespace, new GUILayoutOption[0]);
      if (!(str2 != generationRootNamespace))
        return;
      EditorSettings.projectGenerationRootNamespace = str2;
    }

    private static int GetIndexById(DevDevice[] elements, string id, int defaultIndex)
    {
      for (int index = 0; index < elements.Length; ++index)
      {
        if (elements[index].id == id)
          return index;
      }
      return defaultIndex;
    }

    private static int GetIndexById(EditorSettingsInspector.PopupElement[] elements, string id, int defaultIndex)
    {
      for (int index = 0; index < elements.Length; ++index)
      {
        if (elements[index].id == id)
          return index;
      }
      return defaultIndex;
    }

    private void ShowUnityRemoteGUI(bool editorEnabled)
    {
      GUI.enabled = true;
      GUILayout.Label("Unity Remote", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = editorEnabled;
      int indexById1 = EditorSettingsInspector.GetIndexById(this.remoteDeviceList, EditorSettings.unityRemoteDevice, 0);
      GUIContent content1 = new GUIContent(this.remoteDevicePopupList[indexById1].content);
      Rect rect1 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content1, EditorStyles.popup), 0, new GUIContent("Device"));
      if (EditorGUI.ButtonMouseDown(rect1, content1, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect1, this.remoteDevicePopupList, indexById1, new GenericMenu.MenuFunction2(this.SetUnityRemoteDevice));
      int indexById2 = EditorSettingsInspector.GetIndexById(this.remoteCompressionList, EditorSettings.unityRemoteCompression, 0);
      GUIContent content2 = new GUIContent(this.remoteCompressionList[indexById2].content);
      Rect rect2 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content2, EditorStyles.popup), 0, new GUIContent("Compression"));
      if (EditorGUI.ButtonMouseDown(rect2, content2, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect2, this.remoteCompressionList, indexById2, new GenericMenu.MenuFunction2(this.SetUnityRemoteCompression));
      int indexById3 = EditorSettingsInspector.GetIndexById(this.remoteResolutionList, EditorSettings.unityRemoteResolution, 0);
      GUIContent content3 = new GUIContent(this.remoteResolutionList[indexById3].content);
      Rect rect3 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content3, EditorStyles.popup), 0, new GUIContent("Resolution"));
      if (!EditorGUI.ButtonMouseDown(rect3, content3, FocusType.Passive, EditorStyles.popup))
        return;
      this.DoPopup(rect3, this.remoteResolutionList, indexById3, new GenericMenu.MenuFunction2(this.SetUnityRemoteResolution));
    }

    private void DrawOverlayDescriptions()
    {
      if ((UnityEngine.Object) Provider.overlayAtlas == (UnityEngine.Object) null)
        return;
      GUILayout.Space(10f);
      GUILayout.Label("Overlay legends", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      this.DrawOverlayDescription(Asset.States.Local);
      this.DrawOverlayDescription(Asset.States.OutOfSync);
      this.DrawOverlayDescription(Asset.States.CheckedOutLocal);
      this.DrawOverlayDescription(Asset.States.CheckedOutRemote);
      this.DrawOverlayDescription(Asset.States.DeletedLocal);
      this.DrawOverlayDescription(Asset.States.DeletedRemote);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      this.DrawOverlayDescription(Asset.States.AddedLocal);
      this.DrawOverlayDescription(Asset.States.AddedRemote);
      this.DrawOverlayDescription(Asset.States.Conflicted);
      this.DrawOverlayDescription(Asset.States.LockedLocal);
      this.DrawOverlayDescription(Asset.States.LockedRemote);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private void DrawOverlayDescription(Asset.States state)
    {
      Rect atlasRectForState = Provider.GetAtlasRectForState((int) state);
      if ((double) atlasRectForState.width == 0.0)
        return;
      Texture2D overlayAtlas = Provider.overlayAtlas;
      if ((UnityEngine.Object) overlayAtlas == (UnityEngine.Object) null)
        return;
      GUILayout.Label("    " + Asset.StateToString(state), EditorStyles.miniLabel, new GUILayoutOption[0]);
      Rect lastRect = GUILayoutUtility.GetLastRect();
      lastRect.width = 16f;
      GUI.DrawTextureWithTexCoords(lastRect, (Texture) overlayAtlas, atlasRectForState);
    }

    private void DoPopup(Rect popupRect, EditorSettingsInspector.PopupElement[] elements, int selectedIndex, GenericMenu.MenuFunction2 func)
    {
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < elements.Length; ++index)
      {
        EditorSettingsInspector.PopupElement element = elements[index];
        if (element.Enabled)
          genericMenu.AddItem(element.content, index == selectedIndex, func, (object) index);
        else
          genericMenu.AddDisabledItem(element.content);
      }
      genericMenu.DropDown(popupRect);
    }

    private bool VersionControlSystemHasGUI()
    {
      ExternalVersionControl externalVersionControl = (ExternalVersionControl) EditorSettings.externalVersionControl;
      if ((string) externalVersionControl != ExternalVersionControl.Disabled && (string) externalVersionControl != ExternalVersionControl.AutoDetect && (string) externalVersionControl != ExternalVersionControl.AssetServer)
        return (string) externalVersionControl != ExternalVersionControl.Generic;
      return false;
    }

    private void SetVersionControlSystem(object data)
    {
      int index = (int) data;
      if (index < 0 && index >= this.vcPopupList.Length)
        return;
      EditorSettingsInspector.PopupElement vcPopup = this.vcPopupList[index];
      string externalVersionControl = EditorSettings.externalVersionControl;
      EditorSettings.externalVersionControl = vcPopup.content.text;
      Provider.UpdateSettings();
      AssetDatabase.Refresh();
      if (!(externalVersionControl != vcPopup.content.text))
        return;
      if (vcPopup.content.text == ExternalVersionControl.AssetServer || vcPopup.content.text == ExternalVersionControl.Disabled || vcPopup.content.text == ExternalVersionControl.Generic)
      {
        WindowPending.CloseAllWindows();
      }
      else
      {
        ASMainWindow[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (ASMainWindow)) as ASMainWindow[];
        ASMainWindow asMainWindow = objectsOfTypeAll.Length <= 0 ? (ASMainWindow) null : objectsOfTypeAll[0];
        if (!((UnityEngine.Object) asMainWindow != (UnityEngine.Object) null))
          return;
        asMainWindow.Close();
      }
    }

    private void SetAssetSerializationMode(object data)
    {
      EditorSettings.serializationMode = (SerializationMode) data;
    }

    private void SetUnityRemoteDevice(object data)
    {
      EditorSettings.unityRemoteDevice = this.remoteDeviceList[(int) data].id;
    }

    private void SetUnityRemoteCompression(object data)
    {
      EditorSettings.unityRemoteCompression = this.remoteCompressionList[(int) data].id;
    }

    private void SetUnityRemoteResolution(object data)
    {
      EditorSettings.unityRemoteResolution = this.remoteResolutionList[(int) data].id;
    }

    private void SetDefaultBehaviorMode(object data)
    {
      EditorSettings.defaultBehaviorMode = (EditorBehaviorMode) data;
    }

    private void SetSpritePackerMode(object data)
    {
      EditorSettings.spritePackerMode = (SpritePackerMode) data;
    }

    private void SetSpritePackerPaddingPower(object data)
    {
      EditorSettings.spritePackerPaddingPower = (int) data + 1;
    }

    private struct PopupElement
    {
      public readonly string id;
      public readonly bool requiresTeamLicense;
      public readonly GUIContent content;

      public bool Enabled
      {
        get
        {
          if (this.requiresTeamLicense)
            return InternalEditorUtility.HasTeamLicense();
          return true;
        }
      }

      public PopupElement(string content)
      {
        this = new EditorSettingsInspector.PopupElement(content, false);
      }

      public PopupElement(string content, bool requiresTeamLicense)
      {
        this.id = content;
        this.content = new GUIContent(content);
        this.requiresTeamLicense = requiresTeamLicense;
      }
    }
  }
}
