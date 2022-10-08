using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MobCanvasScript : MonoBehaviour
{
    Slider mobHPSlider;
    public MobScriptTest mobScriptTest;
    // Start is called before the first frame update
    void Start()
    {
        mobHPSlider = transform.Find("MobHP").GetComponent<Slider>();
        mobScriptTest = GetComponentInParent<MobScriptTest>();
    }

    // Update is called once per frame
    void Update()
    {
        mobHPSlider.value = (mobScriptTest.hp / mobScriptTest.maxHP);
    }
}
