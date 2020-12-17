///GA contains some steps: sort, find mating individuals, crossover, mutation, generate offspring
///need some data structures: Chromosome

using System;
using System.Collections.Generic;
using System.Linq;
public class GeneticAlgorithm
{
    /// <summary>
    /// the population of each generation
    /// </summary>
    private int populationSize;
    /// <summary>
    /// percentage of crossover (range 0-1, default value is 1)
    /// </summary>
    private double cross_ratio = 1;

    /// <summary>
    /// percentage of mutation (range 0-1, default value is 0.1)
    /// </summary>
    private double muta_ratio = 0.3;
    private double dertFit;

    public GeneticAlgorithm(int populationSize)
    {
        this.populationSize = populationSize;
    }

    /// <summary>
    /// something happened in each generation of GA
    /// </summary>
    /// <param name="parents">
    /// the list of last generation Genome
    /// </param>
    /// <returns>
    /// the list of current generation Genome
    /// </returns>
    public List<double[]> Execute(List<Genome> parents)
    {
        List<double[]> children = new List<double[]>();

        #region sort parents by fitness
        parents.Sort((x, y) =>
        {
            if (x.fitness > y.fitness)
            {
                return 1;
            }
            else if (x.fitness < y.fitness)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });
        #endregion


        #region adapt negative fitness
        dertFit = parents.Min(a => a.fitness);
        if (dertFit < 0) dertFit = -dertFit;
        parents.ForEach(a => a.fitness += dertFit);
        #endregion

        #region 1/4 parents can be survive and generate
        int elite_num = populationSize / 4;
        List<double[]> elite = new List<double[]>();
        List<Genome> eliteGenomeList = new List<Genome>();
        for (int i = 0; i < elite_num; i++)
        {
            eliteGenomeList.Add(parents[populationSize - i - 1]);
            elite.Add(parents[populationSize - i - 1].weights.CloneArr());
        }
        for (int i = 0; i < elite.Count; i++)
        {
            children.Add(elite[i].CloneArr());
        }
        #endregion

        #region crossover
        while (true)
        {
            Genome dad = GetParent(eliteGenomeList);
            Genome mum = GetParent(eliteGenomeList);
            double[] baby1 = null;
            double[] baby2 = null;
            CrossoverAtSplitPoint(dad.splitPoints, dad.weights, mum.weights, out baby1, out baby2);
            children.Add(baby1);
            children.Add(baby2);
            int n = populationSize;// (int)(population * 3.6f / 4); whether abandon weakness population
            if (children.Count >= n)
            {
                while (true)
                {
                    if (children.Count > n)
                    {
                        children.RemoveAt(children.Count - 1);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            }
        }
        #endregion

        #region mutation
        for (int i = elite.Count; i < children.Count(); i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < muta_ratio)
            {
                int index = UnityEngine.Random.Range(0, children[0].Length);
                children[i][index] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
        #endregion

        #region alow weakness survive
        //while (populationSize > children.Count)
        //{
        //    double[] ws = new double[children[0].Length];
        //    for (int j = 0; j < ws.Length; j++)
        //    {
        //        ws[j] = UnityEngine.Random.Range(-1f, 1f);
        //    }
        //    children.Add(ws);
        //}
        #endregion

        return children;
    }

    private Genome GetParent(List<Genome> parents)
    {
        double totalFit = 0;
        double min = 9999;
        foreach (var t in parents)
        {
            if (t.fitness < min)
            {
                min = t.fitness;
            }
        }
        foreach (var t in parents)
        {
            if (min < 0)
            {
                t.fitness += Math.Abs(min);
            }
            totalFit += t.fitness;
        }
        float rand = UnityEngine.Random.Range(0f, (float)totalFit);
        double tempFit = 0;
        int index = parents.Count - 1;
        for (int i = 0; i < parents.Count; i++)
        {
            tempFit += parents[i].fitness;
            if (tempFit >= rand)
            {
                index = i;
                break;
            }
        }
        return new Genome(parents[index].weights.CloneArr(), parents[index].fitness, parents[index].splitPoints);
    }

    private void CrossoverAtSplitPoint(int[] splitPoints, double[] dad, double[] mum, out double[] baby1, out double[] baby2)
    {
        baby1 = new double[dad.Length];
        baby2 = new double[dad.Length];
        if ((UnityEngine.Random.Range(0f, 1f) > cross_ratio) || (mum == dad))
        {
            baby1 = mum.CloneArr();
            baby2 = dad.CloneArr();
            return;
        }
        int index1 = UnityEngine.Random.Range(0, splitPoints.Length - 2);
        int index2 = UnityEngine.Random.Range(index1, splitPoints.Length - 1);
        int cp1 = splitPoints[index1];
        int cp2 = splitPoints[index2];

        for (int i = 0; i < mum.Length; ++i)
        {
            if ((i < cp1) || (i >= cp2))
            {
                // out of crossover part do not change
                baby1[i] = mum[i];
                baby2[i] = dad[i];
            }
            else
            {
                // change inside part
                baby1[i] = dad[i];
                baby2[i] = mum[i];
            }
        }
    }
}

public static class Extension
{
    public static double[] CloneArr(this double[] d)
    {
        double[] dl = new double[d.Length];
        for (int i = 0; i < d.Length; i++)
        {
            dl[i] = d[i];
        }
        return dl;
    }
}

