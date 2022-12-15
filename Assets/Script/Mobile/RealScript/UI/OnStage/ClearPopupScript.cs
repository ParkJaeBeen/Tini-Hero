using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearPopupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {
        GameManagerScript.instance.False();

        Time.timeScale = 1.0f;
        GameManagerScript.instance.isGameClear = false;
        SceneManager.LoadScene("Loading");
    }
}
