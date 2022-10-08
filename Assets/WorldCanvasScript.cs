using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldCanvasScript : MonoBehaviour
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
        //CharOneAtkBt.onClick.AddListener(PlayerManager.instance.charOneScriptPublic.Attack);
    }

    public void SelectCharOne()
    {
        PlayerManager.instance.SelectCharacter(1);
    }
    public void SelectCharTwo()
    {
        PlayerManager.instance.SelectCharacter(2);
    }
    public void SelectCharThree()
    {
        PlayerManager.instance.SelectCharacter(3);
    }
    public void GatherOn()
    {
        PlayerManager.instance.GatherOn();
    }
    public void GatherOff()
    {
        PlayerManager.instance.GatherOff();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Loading2");
    }

}
