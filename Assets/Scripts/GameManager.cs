using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Obstacles = new List<GameObject>();

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton == null)
                {
                    singleton = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return singleton;
        }
    }

    private static GameManager singleton = null;


    public TMP_Text msgText;

    public void AppendMsg(int generation, float maxFit, float avgFit)
    {
        var genStr = string.Format("{0:000}", generation);
        var maxStr = string.Format("{0:000}", maxFit);
        var avgStr = string.Format("{0:000.00}", avgFit);
        msgText.text += " " + genStr + " | " + maxStr + " | " + avgStr + "\n";
    }

    public void ClearMsg()
    {
        msgText.text = "";
    }

    public void SetObsActive(bool isShow)
    {
        foreach(var o in Obstacles)
        {
            o.SetActive(isShow);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
