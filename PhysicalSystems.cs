using System.Collections.Generic;
using System.Threading.Tasks;

namespace Physics
{
    /// <summary>
    /// A PhysicalSystem is a collection of Particles and Interactions.
    /// </summary>
    public class PhysicalSystem
    {
        // Can I give the user direct access to members of the Particles without making this public?
        public List<Particle> particles = new List<Particle>();

        private List<Interaction> _interactions = new List<Interaction>();

        public PhysicalSystem()
        {
            particles = new List<Particle>();
            _interactions = new List<Interaction>();
        }
        public PhysicalSystem(List<Particle> particles)
        {
            this.particles = particles;
            _interactions = new List<Interaction>();
        }
        public PhysicalSystem(List<Particle> particles, List<Interaction> interactions)
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
        /// Update the positions and momenta of all particles in the system using Runge-Kutta method... 
        /// Does not seem to be working correctly. Leave private until debugged.
        /// </summary>
        /// <param name="timeInterval">The amount of time passing during this iteration</param>
        private void RK4Iterate(Time timeInterval)
        {
            List<Task> tasks = new List<Task>();
            // store temporary particle data for Runge-Kutta method
            Particle[] virtualParticles = new Particle[particles.Count];
            Momentum[,] momenta = new Momentum[particles.Count, 4];
            Displacement[,] displacements = new Displacement[particles.Count, 4];

            // initialize virtualParticles
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(InitializeParticle(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // first iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 0));
            }
            Task.WaitAll(tasks.ToArray());

            // update before 2nd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(UpdateParticleBefore2ndOr3rdIteration(particleIndex, 0));
            }
            Task.WaitAll(tasks.ToArray());

            // 2nd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 1));
            }
            Task.WaitAll(tasks.ToArray());

            // update before 3rd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(UpdateParticleBefore2ndOr3rdIteration(particleIndex, 1));
            }
            Task.WaitAll(tasks.ToArray());

            // 3rd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 2));
            }
            Task.WaitAll(tasks.ToArray());

            // update before 4th iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(UpdateParticleBefore4thIteration(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // 4th iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 3));
            }
            Task.WaitAll(tasks.ToArray());

            // update actual particles using the Runge-Kutta coefficients
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(UpdateParticleUsingRungeKuttaCoefficients(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            async Task InitializeParticle(int particleIndex)
            {
                virtualParticles[particleIndex] = new Particle(particles[particleIndex]);
                for (int iterationNumber = 0; iterationNumber < 4; iterationNumber++)
                    momenta[particleIndex, iterationNumber] = new Momentum();
            }

            async Task CalculateRungeKuttaCoefficients(int particleIndex, int iterationNumber)
            {
                displacements[particleIndex, iterationNumber] =
                    timeInterval * particles[particleIndex].Velocity();
                foreach (Interaction interaction in virtualParticles[particleIndex].interactions)
                    momenta[particleIndex, iterationNumber] +=
                        timeInterval * interaction.InteractionForce(virtualParticles[particleIndex], interaction.B);
            }

            async Task UpdateParticleBefore2ndOr3rdIteration(int particleIndex, int iterationJustFinished)
            {
                virtualParticles[particleIndex] = new Particle(particles[particleIndex]);
                virtualParticles[particleIndex].momentum +=
                    momenta[particleIndex, iterationJustFinished];// / 2.0;
                virtualParticles[particleIndex].position +=
                    displacements[particleIndex, iterationJustFinished];// / 2.0;
            }

            async Task UpdateParticleBefore4thIteration(int particleIndex)
            {
                virtualParticles[particleIndex] = new Particle(particles[particleIndex]);
                virtualParticles[particleIndex].momentum +=
                    momenta[particleIndex, 2];
                virtualParticles[particleIndex].position +=
                    displacements[particleIndex, 2];
            }

            async Task UpdateParticleUsingRungeKuttaCoefficients(int particleIndex)
            {
                //particles[particleIndex].position += (displacements[particleIndex, 0] +
                //    2.0 * displacements[particleIndex, 1] + 2.0 * displacements[particleIndex, 2] +
                //    displacements[particleIndex, 3]) / 6.0;
                //particles[particleIndex].momentum += (momenta[particleIndex, 0] +
                //    2.0 * momenta[particleIndex, 1] + 2.0 * momenta[particleIndex, 2] +
                //    momenta[particleIndex, 3]) / 6.0;
                particles[particleIndex].position += (displacements[particleIndex, 0] +
                    displacements[particleIndex, 1]) / 2.0;
                particles[particleIndex].momentum += (momenta[particleIndex, 0] +
                    momenta[particleIndex, 1]) / 2.0;
            }
        }

        /// <summary>
        /// Update the positions and momenta of all particles in the system 
        /// using the 2nd-order Runge-Kutta method. 
        /// Doesn't perform as well as NotQuiteRK2(). Am I interpretting the Runge-Kutta method incorrectly?
        /// </summary>
        /// <param name="timeInterval">The amount of time passing during this iteration</param>
        private void RK2Iterate(Time timeInterval)
        {
            List<Task> tasks = new List<Task>();
            // store temporary particle data for Runge-Kutta method
            Displacement[] initialPositions = new Displacement[particles.Count];
            Momentum[] initialMomenta = new Momentum[particles.Count];
            Momentum[,] momenta = new Momentum[particles.Count, 2];
            Displacement[,] displacements = new Displacement[particles.Count, 2];

            // store initial condition
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(StoreInitialConditions(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // first iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(FirstIteration(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // 2nd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(SecondIteration(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            async Task StoreInitialConditions(int particleIndex)
            {
                initialPositions[particleIndex] = particles[particleIndex].position;
                initialMomenta[particleIndex] = particles[particleIndex].momentum;
            }

            async Task FirstIteration(int particleIndex)
            {
                Force netForce = new Force();
                foreach (Interaction interaction in particles[particleIndex].interactions)
                    netForce += interaction.InteractionForce();
                // The first iteration in RK2 only moves a half step.
                Momentum changeInMomentum = timeInterval * netForce / 2.0;
                particles[particleIndex].position += timeInterval *
                    particles[particleIndex].Velocity(
                    particles[particleIndex].momentum + changeInMomentum / 2.0) / 2.0;
                particles[particleIndex].momentum += changeInMomentum;
            }

            async Task SecondIteration(int particleIndex)
            {
                Force netForce = new Force();
                foreach (Interaction interaction in particles[particleIndex].interactions)
                    netForce += interaction.InteractionForce();
                Momentum changeInMomentum = timeInterval * netForce;
                particles[particleIndex].position = initialPositions[particleIndex] + timeInterval *
                    particles[particleIndex].Velocity(
                    particles[particleIndex].momentum + changeInMomentum / 2.0);
                particles[particleIndex].momentum = initialMomenta[particleIndex] + changeInMomentum;
            }
        }

        /// <summary>
        /// A modified version of the 2nd-order Runge-Kutta method
        /// </summary>
        /// <param name="timeStep"></param>
        private void NotQuiteRK2(Time timeStep)
        {
            List<Task> tasks = new List<Task>();
            // store temporary particle data for Runge-Kutta method
            Displacement[] initialPositions = new Displacement[particles.Count];
            Momentum[] initialMomenta = new Momentum[particles.Count];
            Momentum[,] momenta = new Momentum[particles.Count, 2];
            Displacement[,] displacements = new Displacement[particles.Count, 2];

            // store initial condition
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(StoreInitialConditions(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // first iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 0));
            }
            Task.WaitAll(tasks.ToArray());

            // update particles based on linear estimate
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(LinearUpdate(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            // 2nd iteration
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(CalculateRungeKuttaCoefficients(particleIndex, 1));
            }
            Task.WaitAll(tasks.ToArray());

            // update actual particles using the Runge-Kutta coefficients
            for (int particleIndex = 0; particleIndex < particles.Count; particleIndex++)
            {
                tasks.Add(UpdateParticles(particleIndex));
            }
            Task.WaitAll(tasks.ToArray());

            async Task StoreInitialConditions(int particleIndex)
            {
                initialPositions[particleIndex] = particles[particleIndex].position;
                initialMomenta[particleIndex] = particles[particleIndex].momentum;
            }

            async Task CalculateRungeKuttaCoefficients(int particleIndex, int iterationNumber)
            {
                displacements[particleIndex, iterationNumber] =
                    timeStep * particles[particleIndex].Velocity();
                momenta[particleIndex, iterationNumber] = new Momentum();
                foreach (Interaction interaction in particles[particleIndex].interactions)
                    momenta[particleIndex, iterationNumber] +=
                        timeStep * interaction.InteractionForce();
            }

            async Task LinearUpdate(int particleIndex)
            {
                particles[particleIndex].position += timeStep * particles[particleIndex].Velocity(
                    particles[particleIndex].momentum + momenta[particleIndex, 0] / 2.0);
                particles[particleIndex].momentum += momenta[particleIndex, 0];
            }

            async Task UpdateParticles(int particleIndex)
            {
                particles[particleIndex].position = initialPositions[particleIndex] +
                    (displacements[particleIndex, 0] + displacements[particleIndex, 1]) / 2.0;
                particles[particleIndex].momentum = initialMomenta[particleIndex] +
                    (momenta[particleIndex, 0] + momenta[particleIndex, 1]) / 2.0;
            }
        }

        /// <summary>
        /// Update the positions and momenta of all particles in the system 
        /// using a modified version of the Runge-Kutta method
        /// </summary>
        /// <param name="timeStep"></param>
        public void Iterate(Time timeStep)
        {
            NotQuiteRK2(timeStep); 
            // This seems to outperform the real Runge-Kutta method for the relationship between position and momentum
        }
    }
}
