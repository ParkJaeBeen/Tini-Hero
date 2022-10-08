using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MBTestScript : MonoBehaviour
{
    public static MBTestScript instance;
    Animator TestAni;
    public GameObject SWR, SWI;
    float count;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        TestAni = GetComponent<Animator>();
        SWR = Resources.Load<GameObject>("Prefabs/ShockWave");
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestAni.SetInteger("aniInt", 1);
        }

        if(TestAni.GetInteger("aniInt") == 2)
        {
            
        }

    }

    public void test()
    {
        if(count >= 0.7f)
        {
            instanceSW();
            count = 0;
        }
    }

    public void instanceSW()
    {
        SWI = GameObject.Instantiate(SWR, transform.position, transform.rotation);
        SWI.transform.SetParent(transform);
        Destroy(SWI, 1.0f);
    }
}
