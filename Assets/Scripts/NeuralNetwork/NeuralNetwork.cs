/// Neural network contains some data structure: every neural unit, neural layer based on neural unit and neural network
/// the algorithm always execute in each neural unit
/// each neural unit have some inputs and the same number of weights, 
/// and after weighting, we get a input's sum.
/// Using this sum, the active function calculates the corresponding output, 
/// which is the one of the inputs of next layer's neural

using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

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
    public int[] crossoverPoint;
    /// <summary>
    /// the number of whole weights in this neural network
    /// </summary>
    public int weightNum;

    public NeuralNetwork(int[] layerShape)
    {
        this.layerShape = layerShape;
        neuralLayers = new List<NeuralLayer>(layerShape.Length);
        int neuralCount = 0;
        for(int i = 0; i < layerShape.Length; i++)
        {
            //+1 is for bias, while i==0 is for input layer
            int weightCount = i == 0 ? 0 : layerShape[i - 1] + 1;
            neuralLayers.Add(new NeuralLayer(layerShape[i], weightCount , i == layerShape.Length - 1));
            neuralCount += layerShape[i];
        }


        crossoverPoint = new int[neuralCount - neuralLayers[0].neurals.Count + 1];
        int tempCounter = 1, n = 0;
        crossoverPoint[0] = 0;
        for(int i = 1; i < neuralLayers.Count; i++)
        {
            for(int j = 0; j < neuralLayers[i].neurals.Count; j++)
            {
                n += neuralLayers[i].neurals[j].weights.Length;
                crossoverPoint[tempCounter] = n;
                tempCounter++;
            }
        }
        weightNum = n;
    }

    public double[] Execute(double[] input)
    {
        double[] res = new double[layerShape[layerShape.Length - 1]];
        if(input.Length != layerShape[0])
        {
            throw new System.Exception("the number of input parameter is incorrect and it should be " + layerShape[0] + ", while current number is " + input.Length + ".");
        }

        neuralLayers[0].Execute(input);
        for (int i = 1; i < neuralLayers.Count; i++)
        {
            neuralLayers[i].Execute(neuralLayers[i - 1].GetValues());
        }
        for(int i = 0; i < res.Length; i++)
        {
            res[i] = neuralLayers[neuralLayers.Count - 1].neurals[i].value;
        }
        return res;
    }

    public void SetWeightsRandom()
    {
        double[] randomWeights = new double[weightNum];
        for(int i = 0; i < weightNum; i++)
        {
            randomWeights[i] = Random.Range(-1f, 1f);
        }
        SetWeights(randomWeights);
    }

    public void SetWeights(double[] w)
    {
        int index = 0;
        for(int i = 0; i < layerShape.Length; i++)
        {
            for(int j = 0; j < layerShape[i]; j++)
            {
                for(int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    neuralLayers[i].neurals[j].weights[k] = w[index];
                    index++;
                }
            }
        }
    }

    public double[] GetWeights()
    {
        double[] res = new double[weightNum];
        int index = 0;
        for (int i = 0; i < layerShape.Length; i++)
        {
            for (int j = 0; j < layerShape[i]; j++)
            {
                for (int k = 0; k < neuralLayers[i].neurals[j].weights.Length; k++)
                {
                    res[index] = neuralLayers[i].neurals[j].weights[k];
                    index++;
                }
            }
        }
        return res;
    }

    public bool LoadWeights(string path)
    {
        if (File.Exists(path))
        {
            StreamReader sr = new FileInfo(path).OpenText();
            string data = sr.ReadToEnd();
            sr.Close();
            string[] arr = data.Split(',');
            double[] w = new double[arr.Length];
            for(int i = 0; i < w.Length; i++)
            {
                w[i] = double.Parse(arr[i]);
            }
            SetWeights(w);
            return true;
        }
        return false;
    }

    public bool SaveWeights(string path)
    {
        double[] dw = GetWeights();
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < weightNum; i++)
        {
            str.Append(dw[i]);
            str.Append(i == weightNum - 1 ? "" : ",");
        }
        StreamWriter sw = new StreamWriter(path, false);
        sw.Write(str);
        sw.Close();
        return true;
    }

}
