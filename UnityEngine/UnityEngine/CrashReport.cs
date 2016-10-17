// Decompiled with JetBrains decompiler
// Type: UnityEngine.CrashReport
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Holds data for a single application crash event and provides access to all gathered crash reports.</para>
  /// </summary>
  public sealed class CrashReport
  {
    private static object reportsLock = new object();
    private static List<CrashReport> internalReports;
    private readonly string id;
    /// <summary>
    ///   <para>Time, when the crash occured.</para>
    /// </summary>
    public readonly DateTime time;
    /// <summary>
    ///   <para>Crash report data as formatted text.</para>
    /// </summary>
    public readonly string text;

    /// <summary>
    ///   <para>Returns all currently available reports in a new array.</para>
    /// </summary>
    public static CrashReport[] reports
    {
      get
      {
        CrashReport.PopulateReports();
        lock (CrashReport.reportsLock)
          return CrashReport.internalReports.ToArray();
      }
    }

    /// <summary>
    ///   <para>Returns last crash report, or null if no reports are available.</para>
    /// </summary>
    public static CrashReport lastReport
    {
      get
      {
        CrashReport.PopulateReports();
        lock (CrashReport.reportsLock)
        {
          if (CrashReport.internalReports.Count > 0)
            return CrashReport.internalReports[CrashReport.internalReports.Count - 1];
        }
        return (CrashReport) null;
      }
    }

    private CrashReport(string id, DateTime time, string text)
    {
      this.id = id;
      this.time = time;
      this.text = text;
    }

    private static int Compare(CrashReport c1, CrashReport c2)
    {
      long ticks1 = c1.time.Ticks;
      long ticks2 = c2.time.Ticks;
      if (ticks1 > ticks2)
        return 1;
      return ticks1 < ticks2 ? -1 : 0;
    }

    private static void PopulateReports()
    {
      lock (CrashReport.reportsLock)
      {
        if (CrashReport.internalReports != null)
          return;
        string[] local_1 = CrashReport.GetReports();
        CrashReport.internalReports = new List<CrashReport>(local_1.Length);
        foreach (string item_0 in local_1)
        {
          double local_5;
          string local_6;
          CrashReport.GetReportData(item_0, out local_5, out local_6);
          DateTime local_7 = new DateTime(1970, 1, 1).AddSeconds(local_5);
          CrashReport.internalReports.Add(new CrashReport(item_0, local_7, local_6));
        }
        CrashReport.internalReports.Sort(new Comparison<CrashReport>(CrashReport.Compare));
      }
    }

    /// <summary>
    ///   <para>Remove all reports from available reports list.</para>
    /// </summary>
    public static void RemoveAll()
    {
      foreach (CrashReport report in CrashReport.reports)
        report.Remove();
    }

    /// <summary>
    ///   <para>Remove report from available reports list.</para>
    /// </summary>
    public void Remove()
    {
      if (!CrashReport.RemoveReport(this.id))
        return;
      lock (CrashReport.reportsLock)
        CrashReport.internalReports.Remove(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string[] GetReports();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetReportData(string id, out double secondsSinceUnixEpoch, out string text);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool RemoveReport(string id);
  }
}
