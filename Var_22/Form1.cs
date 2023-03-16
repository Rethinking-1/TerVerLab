using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Var_22
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Инициализация объекта
            string N_ = textBox2.Text;
            string A_ = textBox4.Text;
            string B_ = textBox6.Text;
            string size_ = textBox18.Text;
            float temp;
            int temp_;
            int N = int.TryParse(N_, out temp_) ? int.Parse(N_) : 24;
            float A = float.TryParse(A_, out temp) ? float.Parse(A_) : 10;
            float B = float.TryParse(B_, out temp) ? float.Parse(B_) : 15;
            int size = int.TryParse(size_, out temp_) ? int.Parse(size_) : 30;
            textBox2.Text = N.ToString();
            textBox4.Text = A.ToString();
            textBox6.Text = B.ToString();
            textBox18.Text = size.ToString();
            Worker worker = new Worker(N, A, B, size);
            // Розыгрыш случайной величины
            List<float> gaussDistribution =  worker.gaussDistribution();
            textBox7.Text = string.Join(Environment.NewLine, gaussDistribution);

            // Получение статистических характеристик случайной величины
            Dictionary<string, float> statCharacter = worker.statisticalCharacteristics();
            textBox9.Text = statCharacter["Мат_ожидание"].ToString();
            textBox10.Text = statCharacter["Выб_мат_ожидание"].ToString();
            textBox11.Text = statCharacter["Абс_мат_ожидание"].ToString();
            textBox12.Text = statCharacter["Дисперсия"].ToString();
            textBox13.Text = statCharacter["Выб_дисперсия"].ToString();
            textBox14.Text = statCharacter["Абс_дисперсия"].ToString();
            textBox15.Text = statCharacter["Выб_медиана"].ToString();
            textBox16.Text = statCharacter["Размах_выборки"].ToString();

            // Графики функций распределения
            // Выборочная функция
            chart1.Series.Clear();
            chart1.Titles[0].Text = $"Выборочная функция распределения N = {N}";
            chart1.Titles[0].Visible = true;

            Series series1 = new Series("Выборочная функция распределения");
            series1.ChartType = SeriesChartType.Line;

            //------------
            series1.BorderWidth = 5;
            //-------------

            chart1.Series.Add(series1);

            chart1.ChartAreas[0].AxisX.Title = "η - общая сумма вклада";
            chart1.ChartAreas[0].AxisY.Title = "F'η";
            chart1.ChartAreas[0].AxisX.Minimum = gaussDistribution.First() - 1;
            chart1.ChartAreas[0].AxisX.Maximum = gaussDistribution.Last() + 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.Series[0].Points.AddXY(gaussDistribution[0] - 3 * Math.Sqrt(statCharacter["Дисперсия"]), (float)0);
            chart1.Series[0].Points.AddXY(gaussDistribution[0], (float)0);
           
            for (int i = 0; i < size - 1; i++)
            {
                chart1.Series[0].Points.AddXY(gaussDistribution[i], ((float)(i + 1) / (float)size));
                chart1.Series[0].Points.AddXY(gaussDistribution[i + 1], ((float)(i + 1) / (float)size));
            }
            chart1.Series[0].Points.AddXY(gaussDistribution[size - 1], ((float)((size)/ (float)size)));
            chart1.Series[0].Points.AddXY(gaussDistribution[size - 1] + 3 * Math.Sqrt(statCharacter["Дисперсия"]), ((float)((size) / (float)size)));
            for (int i = 0; i < chart1.Series[0].Points.Count; i++)
                chart1.Series[0].Points[i].Color = Color.Green;
            // Теоретическая функция
            chart2.Series.Clear();
            chart2.Titles[0].Text = $"Теоретическая функция распределения\n μ = {A * N}, σ = {Math.Sqrt(B * N)}";
            chart2.Titles[0].Visible = true;
            Series series2 = new Series("Теоретическая функция распределения");
            series2.ChartType = SeriesChartType.Point;
            chart2.Series.Add(series2);            
            chart2.ChartAreas[0].AxisX.Title = "η";
            chart2.ChartAreas[0].AxisY.Title = "Fη";
            // -><-
            int n = 50000;
            float l = (A * N - 3 * (float)Math.Sqrt(B * N));
            float r = (A * N + 3 * (float)Math.Sqrt(B * N));
            chart2.ChartAreas[0].AxisX.Minimum = l;
            chart2.ChartAreas[0].AxisX.Maximum = r;
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 1;
            List<float> crutchX = new List<float>();
            List<float> crutchY = new List<float>();
            for (int i = 0; i < n; i++)
            {
                float x = l + i * (r - l) / n;
                crutchX.Add(x);
                crutchY.Add((worker.Erf((x - A * N) / (Math.Sqrt(B * N))) + 1) / 2);
            }
            for (int i = 0; i < crutchY.Count; i++)
                chart2.Series[0].Points.AddXY(crutchX[i], crutchY[i]);
            // -><-

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart2.Legends[0].Enabled = false;
            //this.TopMost = true;
            // this.FormBorderStyle = FormBorderStyle.None;
            //  this.WindowState = FormWindowState.Maximized;


            pictureBox1.Image = Properties.Resources.var22;
            int x = pictureBox1.Image.Size.Width;
            int y = pictureBox1.Image.Size.Height;
            pictureBox1.Size = new System.Drawing.Size(x, y);

            pictureBox2.Image = Properties.Resources._1;
            x = pictureBox2.Image.Size.Width;
            y = pictureBox2.Image.Size.Height;
            pictureBox2.Size = new System.Drawing.Size(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
