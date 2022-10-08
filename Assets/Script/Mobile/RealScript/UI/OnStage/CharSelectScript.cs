using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectScript : MonoBehaviour
{
    string CharNum, realCharName;
    Slider ch1,ch2,ch3;
    Image ch1Image, ch2Image, ch3Image;
    Sprite meleeImg, BowImg, HealImg;

    private void Awake()
    {
        // 캐릭터들의 HP바
        ch1 = GameObject.Find("ch1").transform.Find("HPSlider").GetComponent<Slider>();
        ch2 = GameObject.Find("ch2").transform.Find("HPSlider").GetComponent<Slider>();
        ch3 = GameObject.Find("ch3").transform.Find("HPSlider").GetComponent<Slider>();
        // 캐릭터들의 이미지
        ch1Image = GameObject.Find("ch1").transform.Find("ch1Image").GetComponent<Image>();
        ch2Image = GameObject.Find("ch2").transform.Find("ch2Image").GetComponent<Image>();
        ch3Image = GameObject.Find("ch3").transform.Find("ch3Image").GetComponent<Image>();
        // 
        meleeImg = Resources.Load<Sprite>("Icon/c1");
        BowImg = Resources.Load<Sprite>("Icon/c2");
        HealImg = Resources.Load<Sprite>("Icon/c3");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public string FindCharacter()
    {
        realCharName = PlayerManager.instance.playerTransform.name;
        CharNum = realCharName.Substring(realCharName.Length - 1, 1);
        return CharNum;
    }

    public void changeCharacterOne()
    {
        if (FindCharacter().Equals("1"))
        {

            // HP값
            ch1.value = (PlayerManager.instance.charOneScriptPublic.oneHP / PlayerManager.instance.charOneScriptPublic.maxHP);
            ch2.value = (PlayerManager.instance.charTwoScriptPublic.oneHP / PlayerManager.instance.charTwoScriptPublic.maxHP);
            ch3.value = (PlayerManager.instance.charThreeScriptPublic.oneHP / PlayerManager.instance.charThreeScriptPublic.maxHP);

            //이미지
            ch1Image.sprite = meleeImg;
            ch2Image.sprite = BowImg;
            ch3Image.sprite = HealImg;
        }
        else if (FindCharacter().Equals("2"))
        {
            ch1.value = (PlayerManager.instance.charTwoScriptPublic.oneHP / PlayerManager.instance.charTwoScriptPublic.maxHP);
            ch2.value = (PlayerManager.instance.charOneScriptPublic.oneHP / PlayerManager.instance.charOneScriptPublic.maxHP);
            ch3.value = (PlayerManager.instance.charThreeScriptPublic.oneHP / PlayerManager.instance.charThreeScriptPublic.maxHP);

            //이미지
            ch1Image.sprite = BowImg;
            ch2Image.sprite = meleeImg;
            ch3Image.sprite = HealImg;
        }
        else if (FindCharacter().Equals("3"))
        {
            ch1.value = (PlayerManager.instance.charThreeScriptPublic.oneHP / PlayerManager.instance.charThreeScriptPublic.maxHP);
            ch2.value = (PlayerManager.instance.charOneScriptPublic.oneHP / PlayerManager.instance.charOneScriptPublic.maxHP);
            ch3.value = (PlayerManager.instance.charTwoScriptPublic.oneHP / PlayerManager.instance.charTwoScriptPublic.maxHP);

            //이미지
            ch1Image.sprite = HealImg ;
            ch2Image.sprite = meleeImg;
            ch3Image.sprite = BowImg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update에서 실시간으로 현재체력을 HP바 value에 대입
        FindCharacter();
        changeCharacterOne();
    }
}
