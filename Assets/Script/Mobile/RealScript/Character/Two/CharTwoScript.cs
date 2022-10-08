using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CharTwoScript : MonoBehaviour
{
    private float _attackPoint;
    float randomTime, _moveSpeed, _atkSpeed, _minOp, _maxOp, hp, _maxHP, dp, mp;
    float _atkCoolTime, multiShotCoolTime, UltCoolTime;
    float UltDuration;
    float _multiShotCTText, _ultCoolTimeText;
    int mobOP;
    int middleBossOP;
    int ShockWaveD;
    public GameObject Arrow, resourceArrow, resourceMSarrow, resourceUltEffect, instanceArrow, instanceUltEffect;
    public Transform arrowPos;
    Transform multiPosOne, multiPosTwo, multiPosThree;
    MobWeaponScript mobWeaponScript;
    TextMeshProUGUI damagePro;
    GameObject damageText, gameCanvas;
    Animator CharTwoAni;
    GameObject Monster;
    GameObject instanceStunText;
    MBHitScript mbHitScript;
    ShockWaveScript shockWaveScript;
    Transform rangerUI;
    bool StunTrigger;
    bool _multiShotTrigger, _ultTrigger;
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
    public float maxHP
    {
        get { return _maxHP; }
    }

    public float oneHP
    {
        get { return hp; }
        set { hp = value; }
    }
    public bool StunTriggerPublic
    {
        get { return StunTrigger; }
    }

    public float multiShotCTText
    {
        get { return _multiShotCTText; }
    }

    public bool multiShotTrigger
    {
        get { return _multiShotTrigger; }
    }

    public float ultCoolTimeText
    {
        get { return _ultCoolTimeText; }
    }

    public bool ultTrigger
    {
        get { return _ultTrigger; }
    }

    private void Awake()
    {
        // ���� �ִϸ��̼ǿ��� �����տ� ȭ���� �״ٰ� ���� ���� ������ ȭ�������Ʈ
        Arrow = GameObject.Find("Arrows").transform.GetChild(0).gameObject;
        //�������� ȭ���� �ε�
        resourceArrow = Resources.Load<GameObject>("Prefabs/Arrow_Regular");
        // �������� ��Ƽ�� ȭ�� �ε�
        resourceMSarrow = Resources.Load<GameObject>("Prefabs/Arrow_MultiShot");
        // �������� �ñر� ����Ʈ
        resourceUltEffect = Resources.Load<GameObject>("Prefabs/BowManUltEffect");
        //ȭ���� �߻�Ǵ� ������Ʈ ��ġ
        arrowPos = transform.Find("arrowPosition");
        multiPosOne = transform.Find("multiShotPosition1");
        multiPosTwo = transform.Find("multiShotPosition2");
        multiPosThree = transform.Find("multiShotPosition3");
        CharTwoAni = GetComponent<Animator>();

        // ����ü��, �ִ�ü��
        hp = 300.0f;
        _maxHP = 300.0f;

        maxOp = 18;
        minOp = 23;
        _atkSpeed = 0.8f;

        UltDuration = 10.0f;

        multiShotCoolTime = 7.5f;
        _multiShotCTText = 7.00001f;
        _multiShotTrigger = false;
        UltCoolTime = 15.5f;
        _ultCoolTimeText = 15.00001f;
        _ultTrigger = false;

        rangerUI = GameObject.Find("Canvas").transform.Find("behaviorTwo");
    }
    // Start is called before the first frame update
    void Start()
    {
        _moveSpeed = 3.5f;
        damagePro = PlayerManager.instance.damageText;
        gameCanvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        _atkCoolTime += Time.deltaTime;

        multiShotCoolTime += Time.deltaTime;
        if(multiShotCoolTime <= 7.0f)
        {
            _multiShotCTText -= Time.deltaTime;
            //Debug.Log(_multiShotCTText);
        }
        if(_multiShotCTText <= 0.02f)
        {
            _multiShotTrigger = false;
        }

        UltCoolTime += Time.deltaTime;
        if (UltCoolTime <= 15.0f)
        {
            _ultCoolTimeText -= Time.deltaTime;
        }
        if (_ultCoolTimeText <= 0.02f)
        {
            _ultTrigger = false;
        }

        _attackPoint = Random.Range(_minOp, _maxOp);

        // charTwoSelect �� false �϶� 
        if (!PlayerManager.instance.charTwoSelect)
        {
            if (!StunTrigger)
            {
                if (!PlayerManager.instance.gatherTrigger)
                {
                    if (Monster != null)
                    {
                        if (getDistance(Monster) <= 7.0f)
                        {
                            transform.LookAt(Monster.transform);
                            attackCoolTime(atkSpeed);
                            if (multiShotCoolTime >= 7.0f)
                                CreateMultiShot();
                        }
                        else
                        {
                            moveToTarget();
                        }
                    }
                    else if (Monster == null)
                    {
                        searchTarget();
                    }
                }
                else if (PlayerManager.instance.gatherTrigger)
                {
                    ifGatherTrue();
                }
            }
            


        }
    }

    public void RangerUIOn()
    {
        rangerUI.gameObject.SetActive(true);
    }

    public void RangerUIOff()
    {
        rangerUI.gameObject.SetActive(false);
    }

    public void attackCoolTime(float _ad)
    {
        if (_atkCoolTime >= _ad)
        {
            CharTwoAni.SetInteger("aniInt", 2);
            Attack();
            _atkCoolTime = 0;
        }
    }

    public void Attack()
    {
        CreateArrow(arrowPos, resourceArrow);
    }

    // ���� �Լ� - ȭ���� ������Լ�
    public void CreateArrow(Transform _arrowPos, GameObject _resourceArrow)
    {
        instanceArrow = GameObject.Instantiate(_resourceArrow, _arrowPos.position, _arrowPos.rotation);
        Rigidbody arrowRigidBody = instanceArrow.GetComponent<Rigidbody>();
        //arrowRigidBody.velocity = arrowPos.forward * 20;
        arrowRigidBody.velocity = instanceArrow.transform.forward * 20;
    }

    public void CreateMultiShot()
    {
        if(multiShotCoolTime >= 7.0f)
        {
            CharTwoAni.SetInteger("aniInt", 3);
            StopCoroutine(MultiArrowCoroutine());
            StartCoroutine(MultiArrowCoroutine());
            multiShotCoolTime = 0;
            _multiShotTrigger = true;
            _multiShotCTText = 7.00001f;
        }
    }

    IEnumerator MultiArrowCoroutine()
    {
        CreateArrow(multiPosOne, resourceMSarrow);
        CreateArrow(multiPosTwo, resourceMSarrow);
        CreateArrow(multiPosThree, resourceMSarrow);
        yield return new WaitForSeconds(0.35f); 
        CreateArrow(multiPosOne, resourceMSarrow);
        CreateArrow(multiPosTwo, resourceMSarrow);
        CreateArrow(multiPosThree, resourceMSarrow);
    }

    public void Ult()
    {
        if(UltCoolTime >= 15.0f)
        {
            StopCoroutine(UltCoroutine());
            StartCoroutine(UltCoroutine());
            UltCoolTime = 0;
            _ultTrigger = true;
            _ultCoolTimeText = 15.0001f;
        }
    }

    IEnumerator UltCoroutine()
    {
        CreateUltEffect();
        _atkSpeed -= 0.3f;
        _moveSpeed += 2.0f;
        minOp += 12;
        maxOp += 13;
        yield return new WaitForSeconds(UltDuration);
        _atkSpeed += 0.3f;
        _moveSpeed -= 2.0f;
        minOp -= 12;
        maxOp -= 13;
    }

    public void CreateUltEffect()
    {
        instanceUltEffect = GameObject.Instantiate(resourceUltEffect, transform.position, transform.rotation);
        instanceUltEffect.transform.SetParent(transform);
        Destroy(instanceUltEffect, UltDuration);
    }

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
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z)),
            Quaternion.identity).gameObject;
            // �������ؽ�Ʈ�� �θ� ĵ������ ����(���۾��� ����� �������� ����)
            damageText.transform.SetParent(gameCanvas.transform);
            // 0.5�� �Ŀ� ����
            Destroy(damageText, 0.5f);
            //����������
            hp -= mobOP;
        }
        else if (other.CompareTag("MiddleBossHit"))
        {
            mbHitScript = other.GetComponent<MBHitScript>();
            middleBossOP = (int)mbHitScript.mbOP;
            damagePro.GetComponent<TextMeshProUGUI>().text = middleBossOP.ToString();
            damageText = GameObject.Instantiate(damagePro,
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z)),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            Destroy(damageText, 0.5f);
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
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z)),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            Destroy(damageText, 0.5f);
            hp -= ShockWaveD;
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

    public void moveToTarget()
    {
        CharTwoAni.SetInteger("aniInt", 1);
        if (CharTwoAni.GetInteger("aniInt") == 1)
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
        CharTwoAni.SetInteger("aniInt", 4);
        yield return new WaitForSeconds(3.0f);
        StunTrigger = false;
        Debug.Log("0���ιٲ�");
        CharTwoAni.SetInteger("aniInt", 0);
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
        CharTwoAni.SetInteger("aniInt", 1);
        if (CharTwoAni.GetInteger("aniInt") == 1)
        {
            // ĳ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(PlayerManager.instance.playerTransform.position);
            // ĳ���͸� Ÿ�ٿ� �����ϱ�
            transform.position =
            Vector3.MoveTowards(transform.position, PlayerManager.instance.playerTransform.position, Time.deltaTime * moveSpeed);
        }
    }
}
