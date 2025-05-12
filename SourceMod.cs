// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.SourceMod
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace sourcemod_launcher;

public class SourceMod
{
  public string Title;
  public string DisplayedTitle;
  public string DisplayedTitle2;
  public string LaunchOptions;
  public string FilePath;
  public string LauncherPath;
  private GameInfoFile _gameInfoFile;
  public static List<Tuple<int, string>> SteamAppIDNames = new List<Tuple<int, string>>()
  {
    new Tuple<int, string>(215, "Source SDK Base 2006"),
    new Tuple<int, string>(218, "Source SDK Base 2007"),
    new Tuple<int, string>(220, "Half-Life 2"),
    new Tuple<int, string>(400, "Portal"),
    new Tuple<int, string>(420, "Half-Life 2: Episode Two"),
    new Tuple<int, string>(380, "Half-Life 2: Episode One"),
    new Tuple<int, string>(620, "Portal 2"),
    new Tuple<int, string>(243730, "Source SDK Base 2013 Singleplayer"),
    new Tuple<int, string>(243750, "Source SDK Base 2013 Multiplayer"),
    new Tuple<int, string>(362890, "Black Mesa"),
    new Tuple<int, string>(280, "Half-Life: Source"),
    new Tuple<int, string>(17520, "Synergy"),
    new Tuple<int, string>(4000, "Garry's Mod"),
    new Tuple<int, string>(440, "Team Fortress 2")
  };

  public bool UsesSteamId => this.LauncherPath.Length > 0 && this.LauncherPath[0] == 's';

  public SourceMod(string filePath)
  {
    this._gameInfoFile = new GameInfoFile(Path.Combine(filePath, "gameinfo.txt"));
    this.FilePath = filePath;
    this.Title = this._gameInfoFile.GetValue("game").Trim('"');
    this.DisplayedTitle = this._gameInfoFile.GetValue("title").Trim('"');
    this.DisplayedTitle2 = this._gameInfoFile.GetValue("title2").Trim('"');
    this.LauncherPath = "s" + this._gameInfoFile.GetValue("steamappid").Trim('"');
    this.ReadCustom();
  }

  public SourceMod(params string[] elements)
  {
    this.FilePath = elements[0];
    this._gameInfoFile = new GameInfoFile(Path.Combine(this.FilePath, "gameinfo.txt"));
    this.Title = elements[1];
    this.DisplayedTitle = elements[2];
    this.DisplayedTitle2 = elements[3];
    this.LauncherPath = elements[4];
    this.LaunchOptions = elements[5];
  }

  public SourceMod(SourceMod mod)
  {
    this._gameInfoFile = mod._gameInfoFile;
    this.FilePath = mod.FilePath;
    this.Title = mod.Title;
    this.DisplayedTitle = mod.DisplayedTitle;
    this.DisplayedTitle2 = mod.DisplayedTitle2;
    this.LauncherPath = mod.LauncherPath;
    this.LaunchOptions = mod.LaunchOptions;
  }

  public void ReadCustom()
  {
    string path = Path.Combine(this.FilePath, "slauncher-settings.txt");
    if (!File.Exists(path))
      return;
    StreamReader streamReader = File.OpenText(path);
    this.LaunchOptions = streamReader.ReadLine();
    this.LauncherPath = streamReader.ReadLine();
    streamReader.Close();
  }

  public void WriteCustom()
  {
    string path = Path.Combine(this.FilePath, "slauncher-settings.txt");
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine(this.LaunchOptions);
    stringBuilder.AppendLine(this.LauncherPath);
    string contents = stringBuilder.ToString();
    File.WriteAllText(path, contents);
  }

  public override string ToString() => $"MOD [{this.Title}] LANCHER [{this.LauncherPath}]";

  public void WriteSettings()
  {
    this._gameInfoFile.WriteKeys(new GameInfoKey("game", $"\"{this.Title}\""), new GameInfoKey("title", $"\"{this.DisplayedTitle}\""), new GameInfoKey("title2", $"\"{this.DisplayedTitle2}\""));
    if (this.UsesSteamId)
      this._gameInfoFile.WriteKeys(new GameInfoKey("steamappid", this.LauncherPath.Trim('s') ?? ""));
    this.WriteCustom();
  }

  private string GetFullPath(params string[] subfolders)
  {
    return Path.Combine(this.FilePath, Path.Combine(subfolders));
  }

  public void CopyFile(string path)
  {
    string fileName = Path.GetFileName(path);
    string fullPath1;
    switch (Path.GetExtension(path))
    {
      case ".cfg":
        fullPath1 = this.GetFullPath("cfg", fileName);
        break;
      case ".sav":
        string[] strArray = new string[2];
        string fullPath2;
        if (!File.Exists(this.GetFullPath("save")))
          fullPath2 = this.GetFullPath("SAVE");
        else
          fullPath2 = this.GetFullPath("save");
        strArray[0] = fullPath2;
        strArray[1] = fileName;
        fullPath1 = this.GetFullPath(strArray);
        break;
      default:
        fullPath1 = this.GetFullPath(fileName);
        break;
    }
    if (path == fullPath1)
      return;
    if (File.Exists(fullPath1))
    {
      if (MessageBox.Show(fullPath1 + " already exists! Overwrite?", "Drag and Drop", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      File.Copy(path, fullPath1, true);
    }
    else
      File.Copy(path, fullPath1);
  }

  public void Launch(string launchOptions = "")
  {
    ProcessStartInfo startInfo = (ProcessStartInfo) null;
    if (launchOptions == "")
      launchOptions = this.LaunchOptions;
    launchOptions = $"{launchOptions} {Program.GeneralSettings.Entries[0]}";
    if (this.UsesSteamId)
      startInfo = new ProcessStartInfo()
      {
        FileName = Program.SteamExe,
        Arguments = $"-applaunch {this.LauncherPath.Trim('s')} {launchOptions} -game \"{this.FilePath}\""
      };
    else if (File.Exists(this.LauncherPath))
      startInfo = new ProcessStartInfo()
      {
        FileName = this.LauncherPath,
        Arguments = $" {launchOptions} -game \"{this.FilePath}\""
      };
    if (startInfo == null)
      return;
    Process.Start(startInfo);
  }

  public string GetLauncher()
  {
    if (!this.UsesSteamId)
      return this.LauncherPath;
    int result;
    if (int.TryParse(this.LauncherPath.TrimStart('s'), out result))
      return SourceMod.ConvertAppId(result);
    this.LauncherPath = this.LauncherPath.Substring(1);
    return this.LauncherPath;
  }

  public string GetFilePath(bool full = false)
  {
    return !full && Directory.GetParent(this.FilePath).FullName == Program.SourceModFolder ? Path.GetFileName(this.FilePath) : this.FilePath;
  }

  public string GetDisplayedTitle()
  {
    return $"{this.DisplayedTitle} - {this.DisplayedTitle2}".Trim('-', ' ');
  }

  public void DeleteFiles() => Directory.Delete(this.FilePath, true);

  public static string ConvertAppId(int id)
  {
    return SourceMod.SteamAppIDNames.Any<Tuple<int, string>>((Func<Tuple<int, string>, bool>) (x => x.Item1 == id)) ? SourceMod.SteamAppIDNames.Where<Tuple<int, string>>((Func<Tuple<int, string>, bool>) (x => x.Item1 == id)).First<Tuple<int, string>>().Item2 : "steamappid:" + id.ToString();
  }

  public void MakeShortcut(string filePath)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string str = $"{Program.GeneralSettings.Get(0)} {this.LaunchOptions}";
    if (this.UsesSteamId)
      stringBuilder.Append($"\"{Program.SteamExe}\" -applaunch {this.LauncherPath.Trim('s')} {str}");
    else
      stringBuilder.Append($"\"{this.LauncherPath}\" {str}");
    stringBuilder.AppendLine($" -game \"{this.FilePath}\"");
    stringBuilder.AppendLine("exit");
    File.WriteAllText(filePath, stringBuilder.ToString());
  }
}
