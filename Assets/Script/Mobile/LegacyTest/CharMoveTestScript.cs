using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharMoveTestScript : MonoBehaviour
{
    //public static CharMoveTestScript instance;
    //public JoyStickTestM joystick;
    /*float _inputX, _inputZ, moveSpeed, rotateSpeed;
    public float ATK;
    Animator ani;*/
    Vector3 movingPoint, lastPos;
    Dictionary<string, Transform> playerTransforms;
    public Transform _playerTransform;
    public Transform playerTransform
    {
        get { return _playerTransform; }
    }
    /*public float inputX
    {
        get { return _inputX; }
    }
    public float inputZ
    {
        get { return _inputZ; }
    }
    public Animator charAni
    {
        get { return ani; }
    }*/


    private void Awake()
    {
        /*if (instance == null)
            instance = this;
        moveSpeed = 5.0f;
        rotateSpeed = 10.0f;
        movingPoint = transform.position;*/
        playerTransforms = new Dictionary<string, Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            playerTransforms.Add("Character_" + (i + 1), transform.GetChild(i));
        }
        
        //joystick = GameObject.Find("JSBackground").GetComponent<JoyStickTestM>();
        //ani = GetComponent<Animator>();
        //ATK = Random.Range(15, 21);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        Debug.Log("Enter");
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = playerTransforms["Character_" + 1].transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*_inputX = joystick.inputHorizontal();
        _inputZ = joystick.inputVertical();*/

        //Dir = new Vector3(_inputX, 0, _inputZ);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectCharacter(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectCharacter(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectCharacter(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectCharacter(4);
        if (_playerTransform != null)
        {
            /*if (!(_inputX == 0 && _inputZ == 0))
            {
                _playerTransform.rotation = Quaternion.Lerp(_playerTransform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * rotateSpeed);
                _playerTransform.position += Dir * moveSpeed * Time.deltaTime;
            }*/
        }
        else
        {
            Debug.Log("no char - charScript");
        }
    }

    public Transform SelectCharacter(int i)
    {

        Debug.Log(i + "번 캐릭터");
        movingPoint = playerTransforms["Character_" + i].position;
        lastPos = playerTransforms["Character_" + i].position;
        _playerTransform = playerTransforms["Character_" + i];
        //ani = _playerTransform.GetComponent<Animator>();
        return _playerTransform;
    }
}
