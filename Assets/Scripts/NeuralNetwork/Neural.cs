using System;
public enum FuncType
{
    Tanh,
    Sigmoid
}

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
    public bool bOutput;
    /// <summary>
    /// the index of this neural in current layer
    /// </summary>
    public int index;
    /// <summary>
    /// neural network use which kind of Active Function
    /// </summary>
    public static FuncType funcType = FuncType.Tanh;

    public Neural(int index, bool bOutput, int weightNum)
    {
        this.index = index;
        this.bOutput = bOutput;
        weights = new double[weightNum];
        value = 0;
    }

    public void Execute(double[] inputs)
    {
        double sum = 0;
        if(weights.Length > 0)
        {
            for(int i = 0; i < weights.Length; i++)
            {
                sum += weights[i] * inputs[i];
            }
        }
        else
        {
            sum = inputs[index];
        }

        if(funcType == FuncType.Tanh)
        {
            value = TanhFunction(sum);
        }
        else if(funcType == FuncType.Sigmoid)
        {
            value = SigmoidFunction(sum);
        }
    }

    private double SigmoidFunction(double x, float c = 1)
    {
        return 1.0 / (1 + Math.Exp(-x / c));
    }

    private double TanhFunction(double x)
    {
        return Math.Tanh(x);
    }

}
