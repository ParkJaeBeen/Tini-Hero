using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBHitScript : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    MiddleBossScript middleBossScript;
    float _mbOP;

    public float mbOP
    {
        get { return _mbOP; }
        set { _mbOP = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        middleBossScript = GetComponentInParent<MiddleBossScript>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        _mbOP = middleBossScript.op;
    }

    public void Attack()
    {
        StopCoroutine(attackCoroutine());
        StartCoroutine(attackCoroutine());
    }

    IEnumerator attackCoroutine()
    {
        yield return new WaitForSeconds(0.9f);
        capsuleCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        capsuleCollider.enabled = false;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ÇÃ·¹ÀÌ¾î¿¡ ´ê¾ÑÀ»¶§ false");
            capsuleCollider.enabled = false;
        }

    }*/
}
