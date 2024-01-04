using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class MoneyRetriever : MonoBehaviour
{
    Transform moneyPile;
    Transform startRetrieveTF;

    PurchaseZoneController purchaseZone;
    TableZoneController tableZone;
    public Stack<GameObject> reversedPosZoneStack = new Stack<GameObject>();
    public Stack<GameObject> reversedTableStack = new Stack<GameObject>();
    bool pause;
    IEnumerator RetrieveMoney()
    {
        while (true)
        {
            yield return null;

            if (moneyPile != null && moneyPile.childCount > 0 && !pause)
            {
                GameObject money = null;

                if (purchaseZone != null)
                {
                    if (reversedPosZoneStack.Count == 0)
                        break;

                    money = reversedPosZoneStack.Pop();

                }

                if (tableZone != null)
                {
                    if (reversedTableStack.Count == 0)
                        break;

                    money = reversedTableStack.Pop();
                }

                money.transform.position =
                    startRetrieveTF.position;

                yield return new WaitForSeconds(0.05f);

                DOTweenAnimation doAnim =
                    money.GetComponent<DOTweenAnimation>();

                doAnim.DOPlay();

                AudioManager.instance.PlaySFX("GetObject");
                GameManager.instance.playerMoney++;

                yield return new WaitUntil(() =>
                doAnim.GetComponent<MoneyController>().retrieveAnimFinish);

                Destroy(money);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MoneyPileTF")
        {
            moneyPile = other.transform;
            startRetrieveTF = transform;

            purchaseZone = moneyPile.parent.GetComponentInChildren<PurchaseZoneController>();
            tableZone = other.GetComponentInParent<TableZoneController>();

            if (purchaseZone != null && purchaseZone.moneyPile.Count > 0
                && reversedPosZoneStack.Count == 0)
                reversedPosZoneStack = Utils.ReverseStack(purchaseZone.moneyPile);

            if (tableZone != null && tableZone.moneyPileStack.Count > 0
                && reversedTableStack.Count == 0)
                reversedTableStack = Utils.ReverseStack(tableZone.moneyPileStack);

            pause = false;
            StartCoroutine("RetrieveMoney");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "MoneyPileTF")
        {
            pause = true;
        }
    }
}
