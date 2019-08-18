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

        public Force InteractionForce(Position XtoY)
        {
            // return the Force on x due to this interaction with y
            // this will be MINUS the Force on y due to this interaction with x

            // This method needs to be overloaded for each interaction type

            throw new System.NotImplementedException("Force() not defined for this Interaction");
        }
        public Force InteractionForce(Particle x, Particle y)
        {
            // Needs to be overridden by every interaction type
            throw new System.NotImplementedException("Force() not defined for this Interaction");
        }
    }

    public class Spring : Interaction
    {
        public double springRate;
        public double restLength;

        public Spring(Particle A, Particle B, double springRate = double.MaxValue, double restLength = 0.0)
        {
            this.A = A; this.B = B; this.springRate = springRate; this.restLength = restLength;
        }

        public Spring(double rate = double.MaxValue, double restLength = 0.0)
        {
            springRate = rate;
            this.restLength = restLength;
        }

        public Force InteractionForce()
        {
            return InteractionForce(A, B);
        }

        public Force InteractionForce(Position xToY) 
        {
            // <summary> returns force on x due to y </summary>
            double magnitude = springRate * (xToY.Magnitude() - restLength);
            return new Force(magnitude * xToY.Direction());
        }

        public Force InteractionForce(Particle x, Particle y)
        {
            return InteractionForce(y.position - x.position);
        }
    }

    public class Gravity : Interaction
    {
        const double G = 0.000000000066743015;

    }
}