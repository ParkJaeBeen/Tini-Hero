using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiddleBossScript : MonoBehaviour
{
    // 중간보스 애니메이터컨트롤러
    Animator middleBossAni;
    // 기본 능력치                   어그로 바뀌는딜레이  도발쿨타임  충격파 재생성시간
    float HP, _maxHP, speed, attackDelay, targetDelay, tauntCount, shockWaveCount, shockWaveCoolTime;
    int minOP, maxOP, OP, _ShockWaveDamage, randomInt;
    // 캐릭터들 무기 공격력
    int TwoHandSwordOP;
    int ArrowOP;
    int MagicMissileOP;
    // 현재 지정된 캐릭터의 트랜스폼, 게임오브젝트
    Transform playerTransform;
    GameObject playerGO;
    // 물리, 마법, 원거리 데미지텍스트리소스
    TextMeshProUGUI meleeDamage, rangeDamage, magicDamage;
    // 캔버스와 데미지텍스트(실제로 캔버스에 그려지는 텍스트)
    GameObject gameCanvas, damageText;
    // 본체의 콜라이더 - 피격당할시에 사용
    CapsuleCollider MiddleBossCollider;
    // 공격무기, 궁극기 데미지 계산용 스크립트
    WeaponScript weaponScript;
    UltSphereScript ultSphereScript;
    ArrowScript arrowScript;
    MagicMissileScript magicMissileScript;
    // 공격이 두번이상 발동되지 않도록 하는 트리거, 도발이 걸렸는지 판단하는 트리거
    bool atkTrigger, tauntTrigger, SWTrigger;
    // 플레이어들의 리스트
    List<Transform> playerList;
    // 보스몹의 공격판정 스크립트
    MBHitScript mbHitScript;
    // 쇼크웨이브 리소스, 인스턴스
    GameObject ShockWaveResoucre, ShockWaveInstance;

    public int op
    {
        get { return OP; }
    }

    private void Awake()
    {
        playerList = new List<Transform>();
        ShockWaveResoucre = Resources.Load<GameObject>("Prefabs/ShockWave");
    }

    public float hp
    {
        get { return HP; }
        set { HP = value; }
    }
    public float maxHP
    {
        get { return _maxHP; }
    }

    public bool SWTriggerPublic
    {
        get { return SWTrigger; }
        set { SWTrigger = value; }
    }
    public int ShockWaveDamage
    {
        get { return _ShockWaveDamage; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 몬스터 어그로 시간 (처음에는 16초로 바로 어그로가 끌리게 초기화해줌)
        targetDelay = 16.0f;
        // 공격시 함수가 여러번 실행되지 않게 해주는 트리거
        atkTrigger = false;
        // 도발 트리거
        tauntTrigger = false;
        // 충격파 트리거
        SWTrigger = false;
        // 중간보스의 체력
        HP = 2000.0f;
        _maxHP = 2500.0f;
        // 충격파 쿨타임
        shockWaveCoolTime = 101.0f;
        // 몬스터의 이동속도
        speed = 2.0f;
        // 공격력
        minOP = 45;
        maxOP = 51;
        // 충격파 데미지
        _ShockWaveDamage = 35;

        // 애니컨트롤러
        middleBossAni = GetComponent<Animator>();
        // 본체의 콜라이더
        MiddleBossCollider = GetComponent<CapsuleCollider>();
        // 공격판정 컴포넌트 스크립트
        mbHitScript = GetComponentInChildren<MBHitScript>();
        playerTransform = PlayerManager.instance.playerTransform;
        meleeDamage = MiddleBossPoolScript.instance.meleeDamageText;
        rangeDamage = MiddleBossPoolScript.instance.rangeDamageText;
        magicDamage = MiddleBossPoolScript.instance.magicDamageText;
        gameCanvas = GameObject.Find("Canvas");
        for (int i = 0; i < PlayerManager.instance.playerTransforms.Count; i++)
        {
            playerList.Add(PlayerManager.instance.playerTransforms["Character_" + (i + 1)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        OP = Random.Range(minOP, maxOP);

        attackDelay += Time.deltaTime;
        targetDelay += Time.deltaTime;
        shockWaveCount += Time.deltaTime;
        shockWaveCoolTime += Time.deltaTime;
        randomInt = Random.Range(1, 4);

        // HP가 0 초과일때 - 살아있을때 / 이 안에 모든 움직임을 넣어줘야함
        if (HP > 0)
        {
            if (!tauntTrigger)
            {
                selectTarget(10.0f, randomInt);
            }
            else if (tauntTrigger)
            {
                // 도발당함
                Taunted();
                // 도발시간초 증가
                tauntCount += Time.deltaTime;
                // 5초후에 도발이 풀려야함
                if (tauntCount >= 15.0f)
                {
                    //Debug.Log(tauntCount);
                    // 타겟딜레이를 정해진 시간보다 많이 줌으로써 바로 어그로가 바뀌게
                    targetDelay = 10.5f;
                    tauntCount = 0;
                    tauntTrigger = false;
                }
            }

            if (playerGO == null)
            {
                searchTarget();
            }
            else if (playerGO != null && !SWTrigger)
            {
                if (getDistanceToTarget() < 3.0f && !SWTrigger)
                {
                    // 매개변수는 공격속도(@초에한번씩 공격)
                    attackCoolTime(2.0f);
                }
                else if (getDistanceToTarget() > 3.0f && !SWTrigger)
                {
                    moveToTarget();
                }
            }

            if((HP / maxHP) <= 0.5f)
            {
                if(shockWaveCoolTime >= 100.0f)
                {
                    SWTrigger = true;
                    middleBossAni.SetInteger("aniInt", 3);
                    shockWaveCoolTime = 0;
                }
            }


        }
        // HP가 0 이하일때 = 죽었을때
        else if (HP <= 0)
        {
            //Debug.Log("monster dead");
            MiddleBossCollider.enabled = false;
            middleBossAni.SetInteger("aniInt", 6);
            if (middleBossAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(this.gameObject, 1.5f);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // 무기가 닿았을 때 - 피격판정
        if (other.transform.CompareTag("Weapon"))
        //&& PlayerManager.instance.playerAniController.GetInteger("aniInt") == 2)
        {
            // 데미지 텍스트 띄우는코드 - 무기의 이름에 따라 저장해주는 데미지 값이 달라져야함
            if (other.name.Equals("THS07_Sword"))
            {
                weaponScript = other.GetComponent<WeaponScript>();
                TwoHandSwordOP = ((int)weaponScript.weaponOP);
                meleeDamage.GetComponent<TextMeshProUGUI>().text = TwoHandSwordOP.ToString();
                HP -= TwoHandSwordOP;
            }
            // 두손검전사의 궁극기오브젝트 콜라이더가 닿았을때
            else if (other.name.Equals("UltSphere"))
            {
                ultSphereScript = other.GetComponent<UltSphereScript>();
                TwoHandSwordOP = ((int)ultSphereScript.ultOP);
                meleeDamage.GetComponent<TextMeshProUGUI>().text = TwoHandSwordOP.ToString();
                HP -= TwoHandSwordOP;
            }
            damageText = GameObject.Instantiate(meleeDamage, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
                Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
        }
        // 화살이 닿았을 때
        else if (other.transform.CompareTag("Arrow"))
        {
            arrowScript = other.GetComponent<ArrowScript>();
            ArrowOP = ((int)arrowScript.arrowOP);
            PrintDamage(ArrowOP, "archer");
            HP -= ArrowOP;
        }
        // 매직미사일이 닿았을 때
        else if (other.transform.CompareTag("MagicMissile"))
        {
            magicMissileScript = other.GetComponent<MagicMissileScript>();
            MagicMissileOP = ((int)magicMissileScript.magicMissileOP);
            PrintDamage(MagicMissileOP, "Magician");
            Debug.Log("매직미사일 = "+MagicMissileOP);
            HP -= MagicMissileOP;
        }

        // 도발에 닿았을 때
        if (other.name.Equals("TauntSphere"))
        {
            tauntTrigger = true;
        }

    }

    public void PrintDamage(int _damage, string _name)
    {
        rangeDamage.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
        damageText = GameObject.Instantiate(rangeDamage, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
        damageText.transform.SetParent(gameCanvas.transform);
        if (_name.Equals("Magician"))
        {
            magicDamage.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
            damageText = GameObject.Instantiate(magicDamage, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
                Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
        }
    }

    public void searchTarget()
    {
        // 몬스터 주변의 오브젝트 불러오기
        Collider[] colls = Physics.OverlapSphere(transform.position, 300.0f);
        //Debug.Log("searching - " + colls[0].name);

        // 몬스터 주변에 오브젝트가 있으면..
        for (int i = 0; i < colls.Length; i++)
        {
            Collider tmpColl = colls[i];
            // 캐릭터가 맞으면, 타겟 설정
            if (tmpColl.gameObject.name.Equals(playerTransform.name))
            {
                // 타겟 오브젝트에 넣어주기
                playerGO = tmpColl.gameObject;
                //Debug.Log(playerGO);
                break;
            }
        }
    }

    // 매개변수 _time = 어그로 쿨타임, _targetNum = 타겟의 숫자(이름뒤숫자)
    public void selectTarget(float _time, int _targetNum)
    {
        if (targetDelay >= _time)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].name.Substring(playerList[i].name.Length - 1, 1) == _targetNum.ToString())
                {
                    //Debug.Log("target = " + playerList[i].gameObject.name);
                    playerGO = playerList[i].gameObject;
                }
            }
            targetDelay = 0;
        }
    }

    public void Taunted()
    {
        // 플레이어가 없으면 안되니까 리스트에서 이름으로 검사
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].name.Substring(playerList[i].name.Length - 1, 1) == 1.ToString())
            {
                // 1번으로 바꿔줌
                playerGO = playerList[i].gameObject;
                break;
            }
            else
            {
                Debug.Log("no character1");
            }
        }
    }

    public float getDistanceToTarget()
    {
        return Vector3.Distance(transform.position, playerGO.transform.position);
    }

    public void moveToTarget()
    {
        middleBossAni.SetInteger("aniInt", 1);
        if (middleBossAni.GetInteger("aniInt") == 1)
        {
            // 몬스터가 타겟을 바라보기
            transform.LookAt(playerGO.transform.position);
            // 몬스터를 타겟에 접근하기
            transform.position =
            Vector3.MoveTowards(transform.position, playerGO.transform.position, Time.deltaTime * speed);
        }
    }
    // 거리가 2.0f 이하일 때 발동
    public void attackCoolTime(float _ad)
    {
        // 매개변수 _ad 의 값보다 어택딜레이 값이 클 때 (어택딜레이는 업데이트에서 계속 상승 - 1초마다 1.0f씩)
        if (attackDelay >= _ad)
        {
            // 공격을 딱 한번 실행하기 위한 트리거
            atkTrigger = true;
            
            
            if (atkTrigger)
            {
                // 애니메이션 컨트롤 - 2는 공격
                middleBossAni.SetInteger("aniInt", 2);
                // 공격시 대상을 쳐다보고 공격하기 위한 lookAt 함수
                transform.LookAt(playerGO.transform);
                // use 함수를 통해 공격 콜라이더 활성화
                mbHitScript.Attack();
                // 트리거를 꺼주기 - 공격함수가 업데이트에서 두번이상 실행되지 않기 위함.
                atkTrigger = false;
            }
            // 어택딜레이를 초기화해줌 - 매개변수보다 작게 만들어서 조건을 타지 않기 위해
            attackDelay = 0;
        }
    }

    public void ShockWave()
    {
        if(shockWaveCount >= 0.7f)
        {
            Debug.Log("shock wave");
            instanceSW();
            shockWaveCount = 0;
        }
    }

    public void instanceSW()
    {
        ShockWaveInstance = GameObject.Instantiate(ShockWaveResoucre, transform.position, transform.rotation);
        ShockWaveInstance.transform.SetParent(transform);
        Destroy(ShockWaveInstance, 1.0f);
    }
}
