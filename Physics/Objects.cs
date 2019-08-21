
using System;
using System.Collections.Generic;
using System.Linq;

namespace Physics
{
    public class Particle
    {
        public Scalar mass = new Scalar(Double.NaN, DerivedUnits.Mass);
        public Vector position = new Vector(new List<double>() { 0.0 }, DerivedUnits.Distance);
        public Vector momentum = new Vector(new List<double>() { 0.0 }, DerivedUnits.Momentum);
        public List<Interaction> interactions = new List<Interaction>();

        public Particle (Mass mass)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                position._values.Add(0.0);
                momentum._values.Add(0.0);
            }
            this.mass = mass;
        }

        public Particle ()
        {
        }

        public Vector velocity() { return momentum / mass; }
        public Vector velocity(Vector p) { return p / mass; }

        public void Iterate(Scalar timeStep, Vector force)
        {
            Vector changeInMomentum = timeStep * force;

            Vector changeInPosition = timeStep * velocity(momentum + changeInMomentum / 2.0);

            momentum += changeInMomentum;
            position += changeInPosition;
        }
        public void Iterate(Time timeStep)
        {
            Vector netForce = new Force();
            foreach (Interaction interaction in interactions)
                netForce += interaction.InteractionForce();
            Iterate(timeStep, netForce);
        }
    }


    // <desription> A CelestialBody is a Particle with 
    // constructors to calculate position and momentum 
    // given a phase angle and orbital properties. </description>
    public class CelestialBody : Particle
    {

        public CelestialBody(double mass, double periapsis, double apoapsis, 
            double phaseAngle = 0.0, double Mu = 1.327124400189E20)
        {
            this.mass = new Mass(mass);
            // just calculate at apoapsis for now
            this.position = 
                new Position(new List<double>()
                {
                    apoapsis * Math.Cos(phaseAngle),
                    apoapsis * Math.Sin(phaseAngle),
                    0.0
                });
            double momentumAtApoapsis = mass * Math.Sqrt(Mu * (2.0 / apoapsis - 2.0 / (apoapsis + periapsis)));
            if (double.IsNaN(momentumAtApoapsis))
                momentumAtApoapsis = 0.0;
            this.momentum = 
                new Momentum(new List<double>()
                {
                    -momentumAtApoapsis * Math.Sin(phaseAngle),
                    momentumAtApoapsis * Math.Cos(phaseAngle),
                    0.0
                });
        }
    }
}
