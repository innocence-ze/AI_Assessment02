using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : EvolutionPlayer
{

    public static Bird bestBird;

    public bool isAddForce;
    public float force;
    public float startPosX;
    public float detectDistance;
    public float maxFlyRange;
    public float minFlyRange;


    public RuntimeAnimatorController[] anims;
    public Sprite[] sprites;

    public float output;

    public bool isAlive = true;
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    double[] inputList;

    public float fit;
    [HideInInspector]
    public int score;

    float allX, allY;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        fit = 0;
        score = 0;

        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        var index = Random.Range(0, anims.Length);

        sr.sprite = sprites[index];
        anim.runtimeAnimatorController = anims[index];

        anim.SetBool("IsAlive", isAlive);
        transform.position = new Vector3(startPosX, Random.Range(minFlyRange, maxFlyRange), 0);

        rb = gameObject.GetComponent<Rigidbody2D>();

        inputList = new double[2];

        allY = maxFlyRange - minFlyRange;
        allX = GameManager.Singleton.pipeInitialX - startPosX;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isAlive)
            return;
        base.Update();
        if (isAddForce)
        {
            AddForce();
        }
        fit += Time.deltaTime;
        if(transform.position.y > maxFlyRange || transform.position.y < minFlyRange)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        anim.SetBool("IsAlive", isAlive);
        anim.runtimeAnimatorController = null;
        sr.sprite = null;
    }

    void AddForce()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        isAddForce = false;
    }

    public void Reagain()
    {
        isAlive = true;
        anim.SetBool("IsAlive", isAlive);
        transform.position = new Vector3(startPosX, Random.Range(minFlyRange, maxFlyRange), 0);

        int index = Random.Range(0, anims.Length);
        sr.sprite = sprites[index];
        anim.runtimeAnimatorController = anims[index];
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            score += 1;
            fit += 100;
        }
    }


    public override void UpdateFitness()
    {
        chro.fitness = fit;
        if(bestBird == null || fit > bestBird.fit)
        {
            bestBird = this;
        }
    }

    public override double[] GetInputs()
    {
        if(GameManager.Singleton.activePipeList.Count > 0)
        {
            Transform p = GameManager.Singleton.activePipeList[0].transform;
            if(p.position.x < transform.position.x)
            {
                p = GameManager.Singleton.activePipeList[1].transform;
            }

            Vector2 temp = p.position - transform.position;

            inputList[0] = Mathf.Pow((allX - temp.x) / allX, 2);
            inputList[1] = Mathf.Pow(temp.y / allY, 2);

        }

        return inputList;
    }

    public override void UseOutputs(double[] outputs)
    {
        output = (float)outputs[0];
    }

    public override void RST()
    {
        fit = 0;
        if (bestBird == this)
            bestBird = null;
        Reagain();
    }
}
