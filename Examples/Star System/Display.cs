using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Physics;
using System.Threading;

namespace Star_System
{
    public partial class Display : Form
    {
        Simulation simulation = new Simulation();

        public Display()
        {
            InitializeComponent();
            Task.Run(() => simulation.ImplicitAsyncTest(1000));
            while (simulation.system.particles.Count == 0)
                Application.DoEvents();
        }

        public void MaintainDisplay(int delayTime = 10)
        {
            // size plot appropriately
            double maxDistance = 0.0;
            foreach (Particle body in simulation.system.particles)
            {
                double tmp = body.position.Magnitude().value;
                if (tmp > maxDistance)
                    maxDistance = tmp;
            }
            maxDistance = ((Int64)maxDistance / 1000000) * 1100000.0;
            chart.ChartAreas["ChartArea"].AxisX.Minimum = -maxDistance;
            chart.ChartAreas["ChartArea"].AxisX.Maximum = maxDistance;
            chart.ChartAreas["ChartArea"].AxisY.Minimum = -maxDistance;
            chart.ChartAreas["ChartArea"].AxisY.Maximum = maxDistance;
            chart.ChartAreas["ChartArea"].AxisX.Interval = maxDistance;
            chart.ChartAreas["ChartArea"].AxisY.Interval = maxDistance;

            while (true)
            {
                try
                {
                    UpdatePlot(0);
                    DateTime time = DateTime.Now;
                    while (DateTime.Now < time.AddMilliseconds(delayTime))
                        Application.DoEvents();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        public void UpdatePlot(int centricIndex = 0)
        {

            chart.Series["Series"].Points.Clear();
            foreach (Particle body in simulation.system.particles)
            {
                chart.Series["Series"].Points.AddXY(
                    body.position.values[0] - simulation.system.particles[centricIndex].position.values[0],
                    body.position.values[1] - simulation.system.particles[centricIndex].position.values[1]);
            }
            Application.DoEvents();
        }

        private void Chart_Click(object sender, EventArgs e)
        {
            MaintainDisplay();
        }

        private void Display_Disposed(object sender, EventArgs e)
        {
            Environment.Exit(0);
            Application.Exit();
        }
    }
}
