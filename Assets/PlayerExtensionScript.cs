using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtensionScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<CustomizeScript>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
