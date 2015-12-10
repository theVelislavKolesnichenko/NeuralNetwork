using System;
using System.Collections.Generic;

public class Neuron
{
    private double bias;                       // Bias value.
    private double error;                      // Sum of error.
    private double input;                      // Sum of inputs.
    private double gradient = 5;               // Steepness of sigmoid curve.
    private double learnRate = 0.01;           // Learning rate.
    private double output = double.MinValue;   // Preset value of neuron.
    private List<Weight> weights;              // Collection of weights to inputs.

    public Neuron() { }

    /// <summary>
    /// Конструктор за инициализиране на неврон
    /// </summary>
    /// <param name="inputs"></param>
    /// <param name="rnd"></param>
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
    /// Обща грешка
    /// </summary>
    /// <param name="delta"></param>
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
        bias += error * Derivative * learnRate;
    }

    /// <summary>
    /// Първа производна
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
            //return 1 / (1 + Math.Exp(-gradient * (input + bias)));
            return 1 / (1 + Math.Exp(-input));
        }
        set
        {
            output = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public double[] HyperPlane
    {
        get
        {
            double[] line = new double[3];
            line[0] = weights[0].Value;
            line[1] = weights[1].Value;
            line[2] = bias;
            return line;
        }
    }
}
