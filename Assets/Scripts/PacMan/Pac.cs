using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pac : EvolutionPlayer
{
    public float detectAngle;
    public int detectNum;
    public float detectDistance;

    public float maxSpeed;
    public float maxRotateSpeed;

    public Rigidbody rb;
    public Material mat;

    public bool isDrawLine;
    public List<LineRenderer> lineList = new List<LineRenderer>();

    public List<double> inputList = new List<double>();
    public float fitness;

    public float lifeTime;
    float t;

    public static Pac bestOne;


    private void Start()
    {
        mat = new Material(Shader.Find("Standard"));
        GetComponent<MeshRenderer>().material = mat;
        mat.color = new Color(Random.value, Random.value, Random.value);

        for(int i = 0; i < detectNum; i++)
        {
            LineRenderer lr = new GameObject(i.ToString()).AddComponent<LineRenderer>();
            lr.transform.SetParent(transform);
            lr.transform.localPosition = Vector3.zero;
            lr.transform.rotation = Quaternion.identity;
            lr.startWidth = 0.06f;
            lr.endWidth = 0.06f;
            lr.material = mat;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
            lineList.Add(lr);
        }

    }

    public override double[] GetInputs()
    {
        float dert = -detectNum / 2 * detectAngle;
        inputList.Clear();
        for(int i = 0; i < detectNum; i++)
        {
            lineList[i].gameObject.SetActive(isDrawLine);
            lineList[i].SetPosition(0, transform.position);

            Vector3 dir = Quaternion.AngleAxis(dert + i * detectAngle, transform.up) * transform.forward;
            if(Physics.Raycast(transform.position,dir,out RaycastHit hitInfo, detectDistance))
            {
                float input = Mathf.Pow(1 - hitInfo.distance / detectDistance, 2);
                if (hitInfo.transform.CompareTag("Pac"))
                {
                    inputList.Add(input);
                    inputList.Add(0);
                    inputList.Add(0);
                }
                else if (hitInfo.transform.CompareTag("Obstacle"))
                {
                    inputList.Add(0);
                    inputList.Add(input);
                    inputList.Add(0);
                }
                else if (hitInfo.transform.CompareTag("Food"))
                {
                    inputList.Add(0);
                    inputList.Add(0);
                    inputList.Add(input);
                }
            }
            else
            {
                inputList.Add(0);
                inputList.Add(0);
                inputList.Add(0);
                lineList[i].SetPosition(1, transform.position + dir * detectDistance);
            }
        }
        return inputList.ToArray();
    }

    public override void UseOutputs(double[] outputs)
    {
        rb.velocity = transform.forward * (float)outputs[0] * maxSpeed;
        transform.Rotate(0, (float)outputs[1] * maxSpeed * Time.deltaTime, 0);
    }

    public override void UpdateFitness()
    {
        chro.fitness = fitness;

        if(bestOne == null || bestOne.fitness < fitness)
        {
            if (bestOne != null)
            {
                bestOne.isDrawLine = false;
            }
            bestOne = this;
            isDrawLine = true;
        }
    }

    public override void RST()
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(Random.Range(-20f, 20), 0, Random.Range(-20f, 20));
        fitness = 0;
        t = 0;
    }

    protected override void Update()
    {
        base.Update();
        t += Time.deltaTime;
        if(t > lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

}
