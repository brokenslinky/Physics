using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;

namespace Oscillator
{
    class Oscillator
    {
        Particle particle = new Particle();
        Particle Earth = new Particle();
        Spring spring;

        List<List<double>> DisplacementVsTime(Spring spring, Mass mass, Damper damper)
        {
            List<List<double>> displacementVsTime = new List<List<double>>();

            return displacementVsTime;
        }

    }
}
