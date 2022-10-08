using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    float count, gameOverCount, dieCount;
    GameObject mobPool1, mobPool2, mobPool3, middleBossPool;
    MobPoolScript mobPool1Script, mobPool2Script, mobPool3Script;
    MiddleBossPoolScript middleBossPoolScript;
    Transform GameOverPanel;
    GameObject clearPopup, instanceClearPopup, gameCanvas;
    float clearTime;
    TextMeshProUGUI timeText;
    bool _isGameClear, _isGameOver;
    float min, sec;
    string minStr, secStr;
    Image grayStarTwo, grayStarThree;

    public bool isGameClear
    {
        get { return _isGameClear; }
    }
    public bool isGameOver
    {
        get { return _isGameOver; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        gameOverCount = 0.0f;

        mobPool1 = transform.Find("MonsterPool1").gameObject;
        mobPool2 = transform.Find("MonsterPool2").gameObject;
        mobPool3 = transform.Find("MonsterPool3").gameObject;
        middleBossPool = transform.Find("MiddleBossPool").gameObject;

        mobPool1Script = mobPool1.GetComponent<MobPoolScript>();
        mobPool2Script = mobPool2.GetComponent<MobPoolScript>();
        mobPool3Script = mobPool3.GetComponent<MobPoolScript>();
        middleBossPoolScript = middleBossPool.GetComponent<MiddleBossPoolScript>();

        gameCanvas = GameObject.Find("Canvas");
        GameOverPanel = gameCanvas.transform.Find("GameOverPanel");
        clearPopup = Resources.Load<GameObject>("Prefabs/clearPopup");
        /*mobPool1.SetActive(true);
        mobPool2.SetActive(true);
        mobPool3.SetActive(true);*/
        _isGameClear = false;
        _isGameOver = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameClear)
        {
            count += Time.deltaTime;
            // .5�ʸ��� �����±װ� �޸� ������Ʈ�� ���ӿ� �����ϴ��� Ž�� - �������� ���� �� ���� Ŭ������ �ǹ�
            if (count >= 0.5f)
            {
                searchMonster();
                count = 0;
            }
            // �����±װ� �޷��ִ� ������Ʈ�� �ϳ��� ������ = ������ Ŭ���� ������
            if (searchMonster() == null)
            {
                Debug.Log("no monster");
                gameOverCount += Time.deltaTime;
                Debug.Log(gameOverCount);
                if (gameOverCount >= 5.0f)
                {
                    clearTime = Time.time;
                    // Ÿ�ӽ������� 0�̸� ���� �Ͻ�������
                    Time.timeScale = 0f;
                    Debug.Log(clearTime);
                    Debug.Log("game set");
                    GameOverPanel.gameObject.SetActive(true);
                    GameClearPopup();
                    _isGameClear = true;
                }
            }
            if (mobPool1Script.spawnedMobCount.Equals(3) && mobPool2Script.spawnedMobCount.Equals(3) && mobPool3Script.spawnedMobCount.Equals(3))
            {
                Debug.Log("�߰�������ȯ");
                middleBossPool.SetActive(true);
            }

            if (middleBossPoolScript.MiddleBossDieTrigger)
            {
                dieCount += Time.deltaTime;
                if (dieCount >= 2.0f)
                {
                    mobPool1.SetActive(false);
                    mobPool2.SetActive(false);
                    mobPool3.SetActive(false);
                    middleBossPool.SetActive(false);
                }
            }
        }
    }

    public string searchMonster()
    {
        // �ݰ� 300 ���� ������Ʈ�� ������
        Collider[] colls = Physics.OverlapSphere(transform.position, 300.0f);

        // �ݺ������� �˻�
        for (int i = 0; i < colls.Length; i++)
        {
            Collider tmpColl = colls[i];
            // �����±׸� ���� ��� ������Ʈ�� ������ ���� (�� �±װ� �����ϴ� ������Ʈ�� ������ )
            if (tmpColl.gameObject.CompareTag("MonsterPool")||
                tmpColl.gameObject.CompareTag("Monster") ||
                tmpColl.gameObject.CompareTag("MonsterWeapon") ||
                tmpColl.gameObject.CompareTag("MiddleBossHit") ||
                tmpColl.gameObject.CompareTag("ShockWave"))
            {
                return "exist";
            }
        }
        return null;
    }

    public void GameClearPopup()
    {
        instanceClearPopup = GameObject.Instantiate(clearPopup, gameCanvas.transform.position, Quaternion.identity);
        instanceClearPopup.transform.SetParent(gameCanvas.transform);
        timeText = instanceClearPopup.transform.Find("timeText").GetComponent<TextMeshProUGUI>();
        grayStarTwo = instanceClearPopup.transform.Find("grayStarTwo").GetComponent<Image>();
        grayStarThree = instanceClearPopup.transform.Find("grayStarThree").GetComponent<Image>();
        if (clearTime >= 180.0f)
        {
            grayStarTwo.enabled = true;
            grayStarThree.enabled = true;
        }
        else if(clearTime >= 120.0f)
        {
            grayStarThree.enabled = true;
        }
        min = Mathf.Floor(clearTime / 60);
        sec = Mathf.RoundToInt(clearTime % 60);
        minStr = min.ToString();
        if(sec < 10)
        {
            secStr = "0"+ Mathf.RoundToInt(sec).ToString();
        }
        else
        {
            secStr = Mathf.RoundToInt(sec).ToString();
        }
        timeText.text = minStr+" : "+ secStr;
    }

}
