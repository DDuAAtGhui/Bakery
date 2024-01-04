using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreadController : MonoBehaviour
{
    public BreadData breadData;

    Rigidbody rb;
    [SerializeField] float waitTime = 0.3f;
    [SerializeField] float power = 1f;

    PurchaseZoneController purchaseZone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        purchaseZone = FindObjectOfType<PurchaseZoneController>();
    }

    private void Start()
    {
        StartCoroutine(Pushed());
        StartCoroutine("OnPurchaseFinishCor");

    }



    IEnumerator Pushed()
    {
        yield return new WaitForSeconds(waitTime);
        rb.useGravity = true;

        rb.velocity = transform.forward * power;
    }

    IEnumerator OnPurchaseFinishCor()
    {
        while (true)
        {
            CustomerController customer = GetComponentInParent<CustomerController>();

            if (customer != null && customer.ImFirstOnLine && purchaseZone.isBagSpawned)
            {
                GameObject paperBag = purchaseZone.paperBag;

                yield return new WaitUntil(() =>
                paperBag.GetComponent<PaperBagController>().isOpenFinish);

                CustomerBreadStack customerBreadStack = customer.GetComponent<CustomerBreadStack>();

                Destroy(customerBreadStack.breadStack.Pop());
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
