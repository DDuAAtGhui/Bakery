using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomerPurchase : MonoBehaviour
{
    TableZoneController tableZone;
    CustomerController customerController;
    CustomerBreadStack customerBreadStack;
    WaypointMovement wayMov;

    public bool isPurchaseOnTableFinish = false;

    public static Action PurchaseOnTableFinish;

    GameObject trash;
    private void Awake()
    {
        customerController = GetComponent<CustomerController>();
        customerBreadStack = GetComponent<CustomerBreadStack>();
        wayMov = GetComponent<WaypointMovement>();
    }
    void Start()
    {
        StartCoroutine("PurchaseOnTable");
    }
    IEnumerator PurchaseOnTable()
    {

        while (!isPurchaseOnTableFinish)
        {
            yield return null;

            if (transform.parent != null
                && transform.parent.gameObject.name.Contains("Chair"))
            {
                yield return new WaitForSeconds(1);

                if (tableZone == null)
                    tableZone = GetComponentInParent<TableZoneController>();

                Transform tableBreadTF = tableZone.table.GetChild(0);

                foreach (var bread in customerBreadStack.breadStack)
                {
                    bread.transform.SetParent(tableBreadTF);
                    bread.transform.position = tableBreadTF.position;
                    bread.transform.rotation = transform.rotation;
                }

                yield return new WaitForSeconds(5f);

                InstantiateMoney(tableZone.moneyPrefab, tableZone.moneyPileTF);

                PileUpMoney(3, 3, 0.8f, 0.15f, -0.5f);

                foreach (var bread in customerBreadStack.breadStack)
                    Destroy(bread);

                GetComponent<NavMeshAgent>().enabled = false;

                wayMov.ChangeWaypoints("TableToOutWaypoints");
                wayMov.enabled = true;

                tableZone.chair.rotation =
                    Quaternion.Euler(0, UnityEngine.Random.Range(40f, 180f), 0);

                trash = Instantiate(tableZone.Trash,
                    tableBreadTF.position, Quaternion.identity, tableBreadTF);

                AudioManager.instance.PlaySFX("Trash");
                customerController.smileEmojie.SetActive(true);

                isPurchaseOnTableFinish = true;
            }
        }
    }

    void InstantiateMoney(GameObject moneyPrefab, Transform moneyPileTF)
    {
        int totalMoney = CaculateTotalMoney();

        for (int i = 0; i < totalMoney; i++)
        {
            GameObject go = Instantiate(moneyPrefab, moneyPileTF.position,
                moneyPileTF.transform.rotation, moneyPileTF);

            tableZone.moneyPileStack.Push(go);
        }
    }

    int CaculateTotalMoney()
    {
        int result = 0;
        foreach (var item in customerBreadStack.breadStack)
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

        foreach (GameObject money in tableZone.moneyPileStack)
        {
            money.transform.position = tableZone.moneyPileTF.position +
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

}
