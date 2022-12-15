using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("BossStage") || SceneManager.GetActiveScene().name.Equals("MonsterStage"))
        {
            GameManagerScript.instance.playerManager.GetComponent<PlayerManager>().enabled = true;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharOneScript>().enabled = true;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharTwoScript>().enabled = true;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharThreeScript>().enabled = true;
            GameManagerScript.instance.playerManager.gameCanvas = GameObject.Find("Canvas");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Loading"))
        {
            GameManagerScript.instance.playerManager.GetComponent<PlayerManager>().enabled = false;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharOneScript>().enabled = false;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharTwoScript>().enabled = false;
            GameManagerScript.instance.playerManager.GetComponentInChildren<CharThreeScript>().enabled = false;
            GameManagerScript.instance.False();
        }
    }
}
