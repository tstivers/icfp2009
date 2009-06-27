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
            this._stepButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._distanceLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._targetLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._fuelLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._positionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._scoreLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._delayBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._preOrbit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this._traceBox = new System.Windows.Forms.CheckBox();
            this._scenarioBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this._startTweak = new System.Windows.Forms.TextBox();
            this._endTweak = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._orbitVisualizer = new icfp09.OrbitVisualizer();
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
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this._endTweak);
            this.splitContainer1.Panel2.Controls.Add(this._startTweak);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this._scenarioBox);
            this.splitContainer1.Panel2.Controls.Add(this._traceBox);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this._preOrbit);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this._delayBox);
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
            this.splitContainer1.Size = new System.Drawing.Size(910, 661);
            this.splitContainer1.SplitterDistance = 521;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stepButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(910, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(910, 24);
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
            // _distanceLabel
            // 
            this._distanceLabel.AutoSize = true;
            this._distanceLabel.Location = new System.Drawing.Point(127, 89);
            this._distanceLabel.Name = "_distanceLabel";
            this._distanceLabel.Size = new System.Drawing.Size(35, 13);
            this._distanceLabel.TabIndex = 9;
            this._distanceLabel.Text = "label2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Distance";
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
            // _positionLabel
            // 
            this._positionLabel.AutoSize = true;
            this._positionLabel.Location = new System.Drawing.Point(123, 74);
            this._positionLabel.Name = "_positionLabel";
            this._positionLabel.Size = new System.Drawing.Size(35, 13);
            this._positionLabel.TabIndex = 3;
            this._positionLabel.Text = "label4";
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
            // _scoreLabel
            // 
            this._scoreLabel.AutoSize = true;
            this._scoreLabel.Location = new System.Drawing.Point(123, 34);
            this._scoreLabel.Name = "_scoreLabel";
            this._scoreLabel.Size = new System.Drawing.Size(35, 13);
            this._scoreLabel.TabIndex = 1;
            this._scoreLabel.Text = "label2";
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
            // _delayBox
            // 
            this._delayBox.Location = new System.Drawing.Point(506, 53);
            this._delayBox.Name = "_delayBox";
            this._delayBox.Size = new System.Drawing.Size(100, 20);
            this._delayBox.TabIndex = 10;
            this._delayBox.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(468, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "delay";
            // 
            // _preOrbit
            // 
            this._preOrbit.Location = new System.Drawing.Point(506, 27);
            this._preOrbit.Name = "_preOrbit";
            this._preOrbit.Size = new System.Drawing.Size(100, 20);
            this._preOrbit.TabIndex = 12;
            this._preOrbit.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(453, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "preorbit";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(506, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "execute\r\n";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this._stepButton_Click);
            // 
            // _traceBox
            // 
            this._traceBox.AutoSize = true;
            this._traceBox.Location = new System.Drawing.Point(667, 56);
            this._traceBox.Name = "_traceBox";
            this._traceBox.Size = new System.Drawing.Size(76, 17);
            this._traceBox.TabIndex = 15;
            this._traceBox.Text = "save trace";
            this._traceBox.UseVisualStyleBackColor = true;
            // 
            // _scenarioBox
            // 
            this._scenarioBox.Location = new System.Drawing.Point(720, 27);
            this._scenarioBox.Name = "_scenarioBox";
            this._scenarioBox.Size = new System.Drawing.Size(100, 20);
            this._scenarioBox.TabIndex = 16;
            this._scenarioBox.Text = "3001";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(667, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "scenario";
            // 
            // _startTweak
            // 
            this._startTweak.Location = new System.Drawing.Point(316, 26);
            this._startTweak.Name = "_startTweak";
            this._startTweak.Size = new System.Drawing.Size(100, 20);
            this._startTweak.TabIndex = 18;
            this._startTweak.Text = "0";
            // 
            // _endTweak
            // 
            this._endTweak.Location = new System.Drawing.Point(316, 54);
            this._endTweak.Name = "_endTweak";
            this._endTweak.Size = new System.Drawing.Size(100, 20);
            this._endTweak.TabIndex = 19;
            this._endTweak.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(251, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "start tweak";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(253, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "end tweak";
            // 
            // _orbitVisualizer
            // 
            this._orbitVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._orbitVisualizer.DrawTrail = true;
            this._orbitVisualizer.Location = new System.Drawing.Point(0, 49);
            this._orbitVisualizer.Name = "_orbitVisualizer";
            this._orbitVisualizer.PreOrbitRadius = 0F;
            this._orbitVisualizer.Size = new System.Drawing.Size(910, 472);
            this._orbitVisualizer.TabIndex = 2;
            this._orbitVisualizer.TargetRadius = 0F;
            this._orbitVisualizer.Text = "orbitVisualizer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 661);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _delayBox;
        private System.Windows.Forms.TextBox _preOrbit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox _traceBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _scenarioBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox _endTweak;
        private System.Windows.Forms.TextBox _startTweak;
    }
}

