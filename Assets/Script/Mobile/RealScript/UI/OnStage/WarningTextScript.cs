using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningTextScript : MonoBehaviour
{
    TextMeshProUGUI warningText;

    private void Awake()
    {
        warningText = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WarningText()
    {
        StopCoroutine(WarningCoroutine());
        StartCoroutine(WarningCoroutine());
    }

    IEnumerator WarningCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            warningText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            warningText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
