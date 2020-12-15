using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGround : MonoBehaviour
{
    public Transform[] resetGrounds;
    Vector3[] originalPoses;

    // Start is called before the first frame update
    void Start()
    {
        originalPoses = new Vector3[resetGrounds.Length];
        for(int i = 0; i < resetGrounds.Length; i++)
        {
            originalPoses[i] = resetGrounds[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(resetGrounds[resetGrounds.Length / 2].position.x <= originalPoses[0].x)
        {
            for(int i = 0; i < resetGrounds.Length; i++)
            {
                resetGrounds[i].position = originalPoses[i];
            }
        }
    }

    public void ResetGroundPos()
    {
        for(int i = 0; i < resetGrounds.Length; i++)
        {
            resetGrounds[i].position = originalPoses[i];
        }
    }

}
