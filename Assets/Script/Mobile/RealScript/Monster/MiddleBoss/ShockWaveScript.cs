using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{
    SphereCollider sphereCollider;
    GameObject instanceSW;
    MiddleBossScript middleBossScript;
    int _ShockWaveDamage;

    public int ShockWaveDamage
    {
        get { return _ShockWaveDamage; }
    }
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        middleBossScript = GetComponentInParent<MiddleBossScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sphereCollider.radius <= 4.0f)
        {
            sphereCollider.radius += 0.02f;
        }
        if(sphereCollider.radius >= 4.0f)
        {
            sphereCollider.radius = 0.1f;
        }
        _ShockWaveDamage = middleBossScript.ShockWaveDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("PlayerHit");
        }
    }


}
