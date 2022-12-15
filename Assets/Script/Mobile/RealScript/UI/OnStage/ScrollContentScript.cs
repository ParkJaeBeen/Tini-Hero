using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContentScript : MonoBehaviour
{
    List<Button> FowardBTs, BackwardBTs;
    [SerializeField] private TextMeshProUGUI BodyText, WeaponText, HeadText, HairText, MouthText, EyeText, HelmetText, HeaddressText, MustacheText;
    CustomizeScript customizeScript;
    [SerializeField]
    private Image hairControl, hatControl;
    [SerializeField]
    private Button hairBT, hatBT;
    private void Awake()
    {
        FowardBTs = new List<Button>();
        BackwardBTs = new List<Button>();
        customizeScript = CustomizeScript.instance;
        hairControl.gameObject.SetActive(true);
        hatControl.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScrollStart");
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Panel"))
            {
                for (int j = 0; j < transform.GetChild(j).childCount; j++)
                {
                    Debug.Log(transform.GetChild(j).childCount);
                    if (transform.GetChild(i).GetChild(j).name.Contains("Foward"))
                    {
                        Debug.Log(transform.GetChild(i).GetChild(j).name);
                        FowardBTs.Add(transform.GetChild(i).GetChild(j).GetComponent<Button>());
                    }
                    else
                    {
                        Debug.Log("no contains");
                    }
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Panel"))
            {
                for (int j = 0; j < transform.GetChild(j).childCount; j++)
                {
                    if (transform.GetChild(i).GetChild(j).name.Contains("Backward"))
                    {
                        BackwardBTs.Add(transform.GetChild(i).GetChild(j).GetComponent<Button>());
                    }
                }
            }
                
        }

        for(int i = 0; i < FowardBTs.Count; i++)
        {
            Debug.Log("add");
            int a = i;
            FowardBTs[a].onClick.AddListener(() => FowardBTClick(a+1));
        }

        for (int i = 0; i < BackwardBTs.Count; i++)
        {
            int a = i;
            BackwardBTs[a].onClick.AddListener(() => BackwardBTClick(a + 1));
        }
    }

    public void FowardBTClick(int listNum)
    {
        switch (listNum)
        {
            case 1:
                customizeScript.ChangeActiveObjFoward(customizeScript.headList);
                HeadText.text = customizeScript.FindActiveObj(customizeScript.headList).name;
                break;
            case 2:
                customizeScript.ChangeActiveObjFoward(customizeScript.hairList);
                HairText.text = customizeScript.FindActiveObj(customizeScript.hairList).name;
                break;
            case 3:
                customizeScript.ChangeActiveObjFoward(customizeScript.eyeList);
                EyeText.text = customizeScript.FindActiveObj(customizeScript.eyeList).name;
                break;
            case 4:
                customizeScript.ChangeActiveObjFoward(customizeScript.mouthList);
                MouthText.text = customizeScript.FindActiveObj(customizeScript.mouthList).name;
                break;
            case 5:
                customizeScript.ChangeActiveObjFoward(customizeScript.hatList);
                HelmetText.text = customizeScript.FindActiveObj(customizeScript.hatList).name;
                break;
            case 6:
                customizeScript.ChangeActiveObjFoward(customizeScript.headdressList);
                HeaddressText.text = customizeScript.FindActiveObj(customizeScript.headdressList).name;
                break;
            case 7:
                customizeScript.ChangeActiveObjFoward(customizeScript.mustacheList);
                MustacheText.text = customizeScript.FindActiveObj(customizeScript.mustacheList).name;
                break;
            case 8:
                customizeScript.ChangeActiveObjFoward(customizeScript.bodyList);
                BodyText.text = customizeScript.FindActiveObj(customizeScript.bodyList).name;
                break;
            case 9:
                customizeScript.ChangeActiveObjFoward(customizeScript.WeaponList);
                WeaponText.text = customizeScript.FindActiveObj(customizeScript.WeaponList).name;
                break;
        }
    }

    public void BackwardBTClick(int listNum)
    {
        switch (listNum)
        {
            case 1:
                customizeScript.ChangeActiveObjBackward(customizeScript.headList);
                HeadText.text = customizeScript.FindActiveObj(customizeScript.headList).name;
                break;
            case 2:
                customizeScript.ChangeActiveObjBackward(customizeScript.hairList);
                HairText.text = customizeScript.FindActiveObj(customizeScript.hairList).name;
                break;
            case 3:
                customizeScript.ChangeActiveObjBackward(customizeScript.eyeList);
                EyeText.text = customizeScript.FindActiveObj(customizeScript.eyeList).name;
                break;
            case 4:
                customizeScript.ChangeActiveObjBackward(customizeScript.mouthList);
                MouthText.text = customizeScript.FindActiveObj(customizeScript.mouthList).name;
                break;
            case 5:
                customizeScript.ChangeActiveObjBackward(customizeScript.hatList);
                HelmetText.text = customizeScript.FindActiveObj(customizeScript.hatList).name;
                break;
            case 6:
                customizeScript.ChangeActiveObjBackward(customizeScript.headdressList);
                HeaddressText.text = customizeScript.FindActiveObj(customizeScript.headdressList).name;
                break;
            case 7:
                customizeScript.ChangeActiveObjBackward(customizeScript.mustacheList);
                MustacheText.text = customizeScript.FindActiveObj(customizeScript.mustacheList).name;
                break;
            case 8:
                customizeScript.ChangeActiveObjBackward(customizeScript.bodyList);
                BodyText.text = customizeScript.FindActiveObj(customizeScript.bodyList).name;
                break;
            case 9:
                customizeScript.ChangeActiveObjBackward(customizeScript.WeaponList);
                WeaponText.text = customizeScript.FindActiveObj(customizeScript.WeaponList).name;
                break;
        }
    }

    public void SelectHat()
    {
        hatBT.enabled = false;
        hairBT.enabled = true;
        hairControl.gameObject.SetActive(false);
        hatControl.gameObject.SetActive(true);
        customizeScript.hairList
            [customizeScript.hairList.IndexOf(customizeScript.FindActiveObj(customizeScript.hairList))].gameObject.SetActive(false);
        customizeScript.hatList[0].gameObject.SetActive(true);
    }

    public void SelectHair()
    {
        hatBT.enabled = true;
        hairBT.enabled = false;
        hairControl.gameObject.SetActive(true);
        hatControl.gameObject.SetActive(false);
        customizeScript.hatList
            [customizeScript.hatList.IndexOf(customizeScript.FindActiveObj(customizeScript.hatList))].gameObject.SetActive(false);
        customizeScript.hairList[0].gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
