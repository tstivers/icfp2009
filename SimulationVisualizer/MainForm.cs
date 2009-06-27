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
        private double _configuration = 2003;
        private double _score;
        private EventHandler<VirtualMachineStepArgs> _handler;

        private double _minDistance = Double.MaxValue;

        public MainForm()
        {
            InitializeComponent();
            _vm = new VirtualMachine(new VmProgram("C:\\Users\\tstivers\\Desktop\\bin2.obf"));
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
            //_targetLabel.Text = args.Target.ToString();
            _orbitVisualizer.SetOrbiterPos(args.X, args.Y);
            _orbitVisualizer.SetTargetPos(args.X - args.TargetX, args.Y - args.TargetY);
            _orbitVisualizer.Invalidate();
            Vector2d pos = new Vector2d(args.X, args.Y);
            var distance = new Vector2d(args.TargetX, args.TargetY).length();
            if(distance < _minDistance)
            {
                _minDistance = distance;
                _targetLabel.Text = _minDistance.ToString();
            }

            //double distance = pos.length();
            //_distanceLabel.Text = distance.ToString();
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

            return (Math.Sqrt(GM / r1)) * (Math.Sqrt((r2 * 2) / (r1 + r2)) - 1.0);
        }

        private double ComputeEndForce(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (Math.Sqrt(GM / r2)) * (1.0 - Math.Sqrt((r1 * 2) / (r1 + r2)));
        }

        private double ComputeTransferTime(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return Math.PI * Math.Sqrt(Math.Pow(r1 + r2, 3) / (8 * GM));
        }

        private double ComputePeriod(double r1)
        {
            return 0.0;
        }

        private void _stepButton_Click(object sender, EventArgs e)
        {
            // figure out all the starting crap
            if(_timer != null)
                _timer.Stop();
            _vm.OnStep -= _handler;

            _vm.Reset();
            _vm.Configuration = _configuration;
            var args = _vm.Step();
            
            // starting positions
            var targetPos = new Vector2d(args.X - args.TargetX, args.Y - args.TargetY);
            var targetDistance = targetPos.length();
            _orbitVisualizer.TargetRadius = (float)targetDistance;

            _vm.Reset();
            _vm.Configuration = _configuration;

            ChangeOrbit(targetDistance);

            _vm.OnStep += _handler;
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }

        void ChangeOrbit(double targetRadius)
        {
            var state = _vm.SnapShot();

            var args = _vm.Step();
            var startTime = args.Elapsed - 1;

            var startPos = new Vector2d(args.X, args.Y);
            var startDistance = startPos.length();
            var startVector = startPos.tangent();

            var startVelocity = this.ComputStartForce(startDistance, targetRadius);
            var endVelocity = this.ComputeEndForce(startDistance, targetRadius);
            var transferTime = this.ComputeTransferTime(startDistance, targetRadius);

            // reset the state
            _vm.Reset(state);
            //_vm.Step();

            // do the first burn
            _vm.XVelocity = startVector.x * startVelocity;
            _vm.YVelocity = startVector.y * startVelocity;
            _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;

            // advance to apogee
            for (args = _vm.Step(); args.Elapsed <= startTime + transferTime; args = _vm.Step());

            // compute end vector
            //state = _vm.SnapShot();
            //args = _vm.Step();
            startPos = new Vector2d(args.X, args.Y);
            var endVector = startPos.tangent();
            //_vm.Reset(state);

            //do second burn
            _vm.XVelocity = endVector.x * endVelocity;
            _vm.YVelocity = endVector.y * endVelocity;
            _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;
        }

        private double _lastDistance;
        void _timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                var args = _vm.Step();
                //var distance = new Vector2d(args.X, args.Y).length()
                //               - new Vector2d(args.X - args.TargetX, args.Y - args.TargetY).length();
                //if(Math.Abs(distance) < 500.0)
                //{
                //}
                //else if(distance > 0.0 && distance > _lastDistance)
                //{
                //    var vv = new Vector2d(new Vector2d(args.X, args.Y).angle()).normalize();
                //    _vm.XVelocity = vv.x * 0.05;
                //    _vm.YVelocity = vv.y * 0.05;
                //}
                //else if (distance < 0.0 && distance < _lastDistance)
                //{
                //    var vv = new Vector2d(new Vector2d(args.X, args.Y).angle() - Math.PI).normalize();
                //    _vm.XVelocity = vv.x * 0.05;
                //    _vm.YVelocity = vv.y * 0.05;
                //}

                _distanceLabel.Text = (new Vector2d(args.X, args.Y).length() - new Vector2d(args.X - args.TargetX, args.Y - args.TargetY).length()).ToString();
                //_lastDistance = distance;
            }
        }
    }
}
