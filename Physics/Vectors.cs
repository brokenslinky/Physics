using System;
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
        public Vector(Vector X)
        {
            values = X.values;
            units = X.units;
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
        { units = DerivedUnits.Distance; }
        public Position() : base()
        { units = DerivedUnits.Distance; }
        public Position(Position X) : base(X)
        { units = DerivedUnits.Distance; }
        public Position(Vector X) : base(X)
        {
            if (X.units != DerivedUnits.Distance)
                throw new UnitMismatchException();
        }
        
        public static Position operator +(Position X, Position Y)
        { return new Position((Vector)X + (Vector)Y); }
        public static Position operator -(Position X, Position Y)
        { return new Position((Vector)X - (Vector)Y); }
        public static Position operator *(double x, Position Y)
        { return new Position(x * (Vector)Y); }
        public static Position operator *(Position X, double y)
        { return y * X; }
        public static Position operator /(Position X, double y)
        { return new Position((Vector)X / y); }

        public static Velocity operator /(Position X, Time y)
        { return new Velocity((Vector)X / y.value); }
    }

    public class Velocity : Vector
    {
        public Velocity(List<double> values) : base(values)
        { units = DerivedUnits.Velocity; }
        public Velocity() : base()
        { units = DerivedUnits.Velocity; }
        public Velocity(Position X) : base(X)
        { units = DerivedUnits.Velocity; }
        public Velocity(Vector X) : base(X)
        {
            if (X.units != DerivedUnits.Velocity)
                throw new UnitMismatchException();
        }

        public static Velocity operator +(Velocity X, Velocity Y)
        { return new Velocity((Vector)X + (Vector)Y); }
        public static Velocity operator -(Velocity X, Velocity Y)
        { return new Velocity((Vector)X - (Vector)Y); }
        public static Velocity operator *(double x, Velocity Y)
        { return new Velocity(x * (Vector)Y); }
        public static Velocity operator *(Velocity X, double y)
        { return y * X; }
        public static Velocity operator /(Velocity X, double y)
        { return new Velocity((Vector)X / y); }

        public static Position operator *(Velocity X, Time Y)
        { return new Position((Vector)X * Y); }
        public static Position operator *(Time X, Velocity Y)
        { return Y * X; }

        public static Momentum operator *(Mass X, Velocity Y)
        { return new Momentum((Scalar)X * (Velocity)Y); }
        public static Momentum operator *(Velocity X, Mass Y)
        { return Y * X; }

        public static Acceleration operator /(Velocity X, Time Y)
        { return new Acceleration((Vector)X / (Scalar)Y); }
    }

    public class Acceleration : Vector
    {
        public Acceleration(List<double> values) : base(values)
        { units = DerivedUnits.Acceleration; }
        public Acceleration() : base()
        { units = DerivedUnits.Acceleration; }
        public Acceleration(Position X) : base(X)
        { units = DerivedUnits.Acceleration; }
        public Acceleration(Vector X) : base(X)
        {
            if (X.units != DerivedUnits.Acceleration)
                throw new UnitMismatchException();
        }

        public static Acceleration operator +(Acceleration X, Acceleration Y)
        { return new Acceleration((Vector)X + (Vector)Y); }
        public static Acceleration operator -(Acceleration X, Acceleration Y)
        { return new Acceleration((Vector)X - (Vector)Y); }
        public static Acceleration operator *(double x, Acceleration Y)
        { return new Acceleration(x * (Vector)Y); }
        public static Acceleration operator *(Acceleration X, double y)
        { return y * X; }
        public static Acceleration operator /(Acceleration X, double y)
        { return new Acceleration((Vector)X / y); }

        public static Force operator *(Mass X, Acceleration Y)
        { return new Force((Scalar)X * (Vector)Y); }
        public static Force operator *(Acceleration X, Mass Y)
        { return Y * X; }

        public static Velocity operator *(Time X, Acceleration Y)
        { return new Velocity((Scalar)X * (Vector)Y); }
        public static Velocity operator *(Acceleration X, Time Y)
        { return Y * X; }
    }

    public class Momentum : Vector
    {
        public Momentum(List<double> values) : base(values)
        { units = DerivedUnits.Momentum; }
        public Momentum() : base()
        { units = DerivedUnits.Momentum; }
        public Momentum(Position X) : base(X)
        { units = DerivedUnits.Momentum; }
        public Momentum(Vector X) : base(X)
        {
            if (X.units != DerivedUnits.Momentum)
                throw new UnitMismatchException();
        }

        public static Momentum operator +(Momentum X, Momentum Y)
        { return new Momentum((Vector)X + (Vector)Y); }
        public static Momentum operator -(Momentum X, Momentum Y)
        { return new Momentum((Vector)X - (Vector)Y); }
        public static Momentum operator *(double x, Momentum Y)
        { return new Momentum(x * (Vector)Y); }
        public static Momentum operator *(Momentum X, double y)
        { return y * X; }
        public static Momentum operator /(Momentum X, double y)
        { return new Momentum((Vector)X / y); }

        public static Force operator /(Momentum X, Time Y)
        { return new Force((Vector)X / (Scalar)Y); }

        public static Velocity operator /(Momentum X, Mass Y)
        { return new Velocity((Vector)X / (Scalar)Y); }

        public static Energy operator *(Momentum X, Velocity Y)
        { return new Energy((Vector)X * (Vector)Y); }
        public static Energy operator *(Velocity X, Momentum Y)
        { return Y * X; }

        public static Power operator *(Momentum X, Acceleration Y)
        { return new Power((Vector)X * (Vector)Y); }
        public static Power operator *(Acceleration X, Momentum Y)
        { return Y * X; }
    }

    public class Force : Vector
    {
        public Force(List<double> values) : base(values)
        { units = DerivedUnits.Force; }
        public Force() : base()
        { units = DerivedUnits.Force; }
        public Force(Position X) : base(X)
        { units = DerivedUnits.Force; }
        public Force(Vector X) : base(X)
        {
            if (X.units != DerivedUnits.Force)
                throw new UnitMismatchException();
        }

        public static Force operator +(Force X, Force Y)
        { return new Force((Vector)X + (Vector)Y); }
        public static Force operator -(Force X, Force Y)
        { return new Force((Vector)X - (Vector)Y); }
        public static Force operator *(double x, Force Y)
        { return new Force(x * (Vector)Y); }
        public static Force operator *(Force X, double y)
        { return y * X; }
        public static Force operator /(Force X, double y)
        { return new Force((Vector)X / y); }

        public static Momentum operator *(Force X, Time Y)
        { return new Momentum((Vector)X * (Scalar)Y); }
        public static Momentum operator *(Time X, Force Y)
        { return Y * X; }

        public static Acceleration operator /(Force X, Time Y)
        { return new Acceleration((Vector)X / (Scalar)Y); }

        public static Energy operator *(Force X, Position Y)
        { return new Energy((Vector)X * (Vector)Y); }
        public static Energy operator *(Position X, Force Y)
        { return Y * X; }

        public static Power operator *(Force X, Velocity Y)
        { return new Power((Vector)X * (Vector)Y); }
        public static Power operator *(Velocity X, Force Y)
        { return Y * X; }
    }

    #endregion
}
