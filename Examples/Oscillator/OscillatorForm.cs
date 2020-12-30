using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Physics;

namespace Oscillator
{
    public partial class OscillatorForm : Form
    {
        public OscillatorForm()
        {
            InitializeComponent();
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            Particle Earth = new Particle(new Mass(5.97237E24));
            Particle particle = new Particle(new Mass(Convert.ToDouble(massBox.Text)));
            Spring spring = new Spring(particle, Earth, Convert.ToDouble(springRateBox.Text));
            Damper damper = new Damper(spring, Convert.ToDouble(dampingRatioBox.Text));
            Time lengthOfSimulation = new Time(Convert.ToDouble(timeBox.Text));

            particle.interactions.Add(spring);
            particle.interactions.Add(damper);
            particle.position.values[0] = 1.0;

            PhysicalSystem system = new PhysicalSystem(new List<Particle>() { particle, Earth });

            int numberOfPoints = 1024;
            Time timeStep = lengthOfSimulation / numberOfPoints;
            chart1.ChartAreas["ChartArea"].AxisX.Maximum = lengthOfSimulation.value;
            for (Time time = new Time(); time <= lengthOfSimulation; time += timeStep)
            {
                chart1.Series["Series"].Points.AddXY(time.value, particle.position.values[0]);
                system.Iterate(timeStep); 
            }
            this.Refresh();
        }
    }
}
