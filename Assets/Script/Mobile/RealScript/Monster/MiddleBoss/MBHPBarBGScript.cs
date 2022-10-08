using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MBHPBarBGScript : MonoBehaviour
{
    Slider HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = transform.Find("MBHP").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HP.value = (MiddleBossPoolScript.instance.middleBossScript.hp / MiddleBossPoolScript.instance.middleBossScript.maxHP);
    }
}
