using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UE = UnityEngine;

public enum WaitingType
{
    ToGo = 0, OnTheTable = 1
}
public class CustomerController : MonoBehaviour
{
    BasketController basketController;
    PlayerBreadStack playerStacker;
    CustomerBreadStack customerStacker;
    bool isStackOver;

    GameObject[] basketWaitTFs;

    PurchaseZoneController purchaseZone;

    Transform payWaitTF;
    [SerializeField] Vector3 waitLineInterval = new Vector3(0, 0, 0.5f);

    Transform requestTableTF;

    [SerializeField] int maxBread = 3;
    int demandOfBread;

    public WaitingType waitingType = WaitingType.ToGo;

    public bool isWaitingStackBread = true;
    bool isReachWaypoint = false;
    bool isReachWaitTF_Basket = false;

    public bool ImFirstOnLine;
    public bool isPurchaseFinish = false;

    Transform table;

    Animator anim;
    NavMeshAgent agent;

    public GameObject smileEmojie;

    [Header("UI")]
    [SerializeField] GameObject ballonCanvas;
    [SerializeField] GameObject breadBallon;
    [SerializeField] GameObject payBallon;
    [SerializeField] GameObject tableBallon;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        basketWaitTFs = GameObject.FindGameObjectsWithTag("Wait_Basket");
        payWaitTF = GameObject.FindGameObjectWithTag("Wait_Pay").transform;
        requestTableTF = GameObject.FindGameObjectWithTag("Wait_RequestTable").transform;
        basketController = GameObject.FindGameObjectWithTag("Basket")
            .GetComponent<BasketController>();
        customerStacker = GetComponent<CustomerBreadStack>();
        playerStacker = GameManager.instance.player
            .GetComponent<PlayerBreadStack>();
        purchaseZone = FindObjectOfType<PurchaseZoneController>();
    }
    private void Start()
    {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        StartCoroutine("GoToWayPoint");
        SetNeeds();
        StartCoroutine("GetBread");

        SetBallonCanvas(false);
        SetBreadBallon(false);
        SetPayBallon(false);
        SetTableBallon(false);

        purchaseZone.purchaseFinish += OnPurchaseFinish;
    }


    private void Update()
    {
        SetAnimatorParam();
        OnReachedDestination();
        PayLine();

        if (isPurchaseFinish)
        {
            agent.enabled = false;
        }
    }


    IEnumerator GoToWayPoint()
    {
        while (!isReachWaypoint)
        {
            yield return null;

            transform.position
                = Vector3.MoveTowards(transform.position,
                GameManager.instance.customerWaypoint.position, Time.deltaTime * 5);

            anim.SetBool(GameManager.instance.isMove, !isReachWaypoint);
        }
    }
    void SetNeeds()
    {
        float random = UE.Random.Range(0, 1f);

        if (random <= 0.15f)
            waitingType = WaitingType.OnTheTable;

        else
            waitingType = WaitingType.ToGo;

        demandOfBread = UE.Random.Range(1, maxBread + 1);
    }
    #region Update
    void SetAnimatorParam()
    {
        if (agent.enabled)
            anim.SetBool(GameManager.instance.isMove, agent.velocity.x != 0);

        else
            anim.SetBool(GameManager.instance.isMove, true);
        anim.SetBool(GameManager.instance.isStack, customerStacker.stackTF.childCount != 0);
    }
    void OnReachedDestination()
    {
        if (!agent.enabled)
            return;

        if (isReachWaypoint && agent.remainingDistance <= 0.5f)
        {
            if (transform.parent == null)
                return;


            EqualRotationToParent();

            if (transform.parent.CompareTag("Wait_Basket"))
            {
                SetBallonCanvas(true);
                SetBreadBallon(true);

                isReachWaitTF_Basket = true;
            }

            else if (transform.parent.CompareTag("Wait_Pay"))
            {
                agent.SetDestination(payWaitTF.position
                + waitLineInterval * transform.GetSiblingIndex());

            }

            else if (transform.parent.CompareTag("Wait_RequestTable"))
            {
                agent.SetDestination(requestTableTF.position
                + waitLineInterval * transform.GetSiblingIndex());
            }

            else if (transform.parent.gameObject.name == "Chair")
            {

            }
        }
    }
    private void PayLine()
    {
        if (customerStacker.breadStack.Count == demandOfBread)
        {
            if (!isStackOver)
            {
                isStackOver = true;
                isWaitingStackBread = false;

                if (waitingType == WaitingType.ToGo)
                {

                    SetBreadBallon(false);
                    SetPayBallon(true);

                    TakeLine(payWaitTF);
                }
            }

            if (waitingType == WaitingType.OnTheTable)
            {
                SetBreadBallon(false);
                SetTableBallon(true);

                switch (GameManager.instance.isTableUnlocked)
                {
                    case true:
                        if (transform.GetSiblingIndex() != 0)
                            return;

                        Transform chair =
                            GameObject.FindGameObjectWithTag("TableZone").transform.Find("Chair");

                        Transform table = GameObject.FindGameObjectWithTag("TableZone")
                            .transform.Find("Table");

                        CleanTable cleanTable = table.GetComponent<CleanTable>();

                        if (!agent.enabled)
                            return;

                        if (cleanTable.isTableDirty)
                        {
                            TakeLine(requestTableTF);
                            return;
                        }

                        if (chair.childCount != 0)
                            return;

                        SetBallonCanvas(false);

                        transform.SetParent(chair);

                        if (transform.GetSiblingIndex() == 0)
                            agent.SetDestination(chair.position);

                        break;

                    case false:
                        TakeLine(requestTableTF);
                        break;

                }
            }
        }
    }

    private void TakeLine(Transform targetLine)
    {
        transform.SetParent(targetLine);

        agent.SetDestination(targetLine.position
        + waitLineInterval * transform.GetSiblingIndex());
    }
    #endregion

    void EqualRotationToParent()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,
            transform.parent.rotation, Time.deltaTime * 10f);
    }
    public void SetBallonCanvas(bool active)
    {
        ballonCanvas.SetActive(active);
    }
    public void SetBreadBallon(bool active)
    {
        breadBallon.SetActive(active);

        TextMeshProUGUI countText
            = breadBallon.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetPayBallon(bool active)
    {
        payBallon.SetActive(active);
    }
    public void SetTableBallon(bool active)
    {
        tableBallon.SetActive(active);
    }
    IEnumerator GetBread()
    {
        TextMeshProUGUI countText =
            breadBallon.GetComponentInChildren<TextMeshProUGUI>();

        countText.text = demandOfBread.ToString();

        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (basketController.basket.Count > 0
                && customerStacker.breadStack.Count < demandOfBread
                && basketController.isExhibitOver
                && isReachWaitTF_Basket && !isPurchaseFinish)
            {

                GameObject go = basketController.basket.Pop();

                go.transform.SetParent(customerStacker.stackTF);
                go.transform.position = customerStacker.stackTF.transform.position
                    + customerStacker.stackInterval * customerStacker.breadStack.Count;
                go.transform.rotation = customerStacker.stackTF.rotation;

                customerStacker.breadStack.Push(go);
                AudioManager.instance.PlaySFX("GetObject");

                //0 잠깐 나오는거 방지
                countText.text = Mathf.Clamp((demandOfBread - customerStacker.breadStack.Count)
                    , 1, int.MaxValue).ToString();
            }

        }
    }
    private void OnPurchaseFinish()
    {
        if (ImFirstOnLine)
        {
            transform.SetParent(null);
            ImFirstOnLine = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CustomerWaypoint"
            && !isReachWaypoint)
        {
            isReachWaypoint = true;

            foreach (var go in basketWaitTFs)
            {
                if (go.transform.childCount == 0)
                {
                    transform.SetParent(go.transform);

                    agent.SetDestination(go.transform.position);
                }
            }
        }

        if (other.GetComponent<PurchaseZoneController>() != null
            && transform.parent != null && transform.parent == payWaitTF
            && transform.GetSiblingIndex() == 0)
        {
            ImFirstOnLine = true;
        }

        if (other.gameObject.name == "Chair")
        {
            anim.SetBool(GameManager.instance.isSit, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Chair")
        {
            anim.SetBool(GameManager.instance.isSit, false);
        }
    }
}
