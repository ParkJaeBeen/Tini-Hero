using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobPoolScript : MonoBehaviour
{
    public static MobPoolScript instance;
    GameObject MonsterResource, Monster;
    // 각 풀에 소환되는 몬스터를 저장하는 리스트
    List<GameObject> MonsterPoolOneList, MonsterPoolTwoList, MonsterPoolThreeList;
    // 몬스터 소환 시간
    float monsterSpawnCount;
    // 몬스터 소환 개수
    int MobPoolCount, _spawnedMobCount;
    // 데미지 텍스트 리소스
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
        // 리소스 프리팹 폴더에서 몬스터 리소스를 로드
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
            // 리스폰 시간을 증가시킴
            monsterSpawnCount += Time.deltaTime;
            // 몬스터 소환 쿨타임
            if (monsterSpawnCount >= 10.0f)
            {
                SpawnMonster();
                monsterSpawnCount = 0;
            }
        }
        else if (_spawnedMobCount == 3)
        {
            Debug.Log("풀 사라짐 - 2번소환");
            //Debug.Log("중간보스 등장");
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
        // 몬스터 인스턴스화 - 인스턴스화된 몬스터는 각각의 풀 위치에 생성
        Monster = GameObject.Instantiate(MonsterResource, transform.position, Quaternion.identity);
        Monster.transform.SetParent(transform);
        Monster.gameObject.SetActive(true);

        // 몬스터풀1,2,3이 모두 이 스크립트를 들고있기에 transform 의 이름이 각각 다름 - 이름에 따라 분기를 줌
        if (transform.name.Equals("MonsterPool1"))
        {
            // 몬스터의 이름을 MobPool1,2,3Monster_ 형식으로 만들고 숫자를 순차적으로 넣어줌
            Monster.transform.name = "MobPool1Monster_" + MobPoolCount.ToString();
            // 리스트에 저장
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

        // 1부터 순차적으로 올려야 다음 몬스터가 인스턴스화 될 때 다른 이름을 가질 수 있음
        MobPoolCount++;
        // 스폰될때마다 하나씩 증가 - 15가 되면 풀 비활성화
        _spawnedMobCount++;
    }

    /*public float getDistanceToTarget()
    {
        return Vector3.Distance(transform.position, playerGO.transform.position);
    }*/
}
