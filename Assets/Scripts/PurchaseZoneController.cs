using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PurchaseZoneController : MonoBehaviour
{
    public bool playerTriggered;

    [SerializeField] CustomerController firstCustomerController;
    CustomerBreadStack firstCustomerBreadStack;
    public bool firstCutomerTriggerd;

    [HideInInspector] public Transform paperBagTF;
    [SerializeField] GameObject paperBagPrefab;
    [HideInInspector] public GameObject paperBag;

    public bool isBagSpawned = false;
    [SerializeField] bool isOnCheckOut;

    public Stack<GameObject> moneyPile = new Stack<GameObject>();
    [SerializeField] GameObject moneyPrefab;
    Transform moneyPileTF;
    [SerializeField] Vector3 moneyPileOffset = new Vector3(1, 1, 1);

    public Action purchaseFinish;

    [SerializeField] Transform arrowTF_POS;
    [SerializeField] Transform arrowTF_Moneypile;

    private void Awake()
    {
        paperBagTF = transform.parent.Find("PaperBagTF").transform;
        moneyPileTF = transform.parent.Find("MoneyPileTF").transform;
    }

    private void Start()
    {
        StartCoroutine("Purchase");
    }

    private void Update()
    {
        Progress();

        if (!GameManager.instance.isTutorialArrow_POSFinish
            && firstCutomerTriggerd)
        {
            GameManager.instance.arrow.transform.position
                = arrowTF_POS.position;



            GameManager.instance.isTutorialArrow_POSFinish = true;
        }

        if (!GameManager.instance.isTutorialArrow_MoneypileFinish
            && moneyPile.Count > 0 && GameManager.instance.playerMoney < 1)
        {
            GameManager.instance.arrow.transform.position
                = arrowTF_Moneypile.position;


            GameManager.instance.isTutorialArrow_MoneypileFinish = true;
        }
    }

    private void Progress()
    {
        if (playerTriggered && firstCutomerTriggerd)
        {
            isOnCheckOut = true;

            if (!isBagSpawned && !firstCustomerController.isPurchaseFinish)
            {
                paperBag = Instantiate(paperBagPrefab, paperBagTF.position,
                paperBagPrefab.transform.rotation, paperBagTF);

                isBagSpawned = true;
            }
        }
        else
            isOnCheckOut = false;
    }

    IEnumerator Purchase()
    {
        while (true)
        {
            yield return null;

            if (isOnCheckOut)
            {
                if (firstCustomerController.waitingType == WaitingType.ToGo)
                {
                    yield return new WaitUntil(() =>
                        paperBag.GetComponent<PaperBagController>().isOpenFinish);

                    firstCustomerController.isPurchaseFinish = true;
                    firstCustomerController.smileEmojie.SetActive(true);
                    OutStore();

                    InstantiateMoney();
                    PileUpMoney(3, 3,
                     moneyPileOffset.x, moneyPileOffset.y, moneyPileOffset.z);

                    AudioManager.instance.PlaySFX("Cash");

                    yield return new WaitForSeconds(0.5f);
                    purchaseFinish.Invoke();


                }
            }
        }
    }


    void InstantiateMoney()
    {
        int totalMoney = CaculateTotalMoney();
        Stack<GameObject> tempStack = new Stack<GameObject>();

        for (int i = 0; i < totalMoney; i++)
        {
            GameObject go = Instantiate(moneyPrefab, moneyPileTF.position,
                moneyPileTF.transform.rotation, moneyPileTF);

            moneyPile.Push(go);
        }

    }
    int CaculateTotalMoney()
    {
        int result = 0;
        foreach (var item in firstCustomerBreadStack.breadStack)
        {
            result += item.GetComponent<BreadController>().breadData.value;
        }
        return result;
    }
    void PileUpMoney(int rowLimit, int columnLimit,
        float offsetX = 1, float offsetY = 1, float offsetZ = 1)
    {
        int row = 0; //Çà
        int column = 0; //¿­
        int floor = 0; //Ãþ

        foreach (var money in moneyPile)
        {
            money.transform.position = moneyPileTF.position +
                new Vector3(offsetX * column,
                offsetY * floor, offsetZ * row);

            column++;

            if (column >= columnLimit)
            {
                column = 0;
                row++;
            }

            if (row >= rowLimit)
            {
                row = 0;
                floor++;
            }
        }
    }
    private void OutStore()
    {
        paperBag.transform.position = firstCustomerBreadStack.stackTF.position;
        paperBag.transform.rotation = Quaternion.identity;
        paperBag.transform.SetParent(firstCustomerBreadStack.stackTF);
        isBagSpawned = false;

        firstCustomerController.SetBallonCanvas(false);

        WaypointMovement waypointMove =
            firstCustomerController.GetComponent<WaypointMovement>();

        waypointMove.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTriggered = true;
        }

        if (other.CompareTag("Customer") && other.transform.parent != null
            && other.transform.parent.CompareTag("Wait_Pay"))
        {
            if (other.transform.GetSiblingIndex() != 0)
                return;

            firstCustomerController = other.GetComponent<CustomerController>();
            firstCustomerBreadStack = other.GetComponent<CustomerBreadStack>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Customer") && other.transform.parent != null
            && other.transform.parent.CompareTag("Wait_Pay"))
            firstCutomerTriggerd = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTriggered = false;
        }

        if (other.CompareTag("Customer"))
        {
            firstCutomerTriggerd = false;
        }

    }
}
