using System.Collections.Generic;

public class NeuralLayer
{
    public List<Neural> neurals;

    public NeuralLayer(int neuralNum, int weightNum, bool bOutput)
    {
        neurals = new List<Neural>(neuralNum);
        for(int i = 0; i < neuralNum; i++)
        {
            neurals.Add(new Neural(i, bOutput, weightNum));
        }
    }

    public void Execute(double[] inputs)
    {
        for(int i = 0; i < neurals.Count; i++)
        {
            neurals[i].Execute(inputs);
        }
    }

    public double[] GetValues()
    {
        //+1 because of bias
        double[] res = new double[neurals.Count + 1];
        for(int i = 0; i <neurals.Count; i++)
        {
            res[i] = neurals[i].value;
        }
        res[neurals.Count] = 1;
        return res;
    }
}
