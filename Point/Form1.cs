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

namespace Point
{
    public partial class Form1 : Form
    {
        static int x0 = 45, y0 = 100;
        static int xk = 560, yk = 700;
        static double x1 = x0, y1 = y0;
        static int N = 5;
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 100;
        }
        static public int Fact(int n)
        {
            int rez = 1;
            for(int i = 1; i <= n; i++)
            {
                rez *= i;
            }
            return rez;
        }
        static public double Cos(double x, int n)
        {
            double rez = 0;
            for(int i = 1; i <= n; i++)
            {
                rez += Math.Pow(-1, (i - 1)) * Math.Pow(x, (2 * i - 2)) / Fact(2 * i - 2);
            }
            return rez;
        }
        static public double Sin(double x, int n)
        {
            double rez = 0;
            for (int i = 1; i <= n; i++)
            {
                rez += Math.Pow(-1, (i - 1)) * Math.Pow(x, (2 * i - 1)) / Fact(2 * i - 1);
            }
            return rez;
        }
        static public double Atan(double x, int n)
        {
            double rez = 0;
            for(int i = 1; i <= n; i++)
            {
                rez += Math.Pow(-1, (i-1)) * Math.Pow(x, (2 * i - 1)) / (2 * i - 1);
            }
            return rez;
        }
        int Find_n(int x0, int y0, int xk, int yk, double eps)
        {
            int num = 1;
            bool finish = false;
            double alfa;
            while ((num<50) && (finish == false))
            {
                int dx = Math.Abs(xk - x0);
                int dy = Math.Abs(yk - y0);
                double df = (double)dy / dx;
                if (df <= 1)
                {
                    alfa = Atan(df, num);
                }
                else
                {
                    df = (double)dx / dy;
                    alfa = Math.PI / 2 - Atan(df, num);
                }
                double dist = Math.Sqrt(Math.Pow(x0 - xk, 2) + Math.Pow(y0 - yk, 2));
                double dist_prev = Math.Sqrt(Math.Pow(x0 - xk, 2) + Math.Pow(y0 - yk, 2));
                double x = x0;
                double y = y0;
                while (true)
                {
                    if (dist <= dist_prev)
                    {
                        dist_prev = Math.Sqrt(Math.Pow((x - xk), 2) + Math.Pow((y - yk), 2));
                        x += 1 * Cos(alfa, num);
                        y += 1 * Sin(alfa, num);
                        dist = Math.Sqrt(Math.Pow((x - xk), 2) + Math.Pow((y - yk), 2));
                        if (dist <= eps)
                        {
                            finish = true;
                            break;
                        }
                    }
                    else
                    {
                        num++;
                        break;
                    }
                }
            }
            return num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            chart1.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("area 1");
            chart1.Series.Add("Dependence of calculation accuracy on the radius of the hit zone");
            chart1.Series["Dependence of calculation accuracy on the radius of the hit zone"].ChartType = SeriesChartType.Spline;
            chart1.Series["Dependence of calculation accuracy on the radius of the hit zone"].ChartArea = "area 1";
            chart1.Series["Dependence of calculation accuracy on the radius of the hit zone"].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.Title = "Radius of the hit zone";
            chart1.ChartAreas[0].AxisY.Title = "Calculation accuracy";
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            for(double i=1; i<=51; i = i + 5)
            {
                int num = Find_n(x0, y0, xk, yk, i);
                Chart(i, num);
            }
        }
        void Chart(double x, double y)
        {
            chart1.Series["Dependence of calculation accuracy on the radius of the hit zone"].Points.AddXY(x, y);
        }
        
        bool finish1 = false;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            double eps = 5;
            double alfa;
            Graphics g = e.Graphics;
            g.ScaleTransform(0.5f, 0.5f);
            Brush b = new SolidBrush(Color.BlueViolet);
            g.FillEllipse(b, x0 - 10, y0 - 10, 30, 30);
            g.FillEllipse(b, xk - 10, yk - 10, 30, 30);
            int dx = Math.Abs(xk - x0);
            int dy = Math.Abs(yk - y0);
            double df = (double)dy / dx;
            if (df <= 1)
            {
                alfa = Atan(df, N);
            }
            else
            {
                df = (double)dx / dy;
                alfa = Math.PI / 2 - Atan(df, N);
            }
            x1 += 5 * Cos(alfa, N);
            y1 += 5 * Sin(alfa, N);
            double dist = Math.Sqrt(Math.Pow(x1 - xk, 2) + Math.Pow(y1 - yk, 2));
            if (dist <= eps)
                finish1 = true;
            if (!finish1)
            { 
                g.FillEllipse(b, (int)x1 - 5, (int)y1 - 5, 15, 15); 
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Visible = false;
        }
    }
}
