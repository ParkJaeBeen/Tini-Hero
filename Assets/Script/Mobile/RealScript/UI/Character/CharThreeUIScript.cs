using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharThreeUIScript : MonoBehaviour
{
    Transform healBG;
    Image skillOneCoolTimeBG, skillTwoCoolTimeBG, ch1HealCoolTime, ch2HealCoolTime, ch3HealCoolTime;
    TextMeshProUGUI skillOneCoolTimeText, skillTwoCoolTimeText, ch1HealCoolTimeText, ch2HealCoolTimeText, ch3HealCoolTimeText;
    float coolTimeHeal, coolTimeBuff, coolTimeUlt;
    // Start is called before the first frame update
    void Start()
    {
        skillOneCoolTimeBG = transform.Find("Skill1CoolTime").GetComponent<Image>();
        skillOneCoolTimeText = transform.Find("Skill1CoolTimeText").GetComponent<TextMeshProUGUI>();
        skillTwoCoolTimeBG = transform.Find("Skill2CoolTime").GetComponent<Image>();
        skillTwoCoolTimeText = transform.Find("Skill2CoolTimeText").GetComponent<TextMeshProUGUI>();

        healBG = transform.Find("healBG");

        ch1HealCoolTime = healBG.Find("ch1HealCoolTime").GetComponent<Image>();
        ch2HealCoolTime = healBG.Find("ch2HealCoolTime").GetComponent<Image>();
        ch3HealCoolTime = healBG.Find("ch3HealCoolTime").GetComponent<Image>();

        ch1HealCoolTimeText = healBG.Find("ch1HealCoolTimeText").GetComponent<TextMeshProUGUI>();
        ch2HealCoolTimeText = healBG.Find("ch2HealCoolTimeText").GetComponent<TextMeshProUGUI>();
        ch3HealCoolTimeText = healBG.Find("ch3HealCoolTimeText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        coolTimeHeal = PlayerManager.instance.charThreeScriptPublic.healCoolTimeText;
        coolTimeBuff = PlayerManager.instance.charThreeScriptPublic.buffCoolTimeText;
        coolTimeUlt = PlayerManager.instance.charThreeScriptPublic.ultCoolTimeText;
        //Debug.Log(coolTimeHeal + "," + coolTimeBuff + "," + coolTimeUlt);

        if (PlayerManager.instance.charThreeScriptPublic.healTrigger)
        {
            ch1HealCoolTime.enabled = true;
            ch2HealCoolTime.enabled = true;
            ch3HealCoolTime.enabled = true;

            ch1HealCoolTimeText.enabled = true;
            ch2HealCoolTimeText.enabled = true;
            ch3HealCoolTimeText.enabled = true;

            ch1HealCoolTime.fillAmount = (coolTimeHeal / 2.0f);
            ch1HealCoolTimeText.text = coolTimeHeal.ToString().Substring(0, 3);
            ch2HealCoolTime.fillAmount = (coolTimeHeal / 2.0f);
            ch2HealCoolTimeText.text = coolTimeHeal.ToString().Substring(0, 3);
            ch3HealCoolTime.fillAmount = (coolTimeHeal / 2.0f);
            ch3HealCoolTimeText.text = coolTimeHeal.ToString().Substring(0, 3);
        }
        else if(coolTimeHeal <= 0.02f)
        {
            ch1HealCoolTime.enabled = false;
            ch2HealCoolTime.enabled = false;
            ch3HealCoolTime.enabled = false;

            ch1HealCoolTimeText.enabled = false;
            ch2HealCoolTimeText.enabled = false;
            ch3HealCoolTimeText.enabled = false;

            ch1HealCoolTime.fillAmount = 1.0f;
            ch2HealCoolTime.fillAmount = 1.0f;
            ch3HealCoolTime.fillAmount = 1.0f;
        }

        if (PlayerManager.instance.charThreeScriptPublic.buffTrigger)
        {
            skillOneCoolTimeBG.enabled = true;
            skillOneCoolTimeText.enabled = true;
            skillOneCoolTimeBG.fillAmount = (coolTimeBuff / 15.0f);
            skillOneCoolTimeText.text = coolTimeBuff.ToString().Substring(0, 3);
        }
        else if(coolTimeBuff <= 0.02f)
        {
            skillOneCoolTimeBG.enabled = false;
            skillOneCoolTimeText.enabled = false;
            skillOneCoolTimeBG.fillAmount = 1.0f;
        }

        if (PlayerManager.instance.charThreeScriptPublic.ultTrigger)
        {
            skillTwoCoolTimeBG.enabled = true;
            skillTwoCoolTimeText.enabled = true;
            skillTwoCoolTimeBG.fillAmount = (coolTimeUlt / 20.0f);
            skillTwoCoolTimeText.text = coolTimeUlt.ToString().Substring(0, 3);
        }
        else if(coolTimeUlt <= 0.02f)
        {
            skillTwoCoolTimeBG.enabled = false;
            skillTwoCoolTimeText.enabled = false;
            skillTwoCoolTimeBG.fillAmount = 1.0f;
        }
    }

    public void CharThreeAtk()
    {
        PlayerManager.instance.charThreeScriptPublic.attackCoolTime(PlayerManager.instance.charThreeScriptPublic.atkSpeed);
    }

    public void CharThreeSkillOne()
    {
        PlayerManager.instance.charThreeScriptPublic.Buff();
    }
    public void CharThreeSkillTwo()
    {
        PlayerManager.instance.charThreeScriptPublic.Ult();
    }

    public void CharThreeHealOne()
    {
        PlayerManager.instance.charThreeScriptPublic.HealCoolTime(2.0f, 1);
    }
    public void CharThreeHealTwo()
    {
        PlayerManager.instance.charThreeScriptPublic.HealCoolTime(2.0f, 2);
    }
    public void CharThreeHealThree()
    {
        PlayerManager.instance.charThreeScriptPublic.HealCoolTime(2.0f, 3);
    }
}
