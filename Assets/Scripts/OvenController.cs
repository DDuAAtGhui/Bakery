using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OvenController : MonoBehaviour
{
    [SerializeField] GameObject bread;
    [SerializeField] Transform breadSpawnPoint;
    [SerializeField] float breadSpawnInterval = 1f;
    [SerializeField] int maxBreadCount = 13;
    PlayerBreadStack stacker;

    [SerializeField] Transform arrowTF;
    //����ż� �ٽ��� �ȿ� ��� ������ ť
    //���� ����� �ֵ��� ����� �ϴϱ� ť ���
    public Queue<GameObject> breadsInOvenBasket = new Queue<GameObject>();

    private void Awake()
    {
        stacker = FindObjectOfType<PlayerBreadStack>();
    }
    private void Start()
    {
        StartCoroutine("SpawnBreads");

    }
    private void Update()
    {
        if (!GameManager.instance.isTutorialArrow_OvenFinish)
        {
            GameManager.instance.arrow.transform.position
            = arrowTF.position;

            GameManager.instance.arrow.transform.SetParent(arrowTF);


            GameManager.instance.isTutorialArrow_OvenFinish = true;
        }
    }
    IEnumerator SpawnBreads()
    {

        while (true)
        {
            if (breadsInOvenBasket.Count < maxBreadCount)
            {
                GameObject go =
                Instantiate(bread, breadSpawnPoint.position, bread.transform.rotation);

                breadsInOvenBasket.Enqueue(go);
            }
            yield return new WaitForSeconds(breadSpawnInterval);
        }
    }
    IEnumerator BreadStacking()
    {
        while (true)
        {
            yield return null;

            if (breadsInOvenBasket.Count > 0
                && stacker.breadStack.Count < stacker.maxStackOfBreads)
            {
                GameObject go = breadsInOvenBasket.Dequeue();
                Rigidbody rb = go.GetComponent<Rigidbody>();
                Collider col = go.GetComponent<Collider>();

                col.enabled = false;
                rb.isKinematic = true;

                go.transform.SetParent(GameManager.instance.player.transform);
                go.transform.position = stacker.stackTF.transform.position
                    + stacker.stackInterval * stacker.breadStack.Count;
                go.transform.rotation = stacker.stackTF.transform.rotation;

                stacker.breadStack.Push(go);
                AudioManager.instance.PlaySFX("GetObject");
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ������");
            Debug.Log($"�÷��̾ ������� �ʰ��ִ� ���� ����" +
                $" : {breadsInOvenBasket.Count}");

            StartCoroutine("BreadStacking");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("BreadStacking");
            Debug.Log("�÷��̾ ��� �� ���� : " +
                $"{stacker.breadStack.Count}");
        }
    }
}
