using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanTable : MonoBehaviour
{
    TableZoneController tableZone;
    Transform breadTF;
    GameObject trash;
    public bool isTableDirty = false;
    bool isCleanFinish;

    [SerializeField] ParticleSystem cleanVFX;

    private void Awake()
    {
        tableZone = GetComponentInParent<TableZoneController>();
        breadTF = transform.GetChild(0);
        cleanVFX = Instantiate(cleanVFX, breadTF.position, cleanVFX.transform.rotation, transform);
        cleanVFX.Stop();
    }
    private void Update()
    {
        if (breadTF.childCount > 0)
        {
            isTableDirty = breadTF.GetChild(0).CompareTag("Trash");

            if (isTableDirty)
                trash = breadTF.GetChild(0).gameObject;
        }

        else
            isTableDirty = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTableDirty)
        {
            StartCoroutine("SlerpChair");
            Destroy(trash);
            AudioManager.instance.PlaySFX("Success");
            isCleanFinish = false;
            cleanVFX.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isTableDirty)
        {
            isCleanFinish = true;
        }
    }

    IEnumerator SlerpChair()
    {
        while (true)
        {
            yield return null;

            //if (!isCleanFinish)
            //    tableZone.chair.rotation
            //        = Quaternion.Slerp(tableZone.chair.rotation, Quaternion.LookRotation
            //        ((tableZone.table.position - transform.position).normalized, Vector3.up),
            //        Time.deltaTime * 4f);

            tableZone.chair.rotation = Quaternion.Euler(0, 180, 0);

        }
    }
}
