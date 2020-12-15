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
        return null;
    }

    public virtual void UseOutputs(double[] outputs)
    {

    }

    public virtual void UpdateFitness()
    {

    }

    public virtual void RST()
    {

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
