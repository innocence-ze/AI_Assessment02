public class Chromosome : System.IComparable
{
    /// <summary>
    /// the message(gene) in each chromosome
    /// </summary>
    public double[] weights;
    /// <summary>
    /// the score of each individual
    /// </summary>
    public double fitness;
    /// <summary>
    /// the index of point which can be crossover by the parents
    /// </summary>
    public int[] crossoverPoints;

    public Chromosome(double[] weights, double fitness, int[] crossoverPoints)
    {
        this.weights = weights;
        this.fitness = fitness;
        this.crossoverPoints = crossoverPoints;
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;
        if (obj is Chromosome otherChro)
        {
            return fitness.CompareTo(otherChro.fitness) * -1;
        }
        else
        {
            throw new System.ArgumentException("Object is not a Chromosome");
        }
    }
}
