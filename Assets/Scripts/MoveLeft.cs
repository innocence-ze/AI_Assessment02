using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;

    public bool isMoveChildren;

    Transform[] children;

    // Start is called before the first frame update
    void Start()
    {
        children = new Transform[transform.childCount];
        for(int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveChildren)
        {
            foreach(var c in children)
            {
                c.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
