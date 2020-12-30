using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Physics;
using System.Windows.Forms;

namespace Star_System
{
    class Simulation
    {
        internal static readonly Time day = new Time(86400.0);
        internal static Time timeStep = day / 1.0;

        public Time simulatedTime = new Time(0.0);
        public Physics.System system = new Physics.System();
        public List<CelestialBody> Bodies = new List<CelestialBody>();

        internal static List<Force> netForces = new List<Force>();

        bool end = false;

        public async Task CalculateForce(int bodyIndex, int maxGravityContributors = 9)
        {
            try
            {
                // calculate force
                Force netForce = new Force();
                for (int j = 0; j < maxGravityContributors; j++)
                {
                    if (bodyIndex != j)
                        netForce += Gravity.InteractionForce(Bodies[bodyIndex], Bodies[j]);
                }
                netForces[bodyIndex] = netForce;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public async Task CalculateForces(int bodyIndexStart, int bodyIndexEnd, int maxGravityContributors = 9)
        {
            try
            {
                for (int bodyIndex = bodyIndexStart; bodyIndex <= bodyIndexEnd; bodyIndex++)
                {
                    // calculate force
                    Force netForce = new Force();
                    for (int j = 0; j < maxGravityContributors; j++)
                    {
                        if (bodyIndex != j)
                            netForce += Gravity.InteractionForce(Bodies[bodyIndex], Bodies[j]);
                    }
                    netForces[bodyIndex] = netForce;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public async Task MoveBody(int bodyIndex)
        {
            try
            {
                Bodies[bodyIndex].Iterate(timeStep, netForces[bodyIndex]);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public async Task MoveBodies(int bodyIndexStart, int bodyIndexEnd)
        {
            try
            {
                for (int bodyIndex = bodyIndexStart; bodyIndex <= bodyIndexEnd; bodyIndex++)
                    Bodies[bodyIndex].Iterate(timeStep, netForces[bodyIndex]);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public async Task ImplicitAsyncTest(int numberOfAsteroids = 0)
        {
            system = new Physics.System(new List<Particle>(GetCelestialBodies(numberOfAsteroids)));

            while(!end)
            {
                try { 
                    system.Iterate(timeStep);
                    simulatedTime.value += timeStep.value;
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        public async Task Orbital_Simulation_Async(int numberOfAsteroids = 100, int maxGravityContributors = 9)
        {
            Bodies = GetCelestialBodies(numberOfAsteroids);
            if (maxGravityContributors > Bodies.Count)
                maxGravityContributors = Bodies.Count;

            netForces = new List<Force>(new Force[Bodies.Count]);

            while (!end)
            {
                try
                {
                    List<Task> tasks = new List<Task>();

                    // Calculate all forces before moving anything
                    for (int bodyIndex = 0; bodyIndex < Bodies.Count; bodyIndex ++)
                    {
                        int i = bodyIndex; // apparently I need a value that goes out of scope...
                        // start a new thread to calculate forces on these bodies
                        tasks.Add(CalculateForce(i, maxGravityContributors));
                    }

                    Task.WaitAll(tasks.ToArray());

                    // Move every body at once
                    for (int bodyIndex = 0; bodyIndex < Bodies.Count; bodyIndex ++)
                    {
                        int i = bodyIndex; // again, I need a value that goes out of scope before the next thread starts
                        // start a new thread to move these bodies
                        tasks.Add(MoveBody(i));
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            MessageBox.Show("Simulation has ended");
        }

        public async Task Orbital_Simulation(int numberOfAsteroids = 100, int maxGravityContributors = 9)
        {
            Bodies = GetCelestialBodies(numberOfAsteroids);
            if (maxGravityContributors > Bodies.Count)
                maxGravityContributors = Bodies.Count;


            while (!end)
            {
                try
                {
                    netForces.Clear();

                    // calculate the force acting on each body
                    for (int i = 0; i < Bodies.Count; i++)
                    {
                        Force netForce = new Force();
                        for (int j = 0; j < maxGravityContributors; j++)
                        {
                            if (i == j)
                                continue;
                            netForce += Gravity.InteractionForce(Bodies[i], Bodies[j]);
                        }
                        netForces.Add(netForce);
                    }

                    // now positions can update based on net forces
                    for (int i = 0; i < Bodies.Count; i++)
                    {
                        Bodies[i].Iterate(timeStep, netForces[i]);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        
        public void AssignInteractions(int maxGravityContributors = int.MaxValue)
        {
            if (maxGravityContributors > Bodies.Count)
                maxGravityContributors = Bodies.Count;
            foreach (CelestialBody body in Bodies)
            {
                for (int i = 0; i < maxGravityContributors; i++)
                {
                    if (Bodies[i] != body)
                    {
                        body.interactions.Add(new Gravity(body, Bodies[i]));
                    }
                }
            }
        }

        public void End()
        {
            end = true;
        }

        public static List<CelestialBody> GetCelestialBodies(int numberOfAsteroids = 0)
        {
            List<CelestialBody> bodies = new List<CelestialBody>();
            Random RNG = new Random();
            double seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            double mass, periapsis, apoapsis;

            // Sun
            mass = 1.9891E30;
            periapsis = 0.0;
            apoapsis = 0.0;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Mercury
            mass = 3.285E23;
            periapsis = 46001200000.0;
            apoapsis = 69816900000.0;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Venus
            mass = 4.8675E24;
            periapsis = 107477000000;
            apoapsis = 108939000000;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Earth
            mass = 5.97237E24;
            periapsis = 147095000000;
            apoapsis = 152100000000;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Mars
            mass = 6.4171E23;
            periapsis = 206700000000;
            apoapsis = 249200000000;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Jupiter
            mass = 1.8982E27;
            periapsis = 740.52E9;
            apoapsis = 816.62E9;
            seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            //// Saturn
            //mass = 5.6834E26;
            //periapsis = 1.35255E12;
            //apoapsis = 1.5145E12;
            //seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            //bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            //// Uranus
            //mass = 8.681E25;
            //periapsis = 2.742E12;
            //apoapsis = 3.008E12;
            //seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            //bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            //// Neptune
            //mass = 1.02413E26;
            //periapsis = 4.46E12;
            //apoapsis = 4.54E12;
            //seedAngle = 2.0 * Math.PI * RNG.NextDouble();
            //bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));

            // Asteroids
            while (numberOfAsteroids-- > 0)
            {
                mass = 1E-20; // Asteroid mass can be considered negligible for now
                periapsis = 4.04E11 + 4.49E10 * Math.Sqrt(-2.0 * Math.Log(RNG.NextDouble())) *
                    Math.Sin(2.0 * Math.PI * RNG.NextDouble());
                apoapsis = periapsis;
                seedAngle = 2.0 * Math.PI * RNG.NextDouble();
                bodies.Add(new CelestialBody(mass, periapsis, apoapsis, seedAngle));
            }

            for (int i = 0; i < bodies.Count; i++)
            {
                for (int j = 0; j < bodies.Count; j++)
                {
                    if (i != j)
                    {
                        if ((Gravity.InteractionForce(bodies[i], bodies[j]).Magnitude() / bodies[i].mass).value > 1E-12)
                            bodies[i].interactions.Add(new Gravity(bodies[i], bodies[j]));
                    }
                }
            }

            return bodies;
        }
    }
}
