using System;
using System.Collections.Generic;
public class Genome
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
    public int[] splitPoints;

    public Genome(double[] weights, double fitness, int[] splitPoints)
    {
        this.weights = (double[])weights.Clone();
        this.fitness = fitness;
        this.splitPoints = (int[])splitPoints.Clone();
    }
}

