using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public NeuralNetwork nn;
    public Genome ge;

    public virtual double[] GetInputs()
    {
        throw new System.Exception();
    }

    public virtual void Reset()
    {
        throw new System.Exception();
    }

    public virtual void UseOutputs(double[] outputs)
    {
        throw new System.Exception();
    }

    public virtual void UpdateFitness()
    {
        throw new System.Exception();
    }
    public virtual void SetInfo(NeuralNetwork nn, Genome ge)
    {
        this.nn = nn;
        this.ge = ge;
    }
    private void FixedUpdate()
    {
        double[] inputs = GetInputs();
        double[] outputs = nn.Execute(inputs);
        UseOutputs(outputs);
        UpdateFitness();
    }
}
