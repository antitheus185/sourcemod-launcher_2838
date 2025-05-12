// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.Forms.EditModForm
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace sourcemod_launcher.Forms;

public class EditModForm : Form
{
  public SourceMod Mod;
  public bool Modified;
  private IContainer components;
  private GroupBox gModInfo;
  private TableLayoutPanel tableModInfo;
  private Label labModNameLabel;
  private Label labDisplayedNameLabel;
  private Label labLaunchParamsLabel;
  private TextBox boxLaunchParams;
  private Label labGameLabel;
  private Button butBrowseGame;
  private TextBox boxDisplayedName;
  private TextBox boxModName;
  private TextBox boxDisplayedNameSecond;
  private TextBox boxGamePath;
  private CheckBox chkSteamAppID;
  private Button butCancel;
  private Button butSave;

  public EditModForm(SourceMod mod)
  {
    this.InitializeComponent();
    this.Mod = mod;
    this.boxModName.Text = mod.Title;
    this.boxDisplayedName.Text = mod.DisplayedTitle;
    this.boxDisplayedNameSecond.Text = mod.DisplayedTitle2;
    this.boxGamePath.Text = mod.LauncherPath.Trim('s');
    this.boxLaunchParams.Text = mod.LaunchOptions;
    this.chkSteamAppID.Checked = mod.UsesSteamId;
    int num = (int) this.ShowDialog();
  }

  private void butCancel_Click(object sender, EventArgs e) => this.Close();

  private void butSave_Click(object sender, EventArgs e)
  {
    this.Mod.Title = this.boxModName.Text;
    this.Mod.DisplayedTitle = this.boxDisplayedName.Text;
    this.Mod.DisplayedTitle2 = this.boxDisplayedNameSecond.Text;
    this.Mod.LauncherPath = this.chkSteamAppID.Checked ? "s" + this.boxGamePath.Text : this.boxGamePath.Text;
    this.Mod.LaunchOptions = this.boxLaunchParams.Text;
    this.Mod.WriteSettings();
    this.Modified = true;
    this.Close();
  }

  private void butBrowseGame_Click(object sender, EventArgs e)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Filter = "Game Executable|*.exe";
    if (openFileDialog.ShowDialog() != DialogResult.OK)
      return;
    this.boxGamePath.Text = openFileDialog.FileName;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.gModInfo = new GroupBox();
    this.butBrowseGame = new Button();
    this.tableModInfo = new TableLayoutPanel();
    this.boxDisplayedName = new TextBox();
    this.labModNameLabel = new Label();
    this.labDisplayedNameLabel = new Label();
    this.labGameLabel = new Label();
    this.boxModName = new TextBox();
    this.boxDisplayedNameSecond = new TextBox();
    this.boxGamePath = new TextBox();
    this.labLaunchParamsLabel = new Label();
    this.boxLaunchParams = new TextBox();
    this.chkSteamAppID = new CheckBox();
    this.butCancel = new Button();
    this.butSave = new Button();
    this.gModInfo.SuspendLayout();
    this.tableModInfo.SuspendLayout();
    this.SuspendLayout();
    this.gModInfo.Controls.Add((Control) this.butBrowseGame);
    this.gModInfo.Controls.Add((Control) this.tableModInfo);
    this.gModInfo.Location = new Point(12, 12);
    this.gModInfo.Name = "gModInfo";
    this.gModInfo.Size = new Size(516, 178);
    this.gModInfo.TabIndex = 2;
    this.gModInfo.TabStop = false;
    this.gModInfo.Text = "Mod Info";
    this.butBrowseGame.Location = new Point(434, 96 /*0x60*/);
    this.butBrowseGame.Name = "butBrowseGame";
    this.butBrowseGame.Size = new Size(75, 23);
    this.butBrowseGame.TabIndex = 3;
    this.butBrowseGame.Text = "Browse";
    this.butBrowseGame.UseVisualStyleBackColor = true;
    this.butBrowseGame.Click += new EventHandler(this.butBrowseGame_Click);
    this.tableModInfo.ColumnCount = 2;
    this.tableModInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125f));
    this.tableModInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableModInfo.Controls.Add((Control) this.boxDisplayedName, 1, 1);
    this.tableModInfo.Controls.Add((Control) this.labModNameLabel, 0, 0);
    this.tableModInfo.Controls.Add((Control) this.labDisplayedNameLabel, 0, 1);
    this.tableModInfo.Controls.Add((Control) this.labGameLabel, 0, 3);
    this.tableModInfo.Controls.Add((Control) this.boxModName, 1, 0);
    this.tableModInfo.Controls.Add((Control) this.boxDisplayedNameSecond, 1, 2);
    this.tableModInfo.Controls.Add((Control) this.boxGamePath, 1, 3);
    this.tableModInfo.Controls.Add((Control) this.labLaunchParamsLabel, 0, 5);
    this.tableModInfo.Controls.Add((Control) this.boxLaunchParams, 1, 5);
    this.tableModInfo.Controls.Add((Control) this.chkSteamAppID, 1, 4);
    this.tableModInfo.Location = new Point(7, 20);
    this.tableModInfo.Name = "tableModInfo";
    this.tableModInfo.RowCount = 6;
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.Size = new Size(421, 150);
    this.tableModInfo.TabIndex = 0;
    this.boxDisplayedName.Location = new Point(128 /*0x80*/, 28);
    this.boxDisplayedName.Name = "boxDisplayedName";
    this.boxDisplayedName.Size = new Size(290, 20);
    this.boxDisplayedName.TabIndex = 9;
    this.labModNameLabel.Anchor = AnchorStyles.Left;
    this.labModNameLabel.AutoSize = true;
    this.labModNameLabel.Location = new Point(3, 6);
    this.labModNameLabel.Name = "labModNameLabel";
    this.labModNameLabel.Size = new Size(35, 13);
    this.labModNameLabel.TabIndex = 0;
    this.labModNameLabel.Text = "Name";
    this.labDisplayedNameLabel.Anchor = AnchorStyles.Left;
    this.labDisplayedNameLabel.AutoSize = true;
    this.labDisplayedNameLabel.Location = new Point(3, 31 /*0x1F*/);
    this.labDisplayedNameLabel.Name = "labDisplayedNameLabel";
    this.labDisplayedNameLabel.Size = new Size(84, 13);
    this.labDisplayedNameLabel.TabIndex = 1;
    this.labDisplayedNameLabel.Text = "Displayed Name";
    this.labGameLabel.Anchor = AnchorStyles.Left;
    this.labGameLabel.AutoSize = true;
    this.labGameLabel.Location = new Point(3, 81);
    this.labGameLabel.Name = "labGameLabel";
    this.labGameLabel.Size = new Size(35, 13);
    this.labGameLabel.TabIndex = 4;
    this.labGameLabel.Text = "Game";
    this.boxModName.Location = new Point(128 /*0x80*/, 3);
    this.boxModName.Name = "boxModName";
    this.boxModName.Size = new Size(290, 20);
    this.boxModName.TabIndex = 8;
    this.boxDisplayedNameSecond.Location = new Point(128 /*0x80*/, 53);
    this.boxDisplayedNameSecond.Name = "boxDisplayedNameSecond";
    this.boxDisplayedNameSecond.Size = new Size(290, 20);
    this.boxDisplayedNameSecond.TabIndex = 10;
    this.boxGamePath.Location = new Point(128 /*0x80*/, 78);
    this.boxGamePath.Name = "boxGamePath";
    this.boxGamePath.Size = new Size(290, 20);
    this.boxGamePath.TabIndex = 11;
    this.labLaunchParamsLabel.Anchor = AnchorStyles.Left;
    this.labLaunchParamsLabel.AutoSize = true;
    this.labLaunchParamsLabel.Location = new Point(3, 131);
    this.labLaunchParamsLabel.Name = "labLaunchParamsLabel";
    this.labLaunchParamsLabel.Size = new Size(99, 13);
    this.labLaunchParamsLabel.TabIndex = 6;
    this.labLaunchParamsLabel.Text = "Launch Parameters";
    this.boxLaunchParams.Location = new Point(128 /*0x80*/, 128 /*0x80*/);
    this.boxLaunchParams.Name = "boxLaunchParams";
    this.boxLaunchParams.Size = new Size(290, 20);
    this.boxLaunchParams.TabIndex = 7;
    this.boxLaunchParams.WordWrap = false;
    this.chkSteamAppID.Anchor = AnchorStyles.Left;
    this.chkSteamAppID.AutoSize = true;
    this.chkSteamAppID.Location = new Point(128 /*0x80*/, 104);
    this.chkSteamAppID.Name = "chkSteamAppID";
    this.chkSteamAppID.Size = new Size(92, 17);
    this.chkSteamAppID.TabIndex = 12;
    this.chkSteamAppID.Text = "Steam App ID";
    this.chkSteamAppID.UseVisualStyleBackColor = true;
    this.butCancel.Location = new Point(454, 196);
    this.butCancel.Name = "butCancel";
    this.butCancel.Size = new Size(75, 23);
    this.butCancel.TabIndex = 3;
    this.butCancel.Text = "Cancel";
    this.butCancel.UseVisualStyleBackColor = true;
    this.butCancel.Click += new EventHandler(this.butCancel_Click);
    this.butSave.Location = new Point(373, 196);
    this.butSave.Name = "butSave";
    this.butSave.Size = new Size(75, 23);
    this.butSave.TabIndex = 4;
    this.butSave.Text = "Save";
    this.butSave.UseVisualStyleBackColor = true;
    this.butSave.Click += new EventHandler(this.butSave_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(541, 228);
    this.Controls.Add((Control) this.butSave);
    this.Controls.Add((Control) this.butCancel);
    this.Controls.Add((Control) this.gModInfo);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.Name = nameof (EditModForm);
    this.Text = "Edit Mod";
    this.gModInfo.ResumeLayout(false);
    this.tableModInfo.ResumeLayout(false);
    this.tableModInfo.PerformLayout();
    this.ResumeLayout(false);
  }
}
