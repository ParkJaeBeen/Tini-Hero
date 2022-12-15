using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    private void Awake()
    {
        progressbar = Canvas.FindObjectOfType<Slider>();
        loadtext = Canvas.FindObjectOfType<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(LoadScene(GameManagerScript.instance.sceneName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene(string _sceneName)
    {
        Debug.Log("loading");
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(_sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            if (progressbar.value < 1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
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
