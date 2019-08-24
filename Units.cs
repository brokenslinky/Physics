using System;
using System.Collections.Generic;

namespace Physics
{
    public enum BaseUnits
    {
        Unitless = 1,
        Displacement = 2,
        Time = 3,
        Mass = 5
    }

    public class DerivedUnits
    {
        public static readonly DerivedUnits Unitless = new DerivedUnits(BaseUnits.Unitless);
        public static readonly DerivedUnits Length = new DerivedUnits(BaseUnits.Displacement);
        public static readonly DerivedUnits Time = new DerivedUnits(BaseUnits.Time);
        public static readonly DerivedUnits Mass = new DerivedUnits(BaseUnits.Mass);
        public static readonly DerivedUnits Velocity = Length / Time;
        public static readonly DerivedUnits Momentum = Mass * Velocity;
        public static readonly DerivedUnits Acceleration = Velocity / Time;
        public static readonly DerivedUnits Force = Momentum / Time;
        public static readonly DerivedUnits Energy = Force * Length;
        public static readonly DerivedUnits Power = Energy / Time;
        public static readonly DerivedUnits AngularVelocity = Unitless / Time;
        public static readonly DerivedUnits RotationalInertia = Mass * Length * Length;
        public static readonly DerivedUnits AngularMomentum = RotationalInertia * AngularVelocity;

        public static readonly Dictionary<double, string> UnitType = new Dictionary<double, string>()
        {
            { Unitless._unitType, "Unitless" },
            { Length._unitType, "Length" },
            { Time._unitType, "Time" },
            { Mass._unitType, "Mass" },
            { Velocity._unitType, "Velocity" },
            { Momentum._unitType, "Momentum" },
            { Acceleration._unitType, "Acceleration" },
            { Force._unitType, "Force" },
            { Energy._unitType, "Energy" },
            { Power._unitType, "Power" },
            { AngularVelocity._unitType, "AngularVelocity" },
            { RotationalInertia._unitType, "RotationalInertia" },
            { AngularMomentum._unitType, "AngularMomentum" },
        };

        internal double _unitType = 1.0;

        public DerivedUnits(BaseUnits baseUnit) { _unitType = (double)baseUnit; }
        public DerivedUnits(double unitValue) { this._unitType = unitValue; }
        public DerivedUnits(DerivedUnits derivedUnits) { new DerivedUnits(derivedUnits._unitType); }
        public DerivedUnits() { new DerivedUnits(Unitless); }

        public string getUnitType()
        {
            try { return UnitType[_unitType]; }
            catch { return "Unknown unit"; }
        }

        public static DerivedUnits operator *(DerivedUnits X, DerivedUnits Y)
        {
            return new DerivedUnits(X._unitType * Y._unitType);
        }
        public static DerivedUnits operator /(DerivedUnits X, DerivedUnits Y)
        {
            return new DerivedUnits(X._unitType / Y._unitType);
        }
        public static bool operator ==(DerivedUnits X, DerivedUnits Y)
        {
            return X._unitType == Y._unitType;
        }
        public static bool operator !=(DerivedUnits X, DerivedUnits Y)
        {
            return X._unitType != Y._unitType;
        }
    }

    public class UnitMismatchException : Exception
    {
        public UnitMismatchException()
            : base("Units must match for addition, subtraction, comparison, or assignment") { }
    }

    public class DimensionMismatchException : Exception
    {
        public DimensionMismatchException()
            : base("The dimensions of these vectors do not match.") { }
    }
}
