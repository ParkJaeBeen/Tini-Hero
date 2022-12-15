using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCameraScript : MonoBehaviour
{
    float DelayTime;
    Vector3 point, point1, point2, point3;
    int _posNum;
    public int posNum
    {
        get { return _posNum; }
        set { _posNum = value; }
    }
    private void Awake()
    {
        point = new Vector3(1.35f, 1, -4.5f);
        point1 = new Vector3(-0.45f, 1, -2.5f);
        point2 = new Vector3(2.2f, 1, -2.5f);
        point3 = new Vector3(5.2f, 1, -2.5f);
        DelayTime = 5.0f;
        transform.position = point;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (posNum)
        {
            case 1:
                transform.position = Vector3.Lerp(transform.position, point1, Time.deltaTime * DelayTime);
                break;
            case 2:
                transform.position = Vector3.Lerp(transform.position, point2, Time.deltaTime * DelayTime);
                break;
            case 3:
                transform.position = Vector3.Lerp(transform.position, point3, Time.deltaTime * DelayTime);
                break;
            default:
                transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * DelayTime);
                break;
        }
    }
}
