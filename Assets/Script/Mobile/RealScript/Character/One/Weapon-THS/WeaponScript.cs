using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class WeaponScript : MonoBehaviour
{
    CapsuleCollider weaponCollider;
    CharOneScript charoneScript;
    float _weaponOP;
    float _enableTime;
    public float weaponOP
    {
        get { return _weaponOP; }
    }
    public float enableTime
    {
        get { return _enableTime; }
        set { _enableTime = value; }
    }

    private void Awake()
    {
        _enableTime = 0.35f;
    }
    // Start is called before the first frame update
    void Start()
    {
        charoneScript = PlayerManager.instance.charOneScriptPublic;
        weaponCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���ݷ��� �������̱⶧���� ��� �޾ƿ;���
        _weaponOP = charoneScript.attackPoint;
        // �ڷ�ƾ�� �ӵ��� ���ݼӵ��� ���� ��������� �ǰ������� ����ϰ� ǥ��� �� ����
        enableTime = charoneScript.atkCorSpeed;
    }

    public void use()
    {
        StopCoroutine(attack());
        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(enableTime);
        weaponCollider.enabled = false;
    }
}
