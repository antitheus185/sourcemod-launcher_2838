// Decompiled with JetBrains decompiler
// Type: sourcemod_launcher.Forms.RemoveModForm
// Assembly: sourcemod-launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CFEDC7E-164D-462B-A05E-BAF219559056
// Assembly location: C:\Users\veeanti\Downloads\sourcemod-launcher\sourcemod-launcher.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace sourcemod_launcher.Forms;

public class RemoveModForm : Form
{
  public RemoveModResult Result;
  private IContainer components;
  private Label labConfirm;
  private CheckBox chkDeleteFIles;
  private TableLayoutPanel tableLayoutPanel1;
  private Button butCancel;
  private Button butConfirm;

  public RemoveModForm()
  {
    this.InitializeComponent();
    int num = (int) this.ShowDialog();
  }

  private void butConfirm_Click(object sender, EventArgs e)
  {
    this.Result = this.chkDeleteFIles.Checked ? RemoveModResult.YesWithFiles : RemoveModResult.Yes;
    this.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.labConfirm = new Label();
    this.chkDeleteFIles = new CheckBox();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.butCancel = new Button();
    this.butConfirm = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.labConfirm.Anchor = AnchorStyles.Left;
    this.labConfirm.AutoSize = true;
    this.labConfirm.Location = new Point(3, 6);
    this.labConfirm.Name = "labConfirm";
    this.labConfirm.Size = new Size(205, 13);
    this.labConfirm.TabIndex = 0;
    this.labConfirm.Text = "Are you sure to delete the selected mods?";
    this.chkDeleteFIles.Anchor = AnchorStyles.Left;
    this.chkDeleteFIles.AutoSize = true;
    this.chkDeleteFIles.Location = new Point(3, 29);
    this.chkDeleteFIles.Name = "chkDeleteFIles";
    this.chkDeleteFIles.Size = new Size(185, 17);
    this.chkDeleteFIles.TabIndex = 1;
    this.chkDeleteFIles.Text = "Also permanently delete their files.";
    this.chkDeleteFIles.UseVisualStyleBackColor = true;
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.Controls.Add((Control) this.labConfirm, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.chkDeleteFIles, 0, 1);
    this.tableLayoutPanel1.Location = new Point(12, 12);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));
    this.tableLayoutPanel1.Size = new Size(251, 50);
    this.tableLayoutPanel1.TabIndex = 2;
    this.butCancel.Location = new Point(188, 68);
    this.butCancel.Name = "butCancel";
    this.butCancel.Size = new Size(75, 23);
    this.butCancel.TabIndex = 3;
    this.butCancel.Text = "Cancel";
    this.butCancel.UseVisualStyleBackColor = true;
    this.butConfirm.Location = new Point(12, 68);
    this.butConfirm.Name = "butConfirm";
    this.butConfirm.Size = new Size(75, 23);
    this.butConfirm.TabIndex = 4;
    this.butConfirm.Text = "Yes";
    this.butConfirm.UseVisualStyleBackColor = true;
    this.butConfirm.Click += new EventHandler(this.butConfirm_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(273, 102);
    this.Controls.Add((Control) this.butConfirm);
    this.Controls.Add((Control) this.butCancel);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.Name = nameof (RemoveModForm);
    this.Text = "Remove Mod";
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
