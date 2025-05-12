// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.MainForm
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using sourcemod_launcher.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace sourcemod_launcher;

public class MainForm : Form
{
  private List<SourceMod> _mods;
  private Size _maxStartSize = new Size(900, 600);
  private IContainer components;
  private GroupBox gModList;
  private DataGridView dgvMods;
  private Button butRemoveMod;
  private Button butAddMod;
  private GroupBox gModInfo;
  private Button butEditMod;
  private Button butLaunchMod;
  private TableLayoutPanel tableModInfo;
  private Label labLaunchParamsLabel;
  private Label labDisplayedName;
  private Label labModName;
  private Label labModNameLabel;
  private Label labDisplayedNameLabel;
  private Label labGameLabel;
  private Label labGame;
  private TextBox labLaunchParams;
  private Label labFilePath;
  private Label labFilePathLabel;
  private GroupBox gCommonLaunchParam;
  private TextBox boxCommonLaunchParams;
  private Button butExplore;
  private RichTextBox rboxDragDrop;
  private Label label1;
  private DataGridViewTextBoxColumn dgvcModName;
  private DataGridViewTextBoxColumn dgvcDisplayedName;
  private DataGridViewTextBoxColumn dgvcGame;
  private DataGridViewTextBoxColumn dgvcFilePath;
  private DataGridViewTextBoxColumn dgvcLaunchParams;
  private DataGridViewTextBoxColumn dgvcFullPath;
  private Button butShortcut;

  private int _dgvModsCurrentlySelected => this.dgvMods.SelectedCells[0].RowIndex;

  private SourceMod _currentSelectedMod
  {
    get
    {
      if (this._mods.Count == 0)
        return (SourceMod) null;
      return this.dgvMods.SelectedCells.Count == 0 ? this._mods.First<SourceMod>() : this._mods.Where<SourceMod>((Func<SourceMod, bool>) (x => x.FilePath == (string) this.dgvMods.Rows[this._dgvModsCurrentlySelected].Cells[5].Value)).First<SourceMod>();
    }
  }

  private int ModIndexToDGVIndex(int modIndex)
  {
    foreach (DataGridViewRow row in (IEnumerable) this.dgvMods.Rows)
    {
      if ((string) row.Cells[5].Value == this._mods[modIndex].FilePath)
        return row.Index;
    }
    return -1;
  }

  private float Area(Size s) => (float) (s.Width * s.Height);

  public MainForm()
  {
    this._mods = new List<SourceMod>();
    this.InitializeComponent();
    this.FillLabels();
    this.dgvMods.SelectionChanged += new EventHandler(this.DgvMods_SelectionChanged);
    Directory.EnumerateDirectories(Program.SourceModFolder).Concat<string>((IEnumerable<string>) Program.IncludedSettings.Entries).ToList<string>().ForEach((Action<string>) (x =>
    {
      if (Program.OmittedSettings.Entries.Contains(x))
        return;
      SourceMod mod;
      try
      {
        mod = new SourceMod(x);
      }
      catch
      {
        return;
      }
      this.AddMod(mod);
    }));
    this.dgvMods.ClearSelection();
    this.dgvMods_Highlight(0);
    this.boxCommonLaunchParams.Text = Program.GeneralSettings.Get(0);
    this.rboxDragDrop.AllowDrop = true;
    this.rboxDragDrop.DragDrop += new DragEventHandler(this.RboxDragDrop_DragDrop);
    this.rboxDragDrop.KeyPress += new KeyPressEventHandler(this.RboxDragDrop_KeyPress);
    this.RboxDragDrop_KeyPress((object) null, (KeyPressEventArgs) null);
    this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
    this.Size = new Size(int.Parse("0" + Program.GeneralSettings.Get(1)), int.Parse("0" + Program.GeneralSettings.Get(2)));
    this.Size = (double) this.Area(this.Size) > (double) this.Area(this._maxStartSize) ? this._maxStartSize : this.Size;
    this.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
  }

  private void MainForm_SizeChanged(object sender, EventArgs e)
  {
    Program.GeneralSettings.Set(1, this.Width.ToString());
    Program.GeneralSettings.Set(2, this.Height.ToString());
    bool flag = (double) Math.Abs(this.Area(Screen.PrimaryScreen.Bounds.Size - this.Size) / this.Area(Screen.PrimaryScreen.Bounds.Size - this.MinimumSize)) < 0.5;
    this.dgvcDisplayedName.Width = flag ? 300 : 100;
    this.dgvcLaunchParams.Width = flag ? 500 : 150;
  }

  private void RboxDragDrop_KeyPress(object sender, KeyPressEventArgs e)
  {
    this.rboxDragDrop.Text = "\n\nDrag and Drop files here to move them into appropriate folders\n(.cfg into cfg folder, .sav into save folder, etc..)";
    if (e == null)
      return;
    e.Handled = true;
  }

  private void RboxDragDrop_DragDrop(object sender, DragEventArgs e)
  {
    this._currentSelectedMod.CopyFile((e.Data?.GetData(DataFormats.FileDrop) as string[])[0] ?? "");
  }

  private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    Program.GeneralSettings.WriteSettings();
    Program.OmittedSettings.WriteSettings();
    Program.IncludedSettings.WriteSettings();
  }

  private void AddMod(SourceMod mod)
  {
    if (this._mods.Any<SourceMod>((Func<SourceMod, bool>) (x => x.FilePath == mod.FilePath)))
      return;
    this._mods.Add(mod);
    this.dgvMods.Rows.Add((object[]) new string[6]
    {
      mod.Title,
      mod.GetDisplayedTitle(),
      mod.GetLauncher(),
      mod.GetFilePath(),
      mod.LaunchOptions,
      mod.FilePath
    });
    this.dgvMods.ClearSelection();
    this.dgvMods_Highlight(this.dgvMods.Rows.Count - 1);
  }

  private void UpdateMod(int modIndex, SourceMod mod)
  {
    this.dgvMods.Rows[this.ModIndexToDGVIndex(modIndex)].SetValues((object[]) new string[6]
    {
      mod.Title,
      mod.GetDisplayedTitle(),
      mod.GetLauncher(),
      mod.GetFilePath(),
      mod.LaunchOptions,
      mod.FilePath
    });
    this._mods[modIndex] = new SourceMod(mod);
  }

  private void dgvMods_Highlight(int i)
  {
    if (this.dgvMods.Rows.Count <= 0)
      return;
    this.dgvMods.Rows[i].Cells[0].Selected = true;
    this.dgvMods.FirstDisplayedScrollingRowIndex = i;
  }

  private void DgvMods_SelectionChanged(object sender, EventArgs e)
  {
    this.FillLabels(this._currentSelectedMod);
  }

  private void FillLabels(SourceMod mod = null)
  {
    this.labModName.Text = mod?.Title ?? "";
    this.labDisplayedName.Text = mod?.GetDisplayedTitle() ?? "";
    this.labGame.Text = mod?.GetLauncher() ?? "";
    this.labFilePath.Text = mod?.GetFilePath(true) ?? "";
    this.labLaunchParams.Text = mod?.LaunchOptions ?? "";
  }

  private void butLaunchMod_Click(object sender, EventArgs e) => this._currentSelectedMod.Launch();

  private void boxCommonLaunchParams_TextChanged(object sender, EventArgs e)
  {
    Program.GeneralSettings.Set(0, this.boxCommonLaunchParams.Text);
  }

  private void butRemoveMod_Click(object sender, EventArgs e)
  {
    RemoveModResult result = new RemoveModForm().Result;
    if (result == RemoveModResult.No)
      return;
    List<string> stringList = new List<string>();
    foreach (DataGridViewCell selectedCell in (BaseCollection) this.dgvMods.SelectedCells)
    {
      if (!stringList.Contains((string) selectedCell.OwningRow.Cells[5].Value))
        stringList.Add((string) selectedCell.OwningRow.Cells[5].Value);
    }
    foreach (string str in stringList)
    {
      string path = str;
      foreach (DataGridViewRow row in (IEnumerable) this.dgvMods.Rows)
      {
        if ((string) row.Cells[5].Value == path)
        {
          this.dgvMods.Rows.Remove(row);
          break;
        }
      }
      SourceMod sourceMod = this._mods.Where<SourceMod>((Func<SourceMod, bool>) (x => x.FilePath == path)).First<SourceMod>();
      if (result == RemoveModResult.YesWithFiles)
        sourceMod.DeleteFiles();
      Program.OmittedSettings.AddEntry(path);
      Program.IncludedSettings.RemoveEntry(path);
      this._mods.Remove(sourceMod);
    }
    if (this._mods.Count<SourceMod>() != 0)
      return;
    this.FillLabels();
  }

  private void butAddMod_Click(object sender, EventArgs e)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Filter = "gameinfo file|gameinfo.txt";
    if (openFileDialog.ShowDialog() != DialogResult.OK)
      return;
    string path = Directory.GetParent(openFileDialog.FileName).FullName;
    if (!this._mods.Any<SourceMod>((Func<SourceMod, bool>) (x => x.FilePath == path)))
    {
      Program.IncludedSettings.AddEntry(path);
      Program.OmittedSettings.RemoveEntry(path);
      this.AddMod(new SourceMod(path));
    }
    else
    {
      int num = (int) MessageBox.Show("A mod with the same file path is already present!", "Mod already included!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void butEditMod_Click(object sender, EventArgs e)
  {
    EditModForm editModForm = new EditModForm(this._currentSelectedMod);
    if (!editModForm.Modified)
      return;
    this.UpdateMod(this._mods.IndexOf(this._currentSelectedMod), editModForm.Mod);
    this.dgvMods_Highlight(this._dgvModsCurrentlySelected);
    this.FillLabels(this._currentSelectedMod);
  }

  private void butExplore_Click(object sender, EventArgs e)
  {
    Process.Start(this._currentSelectedMod.FilePath);
  }

  private void butShortcut_Click(object sender, EventArgs e)
  {
    if (this._currentSelectedMod == null)
      return;
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Filter = "bat file|*.bat";
    if (saveFileDialog.ShowDialog() != DialogResult.OK)
      return;
    this._currentSelectedMod.MakeShortcut(saveFileDialog.FileName);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
    this.gModList = new GroupBox();
    this.butExplore = new Button();
    this.butEditMod = new Button();
    this.butRemoveMod = new Button();
    this.butLaunchMod = new Button();
    this.butAddMod = new Button();
    this.dgvMods = new DataGridView();
    this.dgvcModName = new DataGridViewTextBoxColumn();
    this.dgvcDisplayedName = new DataGridViewTextBoxColumn();
    this.dgvcGame = new DataGridViewTextBoxColumn();
    this.dgvcFilePath = new DataGridViewTextBoxColumn();
    this.dgvcLaunchParams = new DataGridViewTextBoxColumn();
    this.dgvcFullPath = new DataGridViewTextBoxColumn();
    this.gModInfo = new GroupBox();
    this.tableModInfo = new TableLayoutPanel();
    this.labFilePath = new Label();
    this.labDisplayedName = new Label();
    this.labModName = new Label();
    this.labModNameLabel = new Label();
    this.labDisplayedNameLabel = new Label();
    this.labLaunchParamsLabel = new Label();
    this.labLaunchParams = new TextBox();
    this.labGameLabel = new Label();
    this.labGame = new Label();
    this.labFilePathLabel = new Label();
    this.gCommonLaunchParam = new GroupBox();
    this.boxCommonLaunchParams = new TextBox();
    this.rboxDragDrop = new RichTextBox();
    this.label1 = new Label();
    this.butShortcut = new Button();
    this.gModList.SuspendLayout();
    ((ISupportInitialize) this.dgvMods).BeginInit();
    this.gModInfo.SuspendLayout();
    this.tableModInfo.SuspendLayout();
    this.gCommonLaunchParam.SuspendLayout();
    this.SuspendLayout();
    this.gModList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.gModList.Controls.Add((Control) this.butShortcut);
    this.gModList.Controls.Add((Control) this.butExplore);
    this.gModList.Controls.Add((Control) this.butEditMod);
    this.gModList.Controls.Add((Control) this.butRemoveMod);
    this.gModList.Controls.Add((Control) this.butLaunchMod);
    this.gModList.Controls.Add((Control) this.butAddMod);
    this.gModList.Controls.Add((Control) this.dgvMods);
    this.gModList.Location = new Point(13, 13);
    this.gModList.Name = "gModList";
    this.gModList.Size = new Size(663, 300);
    this.gModList.TabIndex = 0;
    this.gModList.TabStop = false;
    this.gModList.Text = "Mods List";
    this.butExplore.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.butExplore.Location = new Point(392, 271);
    this.butExplore.Name = "butExplore";
    this.butExplore.Size = new Size(103, 23);
    this.butExplore.TabIndex = 4;
    this.butExplore.Text = "Show in Explorer";
    this.butExplore.UseVisualStyleBackColor = true;
    this.butExplore.Click += new EventHandler(this.butExplore_Click);
    this.butEditMod.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.butEditMod.Location = new Point(501, 271);
    this.butEditMod.Name = "butEditMod";
    this.butEditMod.Size = new Size(75, 23);
    this.butEditMod.TabIndex = 3;
    this.butEditMod.Text = "Edit";
    this.butEditMod.UseVisualStyleBackColor = true;
    this.butEditMod.Click += new EventHandler(this.butEditMod_Click);
    this.butRemoveMod.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.butRemoveMod.Location = new Point(6, 271);
    this.butRemoveMod.Name = "butRemoveMod";
    this.butRemoveMod.Size = new Size(75, 23);
    this.butRemoveMod.TabIndex = 2;
    this.butRemoveMod.Text = "Remove";
    this.butRemoveMod.UseVisualStyleBackColor = true;
    this.butRemoveMod.Click += new EventHandler(this.butRemoveMod_Click);
    this.butLaunchMod.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.butLaunchMod.Location = new Point(582, 271);
    this.butLaunchMod.Name = "butLaunchMod";
    this.butLaunchMod.Size = new Size(75, 23);
    this.butLaunchMod.TabIndex = 2;
    this.butLaunchMod.Text = "Launch";
    this.butLaunchMod.UseVisualStyleBackColor = true;
    this.butLaunchMod.Click += new EventHandler(this.butLaunchMod_Click);
    this.butAddMod.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.butAddMod.Location = new Point(87, 271);
    this.butAddMod.Name = "butAddMod";
    this.butAddMod.Size = new Size(75, 23);
    this.butAddMod.TabIndex = 1;
    this.butAddMod.Text = "Add";
    this.butAddMod.UseVisualStyleBackColor = true;
    this.butAddMod.Click += new EventHandler(this.butAddMod_Click);
    this.dgvMods.AllowUserToAddRows = false;
    this.dgvMods.AllowUserToDeleteRows = false;
    this.dgvMods.AllowUserToOrderColumns = true;
    this.dgvMods.AllowUserToResizeRows = false;
    this.dgvMods.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.dgvMods.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dgvMods.Columns.AddRange((DataGridViewColumn) this.dgvcModName, (DataGridViewColumn) this.dgvcDisplayedName, (DataGridViewColumn) this.dgvcGame, (DataGridViewColumn) this.dgvcFilePath, (DataGridViewColumn) this.dgvcLaunchParams, (DataGridViewColumn) this.dgvcFullPath);
    this.dgvMods.Location = new Point(6, 20);
    this.dgvMods.Name = "dgvMods";
    this.dgvMods.ReadOnly = true;
    this.dgvMods.RowHeadersVisible = false;
    this.dgvMods.Size = new Size(650, 245);
    this.dgvMods.TabIndex = 0;
    this.dgvcModName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
    this.dgvcModName.HeaderText = "Mod Name";
    this.dgvcModName.Name = "dgvcModName";
    this.dgvcModName.ReadOnly = true;
    this.dgvcModName.Width = 78;
    this.dgvcDisplayedName.HeaderText = "Displayed Name";
    this.dgvcDisplayedName.Name = "dgvcDisplayedName";
    this.dgvcDisplayedName.ReadOnly = true;
    this.dgvcGame.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
    this.dgvcGame.HeaderText = "Game";
    this.dgvcGame.Name = "dgvcGame";
    this.dgvcGame.ReadOnly = true;
    this.dgvcGame.Width = 60;
    this.dgvcFilePath.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
    this.dgvcFilePath.HeaderText = "File Path";
    this.dgvcFilePath.Name = "dgvcFilePath";
    this.dgvcFilePath.ReadOnly = true;
    this.dgvcFilePath.Width = 68;
    this.dgvcLaunchParams.HeaderText = "Launch Parameters";
    this.dgvcLaunchParams.Name = "dgvcLaunchParams";
    this.dgvcLaunchParams.ReadOnly = true;
    this.dgvcLaunchParams.Width = 150;
    this.dgvcFullPath.HeaderText = "you shouldn't see this...";
    this.dgvcFullPath.Name = "dgvcFullPath";
    this.dgvcFullPath.ReadOnly = true;
    this.dgvcFullPath.Visible = false;
    this.gModInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.gModInfo.Controls.Add((Control) this.tableModInfo);
    this.gModInfo.Location = new Point(13, 319);
    this.gModInfo.Name = "gModInfo";
    this.gModInfo.Size = new Size(663, 165);
    this.gModInfo.TabIndex = 1;
    this.gModInfo.TabStop = false;
    this.gModInfo.Text = "Mod Info";
    this.tableModInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tableModInfo.AutoSize = true;
    this.tableModInfo.ColumnCount = 2;
    this.tableModInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125f));
    this.tableModInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableModInfo.Controls.Add((Control) this.labFilePath, 1, 3);
    this.tableModInfo.Controls.Add((Control) this.labDisplayedName, 1, 1);
    this.tableModInfo.Controls.Add((Control) this.labModName, 1, 0);
    this.tableModInfo.Controls.Add((Control) this.labModNameLabel, 0, 0);
    this.tableModInfo.Controls.Add((Control) this.labDisplayedNameLabel, 0, 1);
    this.tableModInfo.Controls.Add((Control) this.labLaunchParamsLabel, 0, 4);
    this.tableModInfo.Controls.Add((Control) this.labLaunchParams, 1, 4);
    this.tableModInfo.Controls.Add((Control) this.labGameLabel, 0, 2);
    this.tableModInfo.Controls.Add((Control) this.labGame, 1, 2);
    this.tableModInfo.Controls.Add((Control) this.labFilePathLabel, 0, 3);
    this.tableModInfo.Location = new Point(7, 20);
    this.tableModInfo.Name = "tableModInfo";
    this.tableModInfo.RowCount = 5;
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableModInfo.Size = new Size(649, 135);
    this.tableModInfo.TabIndex = 0;
    this.labFilePath.Anchor = AnchorStyles.Left;
    this.labFilePath.AutoSize = true;
    this.labFilePath.Location = new Point(128 /*0x80*/, 91);
    this.labFilePath.Name = "labFilePath";
    this.labFilePath.Size = new Size(56, 13);
    this.labFilePath.TabIndex = 9;
    this.labFilePath.Text = "<file path>";
    this.labDisplayedName.Anchor = AnchorStyles.Left;
    this.labDisplayedName.AutoSize = true;
    this.labDisplayedName.Location = new Point(128 /*0x80*/, 41);
    this.labDisplayedName.Name = "labDisplayedName";
    this.labDisplayedName.Size = new Size(92, 13);
    this.labDisplayedName.TabIndex = 3;
    this.labDisplayedName.Text = "<displayed name>";
    this.labModName.Anchor = AnchorStyles.Left;
    this.labModName.AutoSize = true;
    this.labModName.Font = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Bold);
    this.labModName.Location = new Point(128 /*0x80*/, 9);
    this.labModName.Name = "labModName";
    this.labModName.Size = new Size(96 /*0x60*/, 16 /*0x10*/);
    this.labModName.TabIndex = 2;
    this.labModName.Text = "<mod name>";
    this.labModNameLabel.Anchor = AnchorStyles.Left;
    this.labModNameLabel.AutoSize = true;
    this.labModNameLabel.Location = new Point(3, 11);
    this.labModNameLabel.Name = "labModNameLabel";
    this.labModNameLabel.Size = new Size(35, 13);
    this.labModNameLabel.TabIndex = 0;
    this.labModNameLabel.Text = "Name";
    this.labDisplayedNameLabel.Anchor = AnchorStyles.Left;
    this.labDisplayedNameLabel.AutoSize = true;
    this.labDisplayedNameLabel.Location = new Point(3, 41);
    this.labDisplayedNameLabel.Name = "labDisplayedNameLabel";
    this.labDisplayedNameLabel.Size = new Size(84, 13);
    this.labDisplayedNameLabel.TabIndex = 1;
    this.labDisplayedNameLabel.Text = "Displayed Name";
    this.labLaunchParamsLabel.Anchor = AnchorStyles.Left;
    this.labLaunchParamsLabel.AutoSize = true;
    this.labLaunchParamsLabel.Location = new Point(3, 116);
    this.labLaunchParamsLabel.Name = "labLaunchParamsLabel";
    this.labLaunchParamsLabel.Size = new Size(99, 13);
    this.labLaunchParamsLabel.TabIndex = 6;
    this.labLaunchParamsLabel.Text = "Launch Parameters";
    this.labLaunchParams.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.labLaunchParams.Location = new Point(128 /*0x80*/, 113);
    this.labLaunchParams.Name = "labLaunchParams";
    this.labLaunchParams.ReadOnly = true;
    this.labLaunchParams.Size = new Size(518, 20);
    this.labLaunchParams.TabIndex = 7;
    this.labLaunchParams.WordWrap = false;
    this.labGameLabel.Anchor = AnchorStyles.Left;
    this.labGameLabel.AutoSize = true;
    this.labGameLabel.Location = new Point(3, 66);
    this.labGameLabel.Name = "labGameLabel";
    this.labGameLabel.Size = new Size(35, 13);
    this.labGameLabel.TabIndex = 4;
    this.labGameLabel.Text = "Game";
    this.labGame.Anchor = AnchorStyles.Left;
    this.labGame.AutoSize = true;
    this.labGame.Location = new Point(128 /*0x80*/, 66);
    this.labGame.Name = "labGame";
    this.labGame.Size = new Size(60, 13);
    this.labGame.TabIndex = 5;
    this.labGame.Text = "<launcher>";
    this.labFilePathLabel.Anchor = AnchorStyles.Left;
    this.labFilePathLabel.AutoSize = true;
    this.labFilePathLabel.Location = new Point(3, 91);
    this.labFilePathLabel.Name = "labFilePathLabel";
    this.labFilePathLabel.Size = new Size(48 /*0x30*/, 13);
    this.labFilePathLabel.TabIndex = 8;
    this.labFilePathLabel.Text = "File Path";
    this.gCommonLaunchParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.gCommonLaunchParam.Controls.Add((Control) this.boxCommonLaunchParams);
    this.gCommonLaunchParam.Location = new Point(13, 490);
    this.gCommonLaunchParam.Name = "gCommonLaunchParam";
    this.gCommonLaunchParam.Size = new Size(331, 52);
    this.gCommonLaunchParam.TabIndex = 2;
    this.gCommonLaunchParam.TabStop = false;
    this.gCommonLaunchParam.Text = "Additional Launch Paramters (applies to all mods)";
    this.boxCommonLaunchParams.Anchor = AnchorStyles.Left | AnchorStyles.Right;
    this.boxCommonLaunchParams.Location = new Point(7, 20);
    this.boxCommonLaunchParams.Name = "boxCommonLaunchParams";
    this.boxCommonLaunchParams.Size = new Size(318, 20);
    this.boxCommonLaunchParams.TabIndex = 0;
    this.boxCommonLaunchParams.TextChanged += new EventHandler(this.boxCommonLaunchParams_TextChanged);
    this.rboxDragDrop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.rboxDragDrop.BackColor = SystemColors.ControlLight;
    this.rboxDragDrop.BorderStyle = BorderStyle.None;
    this.rboxDragDrop.ForeColor = SystemColors.WindowFrame;
    this.rboxDragDrop.Location = new Point(350, 490);
    this.rboxDragDrop.Name = "rboxDragDrop";
    this.rboxDragDrop.RightToLeft = RightToLeft.Yes;
    this.rboxDragDrop.Size = new Size(326, 52);
    this.rboxDragDrop.TabIndex = 3;
    this.rboxDragDrop.Text = "\n\n\nDrag and Drop files here to move them into appropriate folders";
    this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Font = new Font("Microsoft Sans Serif", 7.25f);
    this.label1.ForeColor = SystemColors.AppWorkspace;
    this.label1.Location = new Point(12, 547);
    this.label1.Name = "label1";
    this.label1.RightToLeft = RightToLeft.Yes;
    this.label1.Size = new Size(228, 13);
    this.label1.TabIndex = 4;
    this.label1.Text = "SourceMod Launcher - by 2838 (2021) - modified slightly by veeanti mainly for learning (2025)";
    this.butShortcut.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.butShortcut.Location = new Point(295, 271);
    this.butShortcut.Name = "butShortcut";
    this.butShortcut.Size = new Size(91, 23);
    this.butShortcut.TabIndex = 5;
    this.butShortcut.Text = "Make Shortcut";
    this.butShortcut.UseVisualStyleBackColor = true;
    this.butShortcut.Click += new EventHandler(this.butShortcut_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(688, 569);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.rboxDragDrop);
    this.Controls.Add((Control) this.gCommonLaunchParam);
    this.Controls.Add((Control) this.gModInfo);
    this.Controls.Add((Control) this.gModList);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.MinimumSize = new Size(704, 608);
    this.Name = nameof (MainForm);
    this.Text = "SourceMod Launcher";
    this.gModList.ResumeLayout(false);
    ((ISupportInitialize) this.dgvMods).EndInit();
    this.gModInfo.ResumeLayout(false);
    this.gModInfo.PerformLayout();
    this.tableModInfo.ResumeLayout(false);
    this.tableModInfo.PerformLayout();
    this.gCommonLaunchParam.ResumeLayout(false);
    this.gCommonLaunchParam.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
