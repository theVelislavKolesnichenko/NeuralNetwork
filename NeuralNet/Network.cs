using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Network : List<Layer>
{
    private int[] dimensions; //размера на невроната мрежа броя неврони на всеки слой
    private List<Pattern> patterns; //входните данни и очакваните изходи

    /// <summary>
    /// Конструктор за инициализиране на мрежата
    /// </summary>
    /// <param name="dimensions"></param>
    /// <param name="file"></param>
    public Network(int[] dimensions, string file, bool hasTraining = true)
    {
        this.dimensions = dimensions;
        Initialise(hasTraining);
        LoadPatterns(file);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Initialise(bool hasTraining = true)
    {
        if(hasTraining)
        {
            base.Clear();
            base.Add(new Layer(dimensions[0]));
            for (int i = 1; i < dimensions.Length; i++)
            {
                base.Add(new Layer(dimensions[i], base[i - 1], new Random()));
                //base.Add(new Layer(dimensions[i], base[i - 1], i-1));
            }
        }
        else
        {
            base.Clear();
            base.Add(new Layer(dimensions[0]));
            for (int i = 1; i < dimensions.Length; i++)
            {
                int count = 0;
                int index = count;
                count = dimensions[i - 1] * dimensions[i];
                base.Add(new Layer(dimensions[i], base[i - 1], patterns[0].Weight.ToList().GetRange(index, count)));
            }
        }

    }

    /// <summary>
    /// Инициализира входдните и очакваните данни на изхода 
    /// </summary>
    /// <param name="file"></param>
    private void LoadPatterns(string file)
    {
        patterns = new List<Pattern>();
        StreamReader reader = File.OpenText(file);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            patterns.Add(new Pattern(line, Inputs.Count, Outputs.Count));
        }
        reader.Close();
    }

    /// <summary>
    /// Тренировка на невронната мрежа
    /// </summary>
    /// <returns></returns>
    public double Train()
    {
        double[] target = new double[] { 0.1, 0.99}; 
        double error = 0;
        foreach (Pattern pattern in patterns)
        {
            Activate(pattern);
            //ConsolePrintOutput();
            for (int i = 0; i < Outputs.Count; i++)
            {
                double delta = pattern.Outputs[i] - Outputs[i].Output;
                //double delta = target[i] - Outputs[i].Output;
                //Console.WriteLine("y:{0}\nO:{1}\ne:{2}\n", pattern.Outputs[i], Outputs[i].Output, delta);

                Outputs[i].CollectError(delta);
                error += (1.0 / 2.0) * (Math.Pow(delta, 2));//Math.Pow(delta, 2)/2;
            }
            AdjustWeights();
        }
        return error;
    }

    //public void ConsolePrintOutput()
    //{
    //    for (int i = 0; i < Outputs.Count; i++)
    //    {
    //        Console.WriteLine("{0}{1}", pattern.Outputs[i] - Outputs[i].Output , Outputs[i].Output);
    //    }
    //}

    /// <summary>
    /// Прав пас на обхождане на мрежата
    /// </summary>
    /// <param name="pattern"></param>
    private void Activate(Pattern pattern)
    {
        for (int i = 0; i < Inputs.Count; i++)
        {
            Inputs[i].Output = pattern.Inputs[i];
        }
        for (int i = 1; i < base.Count; i++)
        {
            foreach (Neuron neuron in base[i])
            {
                neuron.Activate();
            }
        }
    }

    /// <summary>
    /// Коригиране на теглата
    /// </summary>
    private void AdjustWeights()
    {
        for (int i = base.Count - 1; i > 0; i--)
        {
            foreach (Neuron neuron in base[i])
            {
                neuron.AdjustWeights();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<double[]> HyperPlanes()
    {
        List<double[]> lines = new List<double[]>();
        foreach (Neuron n in Outputs)
        {
            lines.Add(n.HyperPlane);
        }
        return lines;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<float[]> Points2D()
    {
        int penultimate = base.Count - 2;
        //if (base[penultimate].Count != 2)
        //{
        //    throw new Exception("Penultimate layer must be 2D for graphing.");
        //}
        List<float[]> points = new List<float[]>();
        for (int i = 0; i < patterns.Count; i++)
        {
            Activate(patterns[i]);
            float[] point = new float[3];
            point[0] = (float)base[penultimate][0].Output;
            point[1] = (float)base[penultimate][1].Output;
            if (Outputs.Count > 1)
            {
                point[2] = patterns[i].MaxOutput;
            }
            else
            {
                point[2] = (patterns[i].Outputs[0] >= 0.5) ? 1 : 0;
            }
            points.Add(point);
        }
        return points;
    }

    /// <summary>
    /// Входния слой на невронната мрежа
    /// </summary>
    public Layer Inputs
    {
        get { return base[0]; }
    }

    /// <summary>
    /// Изходен слой на мрежата
    /// </summary>
    public Layer Outputs
    {
        get { return base[base.Count - 1]; }
    }
}
