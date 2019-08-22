﻿using System;
using System.Collections.Generic;

namespace Physics
{
    public class Vector
    {
        DerivedUnits _units = DerivedUnits.Unitless;
        List<double> _values = new List<double>();

        public List<double> values { get { return _values; } set { _values = value; } }

        public void Add(double value) { _values.Add(value); }

        public DerivedUnits units { get { return _units; } set { _units = value; } }
        public Vector(List<double> values, DerivedUnits units)
        {
            _values = values;
            _units = units;
        }
        public Vector(List<double> values) { _values = values; }
        public Vector()
        {
            _values = new List<double>() { 0.0, 0.0, 0.0 };
        }

        public Scalar Magnitude()
        {
            double magnitude = 0.0;
            foreach (double value in _values)
                magnitude += value * value;
            magnitude = Math.Sqrt(magnitude);
            return new Scalar(magnitude, units);
        }

        public List<double> Direction()
        {
            double magnitude = this.Magnitude().value;
            List<double> direction = new List<double>();
            foreach (double value in _values)
                direction.Add(value / magnitude);
            return direction;
        }

        #region Operators

        public static Vector operator +(Vector X, Vector Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            if (X._values.Count != Y._values.Count)
                throw new DimensionMismatchException();

            List<double> values = new List<double>();
            for (int axis = 0; axis < X._values.Count; axis++)
                values.Add(X._values[axis] + Y._values[axis]);

            return new Vector(values, X.units);
        }

        public static Vector operator -(Vector X, Vector Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            if (X._values.Count != Y._values.Count)
                throw new DimensionMismatchException();

            List<double> values = new List<double>();
            for (int axis = 0; axis < X._values.Count; axis++)
                values.Add(X._values[axis] - Y._values[axis]);

            return new Vector(values, X.units);
        }

        public static Vector operator *(double x, Vector Y)
        {
            List<double> values = new List<double>();
            foreach (double value in Y._values)
                values.Add(value * x);

            return new Vector(values, Y.units);
        }
        public static Vector operator *(Vector X, double y)
        {
            return y * X;
        }
        public static Vector operator *(Scalar X, Vector Y)
        {
            Vector vector = X.value * Y;
            vector.units = X.units * Y.units;

            return vector;
        }
        public static Vector operator *(Vector X, Scalar Y)
        {
            return Y * X;
        }

        public static Vector operator /(Vector X, double y)
        {
            List<double> values = new List<double>();
            foreach (double value in X._values)
                values.Add(value / y);

            return new Vector(values, X.units);
        }
        public static Vector operator /(Vector X, Scalar Y)
        {
            Vector vector = X / Y.value;
            vector.units = X.units / Y.units;

            return vector;
        }

        /// <summary>Dot Product</summary>
        public static Scalar operator *(Vector X, Vector Y)
        {
            if (X._values.Count != Y._values.Count)
                throw new DimensionMismatchException();

            double value = 0.0;

            throw new NotImplementedException();
        }
        #endregion
    }

    #region Vector Types

    public class Position : Vector
    {
        public Position(List<double> values) : base(values)
        {
            units = DerivedUnits.Distance;
        }
        public Position() : base()
        {
            units = DerivedUnits.Distance;
        }
    }

    public class Velocity : Vector
    {

    }

    public class Acceleration : Vector
    {

    }

    public class Momentum : Vector
    {
        public Momentum() : base()
        {
            new Vector(new List<double>(), DerivedUnits.Momentum);
        }
        public Momentum(List<double> values) : base(values)
        {
            this.values = values;
            units = DerivedUnits.Momentum;
        }
    }

    public class Force : Vector
    {
        public Force() : base() { units = DerivedUnits.Force; }

        public Force(List<double> values) : base(values)
        {
            units = DerivedUnits.Force;
        }
    }

    #endregion

    /*
#region 3D Vectors

public class Vector_3d
{
    public double[] value = { 0.0, 0.0, 0.0 };

    public Vector_3d(double x = 0.0, double y = 0.0, double z = 0.0)
    {
        this.value[0] = x; this.value[1] = y; this.value[2] = z;
    }
    public Vector_3d(Vector_3d V) : this(V.value[0], V.value[1], V.value[2]) { }

    public double Magnitude()
    {
        return Math.Sqrt(value[0] * value[0] + value[1] * value[1] + value[2] * value[2]);
    }

    public Vector_3d Direction()
    {
        double magnitude = Magnitude();
        return new Vector_3d(value[0] / magnitude, value[1] / magnitude, value[2] / magnitude);
    }

    public static Vector_3d operator +(Vector_3d a, Vector_3d b)
    {
        for (int axis = 0; axis < a.value.Length; axis++)
            a.value[axis] += b.value[axis];
        return a;
    }
    public static Vector_3d operator -(Vector_3d a, Vector_3d b)
    {
        for (int axis = 0; axis < a.value.Length; axis++)
            a.value[axis] -= b.value[axis];
        return a;
    }
    public static Vector_3d operator *(double multiplier, Vector_3d vector)
    {
        for (int axis = 0; axis < vector.value.Length; axis++)
            vector.value[axis] *= multiplier;
        return vector;
    }
    public static Vector_3d operator *(Vector_3d vector, double multiplier)
    { return multiplier * vector; }
    public static Vector_3d operator /(Vector_3d vector, double divisor)
    {
        for (int axis = 0; axis < vector.value.Length; axis++)
            vector.value[axis] /= divisor;
        return vector;
    }

    public static Scalar operator *(Vector_3d v1, Vector_3d v2)
    {
        double dotProduct = 0.0;
        for (int axis = 0; axis < v1.value.Length; axis++)
            dotProduct += v1.value[axis] * v2.value[axis];
        return new Scalar(dotProduct);
    }
}

public class Position : Vector_3d
{
    public Position(double x = 0.0, double y = 0.0, double z = 0.0) : base(x, y, z) { }
    public Position(Vector_3d v) : base(v.value[0], v.value[1], v.value[2]) { }

    public static Position operator +(Position a, Position b)
    { return new Position(new Vector_3d(a) + new Vector_3d(b)); }
    public static Position operator -(Position a, Position b)
    { return new Position(new Vector_3d(a) - new Vector_3d(b)); }
    public static Position operator *(double n, Position x)
    { return new Position(n * new Vector_3d(x)); }
    public static Position operator *(Position x, double n)
    { return new Position(n * new Vector_3d(x)); }
    public static Position operator /(Position x, double n)
    { return new Position(new Vector_3d(x) / n); }

    public static Velocity operator / (Position x, Time t) { return new Velocity(x / t.value); }
}

public class Velocity : Vector_3d
{
    public Velocity(double x = 0.0, double y = 0.0, double z = 0.0) : base(x, y, z) { }
    public Velocity(Vector_3d v) : base(v.value[0], v.value[1], v.value[2]) { }

    public static Velocity operator +(Velocity a, Velocity b)
    { return new Velocity(new Vector_3d(a) + new Vector_3d(b)); }
    public static Velocity operator -(Velocity a, Velocity b)
    { return new Velocity(new Vector_3d(a) - new Vector_3d(b)); }
    public static Velocity operator *(double n, Velocity v)
    { return new Velocity(n * new Vector_3d(v)); }
    public static Velocity operator *(Velocity v, double n)
    { return new Velocity(n * new Vector_3d(v)); }
    public static Velocity operator /(Velocity v, double n)
    { return new Velocity(new Vector_3d(v) / n); }

    public static Momentum operator *(Mass m, Velocity v) { return new Momentum(m.value * v); }
    public static Momentum operator *(Velocity v, Mass m) { return new Momentum(m.value * v); }

    public static Position operator *(Time t, Velocity v)
    { return new Position(new Vector_3d(v) * t.value); }
    public static Position operator *(Velocity v, Time t)
    { return new Position(new Vector_3d(v) * t.value); }

    public static Acceleration operator / (Velocity v, Time t)
    { return new Acceleration(new Vector_3d(v) / t.value); }
}

public class Acceleration : Vector_3d
{
    public Acceleration(double x = 0.0, double y = 0.0, double z = 0.0) : base(x, y, z) { }
    public Acceleration(Vector_3d v) : base(v.value[0], v.value[1], v.value[2]) { }

    public static Acceleration operator *(double n, Acceleration a)
    { return new Acceleration(n * new Vector_3d(a)); }
    public static Acceleration operator *(Acceleration a, double n)
    { return new Acceleration(n * new Vector_3d(a)); }

    public static Velocity operator *(Acceleration a, Time t) { return new Velocity(a * t.value); }
    public static Velocity operator *(Time t, Acceleration a) { return new Velocity(a * t.value); }

    public static Force operator *(Mass m, Acceleration a) { return new Force(m.value * a); }
    public static Force operator *(Acceleration a, Mass m) { return new Force(m.value * a); }
}

public class Force : Vector_3d
{
    public Force(double x = 0.0, double y = 0.0, double z = 0.0) : base(x, y, z) { }
    public Force(Vector_3d v) : base(v.value[0], v.value[1], v.value[2]) { }

    public static Force operator +(Force a, Force b)
    { return new Force(new Vector_3d(a) + new Vector_3d(b)); }
    public static Force operator -(Force a, Force b)
    { return new Force(new Vector_3d(a) - new Vector_3d(b)); }
    public static Force operator *(double n, Force F)
    { return new Force(n * new Vector_3d(F)); }
    public static Force operator *(Force F, double n)
    { return new Force(n * new Vector_3d(F)); }
    public static Force operator /(Force F, double n)
    { return new Force(new Vector_3d(F) / n); }

    public static Momentum operator *(Force F, Time t)
    { return new Momentum(new Vector_3d(F) * t.value); }
    public static Momentum operator *(Time t, Force F)
    { return new Momentum(new Vector_3d(F) * t.value); }

    public static Energy operator *(Force F, Position x)
    { return new Energy(new Vector_3d(F) * new Vector_3d(x)); }
    public static Energy operator *(Position x, Force F)
    { return new Energy(new Vector_3d(F) * new Vector_3d(x)); }

    public static Acceleration operator /(Force F, Mass m)
    { return new Acceleration(new Vector_3d(F) / m.value); }
}

public class Momentum : Vector_3d
{
    public Momentum(double x, double y, double z) : base(x, y, z) { }
    public Momentum(Vector_3d v) : base(v.value[0], v.value[1], v.value[2]) { }

    public static Momentum operator +(Momentum a, Momentum b)
    { return new Momentum(new Vector_3d(a) + new Vector_3d(b)); }
    public static Momentum operator -(Momentum a, Momentum b)
    { return new Momentum(new Vector_3d(a) - new Vector_3d(b)); }
    public static Momentum operator *(double n, Momentum p)
    { return new Momentum(n * new Vector_3d(p)); }
    public static Momentum operator *(Momentum p, double n)
    { return new Momentum(n * new Vector_3d(p)); }
    public static Momentum operator /(Momentum p, double n)
    { return new Momentum(new Vector_3d(p) / n); }

    public static Energy operator *(Momentum p, Velocity v)
    { return new Energy(new Vector_3d(p) * new Vector_3d(v)); }
    public static Energy operator *(Velocity v, Momentum p)
    { return new Energy(new Vector_3d(p) * new Vector_3d(v)); }

    public static Force operator / (Momentum p, Time t)
    { return new Force(new Vector_3d(p) / t.value); }

    public static Velocity operator /(Momentum p, Mass m)
    { return new Velocity(new Vector_3d(p) / m.value); }
}

#endregion
*/
}
