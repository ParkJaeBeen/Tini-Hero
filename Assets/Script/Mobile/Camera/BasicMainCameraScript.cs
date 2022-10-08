using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMainCameraScript : MonoBehaviour
{
    float DelayTime;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public BasicJoystickMoveScript BJMS;

    private void Awake()
    {
        offsetX = transform.position.x;
        offsetY = transform.position.y;
        offsetZ = transform.position.z;
        DelayTime = 5.0f;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        BJMS = FindObjectOfType<BasicJoystickMoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 FixedPos = new Vector3(BJMS.transform.position.x + offsetX,
                                      BJMS.transform.position.y + offsetY,
                                      BJMS.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * DelayTime);
    }
}
