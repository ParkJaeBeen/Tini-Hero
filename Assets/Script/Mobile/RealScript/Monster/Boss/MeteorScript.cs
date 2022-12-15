using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    [SerializeField] private GameObject ExEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Effect"))
        {
            ExplosionEffect();
            Destroy(gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Effect"))
        {
            ExplosionEffect(collision.GetContact(0).point);
            Destroy(gameObject);
        }
    }*/

    public void ExplosionEffect()
    {
        Instantiate(ExEffect, transform.position, Quaternion.identity);
    }

    public void ExplosionEffect(Vector3 _point)
    {
        Instantiate(ExEffect, _point, Quaternion.identity);
    }
}
