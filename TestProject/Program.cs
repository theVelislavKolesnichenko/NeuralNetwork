using System;
using System.Collections.Generic;
using System.IO;

namespace TestProject
{
    public class Program
    {
        public static List<double> TargetEnd(int shapes1)
        { 
            List<double> end = new List<double>();
            Shapes shapes = (Shapes)shapes1;
            if (shapes == Shapes.circles)
            {
                end.Add(0);
                end.Add(0);
                end.Add(1);
            }

            if (shapes == Shapes.square)
            {
                end.Add(0);
                end.Add(1);
                end.Add(0);
            }

            if (shapes == Shapes.triangle)
            {
                end.Add(1);
                end.Add(0);
                end.Add(0);
            }

            return end;
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
        }

    }
}
