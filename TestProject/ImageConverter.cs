using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TestProject
{
    /// <summary>
    /// Description of ImageConverter.
    /// </summary>
    public static class ImageConverter
    {
        public static byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static byte[] imageToByteArray(Image imageIn, int size)
        {
            Bitmap bit = new Bitmap(imageIn);
            List<byte> lb = new List<byte>();

            int startH = bit.Height / size;
            int startW = bit.Width / size;
            int height = 0;
            int width = 0;
            int i = 0, j = 0;

            for (int item = 0; item < size; item++)
            {
                height += startH;
                i += startH;
                for (int jtem = 0; jtem < size; jtem++)
                {
                    width += startW;
                    lb.Add(ImageRectangle(bit, i, j, height, width));
                    j += startW;
                }
            }

            return lb.ToArray();
        }

        public static double[] imageToDoubleArray(Image imageIn, int size)
        {
            Bitmap bit = new Bitmap(imageIn);
            List<double> returnArrey = new List<double>();

            int endH = 0;
            int sum = 0;
            int index = 0;
            double index2 = 0;
            int H = bit.Height;
            int W = bit.Width;
            while (H - endH > H / size)
            {
                int endW = 0;
                int startH = endH;
                endH += H / size;
                index++;
                //Console.WriteLine(endH);
                while (W - endW > W / size)
                {
                    int startW = endW;
                    endW += W / size;
                    sum = 0;
                    for (int i = startW; i < endW; i++)
                    {
                        for (int j = startH; j < endH; j++)
                        {
                            //Console.Write("{0} ", mas[i, j]);
                            Color color = bit.GetPixel(i, j);
                            //sum += mas[i, j];
                            sum += (color.R + color.G + color.B) / 3;
                            index2++;
                        }
                    }

                    Console.WriteLine("{0} {1}", index, (sum / index2) / 255.0);
                    returnArrey.Add((sum / index2) / 255.0);
                    index2 = 0;
                }
            }
            returnArrey.Add(0);
            returnArrey.Add(0);
            returnArrey.Add(0);
            return returnArrey.ToArray();
        }

        private static byte ImageRectangle(Bitmap bit, int startH, int startW, int height, int width)
        {
            int b = 0;
            int length = 0;

            for (int i = startH; i < height; i++)
            {
                for (int j = startW; j < width; j++)
                {
                    b += PixelArc(bit.GetPixel(j, i));
                    length++;
                }
            }

            return Convert.ToByte(b / length);
        }

        private static byte PixelArc(Color color)
        {
            return Convert.ToByte((color.R + color.B + color.G) / 3);
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Method to resize, convert and save the image.
        /// </summary>
        /// <param name="image">Bitmap image.</param>
        /// <param name="maxWidth">resize width.</param>
        /// <param name="maxHeight">resize height.</param>
        /// <param name="quality">quality setting value.</param>
        /// <param name="filePath">file path.</param>      
        public static void Save(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
        {
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(filePath, imageCodecInfo, encoderParameters);
        }

        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
    }

}