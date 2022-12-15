using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobScriptTest : MonoBehaviour
{
    Animator mobAni;
    float HP, _maxHP, speed, attackDelay, targetDelay, tauntCount;
    int OP, randomInt;
    int TwoHandSwordOP;
    int ArrowOP;
    int MagicMissileOP;
    Transform playerTransform;
    GameObject playerGO;
    Rigidbody rb;
    TextMeshProUGUI meleeDamage, rangeDamage, magicDamage;
    GameObject gameCanvas, damageText;
    CapsuleCollider monsterCollider;
    WeaponScript weaponScript;
    UltSphereScript ultSphereScript;
    ArrowScript arrowScript;
    MagicMissileScript magicMissileScript;
    public MobWeaponScript mobWeaponScript;
    bool atkTrigger, tauntTrigger;
    List<Transform> playerList;

    public int op
    {
        get { return OP; }
    }

    private void Awake()
    {
        playerList = new List<Transform>();
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

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��׷� �ð� (ó������ 16�ʷ� �ٷ� ��׷ΰ� ������ �ʱ�ȭ����)
        targetDelay = 16.0f;
        // ���ݽ� �Լ��� ������ ������� �ʰ� ���ִ� Ʈ����
        atkTrigger = false;
        // ���� Ʈ����
        tauntTrigger = false;
        // ������ ü��
        HP = 200.0f;
        _maxHP = 200.0f;
        // ������ �̵��ӵ�
        speed = 2.0f;
        mobAni = GetComponent<Animator>();
        monsterCollider = GetComponent<CapsuleCollider>();
        playerTransform = PlayerManager.instance.playerTransform;
        rb = GetComponent<Rigidbody>();
        meleeDamage = MobPoolScript.instance.meleeDamageText;
        rangeDamage = MobPoolScript.instance.rangeDamageText;
        gameCanvas = GameObject.Find("Canvas");
        mobWeaponScript = GetComponentInChildren<MobWeaponScript>();
        for (int i = 0; i < PlayerManager.instance.playerTransforms.Count; i++)
        {
            playerList.Add(PlayerManager.instance.playerTransforms["Character_" + (i + 1)]);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            //Debug.Log(playerList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        OP = Random.Range(10, 16);

        attackDelay += Time.deltaTime;
        targetDelay += Time.deltaTime;
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
                Taunted();
                tauntCount += Time.deltaTime;
                // 5���Ŀ� ������ Ǯ������
                if(tauntCount >= 5.0f)
                {
                    //Debug.Log(tauntCount);
                    targetDelay = 10.5f;
                    tauntCount = 0;
                    tauntTrigger = false;
                }
            }

            if (playerGO == null)
            {
                searchTarget();
            }
            else if (playerGO != null)
            {
                if (getDistanceToTarget() < 2.0f)
                {
                    attackCoolTime(2.0f);
                }
                else if (getDistanceToTarget() > 2.0f)
                {
                    moveToTarget();
                }
            }


        }
        // HP�� 0 �����϶� = �׾�����
        else if (HP <= 0)
        {
            //Debug.Log("monster dead");
            monsterCollider.enabled = false;
            mobAni.SetInteger("aniInt", 6);
            if (mobAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(this.gameObject, 0.5f);
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
            if (other.name.Equals(PlayerManager.instance.charOneScriptPublic.weaponScriptPublic.name))
            {
                weaponScript = other.GetComponent<WeaponScript>();
                TwoHandSwordOP = ((int)weaponScript.weaponOP);
                meleeDamage.GetComponent<TextMeshProUGUI>().text = TwoHandSwordOP.ToString();
                HP -= TwoHandSwordOP;
            }
            else if (other.name.Equals("UltSphere"))
            {
                ultSphereScript = other.GetComponent<UltSphereScript>();
                TwoHandSwordOP = ((int)ultSphereScript.ultOP);
                meleeDamage.GetComponent<TextMeshProUGUI>().text = TwoHandSwordOP.ToString();
                HP -= TwoHandSwordOP;
            }
            damageText = GameObject.Instantiate(meleeDamage, 
                Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
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
            PrintDamage(MagicMissileOP, "range");
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
        damageText = GameObject.Instantiate(rangeDamage, 
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
            Quaternion.identity).gameObject;
        damageText.transform.SetParent(gameCanvas.transform);
        if (_name.Equals("Magician"))
        {
            magicDamage.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
            damageText = GameObject.Instantiate(magicDamage, 
                Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 2.0f + Random.Range(-0.3f, 0.3f), transform.position.z + Random.Range(-0.3f, 0.3f))),
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
        // Debug.Log("### InitMonster.moveToTarget ###");
        mobAni.SetInteger("aniInt", 1);
        if (mobAni.GetInteger("aniInt") == 1)
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
            // �ִϸ��̼� ��Ʈ�� - 2�� ����
            mobAni.SetInteger("aniInt", 2);
            if (atkTrigger)
            {
                // ���ݽ� ����� �Ĵٺ��� �����ϱ� ���� lookAt �Լ�
                transform.LookAt(playerGO.transform);
                // use �Լ��� ���� ���� �ݶ��̴� Ȱ��ȭ
                mobWeaponScript.use();
                // Ʈ���Ÿ� ���ֱ� - �����Լ��� ������Ʈ���� �ι��̻� ������� �ʱ� ����.
                atkTrigger = false;
            }
            // ���õ����̸� �ʱ�ȭ���� - �Ű��������� �۰� ���� ������ Ÿ�� �ʱ� ����
            attackDelay = 0;
        }
    }
}
