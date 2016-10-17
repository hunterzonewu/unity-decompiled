// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ISettingEditorExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal interface ISettingEditorExtension
  {
    void OnEnable(PlayerSettingsEditor settingsEditor);

    bool HasPublishSection();

    void PublishSectionGUI(float h, float midWidth, float maxWidth);

    bool HasIdentificationGUI();

    void IdentificationSectionGUI();

    void ConfigurationSectionGUI();

    bool SupportsOrientation();

    bool SupportsStaticBatching();

    bool SupportsDynamicBatching();

    bool CanShowUnitySplashScreen();

    void SplashSectionGUI();

    bool UsesStandardIcons();

    void IconSectionGUI();

    bool HasResolutionSection();

    void ResolutionSectionGUI(float h, float midWidth, float maxWidth);

    bool HasBundleIdentifier();
  }
}
