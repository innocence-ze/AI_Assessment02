using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pac : Agent
{
    public float eyeAngle;
    public int eyeNum;
    public float seeDis;
    public float maxSpeed;
    public float maxTurnSpeed;
    public Rigidbody rb;
    public Material mat;
    public bool isDraw;
    public float fit;
    public float lifeTime;
    public float t;
    public List<LineRenderer> lineList = new List<LineRenderer>();
    public static Pac bestOne;

    void Start()
    {
        mat = new Material(Shader.Find("Standard"));
        GetComponent<MeshRenderer>().material = mat;
        mat.color = new Color(Random.value, Random.value, Random.value);

        for (int i = 0; i < eyeNum; i++)
        {
            LineRenderer lr = new GameObject("line" + i).AddComponent<LineRenderer>();
            lr.transform.SetParent(transform);
            lr.transform.localPosition = Vector3.zero;
            lr.transform.localRotation = Quaternion.identity;
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
        List<double> inputsList = new List<double>();
        float dert = -eyeNum / 2 * eyeAngle;
        for (int i = 0; i < eyeNum; i++)
        {
            lineList[i].gameObject.SetActive(isDraw);
            lineList[i].SetPosition(0, transform.position);
            Vector3 dir = Quaternion.AngleAxis(dert + i * eyeAngle, transform.up) * transform.forward;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, seeDis))
            {
                float pd = Mathf.Pow(1 - hitInfo.distance / seeDis, 2);
                switch (hitInfo.transform.tag)
                {
                    case "Pac":
                        {
                            inputsList.Add(pd);
                            inputsList.Add(0);
                            inputsList.Add(0);
                            break;
                        }
                    case "Obstacle":
                        {
                            inputsList.Add(0);
                            inputsList.Add(pd);
                            inputsList.Add(0);
                            break;
                        }
                    case "Food":
                        {
                            inputsList.Add(0);
                            inputsList.Add(0);
                            inputsList.Add(pd);
                            break;
                        }
                }
                lineList[i].SetPosition(1, hitInfo.point);
            }
            else
            {
                inputsList.Add(0);
                inputsList.Add(0);
                inputsList.Add(0);
                lineList[i].SetPosition(1, transform.position + dir * seeDis);
            }
        }
        return inputsList.ToArray();
    }

    public override void UseOutputs(double[] outputs)
    {
        rb.velocity = transform.forward * (float)outputs[0] * maxSpeed;
        transform.Rotate(0, (float)outputs[1] * maxTurnSpeed * Time.deltaTime, 0);
    }

    public override void UpdateFitness()
    {
        ge.fitness = fit;

        if (bestOne == null || bestOne.fit < fit)
        {
            if (bestOne != null)
            {
                bestOne.isDraw = false;
            }
            bestOne = this;
            bestOne.isDraw = true;
        }
    }

    public override void Reset()
    {
        if (!PacEvolutionManager.livePacList.Contains(this))
        {
            gameObject.SetActive(true);
            PacEvolutionManager.deadPacList.Remove(this);
            PacEvolutionManager.livePacList.Add(this);
        }

        transform.position = new Vector3(Random.Range(-20f, 20f), 1, Random.Range(-20f, 20f));
        transform.rotation = Quaternion.identity;
        fit = 0;
        t = 0;
    }

    public void Update()
    {
        t += Time.deltaTime;
        if (t > lifeTime)
        {
            gameObject.SetActive(false);
            PacEvolutionManager.deadPacList.Add(this);
            PacEvolutionManager.livePacList.Remove(this);
        }
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Obstacle") && t > 1)
        {
            fit -= 3;
            gameObject.SetActive(false);
            PacEvolutionManager.livePacList.Remove(this);
            PacEvolutionManager.deadPacList.Add(this);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Food"))
        {
            fit++;
            PacEvolutionManager.showFoodList.Remove(c.gameObject);
            PacEvolutionManager.hideFoodList.Add(c.gameObject);
            c.gameObject.SetActive(false);
        }
    }

}
