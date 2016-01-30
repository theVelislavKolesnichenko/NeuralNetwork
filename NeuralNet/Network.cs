using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Network : List<Layer>
{
    /// <summary>
    /// размера на невроната мрежа броя неврони на всеки слой
    /// </summary>
    private int[] dimensions;
    /// <summary>
    /// входните данни и очакваните изходи
    /// </summary>
    private List<Pattern> patterns;

    /// <summary>
    /// Конструктор за инициализиране на мрежата
    /// </summary>
    /// <param name="dimensions"></param>
    /// <param name="file">Име на файла с входните и еталонни данни</param>
    public Network(int[] dimensions, string file, bool hasTraining = true)
    {
        this.dimensions = dimensions;
        Initialise(hasTraining);
        LoadPatterns(file);
    }

    /// <summary>
    /// Инициализиране на мрежата
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
            }
        }
        else
        {
            //base.Clear();
            //base.Add(new Layer(dimensions[0]));
            //for (int i = 1; i < dimensions.Length; i++)
            //{
            //    int count = 0;
            //    int index = count;
            //    count = dimensions[i - 1] * dimensions[i];
            //    base.Add(new Layer(dimensions[i], base[i - 1], patterns[0].Weight.ToList().GetRange(index, count)));
            //}
        }
    }

    /// <summary>
    /// Инициализира входдните и очакваните данни на изхода 
    /// </summary>
    /// <param name="file">>Име на файла с входните и еталонни данни</param>
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
    /// <returns>Средно квадратична грешка</returns>
    public double Train()
    {
        double error = 0;
        foreach (Pattern pattern in patterns)
        {
            Activate(pattern);

            for (int i = 0; i < Outputs.Count; i++)
            {
                double delta = pattern.Outputs[i] - Outputs[i].Output;

                Outputs[i].CollectError(delta);
                error += (1.0 / 2.0) * (Math.Pow(delta, 2));
            }
            AdjustWeights();
        }
        return error;
    }

    /// <summary>
    /// Прав пас на обхождане на мрежата за убочение със еталонни данни
    /// </summary>
    /// <param name="pattern">Обект с входните и еталонни данни</param>
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
    /// Прав пас на обхождане на мрежата за класифициране на обекта
    /// </summary>
    /// <param name="input">Масив с описание на входния обект</param>
    public void Activate(double[] input)
    {
        for (int i = 0; i < Inputs.Count; i++)
        {
            Inputs[i].Output = input[i];
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
