using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacEvolutionManager : EvolutionManager
{
    public GameObject douPre;

    public static List<GameObject> showFood = new List<GameObject>();
    public static List<GameObject> hideFood = new List<GameObject>();

    public override void InitWorld()
    {
        for (int i = -20; i < 20; i += 4)
        {
            for (int j = -20; j < 20; j += 4)
            {
                GameObject food = Instantiate(douPre, new Vector3(i, 0.5f, j), Quaternion.identity);
                showFood.Add(food);
            }
        }
    }

    public override void ResetWorld()
    {


        for (int i = hideFood.Count - 1; i >= 0; i--)
        {
            hideFood[i].SetActive(true);
            showFood.Add(hideFood[i]);
            hideFood.RemoveAt(i);
        }
    }
}
