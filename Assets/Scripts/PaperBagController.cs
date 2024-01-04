using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBagController : MonoBehaviour
{
    Animator anim;
    PurchaseZoneController purchaseZone;

    public bool isOpenFinish;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        purchaseZone = FindObjectOfType<PurchaseZoneController>();
    }

    private void Start()
    {
        //purchaseZone.purchaseFinish += OnPurchaseFinish;
    }

    //private void OnPurchaseFinish()
    //{
    //    anim.SetTrigger(GameManager.instance.isClose);
    //}

    public void OpenAniamtionFinish()
    {
        isOpenFinish = true;
    }
}
