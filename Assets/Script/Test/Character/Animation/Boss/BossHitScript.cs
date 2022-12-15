using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitScript : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    RedDragonScript redDragonScript;
    float _BossOP;

    public float BossOP
    {
        get { return _BossOP; }
        set { _BossOP = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        redDragonScript = GetComponentInParent<RedDragonScript>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        _BossOP = redDragonScript.op;
    }

    public void Attack()
    {
        //StopCoroutine(attackCoroutine());
        StartCoroutine(attackCoroutine());
    }

    IEnumerator attackCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        capsuleCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        capsuleCollider.enabled = false;
        RedDragonScript.instance.BossAni.SetInteger("aniInt", 0);
    }
}
