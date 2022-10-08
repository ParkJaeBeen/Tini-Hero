using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorTest : MonoBehaviour
{
    public static PlayerBehaviorTest instance;
    Vector3 movingPoint, lastPos;
    Vector3 direction;
    float moveSpeed, dis;
    Dictionary<string, Transform> playerTransforms;
    public Transform _playerTransform;
    Animator ani;

    public Transform playerTransform
    {
        get { return _playerTransform; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        movingPoint = transform.position;
        moveSpeed = 2.0f;
        playerTransforms = new Dictionary<string, Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            playerTransforms.Add("Character_" + (i + 1), transform.GetChild(i));
            //Debug.Log(playerTransforms["Character_" + (i+1)]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("캐릭터 존재");
            MoveCharacter();

            dis = Vector3.Distance(movingPoint, _playerTransform.position);

            if (lastPos != _playerTransform.position)
            {
                ani.SetInteger("aniInt", 1);
            }
            if(dis < 0.015f)
            {
                Debug.Log("1");
                ani.SetInteger("aniInt", 0);
            }
        }
        else
        {
            Debug.Log("캐릭터가 없습니다.");
        }
    }

    public void MoveCharacter()
    {
        if (Input.GetMouseButton(1))
        {
            lastPos = _playerTransform.position;
            Debug.Log("마우스 클릭");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                //if (hitInfo.collider.CompareTag("Terrain"))
                movingPoint = hitInfo.point;

                direction = movingPoint - _playerTransform.position;
                _playerTransform.forward = direction;
            }
        }
        _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, movingPoint, Time.deltaTime * moveSpeed);
    }

    public Transform SelectCharacter(int i)
    {

        Debug.Log(i + "번 캐릭터");
        movingPoint = playerTransforms["Character_" + i].position;
        lastPos = playerTransforms["Character_" + i].position;
        _playerTransform = playerTransforms["Character_" + i];
        ani = _playerTransform.GetComponent<Animator>();
        return _playerTransform;
    }
}
