using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Button playBt;
    private void Awake()
    {
        playBt = Canvas.FindObjectOfType<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playBtn()
    {
        SceneManager.LoadScene("CustomizingScene");
    }
}