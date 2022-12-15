using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharOneScript : MonoBehaviour
{
    private float _attackPoint;
    Vector3 randomVec;
    WeaponScript weaponScript;
    TauntScript tauntScript;
    UltSphereScript ultSphereScript;
    UltEffectOBJScript ultEffectOBJScript;
    MobWeaponScript mobWeaponScript;
    MBHitScript mbHitScript;
    BossHitScript bossHitScript;
    RedDragonScript redDragonScript;
    ShockWaveScript shockWaveScript;
    float randomTime, _moveSpeed, _minOp, _maxOp, _atkSpeed, hp, _maxHP, dp, mp, _atkAniSpeed, _atkCorSpeed;
    float _atkCoolTime, tauntCoolTime, ultCoolTime;
    float _tauntCoolTimeText, _ultCoolTimeText;
    float mobDistance;
    int mobOP;
    int middleBossOP;
    int BossOP;
    int meteorOP;
    int FlameOP;
    int ShockWaveD;
    TextMeshProUGUI damagePro;
    GameObject damageText, tauntEffect, instanceTauntEffect, ultEffect, instanceUltEffect;
    public GameObject gameCanvas;
    Animator charOneAni;
    GameObject Monster, instanceStunText;
    List<GameObject> monsterList;
    List<GameObject> distinctMobList;
    bool StunTrigger;
    bool _tauntTrigger;
    bool _ultTrigger;


    public float attackPoint
    {
        get { return _attackPoint; }
        set { _attackPoint = value; }
    }
    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    public float atkSpeed
    {
        get { return _atkSpeed; }
        set { _atkSpeed = value; }
    }

    public float minOp
    {
        get { return _minOp; }
        set { _minOp = value; }
    }
    public float maxOp
    {
        get { return _maxOp; }
        set { _maxOp = value; }
    }
    public float atkAniSpeed
    {
        get { return _atkAniSpeed; }
        set { _atkAniSpeed = value; }
    }
    public float atkCorSpeed
    {
        get { return _atkCorSpeed; }
        set { _atkCorSpeed = value; }
    }
    public Animator charOneAniPub
    {
        get { return charOneAni; }
    }

    public float oneHP
    {
        get { return hp; }
        set { hp = value; }
    }

    public float maxHP
    {
        get { return _maxHP; }
    }

    public bool StunTriggerPublic
    {
        get { return StunTrigger; }
    }

    public float tauntCoolTimeText
    {
        get { return _tauntCoolTimeText; }
    }

    public bool tauntTrigger
    {
        get { return _tauntTrigger; }
    }

    public float ultCoolTimeText
    {
        get { return _ultCoolTimeText; }
    }

    public bool ultTrigger
    {
        get { return _ultTrigger; }
    }

    public WeaponScript weaponScriptPublic
    {
        get { return weaponScript; }
    }
    private void Awake()
    {
        // 도발과 궁극기가 처음에는 쿨타임이 없도록 초기화
        tauntCoolTime = 15.5f;
        _tauntCoolTimeText = 15.0f;
        ultCoolTime = 25.5f;
        _ultCoolTimeText = 25.0f;
        _tauntTrigger = false;
        _ultTrigger = false;

        // 체력
        hp = 1000.0f;
        _maxHP = 1000.0f;

        // 최소,최대공격력
        _minOp = 15;
        _maxOp = 21;

        // 공격속도
        _atkSpeed = 1.0f;
        _atkAniSpeed = -0.5f;

        // 기절이 걸려있지 않다 = false
        StunTrigger = false;

        tauntEffect = Resources.Load<GameObject>("Prefabs/TauntEffect");
        ultEffect = Resources.Load<GameObject>("Prefabs/WhrilWindEffect");
        tauntScript = GetComponentInChildren<TauntScript>();
        charOneAni = GetComponent<Animator>();
        ultEffectOBJScript = GetComponentInChildren<UltEffectOBJScript>();
        monsterList = new List<GameObject>();
        distinctMobList = new List<GameObject>();

    }
    // Start is called before the first frame update
    void Start()
    {
        randomTime = 0.0f;
        _moveSpeed = 3.0f;
        randomVec = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        weaponScript = GetComponentInChildren<WeaponScript>();
        Debug.Log(weaponScript.name);
        _atkCorSpeed = 0.35f;
        ultSphereScript = GetComponentInChildren<UltSphereScript>();
        ultEffectOBJScript = weaponScript.transform.Find("UltEffect").GetComponent<UltEffectOBJScript>();
        damagePro = PlayerManager.instance.damageText;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCanvas == null)
            gameCanvas = PlayerManager.instance.gameCanvas;

        // 공격력 (범위랜덤)
        _attackPoint = UnityEngine.Random.Range(_minOp, _maxOp);

        // 기본공격 쿨타임
        _atkCoolTime += Time.deltaTime;

        // 도발 쿨타임
        tauntCoolTime += Time.deltaTime;

        if(tauntCoolTime <= 15.0f)
        {
            //Debug.Log(_tauntCoolTimeText);
            _tauntCoolTimeText -= Time.deltaTime;
        }

        if (_tauntCoolTimeText <= 0.02f)
        {
            _tauntTrigger = false;
        }

        // 궁극기 쿨타임
        ultCoolTime += Time.deltaTime;

        if (ultCoolTime <= 25.0f)
        {
            //Debug.Log(_ultCoolTimeText);
            _ultCoolTimeText -= Time.deltaTime;
        }

        if (_ultCoolTimeText <= 0.02f)
        {
            _ultTrigger = false;
        }


        // 현재 플레이어가 조종하는 대상이 1번캐릭터가 아닐 때
        if (!PlayerManager.instance.charOneSelect)
        {
            // 기절이 걸려있지 않을 때만
            if (!StunTrigger)
            {
                if (!PlayerManager.instance.gatherTrigger)
                {
                    // 타겟으로 지정한 몬스터가 있을때
                    if (Monster != null)
                    {
                        if(Monster.transform.position.y <= 0.7f)
                        {
                            // 공격, 이동 함수
                            if (getDistance(Monster) >= 2.0f)
                                moveToTarget();
                            else if (getDistance(Monster) <= 2.0f && !PlayerManager.instance.COneUltTrigger)
                            {
                                transform.LookAt(Monster.transform);
                                attackCoolTime(atkSpeed);
                            }
                        }
                    }
                    else if (Monster == null || Monster.GetComponent<MobScriptTest>().hp <= 0)
                    {
                        searchTarget();
                    }

                    for (int i = 0; i < distinctMobList.Count; i++)
                    {
                        if (distinctMobList[i] == null)
                        {
                            Debug.Log("null?");
                            distinctMobList.Remove(distinctMobList[i]);
                        }
                    }

                    // 쿨타임이 다 돌았을때만
                    if (tauntCoolTime >= 15.0f)
                    {
                        if (distinctMobList.Count < 4)
                        {
                            searchTargetTaunt();
                            distinctMobList = monsterList.Distinct<GameObject>().ToList();

                            //Debug.Log(distinctMobList.Count);
                        }

                        try
                        {
                            if (distinctMobList.Count >= 3 &&
                            getDistance(distinctMobList[0]) <= 4.0f &&
                            getDistance(distinctMobList[1]) <= 4.0f &&
                            getDistance(distinctMobList[2]) <= 4.0f)
                            {
                                Taunt();

                                monsterList.Clear();
                                distinctMobList.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex + " - 예외발생,무시");
                        }
                    }
                }
                else if (PlayerManager.instance.gatherTrigger)
                {
                    ifGatherTrue();
                }
            }
            
            // 플레이어를 바꿔도 궁극기가 끊기질 않도록 애니메이션 3번을 유지함
            if (PlayerManager.instance.COneUltTrigger)
            {
                charOneAni.SetInteger("aniInt", 3);
            }
            // 궁극기 시전중이 아닐 때
            /*else if (!PlayerManager.instance.COneUltTrigger)
            {
                charOneAni.SetInteger("aniInt", 0);
            }*/

            
        }

    }

    public void MeleeUIOn()
    {
        WorldCanvasScript.instance.meleeUI.gameObject.SetActive(true);
    }

    public void MeleeUIOff()
    {
        WorldCanvasScript.instance.meleeUI.gameObject.SetActive(false);
    }

    public void attackCoolTime(float _ad)
    {
        if (_atkCoolTime >= _ad)
        {
            Attack();
            _atkCoolTime = 0;
        }
    }

    public void Attack()
    {
        charOneAni.SetInteger("aniInt", 2);
        weaponScript.use();
    }

    public void Taunt()
    {
        // 도발 쿨타임이 15초 지났을 때
        if (tauntCoolTime >= 15.0f)
        {
            tauntScript.useTaunt();
            TauntEffect();
            tauntCoolTime = 0;
            _tauntTrigger = true;
            _tauntCoolTimeText = 15.0001f;
        }
        else
        {
            Debug.Log("taunt cooltime");
        }
    }

    public void TauntEffect()
    {
        instanceTauntEffect = GameObject.Instantiate(tauntEffect, transform.position, transform.rotation);
        Destroy(instanceTauntEffect, 0.8f);
    }

    // 각성기 사용 함수
    public void useUlt()
    {
        if(ultCoolTime >= 25.0f)
        {
            PlayerManager.instance.COneUltTrigger = true;
            StopCoroutine(durationOfUlt());
            StartCoroutine(durationOfUlt());
            UltEffect();
            ultSphereScript.UltColliderDisable();
            ultCoolTime = 0;
            _ultTrigger = true;
            _ultCoolTimeText = 25.0001f;
        }
        else
        {
            Debug.Log("uit cooltime");
        }
    }

    // 각성기에 3초 지속시간을 주는 코루틴
    IEnumerator durationOfUlt() 
    {
        charOneAni.SetInteger("aniInt", 3);
        ultEffectOBJScript.gameObject.SetActive(true);
        //Debug.Log(ultEffectOBJScript.gameObject.name);
        yield return new WaitForSeconds(3.0f);
        ultEffectOBJScript.gameObject.SetActive(false);
        charOneAni.SetInteger("aniInt", 0);
        yield return new WaitForSeconds(0.25f);
        PlayerManager.instance.COneUltTrigger = false;
    }

    // 각성기 이펙트 인스턴스함수
    public void UltEffect()
    {
        instanceUltEffect = GameObject.Instantiate(ultEffect, weaponScript.transform.position, weaponScript.transform.rotation);
        Destroy(instanceUltEffect, 3.0f);
    }

    // 각성기 이펙트가 나오는 위치를 변경하는 함수
    public void ChangeUltEffectPosition(Vector3 position, Quaternion rotation)
    {
        if (instanceUltEffect != null)
        {
            instanceUltEffect.transform.position = position;
            instanceUltEffect.transform.rotation = rotation;
        }
        else
        {
            Debug.Log(null);
        }
    }
    

    // 트리거가 들어왔을때 - 피격판정
    private void OnTriggerEnter(Collider other)
    {
        // 몬스터의 무기에는 monsterWeapon 태그가 붙어있다.
        if (other.CompareTag("MonsterWeapon"))
        {
            // 몬스터의 무기에 있는 스크립트를 가져옴
            mobWeaponScript = other.GetComponent<MobWeaponScript>();
            // 몬스터 스크립트에 있는 공격력을 mobOP 매개변수에 대입
            mobOP = ((int)mobWeaponScript.mobWeaponOP);
            // playerManager 에서 가지고있는 리소스에서 로드한 데미지텍스트(빨간색)에 mobweaponscript 의 공격력을 텍스트로 대입
            damagePro.GetComponent<TextMeshProUGUI>().text = mobOP.ToString();
            // 데미지텍스트 인스턴스화 - WorldToScreenPoint 함수를 통해 바라보는 화면에서 현재 트랜스폼 위쪽에 데미지 표시
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            // 데미지텍스트의 부모를 캔버스로 지정(이작업을 해줘야 데미지가 보임)
            damageText.transform.SetParent(gameCanvas.transform);
            //데미지연산
            hp -= mobOP;
        }
        else if (other.CompareTag("MiddleBossHit"))
        {
            mbHitScript = other.GetComponent<MBHitScript>();
            middleBossOP = (int)mbHitScript.mbOP;
            damagePro.GetComponent<TextMeshProUGUI>().text = middleBossOP.ToString();
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f,0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            hp -= middleBossOP;
        }
        else if (other.CompareTag("ShockWave"))
        {
            Debug.Log("SW Hit");
            Stun();
            CreateStunText();
            shockWaveScript = other.GetComponent<ShockWaveScript>();
            ShockWaveD = shockWaveScript.ShockWaveDamage;
            damagePro.GetComponent<TextMeshProUGUI>().text = ShockWaveD.ToString();
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            hp -= ShockWaveD;
        }
        else if (other.CompareTag("BossHit"))
        {
            Debug.Log("boss hit");
            bossHitScript = other.GetComponent<BossHitScript>();
            BossOP = (int)bossHitScript.BossOP;
            damagePro.GetComponent<TextMeshProUGUI>().text = BossOP.ToString();
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            hp -= BossOP;
        }
        else if (other.CompareTag("Effect"))
        {
            redDragonScript = other.transform.root.GetComponentInChildren<RedDragonScript>();
            Debug.Log(redDragonScript);
            if (other.name.Equals("FlameThrowPoint"))
            {
                FlameOP = (int)redDragonScript.flameOP;
                damagePro.GetComponent<TextMeshProUGUI>().text = FlameOP.ToString();
                hp -= FlameOP;
            }
            else if (other.name.Equals("Meteor") || other.name.Equals("Explosion2"))
            {
                meteorOP = (int)redDragonScript.meteorOP;
                damagePro.GetComponent<TextMeshProUGUI>().text = meteorOP.ToString();
                hp -= meteorOP;
            }
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
        }
    }

    public float getDistance(GameObject mob)
    {
        return Vector3.Distance(transform.position, mob.transform.position);
    }

    public void searchTarget()
    {
        // 몬스터 주변의 오브젝트 불러오기
        Collider[] colls = Physics.OverlapSphere(transform.position, 15.0f);
        //Debug.Log("searching - " + colls[0].name);
        
        // 몬스터 주변에 오브젝트가 있으면..
        for (int i = 0; i < colls.Length; i++)
        {
            Collider tmpColl = colls[i];
            // 캐릭터가 맞으면, 타겟 설정
            if (tmpColl.gameObject.CompareTag("Monster"))
            {
                // 타겟 오브젝트에 넣어주기
                Monster = tmpColl.gameObject;
                //Debug.Log(playerGO);
                break;
            }
            else
            {
                //Debug.Log("no mob");
            }
        }
    }

    public void searchTargetTaunt()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 5.0f);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.CompareTag("Monster"))
            {
                monsterList.Add(colls[i].gameObject);
            }
            else
            {
                //Debug.Log("몬스터가 아닌 콜라이더 = "+colls[i].name);
            }
        }
    }

    public void moveToTarget()
    {
        charOneAni.SetInteger("aniInt", 1);
        if (charOneAni.GetInteger("aniInt") == 1)
        {
            // 캐릭터가 타겟을 바라보기
            transform.LookAt(Monster.transform.position);
            // 캐릭터를 타겟에 접근하기
            transform.position =
            Vector3.MoveTowards(transform.position, Monster.transform.position, Time.deltaTime * moveSpeed);
        }
    }

    public void Stun()
    {
        StopCoroutine("StunCoroutine");
        StartCoroutine("StunCoroutine");
    }

    IEnumerator StunCoroutine()
    {
        StunTrigger = true;
        Debug.Log("4로바뀜");
        charOneAni.SetInteger("aniInt", 4);
        yield return new WaitForSeconds(2.0f);
        StunTrigger = false;
        Debug.Log("0으로바뀜");
        charOneAni.SetInteger("aniInt", 0);
    }

    public void CreateStunText()
    {
        instanceStunText = GameObject.Instantiate(PlayerManager.instance.stunText,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z)),
            Quaternion.identity);
        instanceStunText.transform.SetParent(gameCanvas.transform);
        Destroy(instanceStunText, 2.0f);
    }

    public void ifGatherTrue()
    {
        charOneAni.SetInteger("aniInt", 1);
        if (charOneAni.GetInteger("aniInt") == 1)
        {
            // 캐릭터가 타겟을 바라보기
            transform.LookAt(PlayerManager.instance.playerTransform.position);
            // 캐릭터를 타겟에 접근하기
            transform.position =
            Vector3.MoveTowards(transform.position, PlayerManager.instance.playerTransform.position, Time.deltaTime * moveSpeed);
        }
    }
}
