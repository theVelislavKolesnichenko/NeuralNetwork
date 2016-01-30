using System;
using System.Collections.Generic;

public class Layer : List<Neuron>
{
    /// <summary>
    /// Конструктор за първия слои
    /// </summary>
    /// <param name="size">Брой неврони в слоя</param>
    public Layer(int size)
    {
        for (int i = 0; i < size; i++)
            base.Add(new Neuron());
    }

    /// <summary>
    /// Конструктор за N+1 слои
    /// </summary>
    /// <param name="size">Брой неврони в слоя</param>
    /// <param name="layer">Предходен неврон</param>
    /// <param name="rnd">Random обект за генериране на произволно тегло</param>
    public Layer(int size, Layer layer, Random rnd)
    {
        for (int i = 0; i < size; i++)
            base.Add(new Neuron(layer, rnd));
    }

    //public Layer(int size, Layer layer, int weight)
    //{
    //    for (int i = 0; i < size; i++)
    //        base.Add(new Neuron(layer, weight));
    //}

    //public Layer(int size, Layer layer, List<double> weights)
    //{
    //    int length = layer.Count;
         
    //    for (int i = 0; i < size; i++)
    //        base.Add(new Neuron(layer, weights.GetRange(i * length, length)));
    //}
}