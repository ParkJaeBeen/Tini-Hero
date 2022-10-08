using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowmanUltEffectScript : MonoBehaviour
{
    CharTwoScript charTwoScript;
    // Start is called before the first frame update
    void Start()
    {
        charTwoScript = GetComponentInParent<CharTwoScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, charTwoScript.transform.position, 10.0f * Time.deltaTime);
    }
}
