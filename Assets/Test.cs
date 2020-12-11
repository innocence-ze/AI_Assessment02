using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInto3 = Physics2D.Raycast(transform.position, Vector2.right, 10, 1 << LayerMask.NameToLayer("DetectLayer"));
        Debug.Log(hitInto3.collider.gameObject.layer);
    }
}
