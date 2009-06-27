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
            _vm = new VirtualMachine(new VmProgram("C:\\Users\\tstivers\\Desktop\\bin3.obf"));
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
            _minDistance = Double.MaxValue;
            _score = 0.0;
            _scoreLabel.Text = "";
            _configuration = Double.Parse(_scenarioBox.Text);
            _orbitVisualizer.ClearCircles();

            _vm.Reset();
            _vm.Configuration = _configuration;

            // the plan:
            // find our apogee & perigee
            // find target apogee & perigee
            // transfer to target perigee
            // when opposite target apogee, transfer to apogee elliptically

            var myApogee = Double.MinValue;
            var myPerigee = Double.MaxValue;
            var targetApogee = Double.MinValue;
            var targetPerigee = Double.MaxValue;
            double targetApogeeAngle = 0.0;
            double targetPerigeeAngle = 0.0;

            VirtualMachineStepArgs args = null;

            // find apogee/perigees
            for(int i = 0; i < 90000; i++)
            {
                args = _vm.Step();
                myApogee = Math.Max(myApogee, args.Distance);
                myPerigee = Math.Min(myPerigee, args.Distance);
                if(args.TargetDistance > targetApogee)
                {
                    targetApogee = args.TargetDistance;
                    targetApogeeAngle = args.TargetPosition.angle();
                }
                if (args.TargetDistance < targetPerigee)
                {
                    targetPerigee = args.TargetDistance;
                    targetPerigeeAngle = args.TargetPosition.angle();
                }
            }

            _orbitVisualizer.AddCircle(targetApogee, Pens.Red);
            _orbitVisualizer.AddCircle(targetPerigee, Pens.Red);
            //_orbitVisualizer.AddLine(Vector2d.Zero(), Vector2d.FromAngle(targetApogeeAngle) * targetApogee, Pens.Green);
            _orbitVisualizer.AddLine(Vector2d.Zero(), Vector2d.FromAngle(targetPerigeeAngle) * targetPerigee, Pens.Red);

            _vm.Reset();
            _vm.Configuration = _configuration;


            // figure out when I'm directly across from the target at its perigee
            int startBurn = 0;

            var delay = Int32.Parse(_delayBox.Text);
            for (int i = 0; i < delay; i++)
                _vm.Step();

            for (; startBurn < 90000; startBurn++ )
            {
                args = _vm.Step();
                if(Math.Abs(args.Position.angdiff(targetPerigeeAngle + Math.PI)) < 0.01)
                    break;
            }

            _orbitVisualizer.AddLine(Vector2d.Zero(), Vector2d.FromAngle(args.Position.angle()) * targetPerigee, Pens.Blue);

            _vm.Reset();

            if(_traceBox.Checked)
                _vm.StartTrace(@"trace_" + _configuration + ".osf", (uint)_configuration);

            _vm.Configuration = _configuration;

            for (int i = 0; i < delay; i++)
                _vm.Step();

            for (int i = 0; i < startBurn; i++)
                _vm.Step();

            ChangeOrbit(targetPerigee, false);

            ChangeOrbit(targetApogee, true);

            _vm.OnStep += _handler;
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }

        void ChangeOrbit(double targetRadius, bool eliptical)
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

            startVelocity += Double.Parse(_startTweak.Text);

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

            // advance to apogee
            for (args = _vm.Step(); args.Elapsed < startTime + transferTime; args = _vm.Step());

            // compute end vector
            startPos = new Vector2d(args.X, args.Y);
            var endVector = startPos.tangent();

            endVelocity = this.ComputeEndForce(startDistance, startPos.length());
            endVelocity += Double.Parse(_endTweak.Text);

            Debug.WriteLine("endVelocity = " + endVelocity);

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
