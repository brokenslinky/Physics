
using System;
using System.Collections.Generic;
using System.Linq;

namespace Physics
{
    public class Particle
    {
        public Mass mass = new Mass(Double.NaN);
        public Position position = new Position(0.0, 0.0, 0.0);
        public Momentum momentum = new Momentum(0.0, 0.0, 0.0);
        public List<Interaction> interactions = new List<Interaction>();

        public Particle (Mass mass)
        {
            this.mass = mass;
        }

        public Particle ()
        {
        }

        public Velocity velocity() { return momentum / mass; }
        public Velocity velocity(Momentum p) { return p / mass; }

        public void Iterate(Time timeStep, Force force)
        {
            Momentum changeInMomentum = timeStep * force;

            Position changeInPosition = timeStep * velocity(momentum + changeInMomentum / 2.0);

            momentum += changeInMomentum;
            position += changeInPosition;
        }
        public void Iterate(Time timeStep) { Iterate(timeStep, new Force(0.0, 0.0, 0.0)); }
    }

    public class CelestialBody : Particle
    {
        // <desription> A CelestialBody is a Particle with 
        // constructors to calculate position and momentum 
        // given a phase angle and orbital properties. </description>

        double Mu = 1.327124400189 * Math.Pow(10, 20);

        public CelestialBody(double mass, double periapsis, double apoapsis, double phaseAngle = 0.0)
        {
            this.mass = new Mass(mass);
            // just calculate at apoapsis for now
            this.position = new Position(apoapsis * Math.Cos(phaseAngle), apoapsis * Math.Sin(phaseAngle), 0.0);
            double momentumAtApoapsis = mass * Math.Sqrt(Mu * (2.0 / apoapsis - 2.0 / (apoapsis + periapsis)));
            this.momentum = new Momentum(-momentumAtApoapsis * Math.Sin(phaseAngle),
                momentumAtApoapsis * Math.Cos(phaseAngle), 0.0);
        }
    }
}
