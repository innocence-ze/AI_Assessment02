using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGameManager : EvolutionManager
{
    public float timescale = 1;

    public GameObject foodPrefab;
    public static List<Pac> livePac = new List<Pac>();
    public static List<Pac> deadPac = new List<Pac>();

    public static List<GameObject> showFood = new List<GameObject>();
    public static List<GameObject> hideFood = new List<GameObject>();

    public override bool IsAllDie()
    {
        return livePac.Count == 0 || showFood.Count == 0;
    }

    public override void InitialGameWorld()
    {
        for(int i = -20; i < 20; i += 4)
        {
            for(int j = -20; j < 20; j += 4)
            {
                GameObject food = Instantiate(foodPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                showFood.Add(food);
            }
        }
    }

    public override void ResetGameWorld()
    {
        float avg = 0;
        foreach(var p in deadPac)
        {
            avg += p.fitness;
        }
        avg /= deadPac.Count;
        Debug.Log("best pacman's fitness is: " + Pac.bestOne.fitness + ", while the avg fitness is " + avg 
                + " and still have " + showFood.Count + " food");

        Pac.bestOne.nn.SaveWeights(weightPath);

        for(int i = hideFood.Count - 1; i >= 0; i--)
        {
            hideFood[i].SetActive(true);
            showFood.Add(hideFood[i]);
            hideFood.RemoveAt(i);
        }
    }

    protected override void Update()
    {
        base.Update();
        Time.timeScale = timescale;
    }

}
