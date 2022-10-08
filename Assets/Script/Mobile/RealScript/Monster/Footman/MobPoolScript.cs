using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobPoolScript : MonoBehaviour
{
    public static MobPoolScript instance;
    GameObject MonsterResource, Monster;
    // �� Ǯ�� ��ȯ�Ǵ� ���͸� �����ϴ� ����Ʈ
    List<GameObject> MonsterPoolOneList, MonsterPoolTwoList, MonsterPoolThreeList;
    // ���� ��ȯ �ð�
    float monsterSpawnCount;
    // ���� ��ȯ ����
    int MobPoolCount, _spawnedMobCount;
    // ������ �ؽ�Ʈ ���ҽ�
    TextMeshProUGUI _meleeDamageText, _rangeDamageText, _magicDamageText;

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
    public int spawnedMobCount
    {
        get { return _spawnedMobCount; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        MonsterPoolOneList = new List<GameObject>();
        MonsterPoolTwoList = new List<GameObject>();
        MonsterPoolThreeList = new List<GameObject>();
        // ���ҽ� ������ �������� ���� ���ҽ��� �ε�
        MonsterResource = Resources.Load<GameObject>("Prefabs/FootmanPolyart");
        _meleeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProOrange");
        _rangeDamageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextProBlue");
        //Debug.Log(MonsterResource);
        MobPoolCount = 1;
    }
    void Start()
    {
        monsterSpawnCount = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnedMobCount < 3)
        {
            // ������ �ð��� ������Ŵ
            monsterSpawnCount += Time.deltaTime;
            // ���� ��ȯ ��Ÿ��
            if (monsterSpawnCount >= 10.0f)
            {
                SpawnMonster();
                monsterSpawnCount = 0;
            }
        }
        else if (_spawnedMobCount == 3)
        {
            Debug.Log("Ǯ ����� - 2����ȯ");
            //Debug.Log("�߰����� ����");
            //transform.gameObject.SetActive(false);
        }

        if(transform.childCount == 0)
        {
            MonsterPoolOneList.Clear();
        }
        Debug.Log(_spawnedMobCount);
    }

    public void SpawnMonster()
    {
        // ���� �ν��Ͻ�ȭ - �ν��Ͻ�ȭ�� ���ʹ� ������ Ǯ ��ġ�� ����
        Monster = GameObject.Instantiate(MonsterResource, transform.position, Quaternion.identity);
        Monster.transform.SetParent(transform);
        Monster.gameObject.SetActive(true);

        // ����Ǯ1,2,3�� ��� �� ��ũ��Ʈ�� ����ֱ⿡ transform �� �̸��� ���� �ٸ� - �̸��� ���� �б⸦ ��
        if (transform.name.Equals("MonsterPool1"))
        {
            // ������ �̸��� MobPool1,2,3Monster_ �������� ����� ���ڸ� ���������� �־���
            Monster.transform.name = "MobPool1Monster_" + MobPoolCount.ToString();
            // ����Ʈ�� ����
            MonsterPoolOneList.Add(Monster);
        }
        else if (transform.name.Equals("MonsterPool2"))
        {
            Monster.transform.name = "MobPool2Monster_" + MobPoolCount.ToString();
            MonsterPoolTwoList.Add(Monster);
        }
        else if (transform.name.Equals("MonsterPool3"))
        {
            Monster.transform.name = "MobPool3Monster_" + MobPoolCount.ToString();
            MonsterPoolThreeList.Add(Monster);
        }

        // 1���� ���������� �÷��� ���� ���Ͱ� �ν��Ͻ�ȭ �� �� �ٸ� �̸��� ���� �� ����
        MobPoolCount++;
        // �����ɶ����� �ϳ��� ���� - 15�� �Ǹ� Ǯ ��Ȱ��ȭ
        _spawnedMobCount++;
    }

    /*public float getDistanceToTarget()
    {
        return Vector3.Distance(transform.position, playerGO.transform.position);
    }*/
}
