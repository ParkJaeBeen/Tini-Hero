using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileScript : MonoBehaviour
{
    CharThreeScript charThreeScript;
    float _magicMissileOP;
    public float magicMissileOP
    {
        get { return _magicMissileOP; }
    }
    // Start is called before the first frame update
    void Start()
    {
        charThreeScript = PlayerManager.instance.charThreeScriptPublic;
    }

    // Update is called once per frame
    void Update()
    {
        _magicMissileOP = charThreeScript.attackPoint;
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }

        if (other.transform.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
