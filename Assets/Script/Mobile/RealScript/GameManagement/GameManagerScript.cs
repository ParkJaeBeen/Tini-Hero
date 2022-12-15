using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    GameObject instanceClearPopup;
    TextMeshProUGUI timeText;
    public bool _isGameClear, _isGameOver;
    float min, sec;
    string minStr, secStr;
    string _sceneName;
    Image grayStarTwo, grayStarThree;
    float StartTime;
    [SerializeField]
    private PlayerManager _playerManager;

    public PlayerManager playerManager
    {
        get { return _playerManager; }
    }

    public bool isGameClear
    {
        get { return _isGameClear; }
        set { _isGameClear = value; }
    }
    public bool isGameOver
    {
        get { return _isGameOver; }
    }

    public string sceneName
    {
        get { return _sceneName; }
        set { _sceneName = value; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        _isGameClear = false;
        _isGameOver = false;

        StartTime = Time.time;
        DontDestroyOnLoad(gameObject);
        //playerManager.GetComponent<PlayerManager>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameClear)
        {
            Time.timeScale = 0;
        }
        else if (!_isGameClear)
        {
            Time.timeScale = 1.0f;
        }
    }
    public void GameClearPopup(GameObject gameCanvas, GameObject clearPopup, float clearTime)
    {
        instanceClearPopup = GameObject.Instantiate(clearPopup, gameCanvas.transform.position, Quaternion.identity);
        instanceClearPopup.transform.SetParent(gameCanvas.transform);
        timeText = instanceClearPopup.transform.Find("timeText").GetComponent<TextMeshProUGUI>();
        grayStarTwo = instanceClearPopup.transform.Find("grayStarTwo").GetComponent<Image>();
        grayStarThree = instanceClearPopup.transform.Find("grayStarThree").GetComponent<Image>();
        if (clearTime >= 180.0f)
        {
            grayStarTwo.enabled = true;
            grayStarThree.enabled = true;
        }
        else if(clearTime >= 120.0f)
        {
            grayStarThree.enabled = true;
        }
        min = Mathf.Floor(clearTime / 60);
        sec = Mathf.RoundToInt(clearTime % 60);
        minStr = min.ToString();
        if(sec < 10)
        {
            secStr = "0"+ Mathf.RoundToInt(sec).ToString();
        }
        else
        {
            secStr = Mathf.RoundToInt(sec).ToString();
        }
        timeText.text = minStr+" : "+ secStr;
    }


    public void False()
    {
        _isGameClear = false;
    }
}
