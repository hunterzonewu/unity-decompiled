// Decompiled with JetBrains decompiler
// Type: UnityEditor.PurchaseResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class PurchaseResult : AssetStoreResultBase<PurchaseResult>
  {
    public PurchaseResult.Status status;
    public int packageID;
    public string message;

    public PurchaseResult(AssetStoreResultBase<PurchaseResult>.Callback c)
      : base(c)
    {
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      this.packageID = int.Parse(dict["package_id"].AsString());
      this.message = !dict.ContainsKey("message") ? (string) null : dict["message"].AsString(true);
      string str = dict["status"].AsString(true);
      if (str == "basket-not-empty")
        this.status = PurchaseResult.Status.BasketNotEmpty;
      else if (str == "service-disabled")
        this.status = PurchaseResult.Status.ServiceDisabled;
      else if (str == "user-anonymous")
        this.status = PurchaseResult.Status.AnonymousUser;
      else if (str == "password-missing")
        this.status = PurchaseResult.Status.PasswordMissing;
      else if (str == "password-wrong")
        this.status = PurchaseResult.Status.PasswordWrong;
      else if (str == "purchase-declined")
      {
        this.status = PurchaseResult.Status.PurchaseDeclined;
      }
      else
      {
        if (!(str == "ok"))
          return;
        this.status = PurchaseResult.Status.Ok;
      }
    }

    public enum Status
    {
      BasketNotEmpty,
      ServiceDisabled,
      AnonymousUser,
      PasswordMissing,
      PasswordWrong,
      PurchaseDeclined,
      Ok,
    }
  }
}
