
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    /// <summary>
    /// the list of main player in current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    [HideInInspector]
    public List<EvolutionPlayer> epList;

    /// <summary>
    /// used to create neural network
    /// </summary>
    public int[] layerShape;
    /// <summary>
    /// the list of neural network in current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    public List<NeuralNetwork> nnList;

    /// <summary>
    /// chromosome list of current generation, whose count equals population,
    /// and it needs to reset in every new generation
    /// </summary>
    public List<Chromosome> chroList;
    /// <summary>
    /// the genetic algorithm used in this project, only set at begining
    /// </summary>
    public GeneticAlgorithm ga;
    /// <summary>
    /// the number of population in each generation of ga, have to set at begining
    /// </summary>
    public int population;

    /// <summary>
    /// the prefab of main player
    /// </summary>
    public GameObject mpPrefab;
    /// <summary>
    /// the root of prefab to attatch
    /// </summary>
    public Transform mpParentRoot;

    /// <summary>
    /// whether load weights from local to initial the neural network 
    /// </summary>
    public bool bLoadWeights;
    /// <summary>
    /// the path of local to load the weight for neural network
    /// </summary>
    public string weightPath;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitialManager();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateManager();  
    }

    void InitialManager()
    {
        InitialGameWorld();
        epList = new List<EvolutionPlayer>();
        nnList = new List<NeuralNetwork>();
        chroList = new List<Chromosome>();
        ga = new GeneticAlgorithm(population);
        for (int i = 0; i < population; i++)
        {
            NeuralNetwork nn = new NeuralNetwork(layerShape);
            if (bLoadWeights)
            {
                if (!nn.LoadWeights(weightPath))
                {
                    throw new System.Exception("Please input correct path of weight");
                }
            }
            else
            {
                nn.SetWeightsRandom();
            }
            Chromosome chro = new Chromosome(nn.GetWeights(), 0, nn.crossoverPoint);
            EvolutionPlayer mp = Instantiate(mpPrefab, mpParentRoot).GetComponent<EvolutionPlayer>();
            mp.SetInfo(nn, chro);
            mp.RST();

            nnList.Add(nn);
            chroList.Add(chro);
            epList.Add(mp);
        }
    }


    void UpdateManager()
    {
        if (IsAllDie())
        {
            ResetGameWorld();
            GetNextGeneration();
        }
    }

    private void GetNextGeneration()
    {
        chroList = ga.Execute(chroList);
        for(int i = 0; i< population; i++)
        {
            nnList[i].SetWeights(chroList[i].weights);
            epList[i].SetInfo(nnList[i], chroList[i]);
            epList[i].RST();
        }

    }

    public virtual bool IsAllDie()
    {
        bool res = true;
        for(int i = 0; i < epList.Count; i++)
        {
            if (epList[i].gameObject.activeSelf)
            {
                res = false;
                break;
            }
            
        }
        return res;
    }

    public virtual void InitialGameWorld()
    {

    }

    public virtual void ResetGameWorld()
    {

    }
}
