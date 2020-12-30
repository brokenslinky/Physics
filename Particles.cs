
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physics
{
    /// <summary>
    /// A Particle has mass, position, momentum, and Interactions with other Particles.
    /// </summary>
    public class Particle
    {
        public Mass mass = new Mass();
        public Displacement position = new Displacement(new List<double>() { 0.0, 0.0, 0.0 });
        public Momentum momentum = new Momentum(new List<double>() { 0.0, 0.0, 0.0 });
        public List<Interaction> interactions = new List<Interaction>();

        /// <summary>
        /// Create a new Particle at position {0,0,0} with {0,0,0] momentum
        /// </summary>
        /// <param name="mass"></param>
        public Particle (Mass mass)
        {
            this.mass = mass;
        }
        /// <summary>
        /// Copy a Particle
        /// </summary>
        /// <param name="mass"></param>
        public Particle (Particle particle)
        {
            mass = particle.mass;
            position = particle.position;
            momentum = particle.momentum;
            interactions = particle.interactions;
        }
        /// <summary>
        /// Create a new Particle at position {0,0,0} with {0,0,0] momentum
        /// </summary>
        /// <param name="mass"></param>
        public Particle ()
        {
        }

        /// <summary>
        /// The Velocity of this Particle at this instant
        /// </summary>
        /// <returns>The Velocity of this Particle at this instant</returns>
        public Velocity Velocity() { return momentum / mass; }
        /// <summary>
        /// The Velocity of this Particle given a momentum
        /// </summary>
        /// <returns>The Velocity of this Particle given a momentum</returns>
        public Velocity Velocity(Momentum p) { return p / mass; }

        /// <summary>
        /// Allow timeInterval to pass for the Particle with given netForce applied. 
        /// Recommend putting Particle into a PhysicalSystem and using a PhysicalSystem.Iterate() instead.
        /// </summary>
        /// <param name="timeInterval"></param>
        /// <param name="netForce"></param>
        public void Iterate(Time timeInterval, Force netForce)
        {
            Momentum changeInMomentum = timeInterval * netForce;

            Displacement changeInPosition = timeInterval * Velocity(momentum + changeInMomentum / 2.0);

            momentum += changeInMomentum;
            position += changeInPosition;
        }
        /// <summary>
        /// Allow timeInterval to pass for the Particle. 
        /// Recommend putting Particle into a PhysicalSystem and using a PhysicalSystem.Iterate() instead.
        /// </summary>
        /// <param name="timeInterval"></param>
        public void Iterate(Time timeInterval)
        {
            Force netForce = new Force();
            foreach (Interaction interaction in interactions)
                netForce += interaction.InteractionForce();
            Iterate(timeInterval, netForce);
        }
    }

    /// <summary>
    /// A CelestialBody is a Particle with 
    /// constructors to calculate position and momentum 
    /// given a phase angle and orbital properties.
    /// </summary>
    public class CelestialBody : Particle
    {
        /// <summary>
        /// Create new Celestrial body at an orbit in the 0,1 ("x,y") plane
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="periapsis"></param>
        /// <param name="apoapsis"></param>
        /// <param name="phaseAngle"></param>
        /// <param name="Mu"></param>
        public CelestialBody(double mass, double periapsis, double apoapsis, 
            double phaseAngle = 0.0, double Mu = 1.327124400189E20)
        {
            this.mass = new Mass(mass);
            // just calculate at apoapsis for now
            this.position = 
                new Displacement(new List<double>()
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
