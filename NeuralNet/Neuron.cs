using System;
using System.Collections.Generic;

public class Neuron
{
    private double error;                      // Сума от грешките на предходните неврони
    private double input;                      // Входна сума
    private double learnRate = 0.5;           // Убочаваща скорост
    private double output = double.MinValue;   // Изхудна стоиност от неврона
    private List<Weight> weights;              // Колекция от входните тегла

    //double[,] testWeights = new double[,] { { .15, .20, }, { .25, .30, }, { .40, .45, }, { .50, .55 } };
    //static int sya = 0;
    //public Neuron(Layer inputs, int weight)
    //{
    //    int index=0;

    //    weights = new List<Weight>();
    //    foreach (Neuron input in inputs)
    //    {
    //        Weight w = new Weight();
    //        w.Input = input;
    //        w.Value = testWeights[sya, index];
    //        weights.Add(w);
    //        index++;
    //    }
    //    sya++;
    //}

    /// <summary>
    /// Подразбиращ се конструктор
    /// </summary>
    public Neuron() { }
    
    /// <summary>
    /// Конструктор за инициализиране на неврон
    /// </summary>
    /// <param name="inputs">Списък с входни неврони</param>
    /// <param name="rnd">Random обект за генериране на входните тегла</param>
    public Neuron(Layer inputs, Random rnd)
    {
        weights = new List<Weight>();
        foreach (Neuron input in inputs)
        {
            Weight w = new Weight();
            w.Input = input;
            w.Value = rnd.NextDouble() * 2 - 1;
            weights.Add(w);
        }
    }

    //public Neuron(Layer inputs, List<double> weights)
    //{
    //    int index = 0;
    //    this.weights = new List<Weight>();
    //    foreach (Neuron input in inputs)
    //    {
    //        Weight w = new Weight();
    //        w.Input = input;
    //        w.Value = weights[index++];
    //        this.weights.Add(w);
    //        index++;
    //    }
    //}

    /// <summary>
    /// Активиране на неврона
    /// </summary>
    public void Activate()
    {
        error = 0;
        input = 0;
        foreach (Weight w in weights)
        {
            input += w.Value * w.Input.Output;
        }
    }

    /// <summary>
    /// Предаване на грешка във вътрешност
    /// </summary>
    /// <param name="delta">Грешката от предходния неврон</param>
    public void CollectError(double delta)
    {
        if (weights != null)
        {
            error += delta;
            foreach (Weight w in weights)
            {
                w.Input.CollectError(error * Derivative * w.Value);
            }
        }
    }

    /// <summary>
    /// Коригиране на теглата 
    /// </summary>
    public void AdjustWeights()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            weights[i].Value += learnRate * error * Derivative * weights[i].Input.Output;
        }
    }

    /// <summary>
    /// Първа производна на актвационата функция
    /// </summary>
    private double Derivative
    {
        get
        {
            double activation = Output;
            return activation * (1 - activation);
        }
    }

    /// <summary>
    /// Изход от неврона
    /// </summary>
    public double Output
    {
        get
        {
            if (output != double.MinValue)
            {
                return output;
            }
            return 1.0 / (1.0 + Math.Exp(-input));
        }
        set
        {
            output = value;
        }
    }
}
