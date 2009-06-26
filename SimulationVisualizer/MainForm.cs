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
        private bool _vmloaded = false;
        private Timer _timer;

        private double _velocity = 2200.0;
        private double _lastVelocity;

        private Vector2d _center = new Vector2d(0.0, 0.0);

        private Vector2d _startVector;
        private Vector2d _endVector;
        private Vector2d _startPos;

        private double _distance;

        private EventHandler<VirtualMachineStepArgs> _handler;

        public MainForm()
        {
            InitializeComponent();
            _vm = new VirtualMachine(new VmProgram("C:\\Users\\tstivers\\Desktop\\bin1(2).obf"));
            _vm.Configuration = 1002.0;
            _handler = new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
        }

        private void OnVmStep(Object vm, VirtualMachineStepArgs args)
        {
            _scoreLabel.Text = args.Score.ToString();
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
                _vm.Configuration = 1001.0;
                _vm.OnStep += new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
            }
        }

        private void _stepButton_Click(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _vm.Reset();
                _vm.Configuration = 1001.0;
                var args = _vm.Step();
                _startPos = new Vector2d(args.X, args.Y);
                args = _vm.Step();
                var pos = new Vector2d(args.X, args.Y);
                var lastPos = pos;
                _startVector = (_startPos - pos).normalize();                
                while (true)
                {
                    Debug.WriteLine("trying " + _velocity);
                    _distance = 0.0;
                    lastPos = pos;
                    _vm.Reset();
                    _vm.Configuration = 1001.0;
                    _vm.XVelocity = _startVector.x * _velocity;
                    _vm.YVelocity = _startVector.y * _velocity;
                    _vm.Step();
                    _vm.XVelocity = 0;
                    _vm.YVelocity = 0;

                    while (true)
                    {
                        args = _vm.Step();
                        pos = new Vector2d(args.X, args.Y);
                        Double distance = pos.length();
                        if (distance < _distance)
                            break;
                        _distance = distance;
                    }

                    Debug.WriteLine(
                            "peaked at " + args.Elapsed + " seconds, distance was " + _distance + " ("
                            + (_distance - args.Target) + ")");

                    if(Math.Abs(_distance - args.Target) < 500.0)
                        break;

                    if(_distance - args.Target > 0.0) // overshot
                    {
                        double t = _lastVelocity;
                        _lastVelocity = _velocity;

                        if (t < _velocity)
                        {
                            _velocity -= (_velocity - t) / 2.0;
                        }
                        else
                        {
                            _velocity -= (t - _velocity) / 2.0;
                        }
                    }
                    else // undershot
                    {
                        double t = _lastVelocity;
                        _lastVelocity = _velocity;

                        _velocity += 50;                     
                    }
                }

                //_endVector = (lastPos - pos).normalize();
                //_vm.XVelocity = _endVector.x * _velocity;
                //_vm.YVelocity = _endVector.y * _velocity;
                //_vm.Step();
                //_vm.XVelocity = 0.0;
                //_vm.YVelocity = 0.0;

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
            for (int i = 0; i < 10; i++)
                _vm.Step();
        }
    }
}
