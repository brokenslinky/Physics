namespace Oscillator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.springRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.massBox = new System.Windows.Forms.TextBox();
            this.solveButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dampingRatioBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea2.AxisX.Title = "Time (seconds)";
            chartArea2.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
            chartArea2.AxisY.Title = "Displacement (relative)";
            chartArea2.Name = "ChartArea";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Location = new System.Drawing.Point(13, 13);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerSize = 1;
            series2.Name = "Series";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(775, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart";
            // 
            // springRateBox
            // 
            this.springRateBox.Location = new System.Drawing.Point(13, 345);
            this.springRateBox.Name = "springRateBox";
            this.springRateBox.Size = new System.Drawing.Size(100, 20);
            this.springRateBox.TabIndex = 1;
            this.springRateBox.Text = "49033";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Spring Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 388);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mass";
            // 
            // massBox
            // 
            this.massBox.Location = new System.Drawing.Point(13, 407);
            this.massBox.Name = "massBox";
            this.massBox.Size = new System.Drawing.Size(100, 20);
            this.massBox.TabIndex = 3;
            this.massBox.Text = "272.2";
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point(713, 415);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(75, 23);
            this.solveButton.TabIndex = 5;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = true;
            this.solveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Damping Ratio";
            // 
            // dampingRatioBox
            // 
            this.dampingRatioBox.Location = new System.Drawing.Point(203, 345);
            this.dampingRatioBox.Name = "dampingRatioBox";
            this.dampingRatioBox.Size = new System.Drawing.Size(100, 20);
            this.dampingRatioBox.TabIndex = 6;
            this.dampingRatioBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 388);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Simulation Time";
            // 
            // timeBox
            // 
            this.timeBox.Location = new System.Drawing.Point(203, 407);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(100, 20);
            this.timeBox.TabIndex = 8;
            this.timeBox.Text = "10";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dampingRatioBox);
            this.Controls.Add(this.solveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.massBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.springRateBox);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Simple Harmonic Oscillator";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox springRateBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox massBox;
        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dampingRatioBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox timeBox;
    }
}