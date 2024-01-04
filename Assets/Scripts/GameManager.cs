using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UE = UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public GameObject player;
    [HideInInspector] public int playerMoney = 0;

    public GameObject customer;
    [SerializeField] int existLimit_BusyCustomer = 3;
    [HideInInspector] public Transform customerSpawnTF;
    [HideInInspector] public Transform customerWaypoint;

    public bool isGameOver = false;

    public int totalMoneyOnStage;

    PurchaseZoneController purchaseZone;

    public GameObject arrow;

    #region Flags
    [Header("Flags")]
    [HideInInspector] public bool shouldShowTableRoomFlag = false;
    [HideInInspector] public bool alreadyShowTableRoomFlag = false;
    public bool isTableUnlocked = false;
    public bool isLeftUnlocked = false;

    public bool isTutorialArrow_OvenFinish = false;
    public bool isTutorialArrow_BasketFinish = false;
    public bool isTutorialArrow_POSFinish = false;
    public bool isTutorialArrow_MoneypileFinish = false;
    public bool isTutorialArrow_TableUnlockFinish = false;

    #endregion
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        customerSpawnTF = GameObject.Find("CustomerSpawnTF").transform;
        customerWaypoint = GameObject.Find("CustomerWaypoint").transform;
        purchaseZone = FindObjectOfType<PurchaseZoneController>();
    }

    private void Start()
    {
        AnimateHash();
        StartCoroutine("SpawnCustomers");
    }

    private void Update()
    {
        FlagControl();

        GameObject table = GameObject.FindGameObjectWithTag("TableZone");
        isTableUnlocked = (table != null && table.activeSelf);
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return null;

            if (!isGameOver && ReturnBusyCustomers(out int countOfAll).Count
            < existLimit_BusyCustomer && countOfAll < 7)
            {

                GameObject go = Instantiate(customer, customerSpawnTF.position, customer.transform.rotation);


                //yield return new WaitForSeconds(UE.Random.Range(10f, 18f));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    void FlagControl()
    {
        totalMoneyOnStage = playerMoney + purchaseZone.moneyPile.Count;

        if (totalMoneyOnStage >= 30 && !alreadyShowTableRoomFlag)
        {
            shouldShowTableRoomFlag = true;
            alreadyShowTableRoomFlag = true;
        }
    }

    List<GameObject> ReturnBusyCustomers(out int countOfAll)
    {
        GameObject[] allCustomers = GameObject.FindGameObjectsWithTag("Customer");
        countOfAll = allCustomers.Length;

        List<GameObject> busyCustomers = new List<GameObject>();

        foreach (GameObject go in allCustomers)
        {
            if (go.GetComponent<CustomerController>().isWaitingStackBread)
                busyCustomers.Add(go);
        }

        return busyCustomers;
    }

    [HideInInspector] public int isMove;
    [HideInInspector] public int isStack;
    [HideInInspector] public int isSit;

    [HideInInspector] public int isClose;
    void AnimateHash()
    {
        isMove = Animator.StringToHash("isMove");
        isStack = Animator.StringToHash("isStack");
        isSit = Animator.StringToHash("isSit");

        isClose = Animator.StringToHash("isClose");
    }
}
