using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScript : MonoBehaviour
{
    float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, moveSpeed * Time.deltaTime);
    }
}
