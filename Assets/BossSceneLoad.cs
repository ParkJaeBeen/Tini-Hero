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
        Debug.Log("1");
        //progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
        //progressbar.value = 1f;
    }

    IEnumerator LoadScene()
    {
        Debug.Log("loading");
        yield return null;
        
        AsyncOperation op = SceneManager.LoadSceneAsync("BossTest");

        Debug.Log(op);
        op.allowSceneActivation = false;

        Debug.Log(op.isDone);
        while (!op.isDone)
        {
            Debug.Log("���������");
            yield return null;
            Debug.Log("������´ٴ¶�ƴѰ�?");
            Debug.Log(op.progress);
            if (progressbar.value < 1f)
            {
                Debug.Log("���α׷����ٰ� 1 �������ٵ��翬��");
                
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                Debug.Log(progressbar.value);
            }
            else
            {
                Debug.Log("���������������");
                loadtext.text = "Press Spacebar to Play";
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}
