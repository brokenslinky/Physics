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

        public PhysicalSystem system = new PhysicalSystem();
        public List<CelestialBody> Bodies = new List<CelestialBody>();

        internal static List<Force> netForces = new List<Force>();

        bool end = false;

        public async Task ImplicitAsyncTest(int numberOfAsteroids = 0)
        {
            system = new Physics.PhysicalSystem(new List<Particle>(GetCelestialBodies(numberOfAsteroids)));

            while(!end)
            {
                try { system.Iterate(timeStep); }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
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
