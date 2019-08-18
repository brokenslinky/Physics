using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class Spring
    {
        public double springRate = double.MaxValue;
        public double restLength = 0.0;
        public double dampingRatio = 0.0;

        public Spring(double rate, double restLength = 0.0, double dampingRatio = 0.0)
        {
            springRate = rate; this.restLength = restLength; this.dampingRatio = dampingRatio;
        }

        public Force Force(Position x1, Position x2) // returns force on object1
        {
            Position distance = x2 - x1;
            double magnitude = springRate * (distance.Magnitude() - restLength);
            return new Force(magnitude * distance.Direction());
        }
    }
}