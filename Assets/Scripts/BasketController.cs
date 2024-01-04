using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public List<Transform> sockets = new List<Transform>();
    public Stack<GameObject> basket = new Stack<GameObject>();
    PlayerBreadStack stacker;

    public bool isExhibitOver = false;

    [SerializeField] Transform arrowTF;

    private void Awake()
    {
        stacker = FindObjectOfType<PlayerBreadStack>();
    }

    private void Update()
    {
        if (!GameManager.instance.isTutorialArrow_BasketFinish)
        {
            if (stacker.breadStack.Count > 0)
            {
                GameManager.instance.arrow.transform.position = arrowTF.position;

      

                GameManager.instance.isTutorialArrow_BasketFinish = true;
            }
        }
    }
    IEnumerator BreadIntoBasket()
    {
        while (true)
        {
            yield return null;

            if (stacker.breadStack.Count > 0)
            {
                foreach (var socket in sockets)
                {
                    if (socket.childCount == 0)
                    {
                        GameObject go = stacker.breadStack.Pop();

                        go.transform.position = socket.position;
                        go.transform.rotation = socket.rotation;
                        go.transform.SetParent(socket);

                        basket.Push(go);
                        AudioManager.instance.PlaySFX("PutObject");
                        break;
                    }
                }

            }

            else if (stacker.breadStack.Count == 0)
                isExhibitOver = true;

            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isExhibitOver = false;

            StartCoroutine("BreadIntoBasket");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isExhibitOver = true;

            StopCoroutine("BreadIntoBasket");
        }
    }
}
