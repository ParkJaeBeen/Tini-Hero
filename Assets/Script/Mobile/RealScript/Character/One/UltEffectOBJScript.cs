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
        // ����Ʈ�� ��ġ�� ������ ������ ��� ������Ʈ ����� ���ۺ��� ���� ����Ʈ�� �ϼ��ȴ�.
        charOneScript.ChangeUltEffectPosition(transform.position, transform.rotation);
    }
}
