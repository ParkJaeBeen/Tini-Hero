using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPoolScript : MonoBehaviour
{
    public static BossPoolScript instance;
    TextMeshProUGUI _meleeDamageText, _rangeDamageText, _magicDamageText;
    GameObject gameCanvas, HPbarBackGround;
    [SerializeField] Slider BossHPbar;
    [SerializeField] RedDragonScript redDragonScript;
    bool _BossDieTrigger;
    private float count;
    private float gameOverCount;
    private float clearTime;
    private float StartTime;
    private float finish;

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
    public bool BossDieTrigger
    {
        get { return _BossDieTrigger; }
    }

    public RedDragonScript redDragonScriptP
    {
        get { return redDragonScript; }
    }
    private void Awake()
    {
        finish = 5.2f;
        if (instance == null)
            instance = this;
        // ���ҽ� ������ �������� ���� ���ҽ��� �ε�
        _meleeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProOrange");
        _rangeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProBlue");
        _magicDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProBlue");
        gameCanvas = GameObject.Find("Canvas");
        HPbarBackGround = gameCanvas.transform.Find("BossHPBarBG").gameObject;
        _BossDieTrigger = false;
        StartTime = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (redDragonScript.hp <= 0)
        {
            HPbarBackGround.SetActive(false);
            _BossDieTrigger = true;
        }

        if (!GameManagerScript.instance.isGameClear)
        {
            // �����±װ� �޷��ִ� ������Ʈ�� �ϳ��� ������ = ������ Ŭ���� ������
            if (_BossDieTrigger)
            {
                gameOverCount += Time.deltaTime;
                if (gameOverCount >= 5.0f)
                {
                    clearTime = Time.time - StartTime;
                    Debug.Log("game set");
                    WorldCanvasScript.instance.GameOverPanel.gameObject.SetActive(true);
                    if(finish >= 5.0f)
                    {
                        GameManagerScript.instance.GameClearPopup(WorldCanvasScript.instance.gameObject, WorldCanvasScript.instance.clearPopup, clearTime);
                    }
                    finish = 0.0f;
                    // ����Ŭ����
                    GameManagerScript.instance.isGameClear = true;
                }
            }
        }
    }

}
