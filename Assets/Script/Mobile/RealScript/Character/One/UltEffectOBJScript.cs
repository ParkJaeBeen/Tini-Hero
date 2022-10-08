using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltEffectOBJScript : MonoBehaviour
{
    CharOneScript charOneScript;
    // Start is called before the first frame update
    void Start()
    {
        charOneScript = GetComponentInParent<CharOneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // 이펙트의 위치를 무기의 위에서 계속 업데이트 해줘야 빙글빙글 도는 이펙트가 완성된다.
        charOneScript.ChangeUltEffectPosition(transform.position, transform.rotation);
    }
}
