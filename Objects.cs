
using System;
using System.Linq;

namespace Physics
{

    public class Particle
    {
        public Mass mass = new Mass(Double.NaN);
        public Position position = new Position(0.0, 0.0, 0.0);
        public Momentum momentum = new Momentum(0.0, 0.0, 0.0);

        public Particle (Mass mass)
        {
            this.mass = mass;
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
}
