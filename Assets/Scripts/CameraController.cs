using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVcam;
    [SerializeField] CinemachineVirtualCamera tableRoomVcam;

    private void Start()
    {
        StartCoroutine("CameraControl");
    }

    IEnumerator CameraControl()
    {
        while (true)
        {
            yield return null;

            if (GameManager.instance.shouldShowTableRoomFlag)
            {
                playerVcam.gameObject.SetActive(false);
                tableRoomVcam.gameObject.SetActive(true);

                yield return new WaitForSeconds(5f);

                playerVcam.gameObject.SetActive(true);
                tableRoomVcam.gameObject.SetActive(false);

                GameManager.instance.shouldShowTableRoomFlag = false;
            }

            if (!GameManager.instance.isLeftUnlocked 
                && GameManager.instance.isTableUnlocked)
            {
                GameObject LeftUnlockZone = GameObject.Find("LeftUnlockZone");
                tableRoomVcam.LookAt = LeftUnlockZone.transform;
                tableRoomVcam.Follow = LeftUnlockZone.transform;

                LeftUnlockZone.transform.GetChild(0).gameObject.SetActive(true);

                playerVcam.gameObject.SetActive(false);
                tableRoomVcam.gameObject.SetActive(true);

                yield return new WaitForSeconds(5f);

                playerVcam.gameObject.SetActive(true);
                tableRoomVcam.gameObject.SetActive(false);

                GameManager.instance.isLeftUnlocked = true;
            }
        }
    }
}
