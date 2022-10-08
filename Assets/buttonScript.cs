using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{
    Button bt;
    // Start is called before the first frame update
    void Start()
    {
        bt = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        Debug.Log("test");
    }
}
