using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharOneMoveScript : MonoBehaviour
{
    public JoyStickTestM joystick;
    float _inputX, _inputZ, moveSpeed, rotateSpeed;
    public float ATK;
    Animator ani;
    Vector3 Dir, movingPoint, lastPos;
    public float inputX
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
    }
    private void Awake()
    {
        joystick = GameObject.Find("JSBackground").GetComponent<JoyStickTestM>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _inputX = joystick.inputHorizontal();
        _inputZ = joystick.inputVertical();
        Dir = new Vector3(_inputX, 0, _inputZ);

        if(PlayerManager.instance.playerTransform.name == transform.name)
        {
            Debug.Log("name °°À½");
            if (!(_inputX == 0 && _inputZ == 0))
            {
                Debug.Log("x != 0 , z != 0");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * rotateSpeed);
                transform.position += Dir * moveSpeed * Time.deltaTime;
            }
        }
    }
}
