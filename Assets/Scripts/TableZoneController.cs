using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableZoneController : MonoBehaviour
{
    public Transform moneyPileTF;
    public GameObject moneyPrefab;

    public Stack<GameObject> moneyPileStack = new Stack<GameObject>();

    public Transform table;
    public Transform chair;
    public Quaternion chairInitialRotate;

    public GameObject Trash;

    private void Awake()
    {
        chairInitialRotate = transform.rotation;
    }
}
