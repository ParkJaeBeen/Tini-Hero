using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiddleBossScript : MonoBehaviour
{
    // �߰����� �ִϸ�������Ʈ�ѷ�
    Animator middleBossAni;
    // �⺻ �ɷ�ġ                   ��׷� �ٲ�µ�����  ������Ÿ��  ����� ������ð�
    float HP, _maxHP, speed, attackDelay, targetDelay, tauntCount, shockWaveCount, shockWaveCoolTime;
    int minOP, maxOP, OP, _ShockWaveDamage, randomInt;
    // ĳ���͵� ���� ���ݷ�
    int TwoHandSwordOP;
    int ArrowOP;
    int MagicMissileOP;
    // ���� ������ ĳ������ Ʈ������, ���ӿ�����Ʈ
    Transform playerTransform;
    GameObject playerGO;
    // ����, ����, ���Ÿ� �������ؽ�Ʈ���ҽ�
    TextMeshProUGUI meleeDamage, rangeDamage, magicDamage;
    // ĵ������ �������ؽ�Ʈ(������ ĵ������ �׷����� �ؽ�Ʈ)
    GameObject gameCanvas, damageText;
    // ��ü�� �ݶ��̴� - �ǰݴ��ҽÿ� ���
    CapsuleCollider MiddleBossCollider;
    // ���ݹ���, �ñر� ������ ���� ��ũ��Ʈ
    WeaponScript weaponScript;
    UltSphereScript ultSphereScript;
    ArrowScript arrowScript;
    MagicMissileScript magicMissileScript;
    // ������ �ι��̻� �ߵ����� �ʵ��� �ϴ� Ʈ����, ������ �ɷȴ��� �Ǵ��ϴ� Ʈ����
    bool atkTrigger, tauntTrigger, SWTrigger;
    // �÷��̾���� ����Ʈ
    List<Transform> playerList;
    // �������� �������� ��ũ��Ʈ
    MBHitScript mbHitScript;
    // ��ũ���̺� ���ҽ�, �ν��Ͻ�
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
        // ���� ��׷� �ð� (ó������ 16�ʷ� �ٷ� ��׷ΰ� ������ �ʱ�ȭ����)
        targetDelay = 16.0f;
        // ���ݽ� �Լ��� ������ ������� �ʰ� ���ִ� Ʈ����
        atkTrigger = false;
        // ���� Ʈ����
        tauntTrigger = false;
        // ����� Ʈ����
        SWTrigger = false;
        // �߰������� ü��
        HP = 2000.0f;
        _maxHP = 2500.0f;
        // ����� ��Ÿ��
        shockWaveCoolTime = 101.0f;
        // ������ �̵��ӵ�
        speed = 2.0f;
        // ���ݷ�
        minOP = 45;
        maxOP = 51;
        // ����� ������
        _ShockWaveDamage = 35;

        // �ִ���Ʈ�ѷ�
        middleBossAni = GetComponent<Animator>();
        // ��ü�� �ݶ��̴�
        MiddleBossCollider = GetComponent<CapsuleCollider>();
        // �������� ������Ʈ ��ũ��Ʈ
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

        // HP�� 0 �ʰ��϶� - ��������� / �� �ȿ� ��� �������� �־������
        if (HP > 0)
        {
            if (!tauntTrigger)
            {
                selectTarget(10.0f, randomInt);
            }
            else if (tauntTrigger)
            {
                // ���ߴ���
                Taunted();
                // ���߽ð��� ����
                tauntCount += Time.deltaTime;
                // 5���Ŀ� ������ Ǯ������
                if (tauntCount >= 15.0f)
                {
                    //Debug.Log(tauntCount);
                    // Ÿ�ٵ����̸� ������ �ð����� ���� �����ν� �ٷ� ��׷ΰ� �ٲ��
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
                    // �Ű������� ���ݼӵ�(@�ʿ��ѹ��� ����)
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
        // HP�� 0 �����϶� = �׾�����
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
        // ���Ⱑ ����� �� - �ǰ�����
        if (other.transform.CompareTag("Weapon"))
        //&& PlayerManager.instance.playerAniController.GetInteger("aniInt") == 2)
        {
            // ������ �ؽ�Ʈ �����ڵ� - ������ �̸��� ���� �������ִ� ������ ���� �޶�������
            if (other.name.Equals("THS07_Sword"))
            {
                weaponScript = other.GetComponent<WeaponScript>();
                TwoHandSwordOP = ((int)weaponScript.weaponOP);
                meleeDamage.GetComponent<TextMeshProUGUI>().text = TwoHandSwordOP.ToString();
                HP -= TwoHandSwordOP;
            }
            // �μհ������� �ñر������Ʈ �ݶ��̴��� �������
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
        // ȭ���� ����� ��
        else if (other.transform.CompareTag("Arrow"))
        {
            arrowScript = other.GetComponent<ArrowScript>();
            ArrowOP = ((int)arrowScript.arrowOP);
            PrintDamage(ArrowOP, "archer");
            HP -= ArrowOP;
        }
        // �����̻����� ����� ��
        else if (other.transform.CompareTag("MagicMissile"))
        {
            magicMissileScript = other.GetComponent<MagicMissileScript>();
            MagicMissileOP = ((int)magicMissileScript.magicMissileOP);
            PrintDamage(MagicMissileOP, "Magician");
            Debug.Log("�����̻��� = "+MagicMissileOP);
            HP -= MagicMissileOP;
        }

        // ���߿� ����� ��
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
        // ���� �ֺ��� ������Ʈ �ҷ�����
        Collider[] colls = Physics.OverlapSphere(transform.position, 300.0f);
        //Debug.Log("searching - " + colls[0].name);

        // ���� �ֺ��� ������Ʈ�� ������..
        for (int i = 0; i < colls.Length; i++)
        {
            Collider tmpColl = colls[i];
            // ĳ���Ͱ� ������, Ÿ�� ����
            if (tmpColl.gameObject.name.Equals(playerTransform.name))
            {
                // Ÿ�� ������Ʈ�� �־��ֱ�
                playerGO = tmpColl.gameObject;
                //Debug.Log(playerGO);
                break;
            }
        }
    }

    // �Ű����� _time = ��׷� ��Ÿ��, _targetNum = Ÿ���� ����(�̸��ڼ���)
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
        // �÷��̾ ������ �ȵǴϱ� ����Ʈ���� �̸����� �˻�
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].name.Substring(playerList[i].name.Length - 1, 1) == 1.ToString())
            {
                // 1������ �ٲ���
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
            // ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(playerGO.transform.position);
            // ���͸� Ÿ�ٿ� �����ϱ�
            transform.position =
            Vector3.MoveTowards(transform.position, playerGO.transform.position, Time.deltaTime * speed);
        }
    }
    // �Ÿ��� 2.0f ������ �� �ߵ�
    public void attackCoolTime(float _ad)
    {
        // �Ű����� _ad �� ������ ���õ����� ���� Ŭ �� (���õ����̴� ������Ʈ���� ��� ��� - 1�ʸ��� 1.0f��)
        if (attackDelay >= _ad)
        {
            // ������ �� �ѹ� �����ϱ� ���� Ʈ����
            atkTrigger = true;
            
            
            if (atkTrigger)
            {
                // �ִϸ��̼� ��Ʈ�� - 2�� ����
                middleBossAni.SetInteger("aniInt", 2);
                // ���ݽ� ����� �Ĵٺ��� �����ϱ� ���� lookAt �Լ�
                transform.LookAt(playerGO.transform);
                // use �Լ��� ���� ���� �ݶ��̴� Ȱ��ȭ
                mbHitScript.Attack();
                // Ʈ���Ÿ� ���ֱ� - �����Լ��� ������Ʈ���� �ι��̻� ������� �ʱ� ����.
                atkTrigger = false;
            }
            // ���õ����̸� �ʱ�ȭ���� - �Ű��������� �۰� ���� ������ Ÿ�� �ʱ� ����
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
