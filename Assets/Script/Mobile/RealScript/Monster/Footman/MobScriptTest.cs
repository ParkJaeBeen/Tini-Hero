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
        // 몬스터 어그로 시간 (처음에는 16초로 바로 어그로가 끌리게 초기화해줌)
        targetDelay = 16.0f;
        // 공격시 함수가 여러번 실행되지 않게 해주는 트리거
        atkTrigger = false;
        // 도발 트리거
        tauntTrigger = false;
        // 몬스터의 체력
        HP = 200.0f;
        _maxHP = 200.0f;
        // 몬스터의 이동속도
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

        // HP가 0 초과일때 - 살아있을때 / 이 안에 모든 움직임을 넣어줘야함
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
                // 5초후에 도발이 풀려야함
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
        // HP가 0 이하일때 = 죽었을때
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
        // 무기가 닿았을 때 - 피격판정
        if (other.transform.CompareTag("Weapon"))
        //&& PlayerManager.instance.playerAniController.GetInteger("aniInt") == 2)
        {
            // 데미지 텍스트 띄우는코드 - 무기의 이름에 따라 저장해주는 데미지 값이 달라져야함
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
            PrintDamage(MagicMissileOP, "range");
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
        // Debug.Log("### InitMonster.moveToTarget ###");
        mobAni.SetInteger("aniInt", 1);
        if (mobAni.GetInteger("aniInt") == 1)
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
            // 애니메이션 컨트롤 - 2는 공격
            mobAni.SetInteger("aniInt", 2);
            if (atkTrigger)
            {
                // 공격시 대상을 쳐다보고 공격하기 위한 lookAt 함수
                transform.LookAt(playerGO.transform);
                // use 함수를 통해 공격 콜라이더 활성화
                mobWeaponScript.use();
                // 트리거를 꺼주기 - 공격함수가 업데이트에서 두번이상 실행되지 않기 위함.
                atkTrigger = false;
            }
            // 어택딜레이를 초기화해줌 - 매개변수보다 작게 만들어서 조건을 타지 않기 위해
            attackDelay = 0;
        }
    }
}
