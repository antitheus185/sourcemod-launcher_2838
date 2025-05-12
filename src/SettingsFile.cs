// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.src.SettingsFile
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace sourcemod_launcher.src;

internal class SettingsFile
{
  public List<string> Entries;
  private string _filePath;

  public SettingsFile(string path)
  {
    this.Entries = new List<string>();
    if (!File.Exists(path))
    {
      File.Create(path);
    }
    else
    {
      this._filePath = path;
      StreamReader streamReader = File.OpenText(path);
      string str;
      while ((str = streamReader.ReadLine()) != null)
        this.Entries.Add(str);
      streamReader.Close();
    }
  }

  public void AddEntry(string entry)
  {
    if (this.Entries.Contains(entry))
      return;
    this.Entries.Add(entry);
  }

  public void RemoveEntry(string entry)
  {
    if (!this.Entries.Contains(entry))
      return;
    this.Entries.Remove(entry);
  }

  public void WriteSettings()
  {
    if (!File.Exists(this._filePath))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (string str in this.Entries.Distinct<string>())
      stringBuilder.AppendLine(str);
    File.WriteAllText(this._filePath, stringBuilder.ToString());
  }

  private void Pad(int to)
  {
    if (this.Entries.Count > to)
      return;
    for (int count = this.Entries.Count; count <= to; ++count)
      this.Entries.Add("");
  }

  public string Get(int index)
  {
    this.Pad(index);
    return this.Entries[index];
  }

  public void Set(int index, string input)
  {
    this.Pad(index);
    this.Entries[index] = input;
  }
}
