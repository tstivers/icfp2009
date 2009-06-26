using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace icfp09
{
    public partial class MainForm : Form
    {
        public VirtualMachine _vm;
        private bool _vmloaded = false;

        public MainForm()
        {
            InitializeComponent();
            _vm = new VirtualMachine(new VmProgram("C:\\Users\\tstivers\\Desktop\\bin1(2).obf"));
            _vm.Configuration = 1002.0;
            _vm.OnStep += new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
        }

        private void OnVmStep(Object vm, VirtualMachineStepArgs args)
        {
            _scoreLabel.Text = args.Score.ToString();
            _fuelLabel.Text = args.Fuel.ToString();
            _positionLabel.Text = args.X.ToString() + ", " + args.Y.ToString();
        }

        private void loadProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog {Filter = "VM Programs (*.obf)|*.obf", RestoreDirectory = true};

            if(fd.ShowDialog() == DialogResult.OK)
            {
                _vm = new VirtualMachine(new VmProgram(fd.FileName));
                _vm.Step();
            }


        }

        private void _stepButton_Click(object sender, EventArgs e)
        {
            _vm.Step();
        }
    }
}
