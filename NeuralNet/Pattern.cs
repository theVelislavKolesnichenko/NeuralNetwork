using System;
using System.Threading;
using System.Globalization;

public class Pattern
{
    private double[] inputs;
    private double[] outputs;

    public Pattern(string value, int inputDims, int outputDims)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("us-EN");
        // Sets the UI culture to French (France)
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("us-EN");

        string[] line = value.Split(',',';');
        if (line.Length != inputDims + outputDims)
            throw new Exception("Input does not match network configuration");
        inputs = new double[inputDims];
        for (int i = 0; i < inputDims; i++)
        {
            inputs[i] = double.Parse(line[i]);
        }
        outputs = new double[outputDims];
        for (int i = 0; i < outputDims; i++)
        {
            outputs[i] = double.Parse(line[i + inputDims]);
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
}
