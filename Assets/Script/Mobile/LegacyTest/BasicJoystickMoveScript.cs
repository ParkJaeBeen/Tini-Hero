using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJoystickMoveScript : MonoBehaviour
{
    public JoyStickTestM joystick;
    float inputX, inputZ, moveSpeed, rotateSpeed;
    Animator ani;
    Vector3 Dir, movingPoint;

    private void Awake()
    {
        moveSpeed = 3.0f;
        rotateSpeed = 10.0f;
        movingPoint = transform.position;
        ani = GetComponent<Animator>();
        joystick =  GameObject.Find("JSBackground").GetComponent<JoyStickTestM>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputX = joystick.inputHorizontal();
        inputZ = joystick.inputVertical();

        Dir = new Vector3(inputX, 0, inputZ);

        if (!(inputX == 0 && inputZ == 0))
        {
            Debug.Log("1");
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * rotateSpeed);
            transform.position += Dir * moveSpeed * Time.deltaTime;
        }

        if (inputX == 0 && inputZ == 0)
        {
            ani.SetInteger("aniInt", 0);
        }
        else
        {
            ani.SetInteger("aniInt", 1);
        }
    }
}
