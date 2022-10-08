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
        // 공격력이 랜덤값이기때문에 계속 받아와야함
        _weaponOP = charoneScript.attackPoint;
        // 코루틴의 속도를 공격속도에 따라 제어해줘야 피격판정이 깔끔하게 표기될 수 있음
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
