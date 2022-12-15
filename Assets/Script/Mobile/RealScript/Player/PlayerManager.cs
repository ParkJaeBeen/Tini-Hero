using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Dictionary<string, Transform> playerTransforms;
    Vector3 movingPoint, lastPos, dir;

    public Transform _playerTransform;
    float _inputX, _inputZ, moveSpeed, rotateSpeed, _attackCount, _attakDelay, _attakPoint, _healCount;
    public Animator playerAniController;
    CharOneScript charOneScript;
    CharTwoScript charTwoScript;
    CharThreeScript charThreeScript;
    public bool charOneSelect, charTwoSelect, charThreeSelect;
    public bool COneUltTrigger;
    TextMeshProUGUI _damageText;
    GameObject _stunText;
    public bool gatherTrigger;
    public GameObject gameCanvas;
    //Button gatherOnBt, gatherOffBt;
    public Transform playerTransform
    {
        get { return _playerTransform; }
    }
    public Dictionary<string, Transform> playerTransformsPublic
    {
        get { return playerTransforms; }
    }
    public float inputX
    {
        get { return _inputX; }
    }
    public float inputZ
    {
        get { return _inputZ; }
    }
    public float attackCount
    {
        get { return _attackCount; }
    }
    public float attakDelay
    {
        get { return _attakDelay; }
    }
    public float attakPoint
    {
        get { return _attakPoint; }
        set { _attakPoint = value; }
    }

    public CharOneScript charOneScriptPublic
    {
        get { return charOneScript; }
    }
    public CharTwoScript charTwoScriptPublic
    {
        get { return charTwoScript; }
    }
    public CharThreeScript charThreeScriptPublic
    {
        get { return charThreeScript; }
    }
    public TextMeshProUGUI damageText
    {
        get { return _damageText; }
    }
    public GameObject stunText
    {
        get { return _stunText; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        moveSpeed = 3.0f;
        rotateSpeed = 10.0f;
        _attackCount = 0;
        _healCount = 0;

        playerTransforms = new Dictionary<string, Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            playerTransforms.Add("Character_" + (i + 1), transform.GetChild(i));
        }
        
        _playerTransform = transform.GetChild(0);
        playerAniController = _playerTransform.GetComponent<Animator>();
        attakPoint = _playerTransform.GetComponent<CharOneScript>().attackPoint;
        charOneScript = GetComponentInChildren<CharOneScript>();
        charTwoScript = GetComponentInChildren<CharTwoScript>();
        charThreeScript = GetComponentInChildren<CharThreeScript>();
        charOneSelect = true;
        charTwoSelect = false;
        charThreeSelect = false;
        gatherTrigger = false;
        // 리소스/프리팹 폴더에 있는 데미지텍스트(빨간색)
        _damageText = Resources.Load<TextMeshProUGUI>("Prefabs/DamageTextPro");
        _stunText = Resources.Load<GameObject>("Prefabs/StunText");

        //gatherOnBt = GameObject.Find("Canvas").transform.Find("gatherOnBT").GetComponent<Button>();
        //gatherOffBt = GameObject.Find("Canvas").transform.Find("gatherOffBT").GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.instance.isGameClear)
        {
            _inputX = WorldCanvasScript.instance.joystick.inputHorizontal();
            _inputZ = WorldCanvasScript.instance.joystick.inputVertical();

            dir = new Vector3(_inputX, 0, _inputZ);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
                SelectCharacter(1);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
                SelectCharacter(2);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
                SelectCharacter(3);

            if (_playerTransform != null)
            {
                if (playerTransform == playerTransformsPublic["Character_1"])
                {
                    if (!charOneScript.StunTriggerPublic)
                    {
                        // 공격 모션이 나올때는 움직이지 못하게
                        if (playerAniController.GetInteger("aniInt") != 2)
                        {
                            MoveCharacter(playerTransform);
                        }
                        //궁극기 사용하는 동안 공격을 못하게 하는 트리거
                        if (!COneUltTrigger)
                        {
                            AttackCoolTime(charOneScript.atkSpeed);
                        }
                        Taunt();
                        CharOneUlt();
                    }
                    else if (charThreeScript.StunTriggerPublic)
                    {
                        //Debug.Log("1번기절상태");
                    }
                }
                else if (playerTransform == playerTransformsPublic["Character_2"])
                {
                    if (!charTwoScript.StunTriggerPublic)
                    {
                        // 공격 모션이 나올때는 움직이지 못하게
                        if (playerAniController.GetInteger("aniInt") != 2)
                        {
                            MoveCharacter(playerTransform);
                        }
                        AttackCoolTime(charTwoScript.atkSpeed);
                        MultiShot();
                        CharTwoUlt();
                    }
                    else if (charThreeScript.StunTriggerPublic)
                    {
                        //Debug.Log("2번기절상태");
                    }
                }
                else if (playerTransform == playerTransformsPublic["Character_3"])
                {
                    if (!charThreeScript.StunTriggerPublic)
                    {
                        // 애니메이션 상태가 기본이거나 움직이고 있을 때만 움직이게
                        if (playerAniController.GetInteger("aniInt") == 1 || playerAniController.GetInteger("aniInt") == 0)
                        {
                            MoveCharacter(playerTransform);
                        }
                        AttackCoolTime(charThreeScript.atkSpeed);
                        // 단일힐 코드
                        if (Input.GetKeyDown(KeyCode.X))
                            charThreeScript.HealCoolTime(2.0f, 1);
                        else if (Input.GetKeyDown(KeyCode.C))
                            charThreeScript.HealCoolTime(2.0f, 2);
                        else if (Input.GetKeyDown(KeyCode.V))
                            charThreeScript.HealCoolTime(2.0f, 3);
                        // 광역힐
                        CharThreeUlt();
                        // 버프
                        CharThreeBuff();
                    }
                    else if (charThreeScript.StunTriggerPublic)
                    {
                        //Debug.Log("3번기절상태");
                    }
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    gatherTrigger = true;
                    WorldCanvasScript.instance.gatherOnBt.gameObject.SetActive(true);
                    WorldCanvasScript.instance.gatherOffBt.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.H))
                {
                    gatherTrigger = false;
                    WorldCanvasScript.instance.gatherOnBt.gameObject.SetActive(false);
                    WorldCanvasScript.instance.gatherOffBt.gameObject.SetActive(true);
                    // 버튼은 enabled 가 아니라 interactable 라는 걸로 온오프 해주어야함
                }

                /*gatherOnBt.onClick.AddListener(test);
                gatherOffBt.onClick.AddListener(test2);*/
            }


            _attackCount += Time.deltaTime;
            _healCount += Time.deltaTime;
        }
    }

    public void GatherOff()
    {
        gatherTrigger = false;
        WorldCanvasScript.instance.gatherOnBt.gameObject.SetActive(true);
        WorldCanvasScript.instance.gatherOffBt.gameObject.SetActive(false);
    }

    public void GatherOn()
    {
        gatherTrigger = true;
        WorldCanvasScript.instance.gatherOnBt.gameObject.SetActive(false);
        WorldCanvasScript.instance.gatherOffBt.gameObject.SetActive(true);
    }

    public void MoveCharacter(Transform _playerT)
    {
        if (_playerT.name.Equals("MC01"))
        {
            moveSpeed = charOneScript.moveSpeed;
        }
        else if (_playerT.name.Equals("MC02"))
        {
            moveSpeed = charTwoScript.moveSpeed;
        }
        else if (_playerT.name.Equals("MC02"))
        {
            moveSpeed = charThreeScript.moveSpeed;
        }

        if (!(_inputX == 0 && _inputZ == 0))
        {
            _playerT.rotation = Quaternion.Lerp(_playerT.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
            _playerT.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (UnityEngine.Input.GetKey(KeyCode.Z))
        {
            //Debug.Log("attack");
            playerAniController.SetInteger("aniInt", 2);
            if (playerTransform.name.Equals("MC01"))
            {
                charOneScript.Attack();
            }
            else if (playerTransform.name.Equals("MC02"))
            {
                charTwoScript.Attack();
            }
            else if (playerTransform.name.Equals("MC03"))
            {
                charThreeScript.createMagicMissile();
            }
            _attackCount = 0;
        }
    }
    public void AttackCoolTime(float _ad)
    {
        if (_attackCount >= _ad)
        {
            Attack();
        }
    }

    public void Taunt()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            charOneScript.Taunt();
        }
    }

    public void CharOneUlt()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            charOneScript.useUlt();
            //MoveCharacter(playerTransform);
        }

    }

    public void MultiShot()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            charTwoScript.CreateMultiShot();
        }
    }

    public void CharTwoUlt()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            charTwoScript.Ult();
        }
    }

    public void CharThreeUlt()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            charThreeScript.Ult();
        }
    }

    public void CharThreeBuff()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            charThreeScript.Buff();
        }
    }

    public Transform SelectCharacter(int i)
    {
        // 다른 캐릭터 선택시 기존 캐릭터의 애니메이션을 idle로 초기화
        playerAniController.SetInteger("aniInt", 0);
        movingPoint = playerTransforms["Character_" + i].position;
        lastPos = playerTransforms["Character_" + i].position;
        _playerTransform = playerTransforms["Character_" + i];
        playerAniController = _playerTransform.GetComponent<Animator>();


        if (i == 1)
        {
            attakPoint = _playerTransform.GetComponent<CharOneScript>().attackPoint;
            moveSpeed = charOneScript.moveSpeed;
            //Debug.Log("1번 true");
            charOneSelect = true;
            charTwoSelect = false;
            charThreeSelect = false;
            charOneScript.MeleeUIOn();
            charTwoScript.RangerUIOff();
            charThreeScript.HealUIOff();
        }
        else if (i == 2)
        {
            moveSpeed = charTwoScript.moveSpeed;
            //Debug.Log("2번 true");
            charOneSelect = false;
            charTwoSelect = true;
            charThreeSelect = false;
            charOneScript.MeleeUIOff();
            charTwoScript.RangerUIOn();
            charThreeScript.HealUIOff();
        }
        else if (i == 3)
        {
            moveSpeed = charThreeScript.moveSpeed;
            //Debug.Log("3번 true");
            charOneSelect = false;
            charTwoSelect = false;
            charThreeSelect = true;
            charOneScript.MeleeUIOff();
            charTwoScript.RangerUIOff();
            charThreeScript.HealUIOn();
        }
        return _playerTransform;
    }

}
