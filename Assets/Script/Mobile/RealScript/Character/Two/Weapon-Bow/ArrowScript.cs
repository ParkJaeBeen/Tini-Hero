using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    float _arrowOP;
    CharTwoScript charTwoScript;
    public float arrowOP
    {
        get { return _arrowOP; }
    }
    // Start is called before the first frame update
    void Start()
    {
        charTwoScript = PlayerManager.instance.charTwoScriptPublic;
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        _arrowOP = charTwoScript.attackPoint;
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
