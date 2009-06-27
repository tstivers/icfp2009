using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace icfp09
{
    using System.Drawing;

    public struct Vector2d
    {
        public static Vector2d FromAngle(double angle)
        {
            return new Vector2d(angle);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((Vector2d)obj).x == this.x &&
                ((Vector2d)obj).y == this.y;
        }

        public override string ToString()
        {
            return "<" + x.ToString() + ", " + y.ToString() + ">";
        }

        public Vector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2d(double angle)
        {
            this.x = Math.Sin(angle);
            this.y = Math.Cos(angle);
        }

        public Vector2d tangent()
        {
            var nrm = this.normalize();
            return new Vector2d(-1.0 * nrm.y, nrm.x);
            //if (nrm.x >= 0.0 && nrm.y >= 0.0)
            //    tan = new Vector2d(-1.0 * nrm.y, nrm.x);
            //else if (nrm.x < 0.0 && nrm.y < 0.0)
            //    tan = new Vector2d(-1.0 * nrm.y, nrm.x);
            //else if (nrm.x >= 0.0 && nrm.y < 0.0)
            //    tan = new Vector2d(-1.0 * nrm.y, nrm.x);
            //else //if (nrm.x < 0.0 && nrm.y >= 0.0)
            //    tan = new Vector2d(-1.0 * nrm.y, nrm.x);

            //return tan;
        }

        public double angle(Vector2d other)
        {
            return (double)(Math.Atan2(other.y, other.x) - Math.Atan2(this.y, this.x));
        }

        public double angle()
        {
            return angle(new Vector2d(0, 1));
        }

        public double dot(Vector2d other)
        {
            return (this.x * other.x) + (this.y * other.y);
        }

        public Vector2d normalize()
        {
            double length = this.length();
            return new Vector2d(x / length, y / length);
        }

        public double length()
        {
            return (double)(Math.Sqrt((x * x) + (y * y)));
        }

        public Vector2d lerp(Vector2d end, double distance)
        {
            return ((end - this) * distance) + this;
        }

        public static Vector2d operator +(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2d operator *(double f, Vector2d v)
        {
            return new Vector2d(v.x * f, v.y * f);
        }

        public static Vector2d operator *(Vector2d v, double f)
        {
            return new Vector2d(v.x * f, v.y * f);
        }

        public static double operator *(Vector2d v1, Vector2d v2)
        {
            return v1.dot(v2);
        }

        public static double operator ^(Vector2d v1, Vector2d v2)
        {
            return v1.cross(v2);
        }

        private double cross(Vector2d other)
        {
            return (this.x * other.y) - (this.y * other.x);
        }

        public static implicit operator PointF(Vector2d v)
        {
            return new PointF((float)v.x, (float)v.y);
        }

        public double x;
        public double y;
    }
}
