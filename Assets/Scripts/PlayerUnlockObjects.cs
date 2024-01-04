using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerUnlockObjects : MonoBehaviour
{
    [SerializeField] GameObject moneyPrefab;
    CameraController cameraController;
    UnlockZoneController unlockZone;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }
    IEnumerator InputMoney()
    {
        while (true)
        {
            yield return null;

            if (GameManager.instance.playerMoney > 0
                && unlockZone != null && unlockZone.remainCost > 0)
            {
                GameObject go =
                    Instantiate(moneyPrefab, transform.position,
                    moneyPrefab.transform.rotation);

                MoneyController go_money = go.GetComponent<MoneyController>();

                go_money.targetTF = unlockZone.moneyTargetTF;
                go_money.parabolicMovement = true;

                unlockZone.remainCost--;
                GameManager.instance.playerMoney--;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UnlockZone"))
        {
            unlockZone = other.GetComponent<UnlockZoneController>();

            StartCoroutine("InputMoney");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UnlockZone"))
        {
            StopCoroutine("InputMoney");
        }
    }


}
