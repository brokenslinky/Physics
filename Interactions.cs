using System;

namespace Physics
{
    public enum InteractionType
    {
        Gravity,
        Spring,
        Damper
    }

    /// <summary>
    /// An Interaction between Particles A and B
    /// </summary>
    public class Interaction
    {
        public Particle A, B;
        internal InteractionType interactionType;
        internal Scalar[] scalarParameters = new Scalar[4];

        internal Interaction() { }
        public Interaction(Interaction interaction)
        {
            A = interaction.A; B = interaction.B;
            interactionType = interaction.interactionType;
            scalarParameters = interaction.scalarParameters;
        }

        /// <summary>
        /// The force of this Interaction (on Particle A due to B)
        /// </summary>
        /// <returns>The force of this Interaction (on Particle A due to B)</returns>
        public Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        /// <summary>
        /// The force of this interactionType on Particles A nd B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public Force InteractionForce(Particle A, Particle B)
        {
            switch (interactionType)
            {
                case InteractionType.Gravity:
                    return Gravity.InteractionForce(A, B);
                case InteractionType.Spring:
                    return ((Spring)this).InteractionForce(A, B);
                case InteractionType.Damper:
                    return ((Damper)this).InteractionForce(A, B);
            }
            throw new NotImplementedException("InteractionForce() not defined for this Interaction");
        }
    }

    public class Spring : Interaction
    {
        public Scalar SpringRate
        { get { return scalarParameters[0]; } set { scalarParameters[0] = value; } }
        public Scalar RestLength
        { get { return scalarParameters[1]; } set { scalarParameters[1] = value; } }

        /// <summary>
        /// A spring connecting Particles A and B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="springRate"></param>
        /// <param name="restLength"></param>
        public Spring(Particle A, Particle B, double springRate, double restLength = 0.0)
        {
            interactionType = InteractionType.Spring;
            this.A = A; this.B = B;
            this.SpringRate = new Scalar(springRate, DerivedUnits.Force / DerivedUnits.Length);
            this.RestLength = new Scalar(restLength, DerivedUnits.Length);
        }

        /// <summary>
        /// The Force of this Interaction
        /// </summary>
        /// <returns>The Force of this Interaction</returns>
        public new Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        /// <summary>
        /// The force this spring would apply if stretched/compressed to the given Displacement
        /// </summary>
        /// <param name="xToY"></param>
        /// <returns>The force this spring would apply if stretched/compressed to the given Displacement</returns>
        public Force InteractionForce(Displacement xToY) 
        {
            // <summary> returns force on x due to y </summary>
            if (xToY.units != DerivedUnits.Length)
                throw new UnitMismatchException();
            Scalar magnitude = SpringRate * (xToY.Magnitude() - RestLength);
            return new Force(magnitude * xToY.Direction());
        }

        /// <summary>
        /// The Force applied by this Spring on Particles A and B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>The Force applied by this Spring on Particles A and B</returns>
        public new Force InteractionForce(Particle A, Particle B)
        {
            return InteractionForce(B.position - A.position);
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
                2.0 * dampingRatio * Math.Sqrt((spring.SpringRate * reducedMass).value);
            this.dampingCoefficient =
                new Scalar(dampingCoefficient, DerivedUnits.Force / DerivedUnits.Velocity);
        }

        public new Force InteractionForce(Particle A, Particle B)
        {
            return new Force(dampingCoefficient * (B.Velocity() - A.Velocity()));
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