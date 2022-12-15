using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedDragonScript : MonoBehaviour
{
    public static RedDragonScript instance;
    // 최종보스 애니컨트롤러
    [SerializeField] Animator ani;
    [SerializeField] CapsuleCollider flamePointCollider;
    [SerializeField] GameObject flameThrow;
    [SerializeField] GameObject meteor;
    [SerializeField] public CapsuleCollider BossCollider;
    [SerializeField] public BossHitScript bossHitScript;
    Vector3 upperPos, lowerPos, pos1, pos2;
    float t = 0.0f;
    float HP, _maxHP, speed, attackDelay, targetDelay, tauntCount, FlameThrowCoolTime, meteorCoolTime;
    int minOP, maxOP, OP, _flameOP, _meteorOP,  randomInt;
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
    // 공격무기, 궁극기 데미지 계산용 스크립트
    WeaponScript weaponScript;
    UltSphereScript ultSphereScript;
    ArrowScript arrowScript;
    MagicMissileScript magicMissileScript;
    // 공격이 두번이상 발동되지 않도록 하는 트리거, 도발이 걸렸는지 판단하는 트리거
    bool atkTrigger, tauntTrigger, MeteorTrigger, FlameTrigger, IsInvincibility;
    // 플레이어들의 리스트
    List<Transform> playerList;

    public GameObject flameThrowP
    {
        get { return flameThrow; }
    }

    public bool FlameTriggerP
    {
        get { return FlameTrigger; }
        set { FlameTrigger = value; }
    }

    public bool MeteorTriggerP
    {
        get { return MeteorTrigger; }
        set { MeteorTrigger = value; }
    }

    public bool IsInvincibilityP
    {
        get { return IsInvincibility; }
        set { IsInvincibility = value; }
    }

    public Vector3 lowerPosP
    {
        get { return lowerPos; }
        set { lowerPos = value; }
    }

    public Vector3 upperPosP
    {
        get { return upperPos; }
        set { upperPos = value; }
    }
    public int op
    {
        get { return OP; }
    }

    public int flameOP
    {
        get { return _flameOP; }
    }

    public int meteorOP
    {
        get { return _meteorOP; }
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
    public float FlameThrowCoolTimeP
    {
        set { FlameThrowCoolTime = value; }
    }

    public Animator BossAni
    {
        get { return ani; }
        set { ani = value; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        playerList = new List<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        MeteorTrigger = false;
        FlameTrigger = false;
        IsInvincibility = false;
        // 몬스터 어그로 시간 (처음에는 16초로 바로 어그로가 끌리게 초기화해줌)
        targetDelay = 16.0f;
        FlameThrowCoolTime = 5.1f;
        meteorCoolTime = 100.1f;
        // 중간보스의 체력
        HP = 2300.0f;
        _maxHP = 2500.0f;
        // 몬스터의 이동속도
        speed = 2.0f;
        // 공격력
        minOP = 55;
        maxOP = 61;
        _meteorOP = 80;

        playerTransform = PlayerManager.instance.playerTransform;
        meleeDamage = BossPoolScript.instance.meleeDamageText;
        rangeDamage = BossPoolScript.instance.rangeDamageText;
        magicDamage = BossPoolScript.instance.magicDamageText;
        
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
        FlameThrowCoolTime += Time.deltaTime;
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
            else if (playerGO != null && !MeteorTrigger)
            {
                if (getDistanceToTarget() < 6.0f && FlameThrowCoolTime >= 5.0f )
                {
                    FlameTrigger = true;
                    transform.LookAt(playerTransform);
                    ani.SetInteger("aniInt", 4);
                }
                else if (getDistanceToTarget() < 4.0f && !FlameTrigger)
                {
                    Debug.Log("attack");
                    // 매개변수는 공격속도(@초에한번씩 공격)
                    attackCoolTime(2.0f);
                }
                else if (getDistanceToTarget() > 4.0f && !FlameTrigger)
                {
                    Debug.Log("move");
                    moveToTarget();
                }
            }

            

            if ((HP / maxHP) < 0.5f && !FlameTrigger)
            {
                if(meteorCoolTime >= 100.0f)
                {
                    upperPos = new Vector3(transform.position.x, transform.position.y + 15.0f, transform.position.z);
                    MeteorTrigger = true;
                    IsInvincibility = true;
                    ani.SetInteger("aniInt", 5);
                    BossCollider.enabled = false;
                    meteorCoolTime = 0.0f;
                }
            }


        }
        // HP가 0 이하일때 = 죽었을때
        else if (HP <= 0)
        {
            //Debug.Log("monster dead");
            BossCollider.enabled = false;
            ani.SetInteger("aniInt", 10);
            if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(this.gameObject, 1.5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInvincibility)
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
                Destroy(damageText, 0.5f);
            }
            // 화살이 닿았을 때
            else if (other.transform.CompareTag("Arrow"))
            {
                Debug.Log("arrow hit");
                arrowScript = other.GetComponent<ArrowScript>();
                ArrowOP = ((int)arrowScript.arrowOP);
                PrintDamage(ArrowOP, "archer");
                HP -= ArrowOP;
                Destroy(damageText, 0.5f);
            }
            // 매직미사일이 닿았을 때
            else if (other.transform.CompareTag("MagicMissile"))
            {
                magicMissileScript = other.GetComponent<MagicMissileScript>();
                MagicMissileOP = ((int)magicMissileScript.magicMissileOP);
                PrintDamage(MagicMissileOP, "Magic");
                Debug.Log("매직미사일 = " + MagicMissileOP);
                HP -= MagicMissileOP;
                Destroy(damageText, 0.5f);
            }

            // 도발에 닿았을 때
            if (other.name.Equals("TauntSphere"))
            {
                tauntTrigger = true;
            }
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
        ani.SetInteger("aniInt", 1);
        if (ani.GetInteger("aniInt") == 1)
        {
            // 몬스터가 타겟을 바라보기
            transform.LookAt(playerGO.transform.position);
            // 몬스터를 타겟에 접근하기
            transform.position =
            Vector3.MoveTowards(transform.position, playerGO.transform.position, Time.deltaTime * speed);
        }
    }

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
                ani.SetInteger("aniInt", 2);
                // 공격시 대상을 쳐다보고 공격하기 위한 lookAt 함수
                transform.LookAt(playerGO.transform);
                // use 함수를 통해 공격 콜라이더 활성화

                // 트리거를 꺼주기 - 공격함수가 업데이트에서 두번이상 실행되지 않기 위함.
                atkTrigger = false;
            }
            // 어택딜레이를 초기화해줌 - 매개변수보다 작게 만들어서 조건을 타지 않기 위해
            attackDelay = 0;
        }
    }

    public void RememberPos()
    {
        pos1 = transform.position;
        Debug.Log(pos1);
        pos2 = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10.0f);
        t = 0.0f;
    }

    public void BackAndForth()
    {
        t += 1.0f * Time.deltaTime;
        transform.position = new Vector3(pos1.x, pos1.y, Mathf.Lerp(pos1.z, pos2.z, t));
        transform.LookAt(pos2);
        
        if(t > 1.0f)
        {
            Vector3 tmpVec = pos1;
            pos1 = pos2;
            pos2 = tmpVec;
            t = 0.0f;
        }
    }

    public void startCor(IEnumerator Cor)
    {
        StartCoroutine(Cor);
    }

    public void stopCor(IEnumerator Cor)
    {
        StopCoroutine(Cor);
    }

    public IEnumerator flame()
    {
        _flameOP = Random.Range(35, 41);
        flameThrow.SetActive(true);
        for(int i = 0; i < 10; i++)
        {
            flamePointCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            flamePointCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator IncreaseYPos()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector3.MoveTowards(transform.position, upperPos, Time.deltaTime * 5.0f);
    }

    public IEnumerator DecreaseYPos()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector3.MoveTowards(transform.position, lowerPos, Time.deltaTime * 5.0f);
    }

    public IEnumerator MeteorStrike()
    {
        for(int i = 0; i < 20; i++)
        {
            CreateMeteor();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void CreateMeteor()
    {
        Instantiate(meteor, new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10)),Quaternion.identity);
    }
}
