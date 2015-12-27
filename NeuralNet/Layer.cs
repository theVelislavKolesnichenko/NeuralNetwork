using System;
using System.Collections.Generic;

public class Layer : List<Neuron>
{
    public Layer(int size)
    {
        for (int i = 0; i < size; i++)
            base.Add(new Neuron());
    }

    public Layer(int size, Layer layer, Random rnd)
    {
        for (int i = 0; i < size; i++)
            base.Add(new Neuron(layer, rnd));
    }

    public Layer(int size, Layer layer, int weight)
    {
        for (int i = 0; i < size; i++)
            base.Add(new Neuron(layer, weight));
    }

    public Layer(int size, Layer layer, List<double> weights)
    {
        int length = layer.Count;
         
        for (int i = 0; i < size; i++)
            base.Add(new Neuron(layer, weights.GetRange(i * length, length)));
    }
}