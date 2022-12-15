using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    float duration;
    [SerializeField] ParticleSystem smoke;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(smoke.main.duration);
        Destroy(gameObject, smoke.main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
