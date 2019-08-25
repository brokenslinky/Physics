
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Physics
{
    public class System
    {
        public List<Particle> particles = new List<Particle>();

        private List<Interaction> _interactions = new List<Interaction>();

        public System()
        {
            particles = new List<Particle>();
            _interactions = new List<Interaction>();
        }
        public System(List<Particle> particles)
        {
            this.particles = particles;
            _interactions = new List<Interaction>();
        }
        public System(List<Particle> particles, List<Interaction> interactions)
        {
            this.particles = particles;
            _interactions = interactions;
        }

        /// <summary>
        /// Add a particle to the system
        /// </summary>
        /// <param name="particle"></param>
        public void Add(Particle particle)
        {
            particles.Add(particle);
        }

        /// <summary>
        /// Update the positions and momentums of all particles in the system
        /// </summary>
        /// <param name="timeStep">The amount of time passing during this iteration</param>
        public void Iterate(Time timeStep)
        {
            Force[] forces = new Force[particles.Count];
            List<Task> tasks = new List<Task>();

            async Task CalculateForce(int particleIndex)
            {
                forces[particleIndex] = new Force();
                foreach (Interaction interaction in particles[particleIndex].interactions)
                {
                    forces[particleIndex] += interaction.InteractionForce();
                }
            }

            async Task UpdateParticle(int particleIndex)
            {
                particles[particleIndex].Iterate(timeStep, forces[particleIndex]);
            }

            // calculate the force on each particle
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                int i = particleIndex;
                tasks.Add(CalculateForce(i));
            }
            Task.WaitAll(tasks.ToArray());

            // update the position and momentum of each particle
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                int i = particleIndex;
                tasks.Add(UpdateParticle(i));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }

    public class Particle
    {
        public Mass mass = new Mass();
        public Displacement position = new Displacement(new List<double>() { 0.0, 0.0, 0.0 });
        public Momentum momentum = new Momentum(new List<double>() { 0.0, 0.0, 0.0 });
        public List<Interaction> interactions = new List<Interaction>();

        public Particle (Mass mass)
        {
            this.mass = mass;
        }
        public Particle (Particle particle)
        {
            mass = particle.mass;
            position = particle.position;
            momentum = particle.momentum;
            interactions = particle.interactions;
        }
        public Particle ()
        {
        }

        public Velocity velocity() { return momentum / mass; }
        public Velocity velocity(Momentum p) { return p / mass; }

        public void Iterate(Time timeStep, Force force)
        {
            Momentum changeInMomentum = timeStep * force;

            Displacement changeInPosition = timeStep * velocity(momentum + changeInMomentum / 2.0);

            momentum += changeInMomentum;
            position += changeInPosition;
        }
        public void Iterate(Time timeStep)
        {
            Force netForce = new Force();
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
