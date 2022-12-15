using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedDragonScript : MonoBehaviour
{
    public static RedDragonScript instance;
    // �������� �ִ���Ʈ�ѷ�
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
    // ���ݹ���, �ñر� ������ ���� ��ũ��Ʈ
    WeaponScript weaponScript;
    UltSphereScript ultSphereScript;
    ArrowScript arrowScript;
    MagicMissileScript magicMissileScript;
    // ������ �ι��̻� �ߵ����� �ʵ��� �ϴ� Ʈ����, ������ �ɷȴ��� �Ǵ��ϴ� Ʈ����
    bool atkTrigger, tauntTrigger, MeteorTrigger, FlameTrigger, IsInvincibility;
    // �÷��̾���� ����Ʈ
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
        // ���� ��׷� �ð� (ó������ 16�ʷ� �ٷ� ��׷ΰ� ������ �ʱ�ȭ����)
        targetDelay = 16.0f;
        FlameThrowCoolTime = 5.1f;
        meteorCoolTime = 100.1f;
        // �߰������� ü��
        HP = 2300.0f;
        _maxHP = 2500.0f;
        // ������ �̵��ӵ�
        speed = 2.0f;
        // ���ݷ�
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
                    // �Ű������� ���ݼӵ�(@�ʿ��ѹ��� ����)
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
        // HP�� 0 �����϶� = �׾�����
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
                Destroy(damageText, 0.5f);
            }
            // ȭ���� ����� ��
            else if (other.transform.CompareTag("Arrow"))
            {
                Debug.Log("arrow hit");
                arrowScript = other.GetComponent<ArrowScript>();
                ArrowOP = ((int)arrowScript.arrowOP);
                PrintDamage(ArrowOP, "archer");
                HP -= ArrowOP;
                Destroy(damageText, 0.5f);
            }
            // �����̻����� ����� ��
            else if (other.transform.CompareTag("MagicMissile"))
            {
                magicMissileScript = other.GetComponent<MagicMissileScript>();
                MagicMissileOP = ((int)magicMissileScript.magicMissileOP);
                PrintDamage(MagicMissileOP, "Magic");
                Debug.Log("�����̻��� = " + MagicMissileOP);
                HP -= MagicMissileOP;
                Destroy(damageText, 0.5f);
            }

            // ���߿� ����� ��
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
        ani.SetInteger("aniInt", 1);
        if (ani.GetInteger("aniInt") == 1)
        {
            // ���Ͱ� Ÿ���� �ٶ󺸱�
            transform.LookAt(playerGO.transform.position);
            // ���͸� Ÿ�ٿ� �����ϱ�
            transform.position =
            Vector3.MoveTowards(transform.position, playerGO.transform.position, Time.deltaTime * speed);
        }
    }

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
                ani.SetInteger("aniInt", 2);
                // ���ݽ� ����� �Ĵٺ��� �����ϱ� ���� lookAt �Լ�
                transform.LookAt(playerGO.transform);
                // use �Լ��� ���� ���� �ݶ��̴� Ȱ��ȭ

                // Ʈ���Ÿ� ���ֱ� - �����Լ��� ������Ʈ���� �ι��̻� ������� �ʱ� ����.
                atkTrigger = false;
            }
            // ���õ����̸� �ʱ�ȭ���� - �Ű��������� �۰� ���� ������ Ÿ�� �ʱ� ����
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
