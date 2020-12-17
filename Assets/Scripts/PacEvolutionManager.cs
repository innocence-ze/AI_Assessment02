using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacEvolutionManager : EvolutionManager
{
    public GameObject foodPre;
    int generation = 0;

    public static List<GameObject> showFoodList = new List<GameObject>();
    public static List<GameObject> hideFoodList = new List<GameObject>();

    public static List<Pac> livePacList = new List<Pac>();
    public static List<Pac> deadPacList = new List<Pac>();

    public override void InitialGameWorld()
    {
        showFoodList.Clear();
        hideFoodList.Clear();
        livePacList.Clear();
        deadPacList.Clear();

        for (int i = -20; i < 20; i += 4)
        {
            for (int j = -20; j < 20; j += 4)
            {
                GameObject food = Instantiate(foodPre, new Vector3(i, 0.5f, j), Quaternion.identity);
                showFoodList.Add(food);
            }
        }
    }

    public override void ResetGameWorld()
    {
        generation++;

        GameManager.Singleton.AppendMsg(generation, Pac.bestOne.fit, GetAvgFitness());

        for (int i = hideFoodList.Count - 1; i >= 0; i--)
        {
            hideFoodList[i].SetActive(true);
            showFoodList.Add(hideFoodList[i]);
            hideFoodList.RemoveAt(i);
        }
    }

    public float GetAvgFitness()
    {
        float totalFit = 0;
        if (livePacList.Count > 0)
        {
            foreach (var p in livePacList)
            {
                totalFit += p.fit;
            }
        }
        if (deadPacList.Count > 0)
        {
            foreach (var p in deadPacList)
            {
                totalFit += p.fit;
            }
        }
        return totalFit / populationSize;
    }

    public override bool CheckAllDie()
    {
        return livePacList.Count == 0 || showFoodList.Count == 0;
    }
}
