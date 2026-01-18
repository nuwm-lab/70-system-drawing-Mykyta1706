using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LabWork7
{
    public class GraphForm : Form
    {
        public GraphForm()
        {
            this.Text = "Лабораторна робота №7 - Графік функції (Варіант 5)";
            this.Size = new Size(800, 600);
            this.DoubleBuffered = true; // Щоб не було мерехтіння
            this.Resize += (s, e) => this.Invalidate(); // Перемальовувати при зміні розміру
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Параметри функції за варіантом 5
            double xMin = 2.5, xMax = 9.0;

            // Розрахунок діапазону Y для масштабування
            double yMin = double.MaxValue, yMax = double.MinValue;
            List<PointF> points = new List<PointF>();

            for (double x = xMin; x <= xMax; x += 0.01) // Крок менший для плавності
            {
                double y = (1.5 * x - Math.Log(2 * x)) / (3 * x + 1);
                if (y < yMin) yMin = y;
                if (y > yMax) yMax = y;
                points.Add(new PointF((float)x, (float)y));
            }

            // Відступи
            int margin = 50;
            float w = this.ClientSize.Width - 2 * margin;
            float h = this.ClientSize.Height - 2 * margin;

            // Функції трансформації координат у пікселі
            float TransformX(float x) => margin + (x - (float)xMin) / ((float)xMax - (float)xMin) * w;
            float TransformY(float y) => this.ClientSize.Height - margin - (y - (float)yMin) / ((float)yMax - (float)yMin) * h;

            // Малюємо осі
            Pen axisPen = new Pen(Color.Black, 2);
            g.DrawLine(axisPen, margin, margin, margin, this.ClientSize.Height - margin); // Вісь Y
            g.DrawLine(axisPen, margin, this.ClientSize.Height - margin, this.ClientSize.Width - margin, this.ClientSize.Height - margin); // Вісь X

            // Малюємо графік
            if (points.Count > 1)
            {
                Pen graphPen = new Pen(Color.Blue, 3);
                for (int i = 0; i < points.Count - 1; i++)
                {
                    g.DrawLine(graphPen,
                        TransformX(points[i].X), TransformY(points[i].Y),
                        TransformX(points[i + 1].X), TransformY(points[i + 1].Y));
                }
            }

            // Підписи
            Font font = new Font("Arial", 10);
            g.DrawString($"x: [{xMin}; {xMax}]", font, Brushes.Black, margin, this.ClientSize.Height - 30);
            g.DrawString("y = (1.5x - ln(2x)) / (3x + 1)", font, Brushes.DarkBlue, margin, 20);
        }

        private void InitializeComponent()
        {

        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphForm());
        }
    }
}