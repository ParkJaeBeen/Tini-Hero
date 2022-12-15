using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManagerScript : MonoBehaviour
{
    public static MonsterManagerScript instance;
    GameObject mobPool1, mobPool2, mobPool3, middleBossPool;
    MobPoolScript mobPool1Script, mobPool2Script, mobPool3Script;
    MiddleBossPoolScript middleBossPoolScript;
    float finish;

    public float clearTime { get; private set; }
    public float StartTime { get; private set; }
    public float gameOverCount { get; private set; }
    public float count { get; private set; }
    public float dieCount { get; private set; }

    private void Awake()
    {
        finish = 5.2f;
        if (instance == null)
            instance = this;

        mobPool1 = transform.Find("MonsterPool1").gameObject;
        mobPool2 = transform.Find("MonsterPool2").gameObject;
        mobPool3 = transform.Find("MonsterPool3").gameObject;
        middleBossPool = transform.Find("MiddleBossPool").gameObject;

        mobPool1Script = mobPool1.GetComponent<MobPoolScript>();
        mobPool2Script = mobPool2.GetComponent<MobPoolScript>();
        mobPool3Script = mobPool3.GetComponent<MobPoolScript>();
        middleBossPoolScript = middleBossPool.GetComponent<MiddleBossPoolScript>();
        StartTime = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.instance._isGameClear)
        {
            Debug.Log(GameManagerScript.instance.isGameClear);
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
                gameOverCount += Time.deltaTime;
                if (gameOverCount >= 5.0f)
                {
                    GameManagerScript.instance._isGameClear = true;
                    Debug.Log(GameManagerScript.instance._isGameClear);
                    clearTime = Time.time - StartTime;
                    Debug.Log("game set");
                    WorldCanvasScript.instance.GameOverPanel.gameObject.SetActive(true);
                    if (finish >= 5.0f)
                    {
                        GameManagerScript.instance.GameClearPopup(WorldCanvasScript.instance.gameObject, WorldCanvasScript.instance.clearPopup, clearTime);
                        GameManagerScript.instance.sceneName = "BossStage";
                    }
                    finish = 0.0f;
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
            if (tmpColl.gameObject.CompareTag("MonsterPool") ||
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
}
