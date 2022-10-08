using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharTwoUIScript : MonoBehaviour
{
    Image skillOneCoolTimeBG, skillTwoCoolTimeBG;
    TextMeshProUGUI skillOneCoolTimeText, skillTwoCoolTimeText;
    float cooltimestringMS, cooltimestringUlt;
    // Start is called before the first frame update
    void Start()
    {
        skillOneCoolTimeBG = transform.Find("Skill1CoolTime").GetComponent<Image>();
        skillOneCoolTimeText = transform.Find("Skill1CoolTimeText").GetComponent<TextMeshProUGUI>();
        skillTwoCoolTimeBG = transform.Find("Skill2CoolTime").GetComponent<Image>();
        skillTwoCoolTimeText = transform.Find("Skill2CoolTimeText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        cooltimestringMS = PlayerManager.instance.charTwoScriptPublic.multiShotCTText;
        cooltimestringUlt = PlayerManager.instance.charTwoScriptPublic.ultCoolTimeText;
        //Debug.Log(cooltimestringMS);
        //Debug.Log(cooltimestringUlt);
        if (PlayerManager.instance.charTwoScriptPublic.multiShotCTText <= 0.1f)
        {
            skillOneCoolTimeBG.enabled = false;
            skillOneCoolTimeText.enabled = false;
            skillOneCoolTimeBG.fillAmount = 1.0f;
        }
        else if (PlayerManager.instance.charTwoScriptPublic.multiShotTrigger)
        {
            skillOneCoolTimeBG.enabled = true;
            skillOneCoolTimeText.enabled = true;
            skillOneCoolTimeBG.fillAmount = (PlayerManager.instance.charTwoScriptPublic.multiShotCTText / 7.0f);
            skillOneCoolTimeText.text = cooltimestringMS.ToString().Substring(0, 3);
        }

        if (PlayerManager.instance.charTwoScriptPublic.ultTrigger)
        {
            skillTwoCoolTimeBG.enabled = true;
            skillTwoCoolTimeText.enabled = true;
            skillTwoCoolTimeBG.fillAmount = (PlayerManager.instance.charTwoScriptPublic.ultCoolTimeText / 15.0f);
            skillTwoCoolTimeText.text = cooltimestringUlt.ToString().Substring(0, 3);
        }
        else if (PlayerManager.instance.charTwoScriptPublic.ultCoolTimeText <= 0.1f)
        {
            skillTwoCoolTimeBG.enabled = false;
            skillTwoCoolTimeText.enabled = false;
            skillTwoCoolTimeBG.fillAmount = 1.0f;
        }
    }

    public void CharTwoAtk()
    {
        PlayerManager.instance.charTwoScriptPublic.attackCoolTime(PlayerManager.instance.charTwoScriptPublic.atkSpeed);
    }
    public void CharTwoSkillOne()
    {
        PlayerManager.instance.charTwoScriptPublic.CreateMultiShot();
    }
    public void CharTwoSkillTwo()
    {
        PlayerManager.instance.charTwoScriptPublic.Ult();
    }
}
