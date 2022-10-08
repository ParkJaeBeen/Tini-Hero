using UnityEngine;

public class MainCameraScriptM : MonoBehaviour
{
    Vector3 oldPos;
    float DelayTime;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    private void Awake()
    {
        offsetX = transform.position.x;
        offsetY = transform.position.y;
        offsetZ = transform.position.z;
        DelayTime = 5.0f;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.playerTransform != null)
        {
            Vector3 FixedPos = new Vector3(PlayerManager.instance.playerTransform.position.x + offsetX,
                                       PlayerManager.instance.playerTransform.position.y + offsetY,
                                       PlayerManager.instance.playerTransform.position.z + offsetZ);
            transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * DelayTime);
        }
        else if (PlayerManager.instance.playerTransform == null)
        {
            Debug.Log("no transform - main camera");
        }

    }
}
