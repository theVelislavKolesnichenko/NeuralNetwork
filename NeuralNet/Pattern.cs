using System;
using System.Threading;
using System.Globalization;

public class Pattern
{
    private double[] inputs;
    private double[] outputs;
    private double[] weights;

    public Pattern(string value, int inputDims, int outputDims, bool hasTraining = true)
    {
        if (hasTraining)
        {
            InitializesTraining(value, inputDims, outputDims);
        }
        else
        {
            Initializes(value, inputDims, outputDims);
        }
    }

    private void InitializesTraining(string value, int inputDims, int outputDims)
    {
        string[] line = value.Split(',', ';');
        if (line.Length != inputDims + outputDims)
            throw new Exception("Input does not match network configuration");
        inputs = new double[inputDims];
        for (int i = 0; i < inputDims; i++)
        {
            inputs[i] = double.Parse(line[i]) / 255.0;
        }
        outputs = new double[outputDims];
        for (int i = 0; i < outputDims; i++)
        {
            outputs[i] = double.Parse(line[i + inputDims]);
        }
    }

    private void Initializes(string value, int inputDims, int weightDim)
    {
        string[] line = value.Split(',', ';');

        if (line.Length != inputDims + weightDim)
            throw new Exception("Input does not match network configuration");

        inputs = new double[inputDims];
        for (int i = 0; i < inputDims; i++)
        {
            inputs[i] = double.Parse(line[i]) / 255.0;
        }

        weights = new double[weightDim];
        for (int i = 0; i < weightDim; i++)
        {
            weights[i] = double.Parse(line[i + inputDims]);
        }
    }

    public int MaxOutput
    {
        get
        {
            int item = -1;
            double max = double.MinValue;
            for (int i = 0; i < Outputs.Length; i++)
            {
                if (Outputs[i] > max)
                {
                    max = Outputs[i];
                    item = i;
                }
            }
            return item;
        }
    }

    public double[] Inputs
    {
        get { return inputs; }
    }

    public double[] Outputs
    {
        get { return outputs; }
    }

    public double[] Weight
    {
        get { return weights; }
    }
}
