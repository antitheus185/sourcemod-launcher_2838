// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.GameInfoFile
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace sourcemod_launcher;

internal class GameInfoFile
{
  private static char[] _ignoreChars = new char[4]
  {
    ' ',
    '\t',
    '{',
    '}'
  };
  public string FilePath;
  public List<GameInfoKey> Keys;

  public GameInfoFile(string filePath)
  {
    this.Keys = new List<GameInfoKey>();
    this.FilePath = File.Exists(filePath) ? filePath : throw new Exception("Game Info does not exist!");
    StreamReader streamReader = File.OpenText(filePath);
    string line;
    while ((line = streamReader.ReadLine()) != null)
    {
      GameInfoKey key = this.GetKey(line);
      if (!(key.Name == "searchpaths"))
      {
        if (!string.IsNullOrEmpty(key.Name))
          this.Keys.Add(key);
      }
      else
        break;
    }
    streamReader.Close();
  }

  private GameInfoKey GetKey(string line)
  {
    line = "@" + line;
    string str1 = "";
    string str2 = "";
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 1; index < line.Length; ++index)
    {
      char ch1 = index + 1 != line.Length ? line[index + 1] : char.MinValue;
      char ch2 = line[index];
      if (ch2 == '"')
        flag2 = !flag2;
      if (!flag2 && ((IEnumerable<char>) GameInfoFile._ignoreChars).Contains<char>(ch2))
      {
        if (str1 != "")
          flag1 = true;
      }
      else if (ch1 != '/' || ch2 != '/')
      {
        if (flag1)
          str2 += ch2.ToString();
        else
          str1 += ch2.ToString();
      }
      else
        break;
    }
    return new GameInfoKey(str1.ToLower().Trim('"'), str2);
  }

  public void WriteKeys(params GameInfoKey[] keys)
  {
    StreamReader streamReader = File.OpenText(this.FilePath);
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = false;
label_10:
    string line;
    while ((line = streamReader.ReadLine()) != null)
    {
      GameInfoKey key1 = this.GetKey(line);
      if (key1.Name == "searchpaths")
        flag = true;
      if (!flag)
      {
        foreach (GameInfoKey key2 in keys)
        {
          if (key1.Name == key2.Name)
          {
            stringBuilder.AppendLine(key2.ToString());
            goto label_10;
          }
        }
      }
      stringBuilder.AppendLine(line);
    }
    string str = Path.Combine(Directory.GetParent(this.FilePath).FullName, "gameinfo_original.txt");
    if (!File.Exists(str))
      File.Copy(this.FilePath, str);
    streamReader.Close();
    File.WriteAllText(this.FilePath, stringBuilder.ToString());
  }

  public string GetValue(string key)
  {
    return this.Keys.Any<GameInfoKey>((Func<GameInfoKey, bool>) (x => x.Name == key)) ? this.Keys.Where<GameInfoKey>((Func<GameInfoKey, bool>) (x => x.Name.ToLower() == key)).First<GameInfoKey>().Value : "";
  }
}
