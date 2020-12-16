/// Neural network contains some data structure: every neural unit, neural layer based on neural unit and neural network
/// the algorithm always execute in each neural unit
/// each neural unit have some inputs and the same number of weights, 
/// and after weighting, we get a input's sum.
/// Using this sum, the active function calculates the corresponding output, 
/// which is the one of the inputs of next layer's neural


using System.Collections.Generic;
using System.IO;

public class NeuralNetwork
{
    public List<NeuralLayer> neuralLayers;
    /// <summary>
    /// count equals the layer of neurals,
    /// while value is the count of neurals in each layer
    /// </summary>
    private int[] layerShape;
    /// <summary>
    /// count equals the num of all neurals,
    /// while vale is which can be split of whole weights normally between neurals
    /// </summary>
    public int[] splitPoints;
    /// <summary>
    /// the number of whole weights in this neural network
    /// </summary>
    public int weightNum;

    public NeuralNetwork(int[] layerShape)
    {
        this.layerShape = layerShape;
        neuralLayers = new List<NeuralLayer>();
        for (int i = 0; i < layerShape.Length; i++)
        {
            int weightNum = i == 0 ? 0 : layerShape[i - 1] + 1;
            NeuralLayer layer = new NeuralLayer(layerShape[i], weightNum, i == layerShape.Length - 1);
            neuralLayers.Add(layer);
        }
        GetSplitPoints();
    }

    public double[] Execute(double[] inputs)
    {
        double[] res = new double[layerShape[layerShape.Length - 1]];
        if (inputs.Length != layerShape[0])
        {
            throw new System.Exception("The input number should be: " + layerShape[0] + ", while current input number is" + inputs.Length);
        }
        for (int i = 0; i < layerShape.Length; i++)
        {
            if (i == 0)
            {
                neuralLayers[i].Execute(inputs);
            }
            else
            {
                neuralLayers[i].Execute(neuralLayers[i - 1].GetValues());
            }
        }
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = neuralLayers[neuralLayers.Count - 1].neurals[i].value;
        }
        return res;
    }

    public void SetWeights(double[] weights)
    {
        int index = 0;
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                for (int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    neuralLayers[i].neurals[j].weights[k] = weights[index];
                    index++;
                }
            }
        }
    }

    private void GetSplitPoints()
    {
        List<int> splitPointList = new List<int>();
        int n = 0;
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                splitPointList.Add(n + neuralLayers[i].neurals[j].weights.Length);
                n += neuralLayers[i].neurals[j].weights.Length;
                weightNum += neuralLayers[i].neurals[j].weights.Length;
            }
        }
        splitPoints = splitPointList.ToArray();
    }

    public double[] GetWeights()
    {
        List<double> weightList = new List<double>();
        for (int i = 1; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                for (int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    weightList.Add(neuralLayers[i].neurals[j].weights[k]);
                }
            }
        }
        return weightList.ToArray();
    }

    public void RandomWeights()
    {
        double[] weights = new double[weightNum];
        for (int i = 0; i < weightNum; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        SetWeights(weights);
    }

    public bool LoadWeights(string path)
    {
        if (File.Exists(path))
        {
            FileInfo f = new FileInfo(path);
            StreamReader sr = f.OpenText();
            string data = sr.ReadToEnd();
            sr.Close();
            string[] arr = data.Split(',');
            List<double> list = new List<double>();
            foreach (var item in arr)
            {
                list.Add(double.Parse(item));
            }
            SetWeights(list.ToArray());
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveWeights(string path)
    {
        double[] dw = GetWeights();
        string str = "";
        for (int i = 0; i < weightNum; i++)
        {
            str += dw[i] + (i == weightNum - 1 ? "" : ",");
        }
        StreamWriter sw = new StreamWriter(path, false);
        sw.Write(str);
        sw.Close();
    }
}
