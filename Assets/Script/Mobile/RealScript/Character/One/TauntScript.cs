using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntScript : MonoBehaviour
{
    SphereCollider tauntCollider;
    // Start is called before the first frame update
    void Start()
    {
        tauntCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useTaunt()
    {
        StopCoroutine(Taunt());
        StartCoroutine(Taunt());
    }

    IEnumerator Taunt()
    {
        Debug.Log("Taunted");
        tauntCollider.enabled = true;
        yield return new WaitForSeconds(0.25f);
        tauntCollider.enabled = false;
    }
}
