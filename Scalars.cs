using System;
using System.Collections.Generic;

namespace Physics
{
    public class Scalar
    {
        DerivedUnits _units = new DerivedUnits(1.0);
        double _value = 0.0;

        public double value { get { return _value; } set { _value = value; } }
        public DerivedUnits units { get { return _units; } set { _units = value; } }

        public Scalar(double value, DerivedUnits units) { _value = value; _units = units; }
        public Scalar(double value = 0.0) { _value = value; }

        public Scalar(Scalar scalar)
        {
            _value = scalar.value;
            _units = scalar.units;
        }

        #region Operations

        public static Scalar operator +(Scalar X, Scalar Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            return new Scalar(X._value + Y._value, X.units);
        }

        public static Scalar operator -(Scalar X, Scalar Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            return new Scalar(X._value - Y._value, X.units);
        }

        public static Scalar operator *(Scalar X, Scalar Y)
        {
            return new Scalar(X._value * Y._value, X.units * Y.units);
        }

        public static Scalar operator /(Scalar X, Scalar Y)
        {
            return new Scalar(X._value / Y._value, X.units / Y.units);
        }

        public static Scalar operator *(Scalar X, double y)
        {
            return new Scalar(X._value * y, X.units);
        }
        public static Scalar operator *(double x, Scalar Y)
        {
            return Y * x;
        }

        public static Scalar operator /(Scalar X, double y)
        {
            return new Scalar(X._value / y, X.units);
        }

        public static Vector operator *(Scalar X, List<double> y)
        {
            List<double> values = new List<double>();
            foreach (double value in y)
                values.Add(X._value * value);
            return new Vector(values, X.units);
        }
        public static Vector operator *(List<double> x, Scalar Y)
        {
            return Y * x;
        }

        public static bool operator <(Scalar X, Scalar Y)
        {
            return X.value < Y.value;
        }
        public static bool operator >(Scalar X, Scalar Y)
        {
            return X.value > Y.value;
        }
        public static bool operator <=(Scalar X, Scalar Y)
        {
            return X.value <= Y.value;
        }
        public static bool operator >=(Scalar X, Scalar Y)
        {
            return X.value >= Y.value;
        }

        #endregion
    }

    #region Scalar Types

    public class Mass : Scalar
    {
        public Mass(double value = double.NaN) : base(value) { units = DerivedUnits.Mass; }
        public Mass(Mass mass) : base(mass) { }
        public Mass(Scalar scalar) : base(scalar)
        {
            if (scalar.units != DerivedUnits.Mass)
                throw new UnitMismatchException();
        }

        public static Mass operator +(Mass X, Mass Y)
        {
            return new Mass(X.value + Y.value);
        }
        public static Mass operator -(Mass X, Mass Y)
        {
            return new Mass(X.value - Y.value);
        }
        public static Mass operator *(Mass X, double y)
        {
            return new Mass(X.value * y);
        }
        public static Mass operator *(double x, Mass Y)
        {
            return Y * x;
        }
        public static Mass operator /(Mass X, double y)
        {
            return new Mass(X.value / y);
        }

        public static double operator /(Mass X, Mass Y)
        {
            return X.value / Y.value;
        }
    }

    public class Time : Scalar
    {
        public Time(double value = 0.0) : base(value)
        {
            units = DerivedUnits.Time;
        }
        public Time(Time time) : base((Scalar)time) { }
        public Time(Scalar scalar) : base(scalar)
        {
            if (scalar.units != DerivedUnits.Time)
                throw new UnitMismatchException();
        }

        public static Time operator +(Time X, Time Y)
        {
            return new Time(X.value + Y.value);
        }
        public static Time operator -(Time X, Time Y)
        {
            return new Time(X.value - Y.value);
        }
        public static Time operator *(Time X, double y)
        {
            return new Time(X.value * y);
        }
        public static Time operator *(double x, Time Y)
        {
            return Y * x;
        }
        public static Time operator /(Time X, double y)
        {
            return new Time(X.value / y);
        }

        public static double operator /(Time X, Time Y)
        {
            return X.value / Y.value;
        }
    }

    public class Energy : Scalar
    {
        public Energy(double value = 0.0) : base(value)
        {
            units = DerivedUnits.Energy;
        }
        public Energy(Energy energy) : base((Scalar)energy) { }
        public Energy(Scalar scalar) : base(scalar)
        {
            if (scalar.units != DerivedUnits.Energy)
                throw new UnitMismatchException();
        }

        public static Energy operator +(Energy X, Energy Y)
        {
            return new Energy(X.value + Y.value);
        }
        public static Energy operator -(Energy X, Energy Y)
        {
            return new Energy(X.value - Y.value);
        }
        public static Energy operator *(Energy X, double y)
        {
            return new Energy(X.value * y);
        }
        public static Energy operator *(double x, Energy Y)
        {
            return Y * x;
        }
        public static Energy operator /(Energy X, double y)
        {
            return new Energy(X.value / y);
        }

        public static double operator /(Energy X, Energy Y)
        {
            return X.value / Y.value;
        }

        public static Power operator /(Energy X, Time Y)
        {
            return new Power(X.value / Y.value);
        }
    }

    public class Power : Scalar
    {
        public Power(double value = 0.0) : base(value)
        {
            units = DerivedUnits.Energy;
        }
        public Power(Power power) : base((Scalar)power) { }
        public Power(Scalar scalar) : base(scalar)
        {
            if (scalar.units != DerivedUnits.Power)
                throw new UnitMismatchException();
        }

        public static Power operator +(Power X, Power Y)
        {
            return new Power(X.value + Y.value);
        }
        public static Power operator -(Power X, Power Y)
        {
            return new Power(X.value - Y.value);
        }
        public static Power operator *(Power X, double y)
        {
            return new Power(X.value * y);
        }
        public static Power operator *(double x, Power Y)
        {
            return Y * x;
        }
        public static Power operator /(Power X, double y)
        {
            return new Power(X.value / y);
        }

        public static double operator /(Power X, Power Y)
        {
            return X.value / Y.value;
        }

        public static Energy operator *(Power X, Time Y)
        {
            return new Energy(X.value * Y.value);
        }
        public static Energy operator *(Time X, Power Y)
        {
            return Y * X;
        }
    }

    #endregion
}
