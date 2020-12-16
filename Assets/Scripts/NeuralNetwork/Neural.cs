using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neural
{
    /// <summary>
    /// count equals the input of current neural (output of last neural layer) add one (bias),
    /// while the count is the weight of each input
    /// </summary>
    public double[] weights;

    /// <summary>
    /// output of each neural
    /// </summary>
    public double value;

    /// <summary>
    /// whether the output of whole neural network
    /// </summary>
    public bool isOutput;

    /// <summary>
    /// the index of this neural in current layer
    /// </summary>
    public int index;

    public Neural(int index, int weightNum, bool isOutput)
    {
        this.index = index;
        weights = new double[weightNum];
        this.isOutput = isOutput;
        value = 0;
    }

    public void Execute(double[] inputs)
    {
        double sum = 0;
        if (weights.Length > 0)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i];
            }
        }
        else
        {
            sum += inputs[index];
        }
        value = TanhFunction(sum);

    }

    private double SigmoidFunction(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }
    private double TanhFunction(double x)
    {
        return Math.Tanh(x);
    }
}
