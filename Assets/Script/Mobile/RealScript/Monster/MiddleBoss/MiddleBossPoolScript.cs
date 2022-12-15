using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiddleBossPoolScript : MonoBehaviour
{
    public static MiddleBossPoolScript instance;
    GameObject MiddleBossResource, MiddleBoss;
    TextMeshProUGUI _meleeDamageText, _rangeDamageText, _magicDamageText;
    GameObject gameCanvas, HPbarBackGround;
    Slider MBHPbar;
    WarningEffectScript warningEffect;
    WarningTextScript warningText;
    MiddleBossScript _middleBossScript;
    bool _MiddleBossDieTrigger;

    public TextMeshProUGUI meleeDamageText
    {
        get { return _meleeDamageText; }
    }
    public TextMeshProUGUI rangeDamageText
    {
        get { return _rangeDamageText; }
    }
    public TextMeshProUGUI magicDamageText
    {
        get { return _magicDamageText; }
    }
    public MiddleBossScript middleBossScript
    {
        get { return _middleBossScript; }
    }
    public bool MiddleBossDieTrigger
    {
        get { return _MiddleBossDieTrigger; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        // 리소스 프리팹 폴더에서 몬스터 리소스를 로드
        MiddleBossResource = Resources.Load<GameObject>("Prefabs/Polyart_Golem");
        _meleeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProOrange");
        _rangeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProBlue");
        _magicDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProBlue");
        gameCanvas = GameObject.Find("Canvas");
        warningEffect = gameCanvas.transform.Find("warningEffect").GetComponent<WarningEffectScript>();
        warningText = gameCanvas.transform.Find("warningText").GetComponent<WarningTextScript>();
        HPbarBackGround = gameCanvas.transform.Find("MiddleBossHPBarBG").gameObject;
        _MiddleBossDieTrigger = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnMiddleBoss();
    }

    // Update is called once per frame
    void Update()
    {
        if(_middleBossScript.hp <= 0)
        {
            HPbarBackGround.SetActive(false);
            _MiddleBossDieTrigger = true;
        }
    }

    public void SpawnMiddleBoss()
    {
        MiddleBoss = GameObject.Instantiate(MiddleBossResource, transform.position, transform.rotation);
        MiddleBoss.transform.SetParent(transform);
        _middleBossScript = MiddleBoss.GetComponent<MiddleBossScript>();
        HPbarBackGround.SetActive(true);
        warningEffect.WarningEffect();
        warningText.WarningText();
        
    }
}
