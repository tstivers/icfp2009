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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._binBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this._scenarioBox = new System.Windows.Forms.TextBox();
            this._traceBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this._offsetLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._fuelLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._scoreLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._targetOffsetLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._orbitVisualizer = new icfp09.OrbitVisualizer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._targetOffsetLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this._binBox);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this._scenarioBox);
            this.splitContainer1.Panel2.Controls.Add(this._traceBox);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this._offsetLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this._fuelLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this._scoreLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(910, 661);
            this.splitContainer1.SplitterDistance = 521;
            this.splitContainer1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(826, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 20);
            this.button2.TabIndex = 20;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(666, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "binary";
            // 
            // _binBox
            // 
            this._binBox.Location = new System.Drawing.Point(719, 22);
            this._binBox.Name = "_binBox";
            this._binBox.Size = new System.Drawing.Size(100, 20);
            this._binBox.TabIndex = 18;
            this._binBox.Text = "bin2.obf";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(666, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "scenario";
            // 
            // _scenarioBox
            // 
            this._scenarioBox.Location = new System.Drawing.Point(719, 48);
            this._scenarioBox.Name = "_scenarioBox";
            this._scenarioBox.Size = new System.Drawing.Size(100, 20);
            this._scenarioBox.TabIndex = 16;
            this._scenarioBox.Text = "2001";
            // 
            // _traceBox
            // 
            this._traceBox.AutoSize = true;
            this._traceBox.Location = new System.Drawing.Point(666, 77);
            this._traceBox.Name = "_traceBox";
            this._traceBox.Size = new System.Drawing.Size(76, 17);
            this._traceBox.TabIndex = 15;
            this._traceBox.Text = "save trace";
            this._traceBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(397, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "execute\r\n";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ExecutSimClick);
            // 
            // _offsetLabel
            // 
            this._offsetLabel.AutoSize = true;
            this._offsetLabel.Location = new System.Drawing.Point(127, 74);
            this._offsetLabel.Name = "_offsetLabel";
            this._offsetLabel.Size = new System.Drawing.Size(35, 13);
            this._offsetLabel.TabIndex = 9;
            this._offsetLabel.Text = "label2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Orbit Offset";
            // 
            // _fuelLabel
            // 
            this._fuelLabel.AutoSize = true;
            this._fuelLabel.Location = new System.Drawing.Point(127, 47);
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
            // _scoreLabel
            // 
            this._scoreLabel.AutoSize = true;
            this._scoreLabel.Location = new System.Drawing.Point(127, 34);
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
            // _targetOffsetLabel
            // 
            this._targetOffsetLabel.AutoSize = true;
            this._targetOffsetLabel.Location = new System.Drawing.Point(127, 87);
            this._targetOffsetLabel.Name = "_targetOffsetLabel";
            this._targetOffsetLabel.Size = new System.Drawing.Size(35, 13);
            this._targetOffsetLabel.TabIndex = 22;
            this._targetOffsetLabel.Text = "label2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Target Offset";
            // 
            // _orbitVisualizer
            // 
            this._orbitVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._orbitVisualizer.DrawTrail = true;
            this._orbitVisualizer.Location = new System.Drawing.Point(0, 0);
            this._orbitVisualizer.Name = "_orbitVisualizer";
            this._orbitVisualizer.PreOrbitRadius = 0F;
            this._orbitVisualizer.Size = new System.Drawing.Size(910, 521);
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
            this.Name = "MainForm";
            this.Text = "ICFP 2009 Simulation Visualizer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label _fuelLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label _scoreLabel;
        private System.Windows.Forms.Label label1;
        private OrbitVisualizer _orbitVisualizer;
        private System.Windows.Forms.Label _offsetLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox _traceBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _scenarioBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _binBox;
        private System.Windows.Forms.Label _targetOffsetLabel;
        private System.Windows.Forms.Label label5;
    }
}

