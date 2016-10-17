// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAssetInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AssetStoreAssetInspector))]
  internal class AssetStoreAssetInspector : Editor
  {
    internal static string s_PurchaseMessage = string.Empty;
    internal static string s_PaymentMethodCard = string.Empty;
    internal static string s_PaymentMethodExpire = string.Empty;
    internal static string s_PriceText = string.Empty;
    private bool packageInfoShown = true;
    private static AssetStoreAssetInspector s_SharedAssetStoreAssetInspector;
    private static AssetStoreAssetInspector.Styles styles;
    private Vector2 pos;
    private static GUIContent[] sStatusWheel;
    internal static AssetStoreAssetInspector.PaymentAvailability m_PaymentAvailability;
    private int lastAssetID;
    private EditorWrapper m_PreviewEditor;
    private Object m_PreviewObject;

    public static AssetStoreAssetInspector Instance
    {
      get
      {
        if ((Object) AssetStoreAssetInspector.s_SharedAssetStoreAssetInspector == (Object) null)
        {
          AssetStoreAssetInspector.s_SharedAssetStoreAssetInspector = ScriptableObject.CreateInstance<AssetStoreAssetInspector>();
          AssetStoreAssetInspector.s_SharedAssetStoreAssetInspector.hideFlags = HideFlags.HideAndDontSave;
        }
        return AssetStoreAssetInspector.s_SharedAssetStoreAssetInspector;
      }
    }

    public static bool OfflineNoticeEnabled { get; set; }

    internal static AssetStoreAssetInspector.PaymentAvailability paymentAvailability
    {
      get
      {
        if (AssetStoreClient.LoggedOut())
          AssetStoreAssetInspector.m_PaymentAvailability = AssetStoreAssetInspector.PaymentAvailability.AnonymousUser;
        return AssetStoreAssetInspector.m_PaymentAvailability;
      }
      set
      {
        if (AssetStoreClient.LoggedOut())
          AssetStoreAssetInspector.m_PaymentAvailability = AssetStoreAssetInspector.PaymentAvailability.AnonymousUser;
        else
          AssetStoreAssetInspector.m_PaymentAvailability = value;
      }
    }

    private EditorWrapper previewEditor
    {
      get
      {
        AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
        if (firstAsset == null)
          return (EditorWrapper) null;
        Object preview = firstAsset.Preview;
        if (preview == (Object) null)
          return (EditorWrapper) null;
        if (preview != this.m_PreviewObject)
        {
          this.m_PreviewObject = preview;
          if (this.m_PreviewEditor != null)
            this.m_PreviewEditor.Dispose();
          this.m_PreviewEditor = EditorWrapper.Make(this.m_PreviewObject, EditorFeatures.PreviewGUI);
        }
        return this.m_PreviewEditor;
      }
    }

    private static GUIContent StatusWheel
    {
      get
      {
        if (AssetStoreAssetInspector.sStatusWheel == null)
        {
          AssetStoreAssetInspector.sStatusWheel = new GUIContent[12];
          for (int index = 0; index < 12; ++index)
            AssetStoreAssetInspector.sStatusWheel[index] = new GUIContent()
            {
              image = (Texture) EditorGUIUtility.LoadIcon("WaitSpin" + index.ToString("00"))
            };
        }
        int index1 = (int) Mathf.Repeat(Time.realtimeSinceStartup * 10f, 11.99f);
        return AssetStoreAssetInspector.sStatusWheel[index1];
      }
    }

    public void OnDownloadProgress(string id, string message, int bytes, int total)
    {
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null)
        return;
      AssetStoreAsset.PreviewInfo previewInfo = firstAsset.previewInfo;
      if (previewInfo == null || firstAsset.packageID.ToString() != id)
        return;
      previewInfo.downloadProgress = !(message == "downloading") && !(message == "connecting") || AssetStoreAssetInspector.OfflineNoticeEnabled ? -1f : (float) bytes / (float) total;
      this.Repaint();
    }

    public void Update()
    {
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      bool flag = firstAsset != null && firstAsset.previewInfo != null && ((double) firstAsset.previewInfo.buildProgress >= 0.0 || (double) firstAsset.previewInfo.downloadProgress >= 0.0);
      if (firstAsset == null && this.lastAssetID != 0 || firstAsset != null && this.lastAssetID != firstAsset.id || flag)
      {
        this.lastAssetID = firstAsset != null ? firstAsset.id : 0;
        this.Repaint();
      }
      if (firstAsset == null || !((Object) firstAsset.previewBundle != (Object) null))
        return;
      firstAsset.previewBundle.Unload(false);
      firstAsset.previewBundle = (AssetBundle) null;
      this.Repaint();
    }

    public override void OnInspectorGUI()
    {
      if (AssetStoreAssetInspector.styles == null)
      {
        AssetStoreAssetInspector.s_SharedAssetStoreAssetInspector = this;
        AssetStoreAssetInspector.styles = new AssetStoreAssetInspector.Styles();
      }
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      AssetStoreAsset.PreviewInfo previewInfo = (AssetStoreAsset.PreviewInfo) null;
      if (firstAsset != null)
        previewInfo = firstAsset.previewInfo;
      if (firstAsset != null)
        this.target.name = string.Format("Asset Store: {0}", (object) firstAsset.name);
      else
        this.target.name = "Asset Store";
      EditorGUILayout.BeginVertical();
      bool enabled1 = GUI.enabled;
      GUI.enabled = firstAsset != null && firstAsset.packageID != 0;
      if (AssetStoreAssetInspector.OfflineNoticeEnabled)
      {
        Color color = GUI.color;
        GUI.color = Color.yellow;
        GUILayout.Label("Network is offline");
        GUI.color = color;
      }
      if (firstAsset != null)
      {
        string empty;
        if (firstAsset.className == null)
          empty = string.Empty;
        else
          empty = firstAsset.className.Split(new char[1]{ ' ' }, 2)[0];
        string label2 = empty;
        bool flag1 = firstAsset.id == -firstAsset.packageID;
        if (flag1)
          label2 = "Package";
        if (firstAsset.HasLivePreview)
          label2 = firstAsset.Preview.GetType().Name;
        EditorGUILayout.LabelField("Type", label2, new GUILayoutOption[0]);
        if (flag1)
        {
          this.packageInfoShown = true;
        }
        else
        {
          EditorGUILayout.Separator();
          this.packageInfoShown = EditorGUILayout.Foldout(this.packageInfoShown, "Part of package");
        }
        if (this.packageInfoShown)
        {
          EditorGUILayout.LabelField("Name", previewInfo != null ? previewInfo.packageName : "-", new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Version", previewInfo != null ? previewInfo.packageVersion : "-", new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Price", previewInfo != null ? (firstAsset.price == null || !(firstAsset.price != string.Empty) ? "free" : firstAsset.price) : "-", new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Rating", previewInfo == null || previewInfo.packageRating < 0 ? "-" : previewInfo.packageRating.ToString() + " of 5", new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Size", previewInfo != null ? AssetStoreAssetInspector.intToSizeString(previewInfo.packageSize) : "-", new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Asset count", previewInfo == null || previewInfo.packageAssetCount < 0 ? "-" : previewInfo.packageAssetCount.ToString(), new GUILayoutOption[0]);
          GUILayout.BeginHorizontal();
          EditorGUILayout.PrefixLabel("Web page");
          bool flag2 = previewInfo != null && previewInfo.packageShortUrl != null && previewInfo.packageShortUrl != string.Empty;
          bool enabled2 = GUI.enabled;
          GUI.enabled = flag2;
          if (GUILayout.Button(!flag2 ? EditorGUIUtility.TempContent("-") : new GUIContent(previewInfo.packageShortUrl, "View in browser"), AssetStoreAssetInspector.styles.link, new GUILayoutOption[0]))
            Application.OpenURL(previewInfo.packageShortUrl);
          if (GUI.enabled)
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
          GUI.enabled = enabled2;
          GUILayout.EndHorizontal();
          EditorGUILayout.LabelField("Publisher", previewInfo != null ? previewInfo.publisherName : "-", new GUILayoutOption[0]);
        }
        if (firstAsset.id != 0)
        {
          GUILayout.BeginHorizontal();
          GUILayout.FlexibleSpace();
          string text = previewInfo == null || !previewInfo.isDownloadable ? "Buy for " + firstAsset.price : "Import package";
          bool enabled2 = GUI.enabled;
          bool flag2 = previewInfo != null && (double) previewInfo.buildProgress >= 0.0;
          bool flag3 = previewInfo != null && (double) previewInfo.downloadProgress >= 0.0;
          if (flag2 || flag3 || previewInfo == null)
          {
            text = string.Empty;
            GUI.enabled = false;
          }
          if (GUILayout.Button(text, new GUILayoutOption[2]{ GUILayout.Height(40f), GUILayout.Width(120f) }))
          {
            if (previewInfo.isDownloadable)
              this.ImportPackage(firstAsset);
            else
              this.InitiateBuySelected();
            GUIUtility.ExitGUI();
          }
          if (Event.current.type == EventType.Repaint)
          {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.height -= 4f;
            float width = lastRect.width;
            lastRect.width = lastRect.height;
            lastRect.y += 2f;
            lastRect.x += 2f;
            if (flag2 || flag3)
            {
              lastRect.width = (float) ((double) width - (double) lastRect.height - 4.0);
              lastRect.x += lastRect.height;
              EditorGUI.ProgressBar(lastRect, !flag3 ? previewInfo.buildProgress : previewInfo.downloadProgress, !flag3 ? "Building" : "Downloading");
            }
          }
          GUI.enabled = enabled2;
          GUILayout.Space(4f);
          if (GUILayout.Button("Open Asset Store", new GUILayoutOption[2]{ GUILayout.Height(40f), GUILayout.Width(120f) }))
          {
            AssetStoreAssetInspector.OpenItemInAssetStore(firstAsset);
            GUIUtility.ExitGUI();
          }
          GUILayout.FlexibleSpace();
          GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
      }
      EditorWrapper previewEditor = this.previewEditor;
      if (previewEditor != null && firstAsset != null && firstAsset.HasLivePreview)
        previewEditor.OnAssetStoreInspectorGUI();
      GUI.enabled = enabled1;
      EditorGUILayout.EndVertical();
    }

    public static void OpenItemInAssetStore(AssetStoreAsset activeAsset)
    {
      if (activeAsset.id == 0)
        return;
      AssetStore.Open(string.Format("content/{0}?assetID={1}", (object) activeAsset.packageID, (object) activeAsset.id));
      Analytics.Track(string.Format("/AssetStore/ViewInStore/{0}/{1}", (object) activeAsset.packageID, (object) activeAsset.id));
    }

    private static string intToSizeString(int inValue)
    {
      if (inValue < 0)
        return "unknown";
      float num = (float) inValue;
      string[] strArray = new string[5]{ "TB", "GB", "MB", "KB", "Bytes" };
      int index;
      for (index = strArray.Length - 1; (double) num > 1000.0 && index >= 0; --index)
        num /= 1000f;
      if (index < 0)
        return "<error>";
      return string.Format("{0:#.##} {1}", (object) num, (object) strArray[index]);
    }

    public override bool HasPreviewGUI()
    {
      if (this.target != (Object) null)
        return AssetStoreAssetSelection.Count != 0;
      return false;
    }

    public void OnEnable()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      AssetStoreUtils.RegisterDownloadDelegate((ScriptableObject) this);
    }

    public void OnDisable()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      if (this.m_PreviewEditor != null)
      {
        this.m_PreviewEditor.Dispose();
        this.m_PreviewEditor = (EditorWrapper) null;
      }
      if (this.m_PreviewObject != (Object) null)
        this.m_PreviewObject = (Object) null;
      AssetStoreUtils.UnRegisterDownloadDelegate((ScriptableObject) this);
    }

    public override void OnPreviewSettings()
    {
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null)
        return;
      EditorWrapper previewEditor = this.previewEditor;
      if (previewEditor == null || !firstAsset.HasLivePreview)
        return;
      previewEditor.OnPreviewSettings();
    }

    public override string GetInfoString()
    {
      EditorWrapper previewEditor = this.previewEditor;
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null)
        return "No item selected";
      if (previewEditor != null && firstAsset.HasLivePreview)
        return previewEditor.GetInfoString();
      return string.Empty;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_PreviewObject == (Object) null)
        return;
      EditorWrapper previewEditor = this.previewEditor;
      if (previewEditor != null && this.m_PreviewObject is AnimationClip)
        previewEditor.OnPreviewGUI(r, background);
      else
        this.OnInteractivePreviewGUI(r, background);
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      EditorWrapper previewEditor = this.previewEditor;
      if (previewEditor != null)
        previewEditor.OnInteractivePreviewGUI(r, background);
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null || firstAsset.HasLivePreview || string.IsNullOrEmpty(firstAsset.dynamicPreviewURL))
        return;
      GUIContent statusWheel = AssetStoreAssetInspector.StatusWheel;
      r.y += (float) (((double) r.height - (double) statusWheel.image.height) / 2.0);
      r.x += (float) (((double) r.width - (double) statusWheel.image.width) / 2.0);
      GUI.Label(r, AssetStoreAssetInspector.StatusWheel);
      this.Repaint();
    }

    public override GUIContent GetPreviewTitle()
    {
      return GUIContent.Temp("Asset Store Preview");
    }

    private void InitiateBuySelected(bool firstAttempt)
    {
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null)
        EditorUtility.DisplayDialog("No asset selected", "Please select asset before buying a package", "ok");
      else if (AssetStoreAssetInspector.paymentAvailability == AssetStoreAssetInspector.PaymentAvailability.AnonymousUser)
      {
        if (AssetStoreClient.LoggedIn())
        {
          AssetStoreAssetSelection.RefreshFromServer((AssetStoreAssetSelection.AssetsRefreshed) (() => this.InitiateBuySelected(false)));
        }
        else
        {
          if (!firstAttempt)
            return;
          this.LoginAndInitiateBuySelected();
        }
      }
      else if (AssetStoreAssetInspector.paymentAvailability == AssetStoreAssetInspector.PaymentAvailability.ServiceDisabled)
      {
        if (firstAsset.previewInfo == null)
          return;
        AssetStore.Open(string.Format("content/{0}/directpurchase", (object) firstAsset.packageID));
      }
      else if (AssetStoreAssetInspector.paymentAvailability == AssetStoreAssetInspector.PaymentAvailability.BasketNotEmpty)
      {
        if (firstAsset.previewInfo == null)
          return;
        if (firstAttempt)
          AssetStoreAssetSelection.RefreshFromServer((AssetStoreAssetSelection.AssetsRefreshed) (() => this.InitiateBuySelected(false)));
        else
          AssetStore.Open(string.Format("content/{0}/basketpurchase", (object) firstAsset.packageID));
      }
      else
        AssetStoreInstaBuyWindow.ShowAssetStoreInstaBuyWindow(firstAsset, AssetStoreAssetInspector.s_PurchaseMessage, AssetStoreAssetInspector.s_PaymentMethodCard, AssetStoreAssetInspector.s_PaymentMethodExpire, AssetStoreAssetInspector.s_PriceText);
    }

    private void InitiateBuySelected()
    {
      this.InitiateBuySelected(true);
    }

    private void LoginAndInitiateBuySelected()
    {
      AssetStoreLoginWindow.Login("Please login to the Asset Store in order to get payment information about the package.", (AssetStoreLoginWindow.LoginCallback) (errorMessage =>
      {
        if (errorMessage != null)
          return;
        AssetStoreAssetSelection.RefreshFromServer((AssetStoreAssetSelection.AssetsRefreshed) (() => this.InitiateBuySelected(false)));
      }));
    }

    private void ImportPackage(AssetStoreAsset asset)
    {
      if (AssetStoreAssetInspector.paymentAvailability == AssetStoreAssetInspector.PaymentAvailability.AnonymousUser)
        this.LoginAndImport(asset);
      else
        AssetStoreInstaBuyWindow.ShowAssetStoreInstaBuyWindowBuilding(asset);
    }

    private void LoginAndImport(AssetStoreAsset asset)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      AssetStoreLoginWindow.Login("Please login to the Asset Store in order to get download information for the package.", new AssetStoreLoginWindow.LoginCallback(new AssetStoreAssetInspector.\u003CLoginAndImport\u003Ec__AnonStorey82()
      {
        asset = asset
      }.\u003C\u003Em__140));
    }

    private class Styles
    {
      public GUIStyle link = new GUIStyle(EditorStyles.label);
      public GUIContent assetStoreLogo = EditorGUIUtility.IconContent("WelcomeScreen.AssetStoreLogo");

      public Styles()
      {
        this.link.normal.textColor = new Color(0.26f, 0.51f, 0.75f, 1f);
      }
    }

    internal enum PaymentAvailability
    {
      BasketNotEmpty,
      ServiceDisabled,
      AnonymousUser,
      Ok,
    }
  }
}
