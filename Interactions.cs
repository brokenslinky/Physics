using System;
using System.Collections.Generic;

namespace Physics
{
    enum InteractionType
    {
        Gravity,
        Spring
    }

    public class Interaction
    {
        public Particle A, B;
        internal InteractionType interactionType;
        internal List<Scalar> scalarParameters = new List<Scalar>();

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
            }
            // Needs to be overridden by every interaction type
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
            interactionType = InteractionType.Gravity;
            this.A = A; this.B = B;
            scalarParameters.Clear(); // I don't know if this is necessary in a constructor
            scalarParameters.Add(new Scalar(springRate, DerivedUnits.Force / DerivedUnits.Length));
            scalarParameters.Add(new Scalar(restLength, DerivedUnits.Length));
        }

        public Spring(double springRate = double.MaxValue, double restLength = 0.0)
        {
            interactionType = InteractionType.Gravity;
            scalarParameters.Clear(); // I don't know if this is necessary in a constructor
            scalarParameters.Add(new Scalar(springRate, DerivedUnits.Force / DerivedUnits.Length));
            scalarParameters.Add(new Scalar(restLength, DerivedUnits.Length));
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