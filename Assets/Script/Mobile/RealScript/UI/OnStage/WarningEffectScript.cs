using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningEffectScript : MonoBehaviour
{
    Image warningEffect;
    // Start is called before the first frame update
    void Start()
    {
        warningEffect = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WarningEffect()
    {
        StopCoroutine(WarningCoroutine());
        StartCoroutine(WarningCoroutine());
    }

    IEnumerator WarningCoroutine()
    {
        for(int i = 0; i < 3; i++)
        {
            warningEffect.enabled = true;
            yield return new WaitForSeconds(0.5f);
            warningEffect.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
