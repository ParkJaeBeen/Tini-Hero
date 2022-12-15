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
        // ���߰� �ñرⰡ ó������ ��Ÿ���� ������ �ʱ�ȭ
        tauntCoolTime = 15.5f;
        _tauntCoolTimeText = 15.0f;
        ultCoolTime = 25.5f;
        _ultCoolTimeText = 25.0f;
        _tauntTrigger = false;
        _ultTrigger = false;

        // ü��
        hp = 1000.0f;
        _maxHP = 1000.0f;

        // �ּ�,�ִ���ݷ�
        _minOp = 15;
        _maxOp = 21;

        // ���ݼӵ�
        _atkSpeed = 1.0f;
        _atkAniSpeed = -0.5f;

        // ������ �ɷ����� �ʴ� = false
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

        // ���ݷ� (��������)
        _attackPoint = UnityEngine.Random.Range(_minOp, _maxOp);

        // �⺻���� ��Ÿ��
        _atkCoolTime += Time.deltaTime;

        // ���� ��Ÿ��
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

        // �ñر� ��Ÿ��
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


        // ���� �÷��̾ �����ϴ� ����� 1��ĳ���Ͱ� �ƴ� ��
        if (!PlayerManager.instance.charOneSelect)
        {
            // ������ �ɷ����� ���� ����
            if (!StunTrigger)
            {
                if (!PlayerManager.instance.gatherTrigger)
                {
                    // Ÿ������ ������ ���Ͱ� ������
                    if (Monster != null)
                    {
                        if(Monster.transform.position.y <= 0.7f)
                        {
                            // ����, �̵� �Լ�
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

                    // ��Ÿ���� �� ����������
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
                            Debug.Log(ex + " - ���ܹ߻�,����");
                        }
                    }
                }
                else if (PlayerManager.instance.gatherTrigger)
                {
                    ifGatherTrue();
                }
            }
            
            // �÷��̾ �ٲ㵵 �ñرⰡ ������ �ʵ��� �ִϸ��̼� 3���� ������
            if (PlayerManager.instance.COneUltTrigger)
            {
                charOneAni.SetInteger("aniInt", 3);
            }
            // �ñر� �������� �ƴ� ��
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
        // ���� ��Ÿ���� 15�� ������ ��
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

    // ������ ��� �Լ�
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

    // �����⿡ 3�� ���ӽð��� �ִ� �ڷ�ƾ
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

    // ������ ����Ʈ �ν��Ͻ��Լ�
    public void UltEffect()
    {
        instanceUltEffect = GameObject.Instantiate(ultEffect, weaponScript.transform.position, weaponScript.transform.rotation);
        Destroy(instanceUltEffect, 3.0f);
    }

    // ������ ����Ʈ�� ������ ��ġ�� �����ϴ� �Լ�
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
    

    // Ʈ���Ű� �������� - �ǰ�����
    private void OnTriggerEnter(Collider other)
    {
        // ������ ���⿡�� monsterWeapon �±װ� �پ��ִ�.
        if (other.CompareTag("MonsterWeapon"))
        {
            // ������ ���⿡ �ִ� ��ũ��Ʈ�� ������
            mobWeaponScript = other.GetComponent<MobWeaponScript>();
            // ���� ��ũ��Ʈ�� �ִ� ���ݷ��� mobOP �Ű������� ����
            mobOP = ((int)mobWeaponScript.mobWeaponOP);
            // playerManager ���� �������ִ� ���ҽ����� �ε��� �������ؽ�Ʈ(������)�� mobweaponscript �� ���ݷ��� �ؽ�Ʈ�� ����
            damagePro.GetComponent<TextMeshProUGUI>().text = mobOP.ToString();
            // �������ؽ�Ʈ �ν��Ͻ�ȭ - WorldToScreenPoint �Լ��� ���� �ٶ󺸴� ȭ�鿡�� ���� Ʈ������ ���ʿ� ������ ǥ��
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            // �������ؽ�Ʈ�� �θ� ĵ������ ����(���۾��� ����� �������� ����)
            damageText.transform.SetParent(gameCanvas.transform);
            //����������
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
        // ���� �ֺ��� ������Ʈ �ҷ�����
        Collider[] colls = Physics.OverlapSphere(transform.position, 15.0f);
        //Debug.Log("searching - " + colls[0].name);
        
        // ���� �ֺ��� ������Ʈ�� ������..
        for (int i = 0; i < colls.Length; i++)
        {
            Collider tmpColl = colls[i];
            // ĳ���Ͱ� ������, Ÿ�� ����
            if (tmpColl.gameObject.CompareTag("Monster"))
            {
                // Ÿ�� ������Ʈ�� �־��ֱ�
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
                //Debug.Log("���Ͱ� �ƴ� �ݶ��̴� = "+colls[i].name);
            }
        }
    }

    public void moveToTarget()
    {
        charOneAni.SetInteger("aniInt", 1);
        if (charOneAni.GetInteger("aniInt") == 1)
        {
            // ĳ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(Monster.transform.position);
            // ĳ���͸� Ÿ�ٿ� �����ϱ�
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
        Debug.Log("4�ιٲ�");
        charOneAni.SetInteger("aniInt", 4);
        yield return new WaitForSeconds(2.0f);
        StunTrigger = false;
        Debug.Log("0���ιٲ�");
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
            // ĳ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(PlayerManager.instance.playerTransform.position);
            // ĳ���͸� Ÿ�ٿ� �����ϱ�
            transform.position =
            Vector3.MoveTowards(transform.position, PlayerManager.instance.playerTransform.position, Time.deltaTime * moveSpeed);
        }
    }
}
