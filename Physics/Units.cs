using System;
using System.Collections.Generic;

namespace Physics
{
    public enum BaseUnits
    {
        Unitless = 1,
        Distance = 2,
        Time = 3,
        Mass = 5
    }

    public class DerivedUnits
    {
        public static readonly DerivedUnits Unitless = new DerivedUnits(BaseUnits.Unitless);
        public static readonly DerivedUnits Distance = new DerivedUnits(BaseUnits.Distance);
        public static readonly DerivedUnits Time = new DerivedUnits(BaseUnits.Time);
        public static readonly DerivedUnits Mass = new DerivedUnits(BaseUnits.Mass);
        public static readonly DerivedUnits Momentum = Mass * Distance / Time;
        public static readonly DerivedUnits Force = Momentum / Time;
        public static readonly DerivedUnits Energy = Force * Distance;

        private double unitType = 1.0;

        public DerivedUnits(BaseUnits baseUnit) { unitType = (double)baseUnit; }
        public DerivedUnits(double unitValue) { this.unitType = unitValue; }
        public DerivedUnits(DerivedUnits derivedUnits) { new DerivedUnits(derivedUnits.unitType); }
        public DerivedUnits() { new DerivedUnits(Unitless); }

        public static DerivedUnits operator *(DerivedUnits X, DerivedUnits Y)
        {
            return new DerivedUnits(X.unitType * Y.unitType);
        }
        public static DerivedUnits operator /(DerivedUnits X, DerivedUnits Y)
        {
            return new DerivedUnits(X.unitType / Y.unitType);
        }
        public static bool operator ==(DerivedUnits X, DerivedUnits Y)
        {
            return X.unitType == Y.unitType;
        }
        public static bool operator !=(DerivedUnits X, DerivedUnits Y)
        {
            return X.unitType != Y.unitType;
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
