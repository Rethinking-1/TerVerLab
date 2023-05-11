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
            chart1.Series.Clear();
            // Создание
            // series1.ChartType = SeriesChartType.Point;

            Series series1 = new Series("Выборочная функция распределения");
            series1.ChartType = SeriesChartType.Line;
            chart1.Series.Add(series1);

            chart1.Titles[0].Text = $"Выборочная функция распределения N = {N}";
            chart1.Titles[0].Visible = true;
            chart1.ChartAreas[0].AxisX.Title = "η - общая сумма вклада";
            chart1.ChartAreas[0].AxisY.Title = "Fη";
            series1.BorderWidth = 5;

            Series series2 = new Series("Теоретическая функция распределения");
            series2.ChartType = SeriesChartType.Point;
            chart1.Series.Add(series2);
            chart1.Titles[1].Text = $"Теоретическая функция распределения\n μ = {A * N}, σ = {Math.Sqrt(B * N)}";
            chart1.Titles[1].Visible = true;
            // Границы
            float l = (A * N - 3 * (float)Math.Sqrt(B * N));
            float r = (A * N + 3 * (float)Math.Sqrt(B * N));
            chart1.ChartAreas[0].AxisX.Minimum = l - 3 * Math.Sqrt(statCharacter["Дисперсия"]);
            chart1.ChartAreas[0].AxisX.Maximum = r + 3 * Math.Sqrt(statCharacter["Дисперсия"]);
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            // Выборочная функция
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
            int n = 50000;
            List<double> theoreticalFunctionX = new List<double>();
            List<double> theoreticalFunctionY = new List<double>();
            float h = (r - l + 1) / n;
            for (int i = 0; i < n; i++)
            {
                double x = l + (double)i * h;
            //    float x_ = (x - A * N) / Math.Sqrt(B * N);
                theoreticalFunctionX.Add(x);
                theoreticalFunctionY.Add(worker.Erf(  (x - A * (double)N) / Math.Sqrt(B * (double)N))     / 2);
            }
            theoreticalFunctionY.Sort();
            for (int i = 0; i < theoreticalFunctionY.Count; i++)
                chart1.Series[1].Points.AddXY(theoreticalFunctionX[i], theoreticalFunctionY[i]);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
