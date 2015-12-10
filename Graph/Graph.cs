using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Graph : Form
    {
        private int iteration;
        private Network network;
        private Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };

        public Graph()
        {
            //Image image0 = Image.FromFile(@"E:\DevRepository\VisualStudio\CPlusPlus\NeuralNetwork\TestProject\RectI.gif");
            //double[] arr = TestProject.ImageConverter.imageToDoubleArray(image0, 5);

            int[] dims = { 4, 5, 3 };    // Replace with your network dimensions.
            string file = "iris_data.csv";  // Replace with your input file location.
            network = new Network(dims, file);
            InitializeComponent();
            Initialise();
        }

        private void Initialise()
        {
            ClientSize = new Size(400, 400);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            KeyDown += new KeyEventHandler(Graph_KeyDown);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            double error = network.Train();
            UpdatePlotArea(e.Graphics, error);
            iteration++;
            if(error > 0)
            { Invalidate(); }
        }

        private void UpdatePlotArea(Graphics g, double error)
        {
            string banner = "Iteration={0}  Error={1:0.00}";
            Text = string.Format(banner, iteration, error);
            g.FillRectangle(Brushes.White, 0, 0, 400, 400);
            foreach (float[] point in network.Points2D())
            {
                g.FillRectangle(brushes[(int)point[2]], point[0] * 395, point[1] * 395, 5, 5);
            }
            foreach (double[] line in network.HyperPlanes())
            {
                double a = -line[0] / line[1];
                double c = -line[2] / line[1];
                Point left = new Point(0, (int)(c * 400));
                Point right = new Point(400, (int)((a + c) * 400));
                g.DrawLine(new Pen(Color.Gray), left, right);
            }
        }

        private void Graph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                network.Initialise();
                iteration = 0;
            }
        }
    }
}