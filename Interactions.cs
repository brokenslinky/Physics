using System;
using System.Collections.Generic;

namespace Physics
{
    enum InteractionType
    {
        Gravity,
        Spring,
        Damper
    }

    public class Interaction
    {
        public Particle A, B;
        internal InteractionType interactionType;
        internal Scalar[] scalarParameters = new Scalar[4];

        public Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        public Force InteractionForce(Displacement XtoY)
        {
            // return the Force on x due to this interaction with y
            // this will be MINUS the Force on y due to this interaction with x

            // This method needs to be overloaded for each interaction type

            throw new NotImplementedException("InteractionForce() not defined for this Interaction");
        }
        public Force InteractionForce(Particle x, Particle y)
        {
            switch (interactionType)
            {
                case InteractionType.Gravity:
                    return Gravity.InteractionForce(x, y);
                case InteractionType.Spring:
                    return ((Spring)this).InteractionForce(x, y);
                case InteractionType.Damper:
                    return ((Damper)this).InteractionForce(x, y);
            }
            throw new NotImplementedException("InteractionForce() not defined for this Interaction");
        }
    }

    public class Spring : Interaction
    {
        public Scalar springRate
        { get { return scalarParameters[0]; } set { scalarParameters[0] = value; } }
        public Scalar restLength
        { get { return scalarParameters[1]; } set { scalarParameters[1] = value; } }

        public Spring(Particle A, Particle B, double springRate = double.MaxValue, double restLength = 0.0)
        {
            interactionType = InteractionType.Spring;
            this.A = A; this.B = B;
            this.springRate = new Scalar(springRate, DerivedUnits.Force / DerivedUnits.Length);
            this.restLength = new Scalar(restLength, DerivedUnits.Length);
        }

        public Spring(double springRate = double.MaxValue, double restLength = 0.0)
        {
            interactionType = InteractionType.Spring;
            this.springRate = new Scalar(springRate, DerivedUnits.Force / DerivedUnits.Length);
            this.restLength = new Scalar(restLength, DerivedUnits.Length);
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

    public class Damper : Interaction
    {
        public Scalar dampingCoefficient
        {
            get { return scalarParameters[0]; }
            set { scalarParameters[0] = value; }
        }

        public Damper(double dampingCoefficient)
        {
            interactionType = InteractionType.Damper;
            this.dampingCoefficient =
                new Scalar(dampingCoefficient, DerivedUnits.Force / DerivedUnits.Velocity);
        }
        public Damper(double dampingCoefficient, Particle A, Particle B)
        {
            interactionType = InteractionType.Damper;
            this.dampingCoefficient =
                new Scalar(dampingCoefficient, DerivedUnits.Force / DerivedUnits.Velocity);
            this.A = A; this.B = B;
        }
        public Damper(Particle A, Particle B, double dampingRatio, double springRate)
        {
            interactionType = InteractionType.Damper;
            double reducedMass = (A.mass * B.mass / (A.mass + B.mass)).value;
            if (double.IsNaN(reducedMass))
                reducedMass = A.mass.value;
            if (double.IsNaN(reducedMass))
                reducedMass = B.mass.value;
            double dampingCoefficient = 2.0 * dampingRatio * Math.Sqrt(springRate * reducedMass);
            this.dampingCoefficient =
                new Scalar(dampingCoefficient, DerivedUnits.Force / DerivedUnits.Velocity);
            this.A = A; this.B = B;
        }
        public Damper(Spring spring, double dampingRatio)
        {
            interactionType = InteractionType.Damper;
            this.A = spring.A; this.B = spring.B;
            double reducedMass = (A.mass * B.mass / (A.mass + B.mass)).value;
            if (double.IsNaN(reducedMass))
                reducedMass = A.mass.value;
            if (double.IsNaN(reducedMass))
                reducedMass = B.mass.value;
            double dampingCoefficient = 
                2.0 * dampingRatio * Math.Sqrt((spring.springRate * reducedMass).value);
            this.dampingCoefficient =
                new Scalar(dampingCoefficient, DerivedUnits.Force / DerivedUnits.Velocity);
        }

        public new Force InteractionForce(Particle A, Particle B)
        {
            return new Force(dampingCoefficient * (B.velocity() - A.velocity()));
        }
    }

    public class Gravity : Interaction
    {
        static readonly Scalar G = new Scalar(6.6743015E-11, 
            DerivedUnits.Force * DerivedUnits.Length * DerivedUnits.Length /
            (DerivedUnits.Mass * DerivedUnits.Mass));

        public Gravity (Particle A, Particle B)
        {
            interactionType = InteractionType.Gravity;
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