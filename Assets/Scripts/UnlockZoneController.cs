using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockZoneController : MonoBehaviour
{
    [SerializeField] GameObject lockZone;
    [SerializeField] GameObject unlockZone;
    public GameObject nextUnlockZone;
    public Transform moneyTargetTF;
    public int remainCost = 30;
    public TextMeshProUGUI unlockCountText;
    bool unlockFinish = false;

    [SerializeField] Transform arrowTF;

    private void Update()
    {
        unlockCountText.text = remainCost.ToString();

        if (remainCost <= 0 && !unlockFinish)
        {
            unlockFinish = true;
            lockZone.SetActive(false);
            unlockZone.SetActive(true);
            AudioManager.instance.PlaySFX("Success");
        }

        if (arrowTF == null)
            return;

        if (!GameManager.instance.isTutorialArrow_TableUnlockFinish)
        {
            if (GameManager.instance.playerMoney > 1)
            {
                GameManager.instance.arrow.transform.position = arrowTF.transform.position;

                GameManager.instance.isTutorialArrow_TableUnlockFinish = true;
            }
        }

        else if (unlockZone.gameObject.activeSelf)
        {
            GameManager.instance.arrow.SetActive(false);
        }
    }
}
