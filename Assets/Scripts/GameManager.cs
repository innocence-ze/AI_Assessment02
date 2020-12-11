using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : EvolutionManager
{
    private static GameManager gm = null;
    
    public static GameManager Singleton
    {
        get
        {
            if(gm == null)
            {
                gm = FindObjectOfType<GameManager>();
                if(gm == null)
                {
                    throw new System.Exception("Without a game manager");
                }
            }
            return gm;
        }
    }



    public float addForceThreshold;

    public float cd;
    float timer;

    public float pipeMinVertical, pipeMaxVertical;

    public float pipeInitialX;

    public GameObject pipePrefab;

    List<GameObject> pipeList;
    GameObject pipePool;
    public List<GameObject> activePipeList;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        for(int i = 0; i < epList.Count; i++)
        {
            ((Bird)epList[i]).isAddForce = ((Bird)epList[i]).output > addForceThreshold;
        }

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            ActivePipe();
            timer = cd;
        }
        if(activePipeList.Count > 0 && activePipeList[0].transform.position.x < -pipeInitialX)
        {
            RestrainPipe(activePipeList[0]);
        }
    }

    void GeneratePipe()
    {
        float initialY = Random.Range(pipeMinVertical, pipeMaxVertical);
        var pipe = Instantiate(pipePrefab, new Vector3(pipeInitialX, initialY, 0), Quaternion.identity, pipePool.transform);
        pipe.SetActive(false);
        pipeList.Add(pipe);
    }

    void ActivePipe()
    {
        if(pipeList.Count == 0)
        {
            GeneratePipe();
        }
        var curPipe = pipeList[0];
        pipeList.Remove(curPipe);
        curPipe.SetActive(true);
        curPipe.transform.position = new Vector3(pipeInitialX, Random.Range(pipeMinVertical, pipeMaxVertical), 0);
        activePipeList.Add(curPipe);
    }

    void RestrainPipe(GameObject pipe)
    {
        activePipeList.Remove(pipe);
        pipeList.Add(pipe);
        pipe.SetActive(false);
    }

    void RestrainAllPipes()
    {
        pipeList.AddRange(activePipeList);
        for(int i = 0; i < activePipeList.Count; i++)
        {
            activePipeList[i].SetActive(false);
        }
        activePipeList.Clear();
    }

    public override bool IsAllDie()
    {
        for(int i = 0; i < epList.Count; i++)
        {
            if (((Bird)epList[i]).isAlive)
            {
                return false;
            }
        }
        return true;
    }

    public override void InitialGameWorld()
    {
        pipeList = new List<GameObject>();
        pipePool = new GameObject("PipePool");
        activePipeList = new List<GameObject>();
        timer = 0;
    }

    public override void ResetGameWorld()
    {
        for (int i = 0; i < epList.Count; i++)
        {
            epList[i].RST();
        }
        RestrainAllPipes();
        timer = 0;
        var rgs = FindObjectsOfType<ResetGround>();
        foreach(var rg in rgs)
        {
            rg.ResetGroundPos();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(Bird.bestBird.fit.ToString());
    }
}
