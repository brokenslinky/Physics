using System;
using System.Collections.Generic;

namespace Physics
{
    public class Tensor
    {
        DerivedUnits _units = DerivedUnits.Unitless;
        List<List<double>> _values = new List<List<double>>();

        public List<List<double>> values { get { return _values; } set { _values = value; } }

        public DerivedUnits units { get { return _units; } set { _units = value; } }

        public Tensor(int dimensionality = 3)
        {
            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < dimensionality; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < dimensionality; j++)
                    tmp.Add(0.0);
                values.Add(tmp);
            }
            _values = values;
        }
        public Tensor(Tensor X)
        {
            _values = X.values;
            _units = X.units;
        }
        public Tensor(List<List<double>> values, DerivedUnits units)
        {
            _values = values;
            _units = units;
        }
        public Tensor(List<List<double>> values)
        {
            _values = values;
            _units = DerivedUnits.Unitless;
        }

        #region Operators

        public static Tensor operator +(Tensor X, Tensor Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            if (X.values.Count != Y.values.Count)
                throw new DimensionMismatchException();
            if (X.values[0].Count != Y.values[0].Count)
                throw new DimensionMismatchException();

            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < X.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < X.values[i].Count; j++)
                    tmp.Add(X.values[i][j] + Y.values[i][j]);
                values.Add(tmp);
            }

            return new Tensor(values, X.units);
        }

        public static Tensor operator -(Tensor X, Tensor Y)
        {
            if (X.units != Y.units)
                throw new UnitMismatchException();
            if (X.values.Count != Y.values.Count)
                throw new DimensionMismatchException();
            if (X.values[0].Count != Y.values[0].Count)
                throw new DimensionMismatchException();

            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < X.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < X.values[i].Count; j++)
                    tmp.Add(X.values[i][j] - Y.values[i][j]);
                values.Add(tmp);
            }

            return new Tensor(values, X.units);
        }

        public static Tensor operator *(double x, Tensor Y)
        {
            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < Y.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < Y.values[i].Count; j++)
                    tmp.Add(x * Y.values[i][j]);
                values.Add(tmp);
            }

            return new Tensor(values, Y.units);
        }
        public static Tensor operator *(Tensor X, double y)
        { return y * X; }

        public static Tensor operator *(Scalar X, Tensor Y)
        {
            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < Y.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < Y.values[i].Count; j++)
                    tmp.Add(X.value * Y.values[i][j]);
                values.Add(tmp);
            }

            return new Tensor(values, X.units * Y.units);
        }
        public static Tensor operator *(Tensor X, Scalar Y)
        { return Y * X; }

        public static Tensor operator /(Tensor X, double y)
        {
            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < X.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < X.values[i].Count; j++)
                    tmp.Add(X.values[i][j] / y);
                values.Add(tmp);
            }

            return new Tensor(values, X.units);
        }

        public static Tensor operator /(Tensor X, Scalar y)
        {
            List<List<double>> values = new List<List<double>>();
            for (int i = 0; i < X.values.Count; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = 0; j < X.values[i].Count; j++)
                    tmp.Add(X.values[i][j] / y.value);
                values.Add(tmp);
            }

            return new Tensor(values, X.units / y.units);
        }

        public static Vector operator *(Tensor X, Vector Y)
        {
            if (X.values[0].Count != Y.values.Count)
                throw new DimensionMismatchException();

            List<double> values = new List<double>();
            for (int i = 0; i < X.values[0].Count; i++)
            {
                double tmp = 0.0;
                for (int j = 0; j < Y.values.Count; j++)
                    tmp += X.values[i][j] * Y.values[j];
                values.Add(tmp);
            }

            return new Vector(values, X.units * Y.units);
        }

        #endregion
    }

    #region Tensor Types

    public class RotationalInertia : Tensor
    {
        public RotationalInertia(List<List<double>> values) : base(values)
        { units = DerivedUnits.Mass; }
        public RotationalInertia() : base()
        { units = DerivedUnits.Mass; }
        public RotationalInertia(RotationalInertia X) : base(X)
        { units = DerivedUnits.Mass; }
        public RotationalInertia(Tensor X) : base(X)
        {
            if (X.units != DerivedUnits.Mass)
                throw new UnitMismatchException();
        }
        
        public static RotationalInertia operator +(RotationalInertia X, RotationalInertia Y)
        { return new RotationalInertia((Tensor)X + (Tensor)Y); }
        public static RotationalInertia operator -(RotationalInertia X, RotationalInertia Y)
        { return new RotationalInertia((Tensor)X - (Tensor)Y); }
        public static RotationalInertia operator *(double x, RotationalInertia Y)
        { return new RotationalInertia(x * (Tensor)Y); }
        public static RotationalInertia operator *(RotationalInertia X, double y)
        { return y * X; }
        public static RotationalInertia operator /(RotationalInertia X, double y)
        { return new RotationalInertia((Tensor)X / y); }
    }

    #endregion
}
