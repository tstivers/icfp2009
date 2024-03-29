﻿using System;
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

        private bool ComputeAlignmentDelay(Vector2d startPos, Vector2d targetPos, Vector2d apogeePos, Vector2d perigeePos, ref int burnStart, ref Vector2d burnTarget)
        {
            //double apogeeTime = this.ComputeTransferTime(startPos.length(), apogeePos.length());
            //double perigeeTime = this.ComputeTransferTime(startPos.length(), perigeePos.length());
            //double period = this.ComputePeriod(startPos.length());
            //double targetPeriod = this.ComputePeriod(apogeePos.length());

            //double startOffset = startPos.angle();

            //var currentTarget = Math.Abs(targetPos.length() - apogeePos.length()) < )

            //for(int i = 0; i < 3E6, i++)
            ////double myRv = (Math.PI * 2.0) / this.ComputePeriod(startRadius);

            ////// run forward until we will orbit into place
            ////Vector2d myFuturePosition;
            ////Vector2d targetFuturePosition;
            ////VirtualMachineStepArgs args;
            ////do
            ////{
            ////    args = _vm.Step();
            ////    myFuturePosition = (args.Position.normalize() * -1.0) * args.TargetOrbitRadius;
            ////    targetFuturePosition = Vector2d.FromAngle(args.TargetPosition.angle() + (targetRv * transferTime)) * args.TargetOrbitRadius;
            ////    burnStart = args.Elapsed - 1;
            ////    if(burnStart == 1E5)
            ////        break;
            ////} while ((myFuturePosition - targetFuturePosition).length() > 500.0);

            return burnStart != 1E5;
        }

        private double _lastDv;
        private void Finesse()
        {
            var args = _vm.Step();

            //if (args.TargetDistance > 1000.0 && args.TargetDistance > _lastDv)
            //{
            //    Vector2d targetVector = (args.Position - args.TargetPosition).normalize();
            //    //_vm.XVelocity = targetVector.x * 0.0001;
            //    //_vm.YVelocity = targetVector.y * 0.0001;
            //    //_orbitVisualizer.DrawLine(args.Position, args.Position + (targetVector * 1000000.0), Pens.Brown);
            //}
            //_lastDv = args.TargetDistance;
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

            _vm = new VirtualMachine(new VmProgram(_binBox.Text));
            _vm.Reset();
            _vm.Configuration = _configuration;

            // the plan:
            // find apogee/perigee for target
            // delay our transfer until we can rendevous at either the target's apogee or perigee
            // change orbits to the target's apogee or perigee
            // immediately burn to put us in an elliptical orbit alongside the satellite

            var args = _vm.Step();

            var startPos = args.Position;
            var targetPos = args.TargetPosition;
            var startRadius = args.OrbitRadius;
            var startAngle = args.Position.angle();

            args = _vm.Step();
            _clockWise = startAngle < args.Position.angle();

            // get apogee/perigee
            var targetApogee = Double.MinValue;
            var targetPerigee = Double.MaxValue;
            Vector2d apogeePos = Vector2d.Zero();
            Vector2d perigeePos = Vector2d.Zero();

            for (int i = 0; i < 1E5; args = _vm.Step(), i++)
            {
                if(args.TargetOrbitRadius > targetApogee)
                {
                    targetApogee = args.TargetOrbitRadius;
                    apogeePos = args.TargetPosition;
                }
                if (args.TargetOrbitRadius < targetPerigee)
                {
                    targetPerigee = args.TargetOrbitRadius;
                    perigeePos = args.TargetPosition;
                }
            }

            // add visual aids
            _orbitVisualizer.AddCircle(startRadius, Pens.Green);
            _orbitVisualizer.AddCircle(targetPerigee, Pens.Yellow);
            _orbitVisualizer.AddCircle(targetApogee, Pens.Red);
            _orbitVisualizer.AddLine(Vector2d.Zero(), perigeePos, Pens.Yellow);
            _orbitVisualizer.AddLine(Vector2d.Zero(), apogeePos, Pens.Red);

            _vm.Reset();
            _vm.Configuration = _configuration;

            int burnStart = 0;
            Vector2d burnTarget = Vector2d.Zero();
            this.ComputeAlignmentDelay(startPos, targetPos, apogeePos, perigeePos, ref burnStart, ref burnTarget);

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

            var targetAngle = perigeePos.angle() + Math.PI;
            var angleDiff = Math.Abs(startAngle - targetAngle);
            var period = this.ComputePeriod(startPos.length());
            var t = (angleDiff * period) / (Math.PI * 2.0);

            //t += period * 5.0; // swing around 5 times                

            for (int i = 0; i < t; i++)
                args = _vm.Step();

            args = this.ChangeOrbit(targetPerigee, false);
            Debug.WriteLine("Error: " + (args.Position - perigeePos).length());

            //while (args.TargetDistance > 900.0)
            //    args = _vm.Step();

            ChangeOrbit(targetApogee, true);

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
