using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossSceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    Transform LoadingCanvas;
    // Start is called before the first frame update
    private void Awake()
    {
        LoadingCanvas = GameObject.Find("LoadingCanvas").transform;
        progressbar = LoadingCanvas.Find("Slider").GetComponent<Slider>();
        //progressbar = Canvas.FindObjectOfType<Slider>();
        loadtext = Canvas.FindObjectOfType<Text>();
    }
    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {
        yield return null;
        
        AsyncOperation op = SceneManager.LoadSceneAsync("BossTest");

        Debug.Log(op);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            Debug.Log(op.progress);
            if (progressbar.value < 1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                Debug.Log(progressbar.value);
            }
            else
            {
                loadtext.text = "Press Spacebar to Play";
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}
