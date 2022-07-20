/// <summary>
///   Original Author: Joe Zachary
///   Further Authors: H. James de St. Germain
///   
///   Dates          : 2012-ish - Original 
///                    2020     - Updated for use with ASP Core
///                    
///   This code represents a Windows Form element for a Spreadsheet
///   
///   This code is the "auto-generated" portion of the SimpleSpreadsheetGUI.
///   See the SimpleSpreadsheetGUI.cs for "hand-written" code.
///  
/// </summary>
/// <summary> 
/// Author:    [Deepak] 
/// Partner:   [Keaton] 
/// Date:      [1/21/19] 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and [Your Name(s)] - This work may not be copied for use in Academic Coursework. 
/// 
/// I, [Deepak; Keaton], certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
///     
/// </summary>

using SpreadsheetGrid_Framework;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SpreadsheetPanel
{
    partial class SimpleSpreadsheetGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Help_Menu_Option = new System.Windows.Forms.ToolStripMenuItem();
            this.MainControlArea = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.extraFeatureCheckbox = new System.Windows.Forms.CheckBox();
            this.sample_button = new System.Windows.Forms.Button();
            this.cell_Input_Textbox = new System.Windows.Forms.TextBox();
            this.cell_Input_Label = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Cell_Name_Label = new System.Windows.Forms.Label();
            this.Cell_Value_Label = new System.Windows.Forms.Label();
            this.grid_widget = new SpreadsheetGrid_Framework.SpreadsheetGridWidget();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bg_worker = new BackgroundWorker();

            this.menuStrip.SuspendLayout();
            this.MainControlArea.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.Help_Menu_Option});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(779, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.Open_Menu_Option_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.Save_Menu_Option_Click);
            // 
            // Help_Menu_Option
            // 
            this.Help_Menu_Option.Name = "Help_Menu_Option";
            this.Help_Menu_Option.Size = new System.Drawing.Size(55, 24);
            this.Help_Menu_Option.Text = "Help";
            this.Help_Menu_Option.Click += new System.EventHandler(this.Help_Menu_Option_Click);
            // 
            // MainControlArea
            // 
            this.MainControlArea.AutoSize = true;
            this.MainControlArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainControlArea.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.MainControlArea.Controls.Add(this.tableLayoutPanel2);
            this.MainControlArea.Controls.Add(this.tableLayoutPanel3);
            this.MainControlArea.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MainControlArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainControlArea.Location = new System.Drawing.Point(4, 4);
            this.MainControlArea.Margin = new System.Windows.Forms.Padding(4);
            this.MainControlArea.MinimumSize = new System.Drawing.Size(133, 123);
            this.MainControlArea.Name = "MainControlArea";
            this.MainControlArea.Size = new System.Drawing.Size(771, 123);
            this.MainControlArea.TabIndex = 4;
            
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.sample_button, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cell_Input_Textbox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cell_Input_Label, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.Controls.Add(this.extraFeatureCheckbox, 0, 1);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(402, 100);
            this.tableLayoutPanel2.TabIndex = 3;
            
            // 
            // sample_button
            // 
            this.sample_button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sample_button.BackColor = System.Drawing.Color.Teal;
            this.sample_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sample_button.Location = new System.Drawing.Point(204, 4);
            this.sample_button.Margin = new System.Windows.Forms.Padding(4);
            this.sample_button.Name = "sample_button";
            this.sample_button.Size = new System.Drawing.Size(94, 42);
            this.sample_button.TabIndex = 0;
            this.sample_button.Text = "Submit Input";
            this.sample_button.UseVisualStyleBackColor = false;
            this.sample_button.Click += new System.EventHandler(this.submit_button_Click);
            // 
            // cell_Input_Textbox
            // 
            this.cell_Input_Textbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cell_Input_Textbox.Location = new System.Drawing.Point(104, 14);
            this.cell_Input_Textbox.Margin = new System.Windows.Forms.Padding(4);
            this.cell_Input_Textbox.Name = "cell_Input_Textbox";
            this.cell_Input_Textbox.Size = new System.Drawing.Size(92, 22);
            this.cell_Input_Textbox.TabIndex = 2;
            this.cell_Input_Textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cell_Input_Textbox_KeyDown);
            // 
            // cell_Input_Label
            // 
            this.cell_Input_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cell_Input_Label.AutoSize = true;
            this.cell_Input_Label.Location = new System.Drawing.Point(17, 16);
            this.cell_Input_Label.Name = "cell_Input_Label";
            this.cell_Input_Label.Size = new System.Drawing.Size(66, 17);
            this.cell_Input_Label.TabIndex = 3;
            this.cell_Input_Label.Text = "Cell Input";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.Cell_Name_Label, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.Cell_Value_Label, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(311, 9);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 88);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // Cell_Name_Label
            // 
            this.Cell_Name_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cell_Name_Label.AutoSize = true;
            this.Cell_Name_Label.Location = new System.Drawing.Point(64, 13);
            this.Cell_Name_Label.Name = "Cell_Name_Label";
            this.Cell_Name_Label.Size = new System.Drawing.Size(72, 17);
            this.Cell_Name_Label.TabIndex = 0;
            this.Cell_Name_Label.Text = "Cell Name";
            // 
            // Cell_Value_Label
            // 
            this.Cell_Value_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cell_Value_Label.AutoSize = true;
            this.Cell_Value_Label.Location = new System.Drawing.Point(64, 57);
            this.Cell_Value_Label.Name = "Cell_Value_Label";
            this.Cell_Value_Label.Size = new System.Drawing.Size(71, 17);
            this.Cell_Value_Label.TabIndex = 1;
            this.Cell_Value_Label.Text = "Cell Value";
            // 
            // grid_widget
            // 
            this.grid_widget.AutoSize = true;
            this.grid_widget.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.grid_widget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_widget.Location = new System.Drawing.Point(4, 127);
            this.grid_widget.Margin = new System.Windows.Forms.Padding(4);
            this.grid_widget.MaximumSize = new System.Drawing.Size(2800, 2462);
            this.grid_widget.Name = "grid_widget";
            this.grid_widget.Size = new System.Drawing.Size(771, 285);
            this.grid_widget.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.MainControlArea, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grid_widget, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(779, 416);
            this.tableLayoutPanel1.TabIndex = 6;
            //this.tableLayoutPanel1.Controls.Add(this.textBox1);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);


            // 
            // extraFeature
            // 
            this.extraFeatureCheckbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.extraFeatureCheckbox.AutoSize = true;
            this.extraFeatureCheckbox.Location = new System.Drawing.Point(3, 52);
            this.extraFeatureCheckbox.Name = "extraFeature";
            this.extraFeatureCheckbox.Size = new System.Drawing.Size(89, 17);
            this.extraFeatureCheckbox.TabIndex = 1;
            this.extraFeatureCheckbox.Text = "Extra Feature";
            this.extraFeatureCheckbox.UseVisualStyleBackColor = true;
            this.extraFeatureCheckbox.CheckedChanged += new System.EventHandler(this.extraFeatureCheckBox_CheckedChanged);

            // 
            // SimpleSpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 444);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SimpleSpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.MainControlArea.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();


            //Background worker
            bg_worker.DoWork += addContentsIntoModel;
            

        }

        #endregion


        private SpreadsheetGridWidget grid_widget;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;

        private FlowLayoutPanel MainControlArea;
        private TableLayoutPanel tableLayoutPanel1;
        private Button sample_button;
        private CheckBox extraFeatureCheckbox;
        private TextBox cell_Input_Textbox;
        private TableLayoutPanel tableLayoutPanel2;
        private Label cell_Input_Label;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem Help_Menu_Option;
        private TableLayoutPanel tableLayoutPanel3;
        private Label Cell_Name_Label;
        private Label Cell_Value_Label;

        private System.Windows.Forms.ProgressBar progressBar;
        private BackgroundWorker bg_worker;
    }
}

