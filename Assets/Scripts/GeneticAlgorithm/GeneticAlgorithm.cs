///GA contains some steps: sort, find mating individuals, crossover, mutation, generate offspring
///need some data structures: Chromosome

using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{
    /// <summary>
    /// percentage of crossover (range 0-1, default value is 1)
    /// </summary>
    public float crossoverRate;
    /// <summary>
    /// percentage of mutation (range 0-1, default value is 0.1)
    /// </summary>
    public float mutationRate;
    /// <summary>
    /// the population of each generation
    /// </summary>
    public int populationNum;

    /// <summary>
    /// percentage of survival in the previous generation (range 0-1, default value is 0.25)
    /// </summary>
    public float surviveRate = 0.25f;
    /// <summary>
    /// whether the weak individual in the previous generation should survive
    /// </summary>
    public bool bSurviveWeak = false;

    public GeneticAlgorithm(int populationNum, float mutationRate = 0.1f, float crossoverRate = 1.0f)
    {
        this.populationNum = populationNum;
        this.mutationRate = mutationRate;
        this.crossoverRate = crossoverRate;
    }

    /// <summary>
    /// something happened in each generation of GA
    /// </summary>
    /// <param name="parents">
    /// the list of last generation chromosome
    /// </param>
    /// <returns>
    /// the list of current generation chromosome
    /// </returns>
    public List<Chromosome> Execute(List<Chromosome> parents)
    {
        //sort parents
        SortParents(parents);
        //survive
        if (bSurviveWeak)
            SurviveWithWeak(parents);
        else
            SurviveOnlyStrong(parents);
        //find mating individuals
        int parCount = parents.Count;
        //crossover
        Crossover(parents, parCount);
        //mutation
        Mutation(parents, parCount);
        //generate offspring
        return parents;
    }

    private void SortParents(List<Chromosome> arr)
    {
        arr.Sort();
    }

    private void SurviveOnlyStrong(List<Chromosome> arr)
    {
        int surNum = (int)(populationNum * surviveRate);
        arr.RemoveRange(surNum, arr.Count - surNum);
    }

    private void SurviveWithWeak(List<Chromosome> arr)
    {
        int surNum = (int)(populationNum * surviveRate);
        int weakIndex = Random.Range(surNum, arr.Count);
        var weakChro = arr[weakIndex];
        arr.RemoveRange(surNum, arr.Count - surNum);
        arr.Add(weakChro);
    }

    private void Crossover(List<Chromosome> arr, int parCount)
    {
        //totalFitness is used to set percentage of choosing parents
        double totalFitness = 0;
        for(int i = 0; i < parCount; i++)
        {
            if (arr[parCount - 1].fitness < 0)
            {
                arr[i].fitness -= arr[parCount - 1].fitness;
            }
            totalFitness += arr[i].fitness;
        }
        while (arr.Count < populationNum)
        {
            FindParents(out Chromosome father, out Chromosome mother, arr, totalFitness);
            CrossoverChildrenGene(father, mother, out Chromosome children1, out Chromosome children2);

            arr.Add(children1);
            if (arr.Count < populationNum)
                arr.Add(children2);
        }

    }

    private void FindParents(out Chromosome father, out Chromosome mother, List<Chromosome> arr, double totalFitness)
    {
        father = null;
        mother = null;
        float fatherRand = Random.Range(0, (float)totalFitness);
        float motherRand = Random.Range(0, (float)totalFitness);
        double tempFitness = 0;
        for (int i = 0; i < arr.Count; i++)
        {
            tempFitness += arr[i].fitness;
            if (father == null && tempFitness > fatherRand)
                father = arr[i];
            if (mother == null && tempFitness > motherRand)
                mother = arr[i];
            if (father != null && mother != null)
                break;
        }
    }

    private void CrossoverChildrenGene(Chromosome father, Chromosome mother, out Chromosome children1, out Chromosome children2)
    {
        children1 = new Chromosome(father.weights, father.fitness, father.crossoverPoints);
        children2 = new Chromosome(mother.weights, mother.fitness, mother.crossoverPoints);
        if (mother == father || Random.Range(0, 1f) > crossoverRate)
            return;
        int crossoverIndex1 = Random.Range(0, father.crossoverPoints.Length - 1);
        int crossoverIndex2 = Random.Range(crossoverIndex1 + 1, father.crossoverPoints.Length);
        crossoverIndex1 = father.crossoverPoints[crossoverIndex1];
        crossoverIndex2 = father.crossoverPoints[crossoverIndex2];
        for (int i = crossoverIndex1; i < crossoverIndex2; i++)
        {
            double temp = children1.weights[i];
            children1.weights[i] = children2.weights[i];
            children2.weights[i] = temp;
        }
    }

    private void Mutation(List<Chromosome> arr, int parCount)
    {
        for(int i = parCount; i < arr.Count; i++)
        {
            float mutationRange = Random.Range(0, 1f);
            if(mutationRange <= mutationRate)
            {
                int mutationIndex = Random.Range(0, arr[i].weights.Length);
                arr[i].weights[mutationIndex] = Random.Range(-1f, 1f);
            }
        }
    }
}
