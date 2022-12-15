using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharThreeScript : MonoBehaviour
{
    // RA = recovery Amount
    float randomTime, _moveSpeed, _atkSpeed, hp, _maxHP, dp, mp, _attackPoint, RecoveryPoint, calRecoveryPoint, calRAText;
    float _atkCoolTime, _healCoolTime, _ultCoolTime, _buffCoolTime;
    float _healCoolTimeText, _ultCoolTimeText, _buffCoolTimeText;
    float buffDuration;
    Transform mmPos;
    GameObject ResourceMagicMissile, ResourceHealEffect, ResourceUltEffect, ResourceBuffEffect,
        instanceMagicMissile, instanceHealText, instanceHealEffect, instanceUltEffect, instanceBuffEffect;
    GameObject gameCanvas, damageText;
    TextMeshProUGUI HealText;
    Transform _HealTarget;
    int mobOP;
    int middleBossOP;
    int BossOP;
    int ShockWaveD;
    TextMeshProUGUI damagePro;
    MobWeaponScript mobWeaponScript;
    MBHitScript mbHitScript;
    BossHitScript bossHitScript;
    ShockWaveScript shockWaveScript;
    UltHealScript ultHealScript;
    Animator _CharThreeAni;
    GameObject Monster;
    GameObject instanceStunText;
    bool StunTrigger;
    bool _healTrigger, _buffTrigger, _ultTrigger;
    RedDragonScript redDragonScript;
    int FlameOP;
    private int meteorOP;

    public float moveSpeed
    {
        get { return _moveSpeed; }
    }
    public float atkSpeed
    {
        get { return _atkSpeed; }
    }
    public float attackPoint
    {
        get { return _attackPoint; }
    }
    public Transform HealTarget
    {
        get { return _HealTarget; }
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

    public Animator CharThreeAni
    {
        get { return _CharThreeAni; }
    }
    public bool StunTriggerPublic
    {
        get { return StunTrigger; }
    }

    public float healCoolTimeText
    {
        get { return _healCoolTimeText; }
    }
    public float ultCoolTimeText
    {
        get { return _ultCoolTimeText; }
    }
    public float buffCoolTimeText
    {
        get { return _buffCoolTimeText; }
    }
    public bool healTrigger
    {
        get { return _healTrigger; }
    }
    public bool ultTrigger
    {
        get { return _ultTrigger; }
    }
    public bool buffTrigger
    {
        get { return _buffTrigger; }
    }
    private void Awake()
    {
        // �̻����� �߻�Ǵ� ������Ʈ ��ġ
        mmPos = transform.Find("magicPosition");
        // �����տ��� ���ҽ��� �ҷ���
        ResourceMagicMissile = Resources.Load<GameObject>("Prefabs/MagicMissileGreen");
        ResourceHealEffect = Resources.Load<GameObject>("Prefabs/SmallHealEffect");
        ResourceBuffEffect = Resources.Load<GameObject>("Prefabs/BuffEffect");
        ResourceUltEffect = Resources.Load<GameObject>("Prefabs/UltHealEffect");
        HealText = Resources.Load<TextMeshProUGUI>("Prefabs/HealTextProGreen");

        // �ɷ�ġ

        // ����ü��, �ִ�ü��
        hp = 1000.0f; 
        _maxHP = 1000.0f;

        // ���ݼӵ�
        _atkSpeed = 1.0f;
        // ���ݷ�
        _attackPoint = 10.0f;
        // ��Ÿ��
        _healCoolTime = 2.2f;
        _healCoolTimeText = 2.0f;
        _ultCoolTime = 20.2f;
        _ultCoolTimeText = 20.0f;
        _buffCoolTime = 15.2f;
        _buffCoolTimeText = 15.0f;

        _healTrigger = false;
        _buffTrigger = false;
        _ultTrigger = false;
        // �������ӽð�
        buffDuration = 10.0f;
        // ��������
        RecoveryPoint = 93.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _CharThreeAni = GetComponent<Animator>();
        _moveSpeed = 2.0f;
        damagePro = PlayerManager.instance.damageText;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCanvas == null)
            gameCanvas = PlayerManager.instance.gameCanvas;

        _attackPoint = Random.Range(8, 14);
        // ���� �� ��Ÿ��
        _healCoolTime += Time.deltaTime;
        if(_healCoolTime <= 2.0f)
        {
            _healCoolTimeText -= Time.deltaTime;
        }
        if(_healCoolTimeText <= 0.02f)
        {
            _healTrigger = false;
        }
        // �ñر� ��Ÿ��
        _ultCoolTime += Time.deltaTime;
        if(_ultCoolTime <= 20.0f)
        {
            _ultCoolTimeText -= Time.deltaTime;
        }
        if(_ultCoolTimeText <= 0.02f)
        {
            _ultTrigger = false;
        }
        _buffCoolTime += Time.deltaTime;
        if(_buffCoolTime <= 15.0f)
        {
            _buffCoolTimeText -= Time.deltaTime;
        }
        if(_buffCoolTimeText <= 0.02f)
        {
            _buffTrigger = false;
        }
        // �⺻���� ��Ÿ��
        _atkCoolTime += Time.deltaTime;


        // charThreeSelect �� false �϶� = ������ ���ϰ��ִ�
        if (!PlayerManager.instance.charThreeSelect)
        {
            if (!StunTrigger)
            {
                if (!PlayerManager.instance.gatherTrigger)
                {
                    if (Monster != null)
                    {
                        if(Monster.transform.position.y <= 0.7f)
                        {
                            if (getDistance(Monster) <= 4.5f)
                            {
                                transform.LookAt(Monster.transform);
                                attackCoolTime(atkSpeed);
                            }
                            else
                            {
                                moveToTarget();
                            }
                        }
                        // ĳ���͵��� ü���� 70�ۼ�Ʈ ���Ϸ� ���������� ��
                        if ((PlayerManager.instance.charOneScriptPublic.oneHP / PlayerManager.instance.charOneScriptPublic.maxHP) * 100 <= 70.0f)
                        {
                            HealCoolTime(2.0f, 1);
                        }
                        else if ((PlayerManager.instance.charTwoScriptPublic.oneHP / PlayerManager.instance.charTwoScriptPublic.maxHP) * 100 <= 70.0f)
                        {
                            HealCoolTime(2.0f, 2);
                        }
                        else if ((oneHP / maxHP) * 100 <= 70.0f)
                        {
                            HealCoolTime(2.0f, 3);
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

    public void attackCoolTime(float _ad)
    {
        if (_atkCoolTime >= _ad)
        {
            _CharThreeAni.SetInteger("aniInt", 2);
            createMagicMissile();
            _atkCoolTime = 0;
        }
    }

    // ������ �����̻��� �����ڵ�
    public void createMagicMissile()
    {
        instanceMagicMissile = GameObject.Instantiate(ResourceMagicMissile, mmPos.position, mmPos.rotation);
        Rigidbody magicRigidBody = instanceMagicMissile.GetComponent<Rigidbody>();
        magicRigidBody.velocity = mmPos.forward * 5;
    }

    // �� ����Ʈ(�ؽ�Ʈ) ���� �ڵ�
    public void CreateHealText(Transform _charTransform, float _text)
    {
        instanceHealText = GameObject.Instantiate(HealText,
            Camera.main.WorldToScreenPoint(
            new Vector3(_charTransform.position.x + Random.Range(-0.3f, 0.3f), _charTransform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), _charTransform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
        instanceHealText.GetComponent<TextMeshProUGUI>().text = ((int)_text).ToString();
        instanceHealText.transform.SetParent(gameCanvas.transform);
        Destroy(instanceHealText, 0.5f);
    }

    public int calculateRP(float nowHP, float maxHP)
    {
        int morethanMaxhp = 0;
        int calRA;
        // nowHP = ����ü���� 100�� ���� ��
        if (nowHP >= maxHP)
        {
            morethanMaxhp = (int)(nowHP - maxHP);
            calRA = (int)(RecoveryPoint - morethanMaxhp);
            Debug.Log(calRA);
            return calRA;
        }
        else
        {
            return (int)RecoveryPoint;
        }
    }

    // �� ����Ʈ ���� �ڵ�
    public void CreateHealEffect(Transform _charTransform)
    {
        _HealTarget = _charTransform;
        instanceHealEffect = GameObject.Instantiate(ResourceHealEffect, _charTransform.position, _charTransform.rotation).gameObject;
        Destroy(instanceHealEffect, 1.0f);
    }

    // ĵ���� �� UI �� �ڵ�
    public void HealUIOn()
    {
        WorldCanvasScript.instance.HealUI.gameObject.SetActive(true);
    }

    // ĵ���� �� UI �����ڵ�
    public void HealUIOff()
    {
        WorldCanvasScript.instance.HealUI.gameObject.SetActive(false);
    }

    public void HealOneTarget(int _charNum)
    {
        if (_charNum.Equals(1))
        {
            _HealTarget = PlayerManager.instance.playerTransforms["Character_1"].transform;
            PlayerManager.instance.charOneScriptPublic.oneHP += RecoveryPoint;
            calRecoveryPoint = calculateRP(PlayerManager.instance.charOneScriptPublic.oneHP, PlayerManager.instance.charOneScriptPublic.maxHP);
            if(calRecoveryPoint != 100)
            {
                PlayerManager.instance.charOneScriptPublic.oneHP -= RecoveryPoint - calRecoveryPoint;
                calRAText = calRecoveryPoint;
            }
            else
            {
                calRAText = RecoveryPoint;
            }
            CreateHealText(_HealTarget, calRAText);
            CreateHealEffect(_HealTarget);
        }
        else if (_charNum.Equals(2))
        {
            _HealTarget = PlayerManager.instance.playerTransforms["Character_2"].transform;
            PlayerManager.instance.charTwoScriptPublic.oneHP += RecoveryPoint;
            calRecoveryPoint = calculateRP(PlayerManager.instance.charTwoScriptPublic.oneHP, PlayerManager.instance.charTwoScriptPublic.maxHP);
            if (calRecoveryPoint != RecoveryPoint)
            {
                PlayerManager.instance.charTwoScriptPublic.oneHP -= RecoveryPoint - calRecoveryPoint;
                calRAText = calRecoveryPoint;
            }
            else
            {
                calRAText = RecoveryPoint;
            }
            CreateHealText(_HealTarget, calRAText);
            CreateHealEffect(_HealTarget);
        }
        else if (_charNum.Equals(3))
        {
            _HealTarget = PlayerManager.instance.playerTransforms["Character_3"].transform;
            oneHP += RecoveryPoint;
            calRecoveryPoint = calculateRP(oneHP, maxHP);
            if (calRecoveryPoint != 100)
            {
                oneHP -= RecoveryPoint - calRecoveryPoint;
                calRAText = calRecoveryPoint;
            }
            else
            {
                calRAText = RecoveryPoint;
            }
            CreateHealText(_HealTarget, calRAText);
            CreateHealEffect(_HealTarget);
        }

    }

    public void HealCoolTime(float _hd, int _charNum)
    {
        if (_healCoolTime >= _hd)
        {
            HealOneTarget(_charNum);
            _healCoolTime = 0;
            _healTrigger = true;
            _healCoolTimeText = 2.0001f;
        }
    }

    // �ñر� ������
    public void Ult()
    {
        if (_ultCoolTime >= 20.0f)
        {
            CharThreeAni.SetInteger("aniInt", 3);
            StopCoroutine(UltEffectDelay());
            StartCoroutine(UltEffectDelay());
            _ultCoolTime = 0;
            _ultTrigger = true;
            _ultCoolTimeText = 20.0001f;
        }
        else
        {
            Debug.Log("heal ult cool");
        }
    }

    public void CreateUltEffect()
    {
        instanceUltEffect = GameObject.Instantiate(ResourceUltEffect, transform.position, transform.rotation);
        Destroy(instanceUltEffect, 5.0f);
    }

    IEnumerator UltEffectDelay()
    {
        yield return new WaitForSeconds(1.0f);
        _CharThreeAni.SetInteger("aniInt", 0);
        CreateUltEffect();
    }

    public void Buff()
    {
        if(_buffCoolTime >= 15.0f)
        {
            StopCoroutine(BuffCoroutine());
            StartCoroutine(BuffCoroutine());
            _buffCoolTime = 0;
            _buffTrigger = true;
            _buffCoolTimeText = 15.0001f;
        }
        else
        {
            Debug.Log("buff cool");
        }
    }

    IEnumerator BuffCoroutine()
    {
        // ���ݷ�
        PlayerManager.instance.charOneScriptPublic.minOp += 10;
        PlayerManager.instance.charOneScriptPublic.maxOp += 10;
        PlayerManager.instance.charTwoScriptPublic.minOp += 10;
        PlayerManager.instance.charTwoScriptPublic.maxOp += 10;
        _attackPoint += 10;

        // �̵��ӵ�
        PlayerManager.instance.charOneScriptPublic.moveSpeed += 2.0f;
        PlayerManager.instance.charTwoScriptPublic.moveSpeed += 2.0f;
        _moveSpeed += 2.0f;

        //���ݼӵ�
        PlayerManager.instance.charOneScriptPublic.atkSpeed -= 0.5f;
        // �÷��̾�1���� ����ĳ���Ͷ� �ִϸ��̼��� �ӵ��� �������־����
        PlayerManager.instance.charOneScriptPublic.atkAniSpeed = 0.3f;
        // �÷��̾�1���� �ڷ�ƾ �ӵ��� �����ؾ���
        PlayerManager.instance.charOneScriptPublic.atkCorSpeed = 0.2f;
        PlayerManager.instance.charTwoScriptPublic.atkSpeed -= 0.3f;
        _atkSpeed -= 0.3f;

        //����

        CreateBuffEffect(PlayerManager.instance.charOneScriptPublic.transform);
        CreateBuffEffect(PlayerManager.instance.charTwoScriptPublic.transform);
        CreateBuffEffect(transform);

        yield return new WaitForSeconds(buffDuration);


        PlayerManager.instance.charOneScriptPublic.minOp -= 10;
        PlayerManager.instance.charOneScriptPublic.maxOp -= 10;
        PlayerManager.instance.charTwoScriptPublic.minOp -= 10;
        PlayerManager.instance.charTwoScriptPublic.maxOp -= 10;
        _attackPoint -= 10;

        PlayerManager.instance.charOneScriptPublic.moveSpeed -= 2.0f;
        PlayerManager.instance.charTwoScriptPublic.moveSpeed -= 2.0f;
        _moveSpeed -= 2.0f;

        //���ݼӵ�
        PlayerManager.instance.charOneScriptPublic.atkSpeed += 0.5f;
        // �÷��̾�1���� ����ĳ���Ͷ� �ִϸ��̼��� �ӵ��� �������־����
        PlayerManager.instance.charOneScriptPublic.atkAniSpeed = -0.3f;
        // �÷��̾�1���� �ڷ�ƾ �ӵ��� �����ؾ���
        PlayerManager.instance.charOneScriptPublic.atkCorSpeed = 0.35f;
        PlayerManager.instance.charTwoScriptPublic.atkSpeed += 0.3f;
        _atkSpeed += 0.3f;
    }

    public void CreateBuffEffect(Transform _target)
    {
        instanceBuffEffect = GameObject.Instantiate(ResourceBuffEffect, _target.position, _target.rotation);
        instanceBuffEffect.transform.SetParent(_target);
        Destroy(instanceBuffEffect, buffDuration);
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
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
            damageText.transform.SetParent(gameCanvas.transform);
            hp -= middleBossOP;
        }
        else if (other.CompareTag("ShockWave"))
        {
            //Debug.Log("SW Hit");
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

    public void moveToTarget()
    {
        CharThreeAni.SetInteger("aniInt", 1);
        if (CharThreeAni.GetInteger("aniInt") == 1)
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
        //Debug.Log("4�ιٲ�");
        CharThreeAni.SetInteger("aniInt", 4);
        yield return new WaitForSeconds(3.0f);
        StunTrigger = false;
        //Debug.Log("0���ιٲ�");
        CharThreeAni.SetInteger("aniInt", 0);
    }

    public void CreateStunText()
    {
        instanceStunText = GameObject.Instantiate(PlayerManager.instance.stunText, 
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity);
        instanceStunText.transform.SetParent(gameCanvas.transform);
        Destroy(instanceStunText, 2.0f);
    }

    public void ifGatherTrue()
    {
        CharThreeAni.SetInteger("aniInt", 1);
        if (CharThreeAni.GetInteger("aniInt") == 1)
        {
            // ĳ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(PlayerManager.instance.playerTransform.position);
            // ĳ���͸� Ÿ�ٿ� �����ϱ�
            transform.position =
            Vector3.MoveTowards(transform.position, PlayerManager.instance.playerTransform.position, Time.deltaTime * moveSpeed);
        }
    }
}
