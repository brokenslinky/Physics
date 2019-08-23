using System;

namespace Physics
{
    public class Interaction
    {
        public Particle A, B;

        public Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        public Force InteractionForce(Displacement XtoY)
        {
            // return the Force on x due to this interaction with y
            // this will be MINUS the Force on y due to this interaction with x

            // This method needs to be overloaded for each interaction type

            throw new System.NotImplementedException("InteractionForce() not defined for this Interaction");
        }
        public Force InteractionForce(Particle x, Particle y)
        {
            // Needs to be overridden by every interaction type
            throw new System.NotImplementedException("InteractionForce() not defined for this Interaction");
        }
    }

    public class Spring : Interaction
    {
        public Scalar springRate = new Scalar(double.PositiveInfinity, 
            DerivedUnits.Force / DerivedUnits.Length);
        public Scalar restLength = new Scalar(0.0, DerivedUnits.Length);

        public Spring(Particle A, Particle B, double springRate = double.MaxValue, double restLength = 0.0)
        {
            this.A = A; this.B = B;
            this.springRate.value = springRate; this.restLength.value = restLength;
        }

        public Spring(double springRate = double.MaxValue, double restLength = 0.0)
        {
            this.springRate.value = springRate;
            this.restLength.value = restLength;
        }

        public new Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        public new Force InteractionForce(Displacement xToY) 
        {
            // <summary> returns force on x due to y </summary>
            if (xToY.units != DerivedUnits.Length)
                throw new UnitMismatchException();
            Scalar magnitude = springRate * (xToY.Magnitude() - restLength);
            return new Force(magnitude * xToY.Direction());
        }

        public new Force InteractionForce(Particle x, Particle y)
        {
            return InteractionForce(y.position - x.position);
        }
    }

    public class Gravity : Interaction
    {
        static readonly Scalar G = new Scalar(6.6743015E-11, 
            DerivedUnits.Force * DerivedUnits.Length * DerivedUnits.Length /
            (DerivedUnits.Mass * DerivedUnits.Mass));

        public Gravity (Particle A, Particle B)
        {
            this.A = A; this.B = B;
        }

        public new Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        public new static Force InteractionForce(Particle A, Particle B)
        {
            Displacement AtoB = B.position - A.position;
            Scalar distance = AtoB.Magnitude();
            Scalar magnitudeOfForce = G * (A.mass * B.mass) / (distance * distance);
            return new Force(magnitudeOfForce * AtoB.Direction());
        }


    }
}