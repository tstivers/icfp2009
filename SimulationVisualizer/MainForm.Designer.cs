namespace icfp09
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._stepButton = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this._scoreLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._positionLabel = new System.Windows.Forms.Label();
            this._targetLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._fuelLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._orbitVisualizer = new icfp09.OrbitVisualizer();
            this._distanceLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._orbitVisualizer);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._distanceLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this._targetLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this._fuelLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this._positionLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this._scoreLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1022, 721);
            this.splitContainer1.SplitterDistance = 581;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stepButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1022, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1022, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadProgramToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // loadProgramToolStripMenuItem
            // 
            this.loadProgramToolStripMenuItem.Name = "loadProgramToolStripMenuItem";
            this.loadProgramToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.loadProgramToolStripMenuItem.Text = "Load Program";
            this.loadProgramToolStripMenuItem.Click += new System.EventHandler(this.loadProgramToolStripMenuItem_Click);
            // 
            // _stepButton
            // 
            this._stepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._stepButton.Image = ((System.Drawing.Image)(resources.GetObject("_stepButton.Image")));
            this._stepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._stepButton.Name = "_stepButton";
            this._stepButton.Size = new System.Drawing.Size(23, 22);
            this._stepButton.Text = "toolStripButton1";
            this._stepButton.Click += new System.EventHandler(this._stepButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Score";
            // 
            // _scoreLabel
            // 
            this._scoreLabel.AutoSize = true;
            this._scoreLabel.Location = new System.Drawing.Point(123, 34);
            this._scoreLabel.Name = "_scoreLabel";
            this._scoreLabel.Size = new System.Drawing.Size(35, 13);
            this._scoreLabel.TabIndex = 1;
            this._scoreLabel.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Position";
            // 
            // _positionLabel
            // 
            this._positionLabel.AutoSize = true;
            this._positionLabel.Location = new System.Drawing.Point(123, 74);
            this._positionLabel.Name = "_positionLabel";
            this._positionLabel.Size = new System.Drawing.Size(35, 13);
            this._positionLabel.TabIndex = 3;
            this._positionLabel.Text = "label4";
            // 
            // _targetLabel
            // 
            this._targetLabel.AutoSize = true;
            this._targetLabel.Location = new System.Drawing.Point(123, 61);
            this._targetLabel.Name = "_targetLabel";
            this._targetLabel.Size = new System.Drawing.Size(35, 13);
            this._targetLabel.TabIndex = 7;
            this._targetLabel.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Target";
            // 
            // _fuelLabel
            // 
            this._fuelLabel.AutoSize = true;
            this._fuelLabel.Location = new System.Drawing.Point(123, 47);
            this._fuelLabel.Name = "_fuelLabel";
            this._fuelLabel.Size = new System.Drawing.Size(35, 13);
            this._fuelLabel.TabIndex = 5;
            this._fuelLabel.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(72, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Fuel";
            // 
            // _orbitVisualizer
            // 
            this._orbitVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._orbitVisualizer.DrawTrail = true;
            this._orbitVisualizer.Location = new System.Drawing.Point(0, 49);
            this._orbitVisualizer.Name = "_orbitVisualizer";
            this._orbitVisualizer.Size = new System.Drawing.Size(1022, 532);
            this._orbitVisualizer.TabIndex = 2;
            this._orbitVisualizer.TargetRadius = 0F;
            this._orbitVisualizer.Text = "orbitVisualizer1";
            // 
            // _distanceLabel
            // 
            this._distanceLabel.AutoSize = true;
            this._distanceLabel.Location = new System.Drawing.Point(264, 61);
            this._distanceLabel.Name = "_distanceLabel";
            this._distanceLabel.Size = new System.Drawing.Size(35, 13);
            this._distanceLabel.TabIndex = 9;
            this._distanceLabel.Text = "label2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Distance";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 721);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ICFP 2009 Simulation Visualizer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton _stepButton;
        private System.Windows.Forms.Label _targetLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _fuelLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label _positionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _scoreLabel;
        private System.Windows.Forms.Label label1;
        private OrbitVisualizer _orbitVisualizer;
        private System.Windows.Forms.Label _distanceLabel;
        private System.Windows.Forms.Label label4;
    }
}

