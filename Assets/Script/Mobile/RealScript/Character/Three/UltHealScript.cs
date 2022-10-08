using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltHealScript : MonoBehaviour
{
    SphereCollider sphereCollider;
    CharThreeScript charThreeScript;
    float calRecoveryPoint, RecoveryPoint, calRAText;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        charThreeScript = FindObjectOfType<CharThreeScript>();
        UltCoroutine();
        RecoveryPoint = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UltCoroutine()
    {
        StopCoroutine(UltHealDisable());
        StartCoroutine(UltHealDisable());
    }

    IEnumerator UltHealDisable()
    {
        for (int i = 0; i< 5; i++)
        {
            sphereCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            sphereCollider.enabled = false;
            yield return new WaitForSeconds(0.9f);
        }
    }

    public int calculateRPUlt(float nowHP, float maxHP)
    {
        int morethanMaxhp = 0;
        int calRA;
        // nowHP = 현재체력이 100을 더한 값
        if (nowHP >= maxHP)
        {
            Debug.Log("현재체력이 최대체력과 같거나 더 높을때");
            morethanMaxhp = (int)(nowHP - maxHP);
            calRA = (int)(RecoveryPoint - morethanMaxhp);
            Debug.Log(calRA);
            return calRA;
        }
        else
        {
            return (int)RecoveryPoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);

            if (other.name.Equals("MC01"))
            {
                PlayerManager.instance.charOneScriptPublic.oneHP += RecoveryPoint;
                calRecoveryPoint = calculateRPUlt(PlayerManager.instance.charOneScriptPublic.oneHP, PlayerManager.instance.charOneScriptPublic.maxHP);
                if (calRecoveryPoint != RecoveryPoint)
                {
                    PlayerManager.instance.charOneScriptPublic.oneHP -= RecoveryPoint - calRecoveryPoint;
                    calRAText = calRecoveryPoint;
                }
                else
                {
                    calRAText = RecoveryPoint;
                }
                charThreeScript.CreateHealText(other.transform, calRAText);

            }
            else if (other.name.Equals("MC02"))
            {
                PlayerManager.instance.charTwoScriptPublic.oneHP += RecoveryPoint;
                calRecoveryPoint = calculateRPUlt(PlayerManager.instance.charTwoScriptPublic.oneHP, PlayerManager.instance.charTwoScriptPublic.maxHP);
                if (calRecoveryPoint != 100)
                {
                    PlayerManager.instance.charTwoScriptPublic.oneHP -= RecoveryPoint - calRecoveryPoint;
                    calRAText = calRecoveryPoint;
                }
                else
                {
                    calRAText = RecoveryPoint;
                }
                charThreeScript.CreateHealText(other.transform, calRAText);
            }
            else if (other.name.Equals("MC03"))
            {
                PlayerManager.instance.charThreeScriptPublic.oneHP += RecoveryPoint;
                calRecoveryPoint = calculateRPUlt(PlayerManager.instance.charThreeScriptPublic.oneHP, PlayerManager.instance.charThreeScriptPublic.maxHP);
                if (calRecoveryPoint != 100)
                {
                    PlayerManager.instance.charThreeScriptPublic.oneHP -= RecoveryPoint - calRecoveryPoint;
                    calRAText = calRecoveryPoint;
                }
                else
                {
                    calRAText = RecoveryPoint;
                }
                charThreeScript.CreateHealText(other.transform, calRAText);
            }
            
        }
    }
}
