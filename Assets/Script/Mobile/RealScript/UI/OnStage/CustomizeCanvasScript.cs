using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomizeCanvasScript : MonoBehaviour
{
    public static CustomizeCanvasScript instance;
    Camera mainCam;
    [SerializeField]
    private GameObject CustomizeUI, SelectBTPanel, ScrollContent;
    [SerializeField]
    private TextMeshProUGUI BodyText, WeaponText, HeadText, HairText, MouthText, EyeText, HelmetText, HeaddressText, MustacheText;
    
    CustomizeScript customizeScript;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        mainCam = Camera.main;
        customizeScript = CustomizeScript.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectOne()
    {
        CustomizeCameraScript camera = mainCam.GetComponent<CustomizeCameraScript>();
        camera.posNum = 1;
        customizeScript.SelectedChar(1);
        ChangeUIandText();
    }
    public void SelectTwo()
    {
        CustomizeCameraScript camera = mainCam.GetComponent<CustomizeCameraScript>();
        camera.posNum = 2;
        customizeScript.SelectedChar(2);
        ChangeUIandText();
    }
    public void SelectThree()
    {
        CustomizeCameraScript camera = mainCam.GetComponent<CustomizeCameraScript>();
        camera.posNum = 3;
        customizeScript.SelectedChar(3);
        ChangeUIandText();
    }
    public void SelectMain()
    {
        CustomizeCameraScript camera = mainCam.GetComponent<CustomizeCameraScript>();
        camera.posNum = 0;
        customizeScript.ClearList();
        CustomizeUI.gameObject.SetActive(false);
        SelectBTPanel.gameObject.SetActive(true);
    }

    public void ChangeUIandText()
    {
        CustomizeUI.gameObject.SetActive(true);
        SelectBTPanel.gameObject.SetActive(false);
        // 초기값이기에 무조건 바꿔줘야함
        BodyText.text = customizeScript.bodyList
            [customizeScript.bodyList.IndexOf(customizeScript.FindActiveObj(customizeScript.bodyList))].name;
        WeaponText.text = customizeScript.WeaponList
            [customizeScript.WeaponList.IndexOf(customizeScript.FindActiveObj(customizeScript.WeaponList))].name;
        HeadText.text = customizeScript.headList
            [customizeScript.headList.IndexOf(customizeScript.FindActiveObj(customizeScript.headList))].name;
    }

    

    public void LoadScene()
    {
        GameManagerScript.instance.sceneName = "MonsterStage";
        SceneManager.LoadScene("Loading");
    }
}
