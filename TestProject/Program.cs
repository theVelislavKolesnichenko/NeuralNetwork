using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject
{
    public class Program
    {
        public static void Main()
        {
            Image image0 = Image.FromFile(@"E:\DevRepository\VisualStudio\CPlusPlus\NeuralNetwork\TestProject\RectI.gif");
            Image image1 = Image.FromFile(@"E:\DevRepository\VisualStudio\CPlusPlus\NeuralNetwork\TestProject\v7CDE.png");
            //byte[] ab0 = ImageConverter.imageToByteArray(image0);
            //byte[] ab1 = ImageConverter.imageToByteArray(image1);
            //Image image2 = ImageConverter.byteArrayToImage(ab0);
            //image2.Save("image1.gif");

            ////Resize image
            ////Image originalImage = Image.FromStream(inputStream, true, true);
            //Image resizedImage0 = image0.GetThumbnailImage(100, 100, null, IntPtr.Zero);
            //Image resizedImage1 = image1.GetThumbnailImage(100, 100, null, IntPtr.Zero);
            //resizedImage0.Save("resizedImage.gif");
            //byte[] ab2 = ImageConverter.imageToByteArray(resizedImage0);
            //byte[] ab3 = ImageConverter.imageToByteArray(resizedImage1);
            //byte[] ab0 = ImageConverter.imageToByteArray(image0, 5);
            //byte[] ab1 = ImageConverter.imageToByteArray(image1, 5);

            //string text = string.Join( "\n", ab0.Select(a => (double)a/255).Select(a => a.ToString()) );

            //Console.WriteLine(text);

            Bitmap bit = new Bitmap(image0);

            int[,] mas = new int[,] {{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                     {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                     {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                                     {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                                     {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                                     {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                                     {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                                     {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                                     {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                                     {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}};

            int endH = 0;
            int sum = 0;
            int index = 0;
            double index2 = 0;
            int H = bit.Height;
            int W = bit.Width;
            while (H - endH > H / 5)
            {
                int endW = 0;
                int startH = endH;
                endH += H / 5;
                index++;
                //Console.WriteLine(endH);
                while (W - endW > W / 5)
                {
                    int startW = endW;
                    endW += W / 5;
                    sum = 0;
                    for (int i = startW; i < endW; i++)
                    {
                        for (int j = startH; j < endH; j++)
                        {
                            //Console.Write("{0} ", mas[i, j]);
                            Color color = bit.GetPixel(i, j);
                            //sum += mas[i, j];
                            sum += (color.R + color.G + color.B)/3;
                            index2++;
                        }
                    }
                    
                    Console.WriteLine("{0} {1}", index, (sum / index2) / 255.0);
                    index2 = 0;
                }
            }

            double[] arr = ImageConverter.imageToDoubleArray(image0, 5);

            string filePath = @"E:\test.csv";
            string delimiter = ";";
            //string[][] output = new string[][]{
            //new string[]{"Col 1 Row 1", "Col 2 Row 1", "Col 3 Row 1"},
            //new string[]{"Col1 Row 2", "Col2 Row 2", "Col3 Row 2"}
            //};
            //int length = output.GetLength(0);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < length; i++)
            //    sb.AppendLine(string.Join(delimiter, output[i]));
            //arr.Join(delimiter);

            File.WriteAllText(filePath, String.Join(delimiter, arr.Select(a => a.ToString())));


            Console.ReadKey();
        }

    }
}
