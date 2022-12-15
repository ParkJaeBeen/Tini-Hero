using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarBGScript : MonoBehaviour
{
    Slider HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = transform.Find("BossHP").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HP.value = (BossPoolScript.instance.redDragonScriptP.hp / BossPoolScript.instance.redDragonScriptP.maxHP);
    }
}
