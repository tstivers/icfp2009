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
        private bool _clockWise = false;

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

            return Math.PI * Math.Sqrt(Math.Pow(r1 + r2, 3) / (8.0 * GM));
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

        private bool ComputeAlignmentDelay(double startRadius, double targetRadius, ref int burnStart)
        {
            double transferTime = this.ComputeTransferTime(startRadius, targetRadius);
            double targetRv = (Math.PI * 2.0) / this.ComputePeriod(targetRadius);
            double myRv = (Math.PI * 2.0) / this.ComputePeriod(startRadius);

            // run forward until we will orbit into place
            Vector2d myFuturePosition;
            Vector2d targetFuturePosition;
            VirtualMachineStepArgs args;
            double lowWater = Double.MaxValue;
            int lowTime = 0;
            do
            {
                args = _vm.Step();
                myFuturePosition = (args.Position.normalize() * -1.0) * args.TargetOrbitRadius;
                targetFuturePosition = Vector2d.FromAngle(args.TargetPosition.angle() + (targetRv * transferTime)) * args.TargetOrbitRadius;
                if ((myFuturePosition - targetFuturePosition).length() < lowWater)
                {
                    lowWater = (myFuturePosition - targetFuturePosition).length();
                    lowTime = args.Elapsed - 1;
                }
                burnStart = args.Elapsed - 1;
                if(burnStart == 3E6)
                    break;
            } while ((myFuturePosition - targetFuturePosition).length() > 200.0);

            if (burnStart == 3E6)
                burnStart = lowTime;

            return burnStart != 3E6;
        }

        private double _lastDv;
        private bool _realClose = false;
        private int _closeTime;
        private void Finesse()
        {
            var args = _vm.Step();
            return;

            if (args.TargetDistance > 500.0 && args.TargetDistance > _lastDv && !_realClose)
            {
                Vector2d targetVector = (args.Position - args.TargetPosition).normalize();
                _vm.XVelocity = targetVector.x ;
                _vm.YVelocity = targetVector.y ;
                _orbitVisualizer.DrawLine(args.Position, args.Position + (targetVector * 1000000.0), Pens.Brown);
            }
            else
            {
                _vm.XVelocity = 0.0;
                _vm.YVelocity = 0.0;
            }

            if (args.TargetDistance < 500)
            {
                _realClose = true;
                _closeTime++;
            }

            if (args.TargetDistance > 900 && _realClose)
            {
                _realClose = false;
                Debug.WriteLine("missed it, time was " + _closeTime);
                _closeTime = 0;
            }

            _lastDv = args.TargetDistance;
        }

        private double TestRun(int burnStart, double targetRadius)
        {
            _vm.Reset();
            _vm.Configuration = _configuration;
            VirtualMachineStepArgs args;
            for (args = _vm.Step(); args.Elapsed < burnStart; )
                args = _vm.Step();
            args = ChangeOrbit(targetRadius, false);
            return args.TargetDistance;
        }

        private void ExecuteSimClick(object sender, EventArgs e)
        {
            // reset everything
            if(_timer != null)
                _timer.Stop();
            try { _vm.OnStep -= _handler; } catch(Exception) {}
            _scoreLabel.Text = "";
            _configuration = Double.Parse(_scenarioBox.Text);
            _orbitVisualizer.ClearCircles();
            _orbitVisualizer.ClearLines();
            _orbitVisualizer.SetScale(0.0);

            _vm = new VirtualMachine(new VmBinary(_binBox.Text));
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
            var startAngle = args.Position.angle();

            _orbitVisualizer.AddCircle(startRadius, Pens.Green);
            _orbitVisualizer.AddCircle(targetRadius, Pens.Red);

            args = _vm.Step();
            _clockWise = startAngle < args.Position.angle();

            _vm.Reset();
            _vm.Configuration = _configuration;

            int burnStart = 0;
            this.ComputeAlignmentDelay(startRadius, targetRadius, ref burnStart);

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

            for (args = _vm.Step(); args.Elapsed < burnStart; )
                args = _vm.Step();

            ChangeOrbit(targetRadius, false);

            // continue to run the sim
            _vm.OnStep += _handler;
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Start();
        }

        VirtualMachineStepArgs ChangeOrbit(double targetRadius, bool eliptical)
        {
            var state = _vm.SnapShot();

            var args = _vm.Step();
            var startPos = args.Position;
            var startRadius = args.OrbitRadius;
            var startVector = startPos.tangent(_clockWise);
            var startVelocity = this.ComputStartForce(startRadius, targetRadius);
            var transferTime = this.ComputeTransferTime(startRadius, targetRadius);

            // reset the state
            _vm.Reset(state);

            // do the first burn
            _vm.XVelocity = startVector.x * startVelocity;
            _vm.YVelocity = startVector.y * startVelocity;
            args = _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;

            if (eliptical)
                return args;

            // hit the apogee
            for (int i = 0; i < transferTime - 1; i++)
                args = _vm.Step();

            var endVector = args.Position.tangent(_clockWise);
            var endVelocity = this.ComputeEndForce(startRadius, args.OrbitRadius);

            //do second burn
            _vm.XVelocity = endVector.x * endVelocity;
            _vm.YVelocity = endVector.y * endVelocity;
            args = _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;
            return args;
        }

        void TimerTick(object sender, EventArgs e)
        {
            for (int i = 0; i < _frameSkip; i++)
                this.Finesse();
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
