using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5lab
{
    public partial class Form1 : Form
    {
        public class Point3D
        {
            private double x;
            private double y;
            private double z;
            public Point3D(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public double X
            {
                get
                {
                    return x;
                }
                set
                {
                    this.x = value;
                }
            }
            public double Y
            {
                get
                {
                    return y;
                }
                set
                {
                    this.y = value;
                }
            }
            public double Z
            {
                get
                {
                    return z;
                }
                set
                {
                    this.z = value;
                }
            }
        }

        Bitmap bmp;
        Graphics g;
        Pen p;
        int d = 91;
        int t = 96;
        int w = 0;
        int h = 0;
        int a = 100;
        int b = 30;        
        double border1 = -Math.PI;
        double border2 =  Math.PI;
        public Point3D[,] tmp;
        public Point3D[,] mas;
        public Point3D[,] mas_mirror;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "0,5";
            textBox2.Text = "1";
            textBox3.Text = "25";
            textBox4.Text = "100";
            textBox5.Text = "100";
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            w = pictureBox1.Width / 2;
            h = pictureBox1.Height / 2;
            mas = new Point3D[d, t];
            mas_mirror = new Point3D[d, t];
            tmp = new Point3D[d, t];
            g = Graphics.FromImage(bmp);
            p = new Pen(Color.Orange);
            coordinate(0.5,1);
            draw(100,100);
            timer1.Enabled = true;
        }

        public void coordinate(double m, double n) // Вычисление мировых координат
        {
            double x, y, z;
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    double ax = Math.Sign(Math.Round(Math.Cos(border1 + i * 2 * Math.PI / 180), 4)) * Math.Pow(Math.Abs(Math.Round(Math.Cos(border1 + i * 2 * Math.PI / 180), 4)), m);
                    double b = Math.Sign(Math.Round(Math.Cos(border2 + j * 4 * Math.PI / 180), 4)) * Math.Pow(Math.Abs(Math.Round(Math.Cos(border2 + j * 4 * Math.PI / 180), 4)), n);
                    double az = Math.Sign(Math.Round(Math.Sin(border2 + j * 4 * Math.PI / 180), 4)) * Math.Pow(Math.Abs(Math.Round(Math.Sin(border2 + j * 4 * Math.PI / 180), 4)), n);
                    double ay = Math.Sign(Math.Round(Math.Sin(border1 + i * 2 * Math.PI / 180), 4)) * Math.Pow(Math.Abs(Math.Round(Math.Sin(border1 + i * 2 * Math.PI / 180), 4)), m);
                    x = ax * (2 + b);
                    y = ay * (2 + b);
                    z = az;
                    mas[i, j] = new Point3D(x, y, z);                                                                                                                                                                                                                             mas_mirror[i, j] = new Point3D(-x, -y, z);
                }
            }
        }
        public Point3D Project(Point3D points) //аксононометрическая проекция
        {
            double x = points.X * Math.Cos(a * Math.PI / 180) + points.Y * Math.Sin(a * Math.PI / 180);
            double y = -points.X * Math.Sin(a * Math.PI / 180) * Math.Cos(b * Math.PI / 180) + points.Y * Math.Cos(a * Math.PI / 180) * Math.Cos(b * Math.PI / 180) + points.Z * Math.Sin(b * Math.PI / 180);
            double z = points.X * Math.Sin(a * Math.PI / 180) * Math.Sin(b * Math.PI / 180) - points.Y * Math.Cos(a * Math.PI / 180) * Math.Sin(b * Math.PI / 180) + points.Z * Math.Cos(b * Math.PI / 180);
            return new Point3D(x, y, z);
        }
        //Повороты 

        public Point3D TurnOX(double a, Point3D p)
        {
            double x = p.X;
            double y = p.Y * Math.Cos(a * Math.PI / 180) - p.Z * Math.Sin(a * Math.PI / 180);
            double z = p.Y * Math.Sin(a * Math.PI / 180) + p.Z * Math.Cos(a * Math.PI / 180);
            return new Point3D(x, y, z);
        }

        public Point3D TurnOY(double a, Point3D p)
        {
            double x = p.X * Math.Cos(a * Math.PI / 180) + p.Z * Math.Sin(a * Math.PI / 180);
            double y = p.Y;
            double z = -p.X * Math.Sin(a * Math.PI / 180) + p.Z * Math.Cos(a * Math.PI / 180);
            return new Point3D(x, y, z);
        }

        public Point3D TurnOZ(double a, Point3D p)
        {
            double x = p.X * Math.Cos(a * Math.PI / 180) - p.Y * Math.Sin(a * Math.PI / 180);
            double y = p.X * Math.Sin(a * Math.PI / 180) + p.Y * Math.Cos(a * Math.PI / 180);
            double z = p.Z;
            return new Point3D(x, y, z);
        }

        public Point3D Moving(Point3D p) // Перемещение
        {
            return new Point3D(p.X + w, p.Y + h, p.Z);
        }

        public Point3D Scaling(Point3D p, int kx, int ky) // Масштабирование
        {
            double x = p.X * kx;
            double y = p.Y * ky;
            double z = p.Z;
            return new Point3D(x, y, z);
        }
        public void draw(int kx, int ky) 
        {
            g.Clear(Color.Gray);
            Point[,] point = new Point[d, t];
            Brush brush = new SolidBrush(Color.Black);
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    tmp[i, j] = Project(mas[i, j]);
                    tmp[i, j] = Scaling(tmp[i, j], kx, ky);
                    tmp[i, j] = Moving(tmp[i, j]);
                }
            }
            for (int i = 0; i < 91; i++)
            {
                for (int j = 0; j < 91; j++)
                {
                    point[i, j].X = Convert.ToInt32(tmp[i, j].X);
                    point[i, j].Y = Convert.ToInt32(tmp[i, j].Y);
                }
            }
            for (int i = 0; i < d-1; i++)
            {
                for (int j = 0; j < d-1; j++)
                {
                    Point[] curvePoints = { point[i, j], point[i + 1, j], point[i + 1, j + 1], point[i, j + 1] };
                    g.DrawPolygon(p, curvePoints);
                }
            }
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    tmp[i, j] = Project(mas_mirror[i, j]);
                    tmp[i, j] = Scaling(tmp[i, j], kx, ky);
                    tmp[i, j] = Moving(tmp[i, j]);
                }
            }
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    point[i, j].X = Convert.ToInt32(tmp[i, j].X);
                    point[i, j].Y = Convert.ToInt32(tmp[i, j].Y);
                }
            }
            for (int i = 0; i < d-1; i++)
            {
                for (int j = 0; j < d-1; j++)
                {
                    ;
                    Point[] curvePoints = { point[i, j], point[i + 1, j], point[i + 1, j + 1], point[i, j + 1] };
                    g.DrawPolygon(p, curvePoints);
                }
            }
            pictureBox1.Refresh();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double.TryParse(textBox1.Text, out double m);
            double.TryParse(textBox2.Text, out double n);
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky);            
            coordinate(m, n);
            draw(kx,ky); 
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky);           
            if (e.Button == MouseButtons.Right)
            {
                a = (e.X - pictureBox1.Width / 2) / 3;
                b = -e.Y + pictureBox1.Height / 2;
                draw(kx,ky);
            }
            else if (e.Button == MouseButtons.Left)
            {
                w = e.X;
                h = e.Y;
                draw(kx, ky);
            }
        }

        private void BtnTurnOX_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky);
            double.TryParse(textBox3.Text, out double a);
            for (int i = 0; i < 91; i++)
                for (int j = 0; j < 91; j++)
                {
                    mas[i, j] = TurnOX(a, mas[i, j]);
                    mas_mirror[i, j] = TurnOX(a, mas_mirror[i, j]);
                }
            draw(kx, ky);
        }

        private void BtnTurnOY_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky); 
            double.TryParse(textBox3.Text, out double a);
            for (int i = 0; i < 91; i++)
                for (int j = 0; j < 91; j++)
                {
                    mas[i, j] = TurnOY(a, mas[i, j]);
                    mas_mirror[i, j] = TurnOY(a, mas_mirror[i, j]);
                }
            draw(kx, ky);
        }

        private void BtnTurnOZ_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky);          
            double.TryParse(textBox3.Text, out double a);
            for (int i = 0; i < 91; i++)
                for (int j = 0; j < 91; j++)
                {
                    mas[i, j] = TurnOZ(a, mas[i, j]);
                    mas_mirror[i, j] = TurnOZ(a, mas_mirror[i, j]);
                }
            draw(kx, ky);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int.TryParse(textBox4.Text, out int kx);
            int.TryParse(textBox5.Text, out int ky);
            int l = 3;
            for (int i = 0; i < 91; i++)
                for (int j = 0; j < 91; j++)
                {
                    mas[i, j] = TurnOZ(l, mas[i, j]);
                    mas_mirror[i, j] = TurnOZ(l, mas_mirror[i, j]);
                    mas[i, j] = TurnOY(l, mas[i, j]);
                    mas_mirror[i, j] = TurnOY(l, mas_mirror[i, j]);
                    mas[i, j] = TurnOX(l, mas[i, j]);
                    mas_mirror[i, j] = TurnOX(l, mas_mirror[i, j]);
                }

            draw(kx, ky);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
