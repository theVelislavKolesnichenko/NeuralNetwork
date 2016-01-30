using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFAClient
{
    public partial class Form1 : Form
    {
        string file = "ImageArray.csv";
        Network network;
        double[] АnalysisInput;

        public Form1()
       {
            InitializeComponent();
            label1.Text = string.Empty;
            button3.Enabled = File.Exists(file);

            double d = 0.9;
            Console.WriteLine(Math.Round(d));
        }

        private void SetStatus(string status)
        {
            label1.Text = status;
            label1.Invalidate();
            label1.Update();
            label1.Refresh();
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox1.Text = folderBrowserDialog.SelectedPath;
                    button3.Enabled = TestProject.ImageConverter.ImagesToFile(textBox1.Text, file, ";");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read folder from disk. Original error: " + ex.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream Stream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((Stream = openFileDialog1.OpenFile()) != null)
                    {
                        using (Stream)
                        {
                            pictureBox1.Image = Image.FromFile(((FileStream)Stream).Name);

                            АnalysisInput = TestProject.ImageConverter.imageToDoubleArray(pictureBox1.Image, 5).ToArray();

                            button4.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int era = 0;
            int[] dims = { 5*5, 10, 3 };
            network = new Network(dims, file);


            double error = int.MaxValue;
            while (error > 0.01)
            {
                era++;
                error = network.Train();
                SetStatus(string.Format("era: {0}\nerror: {1}", era, error));
            }

            button2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            network.Activate(АnalysisInput);

            double a = network.Outputs[0].Output;
            double b = network.Outputs[1].Output;
            double c = network.Outputs[2].Output;

            double erO = (1.0 / 2.0) * Math.Pow((0.0 - a), 2.0) + (1.0 / 2.0) * Math.Pow((0.0 - b), 2.0) + (1.0 / 2.0) * Math.Pow((1.0 - c), 2.0);
            double erQ = (1.0 / 2.0) * Math.Pow((0.0 - a), 2.0) + (1.0 / 2.0) * Math.Pow((1.0 - b), 2.0) + (1.0 / 2.0) * Math.Pow((0.0 - c), 2.0);
            double erT = (1.0 / 2.0) * Math.Pow((1.0 - a), 2.0) + (1.0 / 2.0) * Math.Pow((0.0 - b), 2.0) + (1.0 / 2.0) * Math.Pow((0.0 - c), 2.0);

            label1.Top = 50;

            label1.Text = string.Empty;

            label1.Text += string.Format("\n{0}\n{1}\n{2}", network.Outputs[0].Output, network.Outputs[1].Output, network.Outputs[2].Output);
            label1.Text += string.Format("\nerO: {0}\nerQ: {1}\nerT: {2}", erO, erQ, erT);
            label1.Text += string.Format("\n{0}", ShapeType(network).ToString());


        }

        public TestProject.Shapes ShapeType(Network net)
        {
            if (network.Outputs[0].Output > 0.9 && network.Outputs[0].Output <= 1.0)
            {
                return TestProject.Shapes.triangle;
            }
            else if (network.Outputs[1].Output > 0.9 && network.Outputs[1].Output <= 1.0)
            {
                return TestProject.Shapes.square;
            }
            else //if (network.Outputs[2].Output == 1)
            {
                return TestProject.Shapes.circles;
            }
        }
    }
}
