using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            int temp;
            int N = int.TryParse(N_, out temp) ? int.Parse(N_) : 24;
            int A = int.TryParse(A_, out temp) ? int.Parse(A_) : 10;
            int B = int.TryParse(B_, out temp) ? int.Parse(B_) : 15;
            textBox2.Text = N.ToString();
            textBox4.Text = A.ToString();
            textBox6.Text = B.ToString();
            Worker worker = new Worker(N, A, B);
            // Розыгрыш величины
            List<int>gaussDistribution =  worker.gaussDistribution();
            textBox7.Text = string.Join(Environment.NewLine, gaussDistribution);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            pictureBox1.Image = Properties.Resources.var22;

            int x = pictureBox1.Image.Size.Width;
            int y = pictureBox1.Image.Size.Height;
            pictureBox1.Size = new System.Drawing.Size(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
