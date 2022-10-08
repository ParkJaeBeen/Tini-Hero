using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHealEffectScript : MonoBehaviour
{
    CharThreeScript charThreeScript;
    Transform healTarget;
    // Start is called before the first frame update
    void Start()
    {
        charThreeScript = FindObjectOfType<CharThreeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.instance.charOneScriptPublic.transform == charThreeScript.HealTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerManager.instance.charOneScriptPublic.transform.position, 10.0f * Time.deltaTime);
        }
        else if (PlayerManager.instance.charTwoScriptPublic.transform == charThreeScript.HealTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerManager.instance.charTwoScriptPublic.transform.position, 10.0f * Time.deltaTime);
        }
        else if (PlayerManager.instance.charThreeScriptPublic.transform == charThreeScript.HealTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerManager.instance.charThreeScriptPublic.transform.position, 10.0f * Time.deltaTime);
        }
    }
}
