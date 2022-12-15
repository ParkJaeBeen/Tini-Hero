using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextScript : MonoBehaviour
{
    TextMeshProUGUI damageText;
    float count;
    // Start is called before the first frame update
    void Start()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up;
        count += Time.deltaTime;
        if (count >= 0.2f)
            damageText.fontSize -= 1;
    }
}
