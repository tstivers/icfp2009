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
    using System.IO;

    public partial class MainForm : Form
    {
        public VirtualMachine _vm;
        private Timer _timer;
        private double _configuration;
        private EventHandler<VirtualMachineStepArgs> _handler;
        private int _frameSkip = 10;

        private int _elapsed;

        public MainForm()
        {
            InitializeComponent();
            _handler = new EventHandler<VirtualMachineStepArgs>(this.OnVmStep);
        }

        private void OnVmStep(Object vm, VirtualMachineStepArgs args)
        {        
            if(args.Score != 0.0)
            {
                _scoreLabel.Text = args.Score.ToString();
            }

            _elapsed++;

            _fuelLabel.Text = args.Fuel.ToString();
            _orbitVisualizer.SetOrbiterPos(args.Position);
            _orbitVisualizer.SetTargetPos(args.TargetPosition);
            _orbitVisualizer.SetTargetFuturePos(
                    Vector2d.FromAngle(this.ComputeAngleAtTime(_targetAngleOffset, _targetRv, args.Elapsed)) * args.TargetOrbitRadius);
            _orbitVisualizer.Invalidate();
            _offsetLabel.Text = ((int)(args.OrbitRadius - args.TargetOrbitRadius)).ToString();
            _targetOffsetLabel.Text = ((int)((args.Position - args.TargetPosition).length())).ToString();
        }

        private double ComputStartForce(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (Math.Sqrt(GM / r1)) * (Math.Sqrt((r2 * 2.0) / (r1 + r2)) - 1.0);
        }

        private double ComputeEndForce(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (Math.Sqrt(GM / r2)) * (1.0 - Math.Sqrt((r1 * 2.0) / (r1 + r2)));
        }

        private double ComputeTransferTime(double r1, double r2)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return Math.PI * Math.Sqrt(Math.Pow(r1 + r2, 3) / (8 * GM));
        }

        private double ComputeAngleAtTime(double v, double rv, double t)
        {
            return v + (rv * t);
        }

        private double ComputePeriod(double r)
        {
            double G = 6.67428E-11;
            double M = 6E24;
            double GM = G * M;

            return (2.0 * Math.PI) * Math.Sqrt(Math.Pow(r, 3.0) / GM);
        }

        private double _targetRv;

        private double _targetAngleOffset;

        private void ExecutSimClick(object sender, EventArgs e)
        {
            // reset everything
            if(_timer != null)
                _timer.Stop();
            try { _vm.OnStep -= _handler; } catch(Exception) {}
            _scoreLabel.Text = "";
            _configuration = Double.Parse(_scenarioBox.Text);
            _orbitVisualizer.ClearCircles();
            _orbitVisualizer.ClearLines();
            _orbitVisualizer.SetScale(10000000.0);

            _vm = new VirtualMachine(new VmProgram(_binBox.Text));
            _vm.Reset();
            _vm.Configuration = _configuration;

            // the plan:
            // find our start orbit *
            // find target orbit *
            // transfer to another orbit if our orbits are too close together *
            // delay a certain amount of time
            // transfer to target orbit *

            var args = _vm.Step();

            var startRadius = args.OrbitRadius;
            var targetRadius = args.TargetOrbitRadius;

            var transferRadius = 0.0;
            if (Math.Abs(startRadius - targetRadius) < 1000000.0)
                transferRadius = targetRadius + 1000000.0;

            _orbitVisualizer.AddCircle(startRadius, Pens.Blue);
            _orbitVisualizer.AddCircle(targetRadius, Pens.Red);
            _orbitVisualizer.AddCircle(transferRadius, Pens.Green);

            {
                _vm.Reset();
                _vm.Configuration = _configuration;
                args = _vm.Step();
                _orbitVisualizer.AddLine(Vector2d.Zero(), args.TargetPosition, Pens.Blue);
                for (int i = (int)this.ComputePeriod(args.TargetOrbitRadius); i > 0; i--)
                    args = _vm.Step();
                //_orbitVisualizer.AddLine(Vector2d.Zero(), args.TargetPosition, Pens.Red);
            }
            
            // reset the vm and do the run for real
            _vm.Reset();

            if (_traceBox.Checked)
            {
                string filename = @"trace_" + _configuration + ".osf";
                if (File.Exists(filename))
                    File.Delete(filename);
                _vm.StartTrace(@"trace_" + _configuration + ".osf", (uint)_configuration);
            }

            _vm.Configuration = _configuration;

            ChangeOrbit(targetRadius, false);

            // continue to run the sim
            _vm.OnStep += _handler;
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Start();
        }

        void ChangeOrbit(double targetRadius, bool eliptical)
        {
            var state = _vm.SnapShot();

            var args = _vm.Step();
            var startPos = args.Position;
            var startRadius = args.OrbitRadius;
            var startVector = startPos.tangent();
            var startVelocity = this.ComputStartForce(startRadius, targetRadius);
            var transferTime = this.ComputeTransferTime(startRadius, targetRadius);

            // reset the state
            _vm.Reset(state);

            // do the first burn
            _vm.XVelocity = startVector.x * startVelocity;
            _vm.YVelocity = startVector.y * startVelocity;
            _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;

            if (eliptical)
                return;

            // hit the apogee
            for (int i = 0; i < transferTime - 1; i++)
                args = _vm.Step();

            var endVector = args.Position.tangent();
            var endVelocity = this.ComputeEndForce(startRadius, args.OrbitRadius);

            //do second burn
            _vm.XVelocity = endVector.x * endVelocity;
            _vm.YVelocity = endVector.y * endVelocity;
            _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;            
        }

        void TimerTick(object sender, EventArgs e)
        {
            for (int i = 0; i < _frameSkip; i++)
                _vm.Step();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _binBox.Text = ofd.FileName;
            }
        }
    }
}
