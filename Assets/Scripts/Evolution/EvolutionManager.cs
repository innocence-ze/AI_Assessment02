using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    /// <summary>
    /// used to create neural network
    /// </summary>
    public int[] layerShape;
    /// <summary>
    /// the number of population in each generation of ga, have to set at begining
    /// </summary>
    public int populationSize;

    /// <summary>
    /// the prefab of main player
    /// </summary>
    public GameObject agentPrefab;
    /// <summary>
    /// the root of prefab to attatch
    /// </summary>
    public Transform agentsParent;

    /// <summary>
    /// the genetic algorithm used in this project, only set at begining
    /// </summary>
    private GeneticAlgorithm ga;

    /// <summary>
    /// chromosome list of current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    private List<Genome> genomeList = new List<Genome>();
    /// <summary>
    /// the list of neural network in current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    private List<NeuralNetwork> nnList = new List<NeuralNetwork>();
    /// <summary>
    /// the list of main player in current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    private List<Agent> agentList = new List<Agent>();

    public bool loadWeights;
    public string weightPath;
    void Start()
    {
        InitialWorld();
    }

    void Update()
    {
        UpdateWorld();
    }

    void SaveBest()
    {
        NeuralNetwork bestNn = null;
        double maxFit = -9999;
        foreach (var item in agentList)
        {
            if (item.ge.fitness > maxFit)
            {
                bestNn = item.nn;
                maxFit = item.ge.fitness;
            }
        }
        if (bestNn != null)
        {
            bestNn.SaveWeights(Application.dataPath + weightPath);
        }
    }

    void InitialWorld()
    {
        InitialGameWorld();
        ga = new GeneticAlgorithm(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork nn = new NeuralNetwork(layerShape);
            if (loadWeights)
            {
                if (!nn.LoadWeights(Application.dataPath + weightPath))
                {
                    Debug.LogError("please use correct path");
                }
            }
            else
            {
                nn.RandomWeights();
            }


            Genome ge = new Genome(nn.GetWeights(), 0, nn.splitPoints);

            Agent ac = Instantiate(agentPrefab).GetComponent<Agent>();
            ac.transform.SetParent(agentsParent);
            ac.SetInfo(nn, ge);
            ac.Reset();

            nnList.Add(nn);
            genomeList.Add(ge);
            agentList.Add(ac);
        }
    }

    void UpdateWorld()
    {
        if(CheckAllDie())
        {
            ResetGameWorld();
            FinishGA();
        }
    }

    void FinishGA()
    {
        SaveBest();
        List<double[]> weightsList = ga.Execute(genomeList);
        for (int i = 0; i < weightsList.Count; i++)
        {
            nnList[i].SetWeights(weightsList[i]);
            agentList[i].nn = nnList[i];
            genomeList[i] = new Genome(nnList[i].GetWeights(), 0, nnList[i].splitPoints);
            agentList[i].ge = genomeList[i];
            agentList[i].Reset();
        }
    }

    public virtual bool CheckAllDie()
    {
        bool isAllDie = true;
        foreach (var t in agentList)
        {
            if (t.gameObject.activeSelf)
            {
                isAllDie = false;
                break;
            }
        }
        return isAllDie;
    }

    public virtual void ResetGameWorld()
    {

    }

    public virtual void InitialGameWorld()
    {

    }
}
