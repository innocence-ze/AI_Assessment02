using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAgent : EvolutionPlayer
{
    /// <summary>
    /// food's tag
    /// </summary>
    public string foodTag;
    /// <summary>
    /// partner's tag
    /// </summary>
    public string curTag;

    public float initialXRange;
    public float initialYRange;
    public float initialZRange;

    /// <summary>
    /// larget distance that shark can detect
    /// </summary>
    public float detectDistance;
    /// <summary>
    /// the number of ray to use for detecting
    /// </summary>
    public int detectNum;

    Rigidbody rb;

    public float maxTurnSpeed;
    public float maxAcceleration;
    /// <summary>
    /// max speed, while if shark's speed is higher than 60% of max speed, it will lost energy
    /// </summary>
    public float maxSpeed;

    /// <summary>
    /// fitness or socore
    /// </summary>
    public float fit;

    public bool bDrawLine;

    List<LineRenderer> lrList;
    List<Vector3> rayPosList;

    public float maxEnergy;
    [SerializeField]
    float curEnergy;

    double[] inputList;

    public static SharkAgent bestShark;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = curTag;
        transform.position = new Vector3(Random.Range(-initialXRange, initialXRange), Random.Range(0, initialYRange), Random.Range(-initialZRange, initialZRange));
        rb = gameObject.GetComponent<Rigidbody>();

        rayPosList = SetRayPos();
        lrList = new List<LineRenderer>();

        Transform rs = new GameObject("rays").transform;
        rs.parent = transform;
        for(int i = 0; i < detectNum; i++)
        {
            LineRenderer lr = new GameObject("ray" + i).AddComponent<LineRenderer>();
            lr.transform.parent = rs;
            lr.transform.localPosition = Vector3.zero;
            lr.transform.localRotation = Quaternion.identity;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, rayPosList[i] + transform.position);
            lr.startWidth = 0.03f;
            lr.endWidth = 0.03f;
            lrList.Add(lr);
        }
        curEnergy = maxEnergy;
        inputList = new double[3 * detectNum + 2];
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.sqrMagnitude >= maxSpeed * maxSpeed * 0.36f)
        {
            curEnergy -= (rb.velocity.sqrMagnitude - maxSpeed * maxSpeed * 0.36f) * Time.deltaTime;
        }
        else if(curEnergy < maxEnergy)
        {
            curEnergy += Time.deltaTime * maxEnergy * 0.1f;
        }
    }

    List<Vector3> SetRayPos()
    {
        List<Vector3> res = new List<Vector3>();
        for(int i = 0; i < detectNum * 2; i++)
        {
            float t = i / (detectNum * 2f -1f);
            float inc = Mathf.Acos(1f - 2f * t);
            float z = Mathf.Cos(inc);
            if(z > 0)
            {
                float az = 2f * Mathf.PI * 1.618f * i;
                float x = Mathf.Sin(inc) * Mathf.Cos(az);
                float y = Mathf.Sin(inc) * Mathf.Sin(az);
                res.Add(detectDistance * new Vector3(x, y, z));
            }
        }
        return res;
    }

    public override double[] GetInputs()
    {
        for(int i = 0; i < detectNum; i++)
        {
            lrList[i].gameObject.SetActive(bDrawLine);
            lrList[i].SetPosition(0, transform.position);
            if (Physics.Raycast(transform.position, transform.position, out RaycastHit hitInfo, detectDistance))
            {
                float w = Mathf.Pow(1 - hitInfo.distance / detectDistance, 2);
                lrList[i].SetPosition(1, hitInfo.point);
                if (hitInfo.transform.CompareTag("border"))
                {
                    inputList[3 * i] = w;
                    inputList[3 * i + 1] = 0;
                    inputList[3 * i + 2] = 0;
                }
                else if (hitInfo.transform.CompareTag(foodTag))
                {
                    inputList[3 * i] = 0;
                    inputList[3 * i + 1] = w;
                    inputList[3 * i + 2] = 0;
                }
                else if (hitInfo.transform.CompareTag(curTag))
                {
                    inputList[3 * i] = 0;
                    inputList[3 * i + 1] = 0;
                    inputList[3 * i + 2] = w;
                }
            }
            else
            {
                inputList[i] = 0;
                lrList[i].SetPosition(1, transform.position + rayPosList[i]);
                inputList[3 * i] = 0;
                inputList[3 * i + 1] = 0;
                inputList[3 * i + 2] = 0;
            }
        }
        inputList[detectNum] = curEnergy / maxEnergy;
        inputList[detectNum + 1] = rb.velocity.sqrMagnitude / (maxSpeed * maxSpeed);
        return inputList;
    }

    public override void UseOutputs(double[] outputs)
    {
        float x = (float)(outputs[0] * maxSpeed);
        float y = (float)(outputs[1] * maxSpeed);
        float z = (float)(outputs[2] * maxSpeed);

        Vector3 curVelocity = new Vector3(x, y, z);
        rb.velocity = Vector3.RotateTowards(rb.velocity, curVelocity, maxTurnSpeed * Time.deltaTime, maxAcceleration * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    public override void UpdateFitness()
    {
        chro.fitness = fit;
        if(bestShark == null || fit > bestShark.fit)
        {
            if (bestShark != null)
                bestShark.bDrawLine = false;
            bestShark = this;
            bestShark.bDrawLine = true;
        }
    }

    public override void RST()
    {
        fit = 0;
        rb.velocity = Vector3.zero;
        if(bestShark == this)
        {
            bestShark = null;
            bDrawLine = false;
        }
        transform.position = new Vector3(Random.Range(-initialXRange, initialXRange), Random.Range(0, initialYRange), Random.Range(-initialZRange, initialZRange));
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(foodTag))
        {
            fit += 10;
        }
        if (other.CompareTag("border"))
        {
            float f = fit;
            RST();
            fit = f - 30; 
        }
    }

}
