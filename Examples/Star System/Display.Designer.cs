namespace Star_System
{
    partial class Display
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.year_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Black;
            this.chart.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Black;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MaximumAutoSize = 0F;
            chartArea1.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Black;
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(-45, -46);
            this.chart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Color = System.Drawing.Color.White;
            series1.MarkerSize = 1;
            series1.Name = "Series";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(841, 841);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            this.chart.Click += new System.EventHandler(this.Chart_Click);
            // 
            // year_label
            // 
            this.year_label.AutoSize = true;
            this.year_label.ForeColor = System.Drawing.Color.White;
            this.year_label.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.year_label.Location = new System.Drawing.Point(12, 9);
            this.year_label.Name = "year_label";
            this.year_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.year_label.Size = new System.Drawing.Size(96, 20);
            this.year_label.TabIndex = 1;
            this.year_label.Text = "Click to start";
            this.year_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1172, 1050);
            this.Controls.Add(this.year_label);
            this.Controls.Add(this.chart);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Display";
            this.Text = "Star System";
            this.Disposed += new System.EventHandler(this.Display_Disposed);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Label year_label;
    }
}

