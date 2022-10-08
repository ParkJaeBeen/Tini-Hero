using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobWeaponScript : MonoBehaviour
{
    public CapsuleCollider mobWeaponCollider;
    int _mobWeaponOP;
    MobScriptTest mobScriptTest;

    public int mobWeaponOP
    {
        get { return _mobWeaponOP; }
    }
    // Start is called before the first frame update
    void Start()
    {
        mobScriptTest = GetComponentInParent<MobScriptTest>();
        mobWeaponCollider = GetComponent<CapsuleCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        _mobWeaponOP = mobScriptTest.op;
    }

    public void use()
    {
        StopCoroutine(mobAttack());
        StartCoroutine(mobAttack());
    }

    IEnumerator mobAttack()
    {
        mobWeaponCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        mobWeaponCollider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mobWeaponCollider.enabled = false;
        }
            
    }

}
