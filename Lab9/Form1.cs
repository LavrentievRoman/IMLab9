using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rnd = new Random();

        private void startButton_Click(object sender, EventArgs e)
        {
            errorLabel.Visible = false;
            chart1.Series[0].Points.Clear();

            double[] prob = new double[5] { (double)prob1Num.Value, (double)prob2Num.Value, (double)prob3Num.Value,
                                            (double)prob4Num.Value, (double)prob5Num.Value };
            double[] x = new double[5] { 1, 2, 3, 4, 5 };

            double sum = 0;
            for (int i = 0; i < 5; i++)
                sum += prob[i];

            if (sum != 1) errorLabel.Visible = true;

            int N = (int)nNum.Value;
            int[] statistics = new int[5] { 0, 0, 0, 0, 0 };

            for (int i = 0; i < N; i++)
            {
                double a = rnd.NextDouble();
                for (int j = 0; j < 5; j++)
                {
                    a -= prob[j];
                    if (a <= 0)
                    {
                        statistics[j]++;
                        break;
                    }
                }
            }

            double[] frequency = new double[5];

            for (int i = 0; i < 5; i++)
            {
                frequency[i] = (double)statistics[i] / N;
                chart1.Series[0].Points.Add(frequency[i]);
            }

            double variance = 0, average = 0;
            double fvariance = 0, faverage = 0;

            for (int i = 0; i < 5; i++)
            {
                average += prob[i] * x[i];
                faverage += frequency[i] * x[i];

                variance += prob[i] * x[i] * x[i];
                fvariance += frequency[i] * x[i] * x[i];
            }
            variance -= average * average;
            fvariance -= faverage * faverage;

            double averageError = Math.Abs(faverage - average) / Math.Abs(average);
            double varianceError = Math.Abs(fvariance - variance) / Math.Abs(variance);

            averageLabel.Text = Convert.ToString(average) + " (error = " + Convert.ToString(averageError) + "%)";
            varianceLabel.Text = Convert.ToString(variance) + " (error = " + Convert.ToString(varianceError) + "%)";

            double chi = 0, chiA = 9.488;
            for (int i = 0; i < 5; i++)
                chi += (statistics[i] * statistics[i]) / (N * prob[i]);
            chi -= N;

            chiLabel.Text = Convert.ToString(chi);
            if (chi > chiA)
                chiLabel.Text += " > " + Convert.ToString(chiA) + " is True";
            else chiLabel.Text += " <= " + Convert.ToString(chiA) + " is False";
        }
    }
}
