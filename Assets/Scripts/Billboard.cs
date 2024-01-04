using UnityEngine;
public class Billboard : MonoBehaviour
{
    Transform cameraTF;
    void Start()
    {
        cameraTF = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position +
    cameraTF.rotation * Vector3.forward,
    cameraTF.rotation * Vector3.up);

    }
}