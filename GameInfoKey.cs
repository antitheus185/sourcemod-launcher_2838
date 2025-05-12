// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.GameInfoKey
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

#nullable disable
namespace sourcemod_launcher;

internal struct GameInfoKey(string name, string value)
{
  public string Name = name;
  public string Value = value;

  public override string ToString() => $"\t{this.Name}\t\t{this.Value}";
}
