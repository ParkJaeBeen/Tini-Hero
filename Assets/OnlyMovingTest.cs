using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class OnlyMovingTest : MonoBehaviour
{
    float _inputX, _inputZ, moveSpeed;
    public JoyStickTestM joystick;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 3.0f;
        joystick = GameObject.Find("JSBackground").GetComponent<JoyStickTestM>();
    }

    // Update is called once per frame
    void Update()
    {
        _inputX = joystick.inputHorizontal();
        _inputZ = joystick.inputVertical();
        dir = new Vector3(_inputX, 0, _inputZ);
        Debug.Log(transform);
        if (!(_inputX == 0 && _inputZ == 0))
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }
}
