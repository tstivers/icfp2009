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
    using System.Diagnostics;

    public partial class MainForm : Form
    {
        public VirtualMachine _vm;
        private Timer _timer;

        private double _startVelocity;
        private double _endVelocity;

        private Vector2d _startVector;
        private Vector2d _endVector;
        private Vector2d _startPos;

        private double _startDistance;

        private int _orbitTime;

        private double _targetDistance;
        private double _configuration = 1004;

        private double _score;

        private EventHandler<VirtualMachineStepArgs> _handler;

        public MainForm()
        {
            InitializeComponent();
            _vm = new VirtualMachine(new VmProgram("C:\\Users\\tstivers\\Desktop\\bin1(2).obf"));
            _vm.Configuration = _configuration;
            _handler = new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
        }

        private void OnVmStep(Object vm, VirtualMachineStepArgs args)
        {
            if(args.Score > _score)
            {
                _score = args.Score;
                _scoreLabel.Text = args.Score.ToString();
            }
            
            _fuelLabel.Text = args.Fuel.ToString();
            //_positionLabel.Text = args.X.ToString() + ", " + args.Y.ToString();
            _targetLabel.Text = args.Target.ToString();
            _orbitVisualizer.SetOrbiterPos(args.X, args.Y);
            if(_orbitVisualizer.TargetRadius != (float)args.Target)
                _orbitVisualizer.TargetRadius = (float)args.Target;

            Vector2d pos = new Vector2d(args.X, args.Y);

            double distance = pos.length();
            _distanceLabel.Text = distance.ToString();
        }

        private void loadProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog { Filter = "VM Programs (*.obf)|*.obf", RestoreDirectory = true };

            if (fd.ShowDialog() == DialogResult.OK)
            {
                _vm = new VirtualMachine(new VmProgram(fd.FileName));
                _vm.Configuration = _configuration;
                _vm.OnStep += new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
            }
        }

        private double ComputStartForce(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (Math.Sqrt(GM / r1)) * (Math.Sqrt((r2 * 2) / (r1 + r2)) - 1);
        }

        private double ComputeEndForce(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (Math.Sqrt(GM / r2)) * (1 - Math.Sqrt((r1 * 2) / (r1 + r2)));
        }

        private double ComputeTransferTime(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return Math.PI * Math.Sqrt(Math.Pow(r1 + r2, 3) / (8 * GM));
        }

        private void _stepButton_Click(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _vm.Reset();
                _vm.Configuration = _configuration;
                var args = _vm.Step();
                
                _startPos = new Vector2d(args.X, args.Y);
                
                if(_startPos.y > 0.0)
                    _startVector = new Vector2d(-1.0 * _startPos.normalize().y, _startPos.normalize().x);
                else
                    _startVector = new Vector2d(_startPos.normalize().y, -1.0 * _startPos.normalize().x);
                
                _endVector = _startVector * -1.0;

                _startDistance = _startPos.length();
                _targetDistance = args.Target;

                _startVelocity = this.ComputStartForce(_startDistance, _targetDistance);
                _endVelocity = this.ComputeEndForce(_startDistance, _targetDistance);

                _orbitTime = (int)this.ComputeTransferTime(_startDistance, _targetDistance);
                _orbitVisualizer.DrawLine(_startPos, _startPos + (_startVector * -100000000), Pens.Blue);

                 //do first burn
                _vm.Reset();
                _vm.StartTrace(@"C:\\Users\\tstivers\\Desktop\\" + (uint)_configuration + ".osf", (uint)_configuration);
                _vm.Configuration = _configuration;
                _vm.XVelocity = _startVector.x * _startVelocity;
                _vm.YVelocity = _startVector.y * _startVelocity;
                _vm.Step();
                _vm.XVelocity = 0.0;
                _vm.YVelocity = 0.0;

                // advance to apogee
                for (args = _vm.Step(); args.Elapsed <= _orbitTime; args = _vm.Step());                    
                
                // do second burn
                _vm.XVelocity = _endVector.x * _endVelocity;
                _vm.YVelocity = _endVector.y * _endVelocity;
                _vm.Step();
                _vm.XVelocity = 0.0;
                _vm.YVelocity = 0.0;

                //_vm.Reset();
                //_vm.Configuration = 1001.0;
                _vm.OnStep += _handler;
                _timer = new Timer();
                _timer.Interval = 1;
                _timer.Tick += new EventHandler(_timer_Tick);
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                _timer = null;
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++ )
                _vm.Step();
        }
    }
}
