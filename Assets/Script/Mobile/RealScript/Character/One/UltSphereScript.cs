using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltSphereScript : MonoBehaviour
{
    SphereCollider sphereCollider;
    CharOneScript charOneScript;
    float _ultOP;
    public float ultOP
    {
        get { return _ultOP; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _ultOP = 15;
        charOneScript = GetComponentInParent<CharOneScript>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UltColliderDisable()
    {
        StopCoroutine(UltColliderDisableC());
        StartCoroutine(UltColliderDisableC());
    }

    IEnumerator UltColliderDisableC()
    {
        for(int i = 0; i< 7; i++)
        {
            sphereCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            sphereCollider.enabled = false;
            yield return new WaitForSeconds(0.4f);
        }
        charOneScript.charOneAniPub.SetInteger("aniInt", 0);
    }
}
