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
        private double _configuration = 2004;
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

            var delay = Int32.Parse(_delayBox.Text);
            for (int i = 0; i < delay; i++)
                args = _vm.Step();
            
            // starting positions
            var startPos = new Vector2d(args.X, args.Y);
            var targetPos = new Vector2d(args.X - args.TargetX, args.Y - args.TargetY);
            
            // r1 and r2
            var startDistance = startPos.length();
            var targetDistance = targetPos.length();

            var endPos = new Vector2d(startPos.angle() + Math.PI) * targetDistance;

            _orbitVisualizer.TargetRadius = (float)targetDistance;

            // periods
            var myPeriod = this.ComputePeriod(startDistance);
            var targetPeriod = this.ComputePeriod(targetDistance);
            var transferTime = this.ComputeTransferTime(startDistance, targetDistance);

            var targetRv = targetPeriod / (2.0 * Math.PI);
            var myRv = myPeriod / (2.0 * Math.PI);

            var startAngle = startPos.angle();
            var endAngle = startPos.angle() + Math.PI;
            var targetStartAngle = targetPos.angle();
            var targetEndAngle = targetPos.angle() + (targetRv * transferTime);

            // screw this, lets try to brute force it real fast
           
            // transfer vectors
            Vector2d startVector;
            Vector2d endVector;

            //{
            //    var p1 = startPos;
            //    args = _vm.Step();
            //    var p2 = new Vector2d(args.X, args.Y);
            //    startVector = (p1 - p2).normalize();
            //}

            if (startPos.x >= 0.0 && startPos.y >= 0.0)
                startVector = new Vector2d(-1.0 * startPos.normalize().y, startPos.normalize().x);
            else if(startPos.x < 0.0 && startPos.y < 0.0)
                startVector = new Vector2d(-1.0 * startPos.normalize().y, startPos.normalize().x);
            else if(startPos.x >= 0.0 && startPos.y < 0.0)
                startVector = new Vector2d(-1.0 * startPos.normalize().y, startPos.normalize().x);
            else //if (startPos.x < 0.0 && startPos.y >= 0.0)
                startVector = new Vector2d(-1.0 * startPos.normalize().y, startPos.normalize().x);


           
            endVector = startVector * -1.0;

            // transfer forces
            var startVelocity = this.ComputStartForce(startDistance, targetDistance);
            var endVelocity = this.ComputeEndForce(startDistance, targetDistance);
            
            _orbitVisualizer.DrawLine(startPos, startPos + (startVector * -50000000), Pens.Blue);
            _orbitVisualizer.DrawLine(endPos, endPos + (endVector * -50000000), Pens.Red);

             //do first burn
            _vm.Reset();
            //_vm.StartTrace(@"C:\\Users\\tstivers\\Desktop\\" + (uint)_configuration + ".osf", (uint)_configuration);
            _vm.Configuration = _configuration;
            //_vm.Step();
            for (int i = 0; i < delay; i++)
                _vm.Step();

            _vm.XVelocity = startVector.x * startVelocity;
            _vm.YVelocity = startVector.y * startVelocity;
            _vm.Step();
            _vm.XVelocity = 0.0;
            _vm.YVelocity = 0.0;

            // advance to apogee
            for (args = _vm.Step(); args.Elapsed <= transferTime + delay; args = _vm.Step())
                ;
               //if ((new Vector2d(args.X, args.Y).length() - targetDistance) > 0.0)
               //    break;

            Debug.WriteLine("computed = " + transferTime + ", actual = " + args.Elapsed);
            Debug.WriteLine("distance = " + (new Vector2d(args.X, args.Y).length() - targetDistance));
            endPos = new Vector2d(args.X, args.Y);
            endVector = new Vector2d(-1.0 * endPos.normalize().y, endPos.normalize().x);

            //do second burn
            _vm.XVelocity = endVector.x * endVelocity;
            _vm.YVelocity = endVector.y * endVelocity;
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

        void ChangeOrbit(double targetRadius)
        {
            var state = _vm.SnapShot();

            var args = _vm.Step();
            var startPos = new Vector2d(args.X, args.Y);
            var startDistance = startPos.length();
            var startVector = startPos.tangent();


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
