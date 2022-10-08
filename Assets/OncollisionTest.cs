using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OncollisionTest : MonoBehaviour
{
    Animator pAni;
    // Start is called before the first frame update
    void Start()
    {
        pAni =  GetComponentInParent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "FootmanPolyart" && pAni.GetInteger("aniInt") == 2)
        {
            Debug.Log(collision.contactCount);
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FootmanPolyart" && pAni.GetInteger("aniInt") == 2)
        {
            Debug.Log(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
