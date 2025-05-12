// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.Program
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using Microsoft.Win32;
using sourcemod_launcher.src;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace sourcemod_launcher;

internal class Program
{
  public static string SteamExe = "";
  public static string SourceModFolder = "";
  public static SettingsFile GeneralSettings;
  public static SettingsFile OmittedSettings;
  public static SettingsFile IncludedSettings;

  [STAThread]
  private static void Main(string[] args)
  {
    try
    {
      Program.SourceModFolder = (string) Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SourceModInstallPath", (object) "");
      Program.SteamExe = (string) Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamExe", (object) "");
      RegistryKey registryKey1 = Registry.CurrentUser?.OpenSubKey("SOFTWARE\\Valve\\Steam\\Apps") ?? (RegistryKey) null;
      if (registryKey1 != null)
      {
        SourceMod.SteamAppIDNames = new List<Tuple<int, string>>();
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
          if (registryKey2 != null)
          {
            try
            {
              if ((int) registryKey2.GetValue("Installed") != 0)
              {
                int num = int.Parse(Path.GetFileName(registryKey2.Name));
                string str = (string) (registryKey2.GetValue("Name") ?? (object) "");
                if (!(str == ""))
                  SourceMod.SteamAppIDNames.Add(new Tuple<int, string>(num, str));
              }
            }
            catch
            {
            }
          }
        }
      }
      Program.GeneralSettings = new SettingsFile("settings.txt");
      Program.OmittedSettings = new SettingsFile("omitted.txt");
      Program.IncludedSettings = new SettingsFile("included.txt");
      Application.EnableVisualStyles();
      CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
    }
    catch (Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Catastrophic disaster of epic proportions!");
      stringBuilder.AppendLine(ex.ToString());
      stringBuilder.AppendLine(ex.InnerException?.ToString());
      stringBuilder.AppendLine("You need to have a Source Engine game either installed through Steam or at least have run one before running this. It's checking your registry to find installed appids through Steam and it appears that you haven't installed one before or you've run this on a fresh Windows install. It won't work unless you install one, TF2 is free if you don't have HL2.");
      int num = (int) MessageBox.Show(stringBuilder.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }
}
