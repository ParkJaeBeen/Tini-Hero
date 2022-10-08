using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTestScript : MonoBehaviour
{
    TextMeshProUGUI ugui;
    TextMeshPro tmp;
    TextMesh tm;
    Text t;
    // Start is called before the first frame update
    private void Awake()
    {
        ugui = Resources.Load<TextMeshProUGUI>("Prefabs/TestText");
        tmp = Resources.Load<TextMeshPro>("Prefabs/TestText");
        tm = Resources.Load<TextMesh>("Prefabs/TestText");
        t = Resources.Load<Text>("Prefabs/TestText");
    }
    void Start()
    {
        Debug.Log(ugui.text);
        Debug.Log(tmp.text);
        Debug.Log(tm.text);
        Debug.Log(t.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
