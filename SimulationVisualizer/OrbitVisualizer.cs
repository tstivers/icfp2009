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
    public partial class OrbitVisualizer : Control
    {
        public void Clear()
        {
            
        }

        public void SetOrbiterPos(double x, double y)
        {
            _orbiterPos = new PointF((float)x, (float)y);
            this.Invalidate();
        }

        public float TargetRadius
        {
            get {
                return _targetRadius; }
            set
            {
                _targetRadius = value;
                this.UpdateBackground();                
            }
        }

        public void MoveOrbiterTo()
        {
            
        }

        public bool DrawTrail
        {
            get; set;
        }

        private float _aspectRatio;
        private PointF _orbiterPos;
        private float _targetRadius;
        private RectangleF _dimensions;
        private SizeF _scale;

        private PointF _offset;
        private Bitmap _staticBckg;
        private Bitmap _doubleBuffer;

        private Brush _backBrush = new SolidBrush(Color.Black);
        private Brush _earthBrush = new SolidBrush(Color.Blue);
        private Brush _orbiterBrush = new SolidBrush(Color.Yellow);
        private Pen _targetPen = new Pen(Color.Green);

        private Pen _gridPen = new Pen(Color.FromArgb(25, 25, 25));
        private int _gridSizeX = 10;
        private int _gridSizeY = 10;


        public OrbitVisualizer()
        {
            InitializeComponent();
            DrawTrail = true;
            _dimensions = new RectangleF(0, 0, 50000000.0f, 50000000.0f);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            using (Graphics g = Graphics.FromImage(_doubleBuffer))
            {
                g.DrawImageUnscaled(_staticBckg, 0, 0);
                g.FillEllipse(_orbiterBrush, this.GetObjectRect(_orbiterPos, 100000.0f));
            }
            pe.Graphics.DrawImageUnscaled(_doubleBuffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        private void ComputeScale()
        {
            _aspectRatio = (float)Width / (float)Height;
            _offset = new PointF(_dimensions.Width / 2.0f * _aspectRatio, _dimensions.Height / 2.0f);
            _scale = new SizeF(((float)Width / _dimensions.Width) / _aspectRatio, (float)Height / (float)_dimensions.Height);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_staticBckg != null)
                _staticBckg.Dispose();
            _staticBckg = new Bitmap(Width, Height);
            if (_doubleBuffer != null)
                _doubleBuffer.Dispose();
            _doubleBuffer = new Bitmap(Width, Height);
            ComputeScale();
            UpdateBackground();
        }

        private Rectangle GetObjectRect(PointF pos, float radius)
        {
            var center = new PointF((pos.X + _offset.X) * _scale.Width, (pos.Y + _offset.Y) * _scale.Height);
            var rect = new Rectangle(
                    (int)(center.X - (radius * _scale.Width)),
                    (int)(center.Y - (radius * _scale.Height)),
                    (int)((radius * _scale.Width) * 2),
                    (int)((radius * _scale.Height) * 2));

            // make sure it is at least 1 px by 1px
            if (rect.Width < 2)
                rect.Width = 2;
            if (rect.Height < 2)
                rect.Height = 2;

            return rect;
        }

        private void UpdateBackground()
        {
            if (_staticBckg == null)
                return;

            using (var g = Graphics.FromImage(_staticBckg))
            {
                // draw grid
                g.FillRectangle(_backBrush, 0, 0, Width, Height);
                g.DrawRectangle(_gridPen, 0, 0, Width - 1, Height - 1);
                for (int i = 0; i < Width; i += _gridSizeX)
                    g.DrawLine(_gridPen, i, 0, i, Height);
                for (int i = 0; i < Height; i += _gridSizeY)
                    g.DrawLine(_gridPen, 0, i, Width, i);

                // draw the earth
                g.FillEllipse(_earthBrush, GetObjectRect(new PointF(0.0f, 0.0f), 6357000.0f));

                // draw any target radius
                g.DrawEllipse(_targetPen, this.GetObjectRect(new PointF(0.0f, 0.0f), _targetRadius));
            }
            this.Invalidate();
        }
    }
}
