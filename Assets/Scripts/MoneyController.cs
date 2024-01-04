using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public bool retrieveAnimFinish = false;
    public bool parabolicMovement = false;
    public Transform targetTF;

    private void Start()
    {
        if (parabolicMovement)
        {
            transform.DOPath(new Vector3[] {transform.position,
                transform.position + new Vector3(-0.5f,0.5f,0f),
            targetTF.position}, 0.05f, PathType.Linear, PathMode.Full3D, 5, Color.red);
        }
    }
    private void Update()
    {
        if (parabolicMovement)
        {
            //transform.position = Vector3.SlerpUnclamped
            //    (transform.position, targetTF.position, 0.1f);

            if (Vector3.Distance(transform.position, targetTF.position) <= 0.1f)
                Destroy(gameObject);
        }
    }
    public void SetRetrieveAnimFinish()
    {
        retrieveAnimFinish = true;
    }
}
