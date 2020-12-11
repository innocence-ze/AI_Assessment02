using UnityEngine;

public class EvolutionPlayer : MonoBehaviour
{
    public Chromosome chro;
    public NeuralNetwork nn;

    public virtual void SetInfo(NeuralNetwork nn, Chromosome chro)
    {
        this.nn = nn;
        this.chro = chro;
    }

    public virtual double[] GetInputs()
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

    public virtual void RST()
    {
        throw new System.Exception();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        double[] input = GetInputs();
        double[] output = nn.Execute(input);
        UseOutputs(output);
        UpdateFitness();
    }
}
