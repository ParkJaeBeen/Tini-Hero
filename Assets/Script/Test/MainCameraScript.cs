using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCameraScript : MonoBehaviour
{
    Vector3 oldPos;
    float DelayTime;
    public float offsetX; 
    public float offsetY; 
    public float offsetZ;
    private void Awake()
    {
        offsetX = transform.position.x;
        offsetY = transform.position.y;
        offsetZ = transform.position.z;
    }
    // Start is called before the first frame update
    void Start()
    {
        DelayTime = 5.0f;       
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerBehaviorTest.instance.playerTransform != null)
        {
            //transform.LookAt(PlayerBehaviorTest.instance.playerTransform.position);
            Vector3 FixedPos = new Vector3(PlayerBehaviorTest.instance.playerTransform.position.x + offsetX, 
                                            PlayerBehaviorTest.instance.playerTransform.position.y + offsetY,
                                            PlayerBehaviorTest.instance.playerTransform.position.z + offsetZ);
            transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * DelayTime);
        }
        else if (PlayerBehaviorTest.instance.playerTransform == null)
        {
            Debug.Log("카메라가 확인한 캐릭터가 없습니다.");
        }
        //transform.position = characterPosition.position;
            
    }

    private void LateUpdate()
    {
        /*Vector3 delta = characterPosition.position - oldPos;
        transform.position += delta;
        oldPos = characterPosition.transform.position;*/
    }
}
