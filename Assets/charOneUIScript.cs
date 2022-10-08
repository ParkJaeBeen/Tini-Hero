using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class charOneUIScript : MonoBehaviour
{
    Image skillOneCoolTimeBG, skillTwoCoolTimeBG;
    TextMeshProUGUI skillOneCoolTimeText, skillTwoCoolTimeText;
    string cooltimestringTaunt, cooltimestringUlt;
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
        cooltimestringTaunt = PlayerManager.instance.charOneScriptPublic.tauntCoolTimeText.ToString();
        cooltimestringUlt = PlayerManager.instance.charOneScriptPublic.ultCoolTimeText.ToString();
        
        if(PlayerManager.instance.charOneScriptPublic.tauntCoolTimeText <= 0.1f)
        {
            skillOneCoolTimeBG.enabled = false;
            skillOneCoolTimeText.enabled = false;
            skillOneCoolTimeBG.fillAmount = 1.0f;
        }
        else if(PlayerManager.instance.charOneScriptPublic.tauntTrigger)
        {
            skillOneCoolTimeBG.enabled = true;
            skillOneCoolTimeText.enabled = true;
            skillOneCoolTimeBG.fillAmount = (PlayerManager.instance.charOneScriptPublic.tauntCoolTimeText / 15.0f);
            skillOneCoolTimeText.text = cooltimestringTaunt.Substring(0, 3);
        }

        if (PlayerManager.instance.charOneScriptPublic.ultTrigger)
        {
            skillTwoCoolTimeBG.enabled = true;
            skillTwoCoolTimeText.enabled = true;
            skillTwoCoolTimeBG.fillAmount = (PlayerManager.instance.charOneScriptPublic.ultCoolTimeText / 25.0f);
            skillTwoCoolTimeText.text = cooltimestringUlt.Substring(0, 3);
        }
        else if (PlayerManager.instance.charOneScriptPublic.ultCoolTimeText <= 0.1f)
        {
            skillTwoCoolTimeBG.enabled = false;
            skillTwoCoolTimeText.enabled = false;
            skillTwoCoolTimeBG.fillAmount = 1.0f;
        }
    }

    public void CharOneAtk()
    {
        if (!PlayerManager.instance.COneUltTrigger)
        {
            //Debug.Log(PlayerManager.instance.charOneScriptPublic.ultTrigger);
            PlayerManager.instance.charOneScriptPublic.attackCoolTime(PlayerManager.instance.charOneScriptPublic.atkSpeed);
        }
    }

    public void CharOneSkillOne()
    {
        PlayerManager.instance.charOneScriptPublic.Taunt();
    }

    public void CharOneSkillTwo()
    {
        PlayerManager.instance.charOneScriptPublic.useUlt();
    }
}
